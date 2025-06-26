using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class KhachVangLai
{
    public int MaKhachVangLai { get; set; }

    public string HoTen { get; set; }

    public string SoDienThoai { get; set; }

    public string Email { get; set; }

    public DateTime? NgayTao { get; set; }

    public virtual ICollection<DiaChiTaiKhoan> DiaChiTaiKhoans { get; set; } = new List<DiaChiTaiKhoan>();

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
}
