using System;
using System.ComponentModel.DataAnnotations;

namespace WebShop.ModelViews
{
    public class DoiThongTinVM
    {
        [Key]
        public int CustomerId { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
        public DateTime Birthday { get; set; }
        public bool Gender { get; set; }
    }
}
