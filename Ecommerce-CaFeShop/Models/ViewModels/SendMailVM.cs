
using System.ComponentModel.DataAnnotations;

namespace Ecommerce_CaFeShop.Models.ViewModels
{
    public class SendMailVM
    {
        [Required(ErrorMessage = "Vui lòng nhập email người nhận")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email người nhận")]
        public string ToEmail { get; set; } = "";

        [Display(Name = "Tên người nhận")]
        public string? ToName { get; set; }

        [EmailAddress(ErrorMessage = "Email CC không hợp lệ")]
        [Display(Name = "Email CC (tùy chọn)")]
        public string? CcEmail { get; set; }

        [EmailAddress(ErrorMessage = "Email BCC không hợp lệ")]
        [Display(Name = "Email BCC (tùy chọn)")]
        public string? BccEmail { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tiêu đề")]
        [Display(Name = "Tiêu đề")]
        public string Subject { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng nhập nội dung email")]
        [Display(Name = "Nội dung")]
        public string Body { get; set; } = "";

        [Display(Name = "Gửi dưới dạng HTML")]
        public bool IsHtml { get; set; } = false;
    }
}
