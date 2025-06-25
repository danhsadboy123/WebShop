using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using WebShop.Models;

namespace WebShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]

    public class AdminBrandsController : Controller
    {
        private readonly DbMarketsContext _context;
        public INotyfService _notyfService { get; }
        public IWebHostEnvironment _webHost;
        public AdminBrandsController(DbMarketsContext context, INotyfService notyfService, IWebHostEnvironment webHost)
        {
            _context = context;
            _notyfService = notyfService;
            _webHost = webHost;
        }
        // GET: Admin/AdminBrands
        public IActionResult Index()
        {
            var lsBrands = _context.Brands
                .Include(b => b.CategoryBrands)                
                .Include(p => p.Products)
                .AsNoTracking()
                .OrderBy(x => x.BrandId);
            var allCategories = _context.Categories.ToList();
            ViewBag.AllCategories = allCategories;    
            return View(lsBrands);
        }
        // GET: Admin/AdminBrands/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Brands == null)
            {
                return NotFound();
            }
            var brand = await _context.Brands
                .FirstOrDefaultAsync(m => m.BrandId == id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }
        // GET: Admin/AdminBrands/Create
        public IActionResult Create()
        {
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CatId", "CatName");
            return View();
        }
        // POST: Admin/AdminBrands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BrandId,BrandName,Description")] Brand brand)
        {
            if (ModelState.IsValid)
            {               
                _context.Add(brand);
                await _context.SaveChangesAsync();

                // Lấy danh sách hãng
                List<string> categoryBrands = Request.Form["categoryBrands"].ToList();

                // Thêm bảng ghi categorybrand
                foreach (var value in categoryBrands)
                {
                    //Thêm bảng ghi mới
                    var newcategoryBrand = new CategoryBrand
                    {
                        BrandId = brand.BrandId,
                        CatId = Convert.ToInt32(value),
                        Name = brand.BrandName,
                        Description = brand.Description,
                        Active = true
                    };
                    _context.CategoryBrands.Add(newcategoryBrand);
                    await _context.SaveChangesAsync();
                }
                _notyfService.Success("Tạo mới nhà sản xuất thành công");
                return RedirectToAction(nameof(Index));
            }
            return View(brand);
        }
        // GET: Admin/AdminBrands/Edit/5
        public IActionResult Edit(int? id, int curPage)
        {
            var currentPage = curPage;
            var brand = _context.Brands
                .Include(b => b.CategoryBrands)
                .ThenInclude(cb => cb.Cat)
                .FirstOrDefault(b => b.BrandId == id);

            if (brand == null)
            {
                // Xử lý trường hợp không tìm thấy thương hiệu
                return NotFound();
            }
            // Lấy danh sách các danh mục đã được chọn cho thương hiệu
            var selectedCategories = brand.CategoryBrands.Select(cb => cb.Cat).ToList();
            var video = _context.Videos.Where(v => v.BrandId == id  ).ToList();

            // Lấy danh sách các danh mục từ cơ sở dữ liệu
            var allCategories = _context.Categories.ToList();

            ViewBag.AllCategories = allCategories;
            ViewBag.SelectedCategories = selectedCategories;
            ViewBag.CurrentPage = currentPage;
            ViewBag.videos = video;

            return View(brand);
        }
        // POST: Admin/AdminBrands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int curPage, [Bind("BrandId,BrandName,Description,Thumb")] Brand brand)
        {
            var currentPage = curPage;

            if (id != brand.BrandId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    //Xóa các bảng ghi categorybrand cũ
                    var categoryBrand = _context.CategoryBrands
                           .AsNoTracking()
                           .Where(x => x.BrandId == id).ToList();

                    _context.CategoryBrands.RemoveRange(categoryBrand);
                    // Lấy danh sách danh mục
                    List<string> cats = Request.Form["categoryBrands"].ToList();
                    // Thêm bảng ghi categorybrand
                    foreach (var value in cats)
                    {
                        //Thêm bảng ghi mới
                        var newcategoryBrand = new CategoryBrand
                        {
                            BrandId = brand.BrandId,
                            CatId = Convert.ToInt32(value),
                            Name = brand.BrandName,
                            Description = brand.Description,
                            BrandProduct = true,
                            Topbrand=true,
                            Active = true
                        };
                        _context.CategoryBrands.Add(newcategoryBrand);
                        await _context.SaveChangesAsync();                        
                    }
                    _context.Update(brand);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrandExists(brand.BrandId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewBag.CurrentPage = currentPage;
            return View(brand);
        }

        public IActionResult Editbrandcate(int id, [Bind("BrandId,BrandName,Description,Thumb")] Brand brand,int[] cate)
        {
            return Json("ok");
        }


        // GET: Admin/AdminBrands/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Brands == null)
            {
                return NotFound();
            }
            var brand = await _context.Brands
                .FirstOrDefaultAsync(m => m.BrandId == id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }
        // POST: Admin/AdminBrands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Brands == null)
            {
                return Problem("Entity set 'dbMarketsContext.Brands'  is null.");
            }
            var brand = await _context.Brands.FindAsync(id);
            var productbrand = _context.Products.Where(c=>c.BrandId== id).FirstOrDefault(); 
            if (productbrand == null)
            {
                if (brand != null)
                {
                    _context.Brands.Remove(brand);
                    // Lấy danh sách các bảng ghi categorybrand
                    var categorybrand = await _context.CategoryBrands.Where(x => x.BrandId == brand.BrandId).ToListAsync();

                    // Xóa giá trị thuộc tính
                    _context.CategoryBrands.RemoveRange(categorybrand);
                    _notyfService.Success("Xóa thành công");
                }
            }
            else
            {
               _notyfService.Success("Xóa không thành công: Không thể xóa thương hiệu chứa sản phẩm");
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<ActionResult> UploadFile(int brandId, bool status,string embeddedLink, int sort)
        {
            try
            {
                var file = Request.Form.Files[0]; // Lấy tệp từ yêu cầu

                if (file.Length > 0)
                {
                    var uploads = Path.Combine(_webHost.WebRootPath, "images", "Banner"); // Thư mục lưu trữ tệp

                    // Tạo thư mục nếu chưa tồn tại
                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);
                    }
                    var fileName = file.FileName; // file có đẩy đủ tên.đuôi
                    var filePath = Path.Combine(uploads, fileName); // Đường dẫn đến tệp

                    // Lưu tệp vào thư mục
                    using (var stream = new FileStream(filePath, FileMode.Append))
                    {
                        await file.CopyToAsync(stream);
                    }
                    var relativePath = Path.Combine("images", "Banner", fileName); // Đường dẫn tương đối đến tệp
                    var url = "/" + relativePath.Replace("\\", "/"); // Chuyển đổi đường dẫn tương đối thành URL

                    Video v = new Video()
                    {
                        BrandId = brandId,
                        Image = url,
                        Video1 = embeddedLink,
                        Show = status,
                        Sort = sort
                    };
                    _context.Videos.Add(v);
                    _context.SaveChanges();
                    // Trả về kết quả 
                    return Ok("pass");
                }
                else
                {
                    return BadRequest("No file uploaded.");
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        public IActionResult DeleteVideo(int id )
        {
            var data = _context.Videos.Find(id);
            if( data == null )
            {
                return Json(new { success = false });
            }    
            else
            {
                _context.Videos.Remove(data);
                _context.SaveChanges();
                return Json(new { success = true });
            }
       
        }

        private bool BrandExists(int id)
        {
          return _context.Brands.Any(e => e.BrandId == id);
        }
    }
}
