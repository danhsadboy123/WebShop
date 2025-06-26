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
    public class AdminAttributesPotentailsController : Controller
    {
        private readonly DbMarketsContext _context;
        public INotyfService _notyfService { get; }
        public AdminAttributesPotentailsController(DbMarketsContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminAttributesPotentails
        public async Task<IActionResult> Index()
        {
            var dbMarketsContext = _context.AttributesPotentails.Include(a => a.Potentail);
            return View(await dbMarketsContext.ToListAsync());
        }

        // GET: Admin/AdminAttributesPotentails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AttributesPotentails == null)
            {
                return NotFound();
            }

            var attributesPotentail = await _context.AttributesPotentails
                .Include(a => a.Potentail)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attributesPotentail == null)
            {
                return NotFound();
            }

            return View(attributesPotentail);
        }

        // GET: Admin/AdminAttributesPotentails/Create
        public IActionResult Create()
        {
            ViewData["PotentailId"] = new SelectList(_context.CustomerPotentails, "Id", "Id");
            return View();
        }

        // POST: Admin/AdminAttributesPotentails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PotentailId,Body,TimeSend")] ThuocTinhTiemNang attributesPotentail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(attributesPotentail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PotentailId"] = new SelectList(_context.CustomerPotentails, "Id", "Id", attributesPotentail.PotentailId);
            return View(attributesPotentail);
        }
       
        // update checked
        public IActionResult checksaveInput(int OptionEmailID,int leveruser)
        {
            var text = "";
           
            var cus = _context.CustomerPotentails.Where(c=>c.Checked==1).ToList();
            var custummer = from c in cus select(c);
            if (leveruser != 0)
            {
                custummer = custummer.Where(x => x.LeverId == leveruser).ToList();
            }
            if (OptionEmailID == null)
            {
                return Json(new { success = "No" });
            }
            else {
                var option = _context.EmailMakettings.Where(e=>e.EmailId==OptionEmailID).FirstOrDefault();
     
                if (custummer.Count() > 0)
                {
                    foreach(var item in custummer)
                    {
                        var attributes = new ThuocTinhTiemNang();
                        text = option.Body;
                        attributes.PotentailId = item.Id;
                        attributes.Body = text;
                        attributes.TimeSend=DateTime.Now;
                        _context.AddRange(attributes);
                    }
                }
                       _context.SaveChanges();
            }
            
            return Json(new {success="Ok"});
        }

        [HttpPost]
        public IActionResult deleteItem(int id)
        {
            if (_context.AttributesPotentails == null)
            {
                return Json(new { sussec = "ok" });
            }
            var emailAttribute = _context.AttributesPotentails.Find(id);
            if (emailAttribute != null)
            {
                _context.AttributesPotentails.Remove(emailAttribute);
            }

            _context.SaveChanges();
            return Json(new { success = "Ok", value = "true" });
        }
        [HttpPost]
        public IActionResult deleteEmail()
        {
            var cusId = _context.CustomerPotentails.Where(i=>i.Checked==1).ToList();

            foreach(var item in cusId)
            {
                var itemdelete = _context.AttributesPotentails.Where(x=>x.PotentailId==item.Id);
                _context.AttributesPotentails.RemoveRange(itemdelete);
            }
            _context.SaveChanges();
            return Json(new { success = "Ok", value = "true" });
        }


        // GET: Admin/AdminAttributesPotentails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AttributesPotentails == null)
            {
                return NotFound();
            }

            var attributesPotentail = await _context.AttributesPotentails.FindAsync(id);
            if (attributesPotentail == null)
            {
                return NotFound();
            }
            ViewData["PotentailId"] = new SelectList(_context.CustomerPotentails, "Id", "Id", attributesPotentail.PotentailId);
            return View(attributesPotentail);
        }

        // POST: Admin/AdminAttributesPotentails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PotentailId,Body,TimeSend")] ThuocTinhTiemNang attributesPotentail)
        {
            if (id != attributesPotentail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attributesPotentail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttributesPotentailExists(attributesPotentail.Id))
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
            ViewData["PotentailId"] = new SelectList(_context.CustomerPotentails, "Id", "Id", attributesPotentail.PotentailId);
            return View(attributesPotentail);
        }

        // GET: Admin/AdminAttributesPotentails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AttributesPotentails == null)
            {
                return NotFound();
            }

            var attributesPotentail = await _context.AttributesPotentails
                .Include(a => a.Potentail)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attributesPotentail == null)
            {
                return NotFound();
            }

            return View(attributesPotentail);
        }

        // POST: Admin/AdminAttributesPotentails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AttributesPotentails == null)
            {
                return Problem("Entity set 'DbMarketsContext.AttributesPotentails'  is null.");
            }
            var attributesPotentail = await _context.AttributesPotentails.FindAsync(id);
            if (attributesPotentail != null)
            {
                _context.AttributesPotentails.Remove(attributesPotentail);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttributesPotentailExists(int id)
        {
          return _context.AttributesPotentails.Any(e => e.Id == id);
        }
    }
}
