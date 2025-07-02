using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebShop.Models;

namespace WebShop.ModelViews
{
    public class HeaderMenu
    {
        public Category Category { get; set; } 
        public List<Attribute> attribute { get; set; }
        public List<HeaderMenu> Children { get; set; }
    }
}
