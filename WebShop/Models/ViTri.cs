using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class ViTri
{
    public int MaViTri { get; set; }

    public string Ten { get; set; }

    public int? Cha { get; set; }

    public int? CapDo { get; set; }

    public string Slug { get; set; }

    public string TenKemLoai { get; set; }

    public string Loai { get; set; }

    public virtual ICollection<KhachHang> KhachHangs { get; set; } = new List<KhachHang>();
}
