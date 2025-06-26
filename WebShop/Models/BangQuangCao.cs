using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class BangQuangCao
{
    public int Ma { get; set; }

    public int MaDanhMuc { get; set; }

    public string HinhAnh { get; set; }

    public bool? TrangThai { get; set; }

    public virtual DanhMuc DanhMuc { get; set; }
}
