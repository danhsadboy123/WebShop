
using System.ComponentModel.DataAnnotations;

namespace Ecommerce_CaFeShop.Models.ViewModels
{
    public class CheckoutVM
    {
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; } = string.Empty;

        [Display(Name = "Tỉnh/Thành phố")]
        public string? Province { get; set; }

        [Display(Name = "Quận/Huyện")]
        public string? District { get; set; }

        [Display(Name = "Xã/Phường")]
        public string? Ward { get; set; }

        [Display(Name = "Phương thức thanh toán")]
        public string PaymentMethod { get; set; } = "COD";

        public decimal TotalAmount { get; set; }

        public string TotalAmountVND
        {
            get
            {
                return String.Format(new System.Globalization.CultureInfo("vi-VN"), "{0:C}", TotalAmount);
            }
        }
    }
}
