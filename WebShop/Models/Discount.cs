using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class Discount
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Code { get; set; }

    public bool Gift { get; set; }

    public string Image { get; set; }

    public string MoTa { get; set; }

    public DateTime? TimeOn { get; set; }

    public DateTime? TimeOff { get; set; }

    public bool ShowWeb { get; set; }

    public int? MaxApply { get; set; }

    public bool Startus { get; set; }

    public int? Discount1 { get; set; }

    public bool ConditionCheck { get; set; }

    public int? Money { get; set; }

    public int? ProductCount { get; set; }

    public int? Sum { get; set; }

    public bool ApplyGift { get; set; }

    public virtual ICollection<DiscountAddCustomer> DiscountAddCustomers { get; set; } = new List<DiscountAddCustomer>();

    public virtual ICollection<DiscountAddProduct> DiscountAddProducts { get; set; } = new List<DiscountAddProduct>();

    public virtual ICollection<GitAttribute> GitAttributes { get; set; } = new List<GitAttribute>();

    public virtual ICollection<HistoryDiscount> HistoryDiscounts { get; set; } = new List<HistoryDiscount>();
}
