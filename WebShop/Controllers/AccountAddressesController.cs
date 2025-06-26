using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebShop.Models;

namespace WebShop.Controllers
{
    public class AccountAddressesController : Controller
    {
        private readonly DbMarketsContext _context;

        public AccountAddressesController(DbMarketsContext context)
        {
            _context = context;
        }

        // GET: AccountAddresses
        public async Task<IActionResult> Index()
        {
            var dbMarketsContext = _context.AccountAddresses.Include(a => a.Customer).Include(a => a.District).Include(a => a.Guest).Include(a => a.Province).Include(a => a.Ward);
            return View(await dbMarketsContext.ToListAsync());
        }

        // GET: AccountAddresses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AccountAddresses == null)
            {
                return NotFound();
            }

            var accountAddress = await _context.AccountAddresses
                .Include(a => a.Customer)
                .Include(a => a.District)
                .Include(a => a.Guest)
                .Include(a => a.Province)
                .Include(a => a.Ward)
                .FirstOrDefaultAsync(m => m.AddressId == id);
            if (accountAddress == null)
            {
                return NotFound();
            }
            return View(accountAddress);
        }

        // GET: AccountAddresses/Create
        public IActionResult CreateAddress()
        {
            //kiem tả user dia chi 

            ViewData["lsProvinces"] = new SelectList(_context.Provinces.OrderBy(x => x.ProvinceId).ToList(), "ProvinceId", "ProvinceName");

            return PartialView("CreateAddress");
        }
        public IActionResult UpdateAccountAddress(DateTime Birthday1,string Gender1, string Phone1, string Email1,string FullName1,int id)
        {
            bool success = false;
            var customer = _context.Customers.Where(c => c.CustomerId == id).FirstOrDefault();
            try
            {
                if (customer != null)
                {
                    //customer.Birthday = Birthday1;
                    if (Gender1 == "Name")
                    {
                        customer.Gender = false;
                    }
                    else
                    {
                        customer.Gender = true;
                    }
                    customer.SoDienThoai = Phone1;    
                    customer.Email = Email1;
                    customer.HoTen = FullName1;
                    _context.Customers.Update(customer);
                    _context.SaveChanges();
                    success = true;
                }
                else
                {
                    success = false;
                }
            }
            catch(Exception ex) {
                success = false;
            }
            return Json(success);
        }
        // POST: AccountAddresses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAddress([Bind("AddressId,CustomerId,GuestId,SoDienThoai,UserName,ProvinceId,DistrictId,WardId,Content,IsDefault")] AccountAddress accountAddress)
        {
            try
            {
                var taikhoanID = HttpContext.Session.GetString("CustomerId");
                if (ModelState.IsValid)
                {
                    if (taikhoanID != null)
                    {
                        accountAddress.CustomerId = Convert.ToInt32(taikhoanID);
                    }
                    _context.Add(accountAddress);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Checkout");
                }
                else
                {
                    return PartialView("CreateAddress", accountAddress);

                }
            }
            catch
            {
                return PartialView("CreateAddress", accountAddress);

            }
        }

        // GET: AccountAddresses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AccountAddresses == null)
            {
                return NotFound();
            }

            var accountAddress = await _context.AccountAddresses.FindAsync(id);
            if (accountAddress == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", accountAddress.CustomerId);
            ViewData["DistrictId"] = new SelectList(_context.Districts, "DistrictId", "DistrictId", accountAddress.DistrictId);
            ViewData["GuestId"] = new SelectList(_context.Guests, "GuestId", "GuestId", accountAddress.GuestId);
            ViewData["ProvinceId"] = new SelectList(_context.Provinces, "ProvinceId", "ProvinceId", accountAddress.ProvinceId);
            ViewData["WardId"] = new SelectList(_context.Wards, "WardId", "WardId", accountAddress.WardId);
            return View(accountAddress);
        }

        // POST: AccountAddresses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AddressId,CustomerId,GuestId,SoDienThoai,UserName,ProvinceId,DistrictId,WardId,Content,IsDefault")] AccountAddress accountAddress)
        {
            if (id != accountAddress.AddressId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(accountAddress);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountAddressExists(accountAddress.AddressId))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", accountAddress.CustomerId);
            ViewData["DistrictId"] = new SelectList(_context.Districts, "DistrictId", "DistrictId", accountAddress.DistrictId);
            ViewData["GuestId"] = new SelectList(_context.Guests, "GuestId", "GuestId", accountAddress.GuestId);
            ViewData["ProvinceId"] = new SelectList(_context.Provinces, "ProvinceId", "ProvinceId", accountAddress.ProvinceId);
            ViewData["WardId"] = new SelectList(_context.Wards, "WardId", "WardId", accountAddress.WardId);
            return View(accountAddress);
        }

        // GET: AccountAddresses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AccountAddresses == null)
            {
                return NotFound();
            }
            var accountAddress = await _context.AccountAddresses
                .Include(a => a.Customer)
                .Include(a => a.District)
                .Include(a => a.Guest)
                .Include(a => a.Province)
                .Include(a => a.Ward)
                .FirstOrDefaultAsync(m => m.AddressId == id);
            if (accountAddress == null)
            {
                return NotFound();
            }
            return View(accountAddress);
        }

        // POST: AccountAddresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AccountAddresses == null)
            {
                return Problem("Entity set 'DbMarketsContext.AccountAddresses'  is null.");
            }
            var accountAddress = await _context.AccountAddresses.FindAsync(id);
            if (accountAddress != null)
            {
                _context.AccountAddresses.Remove(accountAddress);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> SetAddressDefault(int? AddressId)
        {
            if (AddressId == null || _context.AccountAddresses == null)
            {
                return NotFound();
            }

            var accountAddress = await _context.AccountAddresses               
                .FirstOrDefaultAsync(m => m.AddressId == AddressId);
            if (accountAddress != null)
            {
                // Đặt địa chỉ được chọn là mặc định
                accountAddress.IsDefault = true;

                // Hủy chọn mặc định cho các địa chỉ khác
                var otherAddresses = _context.AccountAddresses.Where(a => a.AddressId != AddressId);
                foreach (var address in otherAddresses)
                {
                    address.IsDefault = false;
                }

                // Lưu thay đổi vào cơ sở dữ liệu
                _context.SaveChanges();
            }
            return Json(new { success = true });
        }
        private bool AccountAddressExists(int id)
        {
          return _context.AccountAddresses.Any(e => e.AddressId == id);
        }

        public IActionResult GetProvince()
        {
            var Districts = _context.Provinces.ToList(); 
            return Json(Districts);
        }
        public IActionResult GetDistricts(int ProvinceId)
        {
            var Districts = _context.Districts.OrderBy(x => x.DistrictId)
                .Where(x => x.ProvinceId == ProvinceId)
                .ToList();
            return Json(Districts);
        }
        public IActionResult GetWards(int DistrictId)
        {
            var Wards = _context.Wards.OrderBy(x => x.WardId)
                .Where(x => x.DistrictId == DistrictId)
                .ToList();
            return Json(Wards);
        }
    }
}
