using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class KhachHang
{
    public int MaKhachHang { get; set; }

    public string HoTen { get; set; }

    public DateTime? NgaySinh { get; set; }

    public string AnhDaiDien { get; set; }

    public string Email { get; set; }

    public string SoDienThoai { get; set; }

    public DateTime? NgayTao { get; set; }

    public string MatKhau { get; set; }

    public string Salt { get; set; }

    public DateTime? LanDangNhapCuoi { get; set; }

    public bool KichHoat { get; set; }

    public bool? GioiTinh { get; set; }

    public string GhiChu { get; set; }

    public string TenCongTy { get; set; }

    public virtual ICollection<DiaChiTaiKhoan> DiaChiTaiKhoans { get; set; } = new List<DiaChiTaiKhoan>();

    public virtual ICollection<KhuyenMaiThemKhachHang> KhuyenMaiThemKhachHangs { get; set; } = new List<KhuyenMaiThemKhachHang>();

    public virtual ICollection<ThuocTinhEmail> ThuocTinhEmails { get; set; } = new List<ThuocTinhEmail>();

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();

    public virtual ICollection<BaoGia> BaoGias { get; set; } = new List<BaoGia>();
}
