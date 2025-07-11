using Ecommerce_CaFeShop.Models;
using Ecommerce_CaFeShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_CaFeShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly CaFeContext _context;

        public ProductController(CaFeContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? search, string? categories = "", string? brands = "", double? minPrice = null, double? maxPrice = null, string? sortBy = "", string? filterType = "", int page = 1)
        {
            var pageSize = 6;
            var products = _context.SanPhams.AsQueryable();

            // Lọc theo tìm kiếm
            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower().Trim();
                products = products.Where(p =>
                    p.TenSanPham != null && p.TenSanPham.ToLower().Contains(search) ||
                    p.MoTaNgan != null && p.MoTaNgan.ToLower().Contains(search));
            }

            // Lọc theo danh mục
            if (!string.IsNullOrEmpty(categories))
            {
                products = products.Where(p => p.DanhMuc != null && p.DanhMuc.Slug == categories);
            }

            // Lọc theo thương hiệu
            if (!string.IsNullOrEmpty(brands))
            {
                products = products.Where(p => p.ThuongHieu != null && p.ThuongHieu.Slug == brands);
            }

            // Lọc theo giá
            if (minPrice.HasValue)
            {
                products = products.Where(p => p.Gia >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.Gia <= maxPrice.Value);
            }

            // Lọc theo loại sản phẩm
            if (!string.IsNullOrEmpty(filterType))
            {
                switch (filterType)
                {
                    case "discount":
                        products = products.Where(p => p.GiaKhuyenMai.HasValue && p.GiaKhuyenMai < p.Gia);
                        break;
                    case "new":
                        var thirtyDaysAgo = DateTime.Now.AddDays(-30);
                        products = products.Where(p => p.NgayTao >= thirtyDaysAgo);
                        break;
                }
            }

            // Lọc sản phẩm hiển thị và không bị xóa
            products = products.Where(p => p.DaXoa == 0 && p.TrangThai == 1);

            // Sắp xếp
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "price-asc":
                        products = products.OrderBy(p => p.GiaKhuyenMai ?? p.Gia);
                        break;
                    case "price-desc":
                        products = products.OrderByDescending(p => p.GiaKhuyenMai ?? p.Gia);
                        break;
                    case "discount":
                        products = products.OrderByDescending(p => p.GiaKhuyenMai.HasValue ? (p.Gia - p.GiaKhuyenMai.Value) / p.Gia * 100 : 0);
                        break;
                    case "newest":
                        products = products.OrderByDescending(p => p.NgayTao);
                        break;
                    case "name":
                        products = products.OrderBy(p => p.TenSanPham);
                        break;
                    default:
                        products = products.OrderByDescending(p => p.NgayTao);
                        break;
                }
            }
            else
            {
                products = products.OrderByDescending(p => p.NgayTao);
            }

            // Lấy giá min/max cho slider
            var allProducts = await _context.SanPhams.Where(p => p.DaXoa == 0 && p.TrangThai == 1).ToListAsync();
            var minPriceAll = allProducts.Any() ? allProducts.Min(p => p.GiaKhuyenMai ?? p.Gia) : 0;
            var maxPriceAll = allProducts.Any() ? allProducts.Max(p => p.GiaKhuyenMai ?? p.Gia) : 10000000;

            // Lấy tổng số sản phẩm sau khi áp dụng các bộ lọc
            var totalProducts = await products.CountAsync();
            var totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

            // Lấy các sản phẩm cho trang hiện tại
            var result = await products
                .Include(p => p.DanhGiaSanPhams)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductVM
                {
                    ProductId = p.MaSanPham,
                    ProductName = p.TenSanPham ?? "Chưa có tên",
                    Image = string.IsNullOrEmpty(p.HinhAnh) ? "/images/default-image.jpg" : p.HinhAnh,
                    Price = p.Gia,
                    DiscountPrice = (double?)p.GiaKhuyenMai,
                    ShortDescription = p.MoTaNgan ?? "Chưa có mô tả",
                    ProductRating = p.DanhGiaSanPhams.Any() ? p.DanhGiaSanPhams.Average(r => r.DiemDanhGia ?? 0) : 0,
                    Slug = p.Slug
                })
                .ToListAsync();

            var viewModel = new PagedProductListVM
            {
                SanPhams = result,
                CurrentPage = page,
                TotalPages = totalPages,
                PageSize = pageSize
            };

            // Pass filter data to view
            ViewData["search"] = search;
            ViewData["categories"] = categories;
            ViewData["brands"] = brands;
            ViewData["sortBy"] = sortBy;
            ViewData["filterType"] = filterType;
            ViewData["minPrice"] = minPriceAll;
            ViewData["maxPrice"] = maxPriceAll;
            ViewData["currentMinPrice"] = minPrice ?? minPriceAll;
            ViewData["currentMaxPrice"] = maxPrice ?? maxPriceAll;

            return View(viewModel);
        }

        public async Task<IActionResult> SearchProduct(string? search = "", int page = 1)
        {
            var pageSize = 6; // Tăng số sản phẩm mỗi trang
            var products = _context.SanPhams.AsQueryable();

            // Lọc sản phẩm theo trạng thái và không bị xóa trước
            products = products.Where(p => p.TrangThai == 1 && p.DaXoa == 0);

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower().Trim();
                products = products.Where(p =>
                    (p.TenSanPham != null && p.TenSanPham.ToLower().Contains(search)) ||
                    (p.MoTaNgan != null && p.MoTaNgan.ToLower().Contains(search)) ||
                    (p.MoTa != null && p.MoTa.ToLower().Contains(search)) ||
                    (p.DanhMuc != null && p.DanhMuc.TenDanhMuc != null && p.DanhMuc.TenDanhMuc.ToLower().Contains(search)) ||
                    (p.ThuongHieu != null && p.ThuongHieu.TenThuongHieu != null && p.ThuongHieu.TenThuongHieu.ToLower().Contains(search)));
            }

            var totalProducts = await products.CountAsync();
            var totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

            var result = await products
                .Include(p => p.DanhGiaSanPhams)
                .Include(p => p.DanhMuc)
                .Include(p => p.ThuongHieu)
                .OrderByDescending(p => p.NgayTao) // Sắp xếp theo ngày tạo mới nhất
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductVM
                {
                    ProductId = p.MaSanPham,
                    ProductName = p.TenSanPham ?? "Chưa có tên",
                    Image = string.IsNullOrEmpty(p.HinhAnh) ? "default-image.jpg" : p.HinhAnh,
                    Price = p.Gia,
                    DiscountPrice = (double?)p.GiaKhuyenMai,
                    ShortDescription = p.MoTaNgan ?? "Chưa có mô tả",
                    ProductRating = p.DanhGiaSanPhams.Any()
                        ? p.DanhGiaSanPhams.Average(r => (double)(r.DiemDanhGia ?? 0)) : 0,
                    TotalRating = p.DanhGiaSanPhams.Count,
                    Slug = p.Slug
                }).ToListAsync();

            var viewModel = new PagedProductListVM
            {
                SanPhams = result,
                CurrentPage = page,
                TotalPages = totalPages,
                PageSize = pageSize
            };

            // Truyền từ khóa search để hiển thị lại trong view
            ViewData["SearchKeyword"] = search;
            ViewData["TotalResults"] = totalProducts;

            return View(viewModel);
        }

        [Route("ProductDetail/{slug}")]
        [Route("Product/ProductDetail/{id:int}")]
        public async Task<IActionResult> ProductDetail(string? slug = null, int? id = null)
        {
            if (string.IsNullOrEmpty(slug) && !id.HasValue)
            {
                return NotFound();
            }

            var customerIdClaim = User.Claims.FirstOrDefault(c => c.Type == "MaKhachHang");
            var customerId = customerIdClaim != null ? int.Parse(customerIdClaim.Value) : (int?)null;

            // Lấy sản phẩm, đánh giá, và bình luận từ cơ sở dữ liệu
            var product = await _context.SanPhams
                .Include(p => p.DanhMuc)
                .Include(p => p.ThuongHieu)
                .Include(p => p.HinhAnhSanPhams)
                .Include(p => p.BinhLuanSanPhams).ThenInclude(productComment => productComment.KhachHang)
                .Include(p => p.DanhGiaSanPhams)
                .ThenInclude(c => c.KhachHang)
                .FirstOrDefaultAsync(p => (!string.IsNullOrEmpty(slug) && p.Slug == slug) ||
                                         (id.HasValue && p.MaSanPham == id.Value));

            if (product == null) // Kiểm tra sản phẩm tồn tại
            {
                return NotFound();
            }

            var relatedProducts = await _context.SanPhams
                .Where(p => p.MaDanhMuc == product.MaDanhMuc && p.MaThuongHieu == product.MaThuongHieu && p.MaSanPham != product.MaSanPham)
                .Take(5)
                .ToListAsync();

            // Tạo ViewModel
            var viewModel = new ProductDetailVM
            {
                SanPham = product,
                RelatedProducts = relatedProducts,
                ProductRating = product.DanhGiaSanPhams.Any()
                    ? product.DanhGiaSanPhams.Average(r => (double)r.DiemDanhGia!) // Tính trung bình điểm đánh giá
                    : 0,
                TotalRating = product.DanhGiaSanPhams.Count, // Tổng số đánh giá
                Comments = product.BinhLuanSanPhams
                    .Select(c => new ProductCommentVM
                    {
                        CustomerName = c.KhachHang?.TenHienThi ?? "Guest", // Hiển thị tên khách
                        Content = c.NoiDung,
                        CreatedAt = c.NgayTao,
                        Rating = product.DanhGiaSanPhams.FirstOrDefault(r => r.MaKhachHang == c.MaKhachHang)?.DiemDanhGia
                    }).ToList(),
            };

            return View(viewModel); // Trả về View
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview(int id, string content, int rating)
        {
            // Check authentication and get customer ID
            var customerIdClaim = User.Claims.FirstOrDefault(c => c.Type == "MaKhachHang");
            if (customerIdClaim == null || !int.TryParse(customerIdClaim.Value, out int customerId))
            {
                return Json(new { success = false, message = "Bạn cần đăng nhập để đánh giá sản phẩm." });
            }

            // Validate input
            if (string.IsNullOrWhiteSpace(content))
            {
                return Json(new { success = false, message = "Nội dung bình luận không được để trống." });
            }

            if (rating < 1 || rating > 5)
            {
                return Json(new { success = false, message = "Điểm đánh giá phải từ 1 đến 5." });
            }

            // Check if product exists with active status
            var product = await _context.SanPhams
                .Where(p => p.MaSanPham == id && p.DaXoa == 0 && p.TrangThai == 1)
                .FirstOrDefaultAsync();
            if (product == null)
            {
                return Json(new { success = false, message = "Sản phẩm không tồn tại hoặc đã bị xóa." });
            }

            // Check if customer exists
            var customer = _context.KhachHangs.Find(customerId);
            if (customer == null)
            {
                return Json(new { success = false, message = "Thông tin khách hàng không hợp lệ." });
            }

            try
            {
                // Check for existing review
                var existingReview = _context.BinhLuanSanPhams
                    .Any(bl => bl.MaSanPham == id && bl.MaKhachHang == customerId);
                if (existingReview)
                {
                    return Json(new { success = false, message = "Bạn đã đánh giá sản phẩm này rồi." });
                }

                // Create comment
                var comment = new BinhLuanSanPham
                {
                    MaSanPham = id,
                    MaKhachHang = customerId,
                    NoiDung = content.Trim(),
                    NgayTao = DateTime.Now // Current time: 11:49 PM +07, July 05, 2025
                };

                // Create rating
                var productRating = new DanhGiaSanPham
                {
                    MaSanPham = id,
                    MaKhachHang = customerId,
                    DiemDanhGia = rating
                };

                // Add to context and save changes
                _context.BinhLuanSanPhams.Add(comment);
                _context.DanhGiaSanPhams.Add(productRating);
                await _context.SaveChangesAsync();

                // Fetch customer name for response
                var customerName = customer.TenHienThi ?? "Guest";

                // Return success with data for client-side rendering
                return Json(new
                {
                    success = true,
                    message = "Đánh giá của bạn đã được gửi thành công!",
                    review = new
                    {
                        rating,
                        content = content.Trim(),
                        date = DateTime.Now.ToString("dd/MM/yyyy HH:mm"), // e.g., 05/07/2025 23:49
                        customerName = customerName
                    }
                });
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException?.Message ?? "No inner exception details.";
                // Log for debugging (e.g., using Serilog: _logger.LogError(ex, "Database update error: {Message}", innerException));
                return Json(new { success = false, message = $"Có lỗi xảy ra khi lưu dữ liệu: {innerException}" });
            }
            catch (Exception ex)
            {
                // Log the exception
                // _logger.LogError(ex, "Error adding review for product {id}", id);
                return Json(new { success = false, message = "Có lỗi xảy ra khi gửi đánh giá: " + ex.Message });
            }
        }

        // Thêm sản phẩm yêu thích
        [HttpPost]
        public IActionResult AddToWishlist(int productId)
        {
            var customerIdClaim = User.Claims.FirstOrDefault(c => c.Type == "MaKhachHang");
            if (customerIdClaim == null)
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập." });
            }

            var customerId = int.Parse(customerIdClaim.Value);
            var product = _context.SanPhams.Find(productId);
            if (product == null)
            {
                return Json(new { success = false, message = "Sản phẩm không tồn tại." });
            }

            var existingWishlist = _context.YeuThichs
                .FirstOrDefault(yt => yt.MaKhachHang == customerId && yt.MaSanPham == productId);

            if (existingWishlist == null)
            {
                var wishlistItem = new YeuThich
                {
                    MaKhachHang = customerId,
                    MaSanPham = productId
                };
                _context.YeuThichs.Add(wishlistItem);
                _context.SaveChanges();
                return Json(new { success = true, message = "Thêm vào yêu thích thành công.", isFavorite = true });
            }
            else
            {
                _context.YeuThichs.Remove(existingWishlist);
                _context.SaveChanges();
                return Json(new { success = true, message = "Xóa khỏi yêu thích thành công.", isFavorite = false });
            }
        }
    }
}