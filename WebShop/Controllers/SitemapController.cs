using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using WebShop.Models;

namespace WebShop.Controllers
{
    public class SitemapController : Controller
    {
        private readonly DbMarketsContext _context;
        public SitemapController(DbMarketsContext context)
        {
            _context = context;
        }
        [Route("sitemap.xml")]
        public IActionResult Sitemap()
        
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";

            // Danh sách URL danh mục với ngày cập nhật cuối cùng
            var categories = _context.Categories
                .Where(c => c.Published == true)
                .Select(c => new {
                    Url = $"{baseUrl}/{c.Alias}",
                    LastModified = DateTime.Now
                })
                .ToList();

            // Danh sách URL sản phẩm với ngày cập nhật cuối cùng
            var products = _context.Products
                            .Where(p => p.IsActivated)
                            .Select(p => new {
                                Url = $"{baseUrl}/{p.Alias}-{p.ProductId}",
                                LastModified = p.DateCreated ?? DateTime.Now // Đảm bảo LastModified là DateTime
                            })
                            .ToList();

            // Tạo cấu trúc XML cho sitemap
            XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            var sitemap = new XElement(ns + "urlset",
                categories.Select(c => new XElement(ns + "url",
                    new XElement(ns + "loc", c.Url),
                    new XElement(ns + "lastmod", c.LastModified.ToString("yyyy-MM-dd")),
                    new XElement(ns + "changefreq", "weekly"),
                    new XElement(ns + "priority", "0.8")
                )),
                products.Select(p => new XElement(ns + "url",
                    new XElement(ns + "loc", p.Url),
                    new XElement(ns + "lastmod", p.LastModified.ToString("yyyy-MM-dd")),
                    new XElement(ns + "changefreq", "weekly"),
                    new XElement(ns + "priority", "0.7")
                ))
            );

            // Trả về XML cho Google
            return Content(sitemap.ToString(), "application/xml", Encoding.UTF8);
        }
     
    }
}
