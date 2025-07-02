using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MimeKit.Text;
using MimeKit;
using PagedList.Core;
using WebShop.Areas.Admin.Models;
using WebShop.Extension;
using WebShop.Models;
using WebShop.ModelViews;
using System.Runtime.CompilerServices;
using WebShop.Helpper;

namespace WebShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminOrdersController : Controller
    {
        private readonly DbMarketsContext _context;
        public INotyfService _notyfService { get; }
        public AdminOrdersController(DbMarketsContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminOrders

        public IActionResult Index(int TransactStatusID = 0)
        {
            var orderVM = new AdminOrderVM();
            List<Order> lsOrders = new List<Order>();
            if (TransactStatusID != 0)
            {
                lsOrders = _context.Orders
                .AsNoTracking()
                .Where(x => x.DeliveryStatusId == TransactStatusID)
                .Include(o => o.Customer)
                .Include(o => o.Guest)
                .Include(o => o.DeliveryStatus)
                .Include(o => o.Codstatus)
                .Include(o => o.PaymentStatus)
                .Where(o=>o.Draft == false)
                .OrderByDescending(x => x.OrderId)
                .ToList();
            }
            else
            {
                lsOrders = _context.Orders
                .AsNoTracking()
                .Include(o => o.Customer)
                .Include(o => o.Guest)
                .Include(o => o.DeliveryStatus)
                .Include(o => o.Codstatus)
                .Include(o => o.PaymentStatus)
                .Where(o => o.Draft == false)
                .OrderByDescending(x => x.OrderId)
                .ToList();
            }

            var checkboxOrders = new CheckboxOrder
            {
                OrderId = HttpContext.Session.Get<bool?>("OrderId") ?? true,
                OrderDate = HttpContext.Session.Get<bool?>("OrderDate") ?? true,
                CustomerName = HttpContext.Session.Get<bool?>("CustomerName") ?? true,
                Email = HttpContext.Session.Get<bool?>("Email") ?? false,
                SoDienThoai = HttpContext.Session.Get<bool?>("SoDienThoai") ?? false,
                PaymentStatus = HttpContext.Session.Get<bool?>("PaymentStatus") ?? true,
                DeliverStatus = HttpContext.Session.Get<bool?>("DeliverStatus") ?? true,
                CodStatus = HttpContext.Session.Get<bool?>("CodStatus") ?? true,
                Total = HttpContext.Session.Get<bool?>("Total") ?? true,
            };

            ViewBag.CurrentStatusID = TransactStatusID;

            orderVM.CheckboxOrder = checkboxOrders;
            //
            var id = HttpContext.Session.GetString("Addsectionindex");
            if (id == "1")
            {
                 
            }
            if (id == "2")
            {
                lsOrders = lsOrders.Where(o => o.Confirmed == false).ToList();
            }
            if (id == "3")
            {
                lsOrders = lsOrders.Where(o => o.DeliveryStatusId == 2).ToList();
            }
            if (id == "4")
            {
                lsOrders = lsOrders.Where(o => o.PaymentStatusId !=2).ToList();
            }
            ViewBag.check = id;
            orderVM.Orders = lsOrders;
            ViewData["TrangThai"] = new SelectList(_context.DeliveryStatuses, "TransactStatusId", "Status");
            return View(orderVM);
        }
        public IActionResult Filtter(int TransactionID = 0)
        {
            var url = $"/Admin/AdminOrders?TransactStatusID={TransactionID}";
            if (TransactionID == 0)
            {
                url = $"/Admin/AdminOrders";
            }
            return Json(new { status = "success", redirectUrl = url });
        }
        // GET: Admin/AdminOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        { 
            if (id == null)
            {
                return NotFound();
            }
            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Guest)
                .Include(o => o.DeliveryStatus)
                .Include(o => o.Codstatus)
                .Include(o => o.PaymentStatus)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            var discount = _context.Discounts.Where(c=>c.Code ==order.Code).FirstOrDefault();
            if (order == null)
            {
                return NotFound();
            }
            var shippingAddress = _context.ShippingAddresses
                .Include(x => x.Province)
                .Include(x => x.District)
                .Include(x => x.Ward)
                .FirstOrDefault(o => o.OrderId == order.OrderId);

            var orderDetails = _context.OrderDetails
                .Include(x =>x.Product)
                .AsNoTracking()
                .Where(x => x.OrderId == order.OrderId)
                .OrderBy(x => x.OrderDetailId)
                .ToList();
            ViewBag.ChiTiet = orderDetails;
            ViewBag.DiaChi = shippingAddress;
            return View(order);
        }
        // gui email
        //public IActionResult methodSenmail(int id,int optonemail)
        //{
        //    var text = "";
        //    if (id == null)
        //    {
        //        return Json(new { success = "No" });
        //    }
        //    var order = _context.Orders
        //        .Include(o => o.Customer)
        //        .Include(o => o.Guest)
        //        .Include(o => o.DeliveryStatus)
        //        .Include(o => o.Codstatus)
        //        .Include(o => o.PaymentStatus)
        //        .FirstOrDefault(m => m.OrderId == id);
        //    if (order == null)
        //    {
        //        return Json(new { success = "No" });
        //    }
        //    var shippingAddress = _context.ShippingAddresses
        //        .Include(x => x.Province)
        //        .Include(x => x.District)
        //        .Include(x => x.Ward)
        //        .FirstOrDefault(o => o.OrderId == id);

        //    var orderDetails = _context.OrderDetails
        //        .Include(x => x.Product)
        //        .AsNoTracking()
        //        .Where(x => x.OrderId == id)
        //        .OrderBy(x => x.OrderDetailId)
        //        .ToList();
        //    try
        //    {
        //        if (order.CustomerId != null)
        //        {
        //            var customer = order.Customer;
        //            SendEmail1(customer, orderDetails,optonemail ,order.Code);
        //        }
        //        else
        //        {
        //            var Guest = order.Guest;
        //            var address = "";
        //            if(shippingAddress!=null)                    
        //            {
        //                address= shippingAddress.Address+", " + shippingAddress.Ward.WardName +", " + shippingAddress.District.DistrictName + ", " + shippingAddress.Province.ProvinceName;
        //            }
                    
        //            SendEmail(Guest, address, orderDetails, optonemail, order.Code);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return Json(new { success = "No" });
        //    }
           
        //    return Json(new { success = "Ok" });
        //}

        //public IActionResult SendEmail(Guest tk, string address, List<OrderDetail> listO,int EmailId,string code)
        //{
        //    var address1 = "";
        //    if (address != null)
        //    {
        //        address1 = address;
        //    }
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
        //                    var optionEmail = _context.EmailMakettings.Where(i => i.EmailId == EmailId).FirstOrDefault();
        //                    var text = textcover(optionEmail.Body, tk.FullName, tk.Email, tk.SoDienThoai.ToString(), address1, "", listO,code);
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
        //    catch (Exception e)
        //    {

        //    }


        //    return Json(new { succses = "Ok" });
        //}
        public string textcover(string body, string Name, string Email, string SoDienThoai, string Address, string CompanyName, List<OrderDetail> Order, string code)
        {
            var id = "";
            int totalOriginalPrice = 0;
            int totalDiscountedPrice = 0;
            int discountAmount = 0;

            // Khởi tạo bảng HTML sản phẩm
            var productHtml = @"
    <table style='width:100%; border-collapse:collapse;'>
        <thead>
            <tr style='border-bottom:1px solid #ddd; text-align:left;'>
                <th style='padding:8px; width:25%;'>Sản phẩm</th>
                <th style='padding:8px;'>Tên</th>
                <th style='padding:8px;'>Số lượng</th>
                <th style='padding:8px;'>Giá</th>
            </tr>
        </thead>
        <tbody>";

            if (Order != null)
            {
                foreach (var item in Order)
                {
                    if (id == "")
                    {
                        id = code;
                    }

                    int originalPrice = (int)item.Product.Price;
                    int discountedPrice = (int)item.Product.SalePrice;
                    int itemTotalPrice = (int)discountedPrice * (int)item.Amount;

                    // Tính tổng giá trị sản phẩm và giảm giá
                    totalOriginalPrice = totalOriginalPrice+ (int)originalPrice * (int)item.Amount;
                    totalDiscountedPrice += itemTotalPrice;

                    productHtml += $@"
            <tr style='border-bottom:1px solid #ddd;'>
                <td style='padding:8px; text-align:center;'><img src='{item.Product.Avatar}' alt='Product Image' width='100'></td>
                <td style='padding:8px;'>{item.Product.ProductName}</td>
                <td style='padding:8px; text-align:center;'>x{item.Amount}</td>
                <td style='padding:8px; text-align:right;'>{discountedPrice.ToString("#,##0")} VND<br><strike>{originalPrice.ToString("#,##0")} VND</strike></td>
            </tr>";
                }

                // Tính tổng giá trị và giảm giá
                discountAmount = totalOriginalPrice - totalDiscountedPrice;

                // Thêm phần tổng cộng vào bảng
                productHtml += $@"
        </tbody>
        <tfoot>
            <tr>
                <td colspan='3' style='padding:8px; text-align:right;'>Tổng giá trị sản phẩm:</td>
                <td style='padding:8px; text-align:right;'>{totalOriginalPrice.ToString("#,##0")} VND</td>
            </tr>
            <tr>
                <td colspan='3' style='padding:8px; text-align:right;'>Giảm giá:</td>
                <td style='padding:8px; text-align:right;'>{discountAmount.ToString("#,##0")} VND</td>
            </tr>
            <tr style='border-top:2px solid #000;'>
                <td colspan='3' style='padding:8px; text-align:right; font-weight:bold;'>Tổng tiền thanh toán:</td>
                <td style='padding:8px; text-align:right; font-weight:bold;'>{totalDiscountedPrice.ToString("#,##0")} VND</td>
            </tr>
        </tfoot>
    </table>";
            }

            // Thay thế các placeholder trong body bằng thông tin thực tế
            if (!string.IsNullOrEmpty(body) && body != "<p><br></p>")
            {
                body = body.Replace("HovaTenKH", Name)
                           .Replace("TenCongTyKH", CompanyName)
                           .Replace("EmailKH", Email)
                           .Replace("DiaChiKH", Address)
                           .Replace("SDTKH", SoDienThoai)
                           .Replace("MDH", code)
                           .Replace("PTTT", "Thanh toán tại công ty, Thanh toán khi nhận hàng, Thanh toán chuyển khoản ngân hàng")
                           .Replace("PTVC", "Giao hàng nhanh")
                           .Replace("SPKHDM", productHtml);
            }

            return body;
        }

        //public IActionResult SendEmail1(Customer tk, List<OrderDetail> listO, int EmailId, string code)
        //{
        //    var address = "";
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
        //                var addressid = _context.AccountAddresses.Where(c => c.CustomerId == tk.CustomerId).Where(c => c.IsDefault == true).Include(c => c.Ward).Include(c => c.District).Include(c => c.Province).FirstOrDefault();
        //                if (addressid != null)
        //                {
        //                    address = addressid.Content + "/" + addressid.Ward.WardName + "/" + addressid.District.DistrictName + "/" + addressid.Province.ProvinceName;
        //                }
        //                email.From.Add(MailboxAddress.Parse(systemW.EmailSend));
        //                using var smtp = new MailKit.Net.Smtp.SmtpClient();
        //                smtp.Connect(systemW.Server, (int)systemW.Post, SecureSocketOptions.StartTls);
        //                smtp.Authenticate(systemW.EmailSmtp, systemW.PassSmtp);
        //                try
        //                {
        //                    var optionEmail = _context.EmailMakettings.Where(i => i.EmailEvent == EmailId).FirstOrDefault();
        //                    var text = textcover(optionEmail.Body, tk.FullName, tk.Email, tk.SoDienThoai.ToString(), address, "", listO,code);
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
        //    catch (Exception e)
        //    {

        //    }


        //    return Json(new { succses = "Ok" });
        //}

        //
        [HttpPost]
        public IActionResult Addsectionindex(int id)
        {
            HttpContext.Session.SetString("Addsectionindex", id.ToString());
            return Json(new { success = "Ok" });
        }
        // updateComfim
        [HttpPost]
        public IActionResult updateComfim(int id)
        { 
            var text = "No";
            var order = _context.Orders.Where(o=>o.OrderId==id).FirstOrDefault();
            if (order != null)
            {
                try
                {
                    order.Confirmed = true;
                    _context.Update(order);
                    _context.SaveChanges();
                    text = "Ok";
                }catch(Exception e)
                {
                    text = "No";
                }
            }          
           
            return Json(new { success=text });
        }
        // updete delivery cho lay hang
        [HttpPost]
        public IActionResult updateDelivery(int id)
        {
            var text = "No";
            var order = _context.Orders.Where(o => o.OrderId == id).FirstOrDefault();
            if (order != null && order.DeliveryStatusId!=2)
            {
                try
                {
                    order.DeliveryStatusId = 2;
                    _context.Update(order);
                    _context.SaveChanges();
                    text = "Ok";
                }
                catch (Exception e)
                {
                    text = "No";
                }
            }
            else
            {
                text = "No2";
            }

            return Json(new { success = text });
        }

        // updatePayment cho lay hang
        [HttpPost]
        public IActionResult updatePayment(int id)
        {
            var text = "No";
            var order = _context.Orders.Where(o => o.OrderId == id).FirstOrDefault();
            if (order != null && order.PaymentStatusId != 2)
            {
                try
                {
                    order.PaymentStatusId = 2;
                    _context.Update(order);
                    _context.SaveChanges();
                    text = "Ok";
                }
                catch (Exception e)
                {
                    text = "No";
                }
            }
            else
            {
                text = "No2";
            }

            return Json(new { success = text });
        }


         


        [HttpPost]
        public IActionResult updateStatusAll(string[] listid)
        {
            int[] countitem = new int[listid.Length];
            if(listid.Length != 0)
            {
             
                int i = 0;
                foreach (var item in listid)
                {
                    var order = _context.Orders.Where(o => o.OrderId == int.Parse(item)).FirstOrDefault();
                    if (order != null)
                    {
                        try
                        {
                            order.Confirmed = true;
                            order.DeliveryStatusId = 2;
                            _context.Update(order);
                            _context.SaveChanges();
                           
                            countitem[i]= int.Parse(item);
                            i++;
                        }
                        catch(Exception e) { }
                       
                    }
                }

            }
            
            return Json(new {success="Ok", data= countitem });
        }


        [HttpPost]
        public async Task<IActionResult> Details(int id, [Bind("OrderId,CustomerId,OrderDate,ShipDate,TransactStatusId,Deleted,Paid,PaymentDate,TotalMoney,PaymentId,Note,Address,LocationId,District,Ward")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var donhang = await _context.Orders.AsNoTracking().Include(x => x.Customer).FirstOrDefaultAsync(x => x.OrderId == id);
                    if (donhang != null)
                    {
                        donhang.PaymentStatusId = order.PaymentStatusId;
                        donhang.Deleted = order.Deleted;
                        donhang.DeliveryStatusId = order.DeliveryStatusId;
                        if (donhang.PaymentStatusId == 2)
                        {
                            donhang.PaymentDate = DateTime.Now;
                        }
                        if (donhang.DeliveryStatusId == 5) donhang.Deleted = true;
                        if (donhang.DeliveryStatusId == 3) donhang.ShipDate = DateTime.Now;
                    }
                    _context.Update(donhang);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Cập nhật trạng thái đơn hàng thành công");
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeliveryStatus"] = new SelectList(_context.DeliveryStatuses, "DeliveryStatusId", "Status", order.DeliveryStatusId);
            return View(order);
        }

        // GET: Admin/AdminOrders/Create
        public IActionResult Create()
        {
            var customers = _context.Customers.ToList();
            ViewBag.Customers = customers;
            return View();
        }

        // POST: Admin/AdminOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public IActionResult CreateOrder(string note, string type)
        {
            //Lay ra gio hang de xu ly
            var donhang = HttpContext.Session.Get<List<CartItem>>("Order");
            try
            {
                var taikhoanID = HttpContext.Session.GetString("CustomerOD");
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CustomerId == Convert.ToInt32(taikhoanID));
                var address = _context.AccountAddresses.AsNoTracking().SingleOrDefault(x => x.CustomerId == Convert.ToInt32(taikhoanID) && x.IsDefault == true);
                Order order = new Order();

                //Khoi tao don hang
                if (taikhoanID != null)
                {
                    order.CustomerId = khachhang.CustomerId;
                }
                else
                {
                    order.CustomerId = null;
                }
                order.GuestId = null;
                if(type != "draft")
                {
                    order.Draft = false;
                }
                else
                {
                    order.Draft = true;

                }
                order.OrderDate = DateTime.Now;
                if(address != null)
                {
                    order.SoDienThoai = address.SoDienThoai;
                }
                order.DeliveryStatusId = 1;//Don hang moi
                order.Confirmed = false;
                order.PaymentStatusId = 1;
                order.CodstatusId = 1;
                order.Deleted = false;
                order.PaymentStatusId = 1;
                order.Note = note;
                order.TotalMoney = Convert.ToInt32(donhang.Sum(x => x.TotalMoney));
                _context.Orders.Add(order);
                _context.SaveChanges();

                //tao dia chi giao hang
                if(address != null)
                {
                    ShippingAddress shippingAdress = new ShippingAddress();
                    shippingAdress.OrderId = order.OrderId;
                    shippingAdress.Name = address.UserName;
                    shippingAdress.SoDienThoai = address.SoDienThoai;
                    shippingAdress.Address = address.Content;
                    shippingAdress.ProvinceId = address.ProvinceId;
                    shippingAdress.DistrictId = address.DistrictId;
                    shippingAdress.WardId = address.WardId;
                    _context.ShippingAddresses.Add(shippingAdress);
                    _context.SaveChanges();
                }
                //tao chi tiet đơn hàng (Order Detail)
                foreach (var item in donhang)
                {
                    OrderDetail orderDetail = new OrderDetail();
                    orderDetail.OrderId = order.OrderId;
                    orderDetail.ProductId = item.product.ProductId;
                    orderDetail.Amount = item.amount;
                    orderDetail.TotalMoney = order.TotalMoney;
                    orderDetail.Price = item.product.SalePrice;
                    orderDetail.CreatedAt = DateTime.Now;
                    _context.OrderDetails.Add(orderDetail);
                }
                _context.SaveChanges();
                //clear gio hang
                HttpContext.Session.Remove("Order");
                HttpContext.Session.Remove("CustomerOD");
                //Xuat thong bao
                _notyfService.Success("Tạo đơn hàng thành công");
                //cap nhat thong tin khach hang
                return Json(new { success=true, orderId = order.OrderId });
            }
            catch (Exception)
            {
                return Json(new { success=false, message = "Lỗi khi tạo đơn hàng" });
            }
        }
        
        //[HttpPost]
        //public IActionResult CreateDraftOrder(string note)
        //{
        //    //Lay ra gio hang de xu ly
        //    var donhang = HttpContext.Session.Get<List<CartItem>>("Order");
        //    var customers = _context.Customers.ToList();

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var taikhoanID = HttpContext.Session.GetString("CustomerOD");
        //            var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CustomerId == Convert.ToInt32(taikhoanID));
        //            var address = _context.AccountAddresses.AsNoTracking().SingleOrDefault(x => x.CustomerId == Convert.ToInt32(taikhoanID) && x.IsDefault == true);
        //            Order order = new Order();

        //            //Khoi tao don hang
        //            if (taikhoanID != null)
        //            {
        //                order.CustomerId = khachhang.CustomerId;
        //            }
        //            else
        //            {
        //                order.CustomerId = null;
        //            }
        //            order.GuestId = null;
        //            order.Draft = true;
        //            order.OrderDate = DateTime.Now;
        //            order.SoDienThoai = address.SoDienThoai;
        //            order.DeliveryStatusId = 1;//Don hang moi
        //            order.Confirmed = false;
        //            order.PaymentStatusId = 1;
        //            order.CodstatusId = 1;
        //            order.Deleted = false;
        //            order.PaymentStatusId = 1;
        //            order.Note = note;
        //            order.TotalMoney = Convert.ToInt32(donhang.Sum(x => x.TotalMoney));
        //            _context.Orders.Add(order);
        //            _context.SaveChanges();

        //            //tao dia chi giao hang
        //            ShippingAddress shippingAdress = new ShippingAddress();
        //            shippingAdress.OrderId = order.OrderId;
        //            shippingAdress.Name = address.UserName;
        //            shippingAdress.SoDienThoai = address.SoDienThoai;
        //            shippingAdress.Address = address.Content;
        //            shippingAdress.ProvinceId = address.ProvinceId;
        //            shippingAdress.DistrictId = address.DistrictId;
        //            shippingAdress.WardId = address.WardId;
        //            _context.ShippingAddresses.Add(shippingAdress);
        //            _context.SaveChanges();

        //            //tao chi tiet đơn hàng (Order Detail)
        //            foreach (var item in donhang)
        //            {
        //                OrderDetail orderDetail = new OrderDetail();
        //                orderDetail.OrderId = order.OrderId;
        //                orderDetail.ProductId = item.product.ProductId;
        //                orderDetail.Amount = item.amount;
        //                orderDetail.TotalMoney = order.TotalMoney;
        //                orderDetail.Price = item.product.SalePrice;
        //                orderDetail.CreatedAt = DateTime.Now;
        //                _context.OrderDetails.Add(orderDetail);
        //            }
        //            _context.SaveChanges();
        //            //clear gio hang
        //            HttpContext.Session.Remove("Order");
        //            HttpContext.Session.Remove("CustomerOD");
        //            //Xuat thong bao
        //            _notyfService.Success("Tạo đơn hàng thành công");
        //            //cap nhat thong tin khach hang
        //            return RedirectToAction(nameof(Details), new { id = order.OrderId });
        //        }
        //        catch (Exception)
        //        {
        //            ViewBag.Customers = customers;
        //        }
        //    }
        //    ViewBag.Customers = customers;
        //    return View();
        //}
        // GET: Admin/AdminOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", order.CustomerId);
            ViewData["TransactStatusId"] = new SelectList(_context.DeliveryStatuses, "TransactStatusId", "TransactStatusId", order.DeliveryStatusId);
            return View(order);
        }
       
        // POST: Admin/AdminOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,CustomerId,OrderDate,ShipDate,TransactStatusId,Deleted,Paid,PaymentDate,TotalMoney,PaymentId,Note,Address,LocationId,District,Ward")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", order.CustomerId);
            ViewData["TransactStatusId"] = new SelectList(_context.DeliveryStatuses, "TransactStatusId", "TransactStatusId", order.DeliveryStatusId);
            return View(order);
        }

        // GET: Admin/AdminOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.DeliveryStatus)
                .Include(o => o.ShippingAddresses)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            var Chitietdonhang = _context.OrderDetails
                .Include(x => x.Product)
                .AsNoTracking()
                .Where(x => x.OrderId == order.OrderId)
                .OrderBy(x => x.OrderDetailId)
                .ToList();
            ViewBag.ChiTiet = Chitietdonhang;

            return View(order);
        }

        // POST: Admin/AdminOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            order.Deleted = true;
            _context.Update(order);
            await _context.SaveChangesAsync();
            _notyfService.Success("Xóa đơn hàng thành công");
            return RedirectToAction(nameof(Index));
        }
        public IActionResult PrintWarranty(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var order = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Guest)
                .Include(o => o.DeliveryStatus)
                .Include(o => o.Codstatus)
                .Include(o => o.PaymentStatus)
                .FirstOrDefault(m => m.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }
            var shippingAddress = _context.ShippingAddresses
                .Include(x => x.Province)
                .Include(x => x.District)
                .Include(x => x.Ward)
                .FirstOrDefault(o => o.OrderId == order.OrderId);

            string address = shippingAddress.Address +", "+ shippingAddress.Ward.Type +" " + shippingAddress.Ward.WardName + ", " + shippingAddress.District.Type +" " + shippingAddress.District.DistrictName + ", " + shippingAddress.Province.ProvinceName;

            var orderDetails = _context.OrderDetails
                .Include(x => x.Product)
                    .ThenInclude(x=>x.Brand)
                .AsNoTracking()
                .Where(x => x.OrderId == order.OrderId)
                .OrderBy(x => x.OrderDetailId)
                .ToList();
            var cardTemplate = _context.CardTemplates.Find(11);

            var WarrantyCondition = _context.CardTemplates.Find(12);


            string orderDate = order.OrderDate.Value.Day.ToString() + order.OrderDate.Value.Month.ToString() + order.OrderDate.Value.Year.ToString();

            var htmlContent = "";
            if (order.CustomerId == null)
            {
                htmlContent = textCoverWarranty(cardTemplate.HtmlContent, shippingAddress.Name, shippingAddress.SoDienThoai, address, "", orderDate, orderDetails);
            }
            else
            {
                htmlContent = textCoverWarranty(cardTemplate.HtmlContent, shippingAddress.Name, shippingAddress.SoDienThoai, address, order.Customer.CompanyName, orderDate, orderDetails);
            }

            ViewBag.BBBG = htmlContent;

            ViewBag.WarrantyCon = WarrantyCondition.HtmlContent;


            return PartialView("PrintWarranty");
        }

        //public IActionResult AAA()
        //{
        //    return PartialView("AAA");
        //}
        public string textCoverWarranty(string body, string Name, string SoDienThoai, string Address, string CompannyName, string orderDate , List<OrderDetail> Order)
        {
            var url = HttpContext.Request.Host;
            string date = DateTime.Now.Day.ToString();
            string month = DateTime.Now.Month.ToString();
            var year = DateTime.Now.Year.ToString();

            string dateTimeNow = "Ngày " + date + " tháng " + month + " năm " + year;

            var ListDevices = "";
            if (Order != null)
            {
                var stt = 1;
                    foreach (var item in Order)
                    {
                        ListDevices += " <tr valign=\"middle\">\r\n                            <td>" + stt + "</td>\r\n                            <td>" + item.Product.ProductCode + "</td>\r\n                            <td>\r\n                                <b style=\"word-break: break-all; font-weight:500\">" + item.Product.ProductName + "</b><br>\r\n                            </td>\r\n                            <td>" + item.Product.Brand.BrandName + "</td>\r\n                            <td>Pcs</td>\r\n                            <td align=\"center\">" + item.Amount + "</td>\r\n\r\n                            <td align=\"center\">" + item.Product.Warranty + "</td>\r\n\r\n                         </tr>";
                        stt++;
                    }
            }
            var text = "";
            if (body != "<p><br></p>" || body != null)
            {
                var Str = body;
                Str = Str.Replace("HovaTenKH", Name);
                Str = Str.Replace("TenCongTyKH", CompannyName);
                Str = Str.Replace("DiaChiKH", Address);
                Str = Str.Replace("SDTKH", SoDienThoai);
                Str = Str.Replace("SOPBH", orderDate);
                Str = Str.Replace("DateTimeNow", dateTimeNow); 
                /// forea san pham da mua thanh cong chuyen thanh text                
                Str = Str.Replace("ListDevices", ListDevices);
                text = Str;
            }
            return text;
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }

        [HttpPost]
        public IActionResult SaveCheckboxs(CheckboxOrder checkboxOrder)
        {
            // Lưu giá trị của các checkbox vào session
            HttpContext.Session.Set("OrderId", checkboxOrder.OrderId);
            HttpContext.Session.Set("OrderDate", checkboxOrder.OrderDate);
            HttpContext.Session.Set("CustomerName", checkboxOrder.CustomerName);
            HttpContext.Session.Set("Email", checkboxOrder.Email);
            HttpContext.Session.Set("SoDienThoai", checkboxOrder.SoDienThoai);
            HttpContext.Session.Set("PaymentStatus", checkboxOrder.PaymentStatus);
            HttpContext.Session.Set("DeliverStatus", checkboxOrder.DeliverStatus);
            HttpContext.Session.Set("CodStatus", checkboxOrder.CodStatus);
            HttpContext.Session.Set("Total", checkboxOrder.Total);

            // Thêm các checkbox khác tương tự

            return RedirectToAction("Index");
        }

        public List<CartItem> Order
        {
            get
            {
                List<CartItem> od = HttpContext.Session.Get<List<CartItem>>("Order");
                if (od == default(List<CartItem>))
                {
                    od = new List<CartItem>();
                }
                return od;
            }
        }

        [HttpPost]
        [Route("api/order/add")]
        public IActionResult AddToCart(int productID, int? amount)
        {
            List<CartItem> order = Order;
            try
            {
                //Them san pham vao gio hang
                CartItem item = order.FirstOrDefault(p => p.product.ProductId == productID);
                if (item != null) // da co => cap nhat so luong
                {
                    item.amount += amount.Value;
                    //luu lai session
                    HttpContext.Session.Set("Order", order);
                }
                else
                {
                    Product hh = _context.Products
                        .SingleOrDefault(p => p.ProductId == productID);
                    item = new CartItem
                    {
                        amount = amount.HasValue ? amount.Value : 1,
                        product = hh
                    };
                    order.Add(item);//Them vao gio
                }
                //Luu lai Session
                HttpContext.Session.Set("Order", order);
                return Json(new { success = true });
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
        }
        [HttpPost]
        [Route("api/order/update")]
        public IActionResult UpdateCart(int? productID, int? amount, int? vat)
        {
            //Lay gio hang ra de xu ly
            var order = HttpContext.Session.Get<List<CartItem>>("Order");
            try
            {
                if (order != null)
                {
                    CartItem item = order.SingleOrDefault(p => p.product.ProductId == productID);
                    if (item != null && amount.HasValue) // da co -> cap nhat so luong
                    {
                        item.amount = amount.Value;
                    }
                    //Luu lai session
                    HttpContext.Session.Set("Order", order);
                }
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        [Route("api/order/remove")]
        public ActionResult Remove(int productID)
        {
            try
            {
                List<CartItem> order = Order;
                CartItem item = order.SingleOrDefault(p => p.product.ProductId == productID);
                if (item != null)
                {
                    order.Remove(item);
                }
                //luu lai session
                HttpContext.Session.Set("Order", order);
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        [Route("api/order/clear-order")]
        public IActionResult ClearQuotation()
        {
            // Xóa tất cả sản phẩm trong giỏ hàng
            // Ví dụ: Lưu trữ giỏ hàng trong Session, xóa Session để xóa giỏ hàng
            HttpContext.Session.Remove("Order");

            // Trả về kết quả thành công
            return Json(new { success = true });
        }
        [HttpPost]
        [Route("api/order/customerinfo")]
        public IActionResult CustomerInfo(int customerId)
        {
            var customer = _context.Customers
                .FirstOrDefault(x => x.CustomerId == customerId);

            var address = _context.AccountAddresses
                .Include(x => x.Province)
                .Include(x => x.District)
                .Include(x => x.Ward)
                .FirstOrDefault(x => x.CustomerId == customer.CustomerId && x.IsDefault == true);
            HttpContext.Session.SetString("CustomerOD", customer.CustomerId.ToString());
            ViewBag.Address = address;
            return PartialView("CustomerInfo", customer);
        }

        [Route("api/order/remove-customer")]
        public IActionResult RemoveCustomer(int customerId)
        {
            HttpContext.Session.Remove("CustomerOD");
            return Json(new { success = true });
        }


        //vận chuyển
        public IActionResult Transports (){
            return View(); 
        }


    }
}
