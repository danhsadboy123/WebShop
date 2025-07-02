using Ecommerce_CaFeShop.Models;
using Ecommerce_CaFeShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_CaFeShop.Components
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cart = CartHelper.GetCart(HttpContext.Session);
            return View("Default", new CartVM{
                Quantity = cart.Sum(p => p.Quantity),
                Total = cart.Sum(p => p.Total)
            });
        }
    }
}
