using Microsoft.AspNetCore.Mvc;

namespace WebShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminAjaxContentController : Controller
    {
        public IActionResult ProductQuotation()
        {
            return ViewComponent("ProductQuotation");
        }
        public IActionResult ProductQuotationDetails(int data)
        {
            return ViewComponent("ProductQuotationDetails", data);
        }
        public IActionResult ProductOrder()
        {
            return ViewComponent("ProductOrder");
        }
        public IActionResult ProductOrderDetails(int data)
        {
            return ViewComponent("ProductOrderDetails", data);
        }
    }
}
