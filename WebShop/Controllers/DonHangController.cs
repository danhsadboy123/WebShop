using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using WebShop.Models;
using WebShop.ModelViews;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebShop.Controllers
{
    public class DonHangController : BaseController
    {
        private readonly DbMarketsContext _context;
        public INotyfService _notyfService { get; }
        public DonHangController(DbMarketsContext context, INotyfService notyfService):base(context) 
        {
            _context = context;
            _notyfService = notyfService;
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var taikhoanID = HttpContext.Session.GetString("CustomerId");
                if (string.IsNullOrEmpty(taikhoanID)) return RedirectToAction("Login", "TaiKhoans");
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CustomerId == Convert.ToInt32(taikhoanID));

                if (khachhang == null) return NotFound();

                var donhang = await _context.Orders
                    .Include(x => x.DeliveryStatus)
                    .Include(x => x.Customer)
                    .FirstOrDefaultAsync(m => m.OrderId == id && Convert.ToInt32(taikhoanID) == m.CustomerId);

                if (donhang == null) return NotFound();

                var shippingAddress = _context.ShippingAddresses
                    .Include(x => x.Province)
                    .Include(x => x.District)
                    .Include(x => x.Ward)
                    .FirstOrDefault(o => o.OrderId == donhang.OrderId);

                var chitietdonhang = _context.OrderDetails
                    .Include(x => x.Product)
                    .AsNoTracking()
                    .Where(x => x.OrderId == id)
                    .OrderBy(x => x.OrderDetailId)
                    .ToList();
                XemDonHang donHang = new XemDonHang();
                donHang.DonHang = donhang;
                donHang.DiaChi = shippingAddress;
                donHang.ChiTietDonHang = chitietdonhang;
                return PartialView("Details", donHang);

            }
            catch
            {
                return NotFound();
            }
        }

        [Route("/tra-cuu-don-hang")]
        public async Task<IActionResult> LookUp(string code)
        { 
            return View();
        }
    }
}
