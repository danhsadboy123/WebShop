using System;
using System.Collections.Generic;
using System.Linq;

using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using MailKit.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MimeKit.Text;
using MimeKit;
using WebShop.Extension;
using WebShop.Helpper;
using WebShop.Models;
using WebShop.ModelViews;
using System.Net.Mail;
using MailKit.Net.Smtp;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebShop.Controllers
{
    [Authorize]
    public class AccountsController : BaseController
    {
        private readonly DbMarketsContext _context;
        public INotyfService _notyfService { get; }
        public AccountsController(DbMarketsContext context, INotyfService notyfService): base(context)
        {
            _context = context;
            _notyfService = notyfService;
        }

        public static bool IsValid(string email)
        {
            var valid = true;

            try
            {
                var emailAddress = new MailAddress(email);
            }
            catch
            {
                valid = false;
            }

            return valid;
        }
        [HttpGet]
        [AllowAnonymous]
        public bool ValidateEmailOne(string email)
        {            
            if (IsValid(email))
            {
                var customer = _context.Customers.FirstOrDefault(x => x.Email.Contains(email.ToLower()));
                if (customer == null)
                {
                    return true;
                }

            }
            return false;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ValidatePhone(string SoDienThoai)
        {
            try
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.SoDienThoai.ToLower() == SoDienThoai.ToLower());
                if (khachhang != null)
                    return Json(data: "Số điện thoại : " + SoDienThoai + "đã được sử dụng");
                return Json(data: true);                
            }
            catch
            {
                return Json(data: true);
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ValidateEmail(string Email)
        {
            try
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.Email.ToLower() == Email.ToLower());
                if (khachhang != null)
                    return Json(data: "Email : " + Email + " đã được sử dụng");
                return Json(data: true);
            }
            catch
            {
                return Json(data: true);
            }
        }
        

        //thông tin cá nhân
        [Route("/Account", Name = "AccountInfo")]
        public IActionResult AccountInfo()
        {
            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            ChangeInfoViewModel model = new ChangeInfoViewModel();
            if (taikhoanID != null)
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CustomerId == Convert.ToInt32(taikhoanID));
                if(khachhang!= null)
                {
                    model.CustomerId = khachhang.CustomerId;
                    model.FullName = khachhang.FullName;
                    model.Email = khachhang.Email;
                    model.SoDienThoai = khachhang.SoDienThoai;
                    if (khachhang.Birthday != null)
                    {
                        model.Birthday = (DateTime)khachhang.Birthday;
                    }
                    if (khachhang.Gender != null)
                    {
                        model.Gender = (bool)khachhang.Gender;
                    }
                    
                    var lsDonHang = _context.Orders
                        .Include(x => x.DeliveryStatus)
                        .AsNoTracking()
                        .Where(x => x.CustomerId == khachhang.CustomerId)
                        .Include(x=> x.DeliveryStatus)
                        .OrderByDescending(x => x.OrderDate)
                        .ToList();
                    ViewBag.DonHang = lsDonHang;
                }             
                return View(model);
            }
            return RedirectToAction("Login");
        }
        public IActionResult ChangeAccountInfo(ChangeInfoViewModel changeInfoViewModel)
        {
            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            if (taikhoanID != null)
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CustomerId == Convert.ToInt32(taikhoanID));
                if (khachhang != null)
                {
                    khachhang.CustomerId = changeInfoViewModel.CustomerId;
                    khachhang.FullName = changeInfoViewModel.FullName;
                    khachhang.Email = changeInfoViewModel.Email;
                    khachhang.SoDienThoai = changeInfoViewModel.SoDienThoai;
                    if (khachhang.Birthday != null)
                    {
                        khachhang.Birthday = (DateTime)changeInfoViewModel.Birthday;
                    }
                    if (khachhang.Gender != null)
                    {
                        khachhang.Gender = (bool)changeInfoViewModel.Gender;
                    }
                }
                _context.Update(khachhang);
                _context.SaveChangesAsync();
                return View(changeInfoViewModel);
            }
            return RedirectToAction("Login");
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("/Account/Register",Name ="DangKy")]
        public IActionResult Register()
        {
            return View();
        }
        /// dang ky ok gui mail
        [HttpPost]
        [AllowAnonymous]
        [Route("/Account/Register",Name ="DangKy")]
        //public async Task<IActionResult> Register(RegisterViewModel taikhoan)
        //{
        //    var text = "";
        //    if (ValidateEmailOne(taikhoan.Email)==false)
        //    {
        //        text = "Tài khoản đã tồn tại vui lòng nhập email khác để đăng ký";
        //    }
        //    try
        //    {               
        //        if (ModelState.IsValid && ValidateEmailOne(taikhoan.Email))
        //        {
        //            text = "";
        //            string salt = Utilities.GetRandomKey();                    
        //            Customer khachhang = new Customer
        //            {
        //                FullName = taikhoan.FullName,
        //                SoDienThoai = taikhoan.SoDienThoai.Trim().ToLower(),
        //                Email = taikhoan.Email.Trim().ToLower(),
        //                Password = (taikhoan.Password + salt.Trim()).ToMD5(),
        //                IsActivated = true,
        //                Salt = salt,
        //                CreatedAt = DateTime.Now
        //            };
        //            try
        //            {
        //                _context.Add(khachhang);
        //                await _context.SaveChangesAsync();
        //                //Lưu Session MaKh
        //                HttpContext.Session.SetString("CustomerId", khachhang.CustomerId.ToString());
        //                var taikhoanID = HttpContext.Session.GetString("CustomerId");

        //                //Identity
        //                var claims = new List<Claim>
        //                {
        //                    new Claim(ClaimTypes.Name,khachhang.FullName),
        //                    new Claim("CustomerId", khachhang.CustomerId.ToString())
        //                };
        //                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
        //                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        //                await HttpContext.SignInAsync(claimsPrincipal);
        //                // gui email xin chao khach hang
        //                SendEmail(khachhang);
        //                _notyfService.Success("Đăng ký thành công");
        //                return RedirectToAction("Index", "Home");
        //            }
        //            catch 
        //            {

        //                return RedirectToAction("Register", "TaiKhoans");
        //            }
        //        }
        //        else
        //        {
        //            ViewBag.text = text;
        //            return View(taikhoan);
        //        }
               
             
        //    }
        //    catch 
        //    {
        //        ViewBag.text = "Sự cố không thể thêm tài khoản này";
        //        return View(taikhoan);
        //    }
        //}

        //public IActionResult SendEmail(Customer tk)
        //{
        //    var systemW = _context.SystemWebs.FirstOrDefault();
        //    var Admin = _context.PageInfos.FirstOrDefault();
        //    var email = new MimeMessage();
        //    try
        //    {
        //        if (systemW.PassSmtp == "" || systemW.Name == null || systemW.Post == null || systemW.Name == null)
        //        {
        //            return Json(new { succses = "No", value = "Vui lòng kiểm tra Quản lý website > Hệ thống." });
        //        }
        //        else
        //        {
        //            try
        //            {
        //                email.From.Add(MailboxAddress.Parse(systemW.EmailSend));
        //                using var smtp = new MailKit.Net.Smtp.SmtpClient();
        //                smtp.Connect(systemW.Server, (int)systemW.Post, SecureSocketOptions.StartTls);
        //                smtp.Authenticate(systemW.EmailSmtp, systemW.PassSmtp);
        //                try
        //                {
        //                    var optionEmail = _context.EmailMakettings.Where(i => i.EmailEvent == 3).FirstOrDefault();
        //                    var text = textcover(optionEmail.Body, tk.FullName, tk.Email, tk.SoDienThoai.ToString(),"", "");
        //                    email.Bcc.Add(MailboxAddress.Parse(tk.Email));
        //                    email.To.Add(MailboxAddress.Parse(tk.Email));
        //                    email.Subject = optionEmail.Title;
        //                    email.Body = new TextPart(TextFormat.Html) { Text = text };

        //                    smtp.Send(email);
        //                    smtp.Disconnect(true);
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine(ex.ToString());
        //                    return (IActionResult)ex;
        //                }


        //            }
        //            catch (Exception e)
        //            {
        //                return Json(new { succses = "No" });
        //            }

        //        }
        //    }
        //    catch(Exception e)
        //    {

        //    }  
        //    return Json(new { succses = "Ok" });
        //}

        public string textcover(string body, string Name, string Email, string SoDienThoai, string Address, string CompannyName)
        {
            var text = "";

            if (body != "<p><br></p>" || body != null)
            {
                var Str = body;
                Str = Str.Replace("HovaTenKH", Name);
                Str = Str.Replace("TenCongTyKH", CompannyName);
                Str = Str.Replace("EmailKH", Email);
                Str = Str.Replace("DiaChiKH", Address);
                Str = Str.Replace("SDTKH", SoDienThoai);
                text = Str;
            }

            return text;
        }

        [AllowAnonymous]
        [Route("/Account/Login", Name ="DangNhap")]
        public IActionResult Login(string returnUrl = null)
        {
            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            if (taikhoanID != null)
            {
                return RedirectToAction("AccountInfo", "TaiKhoans");   
            }
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("/Account/Login", Name ="DangNhap")]
        public async Task<IActionResult> Login(LoginViewModel customer, string returnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isEmail = Utilities.IsValidEmail(customer.UserName);
                    if (!isEmail) return View(customer);

                    var khachhang = _context.Customers.AsNoTracking()
                                                      .SingleOrDefault(x => x.Email.Trim() == customer.UserName);

                    if (khachhang == null)
                    {
                        _notyfService.Warning("Thông tin đăng nhập chưa chính xác");
                        return View(customer);
                    }
                    string pass = (customer.Password + khachhang.Salt.Trim()).ToMD5();
                    if(khachhang.Password != pass)
                    {
                        _notyfService.Warning("Thông tin đăng nhập chưa chính xác");
                        return View(customer);
                    }
                    //kiem tra xem account co bi disable hay khong

                    if (khachhang.IsActivated == false)
                    {
                        return RedirectToAction("ThongBao", "TaiKhoans");
                    }

                    //Luu Session MaKh
                    HttpContext.Session.SetString("CustomerId", khachhang.CustomerId.ToString());
                    var taikhoanID = HttpContext.Session.GetString("CustomerId");

                    //Identity
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, khachhang.FullName),
                        new Claim("CustomerId", khachhang.CustomerId.ToString())
                    };
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(claimsPrincipal);
                    _notyfService.Success("Đăng nhập thành công");
                    if (string.IsNullOrEmpty(returnUrl))
                    {
                        return RedirectToAction("AccountInfo", "TaiKhoans");
                    }
                    else
                    {
                        return Redirect(returnUrl);
                    }
                }
            }
            catch
            {
                return View(customer);

            }
            return View(customer);
        }
        [HttpGet]
        [Route("dang-xuat.html",Name ="DangXuat")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            HttpContext.Session.Remove("CustomerId");
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            try
            {
                var taikhoanID = HttpContext.Session.GetString("CustomerId");
                if (taikhoanID == null)
                {
                    return RedirectToAction("Login", "TaiKhoans");
                }
                if (ModelState.IsValid)
                {
                    var taikhoan = _context.Customers.Find(Convert.ToInt32(taikhoanID));
                    if (taikhoan == null) return RedirectToAction("Login", "TaiKhoans");
                    var pass = (model.PasswordNow.Trim() + taikhoan.Salt.Trim()).ToMD5();
                    {
                        string passnew = (model.Password.Trim() + taikhoan.Salt.Trim()).ToMD5();
                        taikhoan.Password = passnew;
                        _context.Update(taikhoan);
                        _context.SaveChanges();
                        _notyfService.Success("Đổi mật khẩu thành công");
                        return RedirectToAction("Dashboard", "TaiKhoans");
                    }
                }
            }
            catch 
            {
                _notyfService.Success("Thay đổi mật khẩu không thành công");
                return RedirectToAction("Dashboard", "TaiKhoans");
            }
            _notyfService.Success("Thay đổi mật khẩu không thành công");
            return RedirectToAction("Dashboard", "TaiKhoans");
        }
    }
}
