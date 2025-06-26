using System;
using System.Collections.Generic;
using WebShop.Models;

namespace WebShop.Areas.Admin.Models
{
    public class AttributeValueVM
    {
        public WebShop.Models.ThuocTinh Attribute { get; set; }
        public List<GiaThuocTinh> AttributePrices { get; set; }
    }
}
