using System;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML.Voice;
using Twilio.Types;
using WebShop.Helpper;
using WebShop.Models; 
namespace WebShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminSMSController : Controller
    {
        private readonly DbMarketsContext _context;
        public INotyfService _notyfService { get; }
        public AdminSMSController(DbMarketsContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        public IActionResult Index()
        {
            string toPhoneNumber = "+84984855261"; // Số điện thoại nhận
            string message = "Hello, this is a test message from Twilio!";

            // Gọi phương thức SendSms từ controller hiện tại
            SendSms(toPhoneNumber, message);

            return View();
        }

        public void SendSms(string toPhoneNumber, string message)
        {
            // Thay thế bằng thông tin tài khoản Twilio của bạn
            const string accountSid = "ACc2ccdbd57749a530354a760fbfa5bdff";
            const string authToken = "f5432588ecf7d230aba73c3f3301cf22";

            // Khởi tạo đối tượng TwilioClient với thông tin xác thực
            TwilioClient.Init(accountSid, authToken);

            try
            {
                // Gửi tin nhắn SMS
                var smsMessage = MessageResource.Create(
                    body: message,
                    from: new PhoneNumber("+17867432650"),
                    to: new PhoneNumber(toPhoneNumber)
                );

                // Nếu tin nhắn đã được gửi thành công, hiển thị thông báo thành công
                if (smsMessage != null && !string.IsNullOrEmpty(smsMessage.Sid))
                {
                    Console.WriteLine("Tin nhắn đã được gửi thành công!");
                    Console.WriteLine($"Message SID: {smsMessage.Sid}");
                }
                else
                {
                    Console.WriteLine("Có lỗi xảy ra khi gửi tin nhắn!");
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi khi gửi tin nhắn
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

    }

}
