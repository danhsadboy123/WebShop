using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class BaoGia
{
    public int QuotationId { get; set; }

    public int? CustomerId { get; set; }

    public int? TotalMoney { get; set; }

    public decimal? Vat { get; set; }

    public DateTime? NgayTao { get; set; }

    public string Note { get; set; }

    public bool? Confirmed { get; set; }

    public virtual KhachHang Customer { get; set; }

    public virtual ICollection<ChiTietBaoGia> ChiTietBaoGia { get; set; } = new List<ChiTietBaoGia>();
}
