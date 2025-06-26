using System;
using System.Collections.Generic;
using WebShop.Models;

namespace WebShop.ModelViews
{
    public class XemDonHangVM
    {
        public DonHang DonHang { get; set; }
        public DiaChiGiaoHang DiaChi { get; set; }
        public List<ChiTietDonHang> ChiTietDonHang { get; set; }
    }
}
