using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebShop.Models;

namespace WebShop.ModelViews
{
    public class MuaHangVM
    {
        public int orderId { get; set; }

        public int CustomerId { get; set; }
        public int GuestId { get; set; }

        public bool IsLoggedIn { get; set; }

        [Required(ErrorMessage = "*Vui lòng nhập Họ và Tên")]
        [RegularExpression(@"^[\p{L}\s]+$", ErrorMessage = "Tên người nhận không hợp lệ")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "*Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "*Địa chỉ email không hợp lệ")]
        public string Email { get; set; }
        [Required(ErrorMessage = "*Vui lòng nhập số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string Phone { get; set; }

        public List<AccountAddress> Addresses { get; set; }

        [Required(ErrorMessage = "*Vui lòng nhập địa chỉ nhận hàng")]
        public string Address { get; set; }

        [Required(ErrorMessage = "*Vui lòng chọn Tỉnh/Thành")]
        public int TinhThanh { get; set; }

        [Required(ErrorMessage = "*Vui lòng chọn Quận/Huyện")]
        public int QuanHuyen { get; set; }

        [Required(ErrorMessage = "*Vui lòng chọn Phường/Xã")]
        public int PhuongXa { get; set; }

        public int PaymentID { get; set; }

        public string Companyname { get; set; }
        public string NumberCom { get; set; }
        public string AddressCom { get; set; }
        public string Note { get; set; }
        public bool CheckEmail { get; set; }
        public string Code { get; set; }
        public string NotePay { get; set; }         
    }
}
