using Ecommerce_WatchShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ecommerce_WatchShop.Models.ViewModels;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.AspNetCore.Authorization;
using Ecommerce_WatchShop.Helper;
namespace Ecommerce_WatchShop.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly DongHoContext _context;
        public List<CartRequest> Carts => CartHelper.GetCart(HttpContext.Session);

        public CheckoutController(DongHoContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                TempData["ShowLoginModal"] = true;
                return RedirectToAction("Index", "Home");
            }
            if(Carts is null || Carts.Count == 0)
            {
                TempData["error"] = "Giỏ hàng của bạn đang trống";
                return RedirectToAction("Cart", "Cart");
            }    
            var checkoutValidationVM = new CheckoutValidationVM
            {
                CheckoutVM = new CheckoutVM(),
                CartRequest = Carts
            };
            return View(checkoutValidationVM);
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutValidationVM checkoutValidationVM)
        {
            if (ModelState.IsValid)
            {
                var customerIdClaim = HttpContext.User.Claims.SingleOrDefault(c => c.Type == "CustomerId");
                if (customerIdClaim != null && int.TryParse(customerIdClaim.Value, out var customerId))
                {
                    var bill = new HoaDon
                    {
                        MaKhachHang = customerId,
                        HoTen = checkoutValidationVM.CheckoutVM.FullName,
                        SoDienThoai = checkoutValidationVM.CheckoutVM.Phone,
                        Email = checkoutValidationVM.CheckoutVM.Email,
                        DiaChi = checkoutValidationVM.CheckoutVM.Address,
                        Tinh = checkoutValidationVM.CheckoutVM.Province,
                        Huyen = checkoutValidationVM.CheckoutVM.District,
                        Xa = checkoutValidationVM.CheckoutVM.Ward,
                        PhuongThucThanhToan = checkoutValidationVM.CheckoutVM.PaymentMethod,
                        TongTien = checkoutValidationVM.CheckoutVM.TotalAmount,
                        TrangThai = 1,
                        NgayDatHang = DateTime.Now
                    };
        
                    await _context.Database.BeginTransactionAsync();
                    
                    try
                    {
                        await _context.AddAsync(bill);
                        await _context.SaveChangesAsync();
        
                        var invoices = new List<ChiTietHoaDon>();
                        foreach(var item in checkoutValidationVM.CartRequest)
                        {
                            var productExists = await _context.SanPhams.AnyAsync(p => p.MaSanPham == item.ProductId);
                            if (!productExists)
                            {
                                continue; 
                            }
                            invoices.Add(new ChiTietHoaDon
                            {
                                MaHoaDon   = bill.MaHoaDon,
                                MaSanPham = item.ProductId,
                                SoLuong = item.Quantity,
                                Gia = (decimal)item.Price,
                                TongTien = (decimal)(item.Quantity * item.Price)
                            });
                        }
                        if (invoices.Any())
                        {
                            await _context.AddRangeAsync(invoices);
                            await _context.SaveChangesAsync();
                        }
        
                        await _context.Database.CommitTransactionAsync();
        
                        CartHelper.ClearCart(HttpContext.Session);
                        TempData["success"] = "Đã mua hàng thành công";
                        return RedirectToAction("Index", "Home");
                    }
                    catch
                    {
                        await _context.Database.RollbackTransactionAsync();
                    }
                }    
            }    
            return View("Index", checkoutValidationVM);
        }
    }
}
