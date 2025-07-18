
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ecommerce_CaFeShop.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

namespace Ecommerce_CaFeShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Admin")]
    public class BillController : Controller
    {
        private readonly CaFeContext _context;
        private readonly IConfiguration _configuration;

        public BillController(CaFeContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index(int? status, DateTime? dateFilter, string search, int page = 1)
        {
            const int pageSize = 10;

            var query = _context.HoaDons
                .Include(h => h.KhachHang)
                .AsQueryable();

            // Lọc theo trạng thái
            if (status.HasValue)
            {
                query = query.Where(h => h.TrangThai == status.Value);
            }

            // Lọc theo ngày
            if (dateFilter.HasValue)
            {
                query = query.Where(h => h.NgayDatHang.Date == dateFilter.Value.Date);
            }

            // Lọc theo tìm kiếm
            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim();
                query = query.Where(h =>
                    h.MaHoaDon.ToString().Contains(search) ||
                    h.HoTen.Contains(search) ||
                    h.SoDienThoai.Contains(search) ||
                    h.Email.Contains(search));
            }

            var totalBills = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalBills / (double)pageSize);

            var bills = await query
                .OrderByDescending(h => h.NgayDatHang)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Thống kê trạng thái đơn hàng (tổng, không áp dụng lọc)
            ViewBag.PendingCount = await _context.HoaDons.CountAsync(h => h.TrangThai == 0);
            ViewBag.ConfirmedCount = await _context.HoaDons.CountAsync(h => h.TrangThai == 7);
            ViewBag.ProcessingCount = await _context.HoaDons.CountAsync(h => h.TrangThai == 6);
            ViewBag.CancelledCount = await _context.HoaDons.CountAsync(h => h.TrangThai == 5);
            ViewBag.CompletedCount = await _context.HoaDons.CountAsync(h => h.TrangThai == 8);

            // Thông tin phân trang
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalBills = totalBills;
            ViewBag.HasPrevious = page > 1;
            ViewBag.HasNext = page < totalPages;

            // Truyền các giá trị lọc hiện tại về view
            ViewBag.CurrentStatus = status;
            ViewBag.DateFilter = dateFilter?.ToString("yyyy-MM-dd");
            ViewBag.Search = search;

            return View(bills);
        }

        public async Task<IActionResult> Details(int id)
        {
            var bill = await _context.HoaDons
                .Include(h => h.KhachHang)
                .Include(h => h.ChiTietHoaDons)
                    .ThenInclude(ct => ct.SanPham)
                .FirstOrDefaultAsync(h => h.MaHoaDon == id);

            if (bill == null)
            {
                return NotFound();
            }

            return View(bill);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Admin/Bill/UpdateStatus")]
        public async Task<IActionResult> UpdateStatus(int id, int status)
        {
            try
            {
                Console.WriteLine($"UpdateStatus called with id: {id}, status: {status}");

                if (id <= 0)
                {
                    TempData["error"] = "ID đơn hàng không hợp lệ";
                    return RedirectToAction("Index");
                }

                var bill = await _context.HoaDons.FindAsync(id);
                if (bill == null)
                {
                    TempData["error"] = "Không tìm thấy đơn hàng";
                    return NotFound();
                }

                Console.WriteLine($"Current bill status: {bill.TrangThai}");

                // Kiểm tra trạng thái hợp lệ
                if (status < 0 || status > 10)
                {
                    TempData["error"] = "Trạng thái không hợp lệ";
                    return RedirectToAction("Details", new { id = id });
                }

                // Kiểm tra logic chuyển trạng thái
                if (bill.TrangThai == 5 && status != 5)
                {
                    TempData["error"] = "Không thể thay đổi trạng thái của đơn hàng đã hủy";
                    return RedirectToAction("Details", new { id = id });
                }

                if ((bill.TrangThai == 8 || bill.TrangThai == 4) && status != bill.TrangThai)
                {
                    TempData["error"] = "Không thể thay đổi trạng thái của đơn hàng đã hoàn thành";
                    return RedirectToAction("Details", new { id = id });
                }

                int oldStatus = bill.TrangThai;
                bill.TrangThai = status;

                // Nếu trạng thái là hủy (5), cập nhật thông tin hủy
                if (status == 5 && oldStatus != 5)
                {
                    bill.NgayHuy = DateTime.Now;
                    bill.LyDoHuy = "Đơn hàng bị hủy bởi Admin";
                }

                await _context.SaveChangesAsync();
                Console.WriteLine($"Status updated successfully from {oldStatus} to {status}");

                // Gửi email thông báo nếu đơn hàng bị hủy
                if (status == 5 && oldStatus != 5)
                {
                    try
                    {
                        await SendCancelOrderNotification(bill);
                        Console.WriteLine("Cancel notification email sent successfully");
                    }
                    catch (Exception ex)
                    {
                        // Log lỗi nhưng không làm gián đoạn quá trình
                        Console.WriteLine($"Email notification error: {ex.Message}");
                        // Không throw exception để không làm fail việc cập nhật status
                    }
                }

                string statusText = status switch
                {
                    0 => "Chờ xác nhận",
                    1 => "Chưa thanh toán",
                    2 => "Đã thanh toán",
                    3 => "Đang giao hàng",
                    4 => "Đã giao hàng",
                    5 => "Đã hủy",
                    6 => "Đang xử lý",
                    7 => "Đã xác nhận",
                    8 => "Hoàn thành",
                    9 => "Trả hàng",
                    10 => "Hoàn tiền",
                    _ => "Không xác định"
                };

                TempData["success"] = $"Cập nhật trạng thái đơn hàng thành công: {statusText}";
                return RedirectToAction("Details", new { id = id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateStatus: {ex.Message}");
                TempData["error"] = $"Có lỗi xảy ra khi cập nhật trạng thái: {ex.Message}";
                return RedirectToAction("Details", new { id = id });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelOrder(int id, string reason)
        {
            try
            {
                var bill = await _context.HoaDons.FindAsync(id);
                if (bill == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy đơn hàng" });
                }

                // Kiểm tra xem đơn hàng có thể hủy không
                if (bill.TrangThai == 5)
                {
                    return Json(new { success = false, message = "Đơn hàng đã bị hủy trước đó" });
                }

                if (bill.TrangThai == 8 || bill.TrangThai == 4)
                {
                    return Json(new { success = false, message = "Không thể hủy đơn hàng đã hoàn thành hoặc đã giao" });
                }

                // Kiểm tra lý do hủy
                if (string.IsNullOrWhiteSpace(reason))
                {
                    return Json(new { success = false, message = "Vui lòng nhập lý do hủy đơn hàng" });
                }

                // Cập nhật trạng thái hủy
                int oldStatus = bill.TrangThai;
                bill.TrangThai = 5;
                bill.NgayHuy = DateTime.Now;
                bill.LyDoHuy = reason.Trim();

                await _context.SaveChangesAsync();

                // Gửi email thông báo cho khách hàng
                try
                {
                    await SendCancelOrderNotification(bill);
                    return Json(new
                    {
                        success = true,
                        message = "Đơn hàng đã được hủy thành công và đã gửi thông báo cho khách hàng",
                        newStatus = "Đã hủy",
                        cancelDate = bill.NgayHuy?.ToString("dd/MM/yyyy HH:mm"),
                        cancelReason = bill.LyDoHuy
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Email error: {ex.Message}");
                    return Json(new
                    {
                        success = true,
                        message = "Đơn hàng đã được hủy thành công nhưng không thể gửi email thông báo",
                        newStatus = "Đã hủy",
                        cancelDate = bill.NgayHuy?.ToString("dd/MM/yyyy HH:mm"),
                        cancelReason = bill.LyDoHuy
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error cancelling order: {ex.Message}");
                return Json(new { success = false, message = $"Có lỗi xảy ra: {ex.Message}" });
            }
        }

        private async Task SendCancelOrderNotification(HoaDon order)
        {
            try
            {
                var emailSettings = _configuration.GetSection("EmailSettings");
                var fromEmail = emailSettings["FromEmail"];
                var fromName = emailSettings["FromName"];
                var smtpServer = emailSettings["SmtpServer"];
                var smtpPort = int.Parse(emailSettings["SmtpPort"] ?? "587");
                var smtpUsername = emailSettings["SmtpUsername"];
                var smtpPassword = emailSettings["SmtpPassword"];

                // Kiểm tra cấu hình email
                if (string.IsNullOrEmpty(fromEmail) || string.IsNullOrEmpty(smtpServer) ||
                    string.IsNullOrEmpty(smtpUsername) || string.IsNullOrEmpty(smtpPassword))
                {
                    Console.WriteLine("Email configuration is incomplete. Skipping email notification.");
                    return;
                }

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(fromName, fromEmail));
                message.To.Add(new MailboxAddress(order.HoTen, order.Email));
                message.Subject = $"Thông báo hủy đơn hàng #DH{order.MaHoaDon.ToString("D6")}";

                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                    <div style='background-color: #f8f9fa; padding: 20px; text-align: center;'>
                        <h1 style='color: #dc3545; margin: 0;'>Thông báo hủy đơn hàng</h1>
                    </div>
                    
                    <div style='padding: 30px; background-color: white;'>
                        <p>Xin chào <strong>{order.HoTen}</strong>,</p>
                        
                        <p>Chúng tôi xin thông báo rằng đơn hàng <strong>#DH{order.MaHoaDon.ToString("D6")}</strong> của bạn đã bị hủy.</p>
                        
                        <div style='background-color: #f8f9fa; padding: 20px; border-radius: 5px; margin: 20px 0;'>
                            <h3 style='margin-top: 0; color: #495057;'>Thông tin đơn hàng:</h3>
                            <p><strong>Mã đơn hàng:</strong> #DH{order.MaHoaDon.ToString("D6")}</p>
                            <p><strong>Ngày đặt hàng:</strong> {order.NgayDatHang.ToString("dd/MM/yyyy HH:mm")}</p>
                            <p><strong>Ngày hủy:</strong> {order.NgayHuy?.ToString("dd/MM/yyyy HH:mm")}</p>
                            <p><strong>Tổng tiền:</strong> {order.TongTien:N0} VNĐ</p>
                            <p><strong>Lý do hủy:</strong> {order.LyDoHuy}</p>
                        </div>
                        
                        <p>Nếu bạn đã thanh toán cho đơn hàng này, chúng tôi sẽ tiến hành hoàn tiền trong vòng 3-5 ngày làm việc.</p>
                        
                        <p>Nếu có bất kỳ thắc mắc nào, xin vui lòng liên hệ với chúng tôi qua:</p>
                        <ul>
                            <li>Email: support@cafeshop.com</li>
                            <li>Hotline: 1900 1234</li>
                        </ul>
                        
                        <p>Chúng tôi xin lỗi vì sự bất tiện này và hy vọng được phục vụ bạn trong những lần mua hàng tiếp theo.</p>
                        
                        <p style='margin-top: 30px;'>Trân trọng,<br><strong>Đội ngũ CaFe Shop</strong></p>
                    </div>
                    
                    <div style='background-color: #6c757d; color: white; text-align: center; padding: 15px;'>
                        <p style='margin: 0; font-size: 12px;'>© 2024 CaFe Shop. Tất cả quyền được bảo lưu.</p>
                    </div>
                </div>";

                message.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();
                await client.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(smtpUsername, smtpPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                Console.WriteLine($"Email sent successfully to {order.Email}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
                throw; // Re-throw để controller có thể xử lý
            }
        }
    }
}