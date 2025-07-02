using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using WebShop.Areas.Admin.Models;
using WebShop.Helpper;
using WebShop.Models;

namespace WebShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminCustomersController : Controller
    {
        private readonly DbMarketsContext _context;
        public INotyfService _notyfService { get; }
        public AdminCustomersController(DbMarketsContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;

        }

        // GET: Admin/AdminCustomers
        public IActionResult Index()
        {
            var lsCustomers = _context.Customers
                .Include(c => c.AccountAddresses)
                    .ThenInclude(a => a.Province) 
                .Include(c => c.AccountAddresses)
                    .ThenInclude(a => a.District) 
                .Include(c => c.AccountAddresses)
                    .ThenInclude(a => a.Ward)
                .ToList();
            return View(lsCustomers);
        }

        // GET: Admin/AdminCustomers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.AccountAddresses)
                    .ThenInclude(a => a.Province)
                .Include(c => c.AccountAddresses)
                    .ThenInclude(a => a.District)
                .Include(c => c.AccountAddresses)
                    .ThenInclude(a => a.Ward)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }
            var lsOrders = _context.Orders
                .AsNoTracking()
                .Where(x => x.CustomerId == customer.CustomerId)
                .Include(o => o.DeliveryStatus)
                .OrderBy(x => x.OrderDate)
                .ToList();
            ViewBag.lsOrders = lsOrders;
            return View(customer);
        }

        // GET: Admin/AdminCustomers/Create
        public IActionResult Create()
        {
            ViewData["lsProvinces"] = new SelectList(_context.Provinces.OrderBy(x => x.ProvinceId).ToList(), "ProvinceId", "ProvinceName");
            return View();

        }

        // POST: Admin/AdminCustomers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminCustomerCreateVM adminCustomerCreateVM)
        {
            if (ModelState.IsValid)
            {
                Customer customer = new Customer
                {
                    FullName = adminCustomerCreateVM.FullName,
                    Birthday = adminCustomerCreateVM.Birthday,
                    Email = adminCustomerCreateVM.Email,
                    Phone = adminCustomerCreateVM.Phone,
                    Active = true,
                    Gender = adminCustomerCreateVM.Gender,
                    Note = adminCustomerCreateVM.Note,
                    CompanyName = adminCustomerCreateVM.CompanyName,
                    CreateDate = DateTime.Now,
                };
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
                AccountAddress accountAddress = new AccountAddress
                {
                    CustomerId = customer.CustomerId,
                    UserName = customer.FullName,
                    ProvinceId = adminCustomerCreateVM.ProvinceId,
                    DistrictId = adminCustomerCreateVM.DistrictId,
                    WardId = adminCustomerCreateVM.WardId,
                    Content = adminCustomerCreateVM.Address,
                    IsDefault = true,
                };
                _context.AccountAddresses.Add(accountAddress);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["lsProvinces"] = new SelectList(_context.Provinces.OrderBy(x => x.ProvinceId).ToList(), "ProvinceId", "ProvinceName",adminCustomerCreateVM.ProvinceId);
            return View(adminCustomerCreateVM);
        }

        // GET: Admin/AdminCustomers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Admin/AdminCustomers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,FullName,Birthday,Avatar,Address,Email,Phone,LocationId,District,Ward,CreateDate,Password,Salt,LastLogin,Active")] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
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
            return View(customer);
        }

        // POST: Admin/AdminCustomers/Delete/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteCustomer(int? id)
        {
            try
            {
                var customer = _context.Customers.Find(id);
                if (customer == null)
                {
                    // Xử lý khi không tìm thấy sản phẩm
                    return Json(new { success = false, message = "Không tìm thấy khách hàng!" });
                }
                var customerBuy = _context.Orders.Where(od => od.CustomerId == customer.CustomerId).FirstOrDefault();
                if (customerBuy == null)
                {
                    // Lấy danh sách các địa chỉ của khách hàng
                    var customerAddresses = _context.AccountAddresses.Where(x => x.CustomerId == customer.CustomerId).ToList();

                    // Xóa các product category
                    _context.AccountAddresses.RemoveRange(customerAddresses);

                    _context.Customers.Remove(customer);

                    _context.SaveChanges();
                    _notyfService.Success("Xóa khách hàng thành công");
                    return Json(new { success = true, message = "Xóa khách hàng thành công!" });
                }
                else
                {
                    return Json(new { success = true});
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi khi xóa khách hàng: " + ex.Message });
            }
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}