using Ecommerce_CaFeShop.Models;
using Ecommerce_CaFeShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_CaFeShop.Components
{
    public class MenuCategoryViewComponent : ViewComponent
    {
        private readonly CaFeContext _context;
        public MenuCategoryViewComponent(CaFeContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var category = await _context.DanhMucs.Select(c => new MenuCategoryVM
            {
                CategoryId = c.MaDanhMuc,
                CategoryName = c.TenDanhMuc,
                ParentId = c.MaDanhMucCha,
                Slug = c.Slug,
            }).ToListAsync();
            return View(category);
        }
    }
}
