using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WebShop.Models;
using WebShop.ModelViews;

using WebShop.Extension;
namespace WebShop.Controllers
{
    public class BaseController : Controller
    {
        protected readonly DbMarketsContext _context;
        public BaseController(DbMarketsContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {

            //var lsCats = HttpContext.Session.GetObjectFromJson<List<CategoryViewModel>>("lsCats");
            var lsCats = HttpContext.Session.GetObjectFromJson<List<Category>>("lsCats");

            base.OnActionExecuting(context);
            if (lsCats == null)
            {
                lsCats = _context.Categories.AsNoTracking().Where(x => x.Published && x.Outstanding).OrderBy(x => x.Ordering).Take(16).ToList();
                HttpContext.Session.SetObjectAsJson("lsCats", lsCats);
            }

            //if (lsCats == null)
            //{
            //    lsCats = _context.Categories
            //        .AsNoTracking()
            //        .Where(x => x.Published && x.Outstanding)
            //        .OrderBy(x => x.Ordering)
            //        .Take(16)
            //        .Select(cat => new CategoryViewModel
            //        {
            //            CategoryId = cat.CatId,
            //            CategoryName = cat.CatName,
            //            Alias = cat.Alias,
            //            Icon = cat.Thumb,
            //            ThumbShow = cat.ThumbShow,
            //            BannerThumb = cat.BannerThumb,
            //            Attributes = cat.CategoryAttributes
            //                .Where(ca => ca.IsActivated == true)
            //                .OrderBy(ca => ca.Attribute.Ordering)
            //                .Select(ca => new AttributeViewModel
            //                {
            //                    AttributeId = ca.Attribute.AttributeId,
            //                    AttributeName = ca.Attribute.Name,
            //                    AttributesPrices = ca.Attribute.AttributesPrices
            //                        .Where(ap => ap.ProductId == null)
            //                        .Select(ap => new AttributePriceViewModel
            //                        {
            //                            PriceId = ap.AttributesPriceId,
            //                            Price = ap.Price,
            //                            AttributeId = ap.Attribute.AttributeId,
            //                        }).ToList()
            //                }).ToList(),

            //            Brands = cat.CategoryBrands
            //                .Where(cb => cb.IsActivated == true)
            //                .Select(cb => new BrandViewModel
            //                {
            //                    BrandId = cb.Brand.BrandId,
            //                    BrandName = cb.Brand.BrandName,
            //                    MoTa = cb.Brand.MoTa,
            //                    Image = cb.Brand.Thumb
            //                })
            //                .Take(18)
            //                .ToList(),

            //            Slides = _context.Slides
            //                .Where(s => s.CatId == cat.CatId && s.IsActivated == true)
            //                .OrderBy(s => s.Ordering)
            //                .Select(s => new SlideVM
            //                {
            //                    SlideId = s.SlideId,
            //                    Thumb = s.Thumb,
            //                    Alias = s.Alias,
            //                    SlideName = s.SlideName,
            //                    IsActivated = s.IsActivated
            //                }).ToList(),
            //        })
            //        .ToList();

            //    // Lưu kết quả vào Session
            //    HttpContext.Session.SetObjectAsJson("lsCats", lsCats);
            //}

            // Gán vào ViewBag
            ViewBag.lsCat = lsCats;

        }
    }
}
