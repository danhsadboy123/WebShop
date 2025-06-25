using System.Collections.Generic;
using WebShop.Models;

namespace WebShop.Areas.Admin.Models
{
    public class AdminProductPageVM
    {
        public CheckboxProduct CheckboxProduct { get; set; }
        public List<Product> Products { get; set; }
    }
}
