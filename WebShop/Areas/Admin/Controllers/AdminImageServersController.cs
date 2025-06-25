using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using elFinder.NetCore.Drivers.FileSystem;
using elFinder.NetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using WebShop.Helpper;
using WebShop.Models;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using static OfficeOpenXml.ExcelErrorValue;
using Microsoft.AspNetCore.Authorization;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace WebShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminImageServersController : Controller
    {
        private readonly DbMarketsContext _context;
        public INotyfService _notyfService { get; }
        public AdminImageServersController(DbMarketsContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
       
         
        // GET: Admin/AdminImageServers
        public async Task<IActionResult> Index()
        {
            var imageServer = _context.ImageServers.ToList();
            ViewBag.ImageServer = imageServer;
            return View();
        }
        

        // GET: Admin/AdminImageServers/Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Search(string name)
        {
            var values = "";
            var names = _context.ImageServers.Where(x => x.Name.Contains(name)).FirstOrDefault();
            if (names==null)
            {
                values = "OK";
            }
            else
            {
                values= "No";
            }
            return Json(new {success=true,data = values });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,NameLink")] ImageServer imageServer, Microsoft.AspNetCore.Http.IFormFile NameLink)
        {
            var names = _context.ImageServers.Where(x => x.Name.Contains(imageServer.Name)).FirstOrDefault();
            if (names == null)
            {
                imageServer.Name = imageServer.Name;
            }
            else
            {
                imageServer.Name = imageServer.Name + "1";
            }

            if (NameLink != null)
            {
                string extension = Path.GetExtension(NameLink.FileName);
                string imageName = Utilities.SEOUrl(imageServer.Name) + extension;
                imageServer.NameLink = await Utilities.UploadFile(NameLink, @"newsImageS", imageName.ToLower());
            }
            if (string.IsNullOrEmpty(imageServer.NameLink)) imageServer.NameLink = "default.jpg";
            _context.Add(imageServer);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        
        public IActionResult Delete(int? id)
        {
            if (id == null || _context.ImageServers == null)
            {
                return NotFound();
            }

            try
            {
                var imageServer = _context.ImageServers.Find(id);
                if (imageServer != null)
                {
                    try
                    {
                        string image = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "newsImageS", imageServer.NameLink);
                        Utilities.RemoveFile(image);
                    }
                    catch { 
                    }
                }
                _context.Remove(imageServer);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }



        }

       
    }
}
