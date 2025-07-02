//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using WebShop.Models;

//namespace WebShop.Areas.Admin.Controllers
//{
//    [Area("Admin")]
//    [Authorize]
//    public class AEmailMakettingsController : Controller
//    {
//        private readonly DbMarketsContext _context;

//        public AEmailMakettingsController(DbMarketsContext context)
//        {
//            _context = context;
//        }

//        // GET: Admin/AEmailMakettings
//        public async Task<IActionResult> Index()
//        {
//            var dbMarketsContext = _context.EmailMakettings.Include(e => e.Acount);
//            return View(await dbMarketsContext.ToListAsync());
//        }

//        // GET: Admin/AEmailMakettings/Details/5
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null || _context.EmailMakettings == null)
//            {
//                return NotFound();
//            }

//            var emailMaketting = await _context.EmailMakettings
//                .Include(e => e.Acount)
//                .FirstOrDefaultAsync(m => m.EmailId == id);
//            if (emailMaketting == null)
//            {
//                return NotFound();
//            }

//            return View(emailMaketting);
//        }

//        // GET: Admin/AEmailMakettings/Create
//        public IActionResult Create()
//        {
//            ViewData["AcountId"] = new SelectList(_context.TaiKhoans, "AccountId", "AccountId");
//            return View();
//        }

//        // POST: Admin/AEmailMakettings/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("EmailId,AcountId,Title,ContentName,Body,CreatedAt,CustomDate,EmailEvent,IsActivated,Input")] EmailMaketting emailMaketting)
//        {
//            if (ModelState.IsValid)
//            {
//                _context.Add(emailMaketting);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["AcountId"] = new SelectList(_context.TaiKhoans, "AccountId", "AccountId", emailMaketting.AcountId);
//            return View(emailMaketting);
//        }

//        // GET: Admin/AEmailMakettings/Edit/5
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null || _context.EmailMakettings == null)
//            {
//                return NotFound();
//            }

//            var emailMaketting = await _context.EmailMakettings.FindAsync(id);
//            if (emailMaketting == null)
//            {
//                return NotFound();
//            }
//            ViewData["AcountId"] = new SelectList(_context.TaiKhoans, "AccountId", "AccountId", emailMaketting.AcountId);
//            return View(emailMaketting);
//        }

//        // POST: Admin/AEmailMakettings/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("EmailId,AcountId,Title,ContentName,Body,CreatedAt,CustomDate,EmailEvent,IsActivated,Input")] EmailMaketting emailMaketting)
//        {
//            if (id != emailMaketting.EmailId)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(emailMaketting);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!EmailMakettingExists(emailMaketting.EmailId))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["AcountId"] = new SelectList(_context.TaiKhoans, "AccountId", "AccountId", emailMaketting.AcountId);
//            return View(emailMaketting);
//        }

//        // GET: Admin/AEmailMakettings/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null || _context.EmailMakettings == null)
//            {
//                return NotFound();
//            }

//            var emailMaketting = await _context.EmailMakettings
//                .Include(e => e.Acount)
//                .FirstOrDefaultAsync(m => m.EmailId == id);
//            if (emailMaketting == null)
//            {
//                return NotFound();
//            }

//            return View(emailMaketting);
//        }

//        // POST: Admin/AEmailMakettings/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            if (_context.EmailMakettings == null)
//            {
//                return Problem("Entity set 'DbMarketsContext.EmailMakettings'  is null.");
//            }
//            var emailMaketting = await _context.EmailMakettings.FindAsync(id);
//            if (emailMaketting != null)
//            {
//                _context.EmailMakettings.Remove(emailMaketting);
//            }
            
//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool EmailMakettingExists(int id)
//        {
//          return _context.EmailMakettings.Any(e => e.EmailId == id);
//        }
//    }
//}
