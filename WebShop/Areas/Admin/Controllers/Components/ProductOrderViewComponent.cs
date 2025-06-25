using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebShop.Areas.Admin.Models;
using WebShop.Extension;
using WebShop.ModelViews;

namespace WebShop.Areas.Admin.Controllers.Component
{
    public class ProductOrderViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("Order");
            return View(cart);
        }
    }
}
