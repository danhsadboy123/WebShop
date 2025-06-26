using System.Collections.Generic;
using WebShop.Models;

namespace WebShop.Areas.Admin.Models
{
    public class AdminOrderVM
    {
        public CheckboxOrder CheckboxOrder { get; set; }
        public List<DonHang> Orders { get; set; }
    }
}
