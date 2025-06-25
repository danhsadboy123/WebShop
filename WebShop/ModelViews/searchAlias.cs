using System;
using System.Collections.Generic;
using WebShop.Models;

namespace WebShop.ModelViews
{
    public class searchAlias
    {
        public List<AttributesPrice> attrp { get; set; } = new List<AttributesPrice>(); // Khởi tạo danh sách
        public List<Brand> brand { get; set; } = new List<Brand>(); // Khởi tạo danh sách
        public BrandGroup brandGroup { get; set; }
        public string Alias { get; set; }
        public string sort { get; set; }
        public int top { get; set; }
    }

}
