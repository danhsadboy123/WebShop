using System.Collections.Generic;
using WebShop.Models;

namespace WebShop.ModelViews
{
    public class DanhMucVM
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string TenRutGon { get; set; }
        public string Icon { get; set; }
        public bool? ThumbShow { get; set; } 
        public string BannerThumb { get; set; }
        public List<AttributeViewModel> Attributes { get; set; } = new List<AttributeViewModel>();
        public List<BrandViewModel> Brands { get; set; } = new List<BrandViewModel>(); // Thêm thuộc tính Brands
        public List<SlideVM> Slides { get; set; } = new List<SlideVM>();  
         
    }

    public class AttributeViewModel
    {
        public int AttributeId { get; set; }
        public string AttributeName { get; set; }
        public string TenRutGon { get; set; }

        public List<AttributePriceViewModel> AttributesPrices { get; set; } = new List<AttributePriceViewModel>();
    }

    public class BrandViewModel
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string MoTa { get; set; }
        public string Image { get; set; }
    }

    public class AttributePriceViewModel
    {
        public int PriceId { get; set; }
        public string Gia { get; set; }
        public int AttributeId { get; set; }
    }
    public class SlideVM
    {
        public int SlideId { get; set; }

        public int? CatId { get; set; }

        public string AnhNho { get; set; }

        public string TenRutGon { get; set; }

        public string SlideName { get; set; }

        public bool KichHoat { get; set; } 
        public int? Ordering { get; set; }

        public virtual DanhMuc Cat { get; set; }
    }

}
