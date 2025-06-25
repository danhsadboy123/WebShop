using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class Video
{
    public int Id { get; set; }

    public int BrandId { get; set; }

    public string Image { get; set; }

    public string Video1 { get; set; }

    public bool? Show { get; set; }

    public int? Sort { get; set; }

    public virtual Brand Brand { get; set; }
}
