using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class KhuyenMaiThemSanPham
{
    public int Ma { get; set; }

    public int? MaSanPham { get; set; }

    public int? MaKhuyenMai { get; set; }

    public bool KiemTraKhuyenMai { get; set; }

    public virtual KhuyenMai KhuyenMai { get; set; }

    public virtual SanPham SanPham { get; set; }
}
