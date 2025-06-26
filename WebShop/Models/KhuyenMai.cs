using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class KhuyenMai
{
    public int Ma { get; set; }

    public string Ten { get; set; }

    public string Code { get; set; }

    public bool QuaTang { get; set; }

    public string HinhAnh { get; set; }

    public string MoTa { get; set; }

    public DateTime? ThoiGianBatDau { get; set; }

    public DateTime? ThoiGianKetThuc { get; set; }

    public bool HienThiTrenWeb { get; set; }

    public int? SoLanApDungToiDa { get; set; }

    public bool TrangThai { get; set; }

    public int? GiaTriKhuyenMai { get; set; }

    public bool KiemTraDieuKien { get; set; }

    public int? SoTien { get; set; }

    public int? SoLuongSanPham { get; set; }

    public int? TongCong { get; set; }

    public bool ApDungQuaTang { get; set; }

    public virtual ICollection<KhuyenMaiThemKhachHang> KhuyenMaiThemKhachHangs { get; set; } = new List<KhuyenMaiThemKhachHang>();

    public virtual ICollection<KhuyenMaiThemSanPham> KhuyenMaiThemSanPhams { get; set; } = new List<KhuyenMaiThemSanPham>();

    public virtual ICollection<ThuocTinhQuaTang> ThuocTinhQuaTangs { get; set; } = new List<ThuocTinhQuaTang>();

    public virtual ICollection<LichSuKhuyenMai> LichSuKhuyenMais { get; set; } = new List<LichSuKhuyenMai>();
}
