using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class AnhSanPham
{
    public int ImageId { get; set; }

    public int? ProductId { get; set; }

    public string Alias { get; set; }

    public int? Ordering { get; set; }

    public bool? IsMain { get; set; }

    public virtual Product Product { get; set; }
}
