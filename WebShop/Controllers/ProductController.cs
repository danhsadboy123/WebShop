using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NPOI.HSSF.Record.Chart;
using NPOI.SS.Formula.Functions;
using PagedList.Core;
using WebShop.Areas.Admin.Models;
using WebShop.Extension;
using WebShop.Models;
using WebShop.ModelViews;
using Attribute = WebShop.Models.ThuocTinh;

namespace WebShop.Controllers
{
    public class ProductController : BaseController
    {
        private readonly DbMarketsContext _context;
        private readonly IMemoryCache _cache; //khai báo biến để sử dụng bộ nhớ cache trình duyệt
        public ProductController(DbMarketsContext context, IMemoryCache cache) : base(context)
        {
            _context = context;
            _cache = cache;
        }
        //lấy sản phẩm theo danh mục sản phẩm, phân trang, gán các bộ lọc vào section để lọc sản phẩm
        [Route("{Alias}", Name = ("ListProduct"))]
        public IActionResult ListProductByCategory(string Alias, string brand, string attribute, int pageNumber = 1)
            {
            try
            {
                int pageSize = 24;

                // Truy vấn danh mục với các thương hiệu
                var danhmuc = _context.Categories
                    .Include(c => c.CategoryBrands.Where(cb => cb.Cat.Alias == Alias))
                    .ThenInclude(cb => cb.Brand)
                    .AsNoTracking() // chỉ đọc dữ liệu
                    .FirstOrDefault(c => c.Alias == Alias);

                if (danhmuc == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                // Truy vấn sản phẩm
                IQueryable<Product> productQuery = _context.Products
                    .Include(p => p.AttributesPrices)
                    .AsNoTracking()
                    .Where(p => p.ProductCategories.Any(pc => pc.CatId == danhmuc.CatId))
                     .OrderByDescending(p => p.DateCreated); // Sắp xếp theo ngày tạo giảm dần;

                // Thêm điều kiện lọc theo thương hiệu
                var section = HttpContext.Session.Get<searchAlias>(Alias) ?? new searchAlias();
                HttpContext.Session.Set(Alias, section); // Lưu lại đối tượng vào session

                if (section.brand != null && section.brand.Count > 0)
                {
                    var brandIds = section.brand.Select(b => b.BrandId).ToList();
                    productQuery = productQuery.Where(p => brandIds.Contains((int)p.BrandId));
                }

                // Thêm điều kiện lọc theo thuộc tính
                if (section.attrp != null && section.attrp.Count > 0)
                {
                    var attrPrices = section.attrp.Select(ap => ap.Price).ToList();
                    productQuery = productQuery.Where(p => p.AttributesPrices.Any(ap => attrPrices.Contains(ap.Price)));
                }

                // Sắp xếp sản phẩm theo điều kiện
                switch (section.sort)
                {
                    case "asc":
                        productQuery = productQuery.OrderBy(p => p.Price);
                        break;
                    case "desc":
                        productQuery = productQuery.OrderByDescending(p => p.Price);
                        break;
                    case "new":
                        productQuery = productQuery.OrderByDescending(p => p.DateCreated);
                        break;
                    case "az":
                        productQuery = productQuery.OrderBy(p => p.ProductName);
                        break;
                    case "discount":
                    default:
                        var discountProductIds = _context.DiscountAddProducts
                            .Include(d => d.Discount)
                            .AsNoTracking()
                            .Where(d => d.Discount.ShowWeb && d.Discount.Startus && d.Discount.TimeOff > DateTime.Now)
                            .Select(d => d.ProductId)
                            .ToList();

                        productQuery = productQuery.OrderByDescending(p => discountProductIds.Contains(p.ProductId));
                        break;
                }

                // Lấy dữ liệu sản phẩm đã lọc
                var lsProducts = productQuery
                    .Select(p => new Product
                    {
                        ProductId = p.ProductId,
                        ProductName = p.ProductName,
                        ProductCode = p.ProductCode,
                        SalePrice = p.SalePrice,
                        Price = p.Price,
                        HomeFlag = p.HomeFlag,
                        Alias = p.Alias,
                        UnitsInStock = p.UnitsInStock,
                        Avatar = p.Avatar,
                        DateCreated = p.DateCreated
                    })
                    .OrderByDescending(p=>p.DateCreated)
                    .ToPagedList(pageNumber, pageSize); // Sử dụng phân trang

                if(lsProducts.Count()<=0)
                {
                    return RedirectToAction("Index", "Home");
                }    

                // Truy vấn các thương hiệu liên quan
                var brands = danhmuc.CategoryBrands
                    .Where(cb => cb.CatId == danhmuc.CatId)
                    .Select(cb => new ThuongHieu
                    {
                        BrandName = cb.Brand.BrandName,
                        BrandId = cb.Brand.BrandId
                    })
                    .ToList();  
                // Tối ưu truy vấn thuộc tính
                var atts = _context.Attributes
                    .Include(a => a.CategoryAttributes)
                    .Include(a => a.AttributesPrices)
                    .AsNoTracking()
                    .Where(a => a.CategoryAttributes.Any(ca => ca.CatId == danhmuc.CatId) && a.AttributesPrices.Any(ap => ap.KichHoat) && a.KichHoat)
                    .OrderBy(a => a.Ordering)
                    .ToList();

                // Breadcrumb
                var breadcrumbCategories = GetBreadcrumbCategories(danhmuc.CatId);


                //lấy slider theo danh mục
                var slider = _context.Slides.Where(s => s.KichHoat == true && s.Right == true && s.Bottom == false && s.HomeFlag == false && s.CatId == danhmuc.CatId).ToList();

                ViewBag.Slider = slider; 
                // ViewBag setup
                ViewBag.groupbrand = section.brandGroup;
                ViewBag.CurrentPage = pageNumber;
                ViewBag.CurrentCat = danhmuc; 
                ViewBag.Brands = brands;
                ViewBag.Attrs = atts;
                ViewBag.BreadcrumbCategories = breadcrumbCategories;
                
                return View(lsProducts);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }  
        }


        public IActionResult ListProductByBrand(int id,string brandname, int page = 1)
        {
            try
            { 
                var pageSize = 30;
                var brand = _context.Brands
                    .Where(c => c.BrandName == brandname)
                    .Where(c=>c.BrandId==id)
                    .Include(c=>c.CategoryBrands)
                    .ThenInclude(c=>c.Cat)
                    .FirstOrDefault();

                // search theo category
                if (HttpContext.Session.Get<searchBrand>(brand.BrandName) == null)
                {
                    var searchAliases = new searchBrand();
                    HttpContext.Session.Set(brand.BrandName, searchAliases);
                }
                var section = HttpContext.Session.Get<searchBrand>(brand.BrandName);
                 
                var productcheck = _context.Products
                    .Where(c=>c.BrandId==brand.BrandId)
                    .Include(c=>c.ProductCategories)
                    .Include(c => c.DiscountAddProducts)
                    .Include(c => c.AttributesPrices)
                    .ToList();

                if (section.categories != null)
                {
                    productcheck =productcheck.Where(c=>c.ProductCategories.Any(c=>c.CatId==section.categories.CatId)).ToList();
                }

                IQueryable<Product> lsProducts = productcheck.AsQueryable();
                // lay list thuoc tinh cua product
                var listdiscounts = _context.DiscountAddProducts
                    .Include(p => p.Discount)
                    .Include(p => p.Product)
                    .Where(p => p.Discount.ShowWeb == true)
                    .Where(p => p.Discount.Startus == true)
                    .Where(p => p.Discount.TimeOff > DateTime.Now)
                    .ToList();
               

                // Sắp xếp danh sách sản phẩm theo ngày tạo giảm dần (mới nhất lên đầu)
                if (section.sort == null || section.sort == "discount")
                {
                    foreach (var dis in listdiscounts)
                    {
                        lsProducts = lsProducts.OrderByDescending(c => c.ProductId == dis.ProductId);
                    } 
                }
                if (section.sort == "asc")
                {
                    lsProducts = lsProducts.OrderBy(c => c.Price);
                }
                if (section.sort == "desc")
                {
                    lsProducts = lsProducts.OrderByDescending(c => c.Price);
                }
                if (section.sort == "new")
                {
                    lsProducts = lsProducts.OrderByDescending(x => x.DateCreated);
                }
                if (section.sort == "az")
                {
                    lsProducts = lsProducts.OrderByDescending(x => x.ProductName);
                }
                lsProducts = lsProducts
                    .Select(p => new Product
                    {
                        ProductId = p.ProductId,
                        ProductName = p.ProductName,
                        ProductCode = p.ProductCode,
                        SalePrice = p.SalePrice,
                        HomeFlag = p.HomeFlag,
                        Alias = p.Alias,
                        UnitsInStock = p.UnitsInStock,
                        Avatar = p.Avatar,
                        DateCreated = p.DateCreated,
                    });
                PagedList<Product> models = new PagedList<Product>(lsProducts, page, pageSize);
                ViewBag.Brand = brand;
                ViewBag.CurrentPage = page;
                ViewBag.listdiscount = listdiscounts;
                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult checkSearchBrand(int id, int cateid)
        {
            // Tìm thương hiệu theo ID
            var brand = _context.Brands.FirstOrDefault(c => c.BrandId == id);

            if (brand != null) // Kiểm tra xem thương hiệu có tồn tại không
            {
                // Lấy đối tượng searchAlias từ session hoặc khởi tạo mới nếu không tồn tại
                var section = HttpContext.Session.Get<searchAlias>("searchAlias")
                              ?? new searchAlias
                              {
                                  brand = new List<ThuongHieu>() // Khởi tạo danh sách brand
                              };

                // Thêm thương hiệu vào danh sách nếu chưa có
                if (!section.brand.Any(b => b.BrandId == brand.BrandId))
                {
                    section.brand.Add(new ThuongHieu
                    {
                        BrandId = brand.BrandId,
                        BrandName = brand.BrandName
                    });
                }

                // Cập nhật danh mục
                var cate = _context.Categories.FirstOrDefault(c => c.CatId == cateid);
                section.brandGroup = cate != null ? new NhomThuongHieu
                {
                    // Cấu hình BrandGroup ở đây nếu cần
                    // Ví dụ: Name = cate.CategoryName
                } : null;

                // Lưu lại đối tượng searchAlias vào session
                HttpContext.Session.Set("searchAlias", section);
            }

            return Json(new { success = true });
        }




        public IActionResult checkSearchSort(int id, string name)
        {
            var brand = _context.Brands.Where(c => c.BrandId == id).FirstOrDefault();
            if (HttpContext.Session.Get<searchBrand>(brand.BrandName) == null)
            {
                var searchAliases = new searchBrand();
                HttpContext.Session.Set(brand.BrandName, searchAliases);
            }

            var section = HttpContext.Session.Get<searchBrand>(brand.BrandName);            
            if (name != null)
            {
                section.sort = name;
            }
            else
            {
                section.sort = null;
            }
            HttpContext.Session.Set(brand.BrandName, section);
            return Json(new { success = true });
        }
        public IActionResult loadSearch(int id)
        {
            var brand = _context.Brands.Where(c => c.BrandId == id).FirstOrDefault();
            var section = HttpContext.Session.Get<searchBrand>(brand.BrandName);
            return Json(section);
        }
        public IActionResult loadcheck(string alias)
        {
            var section = HttpContext.Session.Get<searchAlias>(alias);
            return Json(section);
        }
        public IActionResult checkItemsearch(int id,string alias)
        {
            var dataAttribute =_context.AttributesPrices.Where(ap=>ap.AttributesPriceId ==id).FirstOrDefault();
            var section = HttpContext.Session.Get<searchAlias>(alias);
            // listprice
            List<GiaThuocTinh> pricelist = new List<GiaThuocTinh>();
            
            bool checkidp = false;
            if (section.attrp != null)
            {
                pricelist = section.attrp;
                foreach (var pr in pricelist)
                {
                    if(pr.AttributesPriceId == id)
                    {
                        checkidp = true;
                        pricelist.Remove(pr);
                        break;
                    }
                }
            }
           
            if (checkidp == false)
            {
                var prices = _context.AttributesPrices.FirstOrDefault(i=>i.AttributesPriceId == id);
                if (prices != null)
                {
                    pricelist.Add(prices);
                }

            }
            section.attrp = pricelist;             
            HttpContext.Session.Set(alias, section);
            return Json(new {success="OK",data= section });
        }
        //lọc brand trong phần danh mục
        public IActionResult checkItembrand(int id, string alias)
        {
            var section = HttpContext.Session.Get<searchAlias>(alias);
            // listprice
            List<ThuongHieu> brandlist = new List<ThuongHieu>();
            bool checkidp = false;
            if (section.brand != null)
            {
                brandlist = section.brand;
                var brandg = section.brandGroup;
                foreach (var pr in brandlist)
                {
                    if (pr.BrandId == id)
                    {
                        checkidp = true;
                        if(brandg!=null && brandg.BrandId == id)
                        {
                            section.brandGroup = null;
                        }
                        brandlist.Remove(pr);
                        break;
                    }
                }
            }           
            if (checkidp == false)
            {
                var brands = _context.Brands.FirstOrDefault(i => i.BrandId == id);
                if (brands != null)
                {
                    brandlist.Add(brands);
                } 
            }
            section.brand = brandlist;
            HttpContext.Session.Set(alias, section);
            return Json(new { success = "OK", data = section });
        }
        //lọc brand phần nav menu
        public IActionResult checkItembrandg(int id, string alias)
        {
            var section = HttpContext.Session.Get<searchAlias>(alias);
            section.brandGroup = null;
            HttpContext.Session.Set(alias, section);
            return Json(new { success = "OK", data = section });
        }
        public IActionResult catebreand(int id, string alias, int groupid)
        {
            if (HttpContext.Session.Get<searchAlias>(alias) == null)
            {
                var searchAliases = new searchAlias();
                HttpContext.Session.Set(alias, searchAliases);
            }
            var section = HttpContext.Session.Get<searchAlias>(alias);
            var cate = _context.Categories.Where(c => c.Alias == alias).FirstOrDefault();
            var group = _context.BrandGroups.Where(c => c.Id == groupid).FirstOrDefault();
            // listprice
            List<ThuongHieu> brandlist = new List<ThuongHieu>();
            bool checkidp = false;
            if (section.brand != null)
            {
                brandlist = section.brand;
                foreach (var pr in brandlist)
                {
                    if (pr.BrandId == id)
                    {
                        checkidp = true;
                        brandlist.Remove(pr);
                        break;
                    }
                }

            }
            if (checkidp == false)
            {
                var brands = _context.Brands.FirstOrDefault(i => i.BrandId == id);

                if (cate != null && brands != null)
                {
                    var product = _context.Products
                        .Include(c => c.Brand)
                        .Where(c => c.BrandId == brands.BrandId)
                        .Include(c => c.ProductCategories)
                        .ToList();
                    var count = 0;
                    foreach (var pr in product)
                    {
                        if (pr.ProductCategories.Any(c => c.Cat == cate))
                        {
                            if (count != 0) { break; }
                            count++;

                        }
                    }
                    if (count != 0) { brandlist.Add(brands); }

                }
            }
            if (group != null)
            {
                brandlist = _context.Brands.Where(c => c.BrandId == group.BrandId).ToList();
            }

            section.brand = brandlist;
            section.brandGroup = group;
            HttpContext.Session.Set(alias, section);
            return RedirectToAction("ListProductByCategory", new { Alias = alias });
        }


        //fillter phần menu Navbar
        [Route("/loc-san-pham/{id}/{Alias}")]
        public IActionResult cateatribute(int id, string alias)
        {
            // Kiểm tra session  
            var section = HttpContext.Session.Get<searchAlias>(alias) ?? new searchAlias();

            // Danh sách giá thuộc tính nếu chưa có tron session thì tạo mới
            List<GiaThuocTinh> pricelist = section.attrp ?? new List<GiaThuocTinh>();

            // Kiểm tra xem id thuộc tính có tồn tại trong danh sách của section hay không
            bool checkidp = pricelist.Any(pr => pr.AttributesPriceId == id);

            // Nếu tồn tại thì xóa khỏi danh sách, nếu không thì thêm vào
            if (checkidp)
            {
                pricelist = pricelist.Where(pr => pr.AttributesPriceId != id).ToList();
            }
            else
            {
                var prices = _context.AttributesPrices.FirstOrDefault(i => i.AttributesPriceId == id);
                if (prices != null)
                {
                    pricelist.Add(prices);
                }
            }

            // Cập nhật lại session với danh sách giá trị mới
            section.attrp = pricelist;
            HttpContext.Session.Set(alias, section);

            // Chuyển hướng với alias và các tham số khác (brand và attribute nếu có)
            return RedirectToAction("ListProductByCategory", new { Alias = alias });
        }
        public IActionResult checkItemSort(string sort, string alias) 
        {
            var section = HttpContext.Session.Get<searchAlias>(alias);
            if (section.sort == null) {
                section.sort = sort;
            }
            else
            {
                if (section.sort == sort)
                {
                    section.sort = null;
                }
                else
                { 
                    section.sort = sort;                
                }
            }
            HttpContext.Session.Set(alias, section);
            return Json(new { success = "OK", data = section });
        } 
        [Route("{Alias}-{id:int}", Name = ("ProductDetails"))]
        public IActionResult Details(int id)
        {
            var product = _context.Products
                .Include(x => x.Brand)
                    .ThenInclude(c => c.CategoryBrands)
                .Include(x => x.ProductCategories)                
                .Where(x=>x.ProductId==id)
                .FirstOrDefault();
            int caid = 0;
            if (product != null)
            {
                foreach (var item in product.Brand.CategoryBrands)
                {
                    if (caid == 0)
                    {
                        caid = (int)item.CatId;
                    }
                }
            }
            //var listprooption = _context.Products.Where(c => c.ProductOption == product.ProductOption).Where(c=>c.ProductOption!=null).ToList();
            //ViewBag.listprooption = listprooption;
            try
            {
                var attributePrices = _context.AttributesPrices
                                                            .AsNoTracking()
                                                            .Where(c => c.ProductId == product.ProductId)
                                                            .GroupBy(x => new { x.AttributeId, x.Attribute.Name })
                                                            .Select(g => new AttributePriceDto
                                                            {
                                                                AttributeId = (int)g.Key.AttributeId,
                                                                AttributeName = g.Key.Name,
                                                                // Kết hợp các giá trị Price thành một chuỗi, phân cách bởi dấu phẩy
                                                                Price = string.Join(", ", g.Select(x => x.Price.ToString()))
                                                            })
                                                            .ToList();

                //var attr = _context.CategoryAttributes.Where(c=>c.CatId==caid)
                //    .Include(c=>c.Attribute)
                //    .Select(c=>c.Attribute)
                //    .OrderBy(c=>c.Ordering)
                //    .ToList();

                var images = _context.ProductThumbs.Where(x => x.ProductId == id).OrderByDescending(x => x.Ordering).ToList();

                // Lấy danh sách breadcrumb dựa vào CatId               

                //var breadcrumbCategories = GetBreadcrumbCategories(caid);
                var cateid = product.ProductCategories.FirstOrDefault();        
                var breadcrumbCategories = _context.Categories.Where(c => c.CatId == cateid.CatId).FirstOrDefault();

                if (product == null)
                {
                    return RedirectToAction("Index");
                }
                // Lấy CatId của sản phẩm hiện tại (giả sử CatId lưu trong ProductCategories)
                //int currentCatId = (int)product.ProductCategories.FirstOrDefault().CatId;

                // Lấy danh sách sản phẩm cùng loại và cùng thương hiệu
                var lsProduct = _context.Products
                                .AsNoTracking()
                                .Where(x =>
                                    x.ProductCategories.Any(pc => pc.CatId == cateid.CatId) && // Lọc theo cùng danh mục
                                    x.ProductId != id && // Loại bỏ sản phẩm hiện tại
                                    x.KichHoat == true && // Sản phẩm còn hoạt động
                                    x.BrandId == product.BrandId // Lọc theo cùng hãng
                                )
                                .Select(p => new Product
                                {
                                    ProductId = p.ProductId,
                                    ProductName = p.ProductName,
                                    ProductCode = p.ProductCode,
                                    SalePrice = p.SalePrice,
                                    Price = p.Price,
                                    HomeFlag = p.HomeFlag,
                                    Alias = p.Alias,
                                    UnitsInStock = p.UnitsInStock,
                                    Avatar = p.Avatar,
                                    DateCreated = p.DateCreated,
                                    Video = p.Video,
                                })
                                .OrderByDescending(x => x.DateCreated) // Sắp xếp theo ngày tạo 
                                .ToList();

                var discountcheck = _context.DiscountAddProducts
                    .Include(p => p.Discount)
                    .Where(i => i.ProductId == id)
                    .Where(p => p.Discount.ShowWeb == true)
                    .Where(p => p.Discount.Startus == true)
                    .Where(p => p.Discount.TimeOff > DateTime.Now)
                    .FirstOrDefault();

                var discount = _context.Discounts.Where(i => i.TimeOff > DateTime.Now)
                    .Where(i => i.ShowWeb == true)
                    .Where(i => i.Startus == true)
                    .Where(i => i.Name != null)
                    .Include(p => p.DiscountAddProducts)
                        .ThenInclude(p => p.Product)
                    .Include(p => p.GitAttributes)
                        .ThenInclude(p => p.ProductGift)
                    .ToList();

                var gift = _context.GitAttributes.Include(p => p.Discount)
                       .Include(p => p.ProductGift)
                           .Where(i => i.Discount.TimeOff > DateTime.Now)
                           .Where(i => i.Discount.ShowWeb == true)
                           .Where(i => i.Discount.Startus == true)
                           .Where(i => i.Discount.Name != null)
                    .ToList();
                if (discountcheck != null)
                {
                    gift = gift.Where(d => d.Discount.Id == discountcheck.DiscountId).ToList();
                    ViewBag.Gift = gift;
                }

                decimal minPrice = (decimal)product.Price - 1250000; // Mức giá tối thiểu
                decimal maxPrice = (decimal)product.Price + 1250000; // Mức giá tối đa

                var lsProductByCate = _context.Products
                                                .AsNoTracking()
                                                .Where(x =>
                                                    x.ProductCategories.Any(pc => pc.CatId == cateid.CatId) && // Lọc theo danh mục
                                                    x.ProductId != id && // Loại bỏ sản phẩm hiện tại
                                                    x.KichHoat == true && // Sản phẩm còn hoạt động
                                                    x.Price >= minPrice && x.Price <= maxPrice // Lọc theo khoảng giá
                                                )
                                                .Select(p => new Product
                                                {
                                                    ProductId = p.ProductId,
                                                    ProductName = p.ProductName,
                                                    ProductCode = p.ProductCode,
                                                    SalePrice = p.SalePrice,
                                                    Price = p.Price,
                                                    HomeFlag = p.HomeFlag,
                                                    Alias = p.Alias,
                                                    UnitsInStock = p.UnitsInStock,
                                                    Avatar = p.Avatar,
                                                    DateCreated = p.DateCreated,
                                                    Video = p.Video,
                                                })
                                                .OrderByDescending(x => x.DateCreated) // Sắp xếp theo ngày tạo 
                                                .ToList();

                var newpost = _context.Posts.Where(p => p.IsHot == true && p.IsNewfeed == true && p.Published == true).OrderBy(d=>d.CreatedDate).Take(6).ToList();
                ViewBag.newpost = newpost;

                ViewBag.lsProductByCate = lsProductByCate;
                ViewBag.discountcheck = discountcheck;
                ViewBag.discountHome = discount;
                ViewBag.BreadcrumbCategories = breadcrumbCategories;
                ViewBag.SanPham = lsProduct;
                ViewBag.Images = images;
                //ViewBag.attr = attr;
                ViewBag.attrbutePrices= attributePrices;
                return View(product);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
        // Phương thức để lấy danh sách breadcrumb
        public List<DanhMuc> GetBreadcrumbCategories(int? categoryId)
        {
            List<DanhMuc> breadcrumb = new List<DanhMuc>();
            var category = _context.Categories.Find(categoryId);
            while (category != null)
            {
                breadcrumb.Insert(0, category);
                category = _context.Categories.Find(category.ParentId);
            }
            return breadcrumb;
        }

        //tìm kiếm sản phẩm 
        public async Task<IActionResult> Search(int? cateId, string textSearch)
        {
            // Khởi tạo truy vấn với danh sách sản phẩm
            var query = _context.ProductCategories.Where(p=>p.Product.Avatar!=null).AsQueryable();

            // Kiểm tra xem cateId có phải là 0 hay không
            if (cateId.HasValue && cateId.Value > 0)
            {
                // Nếu cateId không phải là 0, tìm kiếm sản phẩm theo danh mục
                query = query.Where(p => p.CatId == cateId.Value);
            }

            // Tìm kiếm sản phẩm theo tên nếu textSearch có giá trị
            if (!string.IsNullOrEmpty(textSearch))
            {
                query = query.Where(p => p.Product.ProductName.Contains(textSearch));
            }

            // Chỉ chọn các trường cần thiết (ví dụ: Id và Name)
            var result = await query.Select(p => new
            {
                p.Product.ProductId,
                p.Product.ProductName,
                p.Product.Alias,
                p.Product.Avatar,
                p.Product.Price,
                p.Product.SalePrice 
            }).ToListAsync();

            // Trả về kết quả dưới dạng JSON
            return Json(new { data = result });
        }
        
        //ajax gọi load thuộc tính theo danh mục
        [HttpGet]         
        public IActionResult loadAttributeByCateId(int id)
        {
            var cacheKey = $"Attributes_CatId_{id}";
            if (!_cache.TryGetValue(cacheKey, out List<AttributeViewModel> attributes))
            {
                // Truy vấn dữ liệu từ database
                attributes = _context.CategoryAttributes
                    .Where(ca => ca.CatId == id && ca.KichHoat ==true) // Lọc theo CatId và KichHoat
                    .OrderBy(ca => ca.Attribute.Ordering) // Sắp xếp theo thứ tự
                    .Select(ca => new AttributeViewModel
                    {
                        AttributeId = ca.Attribute.AttributeId,
                        AttributeName = ca.Attribute.Name,
                        AttributesPrices = ca.Attribute.AttributesPrices
                            .Where(ap => ap.ProductId == null) // Lọc giá mà không gắn với ProductId
                            .Select(ap => new AttributePriceViewModel
                            {
                                PriceId = ap.AttributesPriceId,
                                Price = ap.Price,
                                AttributeId = ap.Attribute.AttributeId,
                            }).ToList()
                    })
                    .ToList();

                // Thiết lập thời gian hết hạn cho cache (ví dụ: 5 phút)
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };

                // Lưu dữ liệu vào cache
                _cache.Set(cacheKey, attributes, cacheOptions);
            }

            // Trả về dữ liệu từ cache (hoặc từ database nếu cache không có)
            return Json(new { attributes });

        }
        //ajax gọi load brand theo danh mục

        [HttpGet]
        public IActionResult loadBrandByCategoryId(int id)
        {
            // Tạo một key duy nhất cho cache theo id danh mục
            string cacheKey = $"CategoryBrands_{id}";

            // Kiểm tra xem dữ liệu đã có trong cache chưa
            if (!_cache.TryGetValue(cacheKey, out List<object> data))
            {
                // Nếu chưa có, truy vấn cơ sở dữ liệu
                data = _context.CategoryBrands
                                .Where(c => c.CatId == id && c.KichHoat == true)
                                .Take(18)
                                .Select(c => new
                                {
                                    Id = c.BrandId,
                                    Thumb = c.Brand.Thumb,
                                    Alias = c.Cat.Alias,
                                    Poster = c.Cat.BannerThumb
                                })
                                .ToList<object>();

                // Cấu hình thời gian lưu trữ cache (ví dụ: 5 phút)
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };

                // Lưu dữ liệu vào cache
                _cache.Set(cacheKey, data, cacheEntryOptions);
            }

            // Trả về dữ liệu từ cache
            return Json(new { data });
        }


        [HttpGet]
        public IActionResult loadProductNavByCateId(int id)
        {
            var data = _context.Slides
                           .Where(s => s.CatId == id && s.KichHoat == true)
                           .OrderBy(s => s.Ordering)
                           .Select(s => new SlideVM
                           {
                               SlideId = s.SlideId,
                               Thumb = s.Thumb,
                               Alias = s.Alias,
                               SlideName = s.SlideName,
                               KichHoat = s.KichHoat
                           }).ToList();
            return Json(new { data });
        }



        //public List<Product> GetProductsByCategoryAndDescendants(int categoryId)
        //{
        //    // Lấy danh mục cha
        //    var category = _context.Categories.Find(categoryId);

        //    // Lấy tất cả danh mục từ cơ sở dữ liệu
        //    var allCategories = _context.Categories.ToList();

        //    // Lấy danh sách các CatId của các danh mục con của danh mục cha
        //    List<int> descendantCategoryIds = new List<int>();
        //    GetDescendantCategoryIds(category, allCategories, ref descendantCategoryIds);

        //    // Thêm CatId của danh mục cha vào danh sách
        //    descendantCategoryIds.Add(categoryId);

        //    // Lấy danh sách sản phẩm có CatId là categoryId của danh mục cha hoặc là các CatId của các danh mục con của nó
        //    var products = _context.Products
        //            .AsNoTracking()
        //            .Include(x => x.ProductThumbs)
        //            .Include(x => x.AttributesPrices)
        //            .Include(x=>x.Brand)
        //            .Where(x => x.KichHoat == true)
        //            .Where(p => descendantCategoryIds.Contains((int)p.CatId))
        //            .ToList();

        //    return products;
        //}

        // Phương thức đệ quy để lấy danh sách các CatId của các danh mục con của danh mục cha
        private void GetDescendantCategoryIds(DanhMuc category, List<DanhMuc> allCategories, ref List<int> descendantCategoryIds)
        {
            if (category != null)
            {
                descendantCategoryIds.Add(category.CatId);
                var subcategories = allCategories.Where(c => c.ParentId == category.CatId).ToList();
                if (subcategories.Count > 0)
                {
                    foreach (var subcategory in subcategories)
                    {
                        GetDescendantCategoryIds(subcategory, allCategories, ref descendantCategoryIds);
                    }
                }
            }
        }

        

    }
}
