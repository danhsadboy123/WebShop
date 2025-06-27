using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class TrangThaiThanhToan
{
    public int MaTrangThaiThanhToan { get; set; }

    public string TrangThai { get; set; }

    public string MoTa { get; set; }

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
}