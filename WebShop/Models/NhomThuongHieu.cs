using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class NhomThuongHieu
{
    public int Ma { get; set; }

    public int? MaThuongHieu { get; set; }

    public int? MaDanhMuc { get; set; }

    public string Ten { get; set; }

    public string MoTa { get; set; }

    public bool? TrangThai { get; set; }

    public virtual ThuongHieu ThuongHieu { get; set; }
}
