using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class ThuocTinhTiemNang
{
    public int Ma { get; set; }

    public int? MaTiemNang { get; set; }

    public string NoiDung { get; set; }

    public DateTime? ThoiGianGui { get; set; }

    public virtual KhachHangTiemNang TiemNang { get; set; }
}
