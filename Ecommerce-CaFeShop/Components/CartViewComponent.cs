
using Ecommerce_CaFeShop.Models;
using Ecommerce_CaFeShop.Models.ViewModels;
using Ecommerce_CaFeShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_CaFeShop.Components
{
    public class CartViewComponent : ViewComponent
    {
        private readonly ICartService _cartService;

        public CartViewComponent(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int? customerId = null;
            if (UserClaimsPrincipal?.Identity?.IsAuthenticated == true)
            {
                var customerIdClaim = UserClaimsPrincipal.Claims.FirstOrDefault(c => c.Type == "MaKhachHang");
                if (customerIdClaim != null && int.TryParse(customerIdClaim.Value, out var id))
                {
                    customerId = id;
                }
            }

            var cart = await _cartService.GetCartAsync(customerId, HttpContext.Session);

            return View("Default", new CartVM
            {
                Quantity = cart.Sum(p => p.Quantity),
                Total = cart.Sum(p => p.Total)
            });
        }
    }
}
