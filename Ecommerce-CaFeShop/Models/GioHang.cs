
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce_CaFeShop.Models
{
    public class GioHang
    {
        [Key]
        public int MaGioHang { get; set; }

        [Required]
        public int MaKhachHang { get; set; }

        [Required]
        public int MaSanPham { get; set; }

        [Required]
        public int SoLuong { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Gia { get; set; }

        public DateTime NgayThem { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("MaKhachHang")]
        public virtual KhachHang? KhachHang { get; set; }

        [ForeignKey("MaSanPham")]
        public virtual SanPham? SanPham { get; set; }
    }
}
