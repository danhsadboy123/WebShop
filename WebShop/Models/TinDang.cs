using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class TinDang
{
    public int MaBaiViet { get; set; }

    public string TieuDe { get; set; }

    public string NoiDungNgan { get; set; }

    public string NoiDung { get; set; }

    public string AnhNho { get; set; }

    public bool DaXuatBan { get; set; }

    public string TenRutGon { get; set; }

    public DateTime? NgayTao { get; set; }

    public string TacGia { get; set; }

    public int? MaTaiKhoan { get; set; }

    public int? CatId { get; set; }

    public bool LaNong { get; set; }

    public bool LaTinMoi { get; set; }

    public string TuKhoaSeo { get; set; }

    public string MoTaSeo { get; set; }

    public int? LuotXem { get; set; }
}
