using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Azure.Core;
using Facebook;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebShop.Models;

namespace WebShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminFacebookPagesController : Controller
    {
        private readonly DbMarketsContext _context;
        public INotyfService _notyfService { get; }

        public AdminFacebookPagesController(DbMarketsContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
       
        // GET: Admin/AdminFacebookPages
        public ActionResult Index()
        {  
            // danh sah san pham
            var Productvalue = _context.Products
               .AsNoTracking()
               .Select(p => new Product
               {
                   ProductId = p.ProductId,
                   ProductName = p.ProductName,
                   ProductCode = p.ProductCode,
                   ShortDesc = p.ShortDesc,
                   SalePrice = p.SalePrice,
                   ProductCategories = p.ProductCategories,
                   HomeFlag = p.HomeFlag,
                   Alias = p.Alias,
                   Price = p.Price,
                   Avatar = p.Avatar,
                   Active = p.Active,
                   UnitsInStock = p.UnitsInStock,
                   DateCreated = p.DateCreated,
               })
               .ToList();
            string check = HttpContext.Session.GetString("Facebookpro");
            if (check == "yesFacebook")
            {
                var facebookpro = _context.ProductFacebooks.ToList();
                List<Product> pro = new List<Product>();
                foreach (var item in facebookpro)
                {
                    pro.AddRange(Productvalue.Where(p => p.ProductId == item.ProductId).ToList());
                }
                Productvalue = pro;
            }
            if (check == "noFacebook")
            {
                var facebookpro = _context.ProductFacebooks.ToList();
                List<Product> pro = new List<Product>();
                foreach (var item in facebookpro)
                {
                    pro.AddRange(Productvalue.Where(p => p.ProductId == item.ProductId).ToList());
                }
                foreach (var item in pro)
                {
                    Productvalue.RemoveAll(t => t.ProductId == item.ProductId);
                }
            }
            var facebook = _context.FacebookPages.FirstOrDefault();
            if(facebook==null)
            ViewBag.check = check;
            ViewBag.Product = Productvalue;
            ViewBag.facebook = facebook;
            return View();
        }
        public IActionResult loadpro(string check)
        {
            HttpContext.Session.SetString("Facebookpro",check);
            return Json(new {data="Ok"});
        }
        public IActionResult loaddata()
        {
            var facebook = _context.FacebookPages.FirstOrDefault();            
            return Json(facebook);
        }
        public IActionResult updatefacebook(string token, string name,string avatar,string namegroup,string avatargroup,string appid,string groupid)
        {
            var facebook = _context.FacebookPages.FirstOrDefault();
            facebook.TokenAccount = token;
            facebook.NameGroup = namegroup;
            facebook.Avatar = avatar;
            facebook.AvartarGroup = avatargroup;
            facebook.Name = name;
            facebook.AppId = appid;
            facebook.GroupId = groupid;
            _context.FacebookPages.Update(facebook);
            _context.SaveChanges();
            return Json(facebook);
        }
        
        public IActionResult postproduct()
        {
            var texx = "";
            var fbcon = _context.FacebookPages.FirstOrDefault();
            FacebookClient fb = new FacebookClient(fbcon.TokenAccount);
            dynamic parameters = new ExpandoObject();
            parameters.access_token = fbcon.TokenAccount;
            parameters.message = "asdasdasda sdasd";            
            dynamic result = fb.Post("me/photos", parameters);
            if (result != null && result.id != null)
            {
                texx = "Photos posted successfully!";                
            }
            else
            {
                texx = "Posting error!";             
            }
            return Json(texx);
        }
        // GET: Admin/AdminFacebookPages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.FacebookPages == null)
            {
                return NotFound();
            }

            var facebookPage = await _context.FacebookPages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (facebookPage == null)
            {
                return NotFound();
            }

            return View(facebookPage);
        }

        // GET: Admin/AdminFacebookPages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminFacebookPages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClientId,RedirectUri,Scope")] FacebookPage facebookPage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(facebookPage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(facebookPage);
        }

        // GET: Admin/AdminFacebookPages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.FacebookPages == null)
            {
                return NotFound();
            }

            var facebookPage = await _context.FacebookPages.FindAsync(id);
            if (facebookPage == null)
            {
                return NotFound();
            }
            return View(facebookPage);
        }

        // POST: Admin/AdminFacebookPages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClientId,RedirectUri,Scope")] FacebookPage facebookPage)
        {
            if (id != facebookPage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(facebookPage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FacebookPageExists(facebookPage.Id))
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
            return View(facebookPage);
        }

        // GET: Admin/AdminFacebookPages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.FacebookPages == null)
            {
                return NotFound();
            }

            var facebookPage = await _context.FacebookPages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (facebookPage == null)
            {
                return NotFound();
            }

            return View(facebookPage);
        }

        // POST: Admin/AdminFacebookPages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.FacebookPages == null)
            {
                return Problem("Entity set 'DbMarketsContext.FacebookPages'  is null.");
            }
            var facebookPage = await _context.FacebookPages.FindAsync(id);
            if (facebookPage != null)
            {
                _context.FacebookPages.Remove(facebookPage);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FacebookPageExists(int id)
        {
          return _context.FacebookPages.Any(e => e.Id == id);
        }
    }
}
