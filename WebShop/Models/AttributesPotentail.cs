using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class AttributesPotentail
{
    public int Id { get; set; }

    public int? PotentailId { get; set; }

    public string Body { get; set; }

    public DateTime? TimeSend { get; set; }

    public virtual CustomerPotentail Potentail { get; set; }
}
