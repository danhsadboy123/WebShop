using System.ComponentModel.DataAnnotations;

namespace Ecommerce_WatchShop.Models.ViewModels
{
    public class CheckoutValidationVM
    {
        public IEnumerable<CartRequest> CartRequest { get; set; } = null!;
        public CheckoutVM CheckoutVM { get; set; } = null!; 
    }
}
