using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class DiaChiGiaoHang
{
    public int ShippingAdressId { get; set; }

    public int? OrderId { get; set; }

    public string Name { get; set; }

    public string SoDienThoai { get; set; }

    public int? ProvinceId { get; set; }

    public int? DistrictId { get; set; }

    public int? WardId { get; set; }

    public string Address { get; set; }

    public virtual Huyen District { get; set; }

    public virtual DonHang Order { get; set; }

    public virtual Tinh Province { get; set; }

    public virtual Xa Ward { get; set; }
}
