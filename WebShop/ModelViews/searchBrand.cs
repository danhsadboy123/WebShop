using System;
using System.Collections.Generic;
using WebShop.Models;

namespace WebShop.ModelViews
{
    public class searchBrand
    {     
        public Category categories { get; set; }
        public List<Brand> brand { get; set; } = new List<Brand>(); // Đảm bảo khởi tạo danh sách
        public string sort { get; set; }  
    }
}
