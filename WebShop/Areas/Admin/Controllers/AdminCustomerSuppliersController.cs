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
    public class AdminCustomerSuppliersController : Controller
    {
        private readonly DbMarketsContext _context;
        public INotyfService _notyfService { get; }
        public AdminCustomerSuppliersController(DbMarketsContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;

        }

        // GET: Admin/AdminCustomerSuppliers
        public async Task<IActionResult> Index()
        {
              return View(await _context.CustomerSuppliers.ToListAsync());
        }

        // GET: Admin/AdminCustomerSuppliers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CustomerSuppliers == null)
            {
                return NotFound();
            }

            var customerSupplier = await _context.CustomerSuppliers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customerSupplier == null)
            {
                return NotFound();
            }

            return View(customerSupplier);
        }

        // GET: Admin/AdminCustomerSuppliers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminCustomerSuppliers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Image,Name,Company,Phone,Email,Address,YearAdd,Lever")] CustomerSupplier customerSupplier)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customerSupplier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customerSupplier);
        }

        // GET: Admin/AdminCustomerSuppliers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CustomerSuppliers == null)
            {
                return NotFound();
            }

            var customerSupplier = await _context.CustomerSuppliers.FindAsync(id);
            if (customerSupplier == null)
            {
                return NotFound();
            }
            return View(customerSupplier);
        }

        // POST: Admin/AdminCustomerSuppliers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Image,Name,Company,Phone,Email,Address,YearAdd,Lever")] CustomerSupplier customerSupplier)
        {
            if (id != customerSupplier.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customerSupplier);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerSupplierExists(customerSupplier.Id))
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
            return View(customerSupplier);
        }

        // GET: Admin/AdminCustomerSuppliers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CustomerSuppliers == null)
            {
                return NotFound();
            }

            var customerSupplier = await _context.CustomerSuppliers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customerSupplier == null)
            {
                return NotFound();
            }

            return View(customerSupplier);
        }

        // POST: Admin/AdminCustomerSuppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CustomerSuppliers == null)
            {
                return Problem("Entity set 'DbMarketsContext.CustomerSuppliers'  is null.");
            }
            var customerSupplier = await _context.CustomerSuppliers.FindAsync(id);
            if (customerSupplier != null)
            {
                _context.CustomerSuppliers.Remove(customerSupplier);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerSupplierExists(int id)
        {
          return _context.CustomerSuppliers.Any(e => e.Id == id);
        }
    }
}
