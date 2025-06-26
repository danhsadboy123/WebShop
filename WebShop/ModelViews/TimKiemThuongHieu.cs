using System;
using System.Collections.Generic;
using WebShop.Models;

namespace WebShop.ModelViews
{
    public class TimKiemThuongHieu
    {     
        public DanhMuc categories { get; set; }
        public List<ThuongHieu> brand { get; set; } = new List<ThuongHieu>(); // Đảm bảo khởi tạo danh sách
        public string sort { get; set; }  
    }
}
