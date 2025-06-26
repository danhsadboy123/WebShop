using System.Collections.Generic;
using System;
using WebShop.Models;

namespace WebShop.Areas.Admin.Models
{
    public class AdminCustomerCreateVM
    {
        public int CustomerId { get; set; }

        public string HoTen { get; set; }

        public string Avatar { get; set; }

        public string Email { get; set; }

        public string SoDienThoai { get; set; }

        public DateTime Birthday { get; set; }
        public bool? Gender { get; set; }

        public string Note { get; set; }

        public string Address { get; set; }

        public string PhoneAddress { get; set; }
        public string CompanyName { get; set; }

        public int ProvinceId { get; set; }
        
        public int DistrictId { get; set; }

        public int WardId { get; set; }


    }
}
