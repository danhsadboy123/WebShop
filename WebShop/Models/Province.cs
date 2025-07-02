using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class Province
{
    public int ProvinceId { get; set; }

    public string ProvinceName { get; set; }

    public string Type { get; set; }

    public virtual ICollection<AccountAddress> AccountAddresses { get; set; } = new List<AccountAddress>();

    public virtual ICollection<District> Districts { get; set; } = new List<District>();

    public virtual ICollection<ShippingAddress> ShippingAddresses { get; set; } = new List<ShippingAddress>();
}
