namespace WebShop.Areas.Admin.Models
{
    public class CheckboxOrder
    {
        public bool OrderId { get; set; }
        public bool OrderDate { get; set; }
        public bool CustomerName { get; set; }
        public bool Email { get; set; }
        public bool Phone { get; set; }
        public bool PaymentStatus { get; set; }
        public bool DeliverStatus { get; set; }
        public bool CodStatus { get; set; }
        public bool Total { get; set; }

    }
}