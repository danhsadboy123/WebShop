using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class TrangThaiGiaoDich
{
    public int TransactStatusId { get; set; }

    public string Status { get; set; }

    public string MoTa { get; set; }

    public virtual ICollection<DonHang> Orders { get; set; } = new List<DonHang>();
}
