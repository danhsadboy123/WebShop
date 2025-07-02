using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class Quotation
{
    public int QuotationId { get; set; }

    public int? CustomerId { get; set; }

    public int? TotalMoney { get; set; }

    public decimal? Vat { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string Note { get; set; }

    public bool? Confirmed { get; set; }

    public virtual Customer Customer { get; set; }

    public virtual ICollection<QuotationDetail> QuotationDetails { get; set; } = new List<QuotationDetail>();
}
