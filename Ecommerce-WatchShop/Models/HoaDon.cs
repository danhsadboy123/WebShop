using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce_WatchShop.Models;

public partial class HoaDon
{
    [Key]
    public int MaHoaDon { get; set; }

    public int MaKhachHang { get; set; }

    public DateTime NgayDatHang { get; set; }

    [Column(TypeName = "nvarchar(200)")]
    public string? HoTen { get; set; }
    [Column(TypeName = "varchar(15)")]
    public string? SoDienThoai { get; set; }
    [Column(TypeName = "varchar(255)")]
    public string? Email { get; set; }
    [Column(TypeName = "nvarchar(255)")]
    public string? DiaChi { get; set; } 
    [Column(TypeName = "nvarchar(200)")]
    public string? Tinh { get; set; }
    [Column(TypeName = "nvarchar(200)")]
    public string? Huyen { get; set; }
    [Column(TypeName = "nvarchar(200)")]
    public string? Xa { get; set; } 
    [Column(TypeName = "nvarchar(50)")]
    public string? PhuongThucThanhToan { get; set; }
    [Column(TypeName = "decimal(18,0)")]
    public decimal TongTien { get; set; } 

    public int TrangThai { get; set; }

    public virtual KhachHang? KhachHang { get; set; }

    public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();
}
