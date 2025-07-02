using Ecommerce_WatchShop.Models;
using Ecommerce_WatchShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_WatchShop.Components

{
    public class BestSellerProductViewComponent :ViewComponent
    {
        private readonly DongHoContext _context;

        public BestSellerProductViewComponent(DongHoContext context)
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
