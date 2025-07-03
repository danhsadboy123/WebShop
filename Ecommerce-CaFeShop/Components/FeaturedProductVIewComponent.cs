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
            DateTime cutoffDate = DateTime.Now.AddDays(-30);
            var featureProduct = await _context.SanPhams
                .Where(p => p.NgayTao >= cutoffDate)
                .Select(p => new ProductVM()
                {
                    Slug = p.Slug,
                    ProductName = p.TenSanPham,
                    Price = p.Gia,
                    Image = p.HinhAnh,
                    ProductRating = p.DanhGiaSanPhams.Any()
                        ? p.DanhGiaSanPhams.Average(r => (double)r.DiemDanhGia!)
                        : 0,
                }).ToListAsync();
            ViewBag.FeaturedProduct = featureProduct;
            return View();
        }
    }
}
