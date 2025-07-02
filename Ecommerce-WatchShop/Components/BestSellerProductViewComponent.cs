using Ecommerce_CaFeShop.Models;
using Ecommerce_CaFeShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_CaFeShop.Components

{
    public class BestSellerProductViewComponent :ViewComponent
    {
        private readonly CaFeContext _context;

        public BestSellerProductViewComponent(CaFeContext context)
        {
            _context = context; 
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var bestSellerProduct = await _context.SanPhams
                .Where(p => p.LuotXem >= 1000)
                .Include(p => p.DanhGiaSanPhams)
                .Select(p => new ProductVM()
                {
                    Slug = p.Slug,
                    ProductName = p.TenSanPham,
                    Price = p.Gia,
                    Image = p.HinhAnh,
                    ProductRating = p.DanhGiaSanPhams.Any()
                        ? p.DanhGiaSanPhams.Average(r => (double)r.DiemDanhGia!) : 0,
                }).ToListAsync();
            ViewBag.BestSellerProduct = bestSellerProduct;
            return View();
        }
    }
}
