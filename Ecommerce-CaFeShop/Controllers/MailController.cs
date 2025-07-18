
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MailKit.Net.Smtp;
using Ecommerce_CaFeShop.Models.ViewModels;

namespace Ecommerce_CaFeShop.Controllers
{
    public class MailController : Controller
    {
        private readonly IConfiguration _configuration;

        public MailController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMail(SendMailVM model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            try
            {
                await SendEmailAsync(model);
                TempData["success"] = "Email đã được gửi thành công!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Có lỗi xảy ra khi gửi email: {ex.Message}";
                return View("Index", model);
            }
        }

        private async Task SendEmailAsync(SendMailVM model)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");
            var fromEmail = emailSettings["FromEmail"];
            var fromName = emailSettings["FromName"];
            var smtpServer = emailSettings["SmtpServer"];
            var smtpPort = int.Parse(emailSettings["SmtpPort"] ?? "587");
            var smtpUsername = emailSettings["SmtpUsername"];
            var smtpPassword = emailSettings["SmtpPassword"];

            if (string.IsNullOrEmpty(fromEmail) || string.IsNullOrEmpty(smtpServer) ||
                string.IsNullOrEmpty(smtpUsername) || string.IsNullOrEmpty(smtpPassword))
            {
                throw new Exception("Cấu hình email chưa đầy đủ. Vui lòng kiểm tra appsettings.json");
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(fromName ?? "CaFe Shop", fromEmail));
            message.To.Add(new MailboxAddress(model.ToName ?? model.ToEmail, model.ToEmail));

            if (!string.IsNullOrEmpty(model.CcEmail))
            {
                message.Cc.Add(new MailboxAddress("", model.CcEmail));
            }

            if (!string.IsNullOrEmpty(model.BccEmail))
            {
                message.Bcc.Add(new MailboxAddress("", model.BccEmail));
            }

            message.Subject = model.Subject;

            var bodyBuilder = new BodyBuilder();

            if (model.IsHtml)
            {
                bodyBuilder.HtmlBody = model.Body;
            }
            else
            {
                bodyBuilder.TextBody = model.Body;
            }

            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(smtpUsername, smtpPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
