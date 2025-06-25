using System;
using System.Collections.Generic;
using System.Linq;
using WebShop.Models;
using WebShop.ModelViews;

namespace WebShop.Areas.Admin.Models
{
    public class QuotationVM
    {
        public List<CartItem> products { get; set; }
        public int VAT { get; set; }
        public double TotalMoneys { get; set; }

    }
}
