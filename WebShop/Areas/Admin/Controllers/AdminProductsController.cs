using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AspNetCore;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NPOI.HPSF;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NuGet.Protocol.Plugins;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Org.BouncyCastle.Asn1.Cms;
using PagedList.Core;
using WebShop.Areas.Admin.Models;
using WebShop.Extension;
using WebShop.Helpper;
using WebShop.Models;
using WebShop.ModelViews;
using OfficeOpenXml;
namespace WebShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminProductsController : Controller
    {
        private readonly DbMarketsContext _context;
        public INotyfService _notyfService { get; }
        public IWebHostEnvironment _webHost;
        public AdminProductsController(DbMarketsContext context, INotyfService notyfService, IWebHostEnvironment webHost)
        {
            _context = context;
            _notyfService = notyfService;
            _webHost = webHost;
        }
        // GET: Admin/AdminProducts
        public IActionResult Index(int CatID = 0)
        {
            var productPageVM = new AdminProductPageVM();
            var query = _context.Products
                .Include(c => c.ProductCategories)
                .ThenInclude(c => c.Cat)
                .AsNoTracking()
                .ToList();

            var checkboxProduct = new CheckboxProduct
            {
                ProductCode = HttpContext.Session.Get<bool?>("ProductCode") ?? false,
                ProductInfo = HttpContext.Session.Get<bool?>("ProductInfo") ?? true,
                InputPrice = HttpContext.Session.Get<bool?>("InputPrice") ?? true,
                SalePrice = HttpContext.Session.Get<bool?>("SalePrice") ?? true,
                UnitsInStock = HttpContext.Session.Get<bool?>("UnitsInStock") ?? true,
                Active = HttpContext.Session.Get<bool?>("Active") ?? true,
                DateCreated = HttpContext.Session.Get<bool?>("DateCreated") ?? false,
            };
            var check = HttpContext.Session.GetString("ProductView");
            if (check == "Full")
            {
                query = query.ToList();
            }
            if (check == "Active")
            {
                query = query.Where(i => i.Active == true).ToList();
            }
            if (check == "Stock")
            {
                query = query.Where(i => i.UnitsInStock == 0).ToList();
            }
            if (check == "Off")
            {
                query = query.Where(i => i.Active == false).ToList();
            }
            var proOption = _context.Products.Where(c => c.ProductCode == c.ProductOption).ToList();
            ViewBag.proOption = proOption;
            ViewBag.AllCategories = _context.Categories.Where(a => a.Published == true).ToList();
            var SaveExcel = HttpContext.Session.GetString("SaveExcel");
            ViewBag.SaveExcel = SaveExcel;
            ViewBag.check = check;
            ViewBag.CurrentCateID = CatID;
            productPageVM.CheckboxProduct = checkboxProduct;
            productPageVM.Products = query;

            return View(productPageVM);
        }
        // get sản phẩm view load
        public IActionResult loadpro2(int id)
        {
            var product = _context.Products
              .Include(b => b.ProductCategories)
              .ThenInclude(cb => cb.Cat)
              .FirstOrDefault(b => b.ProductId == id);
            var selectedCategories = product.ProductCategories.Select(cb => cb.Cat).ToList();
            List<ProductCategory> productsCate = new List<ProductCategory>();
            foreach (var cate in selectedCategories)
            {
                var pro = _context.ProductCategories
                    .Where(c => c.CatId == cate.CatId)
                    .Include(p => p.Product)
                    .Where(c => c.Product.ProductId != id)
                    .ToList();
                productsCate.AddRange(pro);
            }
            HttpContext.Session.Set<List<ProductCategory>>("listsp", productsCate);
            var procate = HttpContext.Session.Get<List<ProductCategory>>("listsp");
            return Json(procate);
        }
        // sanr pham tuong tu
        public IActionResult updateoption()
        {
            var product = _context.Products.ToList();
            foreach (var item in product)
            {
                item.ProductOption = null;
                _context.Update(item);
                _context.SaveChanges();
            }
            return Ok();
        }

        // deleteitemoption
        public IActionResult deleteitemoption(int id)
        {
            var success = false;
            var product = _context.Products.Where(c => c.ProductId == id).FirstOrDefault();
            if (product != null)
            {
                product.ProductOption = null;
                try
                {
                    _context.Update(product);
                    _context.SaveChanges();
                    success = true;
                }
                catch (Exception ex) { }
            }
            return Json(new { success = success });
        }
        [HttpPost]
        public IActionResult AddOption(int id, int iditem)
        {
            var success = false;
            var product = _context.Products.Where(c => c.ProductId == id).FirstOrDefault();
            if (product != null)
            {
                var proitem = _context.Products.Where(c => c.ProductId == iditem).FirstOrDefault();
                if (proitem != null)
                {
                    try
                    {
                        if (product.ProductOption == null)
                        {
                            product.ProductOption = product.ProductCode;
                            _context.Update(product);
                            _context.SaveChanges();
                        }
                        proitem.ProductOption = product.ProductCode;
                        _context.Update(proitem);
                        _context.SaveChanges();
                        success = true;

                    }
                    catch (Exception ex)
                    {
                        success = false;
                    }
                }
            }
            var pro = _context.Products.Where(p => p.ProductOption == product.ProductCode).ToList();
            return Json(new { success = success, data = pro });
        }
        public IActionResult DeleteOption(int iditem)
        {
            var success = false;
            var code = "";
            var proitem = _context.Products.Where(c => c.ProductId == iditem).FirstOrDefault();
            if (proitem != null)
            {
                try
                {
                    code = proitem.ProductOption;
                    proitem.ProductOption = null;
                    _context.Update(proitem);
                    _context.SaveChanges();
                    success = true;

                }
                catch (Exception ex)
                {
                    success = false;
                }
            }
            var pro = _context.Products.Where(p => p.ProductOption == code).ToList();

            return Json(new { success = success, data = pro });
        }

        public IActionResult DeleteTopOption(int id)
        {
            var success = false;
            var code = "";
            string codeconver = "";
            var product = _context.Products.Where(c => c.ProductId == id).FirstOrDefault();
            if (product != null)
            {
                try
                {
                    code = product.ProductOption;
                    product.ProductOption = null;
                    _context.Update(product);
                    _context.SaveChanges();

                    var lstpro = _context.Products.Where(p => p.ProductOption == code).Where(c => c.ProductOption != null).ToList();
                    var i = 0;
                    foreach (var item in lstpro)
                    {
                        if (i < 1)
                        {
                            codeconver = item.ProductCode;
                        }
                        item.ProductOption = codeconver;
                        _context.Update(item);
                        _context.SaveChanges();
                        i++;
                    }
                    success = true;
                }
                catch (Exception ex) { }
            }

            return Json(new { success = success });
        }
        // xóa section them sản phẩm
        public IActionResult sectionadd()
        {
            HttpContext.Session.Remove("SaveExcel");
            return Ok();
        }



        public string[] textExcel()
        {
            string[] text1 = {  "Danh mục(...)", "Tên sản phẩm","Thương hiệu","Nhà cung cấp","Tồn kho","Kho hàng", "Mã sản phẩm", "Đặc điểm nổi bật","Hình ảnh", "Quà tặng",
                                "Thời gian bảo hành(tháng)","Mô tả bảo hành","Giá niêm yết","Giá bán",
                                "Best Seller","Xuất hiện ở trang chủ","Công khai","Tiêu đề SEO",
                                "Mô tả SEO","Từ khóa SEO",
                                "Bắt buộc nhập.\nĐiền chính xác hoặc copy đúng tên danh mục ở tab List danh mục cột \"Tên danh mục\".",
                                "Tối đa 120 ký tự, viết hoa chữ cái đầu tiên và tên thương hiệu.",
                                "Điền chính xác hoặc copy tên thương hiệu từ tab List danh mục + Thương hiệu  cột Tên thương hiệu",
                                "Điền chính xác hoặc copy tên nhà cung cấp từ tab List danh mục + nhà cung cấp  cột Nhà cung cấp, nếu nhà cung cấp mới vui lòng điền thông tin mới nhất và cập nhật lại",
                                "Bắt buộc nhập\nSố lượng tối đa mà khách hàng có thể đặt mua sản phẩm này. ",
                                "Nhập kho hàng tiện ý việc quản lý sản phẩm và liên hệ",
                                "Mã sản phẩm không được trùng nhau.\nTối đa 50 ký tự, bao gồm chữ cái, chữ số",
                                "Các thông tin nổi bật nhất của sản phẩm.\nThường từ 3~8 dòng, mỗi dòng là 1 câu ngắn gọn.\nKHÔNG đặt các ký tự đặc biệt ở đầu mỗi dòng (vd: -, +, *, ...)",
                                "Các link hình ảnh cách nhau bởi dấu phảy (\",\")\r\nLink đầu tiên sẽ là ảnh đại diện",
                                "Các thông tin quà tặng của sản phẩm.\nThường từ 1 ~ 5 dòng, mỗi dòng là 1 câu ngắn gọn.\nKHÔNG đặt các ký tự đặc biệt ở đầu mỗi dòng (vd: -, +, *, ...)",
                                "Nhập vào số tháng bảo hành\nVí dụ: Sản phẩm bảo hành 3 năm nhập số 36",
                                "Nếu sản phẩm không được bảo hành thì cần ghi rõ \"Sản phẩm không hỗ trợ bảo hành\"",
                                "Giá nhập của sản phẩm",
                                "Giá bán sản phẩm",
                                "Nếu muốn sản phẩm nằm trong danh sách BestSeller thì điền \"True\" ngược lại điền \"False\"",
                                "Nếu muốn sản phẩm xuất hiện ở Home Page thì điền \"True\" ngược lại điền \"False\"",
                                "Nếu muốn sản phẩm này được đăng bán công khai thì điền \"True\" ngược lại điền \"False\". \r\nNếu là False thì sản phẩm vẫn được thêm nhưng sẽ bị ẩn",
                                "Điền tiêu đề SEO cho sản phẩm",
                                "Điền mô tả SEO cho sản phẩm",
                                "Điền các từ khóa SEO cho sản phẩm.\r\nMỗi từ khóa cách nhau bởi dấy phảy (,)"
                                 };

            return text1;
        }


        // xuat file excel theo phan loai
        [HttpPost]
        public IActionResult exportExcel(int[] listCategoty)
        {
            using ExcelPackage package = new ExcelPackage();
            // Tạo một worksheet mới trong package
            var worksheet = package.Workbook.Worksheets.Add("Mẫu sản phẩm");
            var worksheet2 = package.Workbook.Worksheets.Add("Thông tin kèm theo");
            // phần dau trang execl
            var text = textExcel();
            int j = 1;
            int i = 1;
            for (int k = 0; k < text.Length; k++)
            {
                worksheet.Cells[i, j].Value = text[k];
                var mergedCell = worksheet.Cells[i, j];
                mergedCell.Style.WrapText = true;
                mergedCell.Style.Font.Bold = true;
                mergedCell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                mergedCell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                mergedCell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                mergedCell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                if (i > 1)
                {
                    mergedCell.Style.Font.Color.SetColor(Color.Red);

                }
                j++;
                if (j > 20)
                {
                    j = 1;
                    i++;
                }
            }

            // loat ten thuong hieu file excel

            var rowsheet2 = 1;
            worksheet2.Cells[rowsheet2, 1].Value = "Tên thương hiệu";
            worksheet2.Cells[rowsheet2, 2].Value = "Tên nhà cung cấp";
            worksheet2.Cells[rowsheet2, 3].Value = "Tên danh mục";
            worksheet2.Cells[rowsheet2, 4].Value = "Chi tiết danh mục";

            // them thuong hiệu
            var brand = _context.Brands.ToList();
            var columb = 2;
            foreach (var item in brand)
            {
                worksheet2.Cells[columb, 1].Value = item.BrandName;
                columb++;
            }
            // them bên nhà cung cấp
            var custo = _context.CustomerSuppliers.ToList();
            var columC = 2;
            foreach (var item in custo)
            {
                worksheet2.Cells[columC, 2].Value = item.Name;
                columC++;
            }
            // thêm cate
            var cate = 2;
            foreach (var item in listCategoty)
            {
                var textPrice = "";
                // lay cate
                var category = _context.Categories
                    .Where(c => c.CatId == item)
                    .Include(c => c.CategoryAttributes)

                    .FirstOrDefault();

                //cate acctribui
                var catep = _context.CategoryAttributes
                    .Where(c => c.CatId == category.CatId)
                    .OrderBy(c => c.Attribute.Ordering)
                    .ToList();
                // cot cate excel
                worksheet2.Cells[cate, 3].Value = category.CatName;
                if (catep != null)
                {
                    foreach (var itemC in catep)
                    {
                        textPrice += itemC.Name + ":";
                        var atteP = _context.AttributesPrices
                            .Where(p => p.ProductId == null)
                            .Where(i => i.AttributeId == itemC.AttributeId)
                            .Include(a => a.Attribute)
                            .OrderBy(a => a.Attribute.Ordering)
                            .ToList();
                        if (atteP != null)
                        {
                            foreach (var attritem in atteP)
                            {
                                textPrice += attritem.Price + ", ";
                            }
                            textPrice += "\r\n";
                        }
                    }
                    // noi dung chi tiec
                    worksheet2.Cells[cate, 4].Value = textPrice;
                    worksheet2.Cells[cate, 4].Style.WrapText = true;
                }
                cate++;

            }

            // cong thuc tren ban 1          

            worksheet.Cells[3, 1].Value = "='Thông tin kèm theo'!$C$2:$C$" + (cate - 1) + "";
            worksheet.Cells[3, 3].Value = "='Thông tin kèm theo'!$A$2:$A$" + (columb - 1) + "";
            worksheet.Cells[3, 4].Value = "='Thông tin kèm theo'!$B$2:$B$" + (columC - 1) + "";
            worksheet.Cells[3, 8].Value = "\r=VLOOKUP(A3,'Thông tin kèm theo'!$C$1:$D$" + (cate - 1) + ",IF(A3<>\"\"," + 2 + ",\"\"),FALSE)";
            worksheet.Cells[3, 11].Value = "36";
            worksheet.Cells[3, 12].Value = "Bảo hành tận nơi";
            //Save the file
            byte[] fileContents = package.GetAsByteArray();
            // Trả về file Excel dưới dạng phản hồi HTTP
            return new FileStreamResult(new MemoryStream(fileContents), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = "QUOTATION.xlsx"
            };

        }
        public IActionResult setionclick(int id)
        {
            if (id == 0)
            {
                HttpContext.Session.SetString("ProductView", "Full");
            }
            if (id == 1)
            {
                HttpContext.Session.SetString("ProductView", "Active");
            }
            if (id == 2)
            {
                HttpContext.Session.SetString("ProductView", "Stock");
            }
            if (id == 3)
            {
                HttpContext.Session.SetString("ProductView", "Off");
            }

            return Json(new { success = true });
        }


        public IActionResult Filtter(int CatID = 0)
        {
            var url = $"/Admin/AdminProducts?CatID={CatID}";

            if (CatID == 0)
            {
                url = $"/Admin/AdminProducts";
            }
            return Json(new { status = "success", redirectUrl = url });
        }
        public IActionResult Filtter2(int Option = 0, int CatID = 0)
        {
            var url = $"/Admin/AdminProducts?CatID={CatID}?Option={Option}";

            if (CatID == 0 && Option == 0)
            {
                url = $"/Admin/AdminProducts";
            }

            return Json(new { status = "success", redirectUrl = url });
        }

        // GET: Admin/AdminProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        // GET: Admin/AdminProducts/Create
        public IActionResult Create()
        {

            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CatId", "CatName");
            ViewData["Hang"] = new SelectList(_context.Brands, "BrandId", "BrandName");
            var lsAttribute = _context.Attributes.Include(x => x.AttributesPrices).ToList();
            ViewBag.lsAttribute = lsAttribute;
            return View();
        }

        // POST: Admin/AdminProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminProductVM adminProductVM, List<IFormFile> productImages)
        {
            var lsAttribute = _context.Attributes.Include(x => x.AttributesPrices).ToList();

            if (ProductExists(adminProductVM.ProductCode))
            {
                ModelState.AddModelError("ProductCode", "Mã sản phẩm đã tồn tại");
                ViewData["DanhMuc"] = new SelectList(_context.Categories, "CatId", "CatName");
                ViewData["Hang"] = new SelectList(_context.Brands, "BrandId", "BrandName", adminProductVM.BrandId);
                ViewBag.lsAttribute = lsAttribute;
                return View(adminProductVM);
            }
            if (ModelState.IsValid)
            {
                Product product = new Product
                {
                    ProductCode = adminProductVM.ProductCode,
                    ProductName = Utilities.ToTitleCase(adminProductVM.ProductName),
                    BrandId = adminProductVM.BrandId,
                    Alias = Utilities.SEOUrl(adminProductVM.ProductName),
                    Gift = adminProductVM.Gift,
                    ShortDesc = adminProductVM.ShortDesc,
                    Description = adminProductVM.Description,
                    ConfigInformation = adminProductVM.ConfigInformation,
                    Warranty = adminProductVM.Warranty,
                    WarrantyNote = adminProductVM.WarrantyNote,
                    Video = adminProductVM.Video,
                    BestSellers = adminProductVM.BestSellers,
                    HomeFlag = adminProductVM.HomeFlag,
                    Active = adminProductVM.Active,
                    Title = adminProductVM.Title,
                    MetaDesc = adminProductVM.MetaDesc,
                    MetaKey = adminProductVM.MetaKey,
                    UnitsInStock = adminProductVM.UnitsInStock,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                };
                if (adminProductVM.SalePrice != null)
                {
                    product.SalePrice = adminProductVM.SalePrice;
                    product.Price = adminProductVM.Price;
                }
                else
                {
                    product.Price = 0;
                    product.SalePrice = 0;
                }
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                // Lấy danh sách danh mục
                List<string> productCategories = Request.Form["productCategories"].ToList();

                // Thêm bảng ghi productCategory
                foreach (var value in productCategories)
                {
                    //Thêm bảng ghi mới
                    var newproductCategory = new ProductCategory
                    {
                        CatId = Convert.ToInt32(value),
                        ProductId = product.ProductId
                    };
                    _context.ProductCategories.Add(newproductCategory);
                    await _context.SaveChangesAsync();
                }
                _notyfService.Success("Thêm mới thành công");
                return RedirectToAction(nameof(Edit), new { id = product.ProductId, curPage = 1 });
            }
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CatId", "CatName");

            ViewData["Hang"] = new SelectList(_context.Brands, "BrandId", "BrandName", adminProductVM.BrandId);
            ViewBag.lsAttribute = lsAttribute;
            return View(adminProductVM);
        }

        //GET: Admin/AdminProducts/Edit/5
        public async Task<IActionResult> Edit(int? id, int curPage)
        {
            var currentPage = curPage;

            // Lấy sản phẩm từ cơ sở dữ liệu dựa trên Id         
            var product = _context.Products
                .Include(b => b.ProductCategories)
                .ThenInclude(cb => cb.Cat)
                .FirstOrDefault(b => b.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }
            // Ánh xạ dữ liệu từ Product sang ProductViewModel
            AdminProductVM productViewModel = new AdminProductVM
            {
                ProductId = product.ProductId,
                ProductCode = product.ProductCode,
                ProductName = Utilities.ToTitleCase(product.ProductName),
                BrandId = product.BrandId,
                Alias = Utilities.SEOUrl(product.ProductName),
                ShortDesc = product.ShortDesc,
                Gift = product.Gift,
                Description = product.Description,
                ConfigInformation = product.ConfigInformation,
                Warranty = product.Warranty,
                WarrantyNote = product.WarrantyNote,
                Price = product.Price,
                SalePrice = product.SalePrice,
                Video = product.Video,
                BestSellers = product.BestSellers,
                HomeFlag = product.HomeFlag,
                Active = product.Active,
                Title = product.Title,
                MetaDesc = product.MetaDesc,
                MetaKey = product.MetaKey,
                UnitsInStock = product.UnitsInStock,
                ProductOption = product.ProductOption,
                DateCreated = DateTime.Now
            };

            // Lấy danh sách các danh mục đã được chọn cho thương hiệu
            var selectedCategories = product.ProductCategories.Select(cb => cb.Cat).ToList();

            List<ProductCategory> productsCate = new List<ProductCategory>();
            foreach (var cate in selectedCategories)
            {
                var pro = _context.ProductCategories
                    .Where(c => c.CatId == cate.CatId)
                    .Include(p => p.Product)
                    .ThenInclude(c => c.Brand)
                    .Where(c => c.Product.ProductId != id)
                    .ToList();
                productsCate.AddRange(pro);
            }



            // Lấy danh sách các danh mục từ cơ sở dữ liệu
            var allCategories = await _context.Categories.ToListAsync();

            var atts = _context.Categories
                .Join(_context.ProductCategories,
                    c => c.CatId,
                    pc => pc.CatId,
                    (c, pc) => new { Category = c, ProductCategory = pc })
                .Where(x => x.ProductCategory.ProductId == product.ProductId)
                .SelectMany(x => x.Category.CategoryAttributes)
                .Include(ca => ca.Attribute.AttributesPrices)
                .Select(ca => ca.Attribute)
                .Distinct()
                .ToList();

            // Lấy danh sách các thuộc tính đã chọn
            var attributeIds = atts.Select(a => a.AttributeId).ToList();
            var selectedPrice = _context.AttributesPrices
                .Where(ap => attributeIds.Contains((int)ap.AttributeId) && ap.ProductId == product.ProductId)
                .ToList();

            var lsImage = _context.ProductThumbs.Where(x => x.ProductId == product.ProductId).ToList();

            ViewData["Hang"] = new SelectList(_context.Brands, "BrandId", "BrandName");
            ViewBag.AllCategories = allCategories;
            ViewBag.SelectedCategories = selectedCategories;
            ViewBag.lsAttribute = atts;
            ViewBag.SelectedPrice = selectedPrice;
            ViewBag.lsImage = lsImage;
            ViewBag.CurrentPage = currentPage;
            ViewBag.productsCate = productsCate;
            return View(productViewModel);
        }
        // POST: Admin/AdminProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int curPage, AdminProductVM adminProductVM, List<IFormFile> productImages)
        {
            var currentPage = curPage;

            Product product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    product.ProductName = Utilities.ToTitleCase(adminProductVM.ProductName);
                    product.BrandId = adminProductVM.BrandId;
                    product.ProductCode = adminProductVM.ProductCode;
                    product.Alias = Utilities.SEOUrl(product.ProductName);
                    product.ShortDesc = adminProductVM.ShortDesc;
                    product.Gift = adminProductVM.Gift;
                    product.Description = adminProductVM.Description;
                    product.ConfigInformation = adminProductVM.ConfigInformation;
                    product.Warranty = adminProductVM.Warranty;
                    product.WarrantyNote = adminProductVM.WarrantyNote;
                    product.Price = adminProductVM.Price;
                    product.SalePrice = adminProductVM.SalePrice;
                    product.Video = adminProductVM.Video;
                    product.BestSellers = adminProductVM.BestSellers;
                    product.HomeFlag = adminProductVM.HomeFlag;
                    product.Active = adminProductVM.Active;
                    product.Title = adminProductVM.Title;
                    product.MetaDesc = adminProductVM.MetaDesc;
                    product.MetaKey = adminProductVM.MetaKey;
                    product.UnitsInStock = adminProductVM.UnitsInStock;


                    product.DateModified = DateTime.Now;
                    _context.Update(product);
                    await _context.SaveChangesAsync();

                    //Xóa các bảng ghi attributeprice cũ
                    var attributePrice = _context.AttributesPrices
                           .AsNoTracking()
                           .Where(x => x.ProductId == id).ToList();

                    _context.AttributesPrices.RemoveRange(attributePrice);

                    //Xóa các bảng ghi productcategory cũ
                    var productCategories = _context.ProductCategories
                           .AsNoTracking()
                           .Where(x => x.ProductId == id).ToList();

                    _context.ProductCategories.RemoveRange(productCategories);

                    // Lấy danh sách danh mục
                    List<string> cats = Request.Form["productCategories"].ToList();
                    // Thêm bảng ghi categorybrand
                    foreach (var value in cats)
                    {
                        //Thêm bảng ghi mới
                        var newproductCategory = new ProductCategory
                        {
                            CatId = Convert.ToInt32(value),
                            ProductId = product.ProductId
                        };
                        _context.ProductCategories.Add(newproductCategory);
                        await _context.SaveChangesAsync();
                    }

                    // Lấy danh sách giá trị thuộc tính từ form
                    List<string> attributeValues = Request.Form["attributeValues"].ToList();

                    // Thêm giá trị thuộc tính cho sản phẩm
                    foreach (var value in attributeValues)
                    {
                        // Lấy bản ghi AttributeValue cần cập nhật
                        var attributeValue = _context.AttributesPrices.FirstOrDefault(av => av.AttributesPriceId == Convert.ToInt32(value));
                        if (attributeValue != null)
                        {
                            //Thêm bảng ghi mới
                            var newAttributeValue = new AttributesPrice
                            {
                                AttributeId = attributeValue.AttributeId,
                                ProductId = product.ProductId,
                                Price = attributeValue.Price,
                                Active = true
                            };
                            _context.AttributesPrices.Add(newAttributeValue);
                            await _context.SaveChangesAsync();
                        }
                    }
                    _notyfService.Success("Cập nhật thành công");
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductCode))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Edit), new { id = product.ProductId, curPage = currentPage });
            }
            // Lấy danh sách các danh mục đã được chọn cho thương hiệu
            var selectedCategories = product.ProductCategories.Select(cb => cb.Cat).ToList();

            // Lấy danh sách các danh mục từ cơ sở dữ liệu
            var allCategories = await _context.Categories.ToListAsync();

            var atts = _context.Categories
                .Join(_context.ProductCategories,
                    c => c.CatId,
                    pc => pc.CatId,
                    (c, pc) => new { Category = c, ProductCategory = pc })
                .Where(x => x.ProductCategory.ProductId == product.ProductId)
                .SelectMany(x => x.Category.CategoryAttributes)
                .Include(ca => ca.Attribute.AttributesPrices)
                .Select(ca => ca.Attribute)
                .Distinct()
                .ToList();

            // Lấy danh sách các thuộc tính đã chọn
            var attributeIds = atts.Select(a => a.AttributeId).ToList();
            var selectedPrice = _context.AttributesPrices
                .Where(ap => attributeIds.Contains((int)ap.AttributeId) && ap.ProductId == product.ProductId)
                .ToList();

            var lsImage = _context.ProductThumbs.Where(x => x.ProductId == product.ProductId).ToList();

            ViewData["Hang"] = new SelectList(_context.Brands, "BrandId", "BrandName");
            ViewBag.AllCategories = allCategories;
            ViewBag.SelectedCategories = selectedCategories;
            ViewBag.lsAttribute = atts;
            ViewBag.SelectedPrice = selectedPrice;
            ViewBag.lsImage = lsImage;
            ViewBag.CurrentPage = currentPage;
            return View(adminProductVM);
        }

        [HttpPost]
        public async Task<IActionResult> UploadProductImage(int productId, List<IFormFile> productImages)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return RedirectToAction(nameof(Edit), new { id = product.ProductId });

            }
            if (productImages == null || productImages.Count == 0)
            {
                return RedirectToAction(nameof(Edit), new { id = product.ProductId });
            }
            //Lưu hình ảnh sản phẩm
            if (productImages != null)
            {
                var count = 0;
                var countImage = _context.ProductThumbs.Where(x => x.ProductId == product.ProductId).Count();
                count = countImage + 1;
                foreach (var item in productImages)
                {
                    string extension = Path.GetExtension(item.FileName);
                    string image = Utilities.SEOUrl(product.ProductName) + "-" + count + extension;
                    var imageUrl = await Utilities.UploadFile(item, @"products", image.ToLower());
                    // Lưu thông tin hình ảnh vào bảng ProductImages
                    var productThumb = new ProductThumb
                    {
                        ProductId = product.ProductId,
                        Alias = imageUrl
                    };
                    count++;
                    _context.ProductThumbs.Add(productThumb);
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Edit), new { id = product.ProductId, curPage = 1 });
        }

        [HttpPost]
        public IActionResult uploadlist(int productId, string imagelist)
        {
            var text = "No";
            try
            {
                List<string> stringList = imagelist.Split(',').ToList();
                var count = 0;
                var countImage = _context.ProductThumbs.Where(x => x.ProductId == productId).Count();
                var product = _context.Products.Where(x => x.ProductId == productId).FirstOrDefault();
                count = countImage + 1;
                if (product != null)
                {
                    foreach (string str in stringList)
                    {
                        var productThumb = new ProductThumb
                        {
                            ProductId = productId,
                            Alias = str
                        };
                        if (count == 1)
                        {
                            productThumb.IsMain = true;
                            product.Avatar = str;
                        }
                        count++;
                        try
                        {
                            _context.ProductThumbs.Add(productThumb);
                            _context.SaveChanges();
                            text = "OK";
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                text = "No";
            }
            return Json(text);
        }
        [HttpPost]
        public ActionResult DeleteImage(int imageId)
        {
            try
            {
                // Xóa hình ảnh từ cơ sở dữ liệu
                var image = _context.ProductThumbs.Find(imageId);
                if (image != null)
                {
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products", image.Alias);
                    Utilities.RemoveFile(imagePath);
                    _context.ProductThumbs.Remove(image);
                    _context.SaveChanges();

                    return Json(new { success = true, message = "Hình ảnh đã được xóa thành công!" });
                }
                else
                {
                    return Json(new { success = false, message = "Không tìm thấy hình ảnh!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi xóa hình ảnh: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult DeleteAllAvatar(int productId)
        {
            try
            {
                // Lấy danh sách hình ảnh dựa trên productID
                var images = _context.ProductThumbs.Where(x => x.ProductId == productId).ToList();
                // Xóa hình ảnh sản phẩm trên server
                foreach (var image in images)
                {
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products", image.Alias);
                    Utilities.RemoveFile(imagePath);
                }
                // Xóa hình ảnh trong db
                if (images.Any())
                {
                    // Xóa tất cả hình ảnh
                    _context.ProductThumbs.RemoveRange(images);
                    _context.SaveChanges();
                    _notyfService.Success("Xóa thành công");
                    return Json(new { success = true, message = "Hình ảnh đã được xóa thành công!" });

                }
                else
                {
                    return Json(new { success = false, message = "Không tìm thấy hình ảnh!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi xóa hình ảnh: " + ex.Message });
            }
        }
        public async Task<IActionResult> SetAvatar(int? ImageId, int? ProductId)
        {
            if (ImageId == null)
            {
                return NotFound();
            }
            Product product = _context.Products.Find(ProductId);
            var images = await _context.ProductThumbs
                .Where(m => m.ProductId == ProductId).ToListAsync();
            if (images != null)
            {
                foreach (var image in images)
                {
                    if (image.ImageId == ImageId)
                    {
                        image.IsMain = true;
                        image.Ordering = 1;
                        product.Avatar = image.Alias;
                    }
                    else
                    {
                        image.IsMain = false;
                    }
                }
                // Lưu thay đổi vào cơ sở dữ liệu
                _context.SaveChanges();
            }
            return Json(new { success = true, imageId = ImageId });

        }
        // GET: Admin/AdminProducts/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = await _context.Products
        //        .FirstOrDefaultAsync(m => m.ProductId == id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(product);
        //}
        // POST: Admin/AdminProducts/Delete/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteProduct(int id)
        {
            var product = _context.Products.Where(x => x.ProductId == id).FirstOrDefault();
            try
            {
                if (product == null)
                {
                    // Xử lý khi không tìm thấy sản phẩm
                    return Json(new { success = false, message = "Không tìm thấy sản phẩm!" });
                }
                else
                {
                    // Lấy danh sách các hình ảnh sản phẩm
                    string folderPath = Path.Combine(_webHost.WebRootPath, "images", "Product", product.ProductId.ToString()); //thư mục Id sản phẩm
                    Directory.Delete(folderPath, true);

                    try
                    {
                        var images = _context.ProductThumbs.Where(x => x.ProductId == product.ProductId).ToList();
                        _context.ProductThumbs.RemoveRange(images);

                    }
                    catch (Exception ex) { }


                    // xoa san pham kien ket voi nhaf cung cap
                    try
                    {
                        var productcus = _context.ProductAddCusPros.Where(p => p.ProductId == product.ProductId).FirstOrDefault();

                        _context.ProductAddCusPros.Remove(productcus);
                        _context.SaveChanges();

                    }
                    catch (Exception ex) { }

                    try
                    {

                        // Lấy danh sách các thuộc tính
                        var attributes = _context.AttributesPrices.Where(x => x.ProductId == product.ProductId).ToList();

                        // Xóa các thuộc tính
                        _context.AttributesPrices.RemoveRange(attributes);
                    }
                    catch (Exception ex) { }


                    try
                    {

                        // Lấy danh sách các product category
                        var productCategory = _context.ProductCategories.Where(x => x.ProductId == product.ProductId).ToList();

                        // Xóa các product category
                        _context.ProductCategories.RemoveRange(productCategory);
                    }
                    catch (Exception ex) { }

                    try
                    {

                        _context.Products.Remove(product);
                    }
                    catch (Exception ex) { }

                    _context.SaveChanges();
                    _notyfService.Success("Xóa thành công");
                    return Json(new { success = true, message = "Xóa sản phẩm thành công!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi khi xóa sản phẩm: " + ex.Message });
            }

        }

        [HttpPost]
        public IActionResult checkactive(int id)
        {
            try
            {
                var text = true;
                var product = _context.Products.Find(id);
                if (product != null)
                {
                    if (product.Active == true)
                    {
                        product.Active = false;
                        text = false;
                    }
                    else
                    {
                        product.Active = true;
                    }
                    _context.Update(product);
                    _context.SaveChanges();
                }
                return Json(new { success = true, data = text });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }



        [HttpPost]
        public IActionResult checkHome(int id)
        {
            try
            {
                var product = _context.Products.Find(id);
                if (product != null)
                {
                    if (product.HomeFlag == true)
                    {
                        product.HomeFlag = false;
                    }
                    else
                    {
                        product.HomeFlag = true;
                    }
                    _context.Update(product);
                    _context.SaveChanges();
                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }


        [HttpPost]
        public IActionResult UploadFile()
        {
            try
            {
                var productIdString = Request.Form["productId"];
                var productName = Request.Form["productName"];
                var files = Request.Form.Files;

                // Chuyển đổi productId từ chuỗi sang số nguyên
                if (!int.TryParse(productIdString, out int productId))
                {
                    return Json(new { success = false, message = "Invalid productId" });
                }

                var product = _context.Products.Find(productId);

                if (product == null)
                {
                    return Json(new { success = false, message = "Product not found" });
                }

                // Tạo đường dẫn thư mục dựa trên productId
                string uploadsFolder = Path.Combine(_webHost.WebRootPath, "images", "Product", productId.ToString());

                // Kiểm tra xem thư mục đã tồn tại chưa, nếu chưa thì tạo mới
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                int fileCount = 0;
                foreach (var file in files)
                {
                    // Lấy phần mở rộng của file
                    string fileExtension = Path.GetExtension(file.FileName);

                    // Tạo tên file tùy chỉnh từ productName và đuôi file
                    string fileName = Utilities.SEOUrl(productName) + (fileCount == 0 ? fileExtension : $"_{fileCount}{fileExtension}");

                    // Tạo đường dẫn tương đối và URL từ tên file đã thay đổi
                    var relativePath = Path.Combine("images", "Product", productIdString, fileName);
                    var url = "/" + relativePath.Replace("\\", "/");

                    if (fileCount == 0)
                    {
                        // Cập nhật Avatar của sản phẩm cho ảnh đầu tiên
                        product.Avatar = url;
                        _context.Products.Update(product);
                        _context.SaveChanges();

                        // Lưu ảnh đầu tiên vào ProductThumbs với IsMain là true
                        ProductThumb productThumb = new ProductThumb
                        {
                            ProductId = productId,
                            Alias = url,
                            IsMain = true
                        };
                        _context.ProductThumbs.Add(productThumb);
                        _context.SaveChanges();
                    }
                    else
                    {
                        // Thêm các ảnh tiếp theo vào ProductThumbs với IsMain là false
                        ProductThumb productThumb = new ProductThumb
                        {
                            ProductId = productId,
                            Alias = url,
                            IsMain = false
                        };
                        _context.ProductThumbs.Add(productThumb);
                        _context.SaveChanges();
                    }

                    // Đường dẫn đầy đủ để lưu file trong thư mục đích
                    string filePath = Path.Combine(uploadsFolder, fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    fileCount++;
                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                throw ex;
            }
             
        }


        private bool ProductExists(string productCode)
        {
            return _context.Products.Any(e => e.ProductCode == productCode);
        }
        public List<AttributesPrice> GetAttributeValues(int attributeId)
        {
            // Truy vấn cơ sở dữ liệu để lấy danh sách attributeValues tương ứng với attributeId
            var attributeValues = _context.AttributesPrices
                .Where(av => av.AttributeId == attributeId)
                .ToList();
            return attributeValues;
        }
        [HttpPost]
        public IActionResult ImportProducts(IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                ModelState.AddModelError("File", "Vui lòng chọn file excel");
                return RedirectToAction(nameof(Index));
            }
            using (var package = new ExcelPackage(excelFile.OpenReadStream()))
            {
                var workSheet = package.Workbook.Worksheets[0];
                //int rowCount = workSheet.Dimension.Rows;
                int rowCount = 0;
                using var transaction = _context.Database.BeginTransaction();


                // tien hanh kiem tra file 
                // tao section file 
                var catelistcheck = _context.Categories.ToList();
                var namecate = "";
                foreach (var checkcate in catelistcheck)
                {
                    namecate += checkcate.CatName + ", ";
                }
                //for (int row = 3; row <= workSheet.Dimension.End.Row; row++)
                //{
                //    for (int col = 1; col < 21; col++)
                //    {
                //        var cellValue = workSheet.Cells[row, col].Value?.ToString();
                //        //
                //        if (col == 1)
                //        {
                //            var errtext = "";
                //            if (namecate.IndexOf(cellValue) == -1)
                //            {
                //                errtext = "Phát hiện lỗi dữ liệu ở dòng " + row + " và cột " + col + "";
                //                break;
                //            }
                //            if (errtext != "")
                //            {
                //                HttpContext.Session.SetString("SaveExcel", errtext);
                //                return RedirectToAction(nameof(Index));
                //            }
                //        }
                //        if (col != 10)
                //        {
                //            if (string.IsNullOrEmpty(cellValue))
                //            {
                //                HttpContext.Session.SetString("SaveExcel", "Phát hiện lỗi dữ liệu ở dòng " + row + " và cột " + col + "");
                //                return RedirectToAction(nameof(Index));
                //            }
                //        }
                //    }
                //}
                try
                {
                    for (int row = 3; row <= workSheet.Dimension.End.Row; row++)
                    {
                        try
                        {
                            if (workSheet.Cells[row, 1].Value != null)
                            {
                                rowCount = row;
                                Product product = new Product();
                                //ten sản phẩm
                                product.ProductName = workSheet.Cells[row, 2].Value?.ToString();
                                // alias
                                product.Alias = Utilities.SEOUrl(product.ProductName);
                                // code 
                                product.ProductCode = workSheet.Cells[row, 7].Value?.ToString();
                                //Xử lý mô tả ngắn
                                string shortDescCell = workSheet.Cells[row, 8].Value?.ToString();
                                string[] shortDescLines = shortDescCell.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                                string concatenatedShortDesc = string.Join("", shortDescLines.Select(line => $"<tr><td>{line.Replace(":", "</td><td>")}</td></tr>"));
                                product.ShortDesc = concatenatedShortDesc;
                                //Xử lý quà tặng
                                var GiftCell = workSheet.Cells[row, 10].Value?.ToString();
                                if (!string.IsNullOrEmpty(GiftCell))
                                {
                                    string[] giftcLines = GiftCell.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                                    string concatenatedGift = string.Join("", giftcLines.Select(line => $"<p>{line}</p>"));
                                    product.Gift = concatenatedGift;
                                }
                                else
                                {

                                }
                                //Thời gian bảo hành
                                var cellValue = workSheet.Cells[row, 11].Value?.ToString();
                                if (!string.IsNullOrEmpty(cellValue) && int.TryParse(cellValue, out int warrantyValue))
                                {
                                    product.Warranty = warrantyValue;
                                }
                                else
                                {
                                    product.Warranty = 0;
                                }
                                //Hình thức bảo hành
                                product.WarrantyNote = workSheet.Cells[row, 12].Value?.ToString();
                                //Giá gốc
                                string priceString = workSheet.Cells[row, 13].Value?.ToString();
                                if (!string.IsNullOrEmpty(priceString) && int.TryParse(priceString, out int priceValue))
                                {
                                    product.Price = priceValue;
                                }
                                else
                                {
                                    product.Price = 0;
                                }

                                //Giá bán
                                string salePriceString = workSheet.Cells[row, 14].Value?.ToString();
                                if (!string.IsNullOrEmpty(salePriceString) && int.TryParse(salePriceString, out int salePriceValue))
                                {
                                    product.SalePrice = salePriceValue;
                                }
                                else
                                {
                                    product.SalePrice = 0;
                                }
                                //Hãng
                                string brandName = workSheet.Cells[row, 3].Value?.ToString();
                                if (!string.IsNullOrEmpty(brandName))
                                {
                                    int brandId = MapBrandNameToId(brandName);
                                    product.BrandId = brandId;
                                }
                                else
                                {
                                    // Xử lý trường hợp giá trị không hợp lệ hoặc null
                                }

                                //tồn kho
                                string unitsInStockString = workSheet.Cells[row, 5].Value?.ToString();
                                if (!string.IsNullOrEmpty(unitsInStockString) && int.TryParse(unitsInStockString, out int unitsInStockValue))
                                {
                                    product.UnitsInStock = unitsInStockValue;
                                }
                                else
                                {
                                    product.UnitsInStock = 0;
                                }
                                // xữ lý sản phẩm lên top ẩn hiện trang chủ                               
                                if (String.Compare(workSheet.Cells[row, 15].Value?.ToString(), "true", true) == 0)
                                {
                                    product.BestSellers = true;
                                }
                                else
                                {
                                    product.BestSellers = false;
                                }

                                if (String.Compare(workSheet.Cells[row, 16].Value?.ToString(), "true", true) == 0)
                                {

                                    product.HomeFlag = true;
                                }
                                else
                                {
                                    product.HomeFlag = false;
                                }
                                if (String.Compare(workSheet.Cells[row, 17].Value?.ToString(), "true", true) == 0)
                                {

                                    product.Active = true;
                                }
                                else
                                {
                                    product.Active = false;
                                }
                                //
                                product.Title = workSheet.Cells[row, 18].Value?.ToString();
                                product.MetaDesc = workSheet.Cells[row, 19].Value?.ToString();
                                product.MetaKey = workSheet.Cells[row, 20].Value?.ToString();
                                product.DateCreated = DateTime.Now;
                                product.DateModified = DateTime.Now;


                                //Danh mục
                                _context.Products.Add(product);
                                _context.SaveChanges();


                                string categoryName = workSheet.Cells[row, 1].Value?.ToString();
                                int categoryId = -1;
                                if (!string.IsNullOrEmpty(categoryName))
                                {
                                    categoryId = MapCategoryNameToId(categoryName);

                                }
                                else
                                {
                                    categoryId = MapCategoryNameToId("Phụ Kiện Laptop,PC,Khác");

                                }
                                ProductCategory catego = new ProductCategory();
                                catego.ProductId = product.ProductId;
                                catego.CatId = categoryId;
                                _context.ProductCategories.Add(catego);
                                _context.SaveChanges();

                                // xu ly chi tiet thuoc tinh sản phẩm và thêm sắp xếp sản phẩm
                                var attrbutess = _context.CategoryAttributes
                                    .Include(c => c.Attribute)
                                    .Where(c => c.CatId == categoryId)
                                    .Select(c => c.Attribute)
                                    .ToList();

                                foreach (var itemAttr in attrbutess)
                                {
                                    var productatrr = _context.AttributesPrices.Where(p => p.ProductId == null).Where(c => c.AttributeId == itemAttr.AttributeId).ToList();
                                    foreach (var attr in productatrr)
                                    {
                                        string price = attr.Price;
                                        if (product.ShortDesc.Contains(price))
                                        {
                                            AttributesPrice addattr = new AttributesPrice();
                                            addattr.AttributeId = itemAttr.AttributeId;
                                            addattr.Price = attr.Price;
                                            addattr.ProductId = product.ProductId;
                                            addattr.Active = true;
                                            _context.AttributesPrices.Add(addattr);
                                            _context.SaveChanges();
                                        }

                                    }

                                }

                                // cus sp
                                string cusName = workSheet.Cells[row, 4].Value?.ToString();
                                int cusid = MapCustomerNameToId(cusName);
                                ProductAddCusPro cusPro = new ProductAddCusPro();
                                var cusSp = _context.CustomerSuppliers.Where(c => c.Id == cusid).FirstOrDefault();
                                if (cusSp.Address != null)
                                {
                                    cusSp.Address = "Kho lưu động hoặc chưa cập nhật";
                                    _context.CustomerSuppliers.Update(cusSp);
                                    _context.SaveChanges();
                                }
                                cusPro.CustomerId = cusid;
                                cusPro.ProductId = product.ProductId;
                                if (workSheet.Cells[row, 6].Value?.ToString() != null || workSheet.Cells[row, 6].Value?.ToString() != "")
                                {
                                    cusPro.Address = workSheet.Cells[row, 6].Value?.ToString();
                                }
                                else
                                {
                                    cusPro.Address = cusSp.Address;
                                }
                                if (!string.IsNullOrEmpty(unitsInStockString) && int.TryParse(unitsInStockString, out int unitsInStock))
                                {
                                    cusPro.Stock = unitsInStock;
                                }
                                else
                                {
                                    cusPro.Stock = 0;
                                }
                                cusPro.Description = "Sản phẩm được thêm từ file excel vào ngày: " + DateTime.Now + "";
                                _context.ProductAddCusPros.Add(cusPro);
                                _context.SaveChanges();

                               
                                //List<string> stringList = imageUrl.Split(',').ToList();
                                //var count = 0;
                                //var countImage = _context.ProductThumbs.Where(x => x.ProductId == product.ProductId).Count();
                                //count = countImage + 1;
                                //foreach (string str in stringList)
                                //{
                                //    var productThumb = new ProductThumb
                                //    {
                                //        ProductId = product.ProductId,
                                //        Alias = str
                                //    };
                                //    if (count == 1)
                                //    {
                                //        productThumb.IsMain = true;
                                //        product.Avatar = str;
                                //    }
                                //    count++;
                                //    _context.ProductThumbs.Add(productThumb);
                                //}
                                //_context.SaveChanges();
                            }
                            else
                            {
                                break;
                            }
                            // nha cung cap dia chi nhap kho

                            _context.SaveChanges();
                        }
                        catch (Exception ex) { }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // Xử lý ngoại lệ - ghi log lỗi, rollback giao dịch và cung cấp phản hồi phù hợp cho người dùng
                    transaction.Rollback();
                    ModelState.AddModelError("Import", "Lỗi khi nhập sản phẩm: " + ex.Message);
                    return RedirectToAction(nameof(Index));
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public static bool DownloadImageFromUrl(string imageUrl, string imageName)
        {
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products", imageName);
            try
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(imageUrl, imagePath);
                }
                return true; // Tải hình ảnh thành công
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                Console.WriteLine($"Lỗi tải hình ảnh từ URL: {ex.Message}");
                return false; // Tải hình ảnh không thành công
            }
        }
        public int MapBrandNameToId(string brandNameFromExcel)
        {
            string brandNameLower = brandNameFromExcel.ToLower();
            var brand = _context.Brands.FirstOrDefault(b => b.BrandName.ToLower() == brandNameLower);
            if (brand != null)
            {
                return brand.BrandId;
            }
            throw new InvalidOperationException("Không tìm thấy brandId cho tên hãng này");
        }
        public int MapCategoryNameToId(string categoryNameFromExcel)
        {
            string catNameLower = categoryNameFromExcel.ToLower();
            var category = _context.Categories.FirstOrDefault(b => b.CatName.ToLower() == catNameLower);
            if (category != null)
            {
                return category.CatId;
            }
            throw new InvalidOperationException("Không tìm thấy brandId cho tên hãng này");
        }
        public int MapCustomerNameToId(string customerNameFromExcel)
        {
            string cusNameLower = customerNameFromExcel.ToLower();
            var cusinfor = _context.CustomerSuppliers.FirstOrDefault(b => b.Name.ToLower() == cusNameLower);
            var cusinfor2 = _context.CustomerSuppliers.FirstOrDefault(b => b.Name.ToLower() == "Đối tác mới");
            if (cusinfor != null)
            {
                return cusinfor.Id;
            }
            else
            {
                if (cusinfor2 != null)
                {
                    return cusinfor2.Id;
                }
                else
                {
                    CustomerSupplier cusSp = new CustomerSupplier();
                    cusSp.Image = "";
                    cusSp.Name = "Đối tác mới";
                    cusSp.Company = "Cập nhật công ty";
                    cusSp.Phone = 123456789;
                    cusSp.Email = null;
                    cusSp.Address = "Kho lưu động hoặc chưa cập nhật";
                    cusSp.YearAdd = DateTime.Now;
                    cusSp.Lever = 1;
                    _context.CustomerSuppliers.Add(cusSp);
                    _context.SaveChanges();
                    return cusSp.Id;
                }
            }
        }

        [HttpPost]
        public IActionResult SaveCheckboxs(CheckboxProduct checkboxProduct)
        {
            // Lưu giá trị của các checkbox vào session
            HttpContext.Session.Set("ProductCode", checkboxProduct.ProductCode);
            HttpContext.Session.Set("ProductInfo", checkboxProduct.ProductInfo);
            HttpContext.Session.Set("InputPrice", checkboxProduct.InputPrice);
            HttpContext.Session.Set("SalePrice", checkboxProduct.SalePrice);
            HttpContext.Session.Set("UnitsInStock", checkboxProduct.UnitsInStock);
            HttpContext.Session.Set("Active", checkboxProduct.Active);
            HttpContext.Session.Set("DateCreated", checkboxProduct.DateCreated);
            //HttpContext.Session.Set("CodStatus", checkboxProduct.CodStatus);
            //HttpContext.Session.Set("Total", checkboxProduct.Total);

            // Thêm các checkbox khác tương tự

            return RedirectToAction("Index");
        }

       
    }
}
