using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebShop.Models;

namespace WebShop.ModelViews
{
    public class MenuHeader
    {
        public DanhMuc Category { get; set; } 
        public List<ThuocTinh> attribute { get; set; }
        public List<MenuHeader> Children { get; set; }
    }
}
