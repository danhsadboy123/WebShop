using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class ProductAddCusPro
{
    public int Id { get; set; }

    public int? CustomerId { get; set; }

    public int? ProductId { get; set; }

    public int? Stock { get; set; }

    public string MoTa { get; set; }

    public string Address { get; set; }

    public virtual NhaCungCapKhachHang Customer { get; set; }

    public virtual Product Product { get; set; }
}
