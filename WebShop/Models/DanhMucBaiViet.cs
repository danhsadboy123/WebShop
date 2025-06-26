using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class DanhMucBaiViet
{
    public int MaDanhMucBaiViet { get; set; }

    public string TenDanhMucBaiViet { get; set; }

    public DateTime? NgayTao { get; set; }

    public DateTime? NgayCapNhat { get; set; }

    public virtual ICollection<BaiViet> BaiViet { get; set; } = new List<BaiViet>();
}
