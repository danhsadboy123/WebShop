using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class Guest
{
    public int GuestId { get; set; }

    public string HoTen { get; set; }

    public string SoDienThoai { get; set; }

    public string Email { get; set; }

    public DateTime? NgayTao { get; set; }

    public virtual ICollection<AccountAddress> AccountAddresses { get; set; } = new List<AccountAddress>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
