using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class QuangCao
{
    public int QuangCaoId { get; set; }

    public string SubTitle { get; set; }

    public string TieuDe { get; set; }

    public string ImageBg { get; set; }

    public string ImageProduct { get; set; }

    public string UrlLink { get; set; }

    public bool KichHoat { get; set; }

    public DateTime? NgayTao { get; set; }
}
