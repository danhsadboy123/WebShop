 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShop.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebShop.Areas.Admin.Controllerst
{
    [Area("Admin")]
    [Authorize]
    public class SearchController : Controller
    {
        private readonly DbMarketsContext _context;
        public INotyfService _notyfService { get; }
        public SearchController(DbMarketsContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        [HttpPost]
        public IActionResult SearchProductQuotation(string keyword)
        {
            List<Product> ls = new List<Product>();
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                return PartialView("QuotationProductsSearchPartial", null);
            }
            ls = _context.Products.AsNoTracking()
                                  .Where(x => x.ProductName.Contains(keyword))
                                  .OrderByDescending(x => x.ProductName)
                                  .ToList();
            if (ls == null)
            {
                return PartialView("QuotationProductsSearchPartial", null);
            }
            else
            {
                return PartialView("QuotationProductsSearchPartial", ls);
            }
        }
        [HttpPost]
        public IActionResult SearchProductOrder(string keyword)
        {
            List<Product> ls = new List<Product>();
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                return PartialView("OrderProductsSearchPartial", null);
            }
            ls = _context.Products.AsNoTracking()
                                  .Where(x => x.ProductName.Contains(keyword))
                                  .OrderByDescending(x => x.ProductName)
                                  .ToList();
            if (ls == null)
            {
                return PartialView("OrderProductsSearchPartial", null);
            }
            else
            {
                return PartialView("OrderProductsSearchPartial", ls);
            }
        }
        public IActionResult FindCategory(string keyword)
        {
            List<Category> ls = new List<Category>();
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                ls = _context.Categories.AsNoTracking()
                                  .OrderByDescending(x => x.CatId)
                                  .Take(20)
                                  .ToList();
                return PartialView("ListCategoriesSearchPartial", ls);
            }
            ls = _context.Categories.AsNoTracking()
                                  .Where(x => x.CatName.Contains(keyword))
                                  .OrderByDescending(x => x.CatName)
                                  .Take(20)
                                  .ToList();
            if (ls == null)
            {
                return PartialView("ListCategoriesSearchPartial", null);
            }
            else
            {
                return PartialView("ListCategoriesSearchPartial", ls);
            }
        }
        public IActionResult FindBrand(string keyword)
        {
            List<Brand> ls = new List<Brand>();
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                ls = _context.Brands.AsNoTracking()
                                  .OrderByDescending(x => x.BrandId)
                                  .Take(20)
                                  .ToList();
                return PartialView("ListBrandsSearchPartial", ls);
            }
            ls = _context.Brands.AsNoTracking()
                                  .Where(x => x.BrandName.Contains(keyword))
                                  .OrderByDescending(x => x.BrandName)
                                  .Take(20)
                                  .ToList();
            if (ls == null)
            {
                return PartialView("ListBrandsSearchPartial", null);
            }
            else
            {
                return PartialView("ListBrandsSearchPartial", ls);
            }
        }
    }
}
