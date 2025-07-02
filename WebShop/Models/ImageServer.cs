using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class ImageServer
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string NameLink { get; set; }

    public string DateCreate { get; set; }

    public string DateUpdate { get; set; }
}
