using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SLib23T1020224.BusinessLayer;
using SLib23T1020224.Models.Entities;

namespace SLib23T1020224.WebApp.Controllers
{
    /// <summary>
    /// Điều khiển các chức năng quản lý thể loại: xem danh sách, xem chi tiết, thêm, sửa và xóa.
    /// </summary>
    public class CategoryController : Controller
    {
        private readonly CategoryService _service;

        /// <summary>
        /// Khởi tạo controller (đối tượng dịch vụ được cung cấp tự động nhờ Dependency Injection).
        /// </summary>
        /// <param name="service">Dịch vụ nghiệp vụ thể loại.</param>
        public CategoryController(CategoryService service)
        {
            _service = service;
        }

        /// <summary>
        /// Hiển thị danh sách thể loại.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var items = await _service.ListAsync();
                // Tên thể loại cha để hiển thị trong bảng
                ViewBag.CategoryNames = items.ToDictionary(c => c.CategoryId, c => c.CategoryName);
                return View(items);
            }
            catch (Exception ex)
            {
                ViewBag.LoadError = ex.Message;
                return View(Enumerable.Empty<Category>());
            }
        }

        /// <summary>
        /// Hiển thị thông tin chi tiết của một thể loại.
        /// </summary>
        /// <param name="id">Mã thể loại.</param>
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var item = await _service.GetAsync(id);
                if (item == null)
                {
                    TempData["Error"] = "Thể loại không tồn tại.";
                    return RedirectToAction(nameof(Index));
                }

                // Cho biết bản ghi có đang được dữ liệu khác sử dụng hay không
                ViewBag.InUse = await _service.InUseAsync(id);
                if (item.ParentCategoryId.HasValue)
                {
                    var parent = await _service.GetAsync(item.ParentCategoryId.Value);
                    ViewBag.ParentName = parent?.CategoryName;
                }
                return View(item);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Mở form thêm mới (dùng chung View "Edit").
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                await LoadParentsAsync(0);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }

            return View("Edit", new Category { IsActive = true });
        }

        /// <summary>
        /// Mở form cập nhật thông tin một thể loại.
        /// </summary>
        /// <param name="id">Mã thể loại cần sửa.</param>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var item = await _service.GetAsync(id);
                if (item == null)
                {
                    TempData["Error"] = "Thể loại không tồn tại.";
                    return RedirectToAction(nameof(Index));
                }
                await LoadParentsAsync(item.CategoryId);
                return View(item);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Lưu dữ liệu từ form: mã bằng 0 là thêm mới, ngược lại là cập nhật.
        /// </summary>
        /// <param name="data">Dữ liệu nhập từ form.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(Category data)
        {
            try
            {
                if (data.CategoryId == 0)
                {
                    await _service.AddAsync(data);
                    TempData["Success"] = "Đã thêm thể loại thành công.";
                }
                else
                {
                    await _service.UpdateAsync(data);
                    TempData["Success"] = "Đã cập nhật thể loại thành công.";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Bỏ các thông báo kiểm tra mặc định (tiếng Anh), chỉ hiện thông báo của tầng nghiệp vụ
                ModelState.Clear();
                ModelState.AddModelError(string.Empty, ex.Message);
                try { await LoadParentsAsync(data.CategoryId); } catch { /* bỏ qua, danh sách thể loại cha sẽ trống */ }
                return View("Edit", data);
            }
        }

        /// <summary>
        /// Xóa một thể loại (chỉ xóa được khi chưa được dữ liệu khác sử dụng).
        /// </summary>
        /// <param name="id">Mã thể loại cần xóa.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                TempData["Success"] = "Đã xóa thể loại thành công.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Nạp danh sách thể loại cha cho ô chọn (loại trừ chính thể loại đang sửa và các thể loại con/cháu của nó).
        /// </summary>
        /// <param name="excludeId">Mã thể loại đang sửa (0 nếu đang thêm mới).</param>
        private async Task LoadParentsAsync(int excludeId)
        {
            var all = (await _service.ListAsync()).ToList();

            var excluded = new HashSet<int>();
            if (excludeId > 0)
            {
                excluded.Add(excludeId);
                bool added;
                do
                {
                    added = false;
                    foreach (var c in all)
                    {
                        if (c.ParentCategoryId.HasValue && excluded.Contains(c.ParentCategoryId.Value) && excluded.Add(c.CategoryId))
                            added = true;
                    }
                } while (added);
            }

            ViewBag.ParentCategories = all
                .Where(c => !excluded.Contains(c.CategoryId))
                .OrderBy(c => c.CategoryName)
                .Select(c => new SelectListItem(c.CategoryName, c.CategoryId.ToString()))
                .ToList();
        }
    }
}
