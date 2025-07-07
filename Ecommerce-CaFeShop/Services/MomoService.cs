
using Ecommerce_CaFeShop.Models.Momo;
using Ecommerce_CaFeShop.Models.ViewModels;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Ecommerce_CaFeShop.Services
{
    public class MomoService : IMomoService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public MomoService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(CheckoutVM model)
        {
            try
            {
                var endpoint = _configuration["Momo:MomoApiUrl"];
                var partnerCode = _configuration["Momo:PartnerCode"];
                var accessKey = _configuration["Momo:AccessKey"];
                var secretKey = _configuration["Momo:SecretKey"];
                var orderInfo = "Thanh toán đơn hàng";
                var redirectUrl = _configuration["Momo:ReturnUrl"];
                var ipnUrl = _configuration["Momo:NotifyUrl"];
                var requestType = "payWithMethod";

                var amount = model.TotalAmount.ToString();
                var orderId = DateTime.UtcNow.Ticks.ToString();
                var requestId = DateTime.UtcNow.Ticks.ToString();
                var extraData = "";

                var rawHash = "accessKey=" + accessKey +
                             "&amount=" + amount +
                             "&extraData=" + extraData +
                             "&ipnUrl=" + ipnUrl +
                             "&orderId=" + orderId +
                             "&orderInfo=" + orderInfo +
                             "&partnerCode=" + partnerCode +
                             "&redirectUrl=" + redirectUrl +
                             "&requestId=" + requestId +
                             "&requestType=" + requestType;

                var signature = ComputeHmacSha256(rawHash, secretKey);

                var message = new
                {
                    partnerCode = partnerCode,
                    partnerName = "Test",
                    storeId = "MomoTestStore",
                    requestId = requestId,
                    amount = amount,
                    orderId = orderId,
                    orderInfo = orderInfo,
                    redirectUrl = redirectUrl,
                    ipnUrl = ipnUrl,
                    lang = "vi",
                    extraData = extraData,
                    requestType = requestType,
                    signature = signature
                };

                var jsonMessage = JsonSerializer.Serialize(message);
                var content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(endpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<MomoCreatePaymentResponseModel>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                return new MomoCreatePaymentResponseModel
                {
                    ErrorCode = -1,
                    Message = ex.Message
                };
            }
        }

        public MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection)
        {
            var amount = collection["amount"];
            var orderInfo = collection["orderInfo"];
            var orderId = collection["orderId"];
            var partnerCode = collection["partnerCode"];
            var requestId = collection["requestId"];
            var responseTime = collection["responseTime"];
            var resultCode = collection["resultCode"];
            var message = collection["message"];
            var payType = collection["payType"];
            var transId = collection["transId"];
            var signature = collection["signature"];

            return new MomoExecuteResponseModel()
            {
                Amount = amount,
                OrderId = orderId,
                OrderInfo = orderInfo,
                PartnerCode = partnerCode,
                RequestId = requestId,
                ResponseTime = responseTime,
                ErrorCode = int.Parse(resultCode),
                Message = message,
                PayType = payType,
                TransId = transId,
                Signature = signature
            };
        }

        private string ComputeHmacSha256(string message, string secretKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            using (var hmac = new HMACSHA256(keyBytes))
            {
                var hashBytes = hmac.ComputeHash(messageBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
