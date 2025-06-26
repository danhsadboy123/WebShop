using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class PaymentStatus
{
    public int PaymentStatusId { get; set; }

    public string Status { get; set; }

    public string MoTa { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
