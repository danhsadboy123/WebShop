using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebShop.ModelViews
{
    public class DangKyVM
    {
        [Key]
        public int maKhachHang { get; set; }

        [Display(Name ="Họ và Tên")]
        [Required(ErrorMessage ="Vui lòng nhập Họ Tên")]
        public string HoTen { get; set; }

        [MaxLength(150)]
        [Required(ErrorMessage ="Vui lòng nhập Email")]
        [DataType(DataType.EmailAddress)]
        [Remote(action:"ValidateEmail", controller:"TaiKhoans")]
        public string Email { get; set; }

        [MaxLength(11)]
        [Required(ErrorMessage ="Vui lòng nhập số điện thoại")]
        [Display(Name ="Điện thoại")]
        [DataType(DataType.PhoneNumber)]
        [Remote(action:"ValidatePhone",controller:"TaiKhoans")]
        public string SoDienThoai { get; set; }

        [Display(Name ="Mật khẩu")]
        [Required(ErrorMessage ="Vui lòng nhập mật khẩu")]
        [MinLength(5,ErrorMessage ="Bạn cần đặt mật khẩu tối thiểu 5 ký tự")]
        public string MatKhau { get; set; }

        [MinLength(5,ErrorMessage ="Bạn cần đặt mật khẩu tối thiểu 5 ký tự")]
        [Display(Name ="Nhập lại mật khẩu")]
        [Compare("MatKhau",ErrorMessage ="Nhập lại mật khẩu không đúng")]
        public string ConfirmPassword { get; set; }
    }
}
