using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class Attribute
{
    public int AttributeId { get; set; }

    public string Name { get; set; }

    public string NameEn { get; set; }

    public int? Ordering { get; set; }

    public bool KichHoat { get; set; }

    public virtual ICollection<AttributesPrice> AttributesPrices { get; set; } = new List<AttributesPrice>();

    public virtual ICollection<CategoryAttribute> CategoryAttributes { get; set; } = new List<CategoryAttribute>();
}
