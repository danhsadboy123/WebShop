using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class CustomerSupplier
{
    public int Id { get; set; }

    public string Image { get; set; }

    public string Name { get; set; }

    public string Company { get; set; }

    public int? Phone { get; set; }

    public string Email { get; set; }

    public string Address { get; set; }

    public DateTime? YearAdd { get; set; }

    public int? Lever { get; set; }

    public virtual ICollection<ProductAddCusPro> ProductAddCusPros { get; set; } = new List<ProductAddCusPro>();
}
