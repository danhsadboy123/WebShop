using System;
using System.Collections.Generic;
using System.Linq;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShop.Extension;
using WebShop.Models;
using WebShop.ModelViews;

namespace WebShop.Controllers
{
    public class ShoppingCartController : BaseController
    {
        private readonly DbMarketsContext _context;
        public INotyfService _notyfService { get; }

        public ShoppingCartController(DbMarketsContext context, INotyfService notyfService) : base(context)
        {
            _context = context;
            _notyfService = notyfService;
        }

        public List<CartItem> GioHang
        {
            get
            {
                var gh = HttpContext.Session.Get<List<CartItem>>("GioHang") ?? new List<CartItem>();
                return gh;
            }
        }

        public class CartItemUpdateModel
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
        }

        [HttpPost]
        [Route("api/cart/add")]
        public IActionResult AddToCart(int productID, int? amount)
        {
            try
            {
                var discountPro = _context.DiscountAddProducts
                    .Include(p => p.Discount)
                    .FirstOrDefault(p => p.ProductId == productID &&
                                         p.Discount.ShowWeb == true &&
                                         p.Discount.Startus == true &&
                                         p.Discount.TimeOff > DateTime.Now &&
                                         p.Discount.TimeOn < DateTime.Now);

                List<CartItem> cart = GioHang;
                CartItem item = cart.SingleOrDefault(p => p.product.ProductId == productID);

                if (item != null) // Cập nhật số lượng nếu sản phẩm đã có trong giỏ hàng
                {
                    item.amount += amount.GetValueOrDefault(1);
                }
                else // Thêm sản phẩm mới vào giỏ hàng
                {
                    Product product = _context.Products.SingleOrDefault(p => p.ProductId == productID);
                    item = new CartItem
                    {
                        amount = amount.GetValueOrDefault(1),
                        product = product
                    };
                    cart.Add(item);
                }

                HttpContext.Session.Set("GioHang", cart); // Lưu lại session sau khi đã xử lý

                _notyfService.Success("Thêm sản phẩm thành công");

                // Xử lý "Mua ngay" nếu có
                if (Request.Form["isBuyNow"] == "true")
                {
                    return Json(new { success = true, result = "Redirect", url = "/cart" });
                }

                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        [Route("api/cart/update")]
        public IActionResult UpdateCart([FromBody] List<CartItemUpdateModel> cartItems)
        {
            try
            {
                List<CartItem> cart = GioHang;

                foreach (var item in cartItems)
                {
                    CartItem cartItem = cart.SingleOrDefault(p => p.product.ProductId == item.ProductId);
                    if (cartItem != null)
                    {
                        cartItem.amount = item.Quantity; // Cập nhật số lượng
                    }
                } 
                HttpContext.Session.Set("GioHang", cart); // Lưu lại session

                return Ok(new { success = true, message = "Giỏ hàng đã được cập nhật." });
            }
            catch
            {
                return Json(new { success = false, message = "Có lỗi xảy ra khi cập nhật giỏ hàng." });
            }
        }

        [HttpPost]
        [Route("api/cart/remove")]
        public ActionResult Remove(int productID)
        {
            try
            {
                List<CartItem> gioHang = GioHang;
                CartItem item = gioHang.SingleOrDefault(p => p.product.ProductId == productID);
                if (item != null)
                {
                    gioHang.Remove(item);
                }
                // Lưu lại session
                HttpContext.Session.Set("GioHang", gioHang);
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        [Route("api/cart/clear-cart")]
        public IActionResult ClearCart()
        {
            // Xóa tất cả sản phẩm trong giỏ hàng
            HttpContext.Session.Remove("GioHang");

            // Trả về kết quả thành công
            return Json(new { success = true });
        }

        [HttpGet]
        public IActionResult GetSessionData()
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            return Json(cart);
        }
    }
}
