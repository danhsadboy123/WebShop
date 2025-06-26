using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using WebShop.Models;

namespace WebShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminDiscountsController : Controller
    {
        private readonly DbMarketsContext _context;

        public AdminDiscountsController(DbMarketsContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminDiscounts
        public IActionResult Index()
        {
            updatediscount();
            return View(_context.Discounts.ToList());
        }
        public IActionResult listdiscount(int id)
        {
            var discount = _context.Discounts.ToList();
            if (id == 1)
            {
                discount = discount.Where(p => p.Startus == true).ToList();
            }
            if (id == 2)
            {
                discount = discount.Where(p => p.Startus == false).ToList();
            }
            ViewBag.discount = discount;
            return Json(new {success="Ok",data=discount});
        }
        public void updatediscount()
        {
            var dis = _context.Discounts
                 .Where(i => i.ShowWeb == true)
                 .Where(i => i.Startus == true)
                 .Where(i => i.TimeOff < DateTime.Now)
                 .ToList();
            foreach (var item in dis)
            {
                item.ShowWeb = false;
                item.Startus = false;
                _context.Discounts.Update(item);
                _context.SaveChanges();
            }
        }
        //
        //public
        // GET: Admin/AdminDiscounts/Details/5


        // GET: Admin/AdminDiscounts/Create
        public IActionResult Create()
        {           
            var product = _context.Products.ToList();  
            var discount = _context.Discounts.Where(i=>i.TimeOff>DateTime.Now).Include(p=>p.DiscountAddProducts).ThenInclude(p=>p.Product).ToList();
            var produtgift = _context.ProductGifts.ToList();
            foreach(var item in discount)
            {
                foreach(var dis in item.DiscountAddProducts)
                {
                    product = product.Where(p => p.ProductId != dis.ProductId).ToList();
                }
            }
          
            ViewBag.product = product; 
            return View();
        }

        public IActionResult CreateGift()
        {
            var produtgift = _context.ProductGifts.ToList();            
            ViewBag.productGift = produtgift;
            return View();
        }
        [HttpGet]
        public IActionResult detailGift(int id)
        {
            var detail = _context.ProductGifts.Where(p=>p.Id==id).FirstOrDefault();
            return Json(new { success = "Ok",data=detail });
        }
        [HttpPost]
        public IActionResult deleteProGift(int id)
        {
            var detail = _context.ProductGifts
                .Include(p=>p.GitAttributes)                    
                .Where(p => p.Id == id).FirstOrDefault();
            if (detail.GitAttributes.Count >0)
            {
               return Json(new { success = "No" });
            }
            else
            {
                var pro = _context.ProductGifts.Include(p=>p.GitAttributes).FirstOrDefault();
                if (pro != null)
                {
                    _context.ProductGifts.Remove(pro);
                    _context.SaveChanges();
                }
            }
            var data = _context.ProductGifts.ToList();
            return Json(new { success = "Ok",data=data});
        }

        [HttpPost]
        public IActionResult CreateProductGiff(string Name,string Avatar,string Alias,int Prite)
        {
             
                ProductGift gift = new ProductGift();
                gift.Name = Name;
                gift.Avatar = Avatar;
                gift.Alias = Alias;
                gift.Price = Prite;

                try
                {
                    _context.ProductGifts.Add(gift);
                    _context.SaveChanges();

                }
                catch (Exception ex)
                {
                    return Json(new { success = "No" });
                }
            
                     
            return Json(new { success = "Ok" });
        }
        [HttpPost]
        public IActionResult updateProductGiff(int id, string Name, string Avatar, string Alias, int Prite)
        {
            if (id != null)
            {
                var giftPro = _context.ProductGifts
                    .Where(p => p.Id == id)
                    .FirstOrDefault();
                try
                {
                    giftPro.Name = Name;
                    giftPro.Avatar = Avatar;
                    giftPro.Alias = Alias;
                    giftPro.Price = Prite;
                    _context.ProductGifts.Update(giftPro);
                    _context.SaveChanges();

                }
                catch (Exception ex)
                {
                    return Json(new { success = "No" });
                }
            }           

            return Json(new { success = "Ok" });
        }

        [HttpPost]
        public IActionResult SearchStatusAll(int[] listid)
        {
            Product[] countitem = new Product[listid.Length];
            if (listid.Length != 0)
            {
                int i = 0;
                foreach (var item in listid)
                {
                    var product = _context.Products.Where(o => o.ProductId == item).FirstOrDefault();
                    if (product != null)
                    {
                        try
                        {
                            countitem[i] = product;
                            i++;
                        }
                        catch (Exception e) { }

                    }
                }

            }
            return Json(new { success = "Ok", data = countitem });
        }

        [HttpPost]
        public IActionResult SearchGiftAll(int[] listid)
        {
            ProductGift[] countitem = new ProductGift[listid.Length];
            if (listid.Length != 0)
            {
                int i = 0;
                foreach (var item in listid)
                {
                    var product = _context.ProductGifts.Where(o => o.Id == item).FirstOrDefault();
                    if (product != null)
                    {
                        try
                        {
                            countitem[i] = product;
                            i++;
                        }
                        catch (Exception e) { }

                    }
                }

            }
            return Json(new { success = "Ok", data = countitem });
        }
        // GET: Admin/AdminDiscounts/SearchCustomer full customer
        public IActionResult SearchCustomer()
        {
            var customer = _context.Customers.ToList();
           return Json(new {success ="Ok",data =customer});
        }

        // POST: Admin/AdminDiscounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]    
        public IActionResult CreateData(string Name, string Image, bool Gift, string MoTa,DateTime TimeOn,DateTime TimeOff,bool ShowWeb,int MaxApply,bool Startus,int Discount1,bool ConditionCheck,int Money, int ProductCount, int Sum, int[] listCustomer,int[] listProduct, int[] listGift)
        {
            var text = "Ok";
            KhuyenMai discount = new KhuyenMai();
            discount.Name = Name;
            discount.Image = Image;
            discount.Gift = Gift;
            discount.MoTa = MoTa;
            discount.TimeOn = TimeOn;
            discount.TimeOff = TimeOff;
            discount.ShowWeb = ShowWeb;
            discount.MaxApply = MaxApply;
            discount.Startus = Startus;
            discount.Discount1 = Discount1;
            discount.ConditionCheck = ConditionCheck;
            discount.ProductCount = ProductCount;
            discount.Money = Money;
            discount.Sum = Sum;
            _context.Add(discount);
            _context.SaveChanges();
            if (TimeOff < TimeOn)
            {
                text = "No2";
            }
            var id =  discount.Id;
            try {                
                try {
                    if (listCustomer.Length > 0)
                    {
                        foreach (var item in listCustomer)
                        {
                            KhuyenMaiThemKhachHang customer = new KhuyenMaiThemKhachHang();
                            customer.DiscountId = id;
                            customer.CustomerId = item;
                            customer.CheckDiscount = false;
                            _context.Add(customer);
                            _context.SaveChanges();
                        }
                    }
                    else
                    {
                        KhuyenMaiThemKhachHang customer = new KhuyenMaiThemKhachHang();
                        customer.DiscountId = id;
                        customer.CheckDiscount = true;
                        _context.Add(customer);
                        _context.SaveChanges();
                    }

                }
                catch (Exception ex) 
                { text = "No1"; }           

                // add product
                try {
                    if (Gift == false && listGift.Length > 0)
                    {

                        List<ProductGift> listgift = new List<ProductGift>();
                        foreach (var item in listGift)
                        {
                            var gift = _context.ProductGifts.Where(g => g.Id == item).FirstOrDefault();
                            listgift.Add(gift);
                        }
                        foreach (var item in listgift)
                        {
                            ThuocTinhQuaTang Gifts = new ThuocTinhQuaTang();
                            Gifts.DiscountId = id;
                            Gifts.ProductGiftId = item.Id;
                            _context.GitAttributes.Add(Gifts);
                            _context.SaveChanges();
                        }

                    }
                    if (listProduct.Length > 0)
                    {
                        foreach (var item in listProduct)
                        {
                            KhuyenMaiThemSanPham product = new KhuyenMaiThemSanPham();
                            product.DiscountId = id;
                            product.ProductId = item;
                            product.CheckDiscount = false;
                            _context.Add(product);
                            _context.SaveChanges();
                        }
                    }
                    else
                    {
                        var addProduct = _context.DiscountAddProducts
                            .Include(d=>d.Discount)
                            .Where(d=>d.Discount.TimeOff>DateTime.Now)                            
                            .ToList();
                        var productlist = _context.Products.ToList();
                        foreach (var item in addProduct)
                        {
                            productlist = productlist.Where(p=>p.ProductId != item.ProductId).ToList();
                        }
                        /// new list san pham ==0 thì ko tao khuyen mai
                        if (productlist.Count == 0)
                        {
                            text = "No2";
                        }
                        else if (productlist.Count >0 && productlist.Count < 30)
                        {
                            foreach (var item in productlist)
                            {
                            KhuyenMaiThemSanPham product = new KhuyenMaiThemSanPham();
                            product.DiscountId = id;
                            product.ProductId = item.ProductId;
                            product.CheckDiscount = false;
                            _context.Add(product);
                            _context.SaveChanges();
                            }

                        }
                        else
                        {
                            foreach (var item in productlist)
                            {
                                KhuyenMaiThemSanPham product = new KhuyenMaiThemSanPham();
                                product.DiscountId = id;
                                product.ProductId = item.ProductId;
                                product.CheckDiscount = true;
                                _context.Add(product);
                                _context.SaveChanges();
                            }
                        }
                    }
                    var taikhoanID = HttpContext.Session.GetString("MaTaiKhoan");
                    var user = _context.TaiKhoans.Where(x=>x.MaTaiKhoan==int.Parse(taikhoanID)).FirstOrDefault();
                    if (user !=null)
                    {
                        LichSuKhuyenMai his = new LichSuKhuyenMai();
                        his.DiscountId = id;
                        his.MoTa = "Khởi tạo bởi "+ user.HoTen + " id:" + taikhoanID;
                        his.TimeCreate = DateTime.Now;
                        _context.HistoryDiscounts.Add(his);
                        _context.SaveChanges();
                    }
                    
                } catch (Exception ex) { text = "No2"; }

                

            }
            catch(Exception e) {
                text = "No";
            }                    
            if(text == "No1")
            {
                var cus = _context.DiscountAddCustomers.Where(i=>i.DiscountId==id).ToList();
                _context.Remove(cus);
                _context.SaveChanges();
                var discout = _context.Discounts.Where(i => i.Id == id).FirstOrDefault();
                if (discount != null)
                {
                    _context.Remove(discout);
                    _context.SaveChanges();
                }
            }
            if(text == "No2")
            {
                var cus = _context.DiscountAddCustomers.Where(i => i.DiscountId == id).ToList();
                if (cus.Count >0)
                {
                    foreach(var item in cus)
                    {
                        _context.Remove(item);
                        _context.SaveChanges();

                    }
                }
                var Giftatrr = _context.GitAttributes.Where(i => i.DiscountId == id).ToList();
                if (Giftatrr.Count > 0)
                {
                    foreach (var item in Giftatrr)
                    {
                        _context.Remove(item);
                        _context.SaveChanges();
                    }

                }

                var pro = _context.DiscountAddProducts.Where(i => i.DiscountId == id).ToList();
                if (pro.Count > 0)
                {
                    foreach(var item in pro)
                    {
                    _context.Remove(item);
                    _context.SaveChanges();
                    }

                }

                var discout = _context.Discounts.Where(i => i.Id == id).FirstOrDefault();
                if (discount != null)
                {
                    _context.Remove(discout);
                    _context.SaveChanges();
                }
            }
            return Json(new { success = true,data =text }) ;
        }



        [HttpPost]
        public IActionResult CreateDiscountGift(string Code, string Image,bool Gift, string MoTa, DateTime TimeOn, DateTime TimeOff, bool ShowWeb, int MaxApply, bool Startus, int Discount1, bool ConditionCheck, int Money, int ProductCount, int Sum, int[] listCustomer, int[] listGift,bool ApplyGift)
        {
            var text = "Ok";
            KhuyenMai discount = new KhuyenMai();
            discount.Code = Code;
            discount.Gift = Gift;
            discount.Image = Image;
            discount.MoTa = MoTa;
            discount.TimeOn = TimeOn;
            discount.TimeOff = TimeOff;
            discount.ShowWeb = ShowWeb;
            discount.MaxApply = MaxApply;
            discount.Startus = Startus;
            discount.Discount1 = Discount1;
            discount.ConditionCheck = ConditionCheck;
            discount.ProductCount = ProductCount;
            discount.Money = Money;
            discount.Sum = Sum;
            discount.ApplyGift = ApplyGift;
            _context.Add(discount);
            _context.SaveChanges();
            if (TimeOff < TimeOn)
            {
                text = "No2";
            }
            var id = discount.Id;
            try
            {
                try
                {
                    if (listCustomer.Length > 0)
                    {
                        foreach (var item in listCustomer)
                        {
                            KhuyenMaiThemKhachHang customer = new KhuyenMaiThemKhachHang();
                            customer.DiscountId = id;
                            customer.CustomerId = item;
                            customer.CheckDiscount = false;
                            _context.Add(customer);
                            _context.SaveChanges();
                        }
                    }
                    else
                    {
                        KhuyenMaiThemKhachHang customer = new KhuyenMaiThemKhachHang();
                        customer.DiscountId = id;
                        customer.CheckDiscount = true;
                        _context.Add(customer);
                        _context.SaveChanges();
                    }

                }
                catch (Exception ex)
                { text = "No1"; }

                // add productGift
                try
                {
                    
                    if (Gift==false && listGift.Length > 0)
                    {
                         
                        List<ProductGift> listgift = new List<ProductGift>(); 
                        foreach(var item in listGift)
                        {
                            var gift = _context.ProductGifts.Where(g=>g.Id==item).FirstOrDefault();
                            listgift.Add(gift);
                        }
                        foreach(var item in listgift)
                        {
                            ThuocTinhQuaTang Gifts = new ThuocTinhQuaTang();
                            Gifts.DiscountId = id;
                            Gifts.ProductGiftId = item.Id;                          
                            _context.GitAttributes.Add(Gifts);
                            _context.SaveChanges();
                        }
                        
                    }
                     
                    var taikhoanID = HttpContext.Session.GetString("MaTaiKhoan");
                    var user = _context.TaiKhoans.Where(x => x.MaTaiKhoan == int.Parse(taikhoanID)).FirstOrDefault();
                    if (user != null)
                    {
                        LichSuKhuyenMai his = new LichSuKhuyenMai();
                        his.DiscountId = id;
                        his.MoTa = "Khởi tạo bởi " + user.HoTen + " id:" + taikhoanID;
                        his.TimeCreate = DateTime.Now;
                        _context.HistoryDiscounts.Add(his);
                        _context.SaveChanges();
                    }

                }
                catch (Exception ex) { text = "No2"; }



            }
            catch (Exception e)
            {
                text = "No";
            }
            if (text == "No1")
            {
                var cus = _context.DiscountAddCustomers.Where(i => i.DiscountId == id).ToList();
                _context.Remove(cus);
                _context.SaveChanges();
                var discout = _context.Discounts.Where(i => i.Id == id).FirstOrDefault();
                if (discount != null)
                {
                    _context.Remove(discout);
                    _context.SaveChanges();
                }
            }
            if (text == "No2")
            {
                var cus = _context.DiscountAddCustomers.Where(i => i.DiscountId == id).ToList();
                if (cus.Count > 0)
                {
                    foreach (var item in cus)
                    {
                        _context.Remove(item);
                        _context.SaveChanges();

                    }
                }


                var Giftatrr = _context.GitAttributes.Where(i => i.DiscountId == id).ToList();
                if (Giftatrr.Count > 0)
                {
                    foreach (var item in Giftatrr)
                    {
                        _context.Remove(item);
                        _context.SaveChanges();
                    }

                }

                var discout = _context.Discounts.Where(i => i.Id == id).FirstOrDefault();
                if (discount != null)
                {
                    _context.Remove(discout);
                    _context.SaveChanges();
                }
            }
            return Json(new { success = true, data = text });
        }



        public IActionResult loadFullProductGiff()
        {
            var gift = _context.ProductGifts.ToList();
            return Json(new { success = "OK", product = gift }) ;
        }


        // GET: Admin/AdminDiscounts/Edit/5
        public IActionResult Edit(int id, int option)
        {
            if (id == null || _context.Discounts == null)
            {
                return Json(new { success = "No" });
            }
            var discount = _context.Discounts.Find(id);
            if (discount == null)
            {
                return Json(new { success = "No" });
            }
            var taikhoanID = HttpContext.Session.GetString("MaTaiKhoan");
            var user = _context.TaiKhoans.Where(x => x.MaTaiKhoan == int.Parse(taikhoanID)).FirstOrDefault();

                LichSuKhuyenMai his = new LichSuKhuyenMai();
            try {
                his.DiscountId = id;
                if (option == 1 && user != null)
                {

                    if (discount.ShowWeb == true)
                    {
                        discount.ShowWeb = false;
                        his.MoTa = "Khuyến mãi đã bị ẩn hiển thị trên web bởi " + user.HoTen + " id:" + taikhoanID;
                    }
                    else
                    {
                        discount.ShowWeb = true;
                        his.MoTa = "Khuyến mãi đã được bật hiển thị trên web bởi " + user.HoTen + " id:" + taikhoanID;
                    }
                    _context.Update(discount);
                    _context.SaveChanges();

                }
                if (option == 2 && user != null)
                {
                    if (discount.Startus == true)
                    {
                        discount.Startus = false;
                        his.MoTa = "Khuyến mãi đã đổi trạng thái ẩn bởi " + user.HoTen + " id:" + taikhoanID;
                    }
                    else
                    {
                        discount.Startus = true;
                        his.MoTa = "Khuyến mãi đã đổi trạng thái bật bởi " + user.HoTen + " id:" + taikhoanID;
                    }
                    _context.Update(discount);
                    _context.SaveChanges();
                }               
                his.TimeCreate = DateTime.Now;
                _context.HistoryDiscounts.Add(his);
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                return Json(new { success = "No" });
            }       
           
            return Json(new {success="Ok"});
        }

        // POST: Admin/AdminDiscounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
     
        // GET: Admin/AdminDiscounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Discounts == null)
            {
                return NotFound();
            }
            var discount = await _context.Discounts
                .FirstOrDefaultAsync(m => m.Id == id);
           
            
            if (discount == null)
            {
                return NotFound();
            }
            var product = _context.DiscountAddProducts.Where(x => x.DiscountId == id).Where(x=>x.CheckDiscount==false)
               .Include(x => x.Product)
               .ToList();
            var customer = _context.DiscountAddCustomers.Where(p => p.DiscountId == id).Where(x => x.CheckDiscount == false).Include(p => p.Customer).ToList();
            var history = _context.HistoryDiscounts.Where(x => x.DiscountId == id).OrderByDescending(x=>x.TimeCreate).ToList();
            
            var Gift = _context.GitAttributes.Where(x => x.DiscountId == id).Include(p=>p.ProductGift).ToList();
            ViewBag.Gift = Gift;
            ViewBag.product = product;
            ViewBag.customer = customer;
            ViewBag.his = history;
            
            return View(discount);
        }

        // POST: Admin/AdminDiscounts/Delete/5
        [HttpPost]        
        public IActionResult Deleteitem(int id)
        {
            if (id == null || _context.Discounts == null)
            {
                return NotFound();
            }
            var discount = _context.Discounts
                .FirstOrDefault(m => m.Id == id);
            if (discount == null)
            {
                return NotFound();
            }

            try
            {
                var product = _context.DiscountAddProducts.Where(x => x.DiscountId == id).ToList();
                var customer = _context.DiscountAddCustomers.Where(x => x.DiscountId == id).ToList();
                var atribute = _context.GitAttributes.Where(x => x.DiscountId == id).ToList();
                var his =_context.HistoryDiscounts.Where(x=>x.DiscountId==id).ToList();
                if (atribute != null)
                {
                    foreach (var item in atribute)
                    {
                        _context.GitAttributes.Remove(item);
                    }
                }
                if (product != null)
                {
                    foreach(var item in product)
                    {
                        _context.DiscountAddProducts.Remove(item);
                    }
                }
                if (customer != null)
                {
                    foreach (var item in customer)
                    {
                        _context.DiscountAddCustomers.Remove(item);
                    }
                }
                if (his != null)
                {
                    foreach (var item in his)
                    {
                        _context.HistoryDiscounts.Remove(item);
                    }
                }

                _context.Remove(discount);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine($"An error occurred: {e.Message}");
                return Json(new { success = "No" });
            }


            return RedirectToAction("Index");
             
        }

        private bool DiscountExists(int id)
        {
          return _context.Discounts.Any(e => e.Id == id);
        }
    }
}
