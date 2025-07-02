using System.ComponentModel.DataAnnotations;

namespace Ecommerce_CaFeShop.Models.ViewModels
{
    public class ForgotPasswordVM
    {
        [Required(ErrorMessage = "Vui lòng nhập email.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string? Email { get; set; }
    }
}