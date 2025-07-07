using Ecommerce_CaFeShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_CaFeShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Admin")]
    public class BannerController : Controller
    {
        private readonly CaFeContext _context;

        public BannerController(CaFeContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách banner
        public async Task<IActionResult> Index()
        {
            var banners = await _context.Sliders
                .OrderByDescending(s => s.NgayTao ?? DateTime.Now)
                .ToListAsync();
            return View(banners);
        }

        // Hiển thị form thêm banner
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Xử lý thêm banner
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slider banner, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Dữ liệu không hợp lệ";
                return View(banner);
            }

            try
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    if (imageFile.Length > 5 * 1024 * 1024)
                    {
                        TempData["error"] = "Kích thước file ảnh không được vượt quá 5MB.";
                        return View(banner);
                    }

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    banner.HinhAnh = "/Images/" + fileName;
                }
                else
                {
                    banner.HinhAnh = "/Images/default-image.jpg";
                }

                banner.NgayTao = DateTime.Now;
                banner.NgayCapNhat = DateTime.Now;
                banner.TrangThai = true;

                _context.Add(banner);
                await _context.SaveChangesAsync();

                TempData["success"] = "Thêm banner thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Lỗi khi thêm banner: {ex.Message}";
                return View(banner);
            }
        }

        // Hiển thị form cập nhật banner
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var banner = await _context.Sliders.FindAsync(id);
            if (banner == null)
            {
                TempData["error"] = "Không tìm thấy banner";
                return RedirectToAction("Index");
            }

            return View(banner);
        }

        // Xử lý cập nhật banner
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Slider banner, IFormFile? imageFile)
        {
            if (id != banner.MaSlider)
            {
                TempData["error"] = "ID banner không hợp lệ";
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                return View(banner);
            }

            try
            {
                var existingBanner = await _context.Sliders.FindAsync(id);
                if (existingBanner == null)
                {
                    TempData["error"] = "Không tìm thấy banner";
                    return RedirectToAction("Index");
                }

                existingBanner.TieuDe = banner.TieuDe;
                existingBanner.MoTa = banner.MoTa;
                existingBanner.Link = banner.Link;
                existingBanner.ThuTuHienThi = banner.ThuTuHienThi;

                if (imageFile != null && imageFile.Length > 0)
                {
                    if (imageFile.Length > 5 * 1024 * 1024)
                    {
                        TempData["error"] = "Kích thước file ảnh không được vượt quá 5MB.";
                        return View(banner);
                    }

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    existingBanner.HinhAnh = "/Images/" + fileName;
                }

                existingBanner.NgayCapNhat = DateTime.Now;

                _context.Update(existingBanner);
                await _context.SaveChangesAsync();

                TempData["success"] = "Cập nhật banner thành công!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Đã xảy ra lỗi: {ex.Message}";
                return View(banner);
            }
        }

        // Ẩn/hiện banner
        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var banner = await _context.Sliders.FindAsync(id);

            if (banner == null)
            {
                TempData["error"] = "Không tìm thấy banner";
                return RedirectToAction("Index");
            }

            banner.TrangThai = !banner.TrangThai;
            banner.NgayCapNhat = DateTime.Now;
            _context.Update(banner);

            await _context.SaveChangesAsync();

            string status = banner.TrangThai ? "hiển thị" : "ẩn";
            TempData["success"] = $"Banner đã được {status}!";
            return RedirectToAction("Index");
        }

        // Xóa banner
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var banner = await _context.Sliders.FindAsync(id);

            if (banner == null)
            {
                TempData["error"] = "Không tìm thấy banner";
                return RedirectToAction("Index");
            }

            try
            {
                _context.Sliders.Remove(banner);
                await _context.SaveChangesAsync();

                TempData["success"] = "Banner đã được xóa!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Lỗi khi xóa banner: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}