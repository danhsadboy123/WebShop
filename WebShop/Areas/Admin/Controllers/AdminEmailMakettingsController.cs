using System;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MimeKit.Text;
using MimeKit;
using PagedList.Core;
using WebShop.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using System.Net.WebSockets;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace WebShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminEmailMakettingsController : Controller
    {
        private readonly DbMarketsContext _context;
        public INotyfService _notyfService { get; }
        public AdminEmailMakettingsController(DbMarketsContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminEmailMakettings
        public async Task<IActionResult> Index()
        {
            var dbMarketsContext = _context.EmailMakettings.Include(e => e.Acount);

            return View(await dbMarketsContext.ToListAsync());
        }

        // GET: Admin/AdminEmailMakettings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EmailMakettings == null)
            {
                return NotFound();
            }

            var emailMaketting = await _context.EmailMakettings
                .Include(e => e.Acount)
                .FirstOrDefaultAsync(m => m.EmailId == id);
            if (emailMaketting == null)
            {
                return NotFound();
            }

            return View(emailMaketting);
        }

        // GET: Admin/AdminEmailMakettings/Create
        public IActionResult Create(string url)
        {
            var taikhoanID = HttpContext.Session.GetString("AccountId");
            ViewBag.Account = _context.Accounts.Where(c => c.AccountId == int.Parse(taikhoanID)).FirstOrDefault();
            ViewBag.ImageServer = _context.ImageServers.ToList();
            ViewBag.Url = url;
            return View();
        }

        // POST: Admin/AdminEmailMakettings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmailId,AcountId,Title,Body,CreateDate,CustomDate")] EmailMaketting emailMaketting, string url)
        {

            if (ModelState.IsValid)
            {
                _context.Add(emailMaketting);
                await _context.SaveChangesAsync();
                if (url != null)
                {

                    return RedirectToAction(url);
                }
            }
            ViewData["AcountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", emailMaketting.AcountId);
            return View(emailMaketting);
        }
        // nguyen code email

        public async Task<IActionResult> CustumerSendEmail(int? page, string? search)
        {
            int pageNumber = page ?? 1; // Trang hiện tại
            var pageSize = 20;
            var lsCustomers = _context.Customers
                .AsNoTracking()
                .OrderByDescending(x => x.CreateDate);
            var customer = from a in lsCustomers select (a);
            if (search != null)
            {
                search = search.ToLower();
                customer = customer.Where(c => c.FullName.Contains(search));
            }

            PagedList<Customer> models = new PagedList<Customer>(customer, pageNumber, pageSize);
            ViewBag.Search = search;
            ViewBag.CurrentPage = pageNumber;

            // danh sach mail
            ViewBag.Email = _context.EmailMakettings.Where(e=>e.EmailEvent==null).ToList();

            return View(models);
        }


        [HttpGet]
        public IActionResult SendEmail(int userId, string OptionEmailID)
        {
            var address = "";
            var systemW = _context.SystemWebs.FirstOrDefault();
            var email = new MimeMessage();
            if (systemW.PassSmtp == "" || systemW.Name == null || systemW.Post == null || systemW.Name == null)
            {
                return Json(new { succses = "No", value = "vui lòng kiểm tra Quản lý website > Hệ thống." });
            }
            else
            {
                try
                {
                    email.From.Add(MailboxAddress.Parse(systemW.EmailSend));
                    using var smtp = new SmtpClient();
                    smtp.Connect(systemW.Server, (int)systemW.Post, SecureSocketOptions.StartTls);
                    smtp.Authenticate(systemW.EmailSmtp, systemW.PassSmtp);

                    if (userId != null && OptionEmailID != null)
                    {
                        try
                        {
                            var customer = _context.Customers.Find(userId);
                            var addressid = _context.AccountAddresses.Where(c => c.CustomerId == userId).Where(c => c.IsDefault == true).Include(c => c.Ward).Include(c => c.District).Include(c => c.Province).FirstOrDefault();
                            if (addressid != null)
                            {
                                address = addressid.Content + "/" + addressid.Ward.WardName + "/" + addressid.District.DistrictName + "/" + addressid.Province.ProvinceName;
                            }
                            var optionEmail = _context.EmailMakettings.Find(int.Parse(OptionEmailID));
                            var text = textcover(optionEmail.Body, customer.FullName, customer.Email, customer.Phone.ToString(), address, "");

                            email.Bcc.Add(MailboxAddress.Parse(customer.Email));
                            email.To.Add(MailboxAddress.Parse(customer.Email));
                            email.Subject = systemW.Name + " " + optionEmail.Title;
                            email.Body = new TextPart(TextFormat.Html) { Text = text };

                            smtp.Send(email);
                            smtp.Disconnect(true);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                            return (IActionResult)ex;
                        }

                    }
                }
                catch (Exception e)
                {

                }

            }

            return Json(new { succses = "Ok" });
        }


        [HttpGet]
        public IActionResult SendAllEmail(string OptionEmailID, string? search)
        {
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            var systemW = _context.SystemWebs.FirstOrDefault();
            var text = "";
            var email = new MimeMessage();

            var customer = _context.Customers
                .Include(c => c.AccountAddresses)
                    .ThenInclude(a => a.Province)
                .Include(c => c.AccountAddresses)
                    .ThenInclude(a => a.District)
                .Include(c => c.AccountAddresses)
                    .ThenInclude(a => a.Ward)
                .ToList();
            var optionEmail = _context.EmailMakettings.First(c => c.EmailId == int.Parse(OptionEmailID));
            if (systemW.PassSmtp == "" || systemW.Name == null || systemW.Post == null || systemW.Name == null)
            {
                return Json(new { succses = "No", value = "vui lòng kiểm tra Quản lý website > Hệ thống." });
            }
            else
            {
                try
                {
                    email.From.Add(MailboxAddress.Parse(systemW.EmailSend));
                    using var smtp = new SmtpClient();
                    smtp.Connect(systemW.Server, (int)systemW.Post, SecureSocketOptions.StartTls);
                    smtp.Authenticate(systemW.EmailSmtp, systemW.PassSmtp);

                    if (optionEmail.EmailId != null)
                    {
                        var body = optionEmail.Body;
                        var HovaTenKH = "";
                        var TenCongTy = "";
                        var Email = "";
                        var DiaChiKH = "";
                        var SDTKH = "";

                        if (body.Contains("HovaTenKH"))
                        {
                            HovaTenKH = "1";
                        }
                        if (body.Contains("DiaChiKH"))
                        {
                            DiaChiKH = "1";
                        }
                        if (body.Contains("SDTKH"))
                        {
                            SDTKH = "1";
                        }

                        email.Subject = systemW.Name + " " + optionEmail.Title;
                        foreach (var item in customer)
                        {
                            /// kiem tra ton tai chuoi
                            if (HovaTenKH == "1")
                            {
                                HovaTenKH = item.FullName;
                            }
                            if (DiaChiKH == "1")
                            {
                                if (item.AccountAddresses.Count() > 0 && item.AccountAddresses != null)
                                {
                                    var address = item.AccountAddresses.Where(a => a.IsDefault == true).FirstOrDefault();
                                    try
                                    {
                                        DiaChiKH = address.Content + "/" + address.Ward.WardName + "/" + address.District.DistrictName + "/" + address.Province.ProvinceName;

                                    }
                                    catch (Exception e)
                                    {

                                    }
                                }


                            }
                            if (SDTKH == "1")
                            {
                                SDTKH = item.Phone;
                            }
                            text = textcover(optionEmail.Body, HovaTenKH, item.Email, SDTKH.ToString(), DiaChiKH, "");
                            if (item.Email != null)
                            {
                                try
                                {
                                    email.Bcc.Add(MailboxAddress.Parse(item.Email));
                                    email.To.Add(MailboxAddress.Parse(item.Email));
                                    email.Body = new TextPart(TextFormat.Html)
                                    {
                                        Text = text
                                    };
                                    try
                                    {
                                        smtp.Send(email);

                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.ToString());
                                        return Json(new { succses = "No", value = "gui that bai" });
                                    }
                                }
                                catch (Exception ex)
                                {

                                }

                            }

                        }


                    }
                }
                catch (Exception e)
                {
                    return Json(new { succses = "No", value = "gui that bai" });
                }

            }
            // chua bao gom search va dieu khien search
            return Json(new { succses = "Ok", value = "gui thanh cong" });
        }


        public IActionResult ListEmailCustomer(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var listEmail = _context.EmailAttributes
                .Include(x => x.Custumer)
                .Where(x => x.CustumerId == id);

            var emailAttr = from Email in listEmail select (Email);
            emailAttr = emailAttr.OrderByDescending(x => x.TimeSend);

            if (emailAttr == null)
            {
                return NotFound();
            }
            return PartialView("ListEmailCustomer", emailAttr.ToList());

        }
        public IActionResult ListEmailCusP(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var listEmail = _context.AttributesPotentails
                .Where(x => x.PotentailId == id);

            var emailAttr = from Email in listEmail select (Email);
            emailAttr = emailAttr.OrderByDescending(x => x.TimeSend);

            if (emailAttr == null)
            {
                return NotFound();
            }
            return PartialView("ListEmailCusP", emailAttr.ToList());

        }
        [HttpPost]
        public IActionResult updatesection(int option, int checksave)
        {
            HttpContext.Session.SetString("option", option.ToString());
            HttpContext.Session.SetString("checksave", checksave.ToString());
            return Ok();
        }
        [HttpPost]
        public IActionResult updatesection1(int leverID)
        {

            HttpContext.Session.SetString("leverID", leverID.ToString());
            return Json(new { success = "OK" });
        }
        // email potentail
        public string textcover(string body, string Name, string Email, string Phone, string Address, string CompannyName)
        {
            var text = "";

            if (body != "<p><br></p>" || body != null)
            {
                var Str = body;
                Str = Str.Replace("HovaTenKH", Name);
                Str = Str.Replace("TenCongTyKH", CompannyName);
                Str = Str.Replace("EmailKH", Email);
                Str = Str.Replace("DiaChiKH", Address);
                Str = Str.Replace("SDTKH", Phone);
                text = Str;
            }

            return text;
        }
        public async Task<IActionResult> CustumerPotentailSendEmail()
        {
            var lever = HttpContext.Session.GetString("leverID");

            var leveruser = 0;
            if (lever != null)
            {
                leveruser = int.Parse(lever);
            }
            int pageNumber = 1; // Trang hiện tại
            var pageSize = 20;
            var acc = _context.CustomerPotentails.Include(x => x.Lever);
            var lsCustomers = _context.CustomerPotentails.Include(x => x.Lever)
                .AsNoTracking()
                .OrderBy(x => x.Name);
            var customer = from a in lsCustomers select (a);

            if (leveruser != 0)
            {
                customer = customer.Where(x => x.LeverId == leveruser);

            }
            PagedList<CustomerPotentail> models = new PagedList<CustomerPotentail>(customer, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.admin = HttpContext.Session.GetString("AccountId");
            // danh sach mail
            ViewBag.itemEmail = HttpContext.Session.GetString("option");
            ViewBag.checksave = HttpContext.Session.GetString("checksave");
            ViewBag.checklever = HttpContext.Session.GetString("leverID");
            //
            ViewBag.Email = _context.EmailMakettings.Where(e=>e.EmailEvent ==null).ToList();
            ViewBag.checknull = _context.CustomerPotentails.Where(x => x.Checked == 0).Count();
            ViewBag.lever = _context.LeverCustommerPtts.ToList();
            return View(models);
        }

        [HttpGet]
        public IActionResult ViewEmail(int option)
        {
            var value = _context.EmailMakettings.Where(x => x.EmailId == option).FirstOrDefault();
            if (value == null)
            {
                return NotFound();
            }
            return Json(new { success = "Ok", value = value });
        }
        [HttpGet]
        public IActionResult SendAllEmailCuP(string OptionEmailID, int leveruser)
        {
            var systemW = _context.SystemWebs.FirstOrDefault();
            var email = new MimeMessage();
            var optionEmail = _context.EmailMakettings.Find(int.Parse(OptionEmailID));
            var cus = _context.CustomerPotentails.Where(x => x.Checked == 1).ToList();
            var customer = from a in cus select (a);
            if (leveruser != 0)
            {
                customer = customer.Where(x => x.LeverId == leveruser).ToList();
            }

            if (customer.Count() == 0)
            {
                return Json(new { succses = "Null", value = "khong tim thay nguoi gui" });
            }
            if (systemW.PassSmtp == "" || systemW.Name == null || systemW.Post == null || systemW.Name == null)
            {
                return Json(new { succses = "No", value = "vui lòng kiểm tra Quản lý website > Hệ thống." });
            }
            else
            {
                try
                {
                    email.From.Add(MailboxAddress.Parse(systemW.EmailSend));
                    using var smtp = new SmtpClient();
                    smtp.Connect(systemW.Server, (int)systemW.Post, SecureSocketOptions.StartTls);
                    smtp.Authenticate(systemW.EmailSmtp, systemW.PassSmtp);
                    if (optionEmail.EmailId != null)
                    {
                        email.Subject = systemW.Name + " " + optionEmail.Title;
                        foreach (var item in customer)
                        {
                            var text = textcover(optionEmail.Body, item.Name, item.Email, item.Phone.ToString(), item.Address, item.CompannyName);
                            email.Bcc.Add(MailboxAddress.Parse(item.Email.ToString()));
                            email.To.Add(MailboxAddress.Parse(item.Email.ToString()));
                            email.Body = new TextPart(TextFormat.Html)
                            {
                                Text = text
                            };
                            try
                            {
                                smtp.Send(email);

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.ToString());
                                return Json(new { succses = "No", value = "gui that bai" });
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    return Json(new { succses = "No", value = "gui that bai" });
                }

            }
            // chua bao gom search va dieu khien search
            return Json(new { succses = "Ok", value = "gui thanh cong" });
        }

        /// khach hang he thong code


        // GET: Admin/AdminEmailMakettings/Edit/5

        public async Task<IActionResult> Edit(int? id, string url)
        {
            if (id == null || _context.EmailMakettings == null)
            {
                return NotFound();
            }

            var emailMaketting = await _context.EmailMakettings.FindAsync(id);
            if (emailMaketting == null)
            {
                return NotFound();
            }
            var taikhoanID = HttpContext.Session.GetString("AccountId");
            ViewBag.ImageServer = _context.ImageServers.ToList();
            ViewBag.Url = url;
            ViewBag.Account = _context.Accounts.Where(c => c.AccountId == int.Parse(taikhoanID)).FirstOrDefault();
            return View(emailMaketting);
        }

        // POST: Admin/AdminEmailMakettings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string url, [Bind("EmailId,AcountId,Title,Body,CreateDate,CustomDate")] EmailMaketting emailMaketting)
        {
            if (id != emailMaketting.EmailId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(emailMaketting);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmailMakettingExists(emailMaketting.EmailId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(url);
            }
            ViewData["AcountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", emailMaketting.AcountId);
            return View(emailMaketting);
        }

        // GET: Admin/AdminEmailMakettings/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null || _context.EmailMakettings == null)
            {
                return Json(new { success = "No" });
            }

            var emailMaketting = _context.EmailMakettings
                .Include(e => e.Acount)
                .FirstOrDefault(m => m.EmailId == id);

            if (emailMaketting != null)
            {
                try
                {
                    _context.EmailMakettings.Remove(emailMaketting);
                    _context.SaveChanges();
                }
                catch (Exception e)
                {
                    return Json(new { success = "No" });
                }
            }

            return Json(new { success = "Ok" });
        }


        private bool EmailMakettingExists(int id)
        {
            return _context.EmailMakettings.Any(e => e.EmailId == id);
        }
    }
}
