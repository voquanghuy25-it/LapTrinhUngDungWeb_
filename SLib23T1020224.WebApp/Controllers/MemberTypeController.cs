using Microsoft.AspNetCore.Mvc;
using SLib23T1020224.BusinessLayer;
using SLib23T1020224.Models.Entities;

namespace SLib23T1020224.WebApp.Controllers
{
    /// <summary>
    /// Điều khiển các chức năng quản lý loại độc giả: xem danh sách, xem chi tiết, thêm, sửa và xóa.
    /// </summary>
    public class MemberTypeController : Controller
    {
        private readonly MemberTypeService _service;

        /// <summary>
        /// Khởi tạo controller (đối tượng dịch vụ được cung cấp tự động nhờ Dependency Injection).
        /// </summary>
        /// <param name="service">Dịch vụ nghiệp vụ loại độc giả.</param>
        public MemberTypeController(MemberTypeService service)
        {
            _service = service;
        }

        /// <summary>
        /// Hiển thị danh sách loại độc giả.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var items = await _service.ListAsync();
                return View(items);
            }
            catch (Exception ex)
            {
                ViewBag.LoadError = ex.Message;
                return View(Enumerable.Empty<MemberType>());
            }
        }

        /// <summary>
        /// Hiển thị thông tin chi tiết của một loại độc giả.
        /// </summary>
        /// <param name="id">Mã loại độc giả.</param>
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var item = await _service.GetAsync(id);
                if (item == null)
                {
                    TempData["Error"] = "Loại độc giả không tồn tại.";
                    return RedirectToAction(nameof(Index));
                }

                // Cho biết bản ghi có đang được dữ liệu khác sử dụng hay không
                ViewBag.InUse = await _service.InUseAsync(id);
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
        public IActionResult Create()
        {
            return View("Edit", new MemberType { MaxActiveLoans = 3, LoanPeriodDays = 14, MaxRenewals = 0, IsActive = true });
        }

        /// <summary>
        /// Mở form cập nhật thông tin một loại độc giả.
        /// </summary>
        /// <param name="id">Mã loại độc giả cần sửa.</param>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var item = await _service.GetAsync(id);
                if (item == null)
                {
                    TempData["Error"] = "Loại độc giả không tồn tại.";
                    return RedirectToAction(nameof(Index));
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
        /// Lưu dữ liệu từ form: mã bằng 0 là thêm mới, ngược lại là cập nhật.
        /// </summary>
        /// <param name="data">Dữ liệu nhập từ form.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(MemberType data)
        {
            try
            {
                if (data.MemberTypeId == 0)
                {
                    await _service.AddAsync(data);
                    TempData["Success"] = "Đã thêm loại độc giả thành công.";
                }
                else
                {
                    await _service.UpdateAsync(data);
                    TempData["Success"] = "Đã cập nhật loại độc giả thành công.";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Bỏ các thông báo kiểm tra mặc định (tiếng Anh), chỉ hiện thông báo của tầng nghiệp vụ
                ModelState.Clear();
                ModelState.AddModelError(string.Empty, ex.Message);
                return View("Edit", data);
            }
        }

        /// <summary>
        /// Xóa một loại độc giả (chỉ xóa được khi chưa được dữ liệu khác sử dụng).
        /// </summary>
        /// <param name="id">Mã loại độc giả cần xóa.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                TempData["Success"] = "Đã xóa loại độc giả thành công.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
