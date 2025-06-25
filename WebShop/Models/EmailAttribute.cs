using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class EmailAttribute
{
    public int Id { get; set; }

    public int CustumerId { get; set; }

    public string Body { get; set; }

    public DateTime? TimeSend { get; set; }

    public virtual Customer Custumer { get; set; }
}
