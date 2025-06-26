using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class DanhMucThuongHieu
{
    public int MaDanhMucThuongHieu { get; set; }

    public int? MaDanhMuc { get; set; }

    public int? MaThuongHieu { get; set; }

    public string Ten { get; set; }

    public string MoTa { get; set; }

    public bool? KichHoat { get; set; }

    public bool? ThuongHieuNoiBat { get; set; }

    public bool? SanPhamThuongHieu { get; set; }

    public virtual ThuongHieu ThuongHieu { get; set; }

    public virtual DanhMuc DanhMuc { get; set; }
}
