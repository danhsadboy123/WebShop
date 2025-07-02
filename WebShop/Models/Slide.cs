using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class Slide
{
    public int SlideId { get; set; }

    public int? CatId { get; set; }

    public string Thumb { get; set; }

    public string Alias { get; set; }

    public string SlideName { get; set; }

    public bool IsActivated { get; set; }

    public bool Right { get; set; }

    public bool Bottom { get; set; }

    public bool HomeFlag { get; set; }

    public int? Ordering { get; set; }

    public virtual Category Cat { get; set; }
}
