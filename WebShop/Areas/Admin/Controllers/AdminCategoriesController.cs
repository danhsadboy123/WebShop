using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using NPOI.XSSF.Streaming.Values;
using OfficeOpenXml.Style.XmlAccess;
using PagedList.Core;
using WebShop.Areas.Admin.Models;
using WebShop.Extension;
using WebShop.Helpper;
using WebShop.Models;

namespace WebShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminCategoriesController : Controller
    {
        private readonly DbMarketsContext _context;
        public INotyfService _notyfService { get; }
        public IWebHostEnvironment _webHost;
        public AdminCategoriesController(DbMarketsContext context, INotyfService notyfService, IWebHostEnvironment webHost)
        {
            _context = context;
            _notyfService = notyfService;
            _webHost = webHost;
        }

        // GET: Admin/AdminCategories
        public IActionResult Index(int? page)
        {
            var lsCategorys = _context.Categories
                .Include(c => c.Slides)
                .Include(c => c.ProductCategories)
                .ThenInclude(p => p.Product)
                .AsNoTracking()
                .OrderBy(x => x.CatId);   
            return View(lsCategorys);
        }
        [HttpPost]
        public IActionResult addSlide(string Thumb, string SlideName, int Ordering, string Alias, int catid)
        {
            Slide slide = new Slide();
            slide.Thumb = Thumb;
            slide.SlideName = SlideName;
            slide.Ordering = Ordering;
            slide.Alias = Alias;
            slide.Right = false;
            slide.Bottom = false;
            slide.CatId = catid;
            slide.HomeFlag = true;
            slide.KichHoat = true;
            try
            {
                _context.Slides.Add(slide);
                _context.SaveChanges();
                return Json("OK");
            }
            catch (Exception ex)
            {
                return Json("NO");
            }
        }
        [HttpPost]
        public IActionResult SaveSlide(string Thumb, string SlideName, int Ordering, string Alias, int id, bool check)
        {
            var slide = _context.Slides.FirstOrDefault(c => c.SlideId == id);
            if (slide == null)
            {
                return Json("NO");
            }
            else
            {
                slide.Thumb = Thumb;
                slide.SlideName = SlideName;
                slide.Ordering = Ordering;
                slide.Alias = Alias;
                slide.HomeFlag = check;
            }
            try
            {
                _context.Slides.Update(slide);
                _context.SaveChanges();
                return Json("OK");
            }
            catch (Exception ex)
            {
                return Json("NO");
            }
        }
        public IActionResult DeleteSlide(int id)
        {

            var slide = _context.Slides.FirstOrDefault(c => c.SlideId == id);

            try
            {
                _context.Slides.Remove(slide);
                _context.SaveChanges();
                return Json("OK");
            }
            catch (Exception ex)
            {
                return Json("NO");
            }
        }
        [HttpPost]
        public IActionResult Thumshow(string Bennerthumb, bool check, int catid)
        {
            var cate = _context.Categories.FirstOrDefault(c => c.CatId == catid);
            if (cate == null)
            {
                return Json("NO");
            }
            else
            {
                cate.BannerThumb = Bennerthumb;
                cate.ThumbShow = check;
            }
            try
            {
                _context.Update(cate);
                _context.SaveChanges();
                return Json("OK");
            }
            catch (Exception ex)
            {
                return Json("NO");
            }
        }
        // check product category
        public IActionResult checkProduct(int? id)
        {
            var ProductCate = _context.ProductCategories.Where(c => c.CatId == id).ToList();
            var errcode = "";
            if (ProductCate.Count > 0)
            {
                errcode = "No";
            }
            else
            {
                var cate = _context.Categories.Where(c => c.CatId == id).FirstOrDefault();
                try
                {
                    _context.Remove(cate);
                    _context.SaveChanges();
                    errcode = "Ok";
                }
                catch (Exception ex)
                {
                    errcode = "No" + ex;
                }
            }
            return Json(new { success = errcode });
        }
        // GET: Admin/AdminCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CatId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Admin/AdminCategories/Create
        public IActionResult Create()
        {

            var categories = _context.Categories.ToList();
            ViewData["Categories"] = categories;

            return View();
        }

        // POST: Admin/AdminCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CatId,CatName,CatNameEn,DescriptionEn,MoTa,ParentId,Levels,Ordering,Outstanding,Published,Thumb,Title,TitleEn,Alias,MetaDesc,MetaDescEn,MetaKey,MetaKeyEn,Cover,SchemaMarkup,Icon")] Category category)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(category.Thumb)) category.Thumb = "default.jpg";
                category.Alias = Utilities.SEOUrl(category.CatName);
                _context.Add(category);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm mới thành công");
                return RedirectToAction(nameof(Index));
            }
            ViewData["DanhMucCha"] = new SelectList(_context.Categories, "CatId", "CatName", category.CatId);
            return View(category);
        }
        public class itemcate
        {
            public int id { get; set; }
            public int num { get; set; }     // as you are passing pts as string
        }
        [HttpPost]
        // updatecategoryattrbute
        public IActionResult updatecategoryattrbute(int cateid, bool itembrand, string namebrand, List<itemcate> arraytrue, string[] arrayfalse)
        {
            var text = "No";
            int dem = 0;
            int count = 0;
            if (itembrand == true) count = 1;
            var catego = _context.Categories.Where(c => c.CatId == cateid).FirstOrDefault();
            if (catego != null)
            {
                if (catego.BrandShow != itembrand || catego.NameBrand != namebrand)
                {
                    catego.BrandShow = itembrand;
                    catego.NameBrand = namebrand;
                    _context.Update(catego);
                    _context.SaveChanges();
                    dem++;
                }
            }
            // su ly khi true
            foreach (var item in arraytrue)
            {
                var cat = _context.CategoryAttributes.Where(c => c.CategoryAttributeId == item.id).FirstOrDefault();
                if (cat != null)
                {
                    if (count < 7)
                    {
                        cat.Show = true;

                    }
                    else
                    {
                        cat.Show = false;
                    }
                    cat.Odering = item.num;
                    count++;
                    try
                    {
                        _context.CategoryAttributes.Update(cat);
                        _context.SaveChanges();
                        dem++;
                    }
                    catch (Exception ex)
                    {

                    }

                }
            }
            foreach (var item in arrayfalse)
            {
                var cat = _context.CategoryAttributes.Where(c => c.CategoryAttributeId == int.Parse(item)).FirstOrDefault();
                if (cat != null)
                {
                    if (cat.Show == true)
                    {
                        cat.Show = false;
                        try
                        {
                            _context.CategoryAttributes.Update(cat);
                            _context.SaveChanges();
                            dem++;
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                }
            }
            if (dem != 0)
            {
                text = "Đã cập nhật " + dem + " dữ liệu";
            }

            return Json(text);
        }
        // GET: Admin/AdminCategories/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var category = _context.Categories
                .Where(c => c.CatId == id)
                .Include(c => c.Slides)
                .Include(c => c.CategoryBrands)
                .ThenInclude(c => c.Brand)
                .ThenInclude(c => c.Products.Where(c => c.ProductCategories.Any(c => c.CatId == id)))
                .FirstOrDefault();
            var brandgrop = _context.BrandGroups.Where(c => c.CatId == id).Include(c => c.Brand).ToList();
            var lsbrand = _context.Brands.ToList();
            var banner = _context.Banners.Where(b => b.CatId == id).ToList();

            if (category == null)
            {
                return NotFound();
            }
            var atrr = _context.CategoryAttributes.Include(c => c.Attribute).ThenInclude(c => c.AttributesPrices).Where(c => c.CatId == id).ToList();

            var cateAtrr = _context.CategoryAttributes.ToList();
            foreach (var item in atrr)
            {
                cateAtrr.RemoveAll(c => c.Name == item.Name);
            }
            var categories = _context.Categories.ToList();
            ViewData["Categories"] = categories;
            ViewBag.atrr = atrr;
            ViewBag.cate = cateAtrr;
            ViewBag.brandg = brandgrop;
            ViewBag.lsbrand = lsbrand;
            ViewBag.lsBanner = banner;
            return View(category);
        }

        // POST: Admin/AdminCategories/Edit/5 
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category updatedCategory)
        {
            if (id != updatedCategory.CatId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Lấy đối tượng Category từ cơ sở dữ liệu dựa trên ID
                    var existingCategory = await _context.Categories.FindAsync(id);
                    if (existingCategory == null)
                    {
                        return NotFound();
                    }

                    // Sử dụng cơ chế tự động mapping để cập nhật thông tin từ updatedCategory vào existingCategory
                    _context.Entry(existingCategory).CurrentValues.SetValues(updatedCategory);

                    // Tạo alias dựa trên CatName
                    existingCategory.Alias = Utilities.SEOUrl(updatedCategory.CatName);

                    // Lưu thay đổi vào cơ sở dữ liệu
                    await _context.SaveChangesAsync();

                    // Hiển thị thông báo thành công
                    _notyfService.Success("Chỉnh sửa thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(updatedCategory.CatId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                // Chuyển hướng về trang danh sách danh mục sau khi chỉnh sửa thành công
                return RedirectToAction(nameof(Index));
            }

            // Nếu dữ liệu không hợp lệ, hiển thị lại biểu mẫu chỉnh sửa với thông báo lỗi
            return View(updatedCategory);
        }


        // add cateatri
        public IActionResult addatribute(int id, int cateid)
        {
            var attr = _context.Attributes.Where(i => i.AttributeId == id).FirstOrDefault();
            try
            {
                CategoryAttribute cate = new CategoryAttribute();
                cate.CatId = cateid;
                cate.Name = attr.Name;
                cate.AttributeId = attr.AttributeId;
                cate.KichHoat = attr.KichHoat;

                _context.CategoryAttributes.Add(cate);
                _context.SaveChanges();
                return Json("Ok");
            }
            catch (Exception ex)
            {
                return Json("No");
            }
        }

        public IActionResult deleteatribute(int id)
        {
            var attr = _context.CategoryAttributes
                .Where(i => i.CategoryAttributeId == id)
                .FirstOrDefault();
            try
            {
                _context.CategoryAttributes.Remove(attr);
                _context.SaveChanges();
                return Json("Ok");
            }
            catch (Exception ex)
            {
                return Json("No");
            }
        }

        public IActionResult updatecate(int id)
        {
            var attr = _context.Attributes
                .Where(i => i.AttributeId == id)
                .FirstOrDefault();
            try
            {
                attr.Ordering = attr.Ordering - 1;
                var attrb = _context.Attributes.Where(i => i.Ordering == attr.Ordering).FirstOrDefault();
                if (attrb != null)
                {
                    attrb.Ordering = attrb.Ordering + 1;
                    _context.Attributes.Update(attrb);
                    _context.SaveChanges();
                }
                _context.Attributes.Update(attr);
                _context.SaveChanges();
                return Json("Ok");
            }
            catch (Exception ex)
            {
                return Json("No");
            }
        }

        [HttpPost]
        public IActionResult fulllAtrti(int id)
        {
            AdminCate attri = new AdminCate();
            var atrr = _context.CategoryAttributes.Include(c => c.Attribute).ThenInclude(c => c.AttributesPrices).Where(c => c.CatId == id).ToList();
            var cateAtrr = _context.CategoryAttributes.ToList();

            attri.catebutes = atrr;
            attri.catebutesAdd = cateAtrr;
            return Json(new { success = "Ok", data = attri });
        }

        // GET: Admin/AdminCategories/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CatId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Admin/AdminCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            string image = Path.Combine(Directory.GetCurrentDirectory(), "images", "category", category.Thumb);
            Utilities.RemoveFile(image);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            _notyfService.Success("Xóa thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int? id)
        {
            return _context.Categories.Any(e => e.CatId == id);
        }

        // brand cung loai
        public IActionResult Deletebrandca(int id)
        {
            var catebra = _context.CategoryBrands.Where(c => c.CategoryBrandId == id).FirstOrDefault();
            var success = false;
            if (catebra != null)
            {
                try
                {
                    _context.CategoryBrands.Remove(catebra);
                    _context.SaveChanges();
                    success = true;
                }
                catch (Exception ex) { }
            }
            return Json(new { success = success });
        }

        // save group
        [HttpPost]
        public IActionResult Savegroup(string text, int id)
        {
            var success = false;
            var cateb = _context.CategoryBrands.Where(c => c.CategoryBrandId == id).FirstOrDefault();
            var brand = _context.Brands.Where(c => c.BrandId == cateb.BrandId).FirstOrDefault();
            if (cateb != null && brand != null)
            {
                try
                {
                    BrandGroup br = new BrandGroup();
                    br.BrandId = cateb.BrandId;
                    br.CatId = cateb.CatId;
                    br.Name = text;
                    br.MoTa = "Đã tạo vào ngày:" + DateTime.Now;
                    br.Status = true;
                    _context.BrandGroups.Add(br);
                    _context.SaveChanges();
                    success = true;
                }
                catch (Exception ex)
                {
                }
            }
            return Json(new { success = success });
        }
        [HttpPost]
        public IActionResult Savepro(int groupid, int proid)
        {
            var success = false;
            var brandg = _context.BrandGroups.Where(c => c.Id == groupid).FirstOrDefault();
            var product = _context.Products.Where(c => c.ProductId == proid).FirstOrDefault();
            if (brandg != null && product != null)
            {
                try
                {
                    product.BrandGroup = brandg.Id;
                    _context.Products.Update(product);
                    _context.SaveChanges();
                    success = true;
                }
                catch (Exception ex) { }
            }
            return Json(new { success = success });
        }
        [HttpPost]
        public IActionResult Deletegroup(int id)
        {
            var success = false;
            var brandg = _context.BrandGroups.Where(c => c.Id == id).FirstOrDefault();
            var product = _context.Products.Where(c => c.BrandGroup == id).ToList();
            if (brandg != null && product != null)
            {
                try
                {
                    foreach (var item in product)
                    {
                        item.BrandGroup = null;
                        _context.Products.Update(item);
                        _context.SaveChanges();
                    }
                    _context.BrandGroups.Remove(brandg);
                    _context.SaveChanges();
                    success = true;
                }
                catch (Exception ex) { }
            }
            return Json(new { success = success });
        }
        [HttpPost]
        public IActionResult Deletepro(int id)
        {
            var success = false;
            var product = _context.Products.Where(c => c.ProductId == id).FirstOrDefault();
            if (product != null)
            {
                try
                {
                    product.BrandGroup = null;
                    _context.Products.Update(product);
                    _context.SaveChanges();
                    success = true;
                }
                catch (Exception ex) { }
            }
            return Json(new { success = success });
        }
        [HttpPost]
        public IActionResult updateGroup(int id, string text)
        {
            var success = false;
            var brandg = _context.BrandGroups.Where(c => c.Id == id).FirstOrDefault();
            if (brandg != null)
            {
                try
                {
                    brandg.Name = text;
                    _context.BrandGroups.Update(brandg);
                    _context.SaveChanges();
                    success = true;

                }
                catch (Exception ex)
                {

                }
            }
            return Json(new { success = success });
        }

        // brand vaf xu kiem
        // them
        public IActionResult Addcatebrand(int id, int catid)
        {
            var success = false;
            var brand = _context.Brands.Where(c => c.BrandId == id).FirstOrDefault();
            var catebrand = _context.CategoryBrands.Where(c => c.CatId == catid).Where(c => c.BrandId == id).FirstOrDefault();
            if (catebrand != null)
            {

            }
            else
            {
                if (brand != null)
                {
                    var newcategoryBrand = new CategoryBrand
                    {
                        BrandId = brand.BrandId,
                        CatId = catid,
                        Name = brand.BrandName,
                        MoTa = brand.MoTa,
                        BrandProduct = true,
                        Topbrand = false,
                        KichHoat = true
                    };
                    _context.CategoryBrands.Add(newcategoryBrand);
                    _context.SaveChanges();
                    success = true;
                }
            }
            return Json(new { success = success });
        }
        // xoa
        public IActionResult Deletecatebrand(int id, int catid)
        {
            var success = false;
            var catebrand = _context.CategoryBrands.Where(c => c.CatId == catid).Where(c => c.BrandId == id).FirstOrDefault();
            if (catebrand != null)
            {
                if (catebrand.Topbrand != true)
                {
                    _context.CategoryBrands.Remove(catebrand);
                    _context.SaveChanges();
                    success = true;
                }
            }
            else
            {
                success = false;
            }
            return Json(new { success = success });
        }



        [HttpPost]
        public async Task<ActionResult> UploadFile(int CatId, bool status)
        {
            try
            {
                var file = Request.Form.Files[0]; // Lấy tệp từ yêu cầu

                if (file.Length > 0)
                {
                    var permittedExtensions = new[] { ".jpg", ".png", ".gif" };
                    var extension = Path.GetExtension(file.FileName).ToLower();

                    if (!permittedExtensions.Contains(extension))
                    {
                        return BadRequest("Invalid file format.");
                    }

                    var uploads = Path.Combine(_webHost.WebRootPath, "images", "Banner"); // Thư mục lưu trữ tệp

                    // Tạo thư mục nếu chưa tồn tại
                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);
                    }

                    var fileName = file.FileName; // file có đẩy đủ tên.đuôi
                    var filePath = Path.Combine(uploads, fileName); // Đường dẫn đến tệp

                    // Lưu tệp vào thư mục (ghi đè nếu đã tồn tại)
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var relativePath = Path.Combine("images", "Banner", fileName); // Đường dẫn tương đối đến tệp
                    var url =relativePath.Replace("\\", "/"); // Chuyển đổi đường dẫn tương đối thành URL

                    if (CatId <= 0)
                    {
                        return BadRequest("Invalid Category ID.");
                    }

                    Banner b = new Banner()
                    {
                        CatId = CatId,
                        Status = status,
                        Banner1 = url
                    };

                    _context.Banners.Add(b);
                    await _context.SaveChangesAsync(); // Sử dụng SaveChangesAsync
                    return Json(new { success = true });
                    
                }
                else
                {
                    return BadRequest("No file uploaded.");
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        public IActionResult DeleteBanner(int id )
        {
            var banner = _context.Banners.Find(id);
            if(banner !=null)
            {
                _context.Banners.Remove(banner);
                _context.SaveChanges();
                return Json(new { success = true });
            }    
            return Json(new { success = false });
        }

    }
}
