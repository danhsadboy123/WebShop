using Ecommerce_CaFeShop.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Ecommerce_CaFeShop.Helper
{
    public static class CartHelper
    {
        private const string CartSessionKey = "Cart";

        public static List<CartRequest> GetCart(ISession session)
        {
            try
            {
                var cartJson = session.GetString(CartSessionKey);
                if (string.IsNullOrEmpty(cartJson))
                {
                    return new List<CartRequest>();
                }

                var cart = JsonSerializer.Deserialize<List<CartRequest>>(cartJson);
                return cart ?? new List<CartRequest>();
            }
            catch
            {
                // Nếu có lỗi deserialize, clear cart và return empty list
                session.Remove(CartSessionKey);
                return new List<CartRequest>();
            }
        }

        public static void SaveCart(ISession session, List<CartRequest> cart)
        {
            try
            {
                if (cart == null || cart.Count == 0)
                {
                    session.Remove(CartSessionKey);
                }
                else
                {
                    var cartJson = JsonSerializer.Serialize(cart);
                    session.SetString(CartSessionKey, cartJson);
                }
            }
            catch
            {
                // Nếu có lỗi serialize, clear cart
                session.Remove(CartSessionKey);
            }
        }

        public static void AddToCart(ISession session, CartRequest item)
        {
            if (item == null) return;

            var cart = GetCart(session);
            var existingItem = cart.FirstOrDefault(x => x.ProductId == item.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                cart.Add(item);
            }

            SaveCart(session, cart);
        }

        public static void RemoveFromCart(ISession session, int productId)
        {
            var cart = GetCart(session);
            cart.RemoveAll(x => x.ProductId == productId);
            SaveCart(session, cart);
        }

        public static void UpdateQuantity(ISession session, int productId, int quantity)
        {
            var cart = GetCart(session);
            var item = cart.FirstOrDefault(x => x.ProductId == productId);

            if (item != null)
            {
                if (quantity <= 0)
                {
                    cart.Remove(item);
                }
                else
                {
                    item.Quantity = quantity;
                }
            }

            SaveCart(session, cart);
        }

        public static void ClearCart(ISession session)
        {
            session.Remove(CartSessionKey);
        }

        public static int GetCartItemCount(ISession session)
        {
            var cart = GetCart(session);
            return cart.Sum(x => x.Quantity);
        }

        public static decimal GetCartTotal(ISession session)
        {
            var cart = GetCart(session);
            return cart.Sum(x => (decimal)(x.Price * x.Quantity));
        }

        public static bool HasItems(ISession session)
        {
            var cart = GetCart(session);
            return cart != null && cart.Count > 0;
        }
    }
}