using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class CategoryAttribute
{
    public int CategoryAttributeId { get; set; }

    public int? CatId { get; set; }

    public int? AttributeId { get; set; }

    public string Name { get; set; }

    public bool? Active { get; set; }

    public bool? Show { get; set; }

    public int? Odering { get; set; }

    public virtual Attribute Attribute { get; set; }

    public virtual Category Cat { get; set; }
}
