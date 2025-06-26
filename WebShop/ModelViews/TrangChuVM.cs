using System;
using System.Collections.Generic;
using WebShop.Models;

namespace WebShop.ModelViews
{
    public class TrangChuVM
    {
        public List<TinDang> TinTucs { get; set; }
        public List<TrangTrinhBay> Slides { get; set; }

        public List<TrangChuSanPhamVM> Products { get; set; }
        public QuangCao quangcao { get; set; }
        public List<KhuyenMai> lsDiscount { get; set; }




    }
}
