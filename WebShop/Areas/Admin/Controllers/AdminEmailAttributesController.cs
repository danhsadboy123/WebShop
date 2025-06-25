using System;
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
    public class AdminEmailAttributesController : Controller
    {
        private readonly DbMarketsContext _context;
        public INotyfService _notyfService { get; }
        public AdminEmailAttributesController(DbMarketsContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminEmailAttributes
        public async Task<IActionResult> Index()
        {
            var dbMarketsContext = _context.EmailAttributes.Include(e => e.Custumer);
            return View(await dbMarketsContext.ToListAsync());
        }

        // GET: Admin/AdminEmailAttributes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EmailAttributes == null)
            {
                return NotFound();
            }

            var emailAttribute = await _context.EmailAttributes
                .Include(e => e.Custumer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (emailAttribute == null)
            {
                return NotFound();
            }

            return View(emailAttribute);
        }

        // GET: Admin/AdminEmailAttributes/Create
        public IActionResult Create()
        {
            ViewData["CustumerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId");
            return View();
        }
        public IActionResult ListEmailCustomer(int? id)
        {
            var listCustomer = _context.EmailAttributes
                .Include(c => c.Custumer).Where(c => c.CustumerId == id).ToList();
            return PartialView("ListEmailCustomer", listCustomer);
        }
        // POST: Admin/AdminEmailAttributes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustumerId,Body,TimeSend")] EmailAttribute emailAttribute)
        {
            if (ModelState.IsValid)
            {
                _context.Add(emailAttribute);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustumerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", emailAttribute.CustumerId);
            return View(emailAttribute);
        }
        [HttpPost]
 
        public  IActionResult CreateApi([Bind("Id,CustumerId,Body")] EmailAttribute emailAttribute)
        {
           
            emailAttribute.TimeSend = DateTime.Now;
                _context.Add(emailAttribute);
                _context.SaveChanges();
            return Json(new {succses="ok"});
        }

        // GET: Admin/AdminEmailAttributes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EmailAttributes == null)
            {
                return NotFound();
            }

            var emailAttribute = await _context.EmailAttributes.FindAsync(id);
            if (emailAttribute == null)
            {
                return NotFound();
            }
            ViewData["CustumerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", emailAttribute.CustumerId);
            return View(emailAttribute);
        }

        // POST: Admin/AdminEmailAttributes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustumerId,Body,TimeSend")] EmailAttribute emailAttribute)
        {
            if (id != emailAttribute.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(emailAttribute);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmailAttributeExists(emailAttribute.Id))
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
            ViewData["CustumerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", emailAttribute.CustumerId);
            return View(emailAttribute);
        }

        // GET: Admin/AdminEmailAttributes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EmailAttributes == null)
            {
                return NotFound();
            }

            var emailAttribute = await _context.EmailAttributes
                .Include(e => e.Custumer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (emailAttribute == null)
            {
                return NotFound();
            }

            return View(emailAttribute);
        }

        // POST: Admin/AdminEmailAttributes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EmailAttributes == null)
            {
                return Problem("Entity set 'DbMarketsContext.EmailAttributes'  is null.");
            }
            var emailAttribute = await _context.EmailAttributes.FindAsync(id);
            if (emailAttribute != null)
            {
                _context.EmailAttributes.Remove(emailAttribute);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult deleteItem(int id)
        {
            if (_context.EmailAttributes == null)
            {
                return Json(new { sussec = "ok" });
            }
            var emailAttribute =  _context.EmailAttributes.Find(id);
            if (emailAttribute != null)
            {
                _context.EmailAttributes.Remove(emailAttribute);
            }

           _context.SaveChanges();
            return Json(new {success="Ok",value="true"});
        }

        private bool EmailAttributeExists(int id)
        {
            return _context.EmailAttributes.Any(e => e.Id == id);
        }
    }
}
