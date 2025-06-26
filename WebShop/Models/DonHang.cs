using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class DonHang
{
    public int MaDonHang { get; set; }

    public int? MaKhachHang { get; set; }

    public int? MaKhachVangLai { get; set; }

    public string SoDienThoai { get; set; }

    public DateTime? NgayDatHang { get; set; }

    public DateTime? NgayGiaoHang { get; set; }

    public int MaTrangThaiGiaoHang { get; set; }

    public int MaTrangThaiThanhToan { get; set; }

    public int MaTrangThaiThanhToanCod { get; set; }

    public bool DaXoa { get; set; }

    public DateTime? NgayThanhToan { get; set; }

    public int TongTien { get; set; }

    public bool? KiemTraEmail { get; set; }

    public int? MaThanhToan { get; set; }

    public bool DaXacNhan { get; set; }

    public string GhiChu { get; set; }

    public bool Nhap { get; set; }

    public string Code { get; set; }

    public string GhiChuThanhToan { get; set; }

    public string GhiChuKhachHang { get; set; }

    public int? KhuyenMai { get; set; }

    public string HoaDonDo { get; set; }

    public virtual TrangThaiThanhToan TrangThaiThanhToans { get; set; }

    public virtual KhachHang KhachHang { get; set; }

    public virtual TrangThaiGiaoHang TrangThaiGiaoHang { get; set; }

    public virtual KhachVangLai KhachVangLai { get; set; }

    public virtual ICollection<ChiTietDonHang> ChiTietDonHang { get; set; } = new List<ChiTietDonHang>();

    public virtual TrangThaiThanhToan TrangThaiThanhToan { get; set; }

    public virtual ICollection<DiaChiGiaoHang> DiaChiGiaoHang { get; set; } = new List<DiaChiGiaoHang>();
}
