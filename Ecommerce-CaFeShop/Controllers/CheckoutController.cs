using Ecommerce_CaFeShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ecommerce_CaFeShop.Models.ViewModels;
using Ecommerce_CaFeShop.Helper;

namespace Ecommerce_CaFeShop.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly CaFeContext _context;
        public List<CartRequest> Carts => CartHelper.GetCart(HttpContext.Session);

        public CheckoutController(CaFeContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                TempData["error"] = "Vui lòng đăng nhập để thanh toán";
                return RedirectToAction("Index", "Home");
            }

            if (Carts == null || Carts.Count == 0)
            {
                TempData["error"] = "Giỏ hàng của bạn đang trống";
                return RedirectToAction("Cart", "Cart");
            }

            var checkoutVM = new CheckoutVM();
            var subtotal = Carts.Sum(item => (decimal)(item.Quantity * item.Price));
            var shippingFee = 30000;
            checkoutVM.TotalAmount = subtotal + shippingFee;

            // Lấy thông tin khách hàng
            var customerIdClaim = HttpContext.User.Claims.SingleOrDefault(c => c.Type == "MaKhachHang");
            if (customerIdClaim != null && int.TryParse(customerIdClaim.Value, out var customerId))
            {
                var customer = await _context.KhachHangs.FindAsync(customerId);
                if (customer != null)
                {
                    checkoutVM.FullName = customer.HoTen ?? "";
                    checkoutVM.Phone = customer.SoDienThoai ?? "";
                    checkoutVM.Email = customer.Email ?? "";
                    checkoutVM.Address = customer.DiaChi ?? "";
                    checkoutVM.Province = customer.Tinh ?? "";
                    checkoutVM.District = customer.Huyen ?? "";
                    checkoutVM.Ward = customer.Xa ?? "";
                }
            }

            ViewBag.CartItems = Carts;
            return View(checkoutVM);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutVM model)
        {
            try
            {
                if (!User.Identity!.IsAuthenticated)
                {
                    TempData["error"] = "Vui lòng đăng nhập để thanh toán";
                    return RedirectToAction("Index", "Home");
                }

                if (Carts == null || Carts.Count == 0)
                {
                    TempData["error"] = "Giỏ hàng của bạn đang trống";
                    return RedirectToAction("Cart", "Cart");
                }

                // Validate model state
                if (!ModelState.IsValid)
                {
                    TempData["error"] = "Vui lòng kiểm tra lại thông tin đã nhập";
                    ViewBag.CartItems = Carts;
                    return View("Index", model);
                }

                // Kiểm tra thông tin bắt buộc
                if (string.IsNullOrWhiteSpace(model.FullName?.Trim()) ||
                    string.IsNullOrWhiteSpace(model.Phone?.Trim()) ||
                    string.IsNullOrWhiteSpace(model.Email?.Trim()) ||
                    string.IsNullOrWhiteSpace(model.Address?.Trim()))
                {
                    TempData["error"] = "Vui lòng điền đầy đủ thông tin bắt buộc";
                    ViewBag.CartItems = Carts;
                    return View("Index", model);
                }

                var customerIdClaim = HttpContext.User.Claims.SingleOrDefault(c => c.Type == "MaKhachHang");
                if (customerIdClaim == null || !int.TryParse(customerIdClaim.Value, out var customerId))
                {
                    TempData["error"] = "Phiên đăng nhập không hợp lệ";
                    return RedirectToAction("Index", "Home");
                }

                // Kiểm tra khách hàng có tồn tại
                var customer = await _context.KhachHangs.FindAsync(customerId);
                if (customer == null)
                {
                    TempData["error"] = "Không tìm thấy thông tin khách hàng";
                    return RedirectToAction("Index", "Home");
                }

                // Kiểm tra sản phẩm trong giỏ hàng có tồn tại
                foreach (var cartItem in Carts)
                {
                    var product = await _context.SanPhams.FindAsync(cartItem.ProductId);
                    if (product == null)
                    {
                        TempData["error"] = $"Sản phẩm {cartItem.ProductName} không còn tồn tại";
                        ViewBag.CartItems = Carts;
                        return View("Index", model);
                    }
                }

                // Tính tổng tiền
                var subtotal = Carts.Sum(item => (decimal)(item.Quantity * item.Price));
                var shippingFee = 30000m;
                var totalAmount = subtotal + shippingFee;

                // Sử dụng transaction để đảm bảo tính toàn vẹn dữ liệu
                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    // Tạo hóa đơn
                    var hoaDon = new HoaDon
                    {
                        MaKhachHang = customerId,
                        NgayDatHang = DateTime.Now,
                        HoTen = model.FullName.Trim(),
                        SoDienThoai = model.Phone.Trim(),
                        Email = model.Email.Trim(),
                        DiaChi = model.Address.Trim(),
                        Tinh = model.Province?.Trim() ?? "",
                        Huyen = model.District?.Trim() ?? "",
                        Xa = model.Ward?.Trim() ?? "",
                        PhuongThucThanhToan = "COD",
                        TongTien = totalAmount,
                        TrangThai = 1 // Chờ xác nhận
                    };

                    _context.HoaDons.Add(hoaDon);
                    await _context.SaveChangesAsync();

                    // Tạo chi tiết hóa đơn
                    foreach (var item in Carts)
                    {
                        var chiTiet = new ChiTietHoaDon
                        {
                            MaHoaDon = hoaDon.MaHoaDon,
                            MaSanPham = item.ProductId,
                            SoLuong = item.Quantity,
                            Gia = (decimal)item.Price,
                            TongTien = (decimal)(item.Quantity * item.Price)
                        };
                        _context.ChiTietHoaDons.Add(chiTiet);
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    // Xóa giỏ hàng
                    CartHelper.ClearCart(HttpContext.Session);

                    TempData["success"] = "Đặt hàng thành công! Mã đơn hàng: #DH" + hoaDon.MaHoaDon.ToString("D6");
                    return RedirectToAction("OrderSuccess", new { orderId = hoaDon.MaHoaDon });
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }


            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException dbEx)
            {
                // Log chi tiết lỗi database
                var innerException = dbEx.InnerException?.Message ?? dbEx.Message;
                TempData["error"] = $"Lỗi cơ sở dữ liệu: {innerException}";
                ViewBag.CartItems = Carts;
                return View("Index", model);
            }
            catch (Exception ex)
            {
                // Log chi tiết lỗi
                TempData["error"] = $"Có lỗi xảy ra: {ex.Message}";
                ViewBag.CartItems = Carts;
                return View("Index", model);
            }
        }

        public async Task<IActionResult> OrderSuccess(int orderId)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            var customerIdClaim = HttpContext.User.Claims.SingleOrDefault(c => c.Type == "MaKhachHang");
            if (customerIdClaim == null || !int.TryParse(customerIdClaim.Value, out var customerId))
            {
                return RedirectToAction("Index", "Home");
            }

            var order = await _context.HoaDons
                .Include(h => h.ChiTietHoaDons)
                    .ThenInclude(ct => ct.SanPham)
                .FirstOrDefaultAsync(h => h.MaHoaDon == orderId && h.MaKhachHang == customerId);

            if (order == null)
            {
                TempData["error"] = "Không tìm thấy đơn hàng";
                return RedirectToAction("Index", "Home");
            }

            return View(order);
        }
    }
}