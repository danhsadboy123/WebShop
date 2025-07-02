using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class Category
{
    public int CatId { get; set; }

    public string CatName { get; set; }

    public string CatNameEn { get; set; }

    public string Description { get; set; }

    public string DescriptionEn { get; set; }

    public int? ParentId { get; set; }

    public int? Levels { get; set; }

    public bool Outstanding { get; set; }

    public int? Ordering { get; set; }

    public bool Published { get; set; }

    public string Thumb { get; set; }

    public string Title { get; set; }

    public string TitleEn { get; set; }

    public string Alias { get; set; }

    public string MetaDesc { get; set; }

    public string MetaDescEn { get; set; }

    public string MetaKey { get; set; }

    public string MetaKeyEn { get; set; }

    public string Cover { get; set; }

    public string SchemaMarkup { get; set; }

    public string Icon { get; set; }

    public bool? BrandShow { get; set; }

    public bool? ThumbShow { get; set; }

    public string BannerThumb { get; set; }

    public string NameBrand { get; set; }

    public virtual ICollection<Banner> Banners { get; set; } = new List<Banner>();

    public virtual ICollection<CategoryAttribute> CategoryAttributes { get; set; } = new List<CategoryAttribute>();

    public virtual ICollection<CategoryBrand> CategoryBrands { get; set; } = new List<CategoryBrand>();

    public virtual ICollection<Category> InverseParent { get; set; } = new List<Category>();

    public virtual Category Parent { get; set; }

    public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();

    public virtual ICollection<Slide> Slides { get; set; } = new List<Slide>();
}
