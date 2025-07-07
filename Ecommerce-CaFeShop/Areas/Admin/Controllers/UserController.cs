
using Ecommerce_CaFeShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_CaFeShop.Areas.Admin.Controllers
{
    [Authorize(Roles = "2")]
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly CaFeContext _context;

        public UserController(CaFeContext context)
        {
            _context = context;
        }

        // GET: Admin/User
        public async Task<IActionResult> Index()
        {
            var users = await _context.KhachHangs
                .Include(k => k.TaiKhoan)
                .ThenInclude(t => t.VaiTro)
                .OrderByDescending(k => k.MaKhachHang)
                .ToListAsync();

            return View(users);
        }

        // GET: Admin/User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs
                .Include(k => k.TaiKhoan)
                .ThenInclude(t => t.VaiTro)
                .FirstOrDefaultAsync(m => m.MaKhachHang == id);

            if (khachHang == null)
            {
                return NotFound();
            }

            return View(khachHang);
        }

        // GET: Admin/User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs
                .Include(k => k.TaiKhoan)
                .FirstOrDefaultAsync(k => k.MaKhachHang == id);

            if (khachHang == null)
            {
                return NotFound();
            }

            ViewBag.VaiTros = await _context.VaiTros.ToListAsync();
            return View(khachHang);
        }

        // POST: Admin/User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, KhachHang khachHang)
        {
            if (id != khachHang.MaKhachHang)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(khachHang);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cập nhật thông tin người dùng thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KhachHangExists(khachHang.MaKhachHang))
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

            ViewBag.VaiTros = await _context.VaiTros.ToListAsync();
            return View(khachHang);
        }

        // POST: Admin/User/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var khachHang = await _context.KhachHangs
                .Include(k => k.TaiKhoan)
                .FirstOrDefaultAsync(k => k.MaKhachHang == id);

            if (khachHang != null)
            {
                // Xóa tài khoản trước
                if (khachHang.TaiKhoan != null)
                {
                    _context.TaiKhoans.Remove(khachHang.TaiKhoan);
                }

                // Xóa khách hàng
                _context.KhachHangs.Remove(khachHang);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Xóa người dùng thành công!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool KhachHangExists(int id)
        {
            return _context.KhachHangs.Any(e => e.MaKhachHang == id);
        }
    }
}
