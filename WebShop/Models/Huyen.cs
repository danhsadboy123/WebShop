using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class Huyen
{
    public int MaHuyen { get; set; }

    public int? MaTinh { get; set; }

    public string TenHuyen { get; set; }

    public string Loai { get; set; }

    public virtual ICollection<DiaChiTaiKhoan> DiaChiTaiKhoans { get; set; } = new List<DiaChiTaiKhoan>();

    public virtual Tinh Tinh { get; set; }

    public virtual ICollection<DiaChiGiaoHang> DiaChiGiaoHangs { get; set; } = new List<DiaChiGiaoHang>();

    public virtual ICollection<Xa> Xas { get; set; } = new List<Xa>();
}
