using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class CateLink
{
    public int Id { get; set; }

    public int? CateId { get; set; }

    public string Link { get; set; }

    public int? OrderCate { get; set; }
}
