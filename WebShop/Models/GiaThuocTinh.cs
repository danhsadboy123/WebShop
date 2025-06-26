using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class GiaThuocTinh
{
    public int MaGiaThuocTinh { get; set; }

    public int? MaThuocTinh { get; set; }

    public int? MaSanPham { get; set; }

    public string Gia { get; set; }

    public string GiaEn { get; set; }

    public bool KichHoat { get; set; }

    public virtual ThuocTinh ThuocTinh { get; set; }

    public virtual SanPham SanPham { get; set; }
}
