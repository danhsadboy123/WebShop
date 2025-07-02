using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using WebShop.Helpper;
using WebShop.Models;

namespace WebShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminSlidesController : Controller
    {
        private readonly DbMarketsContext _context;

        public INotyfService _notyfService { get; }
        public AdminSlidesController(DbMarketsContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminSlides
        public async Task<IActionResult> Index()
        {
              return View(await _context.Slides.Where(c=>c.CatId==null).ToListAsync());
        }

        // GET: Admin/AdminSlides/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Slides == null)
            {
                return NotFound();
            }

            var slide = await _context.Slides
                .Include(p => p.Cat)
                .FirstOrDefaultAsync(m => m.SlideId == id);
            if (slide == null)
            {
                return NotFound();
            }

            return View(slide);
        }

        // GET: Admin/AdminSlides/Create
        public IActionResult Create()
        {
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CatId", "CatName");
            return View();
        }

        // POST: Admin/AdminSlides/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SlideId,Thumb,Alias,SlideName,CatId,IsActivated,HomeFlag,Right,Bottom")] Slide slide, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (ModelState.IsValid)
            {
                slide.SlideName = Utilities.ToTitleCase(slide.SlideName);
                if (fThumb != null)
                {
                    string extension = Path.GetExtension(fThumb.FileName);
                    string image = Utilities.SEOUrl(slide.SlideName) + extension;
                    slide.Thumb = await Utilities.UploadFile(fThumb, @"slides", image.ToLower());
                }
                if (string.IsNullOrEmpty(slide.Thumb)) slide.Thumb = "default.jpg";


                _context.Add(slide);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm mới thành công");
                return RedirectToAction(nameof(Index));
            }
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CatId", "CatName", slide.CatId);
            return View(slide);
        }

        // GET: Admin/AdminSlides/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Slides == null)
            {
                return NotFound();
            }

            var slide = await _context.Slides.FindAsync(id);
            if (slide == null)
            {
                return NotFound();
            }
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CatId", "CatName", slide.CatId);
            return View(slide);
        }

        // POST: Admin/AdminSlides/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SlideId,Thumb,Alias,SlideName,CatId,IsActivated,HomeFlag,Right,Bottom")] Slide slide, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (id != slide.SlideId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (fThumb != null)
                    {
                        string extension = Path.GetExtension(fThumb.FileName);
                        string image = Utilities.SEOUrl(slide.SlideName) + extension;
                        slide.Thumb = await Utilities.UploadFile(fThumb, @"slides", image.ToLower());
                    }
                    if (string.IsNullOrEmpty(slide.Thumb)) slide.Thumb = "default.jpg";


                    _context.Update(slide);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Chỉnh sửa thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SlideExists(slide.SlideId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Edit), new { id = slide.SlideId });
            }
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CatId", "CatName", slide.CatId);
            return View(slide);
        }

        // GET: Admin/AdminSlides/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slide = await _context.Slides
                .Include(p => p.Cat)
                .FirstOrDefaultAsync(m => m.SlideId == id);
            if (slide == null)
            {
                return NotFound();
            }

            return View(slide);
        }

        // POST: Admin/AdminSlides/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var slide = await _context.Slides.FindAsync(id);
            if (slide != null)
            {
                string image = Path.Combine(Directory.GetCurrentDirectory(), "images", "slides", slide.Thumb);
                Utilities.RemoveFile(image);
                _context.Slides.Remove(slide);
            }
            
            await _context.SaveChangesAsync();
            _notyfService.Success("Xóa thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool SlideExists(int id)
        {
          return _context.Slides.Any(e => e.SlideId == id);
        }
    }
}
