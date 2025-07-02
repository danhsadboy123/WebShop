using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class District
{
    public int DistrictId { get; set; }

    public int? ProvinceId { get; set; }

    public string DistrictName { get; set; }

    public string Type { get; set; }

    public virtual ICollection<AccountAddress> AccountAddresses { get; set; } = new List<AccountAddress>();

    public virtual Province Province { get; set; }

    public virtual ICollection<ShippingAddress> ShippingAddresses { get; set; } = new List<ShippingAddress>();

    public virtual ICollection<Ward> Wards { get; set; } = new List<Ward>();
}
