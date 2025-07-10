using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Ecommerce_CaFeShop.Models.ViewModels
{
    public class CustomerVM
    {
        [Required(ErrorMessage = "Họ tên không được để trống")]
        [Display(Name = "Họ tên")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; }

        [Display(Name = "Địa chỉ")]
        public string? Address { get; set; }

        [Display(Name = "Tỉnh/Thành phố")]
        public string? Tinh { get; set; }

        [Display(Name = "Quận/Huyện")]
        public string? Huyen { get; set; }

        [Display(Name = "Phường/Xã")]
        public string? Xa { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Tên hiển thị")]
        public string? DisplayName { get; set; }

        [Display(Name = "Ngày sinh")]
        public DateOnly? Dob { get; set; }

        [Display(Name = "Giới tính")]
        public bool? Gender { get; set; }

        [Display(Name = "Hình đại diện")]
        public string? HinhDaiDien { get; set; }

        [Display(Name = "Upload hình đại diện")]
        public IFormFile? AvatarFile { get; set; }
    }
}