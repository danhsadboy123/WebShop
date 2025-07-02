using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce_WatchShop.Models;

public partial class Blog
{
    [Key]
    public int BlogId { get; set; }
    [Column(TypeName = "varchar(255)")]
    public string? Image { get; set; }
    [Column(TypeName = "nvarchar(255)")]
    public string? Subject { get; set; }

    [Column(TypeName = "nvarchar(MAX)")]
    public string? Contents { get; set; }

    public virtual ICollection<BlogImage> BlogImages { get; set; } = new List<BlogImage>();
}
