using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class DanhMucSanPham
{
    public int ProductCatId { get; set; }

    public int? CatId { get; set; }

    public int? MaSanPham { get; set; }

    public virtual DanhMuc Cat { get; set; }

    public virtual SanPham SanPham { get; set; }
   

}
