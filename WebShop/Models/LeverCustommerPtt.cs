using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class LeverCustommerPtt
{
    public int Id { get; set; }

    public string NameLever { get; set; }

    public virtual ICollection<CustomerPotentail> CustomerPotentails { get; set; } = new List<CustomerPotentail>();
}
