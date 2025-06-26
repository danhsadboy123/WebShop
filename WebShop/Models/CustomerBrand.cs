using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class CustomerBrand
{
    public int Id { get; set; }

    public string Image { get; set; }

    public string Address { get; set; }

    public string MoTa { get; set; }

    public DateTime? YearOn { get; set; }

    public string YearOff { get; set; }

    public string Link { get; set; }
}
