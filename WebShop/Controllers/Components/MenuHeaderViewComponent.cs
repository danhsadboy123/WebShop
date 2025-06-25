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
    public class MenuHeaderViewComponent : ViewComponent
    {
        private readonly DbMarketsContext _context;

        public MenuHeaderViewComponent( DbMarketsContext context)
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
                .AsNoTracking()
                .Where(x => x.Published == true)
                .OrderBy(x => x.Ordering)
                .Take(20)
                .Select(c=> new HeaderMenu
                {
                    Category =c
                })
                .ToList();
             
            foreach (var cat in lsCats)
            {
                cat.attribute = _context.Attributes
                    .Include(c=>c.CategoryAttributes.Where(c=>c.CatId==cat.Category.CatId))                     
                    .Include(c => c.AttributesPrices.Where(c => c.ProductId == null))
                    .ToList();
            }           
            return lsCats;
        }
    }
}
