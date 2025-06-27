using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebShop.Areas.Admin.Models;
using WebShop.Extension;
using WebShop.ModelViews;

namespace WebShop.Areas.Admin.Controllers.Component
{
    public class ProductQuotationDetailsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int vat)
        {
            ViewBag.vat = vat;
            var cart = HttpContext.Session.Get<List<GioHangItem>>("BaoGia");
            return View(cart);
        }
    }
}
