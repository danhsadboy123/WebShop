using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebShop.Models;

namespace WebShop.ModelViews
{
    public class MuaHangVM
    {
        public int maDonHang { get; set; }

        public int maKhachHang { get; set; }
        public int maKhachVangLai { get; set; }

        public bool daDangNhap { get; set; }

        [Required(ErrorMessage = "*Vui long nhap Ho va Ten")]
        [RegularExpression(@"^[\p{L}\s]+$", ErrorMessage = "Ten nguoi nhan khong hop le")]
        public string HoTen { get; set; }

        [Required(ErrorMessage = "*Vui long nhap email")]
        [EmailAddress(ErrorMessage = "*Dia chi email khong hop le")]
        public string Email { get; set; }

        [Required(ErrorMessage = "*Vui long nhap so dien thoai")]
        [RegularExpression(@"^(0[3|5|7|8|9])[0-9]{8}$", ErrorMessage = "So dien thoai khong hop le")]
        public string SoDienThoai { get; set; }

        public List<DiaChiTaiKhoan> danhSachDiaChi { get; set; }

        [Required(ErrorMessage = "*Vui long nhap dia chi nhan hang")]
        public string diaChi { get; set; }

        [Required(ErrorMessage = "*Vui long chon Tinh/Thanh")]
        public int tinhThanh { get; set; }

        [Required(ErrorMessage = "*Vui long chon Quan/Huyen")]
        public int quanHuyen { get; set; }

        [Required(ErrorMessage = "*Vui long chon Phuong/Xa")]
        public int phuongXa { get; set; }

        public int maPhuongThucThanhToan { get; set; }

        public string tenCongTy { get; set; }
        public string maSoCongTy { get; set; }
        public string diaChiCongTy { get; set; }
        public string ghiChu { get; set; }
        public bool kiemTraEmail { get; set; }
        public string maCode { get; set; }
        public string ghiChuThanhToan { get; set; }
    }
}