using System;

namespace WebShop.Areas.Admin.Models
{
    public class GHNOrder
    {
        public decimal CODAmount { get; set; }
        public DateTime? CODTransferDate { get; set; }
        public string ClientOrderCode { get; set; }
        public decimal ConvertedWeight { get; set; }
        public string MoTa { get; set; }
        public OrderFee Fee { get; set; }
        public decimal Height { get; set; }
        public bool IsPartialReturn { get; set; }
        public decimal Length { get; set; }
        public string OrderCode { get; set; }
        public string PartialReturnCode { get; set; }
        public int PaymentType { get; set; }
        public string Reason { get; set; }
        public string ReasonCode { get; set; }
        public long ShopID { get; set; }
        public string Status { get; set; }
        public DateTime Time { get; set; }
        public decimal TotalFee { get; set; }
        public string Type { get; set; }
        public string Warehouse { get; set; }
        public decimal Weight { get; set; }
        public decimal Width { get; set; }
    }
}
