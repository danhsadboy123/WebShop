using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class NhaCungCapKhachHang
{
    public int Ma { get; set; }

    public string HinhAnh { get; set; }

    public string Ten { get; set; }

    public string CongTy { get; set; }

    public int? SoDienThoai { get; set; }

    public string Email { get; set; }

    public string DiaChi { get; set; }

    public DateTime? NamThem { get; set; }

    public int? Lever { get; set; }

    public virtual ICollection<SanPhamThemNhaCungCap> SanPhamThemNhaCungCaps { get; set; } = new List<SanPhamThemNhaCungCap>();
}
