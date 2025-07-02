using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class Banner
{
    public int Id { get; set; }

    public int CatId { get; set; }

    public string Banner1 { get; set; }

    public bool? Status { get; set; }

    public virtual Category Cat { get; set; }
}
