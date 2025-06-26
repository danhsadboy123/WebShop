using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class ProductCategory
{
    public int ProductCatId { get; set; }

    public int? CatId { get; set; }

    public int? ProductId { get; set; }

    public virtual DanhMuc Cat { get; set; }

    public virtual Product Product { get; set; }
   

}
