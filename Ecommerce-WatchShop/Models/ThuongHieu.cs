using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce_WatchShop.Models;

public partial class ThuongHieu
{
    [Key]
    public int MaThuongHieu { get; set; }
    
    [Column(TypeName = "nvarchar(100)")]
    public string? TenThuongHieu { get; set; }
    [Column(TypeName = "varchar(100)")]
    public string? Slug { get; set; }

    public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
}
