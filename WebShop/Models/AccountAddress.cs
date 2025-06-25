using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class AccountAddress
{
    public int AddressId { get; set; }

    public int? CustomerId { get; set; }

    public int? GuestId { get; set; }

    public string Phone { get; set; }

    public string UserName { get; set; }

    public int? ProvinceId { get; set; }

    public int? DistrictId { get; set; }

    public int? WardId { get; set; }

    public string Content { get; set; }

    public bool? IsDefault { get; set; }

    public virtual Customer Customer { get; set; }

    public virtual District District { get; set; }

    public virtual Guest Guest { get; set; }

    public virtual Province Province { get; set; }

    public virtual Ward Ward { get; set; }
}
