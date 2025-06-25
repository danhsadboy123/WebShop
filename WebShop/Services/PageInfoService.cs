using Microsoft.CodeAnalysis;
using System.Linq;
using WebShop.Models;

namespace WebShop.Services
{
    public interface IPageInfoService
    {
        PageInfo GetPageInfo();
    }

    public class PageInfoService : IPageInfoService
    {
        private readonly DbMarketsContext _context;
       public PageInfoService(DbMarketsContext context)
        {
            _context = context;
        }

        public PageInfo GetPageInfo()
        {
            return _context.PageInfos.FirstOrDefault();
        }
    }
}
