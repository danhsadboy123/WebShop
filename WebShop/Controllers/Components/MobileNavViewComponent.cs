using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebShop.Extension;
using WebShop.Models;
using WebShop.ModelViews;

namespace WebShop.Controllers.Components
{
    public class MobileNavViewComponent : ViewComponent
    {
        private readonly DbMarketsContext _context;

        public MobileNavViewComponent(DbMarketsContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var menuItems = await GetMenuItems();
            return View(menuItems);
        }
        private async Task<List<HeaderMenu>> GetMenuItems()
        {

            var lsCats = _context.Categories
                .Include(c => c.ProductCategories)
                .AsNoTracking()
                .Where(x => x.Published == true)
                .OrderByDescending(x => x.Ordering)
                .Take(20)
                .Select(c => new HeaderMenu
                {
                    Category = c
                })
                .ToList();

            foreach (var menuItem in lsCats)
            {
                menuItem.Children = await GetChildMenuItems(menuItem.Category.CatId);
            }

            return lsCats;
        }


        private async Task<List<HeaderMenu>> GetChildMenuItems(int parentId)
        {             
            var lsCats = _context.Categories
               .Include(c => c.CategoryBrands.Where(c => c.Brand.Products.Count() != 0))
               .Include(c => c.CategoryAttributes)
               .Include(c => c.ProductCategories.Take(6))
                   .ThenInclude(c => c.Product)
               .AsNoTracking()
               .Where(x => x.Published == true && x.ParentId == parentId)
               .OrderByDescending(x => x.Ordering)
               .Take(25)
               .Select(c => new HeaderMenu
               {
                   Category = c
               })
               .ToList();
            return lsCats;
        }
    }
}
