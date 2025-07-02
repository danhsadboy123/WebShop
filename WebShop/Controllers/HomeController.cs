using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using System.Text.Json;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;
using MailKit.Search;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NPOI.SS.Formula.Functions;
using WebShop.Extension;
using WebShop.Models;
using WebShop.ModelViews;

namespace WebShop.Controllers
{
    public class HomeController : BaseController 
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DbMarketsContext _context;
        private readonly IMemoryCache _cache;
        public HomeController(ILogger<HomeController> logger, DbMarketsContext context, IMemoryCache cache) : base(context)
        {
            _logger = logger;
            _context = context;
            _cache = cache;
        }
        private string GenerateHashFromId(int id)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(id.ToString()));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }


        public IActionResult Index()
        {

            // Kiểm tra nếu danh mục đã có trong cache
            if (!_cache.TryGetValue("CategoriesWithProductsIndex", out List<ProductHomeVM> categoriesWithProducts))
            {
                // Nếu chưa có thì truy vấn và lưu vào cache
                categoriesWithProducts = _context.Categories
                                                    .Where(cat => cat.Published && cat.Outstanding)
                                                    .Take(6)
                                                    .OrderBy(cat => cat.Ordering)
                                                    .Select(cat => new ProductHomeVM
                                                    {
                                                        category = cat,
                                                        lsProducts = cat.ProductCategories
                                                            .Select(pc => pc.Product)
                                                            .Where(pc => pc.IsActivated && pc.HomeFlag && pc.BestSellers)
                                                            .OrderByDescending(pc => pc.DateCreated)
                                                            .Take(8)
                                                            .ToList(),
                                                        Banner = cat.Banners.FirstOrDefault(b => b.Status == true)
                                                    })
                                                   
                                                    .ToList();

                // Thiết lập cache với thời gian hết hạn
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromMinutes(30)
                };

                _cache.Set("CategoriesWithProductsIndex", categoriesWithProducts, cacheEntryOptions);
            }
            var brand = _context.Brands.Take(30).ToList();
            var discounts = _context.Discounts
            .Where(d => d.ShowWeb == true
                        && d.Code == null
                        && d.Startus == true
                        && d.TimeOff > DateTime.Now
                        && d.TimeOn <= DateTime.Now
                        && d.Code == null)
            .Take(2)
            .ToList();
            var slider = _context.Slides.Where(s => s.IsActivated == true && s.HomeFlag == true &&s.Right==true && s.CatId == null).Take(2).OrderBy(s=>s.Ordering).ToList(); 
            ViewBag.Slider = slider;  

            ViewBag.discounts = discounts;  
            ViewBag.categoriesWithProducts = categoriesWithProducts;
            ViewBag.brand = brand;
            return View();   
        }

        [HttpGet]
        public IActionResult SeeMoreProductCateIndex()
        {
            try
            {
                var categoriesWithProductsSee = _context.Categories
                    .Where(cat => cat.Published && cat.Outstanding)
                    .Skip(6)
                    .Take(6)
                    .OrderBy(cat => cat.Ordering)
                    .Select(cat => new
                    {
                        CategoryId = cat.CatId,
                        CategoryName = cat.CatName,
                        Products = cat.ProductCategories
                            .Select(pc => new
                            {
                                ProductId = pc.Product.ProductId,
                                ProductName = pc.Product.ProductName,
                                Avatar = pc.Product.Avatar,
                                SalePrice = pc.Product.SalePrice,
                                Price = pc.Product.Price,
                                HomeFlag = pc.Product.HomeFlag,  // Thêm thuộc tính này
                                IsActivated = pc.Product.IsActivated,      // Thêm thuộc tính này
                                BestSellers = pc.Product.BestSellers, // Thêm thuộc tính này
                                DateCreated = pc.Product.DateCreated, // Thêm thuộc tính này
                                UnitsInStock=pc.Product.UnitsInStock,
                                Alias = pc.Product.Alias
                            })
                            .Where(pc => pc.IsActivated ==true && pc.HomeFlag == true && pc.BestSellers == true)
                            .OrderByDescending(pc => pc.DateCreated)
                            .Take(8)
                            .ToList(),
                        Banner = cat.Banners
                            .Where(b => b.Status == true)
                            .Select(b => b.Banner1)
                            .FirstOrDefault()
                    })
                    .ToList();

                // Cấu hình bỏ qua vòng lặp khi serialize đối tượng
                var jsonOptions = new JsonSerializerOptions
                {
                    ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles,
                    WriteIndented = true // Tùy chọn: làm đẹp JSON cho dễ nhìn
                };

                return new JsonResult(categoriesWithProductsSee, jsonOptions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Trả về lỗi với chi tiết thông báo
            }
        }






        [HttpGet]
        public IActionResult loadcategoryview(int id)
        {
            CategoryView categoryView = new CategoryView();
            var lsCats = _context.Categories
                .Where(c=>c.CatId==id)
                .Include(c => c.CategoryBrands.OrderByDescending(c=>c.Brand.Products.Count()).Take(22)) 
                    .ThenInclude(c => c.Brand) 
                    .ThenInclude(c=>c.BrandGroups.Where(c=>c.CatId==id))
                .Include(c => c.CategoryAttributes)
                .Include(c => c.Slides.Where(c => c.HomeFlag == true).OrderBy(c => c.Ordering))
                .Where(x => x.Published == true)
                .OrderBy(x => x.Ordering)                 
                .FirstOrDefault();
            List<CategoryAttribute> attributes = new List<CategoryAttribute>(); 
            foreach (var item in lsCats.CategoryAttributes)
            {
                var itemcat = _context.CategoryAttributes.Where(c => c.CategoryAttributeId == item.CategoryAttributeId)
                    .Include(c=>c.Attribute).ThenInclude(c=>c.AttributesPrices.Where(c=>c.ProductId==null)).FirstOrDefault();
                attributes.Add(itemcat);
            }
            if (lsCats != null)
            {
                foreach (var categoryBrand in lsCats.CategoryBrands)
                { 
                    categoryBrand.Brand.Products = _context.Products.Where(c=>c.ProductCategories.Any(c=>c.CatId==id)).Take(1).ToList();
                }
            }
            categoryView.categoryAttributes = attributes.OrderBy(c=>c.Odering).ToList();
            categoryView.category = lsCats;
            HttpContext.Session.Set<CategoryView>(lsCats.CatName, categoryView);
            var cate = HttpContext.Session.Get<CategoryView>(lsCats.CatName);
            return Json(cate);
        }

        public IActionResult GetCate()
        {
            var lsCats = _context.Categories
               .AsNoTracking()
               .Where(x => x.Published == true)
               .OrderBy(x => x.Ordering)
               .Take(18)
               .ToList();

            if (lsCats.Any())
            {
                return Json(new { success = true, data = lsCats });
            }
            else
            {
                return Json(new { success = false });
            }
        }


        public IActionResult SearchProduct(string text, int categoryId)
        {
            if(categoryId <=0)
            {
                var data = _context.Products
               .Select(s => new {
                   Id= s.ProductId,
                   Alias = s.Alias,
                   Name = s.ProductName,
                   Avatar = s.Avatar,
                   Price = s.Price,
                   SalePrice  =s.SalePrice,
               }).Where(s => s.Name.Contains(text))
               .ToList();
                
                if (data.Count() > 0)
                {
                    var toltal = data.Count();
                    return Json(new { success = true, data = data, totalProduct = toltal });
                }
            }  
            if(categoryId >0) {
                if (categoryId > 0)
                {
                    var data = _context.ProductCategories
                .Where(s => s.CatId == categoryId) // Lọc theo categoryId
                .Select(s => new
                {
                    Id = s.ProductId,
                    Alias= s.Product.Alias,
                    Name = s.Product.ProductName, 
                    Avatar = s.Product.Avatar, 
                    Price = s.Product.Price ,
                    SalePrice = s.Product.SalePrice,
                })
                .Where(s => s.Name.Contains(text)) // Lọc theo tên sản phẩm
                .ToList();

                    if (data.Count() > 0)
                    {
                        var toltal = data.Count();
                        return Json(new { success = true, data = data, totalProduct = toltal });
                    }
                }

            }
            return Json(new { success = false });
        }


        [Route("Contact", Name = "Contact")]
        public IActionResult Contact()
        {
            return View();
        }


        [Route("About", Name = "About")]
        public IActionResult About()
        {
            return View();
        }
        [Route("danh-sach-san-pham-Novazone")]
        public IActionResult Products()
        {

            var categoriesWithProducts = _context.Categories
                                                .Where(cat => cat.Published)
                                                .Take(10)
                                                .Select(cat => new ProductHomeVM
                                                {
                                                    category = cat,
                                                    lsProducts = cat.ProductCategories
                                                        .Select(pc => pc.Product)
                                                        .Where(pc => pc.IsActivated && pc.HomeFlag && pc.BestSellers)
                                                        .OrderByDescending(pc => pc.DateCreated)
                                                        .Take(15)
                                                        .ToList(),
                                                    Banner = cat.Banners.FirstOrDefault(b => b.Status == true)
                                                })
                                                .ToList();


            ViewBag.categoriesWithProducts = categoriesWithProducts;
            return View();
        }

        [Route("/gian-hang-khuyen-mai/{id}")]
        public IActionResult Discounts(int id)
        {
            // Lấy discount theo id và các sản phẩm liên kết
            var discount = _context.Discounts
                .Include(d => d.DiscountAddProducts)
                .ThenInclude(dp => dp.Product)
                .ThenInclude(p => p.ProductCategories) // Bao gồm ProductCategories để lấy danh mục sản phẩm
                .Where(d => d.Id == id)
                .FirstOrDefault();

            if (discount == null)
            {
                return NotFound(); // Nếu không tìm thấy discount
            }

            // Lấy danh sách các sản phẩm từ discount
            var products = discount.DiscountAddProducts.Select(dp => dp.Product).ToList();

            // Khởi tạo HashSet để theo dõi các sản phẩm đã được xử lý
            var processedProducts = new HashSet<int>();

            // Lấy danh sách các danh mục
            var categories = _context.Categories.ToList();

            // Chia các sản phẩm theo danh mục, đảm bảo mỗi sản phẩm chỉ xuất hiện một lần trong mỗi danh mục
            var productsByCategory = products
                .SelectMany(p => p.ProductCategories) // Lấy danh sách các danh mục của mỗi sản phẩm
                .GroupBy(pc => pc.CatId) // Nhóm theo CategoryId
                .ToDictionary(
                    g => g.Key, // Nhóm theo CatId
                    g => g.Select(pc => pc.Product)
                        .Where(p => processedProducts.Add(p.ProductId)) // Chỉ thêm sản phẩm chưa xuất hiện
                        .ToList()
                );

            // Lấy tên danh mục từ CatId và lưu vào ViewBag
            var categoryNames = categories.ToDictionary(c => c.CatId, c => c.CatName);

            // Lưu thông tin các sản phẩm theo danh mục vào ViewBag, sử dụng tên danh mục thay vì CatId
            ViewBag.ProductsByCategory = productsByCategory.ToDictionary(
                kvp => categoryNames.ContainsKey((int)kvp.Key) ? categoryNames[(int)kvp.Key] : "Không rõ", // Lấy tên danh mục từ bảng Categories
                kvp => kvp.Value
            );

            return View(discount);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
