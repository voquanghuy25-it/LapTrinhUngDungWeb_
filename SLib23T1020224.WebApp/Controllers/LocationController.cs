using Microsoft.AspNetCore.Mvc;
using SLib23T1020224.BusinessLayer;
using SLib23T1020224.Models.Entities;

namespace SLib23T1020224.WebApp.Controllers
{
    /// <summary>
    /// Điều khiển các chức năng quản lý vị trí lưu trữ: xem danh sách, xem chi tiết, thêm, sửa và xóa.
    /// </summary>
    public class LocationController : Controller
    {
        private readonly LocationService _service;

        /// <summary>
        /// Khởi tạo controller (đối tượng dịch vụ được cung cấp tự động nhờ Dependency Injection).
        /// </summary>
        /// <param name="service">Dịch vụ nghiệp vụ vị trí lưu trữ.</param>
        public LocationController(LocationService service)
        {
            _service = service;
        }

        /// <summary>
        /// Hiển thị danh sách vị trí lưu trữ.
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
                return View(Enumerable.Empty<Location>());
            }
        }

        /// <summary>
        /// Hiển thị thông tin chi tiết của một vị trí lưu trữ.
        /// </summary>
        /// <param name="id">Mã vị trí lưu trữ.</param>
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var item = await _service.GetAsync(id);
                if (item == null)
                {
                    TempData["Error"] = "Vị trí lưu trữ không tồn tại.";
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
            return View("Edit", new Location { IsActive = true });
        }

        /// <summary>
        /// Mở form cập nhật thông tin một vị trí lưu trữ.
        /// </summary>
        /// <param name="id">Mã vị trí lưu trữ cần sửa.</param>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var item = await _service.GetAsync(id);
                if (item == null)
                {
                    TempData["Error"] = "Vị trí lưu trữ không tồn tại.";
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
        public async Task<IActionResult> Save(Location data)
        {
            try
            {
                if (data.LocationId == 0)
                {
                    await _service.AddAsync(data);
                    TempData["Success"] = "Đã thêm vị trí lưu trữ thành công.";
                }
                else
                {
                    await _service.UpdateAsync(data);
                    TempData["Success"] = "Đã cập nhật vị trí lưu trữ thành công.";
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
        /// Xóa một vị trí lưu trữ (chỉ xóa được khi chưa được dữ liệu khác sử dụng).
        /// </summary>
        /// <param name="id">Mã vị trí lưu trữ cần xóa.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                TempData["Success"] = "Đã xóa vị trí lưu trữ thành công.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
