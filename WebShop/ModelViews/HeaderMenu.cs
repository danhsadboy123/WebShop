using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebShop.Models;

namespace WebShop.ModelViews
{
    public class HeaderMenu
    {
        public DanhMuc Category { get; set; } 
        public List<ThuocTinh> attribute { get; set; }
        public List<HeaderMenu> Children { get; set; }
    }
}
