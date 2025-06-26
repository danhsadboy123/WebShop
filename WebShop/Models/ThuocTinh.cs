using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class ThuocTinh
{
    public int MaThuocTinh { get; set; }

    public string Ten { get; set; }

    public string TenTiengAnh { get; set; }

    public int? ThuTu { get; set; }

    public bool KichHoat { get; set; }

    public virtual ICollection<GiaThuocTinh> GiaThuocTinhs { get; set; } = new List<GiaThuocTinh>();

    public virtual ICollection<DanhMucThuocTinh> DanhMucThuocTinhs { get; set; } = new List<DanhMucThuocTinh>();
}
