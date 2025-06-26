using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class AnhSanPham
{
    public int ImageId { get; set; }

    public int? MaSanPham { get; set; }

    public string TenRutGon { get; set; }

    public int? Ordering { get; set; }

    public bool? IsMain { get; set; }

    public virtual SanPham SanPham { get; set; }
}
