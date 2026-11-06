using Microsoft.AspNetCore.Mvc;
using SLib23T1020224.BusinessLayer;
using SLib23T1020224.Models.Entities;

namespace SLib23T1020224.WebApp.Controllers
{
    public class AuthorController : Controller
    {
        private readonly AuthorService _service;

        public AuthorController(AuthorService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                return View(await _service.ListAsync());
            }
            catch (Exception ex)
            {
                ViewBag.LoadError = ex.Message;
                return View(Enumerable.Empty<Author>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var item = await _service.GetAsync(id);
                if (item == null)
                {
                    TempData["Error"] = "Tác giả không tồn tại.";
                    return RedirectToAction(nameof(Index));
                }
                ViewBag.InUse = await _service.InUseAsync(id);
                return View(item);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Edit", new Author { IsActive = true });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var item = await _service.GetAsync(id);
                if (item == null)
                {
                    TempData["Error"] = "Tác giả không tồn tại.";
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(Author data)
        {
            try
            {
                if (data.AuthorId == 0)
                {
                    await _service.AddAsync(data);
                    TempData["Success"] = "Đã thêm tác giả thành công.";
                }
                else
                {
                    await _service.UpdateAsync(data);
                    TempData["Success"] = "Đã cập nhật tác giả thành công.";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                ModelState.AddModelError(string.Empty, ex.Message);
                return View("Edit", data);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                TempData["Success"] = "Đã xóa tác giả thành công.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}