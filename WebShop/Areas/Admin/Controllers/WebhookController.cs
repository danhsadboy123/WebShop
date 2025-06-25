using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebShop.Areas.Admin.Models;

namespace WebShop.Areas.Admin.Controllers
{
    [ApiController]
    [Route("api/")]
    public class WebhookController : ControllerBase
    {
        [HttpPost("ghn/shipping-order/update")]
        public IActionResult ReceiveWebhookGHN([FromBody] GHNOrder data)
        {
            // Xử lý dữ liệu từ webhook ở đây
            // data chứa thông tin gửi từ GHN
            // Trả về response tùy theo yêu cầu của GHN
            var data1 = data;
            return Ok();
        }
    }
}
