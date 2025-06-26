using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class Tinh
{
    public int MaTinh { get; set; }

    public string TenTinh { get; set; }

    public string Loai { get; set; }

    public virtual ICollection<DiaChiTaiKhoan> DiaChiTaiKhoans { get; set; } = new List<DiaChiTaiKhoan>();

    public virtual ICollection<Huyen> Huyens { get; set; } = new List<Huyen>();

    public virtual ICollection<DiaChiGiaoHang> DiaChiGiaoHangs { get; set; } = new List<DiaChiGiaoHang>();
}
