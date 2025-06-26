using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Areas.Admin.Models;
using WebShop.Models;
using Attribute = WebShop.Models.Attribute;

namespace WebShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminAttributesController : Controller
    {
        private readonly DbMarketsContext _context;
        public INotyfService _notyfService { get; }
        public AdminAttributesController(DbMarketsContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        public IActionResult Index(int? page)
        {
            
            var lsAttributes = _context.Attributes
                .AsNoTracking();          
            return View(lsAttributes);
        }
        public IActionResult Create()
        {
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CatId", "CatName");
            return View();
        }

        // POST: Admin/AdminAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AttributeId, Name,NameEn,Ordering,KichHoat")] Attribute attribute)
        {
            if (ModelState.IsValid)
            {
               var check = _context.Attributes.FirstOrDefault(a => a.Name.ToLower() == attribute.Name.ToLower());
                var check2 = _context.Attributes.FirstOrDefault(a => a.NameEn.ToLower() == attribute.NameEn.ToLower());

                if (check == null&& check2==null)
                {
                    _context.Add(attribute);
                    await _context.SaveChangesAsync();

                    // Lấy danh sách hãng
                    List<string> cats = Request.Form["categoryAttributes"].ToList();
                    foreach (var c in cats)
                    {
                        var newCategoryAttribute = new CategoryAttribute
                        {
                            CatId = Convert.ToInt32(c),
                            AttributeId = attribute.AttributeId,
                            Name = attribute.Name,
                            KichHoat = attribute.KichHoat,
                        };
                        _context.CategoryAttributes.Add(newCategoryAttribute);
                        await _context.SaveChangesAsync();
                    }
                    _notyfService.Success("Tạo mới thuộc tính thành công");

                }
                else
                {
                    _notyfService.Success("Tạo mới thuộc tính không thành công do đã tồn tại dữ liệu này!");
                    return View();
                }


                return RedirectToAction(nameof(Index));
            }
            return View(attribute);
        }
        // GET: Admin/AdminAttributes/Edit/5
        public IActionResult Edit(int? id)
        {
            var attribute = _context.Attributes
                .Include(b => b.CategoryAttributes)
                .ThenInclude(cb => cb.Cat)
                .FirstOrDefault(b => b.AttributeId == id);

            if (attribute == null)
            {
                // Xử lý trường hợp không tìm thấy thuộc tính
                return NotFound();
            }
            // Lấy danh sách các danh mục đã được chọn cho thuộc tính
            var selectedCategories = attribute.CategoryAttributes.Select(cb => cb.Cat).ToList();

            // Lấy danh sách các danh mục từ cơ sở dữ liệu
            var allCategories = _context.Categories.ToList();

            ViewBag.AllCategories = allCategories;
            ViewBag.SelectedCategories = selectedCategories;

            return View(attribute);
        }
        // POST: Admin/AdminAttributes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AttributeId, Name,NameEn,Ordering,KichHoat")] Attribute attribute)
        {
            if (id != attribute.AttributeId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    //Xóa các bảng ghi categoryAttribute cũ
                    var categoryAttribute = _context.CategoryAttributes
                           .AsNoTracking()
                           .Where(x => x.AttributeId == id).ToList();

                    _context.CategoryAttributes.RemoveRange(categoryAttribute);
                     List<string> cats = Request.Form["categoryAttributes"].ToList();
                    foreach (var c in cats)
                    {
                        var newCategoryAttribute = new CategoryAttribute
                        {
                            CatId = Convert.ToInt32(c),
                            AttributeId = attribute.AttributeId,
                            Name = attribute.Name,
                            KichHoat = attribute.KichHoat,
                        };
                        _context.CategoryAttributes.Add(newCategoryAttribute);
                        await _context.SaveChangesAsync();
                    }
                    _context.Update(attribute);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttributeExists(attribute.AttributeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(attribute);
        }
        // GET: Admin/AdminBrands/Details/5
        public IActionResult Details(int? id)
        {
            // Truy vấn thuộc tính từ cơ sở dữ liệu
            var attribute = _context.Attributes.FirstOrDefault(a => a.AttributeId == id);

            if (attribute == null)
            {
                return NotFound(); // Xử lý khi thuộc tính không tồn tại
            }

            // Lấy danh sách giá trị thuộc tính
            var attributeValues = _context.AttributesPrices
                                .Where(av => av.AttributeId == id && av.ProductId == null)
                                .OrderByDescending(x=> x.AttributesPriceId)
                                .ToList();
            // Truyền thông tin thuộc tính và giá trị vào view
            var viewModel = new AttributeValueVM
            {
                Attribute = attribute,
                AttributePrices = attributeValues
            };
            return View(viewModel);
        }
        public IActionResult AddValue(int? attributeId, string newValue)
        {
            try 
            {
                // Truy vấn thuộc tính từ cơ sở dữ liệu
                var attribute = _context.Attributes.FirstOrDefault(a => a.AttributeId == attributeId);

                if (attribute == null)
                {
                    return NotFound(); // Xử lý khi thuộc tính không tồn tại
                }

                // Tạo một giá trị mới
                var attributeValue = new AttributesPrice
                {
                    AttributeId = attributeId,
                    Price = newValue,
                    KichHoat = true
                };

                // Thêm giá trị mới vào cơ sở dữ liệu
                _context.AttributesPrices.Add(attributeValue);
                _context.SaveChanges();

                return Json(new { success = true });
            } 
            catch 
            {
                return Json(new { success = false });
            }
            
        }
        public async Task<IActionResult> DeleteAttributePrice(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attPrice = await _context.AttributesPrices
                .FirstOrDefaultAsync(m => m.AttributesPriceId == id);
            if (attPrice == null)
            {
                return NotFound();
            }
            _context.AttributesPrices.Remove(attPrice);

            await _context.SaveChangesAsync();
            _notyfService.Success("Xóa thành công");
            return RedirectToAction(nameof(Details), new { id = attPrice.AttributeId });
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attribute = await _context.Attributes
                .FirstOrDefaultAsync(m => m.AttributeId == id);
            if (attribute == null)
            {
                return NotFound();
            }
            return View(attribute);
        }

        // POST: Admin/AdminAttributes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var attribute = await _context.Attributes.FindAsync(id);
            if (attribute == null)
            {
                // Xử lý khi không tìm thấy thuộc tính
                return NotFound();
            }
            // Lấy danh sách các giá trị của thuộc tính
            var attributeprice = await _context.AttributesPrices.Where(x => x.AttributeId == attribute.AttributeId).ToListAsync();

            // Xóa giá trị thuộc tính
            _context.AttributesPrices.RemoveRange(attributeprice);

            // Lấy danh sách các bảng ghi categoryattribute
            var categoryattribute = await _context.CategoryAttributes.Where(x => x.AttributeId == attribute.AttributeId).ToListAsync();

            // Xóa các bảng ghi categoryattribute
            _context.CategoryAttributes.RemoveRange(categoryattribute);

            _context.Attributes.Remove(attribute);

            await _context.SaveChangesAsync();
            _notyfService.Success("Xóa thành công");
            return RedirectToAction(nameof(Index));
        }
        private bool AttributeExists(int id)
        {
            return _context.Attributes.Any(e => e.AttributeId == id);
        }
    }
}
