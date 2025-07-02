using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using WebShop.Areas.Admin.Models;
using WebShop.Extension;
using WebShop.Helpper;
using WebShop.Models;
using WebShop.ModelViews;

namespace WebShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminQuotationsController : Controller
    {
        private readonly DbMarketsContext _context;
        public INotyfService _notyfService { get; }

        public AdminQuotationsController(DbMarketsContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminQuotations
        public async Task<IActionResult> Index()
        {
            var quotations = await _context.Quotations.Include(q => q.Customer).ToListAsync();
            var customers = await _context.Customers.ToListAsync();
            ViewBag.Customers = customers;
            return View(quotations);
        }

        // GET: Admin/AdminQuotations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Quotations == null)
            {
                return NotFound();
            }

            var quotation = await _context.Quotations
                .Include(q => q.Customer)
                .FirstOrDefaultAsync(m => m.QuotationId == id);
            if (quotation == null)
            {
                return NotFound();
            }

            return View(quotation);
        }

        // GET: Admin/AdminQuotations/Create
        public IActionResult Create()
        {
            var customers =  _context.Customers.ToList();
            ViewBag.Customers = customers;
            return View();
        }

        // POST: Admin/AdminQuotations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("QuotationId,CustomerId,TotalMoney,Vat,CreatedAt,Confirmed")] Quotation quotation)
        {
            var baogia = HttpContext.Session.Get<List<CartItem>>("BaoGia");
            var customers = _context.Customers.ToList();

            if (ModelState.IsValid)
            {
                try
                {
                    //Lay ra gio hang de xu ly
                    var taikhoanID = HttpContext.Session.GetString("CustomerQT");
                    if (taikhoanID != null)
                    {
                        var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CustomerId == Convert.ToInt32(taikhoanID));
                        //Khoi tao bao gia

                        quotation.CustomerId = khachhang.CustomerId;
                        quotation.TotalMoney = Convert.ToInt32(baogia.Sum(x => x.TotalMoney));
                        quotation.CreatedAt = DateTime.Now;
                        quotation.Confirmed = false;
                        _context.Add(quotation);
                        _context.SaveChanges();


                        //tao chi tiet bao gia (Quotation Detail)
                        foreach (var item in baogia)
                        {
                            QuotationDetail quotationDetail = new QuotationDetail();
                            quotationDetail.QuotationId = quotation.QuotationId;
                            quotationDetail.ProductId = item.product.ProductId;
                            quotationDetail.Amount = item.amount;
                            quotationDetail.Price = item.product.SalePrice;
                            quotationDetail.CreatedAt = DateTime.Now;
                            _context.Add(quotationDetail);
                        }
                        _context.SaveChanges();
                        //clear gio hang
                        HttpContext.Session.Remove("BaoGia");
                        HttpContext.Session.Remove("CustomerQT");
                        //Xuat thong bao
                        _notyfService.Success("Tạo báo giá thành công");
                        //cap nhat thong tin khach hang
                        return RedirectToAction("Index");
                    }
                }
                catch(Exception)
                {
                    ViewBag.Customers = customers;
                }
            }
            ViewBag.Customers = customers;
            return View(quotation);
        }

        // GET: Admin/AdminQuotations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Quotations == null)
            {
                return NotFound();
            }

            var quotation = await _context.Quotations.FindAsync(id);
            if (quotation == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", quotation.CustomerId);
            return View(quotation);
        }

        // POST: Admin/AdminQuotations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuotationId,CustomerId,CompanyName,FullName,Address,SoDienThoai,TotalMoney,Vat,CreatedAt,Confirmed")] Quotation quotation)
        {
            if (id != quotation.QuotationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(quotation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuotationExists(quotation.QuotationId))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", quotation.CustomerId);
            return View(quotation);
        }

        // GET: Admin/AdminQuotations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Quotations == null)
            {
                return NotFound();
            }

            var quotation = await _context.Quotations
                .Include(q => q.Customer)
                .FirstOrDefaultAsync(m => m.QuotationId == id);
            if (quotation == null)
            {
                return NotFound();
            }

            return View(quotation);
        }

        // POST: Admin/AdminQuotations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Quotations == null)
            {
                return Problem("Entity set 'DbMarketsContext.Quotations'  is null.");
            }
            var quotation = await _context.Quotations.FindAsync(id);
            if (quotation != null)
            {
                _context.Quotations.Remove(quotation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuotationExists(int id)
        {
          return _context.Quotations.Any(e => e.QuotationId == id);
        }
        public List<CartItem> BaoGia
        {
            get
            {
                List<CartItem> pq = HttpContext.Session.Get<List<CartItem>>("BaoGia");
                if (pq == default(List<CartItem>))
                {
                    pq = new List<CartItem>();
                }
                return pq;
            }
        }

        [HttpPost]
        [Route("api/quotation/add")]
        public IActionResult AddToCart(int productID, int? amount)
        {
            List<CartItem> quotation = BaoGia;
            try
            {
                //Them san pham vao gio hang
                CartItem item = quotation.FirstOrDefault(p => p.product.ProductId == productID);
                if (item != null) // da co => cap nhat so luong
                {
                    item.amount += amount.Value;
                    //luu lai session
                    HttpContext.Session.Set("BaoGia", quotation);
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
                    quotation.Add(item);//Them vao gio
                }
                //Luu lai Session
                HttpContext.Session.Set("BaoGia", quotation);
                return Json(new { success = true });
            }
            catch(Exception)
            {
                return Json(new { success = false });
            }
        }
        [HttpPost]
        [Route("api/quotation/update")]
        public IActionResult UpdateCart(int? productID, int? amount, int? vat)
        {
            //Lay gio hang ra de xu ly
            var quotation = HttpContext.Session.Get<List<CartItem>>("BaoGia");
            try
            {
                if (quotation != null)
                {
                    CartItem item = quotation.SingleOrDefault(p => p.product.ProductId == productID);
                    if (item != null && amount.HasValue) // da co -> cap nhat so luong
                    {
                        item.amount = amount.Value;
                    }
                    //Luu lai session
                    HttpContext.Session.Set("BaoGia", quotation);
                }
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        [Route("api/quotation/remove")]
        public ActionResult Remove(int productID)
        {
            try
            {
                List<CartItem> quotation = BaoGia;
                CartItem item = quotation.SingleOrDefault(p => p.product.ProductId == productID);
                if (item != null)
                {
                    quotation.Remove(item);
                }
                //luu lai session
                HttpContext.Session.Set("BaoGia", quotation);
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        [Route("api/quotation/clear-quotation")]
        public IActionResult ClearQuotation()
        {
            // Xóa tất cả sản phẩm trong giỏ hàng
            // Ví dụ: Lưu trữ giỏ hàng trong Session, xóa Session để xóa giỏ hàng
            HttpContext.Session.Remove("BaoGia");
            HttpContext.Session.Remove("VAT");

            // Trả về kết quả thành công
            return Json(new { success = true });
        }
        [HttpPost]
        [Route("api/quotation/customerinfo")]
        public IActionResult CustomerInfo(int customerId)
        {
            var customer = _context.Customers
                .FirstOrDefault(x => x.CustomerId == customerId);
        
                var address = _context.AccountAddresses
                    .Include(x => x.Province)
                    .Include(x => x.District)
                    .Include(x => x.Ward)
                    .FirstOrDefault(x => x.CustomerId == customer.CustomerId && x.IsDefault == true);
            HttpContext.Session.SetString("CustomerQT", customer.CustomerId.ToString());
            ViewBag.Address = address;
            return PartialView("CustomerInfo", customer);
        }
        [HttpPost]
        [Route("api/quotation/vat")]
        public IActionResult ChangeVAT(string vat)
        {
            // Lưu giá trị VAT vào session
            HttpContext.Session.SetString("VAT", vat);
            //HttpContext.Session.Set("VAT", vat.ToString());
            return Json(new { success = true });
        }

        [Route("api/quotation/remove-customer")]
        public IActionResult RemoveCustomer(int customerId)
        {
            // Lưu giá trị VAT vào session
            HttpContext.Session.Remove("CustomerQT");
            //HttpContext.Session.Set("VAT", vat.ToString());
            return Json(new { success = true });
        }
        public IActionResult ExportQuotation(int? customerid)
        {
            // Lấy danh sách sản phẩm từ session
            var cart = HttpContext.Session.Get<List<CartItem>>("BaoGia");

            var vat = 0;

            string vatValue = HttpContext.Session.GetString("VAT");
            if (!string.IsNullOrEmpty(vatValue))
            {
                if (int.TryParse(vatValue, out int vatInt))
                {
                    vat = vatInt;
                }
            }
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
            worksheet.Cells["F5:H5"].Merge = true;
            worksheet.Cells["D6:H6"].Merge = true;


            worksheet.Cells["A7:H7"].Merge = true;
            worksheet.Row(7).Height = 40;

            worksheet.Cells["A8:H8"].Merge = true;
            worksheet.Cells[string.Format("A8:H8")].Style.Font.Bold = true;
            worksheet.Row(8).Height = 85;

            if (customerid == 0)
            {
                worksheet.Cells["A8"].Value = "To (Kính gửi)                        :\r\nAddress (Địa chỉ)               :\r\nEmail                                       :\r\nContact (Người liên hệ) :\r\nPhone (Số điện thoại)     :";
            }
            else
            {
                var customer = _context.Customers
                        .FirstOrDefault(x => x.CustomerId == customerid);

                var address = _context.AccountAddresses
                        .Include(x => x.Province)
                        .Include(x => x.District)
                        .Include(x => x.Ward)
                        .FirstOrDefault(x => x.CustomerId == customer.CustomerId && x.IsDefault == true);
                string customerAddress = "";
                if (address != null)
                {
                    customerAddress = address.Content + ", " + GetNameWard(address.Ward.WardId) + ", " + GetNameWard(address.District.DistrictId) + ", " + GetNameWard(address.Province.ProvinceId);
                }
                worksheet.Cells["A8"].Value = "To (Kính gửi)                        : " + customer.CompanyName + "\r\nAddress (Địa chỉ)               : " + customerAddress + " \r\nEmail                                       : " + customer.Email + " \r\nContact (Người liên hệ) : " + customer.FullName + " \r\nPhone (Số điện thoại)     : " + customer.SoDienThoai + "    ";
            }


            var cell = worksheet.Cells["A8:H8"];

            // Thiết lập loại đường viền và kiểu nét đứt
            cell.Style.Border.Top.Style = ExcelBorderStyle.Dotted;
            cell.Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;
            cell.Style.Border.Left.Style = ExcelBorderStyle.Dotted;
            cell.Style.Border.Right.Style = ExcelBorderStyle.Dotted;


            worksheet.Cells["A9:H9"].Merge = true;
            worksheet.Row(9).Height = 60;
            worksheet.Cells["A9"].Value = "We highly appreciate your interests in and keep working with our bussiness. Please find the below our quotation which we think serves you best in terms of price and liability. (Công ty laplopstore đánh giá cao sự quan tâm và hợp tác kinh doanh của Quí khách. Quí khách vui lòng xem bảng báo giá sau đây mà Công ty laplopstore cho rằng sẽ đáp ứng Quí khách tốt nhất trong bảng báo giá và trách nhiệm).";

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
            worksheet.Cells["D1"].Value = "CÔNG TY TNHH laplopstore";
            worksheet.Cells["D2"].Value = "Địa chỉ: 11A Hồng Hà, Phường 2, Quận Tân Bình, TP. HCM";
            worksheet.Cells["D3"].Value = "Tel: 0848967333 ";
            worksheet.Cells["D4"].Value = "Email: hello@laplopstore.com.vn - Website: www.laplopstore.co";

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
            mergedCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

            worksheet.Cells["A11:H11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells["A11:H11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

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
            if (vat != null)
            {
                worksheet.Cells[string.Format("A{0}", row + 1)].Value = "VAT (" + vat + "%) (Thuế GTGT "+ vat + "%)";
            }
            else
            {
                worksheet.Cells[string.Format("A{0}", row + 1)].Value = "VAT (%) (Thuế GTGT %)";
            }
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

            worksheet.Cells[string.Format("A{0}", row + 6)].Value = "CÔNG TY TNHH laplopstore\nSố TK: 6927977777 Ngân Hàng TMCP QUÂN ĐỘI MB BANK - CN HCM \nSố TK : Trương Quang Viên -  9273437777 Ngân Hàng Vietcombank -  CN Vạn Phúc ";
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



            worksheet.Cells[string.Format("A{0}", row + 8)].Value = "laplopstore hy vọng tiếp tục cung cấp các dịch vụ thỏa đáng đến Quí Khách";
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
            worksheet.Cells[row + 10, 5, row + 10, 8].Style.VerticalAlignment = ExcelVerticalAlignment.Top;//canh lên trên theo chiều dọc
            worksheet.Cells[row + 10, 5, row + 10, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;//canh giữa theo chiều ngang


            worksheet.Cells[string.Format("G{0}", row)].Value = Extension.Extension.ToVnd(cart.Sum(x => x.TotalMoney));
            if(vat != null)
            {
                var VAT = cart.Sum(x => x.TotalMoney) * Convert.ToInt32(vat) / 100;
                var totalVat = cart.Sum(x => x.TotalMoney) + VAT;
                worksheet.Cells[string.Format("G{0}", row + 1)].Value = Extension.Extension.ToVnd((double)VAT);
                worksheet.Cells[string.Format("G{0}", row + 2)].Value = Extension.Extension.ToVnd((double)totalVat);
            }
            else
            {
                worksheet.Cells[string.Format("G{0}", row + 1)].Value = "-";
                worksheet.Cells[string.Format("G{0}", row + 2)].Value = Extension.Extension.ToVnd(cart.Sum(x => x.TotalMoney));
            }


            //worksheet.Column(row + 4).Style.WrapText = true;


            //worksheet.Cells[string.Format("A{0}", row+7)].Value = "Để biết thêm chi tiết, vui lòng liên hệ";
            //worksheet.Cells[string.Format("A{0}", row + 8)].Value = "Hotline: 0937 963 779 (8h00-21h30 hàng ngày)";
            //worksheet.Cells[string.Format("E{0}", row + 8)].Value = "laplopstore CHÂN THÀNH CẢM ƠN QUÝ KHÁCH";

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
            return new FileStreamResult(new MemoryStream(fileContents), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = "QUOTATION.xlsx"
            };
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
