using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class ShippingAddress
{
    public int ShippingAdressId { get; set; }

    public int? OrderId { get; set; }

    public string Name { get; set; }

    public string Phone { get; set; }

    public int? ProvinceId { get; set; }

    public int? DistrictId { get; set; }

    public int? WardId { get; set; }

    public string Address { get; set; }

    public virtual District District { get; set; }

    public virtual Order Order { get; set; }

    public virtual Province Province { get; set; }

    public virtual Ward Ward { get; set; }
}
