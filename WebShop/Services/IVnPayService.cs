using CodeMegaVNPay.Models;
using Microsoft.AspNetCore.Http;
using WebShop.Models;

namespace CodeMegaVNPay.Services;
public interface IVnPayService
{
    string CreatePaymentUrl(PhuongThucThanhToan model, HttpContext context);
    PhanHoiThanhToan PaymentExecute(IQueryCollection collections);
}