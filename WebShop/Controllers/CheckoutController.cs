using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using WebShop.Extension;
using WebShop.Helpper;
using WebShop.Models;
using WebShop.ModelViews;
using OfficeOpenXml;
using System.Drawing;
using OfficeOpenXml.Style;
using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using NPOI.SS.Formula.Functions;
using System.Web;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebShop.Controllers
{
    public class CheckoutController : BaseController
    {
        private readonly DbMarketsContext _context;
        public INotyfService _notyfService { get; }
        public CheckoutController(DbMarketsContext context, INotyfService notyfService):base(context) 
        {
            _context = context;
            _notyfService = notyfService;
        }
        public List<CartItem> GioHang
        {
            get
            {
                var gh = HttpContext.Session.Get<List<CartItem>>("GioHang");
                if (gh == default(List<CartItem>))
                {
                    gh = new List<CartItem>();
                }
                return gh;
            }
        }
        [Route("cart", Name = "Giỏ Hàng")]
        public IActionResult Index(/*string returnUrl = null*/)
        { 
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            ViewBag.GioHang = cart;
            return View();
        }

        [Route("cart/checkInfor")]
        public IActionResult EnterInfor()
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            ViewBag.GioHang = cart;
            return View();
        }
        //bảng báo giá
        [Route("/PrintQuotation")]
        public IActionResult PrintQuotation(string code, string HoTen, string SoDienThoai, string Email, int ProvinceId, int DistrictId, int WardId, string Address)
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            //var gift = _context.GitAttributes
            //   .Include(d => d.Discount)
            //   .Include(p => p.ProductGift)
            //   .Where(p => p.Discount.ShowWeb == true)
            //       .Where(p => p.Discount.Startus == true)
            //       .Where(p => p.Discount.TimeOff > DateTime.Now)
            //       .Where(p => p.Discount.TimeOn < DateTime.Now)
            //   .ToList();
            //var discode = _context.Discounts.Where(c => c.Code == code)
            //    .Include(c => c.GitAttributes)
            //    .ThenInclude(d => d.ProductGift)
            //    .Where(d => d.TimeOn < DateTime.Now)
            //    .Where(d => d.TimeOff > DateTime.Now)
            //    .Where(d => d.Startus == true)
            //    .FirstOrDefault();
            //var discountcheck = _context.DiscountAddProducts
            //       .Include(p => p.Discount)
            //       .Where(p => p.Discount.ShowWeb == true)
            //       .Where(p => p.Discount.Startus == true)
            //       .Where(p => p.Discount.TimeOff > DateTime.Now)
            //       .Where(p => p.Discount.TimeOn < DateTime.Now)
            //       .ToList();
            var provinceName = _context.Provinces
                              .Where(x => x.ProvinceId == ProvinceId)
                              .Select(x => x.ProvinceName)
                              .FirstOrDefault();
            var districtName = _context.Districts
                             .Where(x => x.DistrictId == DistrictId)
                             .Select(x => x.DistrictName)
                             .FirstOrDefault();
            var warName =  _context.Wards
                             .Where(x => x.WardId == WardId)
                             .Select(x => x.WardName)
                             .FirstOrDefault();
            ViewBag.HoTen = HoTen;
            ViewBag.SoDienThoai = SoDienThoai;
            ViewBag.Email = Email;
            ViewBag.Address = Address + ", " + provinceName + ", " + districtName + ", " + warName;

            //ViewBag.discountcode = discode;
            //ViewBag.discount = discountcheck;
            ViewBag.Cart = cart;
            //ViewBag.gift = gift;
            return PartialView("PrintQuotation");
        }


        [HttpPost]
        [Route("cart", Name = "Giỏ Hàng")]
        public IActionResult Index(MuaHangVM muaHang)
        {
            var note = "";
            var addressSuc = "";
            var discountcheck = _context.DiscountAddProducts
                   .Include(p => p.Discount)
                   .Where(p => p.Discount.ShowWeb == true)
                   .Where(p => p.Discount.Startus == true)
                   .Where(p => p.Discount.TimeOff > DateTime.Now)
                   .Where(p => p.Discount.TimeOn < DateTime.Now)
                   .ToList();

            // list sản phẩm discout
            var gift = _context.GitAttributes
                .Include(d => d.Discount)
                .Include(p => p.ProductGift)
                .Where(p => p.Discount.ShowWeb == true)
                    .Where(p => p.Discount.Startus == true)
                    .Where(p => p.Discount.TimeOff > DateTime.Now)
                    .Where(p => p.Discount.TimeOn < DateTime.Now)
                .ToList();

            // check ma giam gia đung va tinh tien
            int discount = -1;
            var dis = _context.Discounts.Where(d => d.Code == muaHang.Code)
                 .Where(p => p.Startus == true)
                 .Where(p => p.TimeOff > DateTime.Now)
                 .Where(p => p.TimeOn < DateTime.Now)
                 .Include(p => p.GitAttributes)
                    .ThenInclude(p => p.ProductGift)
                 .FirstOrDefault();
            // sản phan khueyn mai           

            if (dis != null && muaHang.Code != null)
            {
                if (dis != null)
                {
                    discount = (int)dis.Discount1;
                }
            }

            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            if (cart == null)
            {
                return RedirectToAction("Index", "Home");
            }
            try
            {
                // ghi chú giam giá và qua tặng tren đơn hàng
                string Nodecart = "";
                foreach (var item in cart)
                {
                    var pro = discountcheck.Where(c => c.ProductId == item.product.ProductId).FirstOrDefault();
                    if (pro != null)
                    {
                        var dispro = _context.Discounts.Where(d => d.Code == pro.Discount.Code)
                                 .Where(p => p.Startus == true)
                                 .Where(p => p.TimeOff > DateTime.Now)
                                 .Where(p => p.TimeOn < DateTime.Now)
                                 .Include(p => p.GitAttributes)
                                    .ThenInclude(p => p.ProductGift)
                                 .FirstOrDefault();

                        Nodecart += "<p>-Chương trình khuyến mãi: " + dispro.Name + "</p>";
                        Nodecart += "<p> + Sản phẩm: " + item.product.ProductName + "<br/> Giá: " + item.product.SalePrice.Value.ToString("#,##0") + "VND -> Giảm còn: " + item.CartTotalMoney.ToString("#,##0") + " VND <br> Số lương:" + item.amount + "</p>\n";
                        foreach (var itemproG in dispro.GitAttributes)
                        {
                            if (itemproG != null)
                            {
                                Nodecart += "<p> + Tặng kèm: sản phẩm: " + itemproG.ProductGift.Name + "->Số lương: " + item.amount + "</p>\n";
                            }
                        }
                        Nodecart += "<hr/>";
                    }
                }

                if (discount != -1)
                {
                    if (discount <= 100)
                    {
                        Nodecart += "<p>- Giảm giá từ mã khuyến mãi:" + dis.Code + "-> " + dis.Discount1.Value.ToString("#,##0") + "%</p>\n";
                    }
                    else
                    {
                        Nodecart += "<p>- Giảm giá từ mã khuyến mãi:" + dis.Code + "-> " + dis.Discount1.Value.ToString("#,##0") + "VND</p>\n";
                    }
                    foreach (var itemproG in dis.GitAttributes)
                    {
                        if (itemproG != null)
                        {
                            Nodecart += "<p> + Tặng kèm: Sản phẩm:" + itemproG.ProductGift.Name + "</p>\n";
                        }
                    }
                }
                note = Nodecart;
                
                //Lay ra gio hang de xu ly
                var taikhoanID = HttpContext.Session.GetString("CustomerId");
                if (taikhoanID != null)
                {
                    var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CustomerId == Convert.ToInt32(taikhoanID));
                    var address = _context.AccountAddresses.AsNoTracking().SingleOrDefault(x => x.CustomerId == Convert.ToInt32(taikhoanID) && x.IsDefault == true);
                    DiaChiTaiKhoan addressnew = new DiaChiTaiKhoan();
                    addressnew.CustomerId = khachhang.CustomerId;
                    addressnew.UserName = muaHang.HoTen;
                    addressnew.SoDienThoai = muaHang.SoDienThoai;
                    addressnew.ProvinceId = muaHang.TinhThanh;
                    addressnew.DistrictId = muaHang.QuanHuyen;
                    addressnew.WardId = muaHang.PhuongXa;
                    addressnew.Content = muaHang.Address;
                    addressnew.IsDefault = true;
                    if (address == null)
                    {
                        _context.AccountAddresses.Add(addressnew);
                        _context.SaveChanges();
                    }

                    //Khoi tao don hang
                    DonHang donhangg = new DonHang();
                    donhangg.CustomerId = khachhang.CustomerId;
                    donhangg.Draft = false;
                    donhangg.CheckEmail = muaHang.CheckEmail;
                    donhangg.OrderDate = DateTime.Now;
                    donhangg.SoDienThoai = muaHang.SoDienThoai;
                    donhangg.DeliveryStatusId = 1;//Don hang moi
                    donhangg.Confirmed = false;
                    donhangg.PaymentStatusId = 1;
                    donhangg.CodstatusId = 1;
                    donhangg.Deleted = false;
                    donhangg.PaymentStatusId = 1;
                    donhangg.NotePay = muaHang.NotePay;
                    donhangg.Note = Nodecart;
                    donhangg.NodeCus = "<p>" + muaHang.Note + "</p>";
                    if (muaHang.Companyname != null || muaHang.AddressCom != null || muaHang.NumberCom != null)
                    {
                        donhangg.RedBill = muaHang.NumberCom + "/ " + muaHang.AddressCom + "/ " + muaHang.Companyname;
                    }
                    if (dis != null)
                    {
                        donhangg.Code = dis.Code;
                        donhangg.Discount = dis.Discount1;
                    }
                    // tinh tong tien theo % hoặc giam giá do mã code
                    if (discount != -1)
                    {
                        if (discount <= 100)
                        {
                            discount = (donhangg.TotalMoney * discount / 100);
                        }
                    }
                    donhangg.TotalMoney = Convert.ToInt32(cart.Sum(x => x.TotalMoney)) - (int)discount;
                    _context.Add(donhangg);
                    _context.SaveChanges();
                    muaHang.orderId=donhangg.OrderId;

                    //tao dia chi giao hang
                    ShippingAddress shippingAdress = new ShippingAddress();
                    shippingAdress.OrderId = donhangg.OrderId;
                    shippingAdress.Name = addressnew.UserName;
                    shippingAdress.SoDienThoai = addressnew.SoDienThoai;
                    shippingAdress.Address = addressnew.Content;
                    shippingAdress.ProvinceId = addressnew.ProvinceId;
                    shippingAdress.DistrictId = addressnew.DistrictId;
                    shippingAdress.WardId = addressnew.WardId;
                    addressSuc = addressSucss(addressnew.Content, (int)shippingAdress.WardId, (int)shippingAdress.DistrictId, (int)shippingAdress.ProvinceId);
                    _context.Add(shippingAdress);
                    _context.SaveChanges();

                    //tao danh sach don hang
                    foreach (var item in cart)
                    {
                        var disp = 0;
                        OrderDetail orderDetail = new OrderDetail();
                        orderDetail.OrderId = donhangg.OrderId;
                        orderDetail.ProductId = item.product.ProductId;
                        orderDetail.Amount = item.amount;
                        orderDetail.TotalMoney = donhangg.TotalMoney;
                        orderDetail.Price = item.product.SalePrice;
                        if (discountcheck != null)
                        {
                            var dispro = discountcheck.Where(c => c.ProductId == item.product.ProductId).FirstOrDefault();
                            if (dispro != null)
                            {
                                disp = (int)dispro.Discount.Discount1;
                            }
                        }
                        orderDetail.Discount = disp;
                        orderDetail.NgayTao = DateTime.Now;
                        _context.Add(orderDetail);
                    }
                    _context.SaveChanges();
                    //clear gio hang
                    HttpContext.Session.Remove("GioHang");
                    //Xuat thong bao
                    _notyfService.Success("Đơn hàng đặt thành công");
                    //cap nhat thong tin khach hang
                    //return RedirectToAction("Success", new { orderId = donhangg.OrderId });
                }
                else
                {
                    MuaHangVM model = new MuaHangVM();
                    if (ModelState.IsValid)
                    {
                        //tạo mới một khách hàng vãng lai
                        KhachVangLai guest = new KhachVangLai
                        {
                            HoTen = muaHang.HoTen,
                            SoDienThoai = muaHang.SoDienThoai.Trim().ToLower(),
                            Email = muaHang.Email.Trim().ToLower(),
                            NgayTao = DateTime.Now
                        };
                        _context.Add(guest);
                        _context.SaveChanges();
                        int guestid = guest.GuestId;
                        //Khoi tao don hang
                        DonHang donhang = new DonHang();
                        donhang.CheckEmail = muaHang.CheckEmail;
                        donhang.Draft = false;
                        donhang.GuestId = guestid;
                        donhang.OrderDate = DateTime.Now;
                        donhang.SoDienThoai = muaHang.SoDienThoai.Trim();
                        donhang.DeliveryStatusId = 1;//Don hang moi
                        donhang.Confirmed = false;
                        donhang.PaymentStatusId = 1;
                        donhang.CodstatusId = 1;
                        donhang.Deleted = false;
                        donhang.Note = Nodecart;
                        donhang.NodeCus = "<p>" + muaHang.Note + "</p>";

                        if (dis != null && muaHang.Code != null)
                        {
                            donhang.Code = dis.Code;
                            donhang.Discount = dis.Discount1;
                        }
                        if (muaHang.Companyname != null || muaHang.AddressCom != null || muaHang.NumberCom != null)
                        {
                            donhang.RedBill = muaHang.NumberCom + "/ " + muaHang.AddressCom + "/ " + muaHang.Companyname;
                        }
                        donhang.NotePay = muaHang.NotePay;
                        if (discount != -1)
                        {
                            if (discount <= 100)
                            {
                                discount = (donhang.TotalMoney * discount / 100);
                            }
                        }
                        if (discount != -1)
                        {
                            donhang.TotalMoney = Convert.ToInt32(cart.Sum(x => x.TotalMoney) - (double)discount);
                        }
                        else
                        {
                            donhang.TotalMoney = Convert.ToInt32(cart.Sum(x => x.TotalMoney));
                        }
                        _context.Add(donhang);
                        _context.SaveChanges();
                        muaHang.orderId = donhang.OrderId;                 
                        //tao dia chi giao hang
                        ShippingAddress shippingAdress = new ShippingAddress();
                        shippingAdress.OrderId = donhang.OrderId;
                        shippingAdress.Name = muaHang.HoTen;
                        shippingAdress.SoDienThoai = muaHang.SoDienThoai;
                        shippingAdress.Address = muaHang.Address;
                        shippingAdress.ProvinceId = muaHang.TinhThanh;
                        shippingAdress.DistrictId = muaHang.QuanHuyen;
                        shippingAdress.WardId = muaHang.PhuongXa;
                        addressSuc = addressSucss(shippingAdress.Address, (int)shippingAdress.WardId, (int)shippingAdress.DistrictId, (int)shippingAdress.ProvinceId);
                        _context.Add(shippingAdress);
                        _context.SaveChanges();
                        //tao danh sach don hang
                        foreach (var item in cart)
                        {
                            var disp = 0;
                            OrderDetail orderDetail = new OrderDetail();
                            orderDetail.OrderId = donhang.OrderId;
                            orderDetail.ProductId = item.product.ProductId;
                            orderDetail.Amount = item.amount;
                            orderDetail.Price = item.product.SalePrice;
                            orderDetail.TotalMoney = donhang.TotalMoney;
                            if (discountcheck != null && muaHang.Code != null)
                            {
                                var dispro = discountcheck.Where(c => c.ProductId == item.product.ProductId).FirstOrDefault();
                                if (dispro != null)
                                {
                                    disp = (int)dispro.Discount.Discount1;
                                }
                            }
                            orderDetail.Discount = disp;
                            orderDetail.NgayTao = DateTime.Now;
                            _context.Add(orderDetail);
                            //đặt hàng thành công thì gửi mail
                        }
                        _context.SaveChanges();
                        //clear gio hang
                        HttpContext.Session.Remove("GioHang");
                        //Xuat thong bao
                        _notyfService.Success("Đơn hàng đặt thành công");
                    }
                }
            }
            catch
            {
                ViewData["lsProvinces"] = new SelectList(_context.Provinces.OrderBy(x => x.ProvinceId).ToList(), "ProvinceId", "ProvinceName");
                ViewBag.GioHang = cart;
            }
            ViewBag.discount = discountcheck;
            ViewBag.gift = gift;
            // dư lieu khi load len view neu loi
            ViewData["lsProvinces"] = new SelectList(_context.Provinces.OrderBy(x => x.ProvinceId).ToList(), "ProvinceId", "ProvinceName");
            if (muaHang.orderId != 0)
            {
                ViewBag.Orderd = _context.Orders
                    .Where(c => c.OrderId == muaHang.orderId)
                    .Include(c=>c.OrderDetails)
                    .ThenInclude(c=>c.Product)
                    .FirstOrDefault();
            }
            ViewBag.GioHang = cart;
            ViewBag.note = note;
            ViewBag.addressCus = addressSuc;
            return View(muaHang);
        }

        [HttpPost]
        [Route("/save-Order")]
        public IActionResult SaveOrder(string HoTen, string SoDienThoai, string Email, int ProvinceId, int DistrictId, int WardId, string Address)
        {
            // Lấy giỏ hàng từ session
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index");
            }
            foreach(var item in cart)
            {
                var product = _context.Products.Find(item.product.ProductId);
                if(product.SalePrice == 0 && product.Price ==0)
                {
                    return Json(new { success = false, message = $"Sản phẩm '{product.ProductName}' có giá không hợp lệ. Vui lòng kiểm tra lại!" }); 
                }    
            }    

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var taikhoanID = HttpContext.Session.GetString("CustomerId");
                    decimal totalMoney = cart.Sum(item =>
                        (decimal)(item.product.SalePrice > 0 ? item.product.SalePrice : item.product.Price) * item.amount);

                    KhachVangLai guest = null;
                    //tạo list Orderdetail để gửi mail
                    List<OrderDetail>lsOD = new List<OrderDetail>();
                    // Nếu khách hàng chưa đăng nhập, tạo mới một khách hàng vãng lai
                    if (taikhoanID == null)
                    {
                        guest = new KhachVangLai
                        {
                            HoTen = HoTen,
                            SoDienThoai = SoDienThoai.Trim().ToLower(),
                            Email = Email.Trim().ToLower(),
                            NgayTao = DateTime.Now
                        };
                        _context.Guests.Add(guest);
                        _context.SaveChanges();
                    }

                    // Tạo đơn hàng
                    var order = new DonHang
                    {
                        GuestId = guest?.GuestId, // Có thể là null nếu khách hàng đã đăng nhập
                        SoDienThoai = guest?.SoDienThoai,
                        OrderDate = DateTime.Now,
                        DeliveryStatusId = 1,
                        PaymentStatusId = 1,
                        CodstatusId = 1,
                        Deleted = false,
                        TotalMoney = Convert.ToInt32(totalMoney),
                        Draft = false,
                        Code = GenerateOrderCode(guest?.GuestId ?? 0),
                        Confirmed = false
                    };

                    _context.Orders.Add(order);
                    _context.SaveChanges();

                    // Tạo chi tiết đơn hàng
                    foreach (var item in cart)
                    {
                        var orderDetail = new OrderDetail
                        {
                            OrderId = order.OrderId,
                            ProductId = item.product.ProductId,
                            Amount = item.amount,
                            Discount = 0,
                            TotalMoney = item.amount * (item.product.SalePrice > 0 ? item.product.SalePrice : item.product.Price),
                            NgayTao = DateTime.Now,
                            Price = (item.product.SalePrice > 0 ? item.product.SalePrice : item.product.Price)
                        };
                        _context.OrderDetails.Add(orderDetail);
                        _context.SaveChanges();
                        lsOD.Add(orderDetail);
                    }

                    // Tạo địa chỉ giao hàng
                    var shippingAddress = new ShippingAddress
                    {
                        OrderId = order.OrderId,
                        Name = guest?.HoTen,
                        SoDienThoai = guest?.SoDienThoai,
                        ProvinceId = ProvinceId,
                        WardId = WardId,
                        DistrictId = DistrictId,
                        Address = Address
                    };
                    _context.ShippingAddresses.Add(shippingAddress);
                    _context.SaveChanges();

                    // Cam kết giao dịch
                    transaction.Commit();
                     
                    HttpContext.Session.SetInt32("CurrentOrderId", order.OrderId);
                    SendEmail2(guest,shippingAddress.Address ,lsOD,order);
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    // Ghi lại lỗi nếu có
                    transaction.Rollback();
                    // Trả về thông báo lỗi hoặc xử lý lỗi tại đây
                    return Json(new { success = false, message = "Có lỗi xảy ra khi lưu đơn hàng." });
                }
            }
        }


        [Route("/OrderSuccess")]
        public IActionResult OrderSuccess()
        {
            int? orderId = HttpContext.Session.GetInt32("CurrentOrderId");

            if (!orderId.HasValue)
            {
                return RedirectToAction("Index");
            }

            var order = _context.Orders.Include(pay=>pay.PaymentStatus).Where(o=>o.OrderId == orderId).FirstOrDefault();

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            // Lấy danh sách chi tiết đơn hàng
            var orderDetails = _context.OrderDetails.Include(p => p.Product).Where(od => od.OrderId == orderId.Value).ToList();

            // Xóa sản phẩm đã đặt hàng khỏi giỏ hàng trong session
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            if (cart != null)
            {
                foreach (var detail in orderDetails)
                {
                    var itemToRemove = cart.FirstOrDefault(c => c.product.ProductId == detail.ProductId);
                    if (itemToRemove != null)
                    {
                        cart.Remove(itemToRemove);
                    }
                }
                HttpContext.Session.Set("GioHang", cart); // Cập nhật lại giỏ hàng trong session
            }

            var muaHangSuccessVM = new MuaHangSuccessVM
            {
                DonHangID = order.OrderId,
                Order = order,
                OrderDetails = orderDetails // Thêm danh sách sản phẩm vào mô hình
            };

            return View(muaHangSuccessVM);
        }



        // Hàm tạo mã đơn hàng
        private string GenerateOrderCode(int guestId)
        {
            string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss"); // Lấy thời gian hiện tại
            string randomString = Guid.NewGuid().ToString().Substring(0, 6); // Lấy 6 ký tự ngẫu nhiên từ GUID
            return $"ORD-{timeStamp}-{randomString}-{guestId}"; // Kết hợp thành mã đơn hàng
        }


        //thay đổi trạng thái khi người dùng thanh toán VNPAY và trả vveef thành công
        [Route("/ChangeStatusPay")]
        [HttpPost] 
        public IActionResult changeStatusPay(string codeId, int OrderId)
        {
            var Order = _context.Orders.Where(c => c.Code == codeId && c.OrderId == OrderId).FirstOrDefault();
            if (Order == null)
            { 
                return RedirectToAction("Index");
            }
            else
            {
                Order.PaymentStatusId= 2;//thanh toán
                _context.Orders.Update(Order);
                _context.SaveChanges();
                return Json(new { success = true });
            }  
        }

            public IActionResult AddAccountAddress(int TinhThanh1,int QuanHuyen1,int PhuongXa1,string Address1, string PhoneAdd,string FullNameAdd,int CustomerAdd)
        {
            bool success = true;
            DiaChiTaiKhoan cus = new DiaChiTaiKhoan();
            try {
                cus.CustomerId = CustomerAdd;
                cus.SoDienThoai=PhoneAdd;
                cus.UserName = FullNameAdd;
                cus.ProvinceId = TinhThanh1;
                cus.DistrictId= QuanHuyen1;
                cus.WardId= PhuongXa1;
                cus.Content = Address1;
                cus.IsDefault = false;
                _context.AccountAddresses.Add(cus);
                _context.SaveChanges();
            }
            catch(Exception ex) { 
                success=false;
            }
            return Json(new {data=success});
        }

        [HttpGet("/cart/getCartCount")]
        public IActionResult GetCartCount()
        {
            // Lấy giỏ hàng từ session
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");

            // Nếu giỏ hàng null thì số lượng sản phẩm là 0, nếu không thì đếm số lượng sản phẩm
            var cartCount = cart?.Count ?? 0;

            return Json(new { cartCount });
        }



        //[Route("dat-hang-thanh-cong.html", Name = "Success")]
        //public IActionResult Success(int guestId, int orderId)
        //{
        //    var discountcheck = _context.DiscountAddProducts
        //          .Include(p => p.Discount)
        //          .Where(p => p.Discount.ShowWeb == true)
        //          .Where(p => p.Discount.Startus == true)
        //          .Where(p => p.Discount.TimeOff > DateTime.Now)
        //          .Where(p => p.Discount.TimeOn < DateTime.Now)
        //          .ToList();

        //    // list sản phẩm discout

        //    var gift = _context.GitAttributes
        //        .Include(d => d.Discount)
        //        .Include(p => p.ProductGift)
        //        .Where(p => p.Discount.ShowWeb == true)
        //            .Where(p => p.Discount.Startus == true)
        //            .Where(p => p.Discount.TimeOff > DateTime.Now)
        //            .Where(p => p.Discount.TimeOn < DateTime.Now)
        //        .ToList();

        //    ViewBag.discount = discountcheck;
        //    ViewBag.gift = gift;
        //    try
        //    {
        //        var taikhoanID = HttpContext.Session.GetString("CustomerId");
        //        if (string.IsNullOrEmpty(taikhoanID))
        //        {
        //            var khachhangvanglai = _context.Guests.AsNoTracking().SingleOrDefault(x => x.GuestId == guestId);
        //            var donhangg = _context.Orders
        //                .Where(x => x.GuestId == guestId && x.OrderId == orderId)
        //                .FirstOrDefault();

        //            var shippingAddress = _context.ShippingAddresses
        //                .Include(x => x.Province)
        //                .Include(x => x.District)
        //                .Include(x => x.Ward)
        //                .Where(o => o.OrderId == donhangg.OrderId)
        //                .FirstOrDefault();

        //            var lsProduct = _context.OrderDetails
        //                .Include(x => x.Product)
        //                .Where(x => x.OrderId == donhangg.OrderId)
        //                .ToList();
        //            MuaHangSuccessVM successVMM = new MuaHangSuccessVM();
        //            successVMM.Order = donhangg;
        //            successVMM.HoTen = khachhangvanglai.HoTen;
        //            successVMM.DonHangID = donhangg.OrderId;
        //            successVMM.SoDienThoai = khachhangvanglai.SoDienThoai;
        //            successVMM.Email = khachhangvanglai.Email;
        //            successVMM.TinhThanh = GetNameProvince(shippingAddress.Province.ProvinceId);
        //            successVMM.QuanHuyen = GetNameDistrict(shippingAddress.District.DistrictId);
        //            successVMM.PhuongXa = GetNameWard(shippingAddress.Ward.WardId);
        //            successVMM.Address = shippingAddress.Address;
        //            ViewBag.lsProduct = lsProduct;
        //            var address = shippingAddress.Address + GetNameWard(shippingAddress.Ward.WardId) + GetNameDistrict(shippingAddress.District.DistrictId) + GetNameProvince(shippingAddress.Province.ProvinceId);
        //            SendEmail2(khachhangvanglai, address, lsProduct,donhangg);
        //            return View(successVMM);
        //        }
        //        else
        //        {
        //            var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CustomerId == Convert.ToInt32(taikhoanID));
        //            var donhang = _context.Orders
        //                .Where(x => x.OrderId == orderId && x.CustomerId == khachhang.CustomerId)
        //                .FirstOrDefault();

        //            var shippingAddress = _context.ShippingAddresses
        //               .Include(x => x.Province)
        //               .Include(x => x.District)
        //               .Include(x => x.Ward)
        //               .Where(o => o.OrderId == donhang.OrderId)
        //               .FirstOrDefault();
        //            var lsProduct = _context.OrderDetails
        //                .Include(x => x.Product)
        //                .Where(x => x.OrderId == donhang.OrderId)
        //                .ToList();

        //            MuaHangSuccessVM successVM = new MuaHangSuccessVM();
        //            successVM.Order = donhang;
        //            successVM.HoTen = khachhang.HoTen;
        //            successVM.DonHangID = donhang.OrderId;
        //            successVM.SoDienThoai = donhang.SoDienThoai;
        //            successVM.Email = khachhang.Email;
        //            successVM.TinhThanh = GetNameProvince(shippingAddress.Province.ProvinceId);
        //            successVM.QuanHuyen = GetNameDistrict(shippingAddress.District.DistrictId);
        //            successVM.PhuongXa = GetNameWard(shippingAddress.Ward.WardId);
        //            successVM.Address = shippingAddress.Address;
        //            ViewBag.lsProduct = lsProduct;
        //            SendEmail(khachhang, lsProduct,donhang);
        //            return View(successVM);
        //        }
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        [HttpPost]
        [Route("api/Checkouts/checkCode")]
        public IActionResult checkCode(string text)
        {
            var code = _context.Discounts
                .Where(d => d.TimeOn < DateTime.Now)
                .Where(d => d.TimeOff > DateTime.Now)
                .Where(d => d.Startus == true)
                .Where(x => x.Code == text)
                .FirstOrDefault();
            if (code == null)
            {
                return Json(new { Success = "No", data = 0 });
            }
            return Json(new { Success = "Ok", data = code });
        }
        //check address
        public string addressSucss(string text, int id1,int id2,int id3)
        {
            var textfull = text;
            var p = _context.Wards.Where(c => c.WardId == id1).FirstOrDefault();
            var q = _context.Districts.Where(c=>c.DistrictId==id2).FirstOrDefault();
            var t = _context.Provinces.Where(c => c.ProvinceId == id3).FirstOrDefault();
            textfull += ", " + p.Type + p.WardName + ", " + q.Type + q.DistrictName + ", " + t.Type + t.ProvinceName;
            return textfull;
        }
        [HttpPost]
        [Route("api/Checkouts/checkGift")]
        public IActionResult checkGift(int id)
        {
            var attr = _context.GitAttributes.Where(p => p.DiscountId == id).ToList();

            if (attr == null)
            {
                return Json(new { Success = "No", data = 0 });
            }
            return Json(new { Success = "Ok", data = attr });
        }

        [Route("api/Checkouts/productgift")]
        public IActionResult productgift()
        {
            var attr = _context.ProductGifts.ToList();
            if (attr == null)
            {
                return Json(new { Success = "No", data = 0 });
            }
            return Json(new { Success = "Ok", data = attr });
        }

        // send email dat hang thanh cong
        public IActionResult SendEmail(KhachHang tk, List<OrderDetail> listO,DonHang donhang)
        {
            var address = "";
            var systemW = _context.SystemWebs.FirstOrDefault();
            var Admin = _context.PageInfos.FirstOrDefault();
            var email = new MimeMessage();
            try
            {
                if (systemW.PassSmtp == "" || systemW.Name == null || systemW.Post == null || systemW.Name == null)
                {
                    return Json(new { succses = "No", value = "Vui lòng kiểm tra Quản lý website > Hệ thống." });
                }
                else
                {
                    try
                    {
                        var addressid = _context.AccountAddresses.Where(c => c.CustomerId == tk.CustomerId).Where(c => c.IsDefault == true).Include(c => c.Ward).Include(c => c.District).Include(c => c.Province).FirstOrDefault();
                        if (addressid != null)
                        {
                            address = addressid.Content + "/" + addressid.Ward.WardName + "/" + addressid.District.DistrictName + "/" + addressid.Province.ProvinceName;
                        }
                        email.From.Add(MailboxAddress.Parse(systemW.EmailSend));
                        using var smtp = new MailKit.Net.Smtp.SmtpClient();
                        smtp.Connect(systemW.Server, (int)systemW.Post, SecureSocketOptions.StartTls);
                        smtp.Authenticate(systemW.EmailSmtp, systemW.PassSmtp);
                        try
                        {
                            var optionEmail = _context.EmailMakettings.Where(i => i.EmailEvent == 8).FirstOrDefault();
                            var text = textcover(optionEmail.Body, tk.HoTen, tk.Email, tk.SoDienThoai.ToString(), address, "", listO,donhang);
                            email.Bcc.Add(MailboxAddress.Parse(tk.Email));
                            email.To.Add(MailboxAddress.Parse(tk.Email));
                            email.Subject = optionEmail.Title;
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
                    catch (Exception e)
                    {
                        return Json(new { succses = "No" });
                    }

                }
            }
            catch (Exception e)
            {

            }


            return Json(new { succses = "Ok" });
        }

        // kahch hang van lai
        public IActionResult SendEmail2(KhachVangLai tk, string address, List<OrderDetail> listO,DonHang donhang)
        {
            var address1 = "";
            if (address != null)
            {
                address1 = address;
            }
            var systemW = _context.SystemWebs.FirstOrDefault();
            var Admin = _context.PageInfos.FirstOrDefault();
            var email = new MimeMessage();
            try
            {
                if (systemW.PassSmtp == "" || systemW.Name == null || systemW.Post == null || systemW.Name == null)
                {
                    return Json(new { succses = "No", value = "Vui lòng kiểm tra Quản lý website > Hệ thống." });
                }
                else
                {
                    try
                    {

                        email.From.Add(MailboxAddress.Parse(systemW.EmailSend));
                        using var smtp = new MailKit.Net.Smtp.SmtpClient();
                        smtp.Connect(systemW.Server, (int)systemW.Post, SecureSocketOptions.StartTls);
                        smtp.Authenticate(systemW.EmailSmtp, systemW.PassSmtp);
                        try
                        {
                            var optionEmail = _context.EmailMakettings.Where(i => i.EmailEvent == 8).FirstOrDefault();
                            var text = textcover(optionEmail.Body, tk.HoTen, tk.Email, tk.SoDienThoai.ToString(), address1, "", listO, donhang);
                            email.Bcc.Add(MailboxAddress.Parse(tk.Email));
                            email.To.Add(MailboxAddress.Parse(tk.Email));
                            email.Subject = optionEmail.Title;
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
                    catch (Exception e)
                    {
                        return Json(new { succses = "No" });
                    }

                }
            }
            catch (Exception e)
            {

            }


            return Json(new { succses = "Ok" });
        }

        //thay đổii nội dung phần Email gửi khách hàng
        public string textcover(string body, string Name, string Email, string SoDienThoai, string Address, string CompannyName, List<OrderDetail> Order, DonHang donhang)
        {
            var url = HttpContext.Request.Host;
            var id = "";
            int productPice = 0;
            var producttata = 0;
            var sale = 0;

            // Khởi tạo sản phẩm với cấu trúc HTML rõ ràng và đơn giản hơn
            var product = @"
                            <table cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse;'>
                                <thead style='background-color:rgb(255, 193, 7);'>
                                    <tr>
                                        <th style='text-align:left; padding: 10px;'>Sản phẩm</th>
                                        <th style='text-align:left; padding: 10px;'>Tên</th>
                                        <th style='text-align:center; padding: 10px;'>Số lượng</th>
                                        <th style='text-align:right; padding: 10px;'>Thành tiền</th>
                                    </tr>
                                </thead>
                                <tbody>";

                                    var node = $@"
                            <tr>
                                <td colspan='4' style='padding: 10px; text-align:center; color:#555;'>{donhang.NodeCus}</td>
                            </tr>
                            <tr>
                                <td colspan='4' style='padding: 10px; text-align:center; color:#555;'>{donhang.Note}</td>
                            </tr>";

                                    if (Order != null && Order.Any())
                                    {
                                        foreach (var item in Order)
                                        {
                                            if (id !=null)
                                            {
                                                 id = item.Order.Code;
                                            }
                                            var Price = (int)item.Product.SalePrice; // Giá gốc
                                            var SalePrice = (int)item.Price; // Giá sau khi giảm
                                            productPice += Price;
                                            producttata += SalePrice;

                                            var totalSale = (Price == SalePrice ? 0 : item.Amount * SalePrice);
                                            decimal totalPrice = (decimal)item.Amount * (decimal)Price;

                                            product += $@"
                                    <tr>
                                        <td style='padding: 10px;'><img src='https://novazone.co/{item.Product.Avatar}' width='100px' style='border-radius: 8px;'></td>
                                        <td style='padding: 10px;'>{item.Product.ProductName}</td>
                                        <td style='padding: 10px; text-align:center;'>X {item.Amount}</td>
                                        <td style='padding: 10px; text-align:right;'>
                                            {(totalSale > 0 ? totalSale?.ToString("#,##0") ?? "0" + " VND" : totalPrice.ToString("#,##0") + " VND")}
                                            <br>
                                            {(totalSale > 0 ? $"<strike style='color: #999;'>{totalPrice.ToString("#,##0")} VND</strike>" : "")}
                                        </td>
                                    </tr>";
                                        }

                                        sale = productPice - producttata;

                                        product += @"
                                    </tbody>
                                </table>
                                <hr style='border: 1px solid #ddd;' />
                                <table width='100%' style='border-collapse: collapse;'>
                                    <tbody>
                                        <tr>
                                            <td style='padding: 10px 0;' width='60%' align='left'>Tổng giá trị sản phẩm:</td>
                                            <td style='padding: 10px 0;' width='40%' align='right'>" + productPice.ToString("#,##0") + @" VND</td>
                                        </tr>
                                        <tr>
                                            <td style='padding: 10px 0;' align='left'>Giảm giá:</td>
                                            <td style='padding: 10px 0;' align='right'>" + sale.ToString("#,##0") + @" VND</td>
                                        </tr>
                                        <tr>
                                            <td colspan='2'><hr /></td>
                                        </tr>
                                        <tr>
                                            <td style='padding: 10px 0;' align='left'><strong>Tổng tiền:</strong></td>
                                            <td style='padding: 10px 0;' align='right'><h3>" + producttata.ToString("#,##0") + @" VND</h3></td>
                                        </tr>
                                    </tbody>
                                </table>";

                                        product += node; // thêm thông tin ghi chú
            }

            var text = "";

            if (!string.IsNullOrEmpty(body) && body != "<p><br></p>")
            {
                var Str = body;
                Str = Str.Replace("HovaTenKH", Name);
                Str = Str.Replace("TenCongTyKH", CompannyName);
                Str = Str.Replace("EmailKH", Email);
                Str = Str.Replace("DiaChiKH", Address);
                Str = Str.Replace("SDTKH", SoDienThoai);
                Str = Str.Replace("MDH", id.ToString());
                Str = Str.Replace("Node", node);
                Str = Str.Replace("SPKHDM", product);
                text = Str;
            }

            return text;
        }



        public IActionResult ExportQuotation()
        {
            // Lấy danh sách sản phẩm từ session
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");


            // Tạo một đối tượng ExcelPackage từ file Excel mẫu
            // Tạo một đối tượng ExcelPackage mới để làm bản sao từ file Excel mẫu
            using ExcelPackage package = new ExcelPackage();

            // Tạo một worksheet mới trong package
            var worksheet = package.Workbook.Worksheets.Add("bao_gia");

            // Tắt lưới (grid) trong sheet
            worksheet.View.ShowGridLines = false;
            worksheet.Cells["A1:Z100"].Style.Font.Size = 11;

            // Đường dẫn tới hình ảnh trong thư mục wwwroot

            // Tạo một bản sao tạm thời của hình ảnh trong thư mục tạm
            // Đường dẫn tới hình ảnh trong thư mục wwwroot
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Adminassets", "images", "logo", "logo-novazone-excel.png");

            // Chèn hình ảnh từ đường dẫn vào Excel
            var image = worksheet.Drawings.AddPicture("Image", new FileInfo(imagePath));
            image.SetPosition(1, 0, 1, 0);

            //Merge

            worksheet.Cells["D1:H1"].Merge = true;
            worksheet.Row(1).Height = 21;
            worksheet.Cells["D2:H2"].Merge = true;
            worksheet.Cells["D3:H3"].Merge = true;
            worksheet.Cells["D4:H4"].Merge = true;
            worksheet.Cells["F5:G5"].Merge = true;
            worksheet.Cells["D6:H6"].Merge = true;


            worksheet.Cells["A7:H7"].Merge = true;
            worksheet.Row(7).Height = 40;

            worksheet.Cells["A8:H8"].Merge = true;
            worksheet.Cells[string.Format("A8:H8")].Style.Font.Bold = true;
            worksheet.Row(8).Height = 85;
            worksheet.Cells["A8"].Value = "To (Kính gửi)                        : \r\nAddress (Địa chỉ)               :\r\nEmail                                       :\r\nContact (Người liên hệ) :\r\nPhone (Số điện thoại)     :    ";

            var cell = worksheet.Cells["A8:H8"];

            // Thiết lập loại đường viền và kiểu nét đứt
            cell.Style.Border.Top.Style = ExcelBorderStyle.Dotted;
            cell.Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;
            cell.Style.Border.Left.Style = ExcelBorderStyle.Dotted;
            cell.Style.Border.Right.Style = ExcelBorderStyle.Dotted;


            worksheet.Cells["A9:H9"].Merge = true;
            worksheet.Row(9).Height = 60;
            worksheet.Cells["A9"].Value = "We highly appreciate your interests in and keep working with our bussiness. Please find the below our quotation which we think serves you best in terms of price and liability. (Công ty Novazone đánh giá cao sự quan tâm và hợp tác kinh doanh của Quí khách. Quí khách vui lòng xem bảng báo giá sau đây mà Công ty Novazone cho rằng sẽ đáp ứng Quí khách tốt nhất trong bảng báo giá và trách nhiệm).";

            var cell1 = worksheet.Cells["A9:H9"];
            cell1.Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;
            cell1.Style.Border.Left.Style = ExcelBorderStyle.Dotted;
            cell1.Style.Border.Right.Style = ExcelBorderStyle.Dotted;

            worksheet.Cells["A7:H7"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells["A7:H7"].Style.Fill.BackgroundColor.SetColor(Color.DodgerBlue);

            worksheet.Cells["A7:H7"].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells["A7:H7"].Style.Font.Bold = true;





            var date = DateTime.Now.ToString("dd/MM/yyyy");

            //Điền dữ liệu
            worksheet.Cells["D1"].Value = "CÔNG TY TNHH NOVAZONE";
            worksheet.Cells["D2"].Value = "Địa chỉ: 11A Hồng Hà, Phường 2, Quận Tân Bình, TP. HCM";
            worksheet.Cells["D3"].Value = "Tel: 0937 963 779 ";
            worksheet.Cells["D4"].Value = "Email: hello@novazone.com.vn - Website: www.novazone.co";

            worksheet.Cells["A7"].Value = "QUOTATION (BẢNG BÁO GIÁ)";

            worksheet.Cells["F5"].Value = "Date (Ngày báo giá): " + date;
            worksheet.Cells["F5"].Style.Font.Italic = true;

            worksheet.Cells["A10"].Style.Font.Bold = true;


            //worksheet.Cells["E10"].Value = "Đơn vị tính: VNĐ";
            //worksheet.Cells["E10"].Style.Font.Bold = true;

            worksheet.Cells["A11"].Value = "No.\nStt";
            worksheet.Cells["B11"].Value = "P/N\nMã Hàng";
            worksheet.Cells["C11"].Value = "Descriptions\nMô Tả";
            worksheet.Cells["D11"].Value = "Unit\nĐvt";
            worksheet.Cells["E11"].Value = "Qty\nSL";
            worksheet.Cells["F11"].Value = "Unit Price\nĐơn Giá";
            worksheet.Cells["G11"].Value = "Total Amount\nThành Tiền";
            worksheet.Cells["H11"].Value = "Warranty\nBảo Hành";

            // Chỉnh chiều rộng của cột A
            worksheet.Column(1).Width = 4.14;
            worksheet.Column(2).Width = 9.57;
            worksheet.Column(3).Width = 34.29;
            worksheet.Column(4).Width = 6.14;
            worksheet.Column(5).Width = 6.57;
            worksheet.Column(6).Width = 14.29;
            worksheet.Column(7).Width = 14.82;
            worksheet.Column(8).Width = 14.14;

            // Thiết lập wrap text cho cột B
            worksheet.Column(1).Style.WrapText = true;
            worksheet.Column(2).Style.WrapText = true;
            worksheet.Column(3).Style.WrapText = true;
            worksheet.Column(4).Style.WrapText = true;
            worksheet.Column(5).Style.WrapText = true;
            worksheet.Column(6).Style.WrapText = true;
            worksheet.Column(7).Style.WrapText = true;
            worksheet.Column(8).Style.WrapText = true;
            Color backgroundColor = Color.LightSkyBlue;
            Color textColor = Color.Black;

            worksheet.Cells["A11:H11"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells["A11:H11"].Style.Fill.BackgroundColor.SetColor(backgroundColor);

            worksheet.Cells["A11:H11"].Style.Font.Color.SetColor(textColor);
            worksheet.Cells["A11:H11"].Style.Font.Bold = true;
            worksheet.Cells["A7"].Style.Font.Bold = true;
            worksheet.Cells["A7"].Style.Font.Size = 17;
            worksheet.Cells["A7"].Style.Font.Color.SetColor(Color.White);

            worksheet.Cells["D1"].Style.Font.Bold = true;
            worksheet.Cells["D1"].Style.Font.Size = 15;
            worksheet.Cells["D1"].Style.Font.Color.SetColor(Color.DodgerBlue);

            worksheet.Cells["A7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells["A7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            //worksheet.Cells["E2:E5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells["D1:D4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

            worksheet.Cells["A8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells["A8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

            worksheet.Cells["A9"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells["A9"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;



            // Ghi dữ liệu từ danh sách hóa đơn vào worksheet
            int row = 12; // Vị trí hàng bắt đầu ghi dữ liệu
            int stt = 1;
            foreach (var hoaDon in cart)
            {
                //worksheet.Cells[row, 3, row, 4].Merge = true;
                //worksheet.Row(row).Height = 35;

                worksheet.Cells[string.Format("A{0}", row)].Value = stt;
                worksheet.Cells[string.Format("B{0}", row)].Value = hoaDon.product.ProductCode;
                worksheet.Cells[string.Format("C{0}", row)].Value = hoaDon.product.ProductName;
                worksheet.Cells[string.Format("D{0}", row)].Value = "Pcs";
                worksheet.Cells[string.Format("E{0}", row)].Value = hoaDon.amount;
                worksheet.Cells[string.Format("F{0}", row)].Value = Extension.Extension.ToVnd(hoaDon.product.SalePrice.Value);
                worksheet.Cells[string.Format("G{0}", row)].Value = Extension.Extension.ToVnd(hoaDon.TotalMoney);
                worksheet.Cells[string.Format("H{0}", row)].Value = hoaDon.product.Warranty + " tháng";

                // ...

                row++;
                stt++;
            }
            worksheet.Cells[row, 1, row, 3].Merge = true;
            worksheet.Cells[row + 1, 1, row + 1, 3].Merge = true;
            worksheet.Cells[row + 2, 1, row + 2, 3].Merge = true;



            //worksheet.Cells[row , 6, row, 7].Merge = true;
            //worksheet.Cells[row + 1, 6, row + 1, 7].Merge = true;
            //worksheet.Cells[row + 2, 6, row + 2, 7].Merge = true;

            worksheet.Cells[row, 7, row, 8].Style.Font.Bold = true;
            worksheet.Cells[row + 1, 7, row + 1, 8].Style.Font.Bold = true;
            worksheet.Cells[row + 2, 7, row + 2, 8].Style.Font.Bold = true;

            // Thiết lập border cho ô
            var mergedCell = worksheet.Cells[11, 1, row + 2, 8];
            //tạo khung nét liền cho bảng báo giá

            //mergedCell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            //mergedCell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            //mergedCell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            //mergedCell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            mergedCell.Style.Border.Top.Style = ExcelBorderStyle.Dotted;
            mergedCell.Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;
            mergedCell.Style.Border.Left.Style = ExcelBorderStyle.Dotted;
            mergedCell.Style.Border.Right.Style = ExcelBorderStyle.Dotted;


            mergedCell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            mergedCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;



            //worksheet.Cells[row+ 3, 1, row+3, 8].Merge = true;
            //worksheet.Cells[row + 4, 1, row + 4, 8].Merge = true;
            //worksheet.Cells[row + 5, 1, row + 5, 8].Merge = true;
            //worksheet.Cells[row + 6, 1, row + 6, 8].Merge = true;

            //worksheet.Cells[row + 7, 1, row + 7, 4].Merge = true;
            //worksheet.Cells[row + 8, 1, row + 8, 4].Merge = true;
            //worksheet.Cells[row + 8, 5, row + 8, 8].Merge = true;

            //worksheet.Cells[row + 5, 1, row + 5, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            //worksheet.Cells[row + 5, 1, row + 5, 8].Style.Border.Bottom.Color.SetColor(Color.Red);

            //worksheet.Cells[row + 6, 1, row + 6, 8].Style.Border.Top.Style = ExcelBorderStyle.Medium;
            //worksheet.Cells[row + 6, 1, row + 6, 8].Style.Border.Top.Color.SetColor(Color.Red);

            worksheet.Cells[string.Format("A{0}", row)].Value = "Total(Tổng Cộng)";
            worksheet.Cells[string.Format("A{0}", row)].Style.Font.Bold = true;
            worksheet.Cells[string.Format("A{0}", row + 1)].Value = "VAT (10%) (Thuế GTGT 10%)";
            worksheet.Cells[string.Format("A{0}", row + 1)].Style.Font.Bold = true;
            worksheet.Cells[string.Format("A{0}", row + 2)].Value = "Grand Total (Tổng giá trị sau thuế)";
            worksheet.Cells[string.Format("A{0}", row + 2)].Style.Font.Bold = true;


            worksheet.Cells[string.Format("A{0}", row + 4)].Value = "Terms & Conditions (Điều khoản và điều kiện):";
            worksheet.Cells[string.Format("A{0}", row + 4)].Style.Font.Bold = true;
            worksheet.Cells[row + 4, 1, row + 4, 8].Merge = true;

            worksheet.Cells[string.Format("A{0}", row + 5)].Value = "1. Warranty time: Follow Manufactory warranty policy (Bảo hành thiết bị theo tiêu chuẩn nhà sản xuất)\n2. Delivery: 1-3 day after you confirm the order (Thời hạn giao: 1-3 ngày sau khi nhận được đơn đặt hàng của Quí khách)\n3.Payment: Cash or Transfer ( Thanh Toán: Bằng Tiền mặt hoặc Chuyển khoản)\n4. This Quotation is within 1 day, (Bảng báo giá này có giá trị trong 1 ngày)\n5. If you have any questions concerning this Quotation, contact us, (Quí khách có bất kì câu hỏi liên quan đến bảng giá, vui lòng liên hệ với chúng tôi)\n6.Bank Account: please make your prompt payment to our following bank Account, (Tài Khoản Ngân Hàng: Quí khách vui lòng thanh toán qua tài khoản của chúng tôi)";
            worksheet.Row(row + 5).Height = 140;
            worksheet.Cells[row + 5, 1, row + 5, 8].Merge = true;
            worksheet.Cells[row + 5, 1, row + 5, 8].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[row + 5, 1, row + 5, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

            worksheet.Cells[string.Format("A{0}", row + 6)].Value = "CÔNG TY TNHH NOVAZONE\nSố TK: 6927977777 Ngân Hàng TMCP QUÂN ĐỘI MB BANK - CN HCM \nSố TK : Trương Quang Viên -  9273437777 Ngân Hàng Vietcombank -  CN Vạn Phúc ";
            worksheet.Cells[row + 6, 1, row + 6, 8].Merge = true;
            worksheet.Cells[string.Format("A{0}", row + 6)].Style.Font.Bold = true;
            worksheet.Row(row + 6).Height = 60;
            worksheet.Cells[row + 6, 1, row + 6, 8].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[row + 6, 1, row + 6, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;


            worksheet.Cells[string.Format("A{0}", row + 7)].Value = "We hope continously to be of your satisfactory services";
            worksheet.Cells[string.Format("A{0}", row + 7)].Style.Font.Bold = true;//in đậm
            worksheet.Cells[row + 7, 1, row + 7, 8].Merge = true;//xóa
            worksheet.Cells[row + 7, 1, row + 7, 8].Style.VerticalAlignment = ExcelVerticalAlignment.Center;//canh giữa theo chiều dọc
            worksheet.Cells[row + 7, 1, row + 7, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;//canh giữa theo chiều ngang



            worksheet.Cells[string.Format("A{0}", row + 8)].Value = "Novazone hy vọng tiếp tục cung cấp các dịch vụ thỏa đáng đến Quí Khách";
            worksheet.Cells[row + 8, 1, row + 8, 8].Merge = true;//xóa
            worksheet.Cells[row + 8, 1, row + 8, 8].Style.VerticalAlignment = ExcelVerticalAlignment.Center;//canh giữa theo chiều dọc
            worksheet.Cells[row + 8, 1, row + 8, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;//canh giữa theo chiều ngang

            worksheet.Cells[string.Format("A{0}", row + 9)].Value = "Yours Sincerely:";
            worksheet.Cells[string.Format("A{0}", row + 9)].Style.Font.Bold = true;//in đậm
            worksheet.Row(row + 9).Height = 30;
            worksheet.Cells[row + 9, 1, row + 9, 4].Merge = true;//xóa
            worksheet.Cells[row + 9, 1, row + 9, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;//canh giữa theo chiều dọc
            worksheet.Cells[row + 9, 1, row + 9, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;//canh giữa theo chiều ngang

            worksheet.Cells[string.Format("A{0}", row + 10)].Value = "Name: Lan Dang\nEmail: lan.dang@sunhitech.vn\nPhone:0906547766";
            worksheet.Cells[row + 10, 1, row + 10, 4].Merge = true;//xóa
            worksheet.Cells[row + 10, 1, row + 10, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;//canh giữa theo chiều dọc
            worksheet.Cells[row + 10, 1, row + 10, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;//canh giữa theo chiều ngang
            worksheet.Row(row + 10).Height = 45;

            worksheet.Cells[string.Format("E{0}", row + 9)].Value = "Customer's confirmation (Xác nhận đặt hàng)";
            worksheet.Cells[row + 9, 5, row + 9, 8].Merge = true;//xóa
            worksheet.Cells[row + 9, 5, row + 9, 8].Style.VerticalAlignment = ExcelVerticalAlignment.Center;//canh giữa theo chiều dọc
            worksheet.Cells[row + 9, 5, row + 9, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;//canh giữa theo chiều ngang

            worksheet.Cells[string.Format("E{0}", row + 10)].Value = "Signed and stamped (Ký tên và đóng dấu) ";
            worksheet.Cells[row + 10, 5, row + 10, 8].Merge = true;//xóa
            worksheet.Cells[row + 10, 5, row + 10, 8].Style.VerticalAlignment = ExcelVerticalAlignment.Center;//canh giữa theo chiều dọc
            worksheet.Cells[row + 10, 5, row + 10, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;//canh giữa theo chiều ngang


            worksheet.Cells[string.Format("G{0}", row)].Value = Extension.Extension.ToVnd(cart.Sum(x => x.TotalMoney));
            worksheet.Cells[string.Format("G{0}", row + 1)].Value = "0 VNĐ";
            worksheet.Cells[string.Format("G{0}", row + 2)].Value = Extension.Extension.ToVnd(cart.Sum(x => x.TotalMoney));


            //worksheet.Column(row + 4).Style.WrapText = true;


            //worksheet.Cells[string.Format("A{0}", row+7)].Value = "Để biết thêm chi tiết, vui lòng liên hệ";
            //worksheet.Cells[string.Format("A{0}", row + 8)].Value = "Hotline: 0937 963 779 (8h00-21h30 hàng ngày)";
            //worksheet.Cells[string.Format("E{0}", row + 8)].Value = "NOVAZONE CHÂN THÀNH CẢM ƠN QUÝ KHÁCH";

            //worksheet.Cells[string.Format("E{0}", row + 8)].Style.Font.Bold = true;
            //worksheet.Cells[string.Format("E{0}", row + 8)].Style.Font.Size = 15;
            //worksheet.Cells[string.Format("E{0}", row + 8)].Style.Font.Color.SetColor(Color.Red);

            //worksheet.Cells[string.Format("H{0}", row + 2)].Value = Extension.Extension.ToVnd(cart.Sum(x => x.TotalMoney));
            //worksheet.Cells[string.Format("H{0}", row + 2)].Style.Font.Color.SetColor(Color.Red);


            // Tự động điều chỉnh độ rộng các cột
            //worksheet.Cells.AutoFitColumns();

            // Chuyển đổi thành mảng byte
            byte[] fileContents = package.GetAsByteArray();

            // Trả về file Excel dưới dạng phản hồi HTTP
            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "QUOTATION.xlsx");
        }


        [Route("/PaySuccess")]
        [HttpGet]
        public IActionResult PaySuccess()
        {
            return View();
        }

        public string GetNameProvince(int idlocation)
        {
            try
            {
                var location = _context.Provinces.AsNoTracking().SingleOrDefault(x => x.ProvinceId == idlocation);
                if (location != null)
                {
                    return location.ProvinceName;
                }
            }
            catch
            {
                return string.Empty;
            }
            return string.Empty;
        }
        public string GetNameDistrict(int idlocation)
        {
            try
            {
                var location = _context.Districts.AsNoTracking().SingleOrDefault(x => x.DistrictId == idlocation);
                if (location != null)
                {
                    return location.DistrictName;
                }
            }
            catch
            {
                return string.Empty;
            }
            return string.Empty;
        }
        public string GetNameWard(int idlocation)
        {
            try
            {
                var location = _context.Wards.AsNoTracking().SingleOrDefault(x => x.WardId == idlocation);
                if (location != null)
                {
                    return location.WardName;
                }
            }
            catch
            {
                return string.Empty;
            }
            return string.Empty;
        }

    }
}
