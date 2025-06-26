using System;
using System.Collections.Generic;
using WebShop.Models;

namespace WebShop.ModelViews
{
    public class GioHangItem
    {
        public SanPham sanPham { get; set; }
        public int soLuong { get; set; }
        public double tongTien => soLuong * sanPham.GiaBan.Value;
        public double tongTienGioHang { get; set; }
        public List<QuaTangSanPham> QuaTangSanPham { get; set; }       

    }
}
