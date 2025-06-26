using System.Collections.Generic;
using WebShop.Models;

namespace WebShop.ModelViews
{
    public class DanhMucTrangChuVM
    {
        public string Catename { get; set; }
        public string Alias { get; set; }
        public int CountProduct { get; set; }
        public string Icon { get; set; }
        public List<Product> Products { get; set; } // Hoặc List<ProductViewModel> nếu bạn có lớp ViewModel cho Product
    }
}
