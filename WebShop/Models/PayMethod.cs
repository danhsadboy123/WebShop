using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class PayMethod
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string PayContent { get; set; }
}
