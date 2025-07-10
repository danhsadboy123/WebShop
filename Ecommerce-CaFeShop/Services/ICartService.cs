
using Ecommerce_CaFeShop.Models.ViewModels;

namespace Ecommerce_CaFeShop.Services
{
    public interface ICartService
    {
        Task<List<CartRequest>> GetCartAsync(int? customerId, ISession session);
        Task AddToCartAsync(int? customerId, ISession session, string slug, int quantity);
        Task UpdateCartAsync(int? customerId, ISession session, string slug, int quantity);
        Task RemoveFromCartAsync(int? customerId, ISession session, string slug);
        Task ClearCartAsync(int? customerId, ISession session);
        Task MergeSessionCartToDatabase(int customerId, ISession session);
    }
}
