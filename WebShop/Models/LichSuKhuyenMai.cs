using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class LichSuKhuyenMai
{
    public int Ma { get; set; }

    public int? MaKhuyenMai { get; set; }

    public string MoTa { get; set; }

    public DateTime? ThoiGianTao { get; set; }

    public virtual KhuyenMai KhuyenMai { get; set; }
}
