using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class CategoryBrand
{
    public int CategoryBrandId { get; set; }

    public int? CatId { get; set; }

    public int? BrandId { get; set; }

    public string Name { get; set; }

    public string MoTa { get; set; }

    public bool? IsActivated { get; set; }

    public bool? Topbrand { get; set; }

    public bool? BrandProduct { get; set; }

    public virtual Brand Brand { get; set; }

    public virtual Category Cat { get; set; }
}
