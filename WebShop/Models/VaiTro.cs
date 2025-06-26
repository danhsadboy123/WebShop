using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebShop.Models;

public partial class VaiTro
{
    [Key]
    public int MaVaiTro { get; set; }

    public string TenVaiTro { get; set; }

    public string MoTa { get; set; }

    public virtual ICollection<TaiKhoan> TaiKhoans { get; set; } = new List<TaiKhoan>();
}
