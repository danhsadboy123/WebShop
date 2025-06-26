using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class QuaTangSanPham
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string AnhDaiDien { get; set; }

    public string TenRutGon { get; set; }

    public int? Gia { get; set; }

    public virtual ICollection<ThuocTinhQuaTang> GitAttributes { get; set; } = new List<ThuocTinhQuaTang>();
}
