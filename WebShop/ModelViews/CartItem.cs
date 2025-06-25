using System;
using System.Collections.Generic;
using WebShop.Models;

namespace WebShop.ModelViews
{
    public class CartItem
    {
        public Product product { get; set; }
        public int amount { get; set; }
        public double TotalMoney => amount * product.SalePrice.Value;
        public double CartTotalMoney { get; set; }
        public List<ProductGift> ProductGift { get; set; }       

    }
}
