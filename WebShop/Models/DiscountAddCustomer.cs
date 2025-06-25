using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class DiscountAddCustomer
{
    public int Id { get; set; }

    public int? CustomerId { get; set; }

    public int? DiscountId { get; set; }

    public bool CheckDiscount { get; set; }

    public virtual Customer Customer { get; set; }

    public virtual Discount Discount { get; set; }
}
