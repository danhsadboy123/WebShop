using System;
using System.Collections.Generic;
using System.Drawing.Printing;
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
    public class AdminLeverCustommerPttsController : Controller
    {
        private readonly DbMarketsContext _context;
        public INotyfService _notyfService { get; }
        public AdminLeverCustommerPttsController(DbMarketsContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminLeverCustommerPtts
        public async Task<IActionResult> Index()
        {
              return View(await _context.LeverCustommerPtts.ToListAsync());
        }
        [HttpPost]
        public IActionResult LeverCus(int id)
        {
            var value = _context.LeverCustommerPtts.Where(c=>c.Id== id).FirstOrDefault();
            return Json(new { success = "Ok", value = value });
        }
        [HttpPost]
        public IActionResult SaveNameLever(int id,string name)
        {
            var success = "";
            var levername = _context.LeverCustommerPtts.Where(c => c.Id == id).FirstOrDefault();
            if (levername == null)
            {
                success = "No";
            }
            else
            {
                levername.NameLever = name;
                _context.Update(levername);
                _context.SaveChanges();
                success = "Ok";
            }
            return Json(new {success=success});
        }
        [HttpGet]
        public IActionResult listLeverCus()
        {
            var value = _context.LeverCustommerPtts.ToList();
            return Json(new { success = "Ok", value = value });
        }
        public IActionResult createLever(string Namelerver)
        {
            var success = "No";
            var value = _context.LeverCustommerPtts.Where(x=>x.NameLever==Namelerver).ToList();
            if (value.Count() == 0)
            {
                var lever = new LeverCustommerPtt();
                lever.NameLever = Namelerver;
                success = "Ok";
                _context.Add(lever);
                _context.SaveChanges();
            }
            return Json(new { success = success});
        }
        // delete 
        [HttpPost]
        public IActionResult deleteitem(int id)
        {
            var success = "";
            var levercus = _context.LeverCustommerPtts.Where(x => x.Id == id).FirstOrDefault();
            var cus =_context.CustomerPotentails.Where(x=>x.LeverId== id).ToList();
            if(levercus == null)
            {
                success = "No";
            }
            else
            {
                if (cus.Count() != 0)
                {
                    success = "No1";
                }
                else
                {
                    success = "Ok";
                    _context.LeverCustommerPtts.Remove(levercus);
                    _context.SaveChanges();
                }
            }
            return Json(new {success = success});
            
        }

        // GET: Admin/AdminLeverCustommerPtts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LeverCustommerPtts == null)
            {
                return NotFound();
            }

            var leverCustommerPtt = await _context.LeverCustommerPtts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leverCustommerPtt == null)
            {
                return NotFound();
            }

            return View(leverCustommerPtt);
        }

        // GET: Admin/AdminLeverCustommerPtts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminLeverCustommerPtts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NameLever")] LeverCustommerPtt leverCustommerPtt)
        {
            if (ModelState.IsValid)
            {
                _context.Add(leverCustommerPtt);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(leverCustommerPtt);
        }

        // GET: Admin/AdminLeverCustommerPtts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LeverCustommerPtts == null)
            {
                return NotFound();
            }

            var leverCustommerPtt = await _context.LeverCustommerPtts.FindAsync(id);
            if (leverCustommerPtt == null)
            {
                return NotFound();
            }
            return View(leverCustommerPtt);
        }

        // POST: Admin/AdminLeverCustommerPtts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameLever")] LeverCustommerPtt leverCustommerPtt)
        {
            if (id != leverCustommerPtt.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(leverCustommerPtt);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeverCustommerPttExists(leverCustommerPtt.Id))
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
            return View(leverCustommerPtt);
        }

        // GET: Admin/AdminLeverCustommerPtts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LeverCustommerPtts == null)
            {
                return NotFound();
            }

            var leverCustommerPtt = await _context.LeverCustommerPtts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leverCustommerPtt == null)
            {
                return NotFound();
            }

            return View(leverCustommerPtt);
        }

        // POST: Admin/AdminLeverCustommerPtts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LeverCustommerPtts == null)
            {
                return Problem("Entity set 'DbMarketsContext.LeverCustommerPtts'  is null.");
            }
            var leverCustommerPtt = await _context.LeverCustommerPtts.FindAsync(id);
            if (leverCustommerPtt != null)
            {
                _context.LeverCustommerPtts.Remove(leverCustommerPtt);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeverCustommerPttExists(int id)
        {
          return _context.LeverCustommerPtts.Any(e => e.Id == id);
        }
    }
}
