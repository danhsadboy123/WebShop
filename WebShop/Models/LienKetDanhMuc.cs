using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class LienKetDanhMuc
{
    public int Ma { get; set; }

    public int? MaDanhMuc { get; set; }

    public string LienKet { get; set; }

    public int? ThuTuDanhMuc { get; set; }
}
