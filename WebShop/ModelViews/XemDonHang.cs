using System;
using System.Collections.Generic;
using WebShop.Models;

namespace WebShop.ModelViews
{
    public class XemDonHang
    {
        public DonHang DonHang { get; set; }
        public ShippingAddress DiaChi { get; set; }
        public List<OrderDetail> ChiTietDonHang { get; set; }
    }
}
