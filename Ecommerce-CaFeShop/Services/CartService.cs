using Ecommerce_CaFeShop.Models;
using Ecommerce_CaFeShop.Models.ViewModels;
using Ecommerce_CaFeShop.Helper;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_CaFeShop.Services
{
    public class CartService : ICartService
    {
        private readonly CaFeContext _context;

        public CartService(CaFeContext context)
        {
            _context = context;
        }

        public async Task<List<CartRequest>> GetCartAsync(int? customerId, ISession session)
        {
            if (customerId.HasValue)
            {
                // Lấy giỏ hàng từ database cho khách hàng đã đăng nhập
                var cartItems = await _context.GioHangs
                    .Include(g => g.SanPham)
                    .Where(g => g.MaKhachHang == customerId.Value)
                    .Select(g => new CartRequest
                    {
                        ProductId = g.SanPham!.MaSanPham,
                        Slug = g.SanPham.Slug!,
                        ProductName = g.SanPham.TenSanPham!,
                        Image = g.SanPham.HinhAnh,
                        Price = (double)g.Gia,
                        OriginalPrice = g.SanPham.GiaKhuyenMai.HasValue && g.SanPham.GiaKhuyenMai.Value > 0 && g.SanPham.GiaKhuyenMai.Value < g.SanPham.Gia ? (double?)g.SanPham.Gia : null,
                        Quantity = g.SoLuong
                    })
                    .ToListAsync();

                return cartItems;
            }
            else
            {
                // Lấy giỏ hàng từ session cho khách chưa đăng nhập
                return CartHelper.GetCart(session);
            }
        }

        public async Task AddToCartAsync(int? customerId, ISession session, string slug, int quantity)
        {
            var product = await _context.SanPhams.FirstOrDefaultAsync(p => p.Slug == slug);
            if (product == null) return;

            var hasDiscount = product.GiaKhuyenMai.HasValue && product.GiaKhuyenMai.Value > 0 && product.GiaKhuyenMai.Value < product.Gia;
            var price = hasDiscount ? (double)product.GiaKhuyenMai.Value : (double)product.Gia;

            if (customerId.HasValue)
            {
                // Thêm vào database cho khách hàng đã đăng nhập
                var existingItem = await _context.GioHangs
                    .FirstOrDefaultAsync(g => g.MaKhachHang == customerId.Value && g.MaSanPham == product.MaSanPham);

                if (existingItem != null)
                {
                    existingItem.SoLuong += quantity;
                    existingItem.Gia = (decimal)price; // Cập nhật giá mới nhất
                }
                else
                {
                    var newCartItem = new GioHang
                    {
                        MaKhachHang = customerId.Value,
                        MaSanPham = product.MaSanPham,
                        SoLuong = quantity,
                        Gia = (decimal)price
                    };
                    _context.GioHangs.Add(newCartItem);
                }
                await _context.SaveChangesAsync();
            }
            else
            {
                // Thêm vào session cho khách chưa đăng nhập
                var cart = CartHelper.GetCart(session);
                var item = cart.FirstOrDefault(p => p.Slug == slug);

                if (item != null)
                {
                    item.Quantity += quantity;
                }
                else
                {
                    item = new CartRequest
                    {
                        ProductId = product.MaSanPham,
                        Slug = product.Slug!,
                        ProductName = product.TenSanPham!,
                        Image = product.HinhAnh,
                        Price = price,
                        OriginalPrice = hasDiscount ? product.Gia : null,
                        Quantity = quantity
                    };
                    cart.Add(item);
                }
                CartHelper.SaveCart(session, cart);
            }
        }

        public async Task UpdateCartAsync(int? customerId, ISession session, string slug, int quantity)
        {
            if (customerId.HasValue)
            {
                var product = await _context.SanPhams.FirstOrDefaultAsync(p => p.Slug == slug);
                if (product == null) return;

                var cartItem = await _context.GioHangs
                    .FirstOrDefaultAsync(g => g.MaKhachHang == customerId.Value && g.MaSanPham == product.MaSanPham);

                if (cartItem != null)
                {
                    cartItem.SoLuong = quantity;
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                var cart = CartHelper.GetCart(session);
                var item = cart.FirstOrDefault(p => p.Slug == slug);
                if (item != null)
                {
                    item.Quantity = quantity;
                    CartHelper.SaveCart(session, cart);
                }
            }
        }

        public async Task RemoveFromCartAsync(int? customerId, ISession session, string slug)
        {
            if (customerId.HasValue)
            {
                var product = await _context.SanPhams.FirstOrDefaultAsync(p => p.Slug == slug);
                if (product == null) return;

                var cartItem = await _context.GioHangs
                    .FirstOrDefaultAsync(g => g.MaKhachHang == customerId.Value && g.MaSanPham == product.MaSanPham);

                if (cartItem != null)
                {
                    _context.GioHangs.Remove(cartItem);
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                var cart = CartHelper.GetCart(session);
                var item = cart.FirstOrDefault(p => p.Slug == slug);
                if (item != null)
                {
                    cart.Remove(item);
                    CartHelper.SaveCart(session, cart);
                }
            }
        }

        public async Task ClearCartAsync(int? customerId, ISession session)
        {
            if (customerId.HasValue)
            {
                var cartItems = await _context.GioHangs
                    .Where(g => g.MaKhachHang == customerId.Value)
                    .ToListAsync();

                _context.GioHangs.RemoveRange(cartItems);
                await _context.SaveChangesAsync();
            }
            else
            {
                CartHelper.ClearCart(session);
            }
        }

        public async Task MergeSessionCartToDatabase(int customerId, ISession session)
        {
            var sessionCart = CartHelper.GetCart(session);
            if (!sessionCart.Any()) return;

            foreach (var sessionItem in sessionCart)
            {
                var existingItem = await _context.GioHangs
                    .FirstOrDefaultAsync(g => g.MaKhachHang == customerId && g.MaSanPham == sessionItem.ProductId);

                if (existingItem != null)
                {
                    existingItem.SoLuong += sessionItem.Quantity;
                    existingItem.Gia = (decimal)sessionItem.Price; // Cập nhật giá mới nhất
                }
                else
                {
                    var newCartItem = new GioHang
                    {
                        MaKhachHang = customerId,
                        MaSanPham = sessionItem.ProductId,
                        SoLuong = sessionItem.Quantity,
                        Gia = (decimal)sessionItem.Price
                    };
                    _context.GioHangs.Add(newCartItem);
                }
            }

            await _context.SaveChangesAsync();
            CartHelper.ClearCart(session); // Xóa session cart sau khi merge
        }
    }
}