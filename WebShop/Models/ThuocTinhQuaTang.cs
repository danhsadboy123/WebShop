using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class ThuocTinhQuaTang
{
    public int Ma { get; set; }

    public int? MaKhuyenMai { get; set; }

    public int? MaSanPhamQuaTang { get; set; }

    public virtual KhuyenMai KhuyenMai { get; set; }

    public virtual SanPhamQuaTang SanPhamQuaTang { get; set; }
}
