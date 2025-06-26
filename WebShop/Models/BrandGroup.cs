using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class BrandGroup
{
    public int Id { get; set; }

    public int? BrandId { get; set; }

    public int? CatId { get; set; }

    public string Name { get; set; }

    public string MoTa { get; set; }

    public bool? Status { get; set; }

    public virtual Brand Brand { get; set; }
}
