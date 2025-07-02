using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce_WatchShop.Models;

public partial class BlogImage
{
    [Key]
    public int BlogImageId { get; set; }

    public int? BlogId { get; set; }
    [Column(TypeName = "nvarchar(255)")]
    public string? Contents { get; set; }
    public string? Image { get; set; }

    public virtual Blog? Blog { get; set; }
}
