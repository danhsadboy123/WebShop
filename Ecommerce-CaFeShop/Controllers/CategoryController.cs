using Ecommerce_CaFeShop.Models;
using Ecommerce_CaFeShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_CaFeShop.Controllers
{

    public class CategoryController : Controller
    {
        private readonly CaFeContext _context;

        public CategoryController(CaFeContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string slug = "", int page = 1)
        {
            // Nếu không có slug, chuyển về trang sản phẩm
            if (string.IsNullOrEmpty(slug))
            {
                return RedirectToAction("Index", "Product");
            }

            // Kiểm tra danh mục tồn tại
            var category = await _context.DanhMucs
                .Where(c => c.Slug == slug && c.DaXoa == 0)
                .FirstOrDefaultAsync();

            if (category == null)
            {
                TempData["ErrorMessage"] = $"Không tìm thấy danh mục với slug: {slug}";
                return RedirectToAction("Index", "Product");
            }

            var pageSize = 8;

            // Lấy sản phẩm theo danh mục với phân trang
            var products = _context.SanPhams
                .Where(p => p.MaDanhMuc == category.MaDanhMuc && p.DaXoa == 0 && p.TrangThai == 1)
                .Include(p => p.DanhGiaSanPhams)
                .Include(p => p.DanhMuc) // Include để debug
                .OrderByDescending(p => p.NgayTao); // Sắp xếp theo ngày tạo mới nhất

            var totalProducts = await products.CountAsync();

            // Debug: Kiểm tra số lượng sản phẩm tìm thấy
            Console.WriteLine($"Danh mục: {category.TenDanhMuc} (ID: {category.MaDanhMuc})");
            Console.WriteLine($"Tổng số sản phẩm tìm thấy: {totalProducts}");

            var totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

            var result = await products
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductVM
                {
                    ProductId = p.MaSanPham,
                    ProductName = p.TenSanPham ?? "Chưa có tên",
                    Image = string.IsNullOrEmpty(p.HinhAnh) ? "/Images/default-image.jpg" : p.HinhAnh,
                    Price = p.Gia,
                    DiscountPrice = p.GiaKhuyenMai,
                    DiscountPercent = p.PhanTramGiam,
                    ShortDescription = p.MoTaNgan ?? "Chưa có mô tả",
                    ProductRating = p.DanhGiaSanPhams.Any()
                        ? p.DanhGiaSanPhams.Average(r => (double)r.DiemDanhGia!)
                        : 0,
                    TotalRating = p.DanhGiaSanPhams.Count,
                    Slug = p.Slug
                }).ToListAsync();

            // Tạo ViewModel cho phân trang
            var viewModel = new PagedProductListVM
            {
                SanPhams = result,
                CurrentPage = page,
                TotalPages = totalPages,
                PageSize = pageSize
            };

            // Truyền thông tin danh mục vào ViewBag
            ViewBag.CategoryName = category.TenDanhMuc;
            ViewBag.CategorySlug = category.Slug;
            ViewBag.TotalProducts = totalProducts;

            return View(viewModel);
        }
    }
}