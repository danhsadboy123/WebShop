using Ecommerce_CaFeShop.Models.ViewModels;
using Ecommerce_CaFeShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace DongHo_Admin.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Admin")]
    public class StatisticalController : Controller
    {
        private readonly CaFeContext _context;

        public StatisticalController(CaFeContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("GetRevenue")]
        public IActionResult GetRevenue()
        {
            try
            {
                // ✅ Tính doanh thu theo Tổng tiền của Hóa đơn, không dùng chi tiết hóa đơn
                var chartData = _context.HoaDons
                    .GroupBy(hd => hd.NgayDatHang.Date)
                    .Select(group => new RevenueStatisticVM
                    {
                        Date = group.Key,
                        Revenue = group.Sum(hd => hd.TongTien)
                    })
                    .OrderBy(s => s.Date)
                    .ToList();

                var result = chartData.Select(item => new
                {
                    date = item.Date.ToString("yyyy-MM-dd"),
                    revenue = item.Revenue
                }).ToList();

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        [HttpPost]
        [Route("SubmitFilterDate")]
        public IActionResult SubmitFilterDate(string filterdate)
        {
            if (!DateTime.TryParse(filterdate, out var parsedDate))
            {
                return BadRequest("Ngày không hợp lệ.");
            }

            try
            {
                // ✅ Tính doanh thu theo ngày lọc và lấy từ bảng Hóa đơn
                var chartData = _context.HoaDons
                    .Where(hd => hd.NgayDatHang.Date == parsedDate.Date)
                    .GroupBy(hd => hd.NgayDatHang.Date)
                    .Select(group => new RevenueStatisticVM
                    {
                        Date = group.Key,
                        Revenue = group.Sum(hd => hd.TongTien)
                    })
                    .ToList();

                var result = chartData.Select(item => new
                {
                    date = item.Date.ToString("yyyy-MM-dd"),
                    revenue = item.Revenue
                }).ToList();

                return Json(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Đã xảy ra lỗi trong quá trình xử lý.");
            }
        }
    }
}
