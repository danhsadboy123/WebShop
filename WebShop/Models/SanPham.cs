using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class SanPham
{
    public int MaSanPham { get; set; }

    public string TenSanPham { get; set; }

    public string TenSanPhamTiengAnh { get; set; }

    public string MaCode { get; set; }

    public string MoTaNgan { get; set; }

    public string MoTaNganTiengAnh { get; set; }

    public string MoTa { get; set; }

    public string MoTaTiengAnh { get; set; }

    public string ThongTinCauHinh { get; set; }

    public string ThongTinCauHinhTiengAnh { get; set; }

    public string QuaTang { get; set; }

    public string QuaTangTiengAnh { get; set; }

    public int? BaoHanh { get; set; }

    public string GhiChuBaoHanh { get; set; }

    public string GhiChuBaoHanhTiengAnh { get; set; }

    public int? Gia { get; set; }

    public int? GiaBan { get; set; }

    public int? MaThuongHieu { get; set; }

    public string AnhDaiDien { get; set; }

    public string PhimAnh { get; set; }

    public DateTime? NgayTao { get; set; }

    public DateTime? NgayCapNhat { get; set; }

    public bool BanChay { get; set; }

    public bool HienThiTrangChu { get; set; }

    public bool KichHoat { get; set; }

    public string TieuDe { get; set; }

    public string TieuDeTiengAnh { get; set; }

    public string TenRutGon { get; set; }

    public string MoTaSeo { get; set; }

    public string MoTaSeoTiengAnh { get; set; }

    public string TuKhoaSeo { get; set; }

    public string TuKhoaSeoTiengAnh { get; set; }

    public int? SoLuongTon { get; set; }

    public string TuyChonSanPham { get; set; }

    public int? NhomThuongHieu { get; set; }

    public virtual ICollection<GiaThuocTinh> GiaThuocTinh { get; set; } = new List<GiaThuocTinh>();

    public virtual ThuongHieu ThuongHieu { get; set; }

    public virtual ICollection<KhuyenMaiThemSanPham> KhuyenMaiThemSanPham { get; set; } = new List<KhuyenMaiThemSanPham>();

    public virtual ICollection<ChiTietDonHang> ChiTietDonHang { get; set; } = new List<ChiTietDonHang>();

    public virtual ICollection<SanPhamThemKhachHang> SanPhamThemKhachHang { get; set; } = new List<SanPhamThemKhachHang>();

    public virtual ICollection<DanhMucSanPham> DanhMucSanPham { get; set; } = new List<DanhMucSanPham>();

    public virtual ICollection<AnhSanPham> AnhSanPham { get; set; } = new List<AnhSanPham>();

    public virtual ICollection<ChiTietBaoGia> ChiTietBaoGia { get; set; } = new List<ChiTietBaoGia>();
}
