using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class ThuongHieu
{
    public int MaThuongHieu { get; set; }

    public string TenThuongHieu { get; set; }

    public string MoTa { get; set; }

    public string HinhAnh { get; set; }

    public virtual ICollection<NhomThuongHieu> NhomThuongHieus { get; set; } = new List<NhomThuongHieu>();

    public virtual ICollection<DanhMucThuongHieu> DanhMucThuongHieus { get; set; } = new List<DanhMucThuongHieu>();

    public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();

    public virtual ICollection<PhimAnh> Videos { get; set; } = new List<PhimAnh>();
}
