using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class NguoiGiaoHang
{
    public int ShipperId { get; set; }

    public string ShipperName { get; set; }

    public string SoDienThoai { get; set; }

    public string Company { get; set; }

    public DateTime? ShipDate { get; set; }
}
