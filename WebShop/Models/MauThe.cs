using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class MauThe
{
    public int MaMauThe { get; set; }

    public string TieuDe { get; set; }

    public string MoTa { get; set; }

    public string NoiDungHtml { get; set; }

    public int? Loai { get; set; }

    public DateTime? NgayTao { get; set; }

    public DateTime? NgaySua { get; set; }
}
