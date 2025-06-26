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
    public class AdminCustomerBrandsController : Controller
    {
        private readonly DbMarketsContext _context;
        public INotyfService _notyfService { get; }
        public AdminCustomerBrandsController(DbMarketsContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        
        // GET: Admin/AdminCustomerBrands
        public async Task<IActionResult> Index()
        {
              return View(await _context.CustomerBrands.ToListAsync());
        }

        // GET: Admin/AdminCustomerBrands/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null || _context.CustomerBrands == null)
        //    {
        //        return NotFound();
        //    }

        //    var customerBrand = await _context.CustomerBrands
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (customerBrand == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(customerBrand);
        //}

        // GET: Admin/AdminCustomerBrands/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminCustomerBrands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Image,Address,MoTa,YearOn,YearOff,Link")] CustomerBrand customerBrand)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customerBrand);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customerBrand);
        }

        // GET: Admin/AdminCustomerBrands/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CustomerBrands == null)
            {
                return NotFound();
            }

            var customerBrand = await _context.CustomerBrands.FindAsync(id);
            if (customerBrand == null)
            {
                return NotFound();
            }
            return View(customerBrand);
        }

        // POST: Admin/AdminCustomerBrands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Image,Address,MoTa,YearOn,YearOff,Link")] CustomerBrand customerBrand)
        {
            if (id != customerBrand.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customerBrand);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerBrandExists(customerBrand.Id))
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
            return View(customerBrand);
        }

        // GET: Admin/AdminCustomerBrands/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CustomerBrands == null)
            {
                return NotFound();
            }

            var customerBrand = await _context.CustomerBrands
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customerBrand == null)
            {
                return NotFound();
            }

            return View(customerBrand);
        }

        // POST: Admin/AdminCustomerBrands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CustomerBrands == null)
            {
                return Problem("Entity set 'DbMarketsContext.CustomerBrands'  is null.");
            }
            var customerBrand = await _context.CustomerBrands.FindAsync(id);
            if (customerBrand != null)
            {
                _context.CustomerBrands.Remove(customerBrand);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerBrandExists(int id)
        {
          return _context.CustomerBrands.Any(e => e.Id == id);
        }
    }
}
