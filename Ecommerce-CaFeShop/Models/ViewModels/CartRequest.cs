namespace Ecommerce_CaFeShop.Models.ViewModels
{
    public class CartRequest
    {
        public int ProductId { get; set; }
        public string Slug { get; set; }
        public string ProductName { get; set; }
        public string? Image { get; set; }
        public double Price { get; set; } // Giá sau khuyến mãi
        public double? OriginalPrice { get; set; } // Giá gốc
        public int Quantity { get; set; }
        public double Total { get; set; }

        // Kiểm tra có khuyến mãi không
        public bool HasDiscount => OriginalPrice.HasValue && OriginalPrice.Value > Price;

        // Tính phần trăm giảm giá
        //public double DiscountPercentage => HasDiscount ? Math.Round(((OriginalPrice.Value - Price) / OriginalPrice.Value) * 100, 0) : 0;
    }
}