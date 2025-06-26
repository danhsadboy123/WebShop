using System;
using System.Collections.Generic;
using WebShop.Models;
 

namespace WebShop.ModelViews
{
    public class DanhMucView
    {
        public DanhMuc danhMuc { get; set; }
        public List<DanhMucThuocTinh> categoryAttributes { get; set; } 
    }
}
