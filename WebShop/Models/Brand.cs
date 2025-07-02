using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class Brand
{
    public int BrandId { get; set; }

    public string BrandName { get; set; }

    public string Description { get; set; }

    public string Thumb { get; set; }

    public virtual ICollection<BrandGroup> BrandGroups { get; set; } = new List<BrandGroup>();

    public virtual ICollection<CategoryBrand> CategoryBrands { get; set; } = new List<CategoryBrand>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<Video> Videos { get; set; } = new List<Video>();
}
