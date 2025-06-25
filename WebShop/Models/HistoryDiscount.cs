using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class HistoryDiscount
{
    public int Id { get; set; }

    public int? DiscountId { get; set; }

    public string Description { get; set; }

    public DateTime? TimeCreate { get; set; }

    public virtual Discount Discount { get; set; }
}
