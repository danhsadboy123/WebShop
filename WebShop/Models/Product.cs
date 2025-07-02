using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; }

    public string ProductNameEn { get; set; }

    public string ProductCode { get; set; }

    public string ShortDesc { get; set; }

    public string ShortDescEn { get; set; }

    public string MoTa { get; set; }

    public string DescriptionEn { get; set; }

    public string ConfigInformation { get; set; }

    public string ConfigInformationEn { get; set; }

    public string Gift { get; set; }

    public string GiftEn { get; set; }

    public int? Warranty { get; set; }

    public string WarrantyNote { get; set; }

    public string WarrantyNoteEn { get; set; }

    public int? Price { get; set; }

    public int? SalePrice { get; set; }

    public int? BrandId { get; set; }

    public string Avatar { get; set; }

    public string Video { get; set; }

    public DateTime? DateCreated { get; set; }

    public DateTime? DateModified { get; set; }

    public bool BestSellers { get; set; }

    public bool HomeFlag { get; set; }

    public bool IsActivated { get; set; }

    public string Title { get; set; }

    public string TitleEn { get; set; }

    public string Alias { get; set; }

    public string MetaDesc { get; set; }

    public string MetaDescEn { get; set; }

    public string MetaKey { get; set; }

    public string MetaKeyEn { get; set; }

    public int? UnitsInStock { get; set; }

    public string ProductOption { get; set; }

    public int? BrandGroup { get; set; }

    public virtual ICollection<AttributesPrice> AttributesPrices { get; set; } = new List<AttributesPrice>();

    public virtual Brand Brand { get; set; }

    public virtual ICollection<DiscountAddProduct> DiscountAddProducts { get; set; } = new List<DiscountAddProduct>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<ProductAddCusPro> ProductAddCusPros { get; set; } = new List<ProductAddCusPro>();

    public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();

    public virtual ICollection<ProductThumb> ProductThumbs { get; set; } = new List<ProductThumb>();

    public virtual ICollection<QuotationDetail> QuotationDetails { get; set; } = new List<QuotationDetail>();
}
