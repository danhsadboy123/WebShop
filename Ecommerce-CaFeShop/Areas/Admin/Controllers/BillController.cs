using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ecommerce_CaFeShop.Models;

namespace Ecommerce_CaFeShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Admin")]
    public class BillController : Controller
    {
        private readonly CaFeContext _context;

        public BillController(CaFeContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var bills = await _context.HoaDons
                .Include(h => h.KhachHang)
                .OrderByDescending(h => h.NgayDatHang)
                .ToListAsync();
            return View(bills);
        }

        public async Task<IActionResult> Details(int id)
        {
            var bill = await _context.HoaDons
                .Include(h => h.KhachHang)
                .Include(h => h.ChiTietHoaDons)
                    .ThenInclude(ct => ct.SanPham)
                .FirstOrDefaultAsync(h => h.MaHoaDon == id);

            if (bill == null)
            {
                return NotFound();
            }

            return View(bill);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, int status)
        {
            var bill = await _context.HoaDons.FindAsync(id);
            if (bill == null)
            {
                return NotFound();
            }

            // Kiểm tra trạng thái hợp lệ
            if (status < 1 || status > 4)
            {
                TempData["error"] = "Trạng thái không hợp lệ";
                return RedirectToAction("Details", new { id = id });
            }

            bill.TrangThai = status;
            await _context.SaveChangesAsync();

            string statusText = status switch
            {
                1 => "Chờ xác nhận",
                2 => "Đã xác nhận",
                3 => "Đang xử lý",
                4 => "Đã hủy",
                _ => "Không xác định"
            };

            TempData["success"] = $"Cập nhật trạng thái đơn hàng thành công: {statusText}";
            return RedirectToAction("Details", new { id = id });
        }
    }
}