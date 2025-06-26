using System;
using System.Collections.Generic;
using WebShop.Models;
 

namespace WebShop.ModelViews
{
    public class CategoryView
    {
        public DanhMuc category { get; set; }
        public List<DanhMucThuocTinh> categoryAttributes { get; set; } 
    }
}
