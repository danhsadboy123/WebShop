using Ecommerce_CaFeShop.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_CaFeShop
{
    public class SeedData
    {
        public static async Task SeedingData(CaFeContext _context)
        {
            //await _context.Database.MigrateAsync();
            //if (!_context.ThuongHieus.Any())
            //{
            //    ThuongHieu TrungNguyen = new ThuongHieu { TenThuongHieu = "Trung Nguyên", Slug = "trung-nguyen" };
            //    ThuongHieu HighlandsCoffee = new ThuongHieu { TenThuongHieu = "Highlands Coffee", Slug = "highlands-coffee" };
            //    ThuongHieu TheCoffeeHouse = new ThuongHieu { TenThuongHieu = "The Coffee House", Slug = "the-coffee-house" };
            //    ThuongHieu G7 = new ThuongHieu { TenThuongHieu = "G7", Slug = "g7" };

            //    await _context.ThuongHieus.AddRangeAsync(TrungNguyen, HighlandsCoffee, TheCoffeeHouse, G7);
            //    await _context.SaveChangesAsync();
            //}

            //if (!_context.DanhMucs.Any())
            //{
            //    DanhMuc cafeHatRang = new DanhMuc { TenDanhMuc = "Cà phê hạt rang", MaDanhMucCha = null, Slug = "ca-phe-hat-rang" };
            //    DanhMuc cafeXay = new DanhMuc { TenDanhMuc = "Cà phê xay", MaDanhMucCha = null, Slug = "ca-phe-xay" };
            //    DanhMuc cafePhinTruyenThong = new DanhMuc { TenDanhMuc = "Cà phê phin truyền thống", MaDanhMucCha = null, Slug = "ca-phe-phin-truyen-thong" };

            //    await _context.DanhMucs.AddRangeAsync(cafeHatRang, cafeXay, cafePhinTruyenThong);
            //    await _context.SaveChangesAsync();
            //}

            //if (!_context.SanPhams.Any())
            //{
            //    var cafeXay = _context.DanhMucs.FirstOrDefault(c => c.TenDanhMuc == "Cà phê xay");
            //    var cafeHatRang = _context.DanhMucs.FirstOrDefault(c => c.TenDanhMuc == "Cà phê hạt rang");
            //    var cafePhinTruyenThong = _context.DanhMucs.FirstOrDefault(c => c.TenDanhMuc == "Cà phê phin truyền thống");

            //    var TrungNguyen = _context.ThuongHieus.FirstOrDefault(b => b.TenThuongHieu == "Trung Nguyên");
            //    var HighlandsCoffee = _context.ThuongHieus.FirstOrDefault(b => b.TenThuongHieu == "Highlands Coffee");
            //    var TheCoffeeHouse = _context.ThuongHieus.FirstOrDefault(b => b.TenThuongHieu == "The Coffee House");
            //    var G7 = _context.ThuongHieus.FirstOrDefault(b => b.TenThuongHieu == "G7");

            //    await _context.SanPhams.AddRangeAsync(

            //    );
            //    await _context.SaveChangesAsync();

            //    await _context.HinhAnhSanPhams.AddRangeAsync(

            //    );
            //    await _context.SaveChangesAsync();
            //}

            //if (!_context.BaiViets.Any())
            //{
            //    await _context.BaiViets.AddRangeAsync(
            //        new BaiViet
            //        {
            //            HinhAnh = "hieubietvequytrinhrangcafe.jpg",
            //            TieuDe = "Hiểu Biết Về Quy Trình Rang Cà Phê",
            //            NoiDung = "Khám phá quy trình rang cà phê và những yếu tố tạo nên hương vị độc đáo của từng loại cà phê."
            //        },
            //        new BaiViet
            //        {
            //            HinhAnh = "meophacaphetainha.jpg",
            //            TieuDe = "Mẹo Pha Cà Phê Ngon Tại Nhà",
            //            NoiDung = "Học cách pha cà phê ngon tại nhà với các mẹo đơn giản từ chọn hạt, rang xay đến pha chế."
            //        },
            //        new BaiViet
            //        {
            //            HinhAnh = "Lscfvn.jpg",
            //            TieuDe = "Lịch Sử Cà Phê Việt Nam",
            //            NoiDung = "Tìm hiểu về hành trình của cà phê Việt Nam, từ khi du nhập đến vị thế hàng đầu thế giới."
            //        },
            //        new BaiViet
            //        {
            //            HinhAnh = "chonloaicfphuhop.jpg",
            //            TieuDe = "Chọn Loại Cà Phê Phù Hợp Với Bạn",
            //            NoiDung = "Bạn đang tìm kiếm loại cà phê phù hợp với khẩu vị? Hãy tham khảo những gợi ý dưới đây để chọn cà phê hoàn hảo."
            //        },
            //        new BaiViet
            //        {
            //            HinhAnh = "cacloaihatphobien.jpg",
            //            TieuDe = "Các Loại Cà Phê Hạt Rang Phổ Biến",
            //            NoiDung = "Tìm hiểu các loại cà phê hạt rang phổ biến như Robusta, Arabica, Moka và cách chọn lựa phù hợp."
            //        },
            //        new BaiViet
            //        {
            //            HinhAnh = "topcafe.jpg",
            //            TieuDe = "Top 10 Loại Cà Phê Ngon Nhất 2025",
            //            NoiDung = "Khám phá danh sách 10 loại cà phê ngon nhất trong năm 2025, từ hương vị truyền thống đến hiện đại."
            //        }
            //    );
            //    await _context.SaveChangesAsync();
            //}

            //if (!_context.HinhAnhBaiViets.Any())
            //{
            //    await _context.HinhAnhBaiViets.AddRangeAsync(
            //        new HinhAnhBaiViet { MaBaiViet = 1, NoiDung = "Hình ảnh quy trình rang cà phê", HinhAnh = "hieubietvequytrinhrangcafe.png.jpg" },
            //        new HinhAnhBaiViet { MaBaiViet = 1, NoiDung = "Hình ảnh hạt cà phê rang", HinhAnh = "cacloaihatphobien.jpg" },
            //        new HinhAnhBaiViet { MaBaiViet = 2, NoiDung = "Hình ảnh dụng cụ pha cà phê", HinhAnh = "dungcucafe.jpg" },
            //        new HinhAnhBaiViet { MaBaiViet = 2, NoiDung = "Hình ảnh quy trình pha phin", HinhAnh = "quytrinhphacafep.jpg" },
            //        new HinhAnhBaiViet { MaBaiViet = 3, NoiDung = "Hình ảnh lịch sử cà phê Việt", HinhAnh = "Lscfvn.jpg" },
            //        new HinhAnhBaiViet { MaBaiViet = 3, NoiDung = "Hình ảnh đồn điền cà phê", HinhAnh = "dondiencafe.png" },
            //        new HinhAnhBaiViet { MaBaiViet = 4, NoiDung = "Hình ảnh các loại cà phê", HinhAnh = "cacloaicafe.jpg" },
            //        new HinhAnhBaiViet { MaBaiViet = 4, NoiDung = "Hình ảnh cà phê cho từng khẩu vị", HinhAnh = "tungkhauvi.jpg" },
            //        new HinhAnhBaiViet { MaBaiViet = 5, NoiDung = "Hình ảnh cà phê hạt rang", HinhAnh = "cacloaihatphobien.jpg" },
            //        new HinhAnhBaiViet { MaBaiViet = 5, NoiDung = "Hình ảnh chi tiết hạt cà phê", HinhAnh = "cacloaihatphobien.jpg" },
            //        new HinhAnhBaiViet { MaBaiViet = 6, NoiDung = "Hình ảnh cà phê cao cấp", HinhAnh = "HighlandsCoffeePhinsuada.png" },
            //        new HinhAnhBaiViet { MaBaiViet = 6, NoiDung = "Hình ảnh cà phê nổi bật 2025", HinhAnh = "topcafe.jpg" }
            //    );
            //    await _context.SaveChangesAsync();
            //}

            if (!_context.Footers.Any())
            {
                await _context.Footers.AddRangeAsync(
                    new Footer
                    {
                        Logo = "Logo.png",
                        MoTa = "Coffeeshop không chỉ là nơi để mua sắm, mà còn là nơi để khám phá, tìm hiểu và đắm mình trong thế giới cà phê Việt Nam.",
                        DiaChi = "65 Đ. Huỳnh Thúc Kháng, Bến Nghé, Quận 1, Hồ Chí Minh",
                        Email = "contact@coffeeshop.com",
                        SoDienThoai = "0123456789",
                        FacebookUrl = "https://www.facebook.com/COFFEESHOP/",
                        TrangThai = true
                    }
                );
                await _context.SaveChangesAsync();
            }

            if (!_context.FooterLinks.Any())
            {
                await _context.FooterLinks.AddRangeAsync(
                    new FooterLink { TieuDe = "Giới Thiệu", Url = "/Home/Introduction", MaNhom = 1, ThuTuHienThi = 1, TrangThai = true },
                    new FooterLink { TieuDe = "Liên Hệ", Url = "/Home/Contact", MaNhom = 1, ThuTuHienThi = 2, TrangThai = true },
                    new FooterLink { TieuDe = "Tài Khoản Của Tôi", Url = "/Account/Index", MaNhom = 2, ThuTuHienThi = 1, TrangThai = true },
                    new FooterLink { TieuDe = "Yêu Thích", Url = "/Account/Favorite", MaNhom = 2, ThuTuHienThi = 2, TrangThai = true },
                    new FooterLink { TieuDe = "Lịch Sử Đơn Hàng", Url = "/Account/Order", MaNhom = 2, ThuTuHienThi = 3, TrangThai = true },
                    new FooterLink { TieuDe = "Cà Phê Hạt Rang", Url = "/ca-phe-hat-rang", MaNhom = 3, ThuTuHienThi = 1, TrangThai = true },
                    new FooterLink { TieuDe = "Cà Phê Xay", Url = "/ca-phe-xay", MaNhom = 3, ThuTuHienThi = 2, TrangThai = true },
                    new FooterLink { TieuDe = "Cà Phê Phin Truyền Thống", Url = "/ca-phe-phin-truyen-thong", MaNhom = 3, ThuTuHienThi = 3, TrangThai = true }
                );
                await _context.SaveChangesAsync();
            }

            //if (!_context.Sliders.Any())
            //{
            //    await _context.Sliders.AddRangeAsync(
            //    );
            //    await _context.SaveChangesAsync();
            //}

            //if (!_context.GioiThieus.Any())
            //{
            //    await _context.GioiThieus.AddRangeAsync(
            //        new GioiThieu
            //        {
            //            NoiDung = @"
            //             Coffeeshop không chỉ là nơi để mua sắm, mà còn là một nơi để khám phá, tìm hiểu và đắm mình trong thế giới cà phê Việt Nam.
            //            <br />
            //             Coffeeshop được xây dựng nhằm cung cấp cho khách hàng những sản phẩm cà phê chất lượng cao, chính gốc, 
            //            cam kết mang đến những ly cà phê hoàn hảo về cả hương vị lẫn trải nghiệm. 
            //            Đồng thời chúng tôi cũng hướng đến những trải nghiệm dễ dàng, an toàn và nhanh chóng khi mua sắm trực tuyến thông qua hệ thống hỗ trợ thanh toán và vận hành vững mạnh.
            //            ",
            //            DiaChi = "65 Đ. Huỳnh Thúc Kháng, Bến Nghé, Quận 1, Hồ Chí Minh",
            //            SoDienThoai = "0306221302",
            //            Email = "0306221302@caothang.edu.vn"
            //        }
            //    );
            //    await _context.SaveChangesAsync();
            //}

            //if (!_context.ChinhSachs.Any())
            //{
            //    await _context.ChinhSachs.AddRangeAsync(
            //        new ChinhSach
            //        {
            //            TieuDe = "Giao hàng nhanh",
            //            NoiDung = @"Chúng tôi cam kết cung cấp dịch vụ giao hàng nhanh chóng và đáng tin cậy. Đơn hàng của bạn sẽ được xử lý và giao trong vòng 1-2 ngày làm việc, tùy thuộc vào địa chỉ giao hàng. 
            //                    Đặc biệt, đối với các đơn hàng trong khu vực nội thành, chúng tôi sẽ giao trong ngày nếu đơn hàng được đặt trước 12h00. 
            //                    Mọi chi phí giao hàng sẽ được hiển thị rõ ràng khi bạn thanh toán, và miễn phí vận chuyển cho đơn hàng có giá trị từ 500,000 VNĐ trở lên. 
            //                    Chúng tôi luôn nỗ lực mang đến trải nghiệm giao hàng nhanh chóng, tiện lợi và không gây phiền phức cho khách hàng."
            //        },
            //        new ChinhSach
            //        {
            //            TieuDe = "Miễn phí giao hàng",
            //            NoiDung = @"Cửa hàng sẽ miễn phí giao hàng cho tất cả các đơn hàng trong phạm vi nội thành.
            //                    Đối với các đơn hàng ở phạm vi ngoài thành phố thì sẽ được tính phí vận chuyển.
            //                    Thời gian nhận hàng sẽ từ 1-5 ngày tùy vào địa điểm nhận hàng.
            //                    Cửa hàng sẽ lựa chọn đối tác vận chuyển uy tín để đảm bảo sản phẩm được giao đến khách hàng một cách an toàn và đúng thời gian.
            //                    Trong quá trình vận chuyển, nếu sản phẩm bị hư hỏng hoặc thất lạc, cửa hàng sẽ chịu trách nhiệm hoàn toàn và có thể gửi lại sản phẩm mới hoặc hoàn tiền cho khách hàng.
            //                    Chính sách miễn phí giao hàng có thể không áp dụng cho các khu vực vùng sâu, vùng xa hoặc quốc tế, và trong trường hợp này, khách hàng sẽ được thông báo rõ ràng về các chi phí phát sinh."
            //        },
            //        new ChinhSach
            //        {
            //            TieuDe = "Cam kết chất lượng",
            //            NoiDung = @"Cửa hàng cam kết tất cả cà phê bán ra đều là sản phẩm chất lượng cao, được chọn lọc từ các nguồn cung cấp uy tín.
            //                    Mỗi sản phẩm sẽ đi kèm với thông tin nguồn gốc rõ ràng và được đóng gói cẩn thận để bảo đảm chất lượng.
            //                    Nếu khách hàng chứng minh được sản phẩm không đạt chất lượng hoặc không đúng mô tả, cửa hàng cam kết hoàn trả toàn bộ số tiền đã thanh toán.
            //                    Cửa hàng cung cấp dịch vụ hậu mãi, bao gồm tư vấn cách bảo quản và pha chế cà phê để đạt hương vị tốt nhất.
            //                    Chính sách đổi trả linh hoạt được áp dụng nếu khách hàng phát hiện sản phẩm có lỗi từ nhà sản xuất."
            //        }
            //    );
            //    await _context.SaveChangesAsync();
            //}

            //if (!_context.VaiTros.Any())
            //{
            //    await _context.VaiTros.AddRangeAsync(
            //        new VaiTro { Loai = "User" },
            //        new VaiTro { Loai = "Admin" }
            //    );
            //    await _context.SaveChangesAsync();
            //}

            //if (!_context.TaiKhoans.Any())
            //{
            //    await _context.TaiKhoans.AddRangeAsync(
            //        new TaiKhoan { TenDangNhap = "admin", MatKhau = "admin", MaVaiTro = 2 },
            //        new TaiKhoan { TenDangNhap = "user1", MatKhau = "user1", MaVaiTro = 1 },
            //        new TaiKhoan { TenDangNhap = "user2", MatKhau = "user2", MaVaiTro = 1 },
            //        new TaiKhoan { TenDangNhap = "user3", MatKhau = "user3", MaVaiTro = 1 },
            //        new TaiKhoan { TenDangNhap = "user4", MatKhau = "user4", MaVaiTro = 1 },
            //        new TaiKhoan { TenDangNhap = "user5", MatKhau = "user5", MaVaiTro = 1 },
            //        new TaiKhoan { TenDangNhap = "user6", MatKhau = "user6", MaVaiTro = 1 },
            //        new TaiKhoan { TenDangNhap = "user7", MatKhau = "user7", MaVaiTro = 1 },
            //        new TaiKhoan { TenDangNhap = "user8", MatKhau = "user8", MaVaiTro = 1 },
            //        new TaiKhoan { TenDangNhap = "user9", MatKhau = "user9", MaVaiTro = 1 },
            //        new TaiKhoan { TenDangNhap = "user10", MatKhau = "user10", MaVaiTro = 1 }
            //    );
            //    await _context.SaveChangesAsync();
            //}

            //if (!_context.KhachHangs.Any())
            //{
            //    await _context.KhachHangs.AddRangeAsync(
            //    );
            //    await _context.SaveChangesAsync();
            //}

            //if (!_context.HoaDons.Any())
            //{
            //    await _context.HoaDons.AddRangeAsync(
            //    );
            //    await _context.SaveChangesAsync();
            //}

            //if (!_context.ChiTietHoaDons.Any())
            //{
            //    await _context.ChiTietHoaDons.AddRangeAsync(
            //        new ChiTietHoaDon { MaHoaDon = 1, MaSanPham = 1, Gia = 150000, SoLuong = 2, TongTien = 300000 },
            //        new ChiTietHoaDon { MaHoaDon = 1, MaSanPham = 7, Gia = 90000, SoLuong = 1, TongTien = 90000 },
            //        new ChiTietHoaDon { MaHoaDon = 2, MaSanPham = 3, Gia = 200000, SoLuong = 2, TongTien = 400000 },
            //        new ChiTietHoaDon { MaHoaDon = 2, MaSanPham = 8, Gia = 140000, SoLuong = 1, TongTien = 160000 },
            //        new ChiTietHoaDon { MaHoaDon = 3, MaSanPham = 2, Gia = 120000, SoLuong = 2, TongTien = 240000 },
            //        new ChiTietHoaDon { MaHoaDon = 3, MaSanPham = 10, Gia = 80000, SoLuong = 2, TongTien = 160000 },
            //        new ChiTietHoaDon { MaHoaDon = 4, MaSanPham = 5, Gia = 100000, SoLuong = 3, TongTien = 300000 },
            //        new ChiTietHoaDon { MaHoaDon = 4, MaSanPham = 9, Gia = 150000, SoLuong = 1, TongTien = 150000 },
            //        new ChiTietHoaDon { MaHoaDon = 5, MaSanPham = 4, Gia = 180000, SoLuong = 2, TongTien = 360000 },
            //        new ChiTietHoaDon { MaHoaDon = 5, MaSanPham = 6, Gia = 160000, SoLuong = 1, TongTien = 160000 },
            //        new ChiTietHoaDon { MaHoaDon = 6, MaSanPham = 7, Gia = 90000, SoLuong = 3, TongTien = 270000 },
            //        new ChiTietHoaDon { MaHoaDon = 6, MaSanPham = 1, Gia = 150000, SoLuong = 1, TongTien = 150000 },
            //        new ChiTietHoaDon { MaHoaDon = 7, MaSanPham = 3, Gia = 200000, SoLuong = 2, TongTien = 400000 },
            //        new ChiTietHoaDon { MaHoaDon = 7, MaSanPham = 8, Gia = 140000, SoLuong = 1, TongTien = 140000 },
            //        new ChiTietHoaDon { MaHoaDon = 8, MaSanPham = 2, Gia = 120000, SoLuong = 2, TongTien = 240000 },
            //        new ChiTietHoaDon { MaHoaDon = 9, MaSanPham = 9, Gia = 170000, SoLuong = 3, TongTien = 510000 },
            //        new ChiTietHoaDon { MaHoaDon = 9, MaSanPham = 10, Gia = 80000, SoLuong = 2, TongTien = 160000 },
            //        new ChiTietHoaDon { MaHoaDon = 10, MaSanPham = 7, Gia = 90000, SoLuong = 4, TongTien = 360000 }
            //    );
            //    await _context.SaveChangesAsync();
            //}
        }
    }
}