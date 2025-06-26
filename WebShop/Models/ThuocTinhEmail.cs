using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class ThuocTinhEmail
{
    public int Ma { get; set; }

    public int MaKhachHang { get; set; }

    public string NoiDung { get; set; }

    public DateTime? ThoiGianGui { get; set; }

    public virtual KhachHang KhachHang { get; set; }
}
