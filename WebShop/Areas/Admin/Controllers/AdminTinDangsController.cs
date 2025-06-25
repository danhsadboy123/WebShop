using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Protocol;
using PagedList.Core;
using WebShop.Helpper;
using WebShop.Models;


namespace WebShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminTinDangsController : Controller
    {

       
        private readonly DbMarketsContext _context;
        public INotyfService _notyfService { get; }
        public AdminTinDangsController(DbMarketsContext context, INotyfService notyfService)
        {
            //_context = NoCache.NoCache.loadContext("server");
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminTinDangs
        public IActionResult Index()
        {
            var collection = _context.Posts.AsNoTracking().ToList();
            foreach (var item in collection)
            {
                if(item.CreatedDate == null){
                    item.CreatedDate = DateTime.Now;
                    _context.Update(item);
                    _context.SaveChanges();
                }
            }          

            var lsTinDangs = _context.Posts.Include(x=>x.PostCat)
                .AsNoTracking()
                .OrderBy(x => x.PostId); 
            return View(lsTinDangs);
        }

        // GET: Admin/AdminTinDangs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tinDang = await _context.Posts
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (tinDang == null)
            {
                return NotFound();
            }

            return View(tinDang);
        }

        // GET: Admin/AdminTinDangs/Create
        public IActionResult Create()
        {
            ViewData["LoaiBaiViet"] = new SelectList(_context.PostCategories, "PostCatId", "PostCatName");
            return View();
        }

        // POST: Admin/AdminTinDangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId,Title,TitleEn,Scontents,ScontentsEn,Contents,ContentsEn,Thumb,Published,Alias,Alias_EN,CreatedDate,Author,AccountId,Tags,PostCatId,IsHot,IsNewfeed,TitleSeo,TitleSeoEn,MetaKey,MetaKeyEn,MetaDesc,MetaDescEn,Views")] Post post, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (ModelState.IsValid)
            {
                //Xu ly Thumb
                if (fThumb != null)
                {
                    string extension = Path.GetExtension(fThumb.FileName);
                    string imageName = Utilities.SEOUrl(post.Title) + extension;
                    post.Thumb = await Utilities.UploadFile(fThumb, @"newsImage", imageName.ToLower());
                }
                if (string.IsNullOrEmpty(post.Thumb)) post.Thumb = "default.jpg";
                post.Alias = Utilities.SEOUrl(post.Title);
                post.CreatedDate = DateTime.Now;


                _context.Add(post);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm mới thành công");
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        // GET: Admin/AdminTinDangs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["LoaiBaiViet"] = new SelectList(_context.PostCategories, "PostCatId", "PostCatName");
            if (id == null)
            {
                return NotFound();
            }

            var tinDang = await _context.Posts.FindAsync(id);
            if (tinDang == null)
            {
                return NotFound();
            }
            return View(tinDang);
        }

        // POST: Admin/AdminTinDangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostId,Title,TitleEn,Scontents,ScontentsEn,Contents,ContentsEn,Thumb,Published,Alias,Alias_EN,CreatedDate,Author,AccountId,Tags,PostCatId,IsHot,IsNewfeed,TitleSeo,TitleSeoEn,MetaKey,MetaKeyEn,MetaDesc,MetaDescEn,Views")] Post post, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (id != post.PostId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Xu ly Thumb
                    if (fThumb != null)
                    {
                        string extension = Path.GetExtension(fThumb.FileName);
                        string imageName = Utilities.SEOUrl(post.Title) + extension;
                        post.Thumb = await Utilities.UploadFile(fThumb, @"newsImage", imageName.ToLower());
                    }
                    if (string.IsNullOrEmpty(post.Thumb)) post.Thumb = "default.jpg";
                    post.Alias = Utilities.SEOUrl(post.Title);

                    _context.Update(post);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Chỉnh sửa thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TinDangExists(post.PostId))
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
            return View(post);
        }
        // POST: Admin/AdminTinDangs/Delete/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult DeletePost(int id)
        {
            try
            {
                var tinDang = _context.Posts.Find(id);
                if (tinDang != null)
                {
                    string image = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "newsImage", tinDang.Thumb);
                    Utilities.RemoveFile(image);
                }
                _context.Posts.Remove(tinDang);
                _context.SaveChanges();
                _notyfService.Success("Xóa thành công");
                return Json(new { success = true, message = "Xóa bài viết thành công!" });
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Lỗi khi xóa bài viết: " + ex.Message });
            }
        }

        private bool TinDangExists(int id)
        {
            return _context.Posts.Any(e => e.PostId == id);
        }
    }
}
