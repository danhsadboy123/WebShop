using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class ChiTietDonHang
{
    public int OrderDetailId { get; set; }

    public int? OrderId { get; set; }

    public int? MaSanPham { get; set; }

    public int? OrderNumber { get; set; }

    public int? Amount { get; set; }

    public int? Discount { get; set; }

    public int? TotalMoney { get; set; }

    public DateTime? NgayTao { get; set; }

    public int? Gia { get; set; }

    public virtual DonHang Order { get; set; }

    public virtual SanPham SanPham { get; set; }
}
