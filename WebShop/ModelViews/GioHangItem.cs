using System;
using System.Collections.Generic;
using WebShop.Models;

namespace WebShop.ModelViews
{
    public class GioHangItem
    {
        public SanPham product { get; set; }
        public int amount { get; set; }
        public double TotalMoney => amount * product.SalePrice.Value;
        public double CartTotalMoney { get; set; }
        public List<QuaTangSanPham> ProductGift { get; set; }       

    }
}
