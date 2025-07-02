using CodeMegaVNPay.Models;
using CodeMegaVNPay.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using WebShop.Extension;
using WebShop.Models;
using WebShop.ModelViews;

namespace WebShop.Controllers
{
    public class PaysController : Controller
    {
        private readonly DbMarketsContext _context;
        private readonly IVnPayService _vnPayService;
        public PaysController(DbMarketsContext context, IVnPayService vnPayService)
        {
            _context = context;
            _vnPayService = vnPayService;
        }
        public List<CartItem> GioHang
        {
            get
            {
                var gh = HttpContext.Session.Get<List<CartItem>>("GioHang");
                if (gh == default(List<CartItem>))
                {
                    gh = new List<CartItem>();
                }
                return gh;
            }
        }
        // Hàm tạo mã đơn hàng
        private string GenerateOrderCode(int guestId)
        {
            string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss"); // Lấy thời gian hiện tại
            string randomString = Guid.NewGuid().ToString().Substring(0, 6); // Lấy 6 ký tự ngẫu nhiên từ GUID
            return $"ORD-{timeStamp}-{randomString}-{guestId}"; // Kết hợp thành mã đơn hàng
        }

        //pay 
        [Route("Pays")]
        [HttpPost]
        public IActionResult CreatePaymentUrl([FromBody] PaymentInformationModel model)
        {
            // Lấy giỏ hàng từ session
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index");
            }
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var taikhoanID = HttpContext.Session.GetString("CustomerId");
                    decimal totalMoney = cart.Sum(item =>
                        (decimal)(item.product.SalePrice > 0 ? item.product.SalePrice : item.product.Price) * item.amount);
                    model.Amount = (double)totalMoney;
                    Guest guest = null;

                    // Nếu khách hàng chưa đăng nhập, tạo mới một khách hàng vãng lai
                    if (taikhoanID == null)
                    {
                        guest = new Guest
                        {
                            FullName = model.FullName,
                            SoDienThoai = model.SoDienThoai.Trim().ToLower(),
                            Email = model.Email.Trim().ToLower(),
                            CreatedAt = DateTime.Now
                        };
                        _context.Guests.Add(guest);
                        _context.SaveChanges();
                    }

                    // Tạo đơn hàng
                    var order = new Order
                    {
                        GuestId = guest?.GuestId, // Có thể là null nếu khách hàng đã đăng nhập
                        SoDienThoai = guest?.SoDienThoai,
                        OrderDate = DateTime.Now,
                        DeliveryStatusId = 1,
                        PaymentStatusId = 1,
                        CodstatusId = 1,
                        Deleted = false,
                        TotalMoney = Convert.ToInt32(totalMoney),
                        Draft = false,
                        Code = GenerateOrderCode(guest?.GuestId ?? 0),
                        Confirmed = false
                    };

                    _context.Orders.Add(order);
                    _context.SaveChanges();

                    // Tạo chi tiết đơn hàng
                    foreach (var item in cart)
                    {
                        var orderDetail = new OrderDetail
                        {
                            OrderId = order.OrderId,
                            ProductId = item.product.ProductId,
                            Amount = item.amount,
                            Discount = 0,
                            TotalMoney = item.amount * (item.product.SalePrice > 0 ? item.product.SalePrice : item.product.Price),
                            CreatedAt = DateTime.Now,
                            Price = (item.product.SalePrice > 0 ? item.product.SalePrice : item.product.Price)
                        };
                        _context.OrderDetails.Add(orderDetail);
                    }

                    // Tạo địa chỉ giao hàng
                    var shippingAddress = new ShippingAddress
                    {
                        OrderId = order.OrderId,
                        Name = guest?.FullName,
                        SoDienThoai = guest?.SoDienThoai,
                        ProvinceId = model.ProvinceId,
                        WardId = model.WardId,
                        DistrictId = model.DistrictId,
                        Address = model.Address
                    };
                    _context.ShippingAddresses.Add(shippingAddress);
                    _context.SaveChanges();

                    // Cam kết giao dịch
                    transaction.Commit(); 
                    model.OrderType = "Thanh toán VN Pay cho đơn hàng " + order.Code;
                    model.OrderDescription = "Khách hàng "+ model.FullName+ " thanh toán VN Pay cho đơn hàng " + order.Code;
                    model.Name = "Khách hàng " + model.FullName + " thanh toán VN Pay cho đơn hàng " + order.Code;
                    HttpContext.Session.SetInt32("CurrentOrderId", order.OrderId); 
                    var url = _vnPayService.CreatePaymentUrl(model, HttpContext);
                    return Ok(url);
                }
                catch (Exception ex)
                {
                    // Ghi lại lỗi nếu có
                    transaction.Rollback();
                    // Trả về thông báo lỗi hoặc xử lý lỗi tại đây
                    return Json(new { success = false, message = "Có lỗi xảy ra khi lưu đơn hàng." });
                }

            }
        }

        [HttpGet]
        public IActionResult PaymentCallback()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            return Ok(response);
        }


    }
}
