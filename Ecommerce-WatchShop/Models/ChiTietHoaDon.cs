using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce_WatchShop.Models;

public partial class ChiTietHoaDon
{
    [Key]
    public int MaChiTietHoaDon { get; set; }

    public int MaHoaDon { get; set; }

    public int MaSanPham { get; set; }

    [Precision(18, 0)]
    public decimal Gia { get; set; }

    public int SoLuong { get; set; }

    [Precision(18, 0)]
    public decimal TongTien { get; set; }

    public virtual HoaDon HoaDon { get; set; } = null!;

    public virtual SanPham SanPham { get; set; } = null!;
}
