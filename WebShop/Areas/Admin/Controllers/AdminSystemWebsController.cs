using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebShop.Models;

namespace WebShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminSystemWebsController : Controller
    {
        private readonly DbMarketsContext _context;
        public INotyfService _notyfService { get; }
        public AdminSystemWebsController(DbMarketsContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminSystemWebs
        public async Task<IActionResult> Index()
        {
              ViewBag.count = _context.SystemWebs.Count();
              return View(await _context.SystemWebs.ToListAsync());
        }

        // GET: Admin/AdminSystemWebs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SystemWebs == null)
            {
                return NotFound();
            }

            var systemWeb = await _context.SystemWebs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (systemWeb == null)
            {
                return NotFound();
            }

            return View(systemWeb);
        }

        // GET: Admin/AdminSystemWebs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminSystemWebs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Server,EmailSend,Name,Post,EmailSmtp,PassSmtp")] SystemWeb systemWeb)
        {
            if (ModelState.IsValid)
            {
                _context.Add(systemWeb);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(systemWeb);
        }

        // GET: Admin/AdminSystemWebs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SystemWebs == null)
            {
                return NotFound();
            }

            var systemWeb = await _context.SystemWebs.FindAsync(id);
            if (systemWeb == null)
            {
                return NotFound();
            }
            return View(systemWeb);
        }

        // POST: Admin/AdminSystemWebs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Server,EmailSend,Name,Post,EmailSmtp,PassSmtp")] SystemWeb systemWeb)
        {
            if (id != systemWeb.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(systemWeb);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SystemWebExists(systemWeb.Id))
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
            return View(systemWeb);
        }

        // GET: Admin/AdminSystemWebs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SystemWebs == null)
            {
                return NotFound();
            }

            var systemWeb = await _context.SystemWebs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (systemWeb == null)
            {
                return NotFound();
            }

            return View(systemWeb);
        }

        // POST: Admin/AdminSystemWebs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SystemWebs == null)
            {
                return Problem("Entity set 'DbMarketsContext.SystemWebs'  is null.");
            }
            var systemWeb = await _context.SystemWebs.FindAsync(id);
            if (systemWeb != null)
            {
                _context.SystemWebs.Remove(systemWeb);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SystemWebExists(int id)
        {
          return _context.SystemWebs.Any(e => e.Id == id);
        }
    }
}
