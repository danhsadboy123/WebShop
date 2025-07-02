using Ecommerce_WatchShop.Helper;
using Ecommerce_WatchShop.Models;

using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;



namespace DongHo_Admin.Areas.Admin.Controllers

{

    [Area("Admin")]

    [Authorize(Policy = "Admin")]

    public class CategoryController : Controller

    {

        private readonly DongHoContext _context;

        public CategoryController(DongHoContext context)

        {



            _context = context;

        }

        public async Task<IActionResult> Index()

        {

            var categories = await _context.DanhMucs.ToListAsync();

            return View(categories);

        }

        // Thêm danh mục

        [HttpPost]

        public async Task<IActionResult> Create(DanhMuc model)

        {

            // Log model để kiểm tra dữ liệu gửi từ client

            Console.WriteLine($"CategoryName: {model.TenDanhMuc}, ParentId: {model.MaDanhMucCha}, Slug: {model.Slug}");



            if (ModelState.IsValid)

            {

                model.Slug = await SlugHelper.GenerateUniqueSlug(_context, model.TenDanhMuc!, SlugHelper.EntityType.Category, model.MaDanhMuc);

                _context.DanhMucs.Add(model);
                try

                {

                    await _context.SaveChangesAsync(); // Dùng async để không bị block thread

                    return Json(new { success = true });

                }

                catch (DbUpdateException ex)

                {

                    // Log lỗi và trả về phản hồi lỗi

                    return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });

                }

                catch (Exception ex)

                {

                    // Lỗi chung

                    return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });

                }

            }

            else

            {

                // Log thông báo nếu ModelState không hợp lệ

                return Json(new { success = false, message = "Dữ liệu không hợp lệ!" });

            }

        }

        // Cập nhật danh mục

        [HttpPost]

        public async Task<IActionResult> Edit(DanhMuc model)

        {

            if (ModelState.IsValid)

            {

                var category = await _context.DanhMucs.FindAsync(model.MaDanhMuc);

                if (category != null)

                {

                    category.TenDanhMuc = model.TenDanhMuc;

                    category.Slug = await SlugHelper.GenerateUniqueSlug(_context, category.TenDanhMuc!, SlugHelper.EntityType.Category, model.MaDanhMuc);

                    category.MaDanhMucCha = model.MaDanhMucCha;



                    _context.Update(category);

                    await _context.SaveChangesAsync();



                    return Json(new { success = true, message = "Cập nhật danh mục thành công!" });

                }

                return Json(new { success = false, message = "Không tìm thấy danh mục!" });

            }

            return Json(new { success = false, message = "Dữ liệu không hợp lệ!" });

        }



        [HttpPost]

        public IActionResult Delete(int id)

        {

            var category = _context.DanhMucs.Find(id);

            if (category != null)

            {

                _context.DanhMucs.Remove(category);

                _context.SaveChanges();



                return Json(new { success = true, message = "Xóa danh mục thành công!" });

            }

            return Json(new { success = false, message = "Không tìm thấy danh mục!" });

        }
        [HttpGet]
        public IActionResult Search(string searchQuery)
        {
            var categories = _context.DanhMucs
                                    .Where(c => c.TenDanhMuc!.ToLower().Contains(searchQuery) || c.Slug!.Contains(searchQuery))
                                    .ToList();

            return Json(new { success = true, data = categories });
        }





    }

}