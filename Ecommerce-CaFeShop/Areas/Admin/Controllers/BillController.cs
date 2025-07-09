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

            // Thống kê trạng thái đơn hàng
            ViewBag.PendingCount = await _context.HoaDons.CountAsync(h => h.TrangThai == 0);
            ViewBag.ConfirmedCount = await _context.HoaDons.CountAsync(h => h.TrangThai == 7);
            ViewBag.ProcessingCount = await _context.HoaDons.CountAsync(h => h.TrangThai == 6);
            ViewBag.CancelledCount = await _context.HoaDons.CountAsync(h => h.TrangThai == 5);

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
            if (status < 0 || status > 10)
            {
                TempData["error"] = "Trạng thái không hợp lệ";
                return RedirectToAction("Details", new { id = id });
            }

            bill.TrangThai = status;
            await _context.SaveChangesAsync();

            string statusText = status switch
            {
                0 => "Chờ xác nhận",
                1 => "Chưa thanh toán",
                2 => "Đã thanh toán",
                3 => "Đang giao hàng",
                4 => "Đã giao hàng",
                5 => "Đã hủy",
                6 => "Đang xử lý",
                7 => "Đã xác nhận",
                8 => "Hoàn thành",
                9 => "Trả hàng",
                10 => "Hoàn tiền",
                _ => "Không xác định"
            };

            TempData["success"] = $"Cập nhật trạng thái đơn hàng thành công: {statusText}";
            return RedirectToAction("Details", new { id = id });
        }
    }
}