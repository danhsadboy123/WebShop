using System;
using System.Collections.Generic;
using WebShop.Models;

namespace WebShop.Areas.Admin.Models
{
    public class AttributeValueVM
    {
        public WebShop.Models.Attribute Attribute { get; set; }
        public List<AttributesPrice> AttributePrices { get; set; }
    }
}
