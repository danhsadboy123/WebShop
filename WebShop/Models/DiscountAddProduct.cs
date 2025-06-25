using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class DiscountAddProduct
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public int? DiscountId { get; set; }

    public bool CheckDiscount { get; set; }

    public virtual Discount Discount { get; set; }

    public virtual Product Product { get; set; }
}
