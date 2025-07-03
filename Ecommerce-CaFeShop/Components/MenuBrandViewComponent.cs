using Ecommerce_CaFeShop.Models;
using Ecommerce_CaFeShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_CaFeShop.Components
{
    public class MenuBrandViewComponent : ViewComponent
    {
        private readonly CaFeContext _context;
        public MenuBrandViewComponent(CaFeContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var menuBrand = await _context.ThuongHieus.Select(b => new MenuBrandVM
            {
                BrandId = b.MaThuongHieu,
                Name = b.TenThuongHieu,
                Slug = b.Slug
            }).ToListAsync();
            return View(menuBrand);
        }
    }
}
