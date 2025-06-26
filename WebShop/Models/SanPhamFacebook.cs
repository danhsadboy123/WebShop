using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class SanPhamFacebook
{
    public int Id { get; set; }

    public int? MaSanPham { get; set; }

    public string Link { get; set; }

    public DateTime? CeateAdd { get; set; }
}
