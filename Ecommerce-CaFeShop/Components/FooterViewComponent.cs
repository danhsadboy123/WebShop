using Ecommerce_CaFeShop.Models;
using Ecommerce_CaFeShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_CaFeShop.Components
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly CaFeContext _context;

        public FooterViewComponent(CaFeContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var footer = await _context.Footers.FirstOrDefaultAsync();
            var footerLinks = await _context.FooterLinks.ToListAsync();

            var informationLinks = footerLinks?.Where(link => link.MaNhom == 1).ToList() ?? new List<FooterLink>();
            var accountLinks = footerLinks?.Where(link => link.MaNhom == 2).ToList() ?? new List<FooterLink>();

            var footerVM = new FooterVM
            {
                Footer = footer,
                InformationLinks = informationLinks,
                AccountLinks = accountLinks
            };

            return View(footerVM);
        }
    }
}