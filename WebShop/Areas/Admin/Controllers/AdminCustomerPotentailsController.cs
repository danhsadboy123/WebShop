using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using WebShop.Models;

namespace WebShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminCustomerPotentailsController : Controller
    {
        private readonly DbMarketsContext _context;
        public INotyfService _notyfService { get; } 
        public AdminCustomerPotentailsController(DbMarketsContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminCustomerPotentails
        public async Task<IActionResult> Index()
        {
            return View(await _context.CustomerPotentails.ToListAsync());
        }

        // GET: Admin/AdminCustomerPotentails/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || _context.CustomerPotentails == null)
            {
                return NotFound();
            }

            var customerPotentail = _context.CustomerPotentails
                .FirstOrDefault(m => m.Id == id);
            if (customerPotentail == null)
            {
                return NotFound();
            }

            return Json(new { success = "Ok", value = customerPotentail });
        }
        // update lerver
        [HttpPost]
        public IActionResult updateValue(int id ,int lerver)
        {
            var success = "";
            var cusP = _context.CustomerPotentails.Where(m => m.Id == id).FirstOrDefault();
            if (cusP == null)
            {
                success = "No";
            }
            else
            {
                if(cusP.LeverId != lerver)
                {
                    success = "Ok";
                    cusP.LeverId = lerver;
                    _context.SaveChanges();

                }
                else
                {
                    success = "No1";
                }
            }
            return Json(new { success = success });
        }

        // check updete checked

        [HttpPost]

        public IActionResult updatecheck(int check)
        {
            if (check == 1)
            {
                (from c in _context.CustomerPotentails where c.Checked == 0 select c).ToList()
                    .ForEach(x => x.Checked = 1);
            }
            else
            {
                (from c in _context.CustomerPotentails where c.Checked == 1 select c).ToList()
                    .ForEach(x => x.Checked = 0);
            }
            _context.SaveChanges();
            return Json(new { success = "Ok" });
        }


        [HttpPost]

        public IActionResult CheckedSendmail(int Id)
        {
            //Id,Name,Email,Address,Phone,Checked
            var cusP = _context.CustomerPotentails.Where(i => i.Id == Id).First();


            if (cusP.Checked == 1)
            {
                cusP.Checked = 0;
            }
            else
            {
                cusP.Checked = 1;
            }
            _context.Update(cusP);
            _context.SaveChanges();

            var cusc = _context.CustomerPotentails.Where(c => c.Checked == 0).Count();
            return Json(new { succses = "ok", value = cusc });
        }

        [HttpPost]
        public IActionResult ImportCustommerP(int LeverID, IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                return Json(new { success = "No" });
            }
            using (var package = new ExcelPackage(excelFile.OpenReadStream()))
            {
                List<string> emailErr = new List<string>();
                string[] err = new string[1];
                var workSheet = package.Workbook.Worksheets[0];
                int rowCount = workSheet.Dimension.Rows;
                string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
                var regex = new Regex(pattern, RegexOptions.IgnoreCase);
                for (int row = 2; row <= rowCount; row++)
                {
                    var email = workSheet.Cells[row, 4].Value?.ToString();
                    if (regex.IsMatch(email))
                    {
                        CustomerPotentail customer = new CustomerPotentail();
                        customer.Name = workSheet.Cells[row, 2].Value?.ToString();
                        customer.CompannyName = workSheet.Cells[row, 3].Value?.ToString();
                        customer.Email= email;
                        customer.Address = workSheet.Cells[row, 5].Value?.ToString();
                        var phoneString = workSheet.Cells[row, 6].Value?.ToString();
                        if (!string.IsNullOrEmpty(phoneString) && int.TryParse(phoneString, out int phoneint))
                        {
                            customer.Phone = phoneint;
                        }
                        customer.LeverId = LeverID;
                        customer.Checked = 0;
                        try
                        {
                            _context.Add(customer);
                            _context.SaveChanges();
                        }
                        catch (Exception e)
                        {
                           
                        }
                    }
                    else {
                        emailErr.Add(email);
                    }
                }
                return RedirectToAction("CustumerPotentailSendEmail", "AdminEmailMakettings");
            }
        }

        [HttpGet]
        public IActionResult deleteEmail(int lever)
        {
            var cus = _context.CustomerPotentails.Where(x => x.Checked == 1).ToList();
            var customerPotentail = from c in cus select (c);
            if (lever != 0)
            {
                customerPotentail = customerPotentail.Where(x => x.LeverId == lever);
            }
            if (customerPotentail.Count() == 0)
            {
                return Json(new { success = "Not" });
            }
            else
            {
                foreach (var item in customerPotentail)
                {
                    var cusP = item;
                    _context.Remove(cusP);
                }
                _context.SaveChanges();
            }
            return Json(new { success = "Ok" });
        }

        [HttpPost]
        public IActionResult Create([Bind("Name,Email,CompannyName,Address,Phone,LeverId")] CustomerPotentail customerPotentail)
        {
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            if (regex.IsMatch(customerPotentail.Email))
            {
                var cusP = _context.CustomerPotentails.Where(x => x.Email == customerPotentail.Email).ToList();
                if (cusP.Count() != 0)
                {
                    return Json(new { success = "No" });
                }
                else
                {
                    customerPotentail.Checked = 0;
                    _context.Add(customerPotentail);
                    _context.SaveChanges();
                }

            }
            else
            {
                return Json(new { success = "No1" });
            }
            return Json(new { success = "Ok" });
        }
        // GET: Admin/AdminCustomerPotentails/Edit/5


        // POST: Admin/AdminCustomerPotentails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public IActionResult Edit([Bind("Id,Name,CompannyName,Address,Phone")] CustomerPotentail customerPotentail)
        {
            if (customerPotentail.Id == null)
            {
                return Json(new { success = "No1" });
            }
            _context.Update(customerPotentail);
            _context.SaveChanges();
            return Json(new { success = "Ok" });
        }

        // GET: Admin/AdminCustomerPotentails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CustomerPotentails == null)
            {
                return NotFound();
            }

            var customerPotentail = await _context.CustomerPotentails
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customerPotentail == null)
            {
                return NotFound();
            }

            return View(customerPotentail);
        }

        // POST: Admin/AdminCustomerPotentails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CustomerPotentails == null)
            {
                return Problem("Entity set 'DbMarketsContext.CustomerPotentails'  is null.");
            }
            var customerPotentail = await _context.CustomerPotentails.FindAsync(id);
            if (customerPotentail != null)
            {
                _context.CustomerPotentails.Remove(customerPotentail);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerPotentailExists(int id)
        {
            return _context.CustomerPotentails.Any(e => e.Id == id);
        }
    }
}
