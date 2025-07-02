using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class Ward
{
    public int WardId { get; set; }

    public int? DistrictId { get; set; }

    public string WardName { get; set; }

    public string Type { get; set; }

    public virtual ICollection<AccountAddress> AccountAddresses { get; set; } = new List<AccountAddress>();

    public virtual District District { get; set; }

    public virtual ICollection<ShippingAddress> ShippingAddresses { get; set; } = new List<ShippingAddress>();
}
