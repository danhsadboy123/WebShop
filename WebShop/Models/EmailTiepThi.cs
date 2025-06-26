using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class EmailTiepThi
{
    public int MaEmail { get; set; }

    public int? MaTaiKhoan { get; set; }

    public string TieuDe { get; set; }

    public string TieuDeNoiDung { get; set; }

    public string TenNoiDung { get; set; }

    public string NoiDung { get; set; }

    public DateTime? NgayTao { get; set; }

    public DateTime? NgayTuyChinh { get; set; }

    public int? SuKienEmail { get; set; }

    public int? KichHoat { get; set; }

    public int? Input { get; set; }

    public virtual TaiKhoan TaiKhoan { get; set; }
}
