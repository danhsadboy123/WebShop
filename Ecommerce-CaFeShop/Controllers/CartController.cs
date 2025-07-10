using Ecommerce_CaFeShop.Models.ViewModels;
using Ecommerce_CaFeShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ecommerce_CaFeShop.Helper;
using Ecommerce_CaFeShop.Services;

public class CartController : Controller
{
    private readonly CaFeContext _context;
    private readonly ICartService _cartService;

    public CartController(CaFeContext context, ICartService cartService)
    {
        _context = context;
        _cartService = cartService;
    }

    private int? GetCustomerId()
    {
        if (!User.Identity!.IsAuthenticated) return null;

        var customerIdClaim = User.Claims.FirstOrDefault(c => c.Type == "MaKhachHang");
        return customerIdClaim != null ? int.Parse(customerIdClaim.Value) : (int?)null;
    }

    public async Task<IActionResult> Cart(int page = 1, int pageSize = 5)
    {
        try
        {
            var customerId = GetCustomerId();
            var cartItems = await _cartService.GetCartAsync(customerId, HttpContext.Session);

            if (cartItems == null || !cartItems.Any())
            {
                if (customerId.HasValue)
                {
                    ViewBag.Message = "Giỏ hàng của bạn đang trống. Hãy khám phá các sản phẩm tuyệt vời của chúng tôi!";
                }
                else
                {
                    ViewBag.Message = "Giỏ hàng của bạn đang trống. Vui lòng đăng nhập hoặc thêm sản phẩm vào giỏ hàng.";
                }
                return View("EmptyCart");
            }

            var totalItems = cartItems.Count;
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var paginatedItems = cartItems
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalItems = totalItems;

            return View(paginatedItems);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khi tải giỏ hàng: {ex.Message}");
            ViewBag.Message = "Có lỗi xảy ra khi tải giỏ hàng.";
            return View("EmptyCart");
        }
    }

    public async Task<IActionResult> AddToCart(string slug, int quantity)
    {
        try
        {
            Console.WriteLine($"Slug: {slug}, Quantity: {quantity}");

            if (quantity <= 0)
            {
                return Json(new { success = false, message = "Số lượng phải lớn hơn 0." });
            }

            var customerId = GetCustomerId();

            // Kiểm tra sản phẩm tồn tại
            var product = await _context.SanPhams.FirstOrDefaultAsync(p => p.Slug == slug);
            if (product == null)
            {
                return Json(new { success = false, message = $"Không tìm thấy sản phẩm có mã {slug}." });
            }

            // Kiểm tra sản phẩm có đang hoạt động không
            if (product.TrangThai != 1 || product.DaXoa == 1)
            {
                return Json(new { success = false, message = "Sản phẩm hiện không có sẵn." });
            }

            // Kiểm tra số lượng tồn kho
            var currentCart = await _cartService.GetCartAsync(customerId, HttpContext.Session);
            var existingQuantity = currentCart?.FirstOrDefault(c => c.Slug == slug)?.Quantity ?? 0;

            if (existingQuantity + quantity > product.SoLuong)
            {
                return Json(new { success = false, message = $"Không thể thêm số lượng vượt quá tồn kho. Tồn kho hiện tại: {product.SoLuong}" });
            }

            await _cartService.AddToCartAsync(customerId, HttpContext.Session, slug, quantity);

            return Json(new { success = true, message = "Sản phẩm đã được thêm vào giỏ hàng!" });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khi thêm sản phẩm vào giỏ hàng: {ex.Message}");
            return Json(new { success = false, message = "Có lỗi xảy ra khi thêm sản phẩm vào giỏ hàng." });
        }
    }

    public async Task<IActionResult> RemoveCartItem(string slug)
    {
        var customerId = GetCustomerId();
        await _cartService.RemoveFromCartAsync(customerId, HttpContext.Session, slug);
        return RedirectToAction("Cart");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateCart(string slug, int quantity)
    {
        var customerId = GetCustomerId();

        // Kiểm tra tồn kho
        var product = await _context.SanPhams.SingleOrDefaultAsync(p => p.Slug == slug);
        if (product != null && quantity > product.SoLuong)
        {
            var currentCart = await _cartService.GetCartAsync(customerId, HttpContext.Session);
            var originalQuantity = currentCart.FirstOrDefault(c => c.Slug == slug)?.Quantity ?? 0;

            return Json(new
            {
                success = false,
                message = "Không thể cập nhật số lượng vượt quá tồn kho",
                originalQuantity = originalQuantity
            });
        }

        await _cartService.UpdateCartAsync(customerId, HttpContext.Session, slug, quantity);
        return Json(new { success = true });
    }

    public async Task<IActionResult> ClearCart()
    {
        var customerId = GetCustomerId();
        await _cartService.ClearCartAsync(customerId, HttpContext.Session);
        TempData["success"] = "Đã xoá tất cả sản phẩm trong giỏ hàng.";
        return RedirectToAction("Cart");
    }

    [HttpGet("cart-summary")]
    public async Task<IActionResult> GetCartSummary()
    {
        var customerId = GetCustomerId();
        var cart = await _cartService.GetCartAsync(customerId, HttpContext.Session);

        var cartVM = new CartVM
        {
            Quantity = cart.Sum(p => p.Quantity),
            Total = cart.Sum(p => p.Total)
        };

        return PartialView("Components/Cart/Default", cartVM);
    }
}