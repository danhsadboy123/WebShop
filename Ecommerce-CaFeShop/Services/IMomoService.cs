using Ecommerce_CaFeShop.Models.Momo;
using Ecommerce_CaFeShop.Models.ViewModels;

namespace Ecommerce_CaFeShop.Services
{
    public interface IMomoService
    {
        Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(CheckoutVM model);
        MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection);
    }
}
