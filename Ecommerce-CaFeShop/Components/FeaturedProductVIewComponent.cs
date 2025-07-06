using Ecommerce_CaFeShop.Models;
using Ecommerce_CaFeShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_CaFeShop.Components
{
    public class FeaturedProductVIewComponent : ViewComponent
    {
        private readonly CaFeContext _context;

        public FeaturedProductVIewComponent(CaFeContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Lấy sản phẩm mới nhất (30 ngày gần đây), loại bỏ sản phẩm cũ
            DateTime cutoffDate = DateTime.Now.AddDays(-30);

            var featureProduct = await _context.SanPhams
                .Where(p => p.DaXoa == 0 && p.TrangThai == 1 && p.NgayTao >= cutoffDate) // Chỉ sản phẩm mới và hoạt động
                .Include(p => p.DanhGiaSanPhams)
                .OrderByDescending(p => p.NgayTao) // Sắp xếp theo ngày tạo mới nhất
                .Take(8) // Chỉ lấy 8 sản phẩm mới nhất
                .Select(p => new ProductVM()
                {
                    Slug = p.Slug,
                    ProductName = p.TenSanPham,
                    Price = p.Gia,
                    Image = p.HinhAnh,
                    ProductRating = p.DanhGiaSanPhams.Any()
                        ? p.DanhGiaSanPhams.Average(r => (double)r.DiemDanhGia!)
                        : 0,
                })
                .ToListAsync();

            ViewBag.FeaturedProduct = featureProduct;
            return View();
        }
    }
}