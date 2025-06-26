using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class DanhMuc
{
    public int MaDanhMuc { get; set; }

    public string TenDanhMuc { get; set; }

    public string TenDanhMucEn { get; set; }

    public string MoTa { get; set; }

    public string MoTaEn { get; set; }

    public int? MaDanhMucCha { get; set; }

    public int? Levels { get; set; }

    public bool NoiBat { get; set; }

    public int? ThuTu { get; set; }

    public bool HienThi { get; set; }

    public string HinhAnh { get; set; }

    public string TieuDe { get; set; }

    public string TieuDeEn { get; set; }

    public string DuongDan { get; set; }

    public string MoTaMeta { get; set; }

    public string MoTaMetaEn { get; set; }

    public string TuKhoaMeta { get; set; }

    public string TuKhoaMetaEn { get; set; }

    public string AnhBia { get; set; }

    public string SchemaMarkup { get; set; }

    public string Icon { get; set; }

    public bool? HienThiThuongHieu { get; set; }

    public bool? HienThiHinhAnh { get; set; }

    public string HinhAnhQuangCao { get; set; }

    public string TenThuongHieu { get; set; }

    public virtual ICollection<Banner> Banners { get; set; } = new List<Banner>();

    public virtual ICollection<DanhMucThuocTinh> DanhMucThuocTinhs { get; set; } = new List<DanhMucThuocTinh>();

    public virtual ICollection<DanhMucThuongHieu> DanhMucThuongHieus { get; set; } = new List<DanhMucThuongHieu>();

    public virtual ICollection<DanhMuc> DanhMucCon { get; set; } = new List<DanhMuc>();

    public virtual DanhMuc DanhMucCha { get; set; }

    public virtual ICollection<SanPhamDanhMuc> SanPhamDanhMucs { get; set; } = new List<SanPhamDanhMuc>();

    public virtual ICollection<Slide> Slides { get; set; } = new List<Slide>();
}
