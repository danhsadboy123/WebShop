using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebShop.Models;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace WebShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminCustomerProjectsController : Controller
    {
        private readonly DbMarketsContext _context;
        public INotyfService _notyfService { get; }
        public AdminCustomerProjectsController(DbMarketsContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminCustomerProjects
        public async Task<IActionResult> Index()
        {
              return View(await _context.CustomerProjects.ToListAsync());
        }

        // GET: Admin/AdminCustomerProjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CustomerProjects == null)
            {
                return NotFound();
            }

            var customerProject = await _context.CustomerProjects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customerProject == null)
            {
                return NotFound();
            }

            return View(customerProject);
        }

        // GET: Admin/AdminCustomerProjects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminCustomerProjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Image,Address,MoTa,YearOn,YearOff,Link")] DuAnKhachHang customerProject)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customerProject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customerProject);
        }

        // GET: Admin/AdminCustomerProjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CustomerProjects == null)
            {
                return NotFound();
            }

            var customerProject = await _context.CustomerProjects.FindAsync(id);
            if (customerProject == null)
            {
                return NotFound();
            }
            return View(customerProject);
        }

        // POST: Admin/AdminCustomerProjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Image,Address,MoTa,YearOn,YearOff,Link")] DuAnKhachHang customerProject)
        {
            if (id != customerProject.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customerProject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerProjectExists(customerProject.Id))
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
            return View(customerProject);
        }

        // GET: Admin/AdminCustomerProjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CustomerProjects == null)
            {
                return NotFound();
            }

            var customerProject = await _context.CustomerProjects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customerProject == null)
            {
                return NotFound();
            }

            return View(customerProject);
        }

        // POST: Admin/AdminCustomerProjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CustomerProjects == null)
            {
                return Problem("Entity set 'DbMarketsContext.CustomerProjects'  is null.");
            }
            var customerProject = await _context.CustomerProjects.FindAsync(id);
            if (customerProject != null)
            {
                _context.CustomerProjects.Remove(customerProject);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerProjectExists(int id)
        {
          return _context.CustomerProjects.Any(e => e.Id == id);
        }
    }
}
