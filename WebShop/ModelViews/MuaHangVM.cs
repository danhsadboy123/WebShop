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

        [Required(ErrorMessage = "*Vui long nhap Ho va Ten")]
        [RegularExpression(@"^[\p{L}\s]+$", ErrorMessage = "Ten nguoi nhan khong hop le")]
        public string HoTen { get; set; }

        [Required(ErrorMessage = "*Vui long nhap email")]
        [EmailAddress(ErrorMessage = "*Dia chi email khong hop le")]
        public string Email { get; set; }

        [Required(ErrorMessage = "*Vui long nhap so dien thoai")]
        [RegularExpression(@"^(0[3|5|7|8|9])[0-9]{8}$", ErrorMessage = "So dien thoai khong hop le")]
        public string SoDienThoai { get; set; }

        public List<AccountAddress> Addresses { get; set; }

        [Required(ErrorMessage = "*Vui long nhap dia chi nhan hang")]
        public string Address { get; set; }

        [Required(ErrorMessage = "*Vui long chon Tinh/Thanh")]
        public int TinhThanh { get; set; }

        [Required(ErrorMessage = "*Vui long chon Quan/Huyen")]
        public int QuanHuyen { get; set; }

        [Required(ErrorMessage = "*Vui long chon Phuong/Xa")]
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