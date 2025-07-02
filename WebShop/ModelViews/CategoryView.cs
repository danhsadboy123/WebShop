using System;
using System.Collections.Generic;
using WebShop.Models;
 

namespace WebShop.ModelViews
{
    public class CategoryView
    {
        public Category category { get; set; }
        public List<CategoryAttribute> categoryAttributes { get; set; } 
    }
}
