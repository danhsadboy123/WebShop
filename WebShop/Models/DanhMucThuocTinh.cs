using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class DanhMucThuocTinh
{
    public int MaDanhMucThuocTinh { get; set; }

    public int? MaDanhMuc { get; set; }

    public int? MaThuocTinh { get; set; }

    public string Ten { get; set; }

    public bool? KichHoat { get; set; }

    public bool? HienThi { get; set; }

    public int? ThuTu { get; set; }

    public virtual ThuocTinh ThuocTinh { get; set; }

    public virtual DanhMuc DanhMuc { get; set; }
}
