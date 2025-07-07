using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce_CaFeShop.Models;

public class Slider
{
    [Key]
    public int MaSlider { get; set; }

    [Column(TypeName = "nvarchar(255)")]
    public string? TieuDe { get; set; }

    [Column(TypeName = "nvarchar(500)")]
    public string? MoTa { get; set; }

    [Column(TypeName = "nvarchar(255)")]
    public string? HinhAnh { get; set; }

    [Column(TypeName = "varchar(500)")]
    public string? Link { get; set; }

    public int? ThuTuHienThi { get; set; }

    public bool TrangThai { get; set; }

    public DateTime? NgayTao { get; set; }

    public DateTime? NgayCapNhat { get; set; }
}