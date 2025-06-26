using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class TrangWeb
{
    public int PageId { get; set; }

    public string PageName { get; set; }

    public string NoiDung { get; set; }

    public string AnhNho { get; set; }

    public bool DaXuatBan { get; set; }

    public string TieuDe { get; set; }

    public string MoTaSeo { get; set; }

    public string TuKhoaSeo { get; set; }

    public string TenRutGon { get; set; }

    public DateTime? NgayTao { get; set; }

    public int? Ordering { get; set; }
}
