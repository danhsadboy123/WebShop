using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class GitAttribute
{
    public int Id { get; set; }

    public int? DiscountId { get; set; }

    public int? ProductGiftId { get; set; }

    public virtual Discount Discount { get; set; }

    public virtual ProductGift ProductGift { get; set; }
}
