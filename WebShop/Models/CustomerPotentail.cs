using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class CustomerPotentail
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string CompannyName { get; set; }

    public string Email { get; set; }

    public string Address { get; set; }

    public int? Phone { get; set; }

    public int? Checked { get; set; }

    public int? LeverId { get; set; }

    public virtual ICollection<AttributesPotentail> AttributesPotentails { get; set; } = new List<AttributesPotentail>();

    public virtual LeverCustommerPtt Lever { get; set; }
}
