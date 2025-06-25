namespace WebShop.Areas.Admin.Models
{
    public class OrderFee
    {
        public decimal CODFailedFee { get; set; }
        public decimal CODFee { get; set; }
        public decimal Coupon { get; set; }
        public decimal DeliverRemoteAreasFee { get; set; }
        public decimal DocumentReturn { get; set; }
        public decimal DoubleCheck { get; set; }
        public decimal Insurance { get; set; }
        public decimal MainService { get; set; }
        public decimal PickRemoteAreasFee { get; set; }
        public decimal R2S { get; set; }
        public decimal Return { get; set; }
        public decimal StationDO { get; set; }
        public decimal StationPU { get; set; }
        public decimal Total { get; set; }
    }
}
