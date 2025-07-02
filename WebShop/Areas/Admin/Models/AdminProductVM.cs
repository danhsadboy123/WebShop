using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebShop.Models;

namespace WebShop.Areas.Admin.Models
{
    public class AdminProductVM
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
        public string ProductName { get; set; }
        public string ProductName_EN { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mã sản phẩm")]
        public string ProductCode { get; set; }

        public string ShortDesc { get; set; }

        public string ShortDesc_EN { get; set; }

        public string Gift { get; set; }

        public string Gift_EN { get; set; }

        public string MoTa { get; set; }

        public string Description_EN { get; set; }

        public string ConfigInformation { get; set; }

        public string ConfigInformation_EN { get; set; }


        public int? Warranty { get; set; }

        public string WarrantyNote { get; set; }

        public string WarrantyNote_EN { get; set; }


        public int? Price { get; set; }

        public int? SalePrice { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn nhà sản xuất")]
        public int? BrandId { get; set; }

        public string Video { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        public bool BestSellers { get; set; }

        public bool HomeFlag { get; set; }

        public bool IsActivated { get; set; }

        public string Title { get; set; }

        public string Title_EN { get; set; }


        public string Alias { get; set; }

        public string MetaDesc { get; set; }

        public string MetaDesc_EN { get; set; }


        public string MetaKey { get; set; }

        public string MetaKey_EN { get; set; }
        public string ProductOption { get; set; }


        [Required(ErrorMessage = "Vui lòng nhập tồn kho")]
        public int? UnitsInStock { get; set; }

        public virtual ICollection<AttributesPrice> AttributesPrices { get; set; } = new List<AttributesPrice>();

        public virtual Brand Brand { get; set; }

        public virtual Category Cat { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        public virtual ICollection<ProductThumb> ProductThumbs { get; set; } = new List<ProductThumb>();
    }
}
