using System;
using System.Collections.Generic;
using WebShop.Models;

namespace WebShop.ModelViews
{
    public class TimKiemAlias
    {
        public List<GiaThuocTinh> attrp { get; set; } = new List<GiaThuocTinh>(); // Khởi tạo danh sách
        public List<ThuongHieu> brand { get; set; } = new List<ThuongHieu>(); // Khởi tạo danh sách
        public NhomThuongHieu brandGroup { get; set; }
        public string Alias { get; set; }
        public string sort { get; set; }
        public int top { get; set; }
    }

}
