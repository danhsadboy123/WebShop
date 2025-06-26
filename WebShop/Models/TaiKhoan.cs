using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebShop.Models;

public partial class TaiKhoan
{
    [Key]
    public int MaTaiKhoan { get; set; }

    public string SoDienThoai { get; set; }

    public string Email { get; set; }

    public string MatKhau { get; set; }

    public string Salt { get; set; }

    public bool KichHoat { get; set; }

    public string HoTen { get; set; }

    public int? MaVaiTro { get; set; }

    public DateTime? LanDangNhapCuoi { get; set; }

    public DateTime? NgayTao { get; set; }

    public virtual ICollection<EmailTiepThi> EmailMakettings { get; set; } = new List<EmailTiepThi>();

    public virtual VaiTro VaiTro { get; set; }
}
