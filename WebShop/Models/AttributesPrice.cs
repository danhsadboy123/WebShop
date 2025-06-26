using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class AttributesPrice
{
    public int AttributesPriceId { get; set; }

    public int? AttributeId { get; set; }

    public int? ProductId { get; set; }

    public string Price { get; set; }

    public string PriceEn { get; set; }

    public bool KichHoat { get; set; }

    public virtual Attribute Attribute { get; set; }

    public virtual Product Product { get; set; }
}
