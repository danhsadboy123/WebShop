using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class DiaChiTaiKhoan
{
    public int MaDiaChi { get; set; }

    public int? MaKhachHang { get; set; }

    public int? MaKhachVangLai { get; set; }

    public string SoDienThoai { get; set; }

    public string TenNguoiDung { get; set; }

    public int? MaTinh { get; set; }

    public int? MaHuyen { get; set; }

    public int? MaXa { get; set; }

    public string NoiDung { get; set; }

    public bool? IsDefault { get; set; }

    public virtual KhachHang KhachHang { get; set; }

    public virtual Huyen Huyen { get; set; }

    public virtual KhachVangLai KhachVangLai { get; set; }

    public virtual Tinh Tinh { get; set; }

    public virtual Xa Xa { get; set; }
}
