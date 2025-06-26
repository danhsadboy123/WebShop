using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class KhachHangTiemNang
{
    public int Ma { get; set; }

    public string Ten { get; set; }

    public string TenCongTy { get; set; }

    public string Email { get; set; }

    public string DiaChi { get; set; }

    public int? SoDienThoai { get; set; }

    public int? DaKiemTra { get; set; }

    public int? MaCapDo { get; set; }

    public virtual ICollection<ThuocTinhTiemNang> ThuocTinhTiemNangs { get; set; } = new List<ThuocTinhTiemNang>();

    public virtual CapDoKhachHang Lever { get; set; }
}
