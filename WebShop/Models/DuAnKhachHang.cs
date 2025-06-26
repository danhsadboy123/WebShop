using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class DuAnKhachHang
{
    public int Ma { get; set; }

    public string HinhAnh { get; set; }

    public string DiaChi { get; set; }

    public string MoTa { get; set; }

    public DateTime? NamBatDau { get; set; }

    public DateTime? NamKetThuc { get; set; }

    public string LienKet { get; set; }
}
