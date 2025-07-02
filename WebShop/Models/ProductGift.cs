using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class ProductGift
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Avatar { get; set; }

    public string Alias { get; set; }

    public int? Price { get; set; }

    public virtual ICollection<GitAttribute> GitAttributes { get; set; } = new List<GitAttribute>();
}
