using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using WebShop.Helpper;
using WebShop.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebShop.Areas.Admin.Controllers
{
    //[Authorize(VaiTros = "Admin")]
    [Area("Admin")]
    [Route("/Admin", Name = "AdminIndex")]
    //[Authorize]
    public class HomeController : Controller
    {
        // GET: /<controller>/
        private readonly DbMarketsContext _context;

        public INotyfService _notyfService { get; }

        public HomeController(DbMarketsContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        public IActionResult Index()
        {
            var taikhoanID = HttpContext.Session.GetString("MaTaiKhoan");
            if (taikhoanID == null) return RedirectToAction("AdminLogin", "Account", new { Area = "Admin" });

            //var latestOrders = _context.Orders
            //    .Include(o => o.Customer)
            //    .Include(o => o.Guest)
            //    .Include(o => o.TransactStatus)
            //    .OrderByDescending(o => o.OrderDate)
            //    .Take(10).ToList();

            return View();
        }

       
        [Route("configuation", Name = ("Cấu hình"))]
        public IActionResult ChangePageInfos()
        {
            var pageInfo = _context.PageInfos.FirstOrDefault();
            if (pageInfo == null)
            {
                return NotFound();
            }
            ViewBag.image = pageInfo.Image;
            ViewBag.OgImage = pageInfo.OgImage;
            ViewBag.header = pageInfo.HeaderCode;

            return View(pageInfo);
        }
        [Route("configuation", Name = ("Cấu hình"))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePageInfos(int id, [Bind("Id,Title,Domain,Profile,MetaDesc,MetaKey,Robots,GoogleSiteVerification,FacebookAppId,FacebookPage,GoogleTracking,Image,SoDienThoai,Email,Address,AddressMap,HeaderCode,FoodterCode")] PageInfo pageInfo, Microsoft.AspNetCore.Http.IFormFile Image, Microsoft.AspNetCore.Http.IFormFile OGImage)
        {


            var taikhoanID = HttpContext.Session.GetString("MaTaiKhoan");
            var Admin = _context.TaiKhoans.Where(x => x.MaTaiKhoan == Int32.Parse(taikhoanID)).First();
            if (id != pageInfo.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                if (Image != null)
                {
                    string extension = Path.GetExtension(Image.FileName);
                    string imageName = Utilities.SEOUrl("Logo") + ".png";
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Adminassets", "images", "logo");
                    pageInfo.Image = await Utilities.UploadFile(Image, imagePath, imageName.ToLower());

                }
                if (string.IsNullOrEmpty(pageInfo.Image)) pageInfo.Image = "default.png";
                if (OGImage != null)
                {
                    string extension = Path.GetExtension(OGImage.FileName);
                    string imageName = Utilities.SEOUrl("favicon-novazone") + ".png";
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Adminassets", "images", "logo");
                    pageInfo.OgImage = await Utilities.UploadFile(OGImage, imagePath, imageName.ToLower());

                }
                if (string.IsNullOrEmpty(pageInfo.OgImage)) pageInfo.OgImage = "default.png";
                try
                {
                    if (Admin.MaVaiTro == 1)
                    {
                        _context.Update(pageInfo);
                    }
                    else
                    {
                        var value = _context.PageInfos.FirstOrDefault();
                        value.Id = pageInfo.Id;
                        value.Title = pageInfo.Title;
                        value.Domain = pageInfo.Domain;
                        value.Profile = pageInfo.Profile;
                        value.MetaDesc = pageInfo.MetaDesc;
                        value.MetaKey = pageInfo.MetaKey;
                        value.Robots = pageInfo.Robots;
                        value.GoogleSiteVerification = pageInfo.GoogleSiteVerification;

                        value.FacebookAppId = pageInfo.FacebookAppId;
                        value.FacebookPage = pageInfo.FacebookPage;
                        value.GoogleTracking = pageInfo.GoogleTracking;
                        value.Image = pageInfo.Image;
                        value.SoDienThoai = pageInfo.SoDienThoai;
                        value.Email = pageInfo.Email;
                        value.Address = pageInfo.Address;
                        _context.Update(value);
                    }
                    //AddressMap,HeaderCode,FoodterCode
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Cập nhật thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(ChangePageInfos));
            }

            return View(pageInfo);
        }


        [HttpGet]
        [Route("Settingfull")]
        public IActionResult Settingfull()
        {
            var taikhoanID = HttpContext.Session.GetString("MaTaiKhoan");
            if (taikhoanID == null) return RedirectToAction("AdminLogin", "Account", new { Area = "Admin" });

            return View();
        }
        [Route("DraftOrder")]
        public IActionResult DraftOrder()
        {
            var order = _context.Orders
                .AsNoTracking()
                .Include(o => o.Customer)
                .Where(od => od.Draft == true)
                .ToList();
            return View(order);
        }
    }
}
