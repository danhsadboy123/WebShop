using Ecommerce_CaFeShop.Models;
using Ecommerce_CaFeShop.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_CaFeShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Admin")]
    public class DashboardController : Controller
    {
        private readonly CaFeContext _context;
        private readonly IWebHostEnvironment _webhostEnvironment;
        public DashboardController(CaFeContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webhostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var footerVM = new FooterVM
            {
                Footer = await _context.Footers.FirstOrDefaultAsync(),
            };

            // Thống kê cơ bản
            ViewBag.customerCount = await _context.KhachHangs.CountAsync();
            ViewBag.productCount = await _context.SanPhams.Where(p => p.TrangThai == 1).CountAsync();
            ViewBag.orderCount = await _context.HoaDons.CountAsync();

            // Thống kê doanh thu tháng hiện tại
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            var monthlyRevenue = await _context.HoaDons
                .Where(h => h.NgayDatHang.Month == currentMonth &&
                           h.NgayDatHang.Year == currentYear &&
                           h.TrangThai != 4) // Loại trừ đơn hàng đã hủy
                .SumAsync(h => h.TongTien);
            ViewBag.monthlyRevenue = monthlyRevenue;

            // Thống kê đơn hàng theo trạng thái
            ViewBag.pendingOrders = await _context.HoaDons.Where(h => h.TrangThai == 1).CountAsync();
            ViewBag.confirmedOrders = await _context.HoaDons.Where(h => h.TrangThai == 2).CountAsync();
            ViewBag.processingOrders = await _context.HoaDons.Where(h => h.TrangThai == 3).CountAsync();
            ViewBag.cancelledOrders = await _context.HoaDons.Where(h => h.TrangThai == 4).CountAsync();

            // Thống kê doanh thu 7 ngày gần nhất
            var last7Days = Enumerable.Range(0, 7)
                .Select(i => DateTime.Now.Date.AddDays(-i))
                .Reverse()
                .ToList();

            var dailyRevenue = new List<decimal>();
            foreach (var day in last7Days)
            {
                var dayRevenue = await _context.HoaDons
                    .Where(h => h.NgayDatHang.Date == day && h.TrangThai != 4)
                    .SumAsync(h => h.TongTien);
                dailyRevenue.Add(dayRevenue);
            }
            ViewBag.dailyRevenue = dailyRevenue;
            ViewBag.last7Days = last7Days.Select(d => d.ToString("dd/MM")).ToList();

            return View(footerVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(FooterVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "File không hợp lệ. Chỉ cho phép ảnh có đuôi là jpg, png, jpeg, gif và bmp";
                return RedirectToAction("Index");
            }

            try
            {
                if (model.LogoFile != null)
                {


                    string uploadsFolder = Path.Combine(_webhostEnvironment.WebRootPath, "Images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.LogoFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Đảm bảo thư mục tồn tại
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        model.LogoFile.CopyTo(fileStream);
                    }
                    model.Footer!.Logo = uniqueFileName;
                }

                if (model.Footer!.Ma == 0)
                    _context.Footers.Add(model.Footer);
                else
                {
                    var existingFooter = _context.Footers.Find(model.Footer.Ma);
                    if (existingFooter != null)
                    {
                        existingFooter.MoTa = model.Footer.MoTa;
                        existingFooter.DiaChi = model.Footer.DiaChi;
                        existingFooter.Email = model.Footer.Email;
                        existingFooter.FacebookUrl = model.Footer.FacebookUrl;
                        if (!string.IsNullOrEmpty(model.Footer.Logo))
                            existingFooter.Logo = model.Footer.Logo;
                    }
                }

                _context.SaveChanges();
                TempData["success"] = "Cập nhật thông tin web thành công!";
            }
            catch (Exception ex)
            {
                TempData["error"] = "Có lỗi xảy ra khi cập nhật thông tin: " + ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}