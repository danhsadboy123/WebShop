using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class CapDoKhachHang
{
    public int Ma { get; set; }

    public string TenCapDo { get; set; }

    public virtual ICollection<KhachHangTiemNang> KhachHangTiemNangs { get; set; } = new List<KhachHangTiemNang>();
}
