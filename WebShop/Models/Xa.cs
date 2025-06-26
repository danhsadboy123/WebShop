using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class Xa
{
    public int MaXa { get; set; }

    public int? MaHuyen { get; set; }

    public string TenXa { get; set; }

    public string Loai { get; set; }

    public virtual ICollection<DiaChiTaiKhoan> DiaChiTaiKhoans { get; set; } = new List<DiaChiTaiKhoan>();

    public virtual Huyen Huyen { get; set; }

    public virtual ICollection<DiaChiGiaoHang> DiaChiGiaoHang { get; set; } = new List<DiaChiGiaoHang>();
}
