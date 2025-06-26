using System;
using System.Collections.Generic;
using System.Linq;
using WebShop.Models;

namespace WebShop.ModelViews
{
    public class TrangChuSanPhamVM
    {
        public DanhMuc category { get; set; }  
        public List<SanPham> lsProducts { get; set; } 
        public List<DanhMucThuongHieu> ctebrands { get; set; }     
        public BangQuangCao Banner { get; set; }
    }
}
