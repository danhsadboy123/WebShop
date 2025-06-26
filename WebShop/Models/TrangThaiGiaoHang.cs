using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class TrangThaiGiaoHang
{
    public int MaTrangThaiGiaoHang { get; set; }

    public string TrangThai { get; set; }

    public string MoTa { get; set; }

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
}
