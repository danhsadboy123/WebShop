using System;
using System.Collections.Generic;
using System.Linq;
using WebShop.Models;

namespace WebShop.ModelViews
{
    public class TrangChuSanPhamVM
    {
        public DanhMuc danhMuc { get; set; }  
        public List<SanPham> danhSachSanPham { get; set; } 
        public List<DanhMucThuongHieu> danhSachThuongHieu { get; set; }     
        public BangQuangCao BangQuangCao { get; set; }
    }
}
