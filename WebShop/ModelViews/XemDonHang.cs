using System;
using System.Collections.Generic;
using WebShop.Models;

namespace WebShop.ModelViews
{
    public class XemDonHang
    {
        public Order DonHang { get; set; }
        public ShippingAddress DiaChi { get; set; }
        public List<OrderDetail> ChiTietDonHang { get; set; }
    }
}
