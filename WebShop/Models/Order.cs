using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? CustomerId { get; set; }

    public int? GuestId { get; set; }

    public string SoDienThoai { get; set; }

    public DateTime? OrderDate { get; set; }

    public DateTime? ShipDate { get; set; }

    public int DeliveryStatusId { get; set; }

    public int PaymentStatusId { get; set; }

    public int CodstatusId { get; set; }

    public bool Deleted { get; set; }

    public DateTime? PaymentDate { get; set; }

    public int TotalMoney { get; set; }

    public bool? CheckEmail { get; set; }

    public int? PaymentId { get; set; }

    public bool Confirmed { get; set; }

    public string Note { get; set; }

    public bool Draft { get; set; }

    public string Code { get; set; }

    public string NotePay { get; set; }

    public string NodeCus { get; set; }

    public int? Discount { get; set; }

    public string RedBill { get; set; }

    public virtual Codstatus Codstatus { get; set; }

    public virtual Customer Customer { get; set; }

    public virtual DeliveryStatus DeliveryStatus { get; set; }

    public virtual Guest Guest { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual PaymentStatus PaymentStatus { get; set; }

    public virtual ICollection<ShippingAddress> ShippingAddresses { get; set; } = new List<ShippingAddress>();
}
