using System;
using System.Collections.Generic;
using System.Linq;
using WebShop.Models;

namespace WebShop.ModelViews
{
    public class ProductHomeVM
    {
        public DanhMuc category { get; set; }  
        public List<Product> lsProducts { get; set; } 
        public List<DanhMucThuongHieu> ctebrands { get; set; }     
        public Banner Banner { get; set; }
    }
}
