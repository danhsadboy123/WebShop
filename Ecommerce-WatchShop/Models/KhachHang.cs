using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce_WatchShop.Models;

public partial class KhachHang
{
    [Key]
    public int MaKhachHang { get; set; }
    [Column(TypeName = "nvarchar(200)")]
    public string? HoTen { get; set; }
    [Column(TypeName = "varchar(15)")]
    public string? SoDienThoai { get; set; }
    [Column(TypeName = "nvarchar(255)")]
    public string? DiaChi { get; set; }
    [Column(TypeName = "varchar(255)")]
    public string? Email { get; set; }
    [Column(TypeName = "varchar(200)")]
    public string? TenHienThi { get; set; }
    public DateOnly? NgaySinh { get; set; }

    public bool? GioiTinh { get; set; }

    public int? MaTaiKhoan { get; set; }

    public TaiKhoan TaiKhoan { get; set; }

    public virtual ICollection<HoaDon> Bills { get; set; } = new List<HoaDon>();

    public virtual ICollection<YeuThich> YeuThichs { get; set; } = new List<YeuThich>();

    public virtual ICollection<BinhLuanSanPham> ProductComments { get; set; } = new List<BinhLuanSanPham>();

    public virtual ICollection<DanhGiaSanPham> ProductRatings { get; set; } = new List<DanhGiaSanPham>();
}
