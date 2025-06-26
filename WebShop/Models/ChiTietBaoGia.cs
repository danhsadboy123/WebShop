using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class ChiTietBaoGia
{
    public int QuotationDetailId { get; set; }

    public int? QuotationId { get; set; }

    public int? MaSanPham { get; set; }

    public int? Amount { get; set; }

    public int? Discount { get; set; }

    public int? Gia { get; set; }

    public DateTime? NgayTao { get; set; }

    public virtual SanPham SanPham { get; set; }

    public virtual BaoGia BaoGia { get; set; }
}
