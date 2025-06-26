using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class KhuyenMaiThemKhachHang
{
    public int Ma { get; set; }

    public int? MaKhachHang { get; set; }

    public int? MaKhuyenMai { get; set; }

    public bool KiemTraKhuyenMai { get; set; }

    public virtual KhachHang KhachHang { get; set; }

    public virtual KhuyenMai KhuyenMai { get; set; }
}
