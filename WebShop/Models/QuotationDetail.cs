using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class QuotationDetail
{
    public int QuotationDetailId { get; set; }

    public int? QuotationId { get; set; }

    public int? ProductId { get; set; }

    public int? Amount { get; set; }

    public int? Discount { get; set; }

    public int? Price { get; set; }

    public DateTime? NgayTao { get; set; }

    public virtual Product Product { get; set; }

    public virtual Quotation Quotation { get; set; }
}
