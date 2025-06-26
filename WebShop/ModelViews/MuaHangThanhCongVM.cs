using System;
using System.Collections.Generic;
using WebShop.Models;

namespace WebShop.ModelViews
{
    public class MuaHangThanhCongVM
    {
        public int DonHangID { get; set; }         // Mã đơn hàng
        public DonHang Order { get; set; }           // Thông tin đơn hàng
        public string HoTen { get; set; }       // Họ tên khách hàng
        public string SoDienThoai { get; set; }          // Số điện thoại khách hàng
        public string Address { get; set; }        // Địa chỉ giao hàng
        public string PhuongXa { get; set; }       // Phường/Xã
        public string QuanHuyen { get; set; }      // Quận/Huyện
        public string TinhThanh { get; set; }      // Tỉnh/Thành phố
        public string Email { get; set; }          // Email khách hàng

        // Thêm danh sách sản phẩm trong đơn hàng thay vì chỉ một sản phẩm
        public List<OrderDetail> OrderDetails { get; set; } // Danh sách sản phẩm trong đơn hàng
        public Product product { get; set; }
    }

}
 
