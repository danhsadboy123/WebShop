using Ecommerce_WatchShop.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_WatchShop
{
    public class SeedData
    {
        public static async Task SeedingData(DongHoContext _context)
        {
            await _context.Database.MigrateAsync();
            if (!_context.ThuongHieus.Any())
            {
                ThuongHieu citizen = new ThuongHieu { TenThuongHieu = "Citizen", Slug = "citizen" };
                ThuongHieu doxa = new ThuongHieu { TenThuongHieu = "Doxa", Slug = "doxa" };
                ThuongHieu curnon = new ThuongHieu { TenThuongHieu = "Curnon", Slug = "curnon" };
                ThuongHieu seiko = new ThuongHieu { TenThuongHieu = "Seiko",  Slug = "seiko" };

                await _context.ThuongHieus.AddRangeAsync(citizen, doxa, curnon, seiko);
                await _context.SaveChangesAsync();

            }
            if (!_context.DanhMucs.Any())
            {

                DanhMuc donghoco = new DanhMuc { TenDanhMuc = "Đồng hồ cơ", MaDanhMucCha = null, Slug = "dong-ho-co" };
                DanhMuc donghopin = new DanhMuc { TenDanhMuc = "Đồng hồ pin", MaDanhMucCha = null, Slug = "dong-ho-pin" };
                DanhMuc donghonangluong = new DanhMuc { TenDanhMuc = "Đồng hồ năng lượng mặt trời", MaDanhMucCha = null, Slug = "dong-ho-nang-luong-mat-troi" };

                await _context.DanhMucs.AddRangeAsync(donghoco, donghopin, donghonangluong);
                await _context.SaveChangesAsync();
            }
            //if (!_context.Suppliers.Any())
            //{
            //    Supplier citizen_supplier = new Supplier { Name = "Công ty Citizen Watch", SoDienThoai = "(800) 321-1023", Information = "CÔNG TY CITIZEN WATCH là một nhà sản xuất thực sự với một quy trình sản xuất toàn diện", DiaChi = "6-1-12, Tanashi-cho, Nishi-Tokyo-shi, Tokyo 188-8511, Japan" };
            //    Supplier doxa_supplier = new Supplier { Name = "Công ty Doxa", SoDienThoai = "1-520-369 -872", Information = "Thương hiệu đồng hồ Doxa nổi tiếng của Thuỵ Sĩ được ra mắt với công chúng vào năm 1889 bởi một nghệ nhân chế tác đồng hồ trẻ tuổi", DiaChi = "Rue de Zurich 23A, 2500 Biel/Bienne, Switzerland" };
            //    Supplier curnon_supplier = new Supplier { Name = "Công ty Curnon", SoDienThoai = "0868889103", Information = "Với những sản phẩm được thiết kế bằng nhiệt huyết, khát khao và khối óc đầy sáng tạo của đội ngũ chính những người trẻ Việt Nam.", DiaChi = "25 Nguyễn Trãi, P.Bến Thành, Quận 1." };
            //    Supplier seiko_supplier = new Supplier { Name = "Công ty Seiko", SoDienThoai = "81-3-3563-2111", Information = "Công ty Nhật Bản thành lập vào năm 1881; nổi tiếng trong lĩnh vực sản xuất và mua bán đồng hồ, thiết bị điện tử", DiaChi = "1-8 Nakase, Mihama-ku, Chiba-shi, Chiba 261-8507, Japan" };

            //    await _context.Suppliers.AddRangeAsync(citizen_supplier, doxa_supplier, curnon_supplier, seiko_supplier);
            //    await _context.SaveChangesAsync();
            //}
            if (!_context.SanPhams.Any())
            {
                var donghopin = _context.DanhMucs.FirstOrDefault(c => c.TenDanhMuc == "Đồng hồ pin");
                var donghoco = _context.DanhMucs.FirstOrDefault(c => c.TenDanhMuc == "Đồng hồ cơ");
                var donghonangluong = _context.DanhMucs.FirstOrDefault(c => c.TenDanhMuc == "Đồng hồ năng lượng mặt trời");

                var citizen = _context.ThuongHieus.FirstOrDefault(b => b.TenThuongHieu == "Citizen");
                var doxa = _context.ThuongHieus.FirstOrDefault(b => b.TenThuongHieu == "Doxa");
                var curnon = _context.ThuongHieus.FirstOrDefault(b => b.TenThuongHieu == "Curnon");
                var seiko = _context.ThuongHieus.FirstOrDefault(b => b.TenThuongHieu == "Seiko");

                //var citizen_supplier = _context.Suppliers.FirstOrDefault(s => s.Name == "Công ty Citizen Watch");
                //var doxa_supplier = _context.Suppliers.FirstOrDefault(s => s.Name == "Công ty Doxa");
                //var curnon_supplier = _context.Suppliers.FirstOrDefault(s => s.Name == "Công ty Curnon");
                //var seiko_supplier = _context.Suppliers.FirstOrDefault(s => s.Name == "Công ty Seiko");

                await _context.SanPhams.AddRangeAsync(
                    new SanPham
                    {
                        HinhAnh = "Curnon Kashmir.png",
                        TenSanPham = "Curnon Kashmir",
                        MaDanhMuc = donghoco.MaDanhMuc,
                        MaThuongHieu = curnon.MaThuongHieu,
                        GioiTinh = 1,
                        Gia = 2279000,
                        MoTaNgan = "Đồng hồ sang trọng dành cho nam",
                        MoTa = "Đồng hồ nam Curnon Kashmir Classic có thiết kế tối giản, mang phong cách trẻ trung; Dây da, có kim rốn, Mặt kính Sapphire chống trầy xước, Chống nước 3ATM…",
                        ThongSoKyThuat = "Kích thước mặt: 40mm<br> Độ dày: 7mm<br> Màu mặt: White<br> Loại máy: MIYOTA QUARTZ<br> Kích cỡ dây: 20mm<br>Chống nước: 3ATM<br> Mặt kính: Sapphire<br> Chất liệu dây: Dây Da Genuine",
                        SoLuong = 10,
                        TrangThai = 1,
                        LuotXem = 1000,
                        NgayTao = DateTime.Now,
                        NgayCapNhat = null,
                        DaXoa = 0,
                        Slug = "curnon-kashmir"
                    },
                    new SanPham
                    {
                        HinhAnh = "Citizen-BI5104-57e.png",
                        TenSanPham = "Citizen BI5104 57e",
                        MaDanhMuc = donghopin.MaDanhMuc,
                        MaThuongHieu = citizen.MaThuongHieu,
                        GioiTinh = 1,
                        Gia = 5280000,
                        MoTaNgan = "Citizen BI5104-57E – Nam – Quartz (Pin) – Mặt Số 41mm, Kính Cứng, Chống Nước 5ATM",
                        MoTa = "Citizen BI5104-57E gây ấn tượng bởi cấu trúc Cushion Lug (vấu đệm) mang đến phong cách thể thao sang trọng. Bộ máy thạch anh in-house đảm bảo thời gian luôn hiển thị chính xác trong khoảng +/- 15 giây mỗi tháng.",
                        ThongSoKyThuat = "<b>Đường kính mặt số: </b>41 mm<br><b>Bề dày mặt số: </b>11 mm<br>Niềng: Thép không gỉ<br>Dây đeo: Thép dáng Oyster",
                        SoLuong = 12,
                        TrangThai = 1,
                        LuotXem = 50,
                        NgayTao = DateTime.Now,
                        NgayCapNhat = null,
                        DaXoa = 0,
                        Slug = "citizen-bi5104-57e"
                    },
                    new SanPham
                    {
                        HinhAnh = "Citizen-Tsuyosa.png",
                        TenSanPham = "Citizen Tsuyosa",
                        MaDanhMuc = donghoco.MaDanhMuc,
                        MaThuongHieu = citizen.MaThuongHieu,
                        GioiTinh = 1,
                        Gia = 12485000,
                        MoTaNgan = "Citizen Tsuyosa NJ0151-88M – Nam – Kính Sapphire – Mặt Số 40mm",
                        MoTa = "Citizen Tsuyosa NJ0151-88M mang đến hơi thở tươi mới từ đại dương, theo đuổi phong cách năng động, trẻ trung, kích thước mặt số 40mm phù hợp đa số với quý ông.",
                        ThongSoKyThuat = "Thương hiệu: Citizen, Bộ sưu tập: Citizen Tsuyosa, Xuất xứ: Nhật Bản, Kính: Sapphire (Kính chống trầy), Máy: Caliber 8210 Automatic (Cơ tự động), Đường kính mặt số: 40 mm, Bề dày mặt số: 11.7 mm, Niềng: Thép không gỉ, Dây đeo: Thép không gỉ, Chống nước: 5 ATM.",
                        SoLuong = 6,
                        TrangThai = 1,
                        LuotXem = 100,
                        NgayTao = DateTime.Now,
                        NgayCapNhat = null,
                        DaXoa = 0,
                        Slug = "citizen-tsuyosa"
                    },
                    new SanPham
                    {
                        HinhAnh = "Citizen-NH9130-84L.png",
                        TenSanPham = "Citizen NH9130 84L",
                        MaDanhMuc = donghoco.MaDanhMuc,
                        MaThuongHieu = citizen.MaThuongHieu,
                        GioiTinh = 2,
                        Gia = 10085000,
                        MoTaNgan = "Citizen NH9130-84L – Nam – Kính Sapphire – Automatic – Trữ Cót 40 Giờ, Họa Tiết Guilloche, Open Heart",
                        MoTa = "Citizen Automatic NH9130-84L thiết kế Open heart cùng họa tiết Guilloche hoàn toàn mới mang đến diện mạo nam tính, lịch lãm. Trang bị bộ máy cơ Japan Movt trữ cót 40 giờ, tự động lên cót khi đeo liên tục mỗi ngày.",
                        ThongSoKyThuat = "Kính: Sapphire (Kính chống trầy), Máy: Automatic (Miyota 8229 trữ cót 40 giờ), Đường kính mặt số: 40 mm, Bề dày mặt số: 10.7 mm, Niềng: Thép không gỉ, Dây Đeo: Thép không gỉ, Chống nước: 5 ATM,Màu mặt số: Xanh dương.",
                        SoLuong = 8,
                        TrangThai = 1,
                        LuotXem = 2000,
                        NgayTao = DateTime.Now,
                        NgayCapNhat = null,
                        DaXoa = 0,
                        Slug = "citizen-nh9130-84l"
                    },
                    new SanPham
                    {
                        HinhAnh = "Citizen-Eco-Drive.png",
                        TenSanPham = "Citizen Eco Drive",
                        MaDanhMuc = donghopin.MaDanhMuc,
                        MaThuongHieu = citizen.MaThuongHieu,
                        GioiTinh = 0,
                        Gia = 7585000,
                        MoTaNgan = "Citizen Eco-Drive EM0506-77A – Nữ – Kính Cứng – Eco-Drive (Năng Lượng Ánh Sáng) – Mặt Số 32mm",
                        MoTa = "Mẫu Citizen Eco-Drive EM0506-77A phiên bản dây đeo tone màu vàng demi, nền mặt số xà cừ với họa tiết Guilloche thẩm mỹ. Mặt số 32mm với trọng lượng vừa phải phù hợp với nữ giới, sử dụng năng lượng mặt trời có tuổi thọ dài giúp tiết kiệm chi phí, cực kỳ trang nhã và thanh lịch.",
                        ThongSoKyThuat = "Kính: Mineral Crystal (Kính cứng), Máy: Eco-Drive (Năng lượng ánh sáng), Đường Kính Mặt Số: 32 mm, Bề Dày Mặt Số: 7.6 mm, Niềng: Thép không gỉ, Dây Đeo: Thép không gỉ, Chống Nước: 5 ATM.",
                        SoLuong = 11,
                        TrangThai = 2,
                        LuotXem = 500,
                        NgayTao = DateTime.Now,
                        NgayCapNhat = null,
                        DaXoa = 0,
                        Slug = "citizen-eco-drive"
                    },
                    new SanPham
                    {
                        HinhAnh = "Citizen-EM0863-53D.png",
                        TenSanPham = "Citizen EM0863-53D",
                        MaDanhMuc = donghonangluong.MaDanhMuc,
                        MaThuongHieu = citizen.MaThuongHieu,
                        GioiTinh = 0,
                        Gia = 12685000,
                        MoTaNgan = "Citizen EM0863-53D – Nữ – Eco-Drive (Năng Lượng Ánh Sáng) – Mặt Số 25mm, Kính Cứng, Chống Nước 5ATM",
                        MoTa = "Citizen Silhouette Crystal EM0863-53D thiết kế mạ vàng PVD sang trọng kết hợp những viên đá pha lê tuyển chọn có độ tán sắc cao, lấp lánh thu hút ánh nhìn. Trang bị bộ máy Eco-Drive hoạt động cực kỳ chính xác mà không phải thay pin thường xuyên.",
                        ThongSoKyThuat = "Kính: Mineral Crystal (Kính cứng), Máy: Eco-Drive (Năng lượng ánh sáng), Đường kính mặt số: 25 mm, Bề dày mặt số: 7.3 mm, Niềng: Thép không gỉ, Dây đeo: Thép không gỉ, Màu mặt số: Trắng xà cừ, Chống nước: 5 ATM.",
                        SoLuong = 17,
                        TrangThai = 2,
                        LuotXem = 250,
                        NgayTao = DateTime.Now,
                        NgayCapNhat = null,
                        DaXoa = 0,
                        Slug = "citizen-em0863-53d"
                    },
                    new SanPham
                    {
                        HinhAnh = "Doxa-Executive-Slim.png",
                        TenSanPham = "Doxa Executive Slim",
                        MaDanhMuc = donghopin.MaDanhMuc,
                        MaThuongHieu = doxa.MaThuongHieu,
                        GioiTinh = 1,
                        Gia = 23250000,
                        MoTaNgan = "Doxa Executive Slim D201RSV – Nam – Kính Sapphire – Quartz (Pin) – Mặt Số 40mm, Swiss Made, Chống Nước 5ATM",
                        MoTa = "Mẫu Doxa D201RSV vẻ ngoài sang trọng với mẫu vạch số tạo hình mỏng mang lại sự tinh tế dành cho phái mạnh đầy nổi bật khi các chi tiết kim chỉ được phủ tông vàng hồng trẻ trung đầy cuốn hút.",
                        ThongSoKyThuat = "Kính: Sapphire (Kính chống trầy),Máy: Quartz (Pin), Đường kính mặt số: 40 mm, Bề dày mặt số: 6.7 mm, Niềng: Thép không gỉ, Dây đeo: Thép không gỉ, Màu mặt số: Trắng, Chống nước: 5 ATM.",
                        SoLuong = 20,
                        TrangThai = 2,
                        LuotXem = 5000,
                        NgayTao = DateTime.Now,
                        NgayCapNhat = null,
                        DaXoa = 0,
                        Slug = "doxa-executive-slim"
                    },
                    new SanPham
                    {
                        HinhAnh = "Doxa-x-Dorian-Ho-Earlymoon.png",
                        TenSanPham = "Doxa x Dorian Ho Earlymoon",
                        MaDanhMuc = donghopin.MaDanhMuc,
                        MaThuongHieu = doxa.MaThuongHieu,
                        GioiTinh = 1,
                        Gia = 2290000,
                        MoTaNgan = "Doxa x Dorian Ho Earlymoon D226RGY – Nam – Kính Sapphire – Quartz (Pin) – Mặt số trẻ trung cùng giờ thế giới tiện dụng – Dây vải Nato bền bỉ mạnh mẽ",
                        MoTa = "Mẫu Doxa D226RGY phiên bản dây vải Nato tone màu xám đen, kết hợp vỏ kim loại mạ vàng hồng, cùng tính năng GMT tiện dụng, tạo nên vẻ ngoài thời trang năng động cho các chàng trong mọi tình huống.",
                        ThongSoKyThuat = "Kính: Sapphire (Kính Chống Trầy),Máy: Quartz (Pin), Đường Kính Mặt Số: 42 mm, Bề Dày Mặt Số: 12 mm",
                        SoLuong = 9,
                        TrangThai = 3,
                        LuotXem = 4000,
                        NgayTao = DateTime.Now,
                        NgayCapNhat = null,
                        DaXoa = 0,
                        Slug = "doxa-x-dorian-ho-earlymoon"
                    },
                    new SanPham
                    {
                        HinhAnh = "Doxa-Noble.png",
                        TenSanPham = "Doxa Noble",
                        MaDanhMuc = donghopin.MaDanhMuc,
                        MaThuongHieu = doxa.MaThuongHieu,
                        GioiTinh = 0,
                        Gia = 25030000,
                        MoTaNgan = "Doxa Noble D132TWH – Nữ – Kính Sapphire – Quartz (Pin) – Mặt số Rococo cùng 8 viên kim cương tự nhiên – Họa tiết Guilloche phong cách Byzantine",
                        MoTa = "Mẫu Doxa Noble D132TWH có thiết kế tinh xảo với họa tiết Guilloché, đính 8 viên kim cương, cùng bộ máy Swiss Made, hứa hẹn mang đên phong thái tự tin và sang trọng cho quý cô.",
                        ThongSoKyThuat = "Kính: Sapphire (Kính chống trầy), Máy: Quartz (Pin), Đường kính mặt số: 29 mm, Niềng: Thép không gỉ, Dây đeo: Thép không gỉ, Màu mặt số: Trắng, Chống nước: 5 ATM",
                        SoLuong = 22,
                        TrangThai = 3,
                        LuotXem = 150,
                        NgayTao = DateTime.Now,
                        NgayCapNhat = null,
                        DaXoa = 0,
                        Slug = "doxa-noble"
                    },
                    new SanPham
                    {
                        HinhAnh = "Seiko-SSC943P1.png",
                        TenSanPham = "Seiko Prospex Speedtimer SSC943P1",
                        MaDanhMuc = donghonangluong.MaDanhMuc,
                        MaThuongHieu = seiko.MaThuongHieu,
                        GioiTinh = 1,
                        Gia = 23600000,
                        MoTaNgan = "Seiko Prospex Speedtimer SSC943P1 là mẫu đồng hồ thể thao sang trọng với chức năng: Chronograph – Tachymeter – Lịch ngày – Kim xăng báo năng lượng còn lại.",
                        MoTa = "Seiko Prospex Speedtimer SSC943P1 là mẫu đồng hồ bấm giờ thể thao sang trọng, sử dụng pin năng lượng ánh sáng. Thuộc BST Seiko Prospex Speedtimer ra mắt lần đầu tiên năm 1969 – Kỷ nguyên của thời trang, âm nhạc và mô tô thể thao.",
                        ThongSoKyThuat = @"
                            <p><strong>Thương Hiệu:</strong> Seiko</p>
                            <p><strong>Số Hiệu Sản Phẩm:</strong> SSC943P1</p>
                            <p><strong>Bộ sưu tập:</strong>Seiko Prospex</a></p>
                            <p><strong>Xuất Xứ:</strong> Nhật Bản</p>
                            <p><strong>Giới Tính:</strong> Nam</p>
                            <p><strong>Kính:</strong> <strong>Kính: </strong>Sapphire (Phủ AR chống chói)</p>
                            <p><strong>Máy:</strong> Solar (Năng Lượng Ánh Sáng) – Caliber V192</p>
                            <p><strong>Bảo Hành Quốc Tế:</strong> 3 năm</p>
                            <p><strong>Bảo Hành Tại Hải Triều:</strong> 5 Năm</p>
                            <p><strong>Đường Kính Mặt Số:</strong> 41.4 mm</p>
                            <p><strong>Bề Dày Mặt Số:</strong> 13 mm</p>
                            <p><strong>Niềng:</strong> Thép không gỉ</p>
                            <p><strong>Dây Đeo:</strong> Dây da chính hãng</p>
                            <p><strong>Màu Mặt Số:</strong> Vàng Champagne</p>
                            <p><strong>Chống Nước: </strong>10 ATM</p>
                        ",
                        SoLuong = 10,
                        TrangThai = 3,
                        LuotXem = 999,
                        NgayTao = DateTime.Now,
                        NgayCapNhat = null,
                        DaXoa = 0,
                        Slug = "seiko-prospex-speedtimer-ssc943p1"

                    }
                );
                await _context.SaveChangesAsync();

                await _context.HinhAnhSanPhams.AddRangeAsync(
                    new HinhAnhSanPham { MaSanPham = 1, HinhAnh = "Curnon_Kashmir_Silver___Abyss-removebg-preview.jpg" },
                    new HinhAnhSanPham { MaSanPham = 1, HinhAnh = "Curnon Kashmir.png" },
                    new HinhAnhSanPham { MaSanPham = 2, HinhAnh = "Citizen-BI5104-57E-2.png" },
                    new HinhAnhSanPham { MaSanPham = 2, HinhAnh = "Citizen-BI5104-57E-3.png" },
                    new HinhAnhSanPham { MaSanPham = 2, HinhAnh = "Citizen-Box1-2.png" },
                    new HinhAnhSanPham { MaSanPham = 3, HinhAnh = "Citizen-Tsuyosa-3.png" },
                    new HinhAnhSanPham { MaSanPham = 3, HinhAnh = "Citizen-Tsuyosa-2.png" },
                    new HinhAnhSanPham { MaSanPham = 4, HinhAnh = "Citizen-NH9130-84L-2.png" },
                    new HinhAnhSanPham { MaSanPham = 4, HinhAnh = "Citizen-NH9130-84L-3.png" },
                    new HinhAnhSanPham { MaSanPham = 5, HinhAnh = "Citizen-Eco-Drive-2.png" },
                    new HinhAnhSanPham { MaSanPham = 5, HinhAnh = "Citizen-Eco-Drive-3.png" },
                    new HinhAnhSanPham { MaSanPham = 5, HinhAnh = "Citizen-Eco-Drive-4.png" },
                    new HinhAnhSanPham { MaSanPham = 6, HinhAnh = "Citizen-EM0863-53D-2.png" },
                    new HinhAnhSanPham { MaSanPham = 6, HinhAnh = "Citizen-EM0863-53D-3.png" },
                    new HinhAnhSanPham { MaSanPham = 6, HinhAnh = "Citizen-EM0863-53D-4.png" },
                    new HinhAnhSanPham { MaSanPham = 7, HinhAnh = "Doxa-Executive-Slim-2.png" },
                    new HinhAnhSanPham { MaSanPham = 7, HinhAnh = "Doxa-Executive-Slim-3.png" },
                    new HinhAnhSanPham { MaSanPham = 8, HinhAnh = "Doxa-x-Dorian-Ho-Earlymoon.png" },
                    new HinhAnhSanPham { MaSanPham = 8, HinhAnh = "Doxa-x-Dorian-Ho-Earlymoon-2.png" },
                    new HinhAnhSanPham { MaSanPham = 9, HinhAnh = "Doxa-Noble.png" },
                    new HinhAnhSanPham { MaSanPham = 9, HinhAnh = "Doxa-Box-2.png" },
                    new HinhAnhSanPham { MaSanPham = 10, HinhAnh = "Hop-Seiko.png" },
                    new HinhAnhSanPham { MaSanPham = 10, HinhAnh = "Seiko-SSC943P1-2.png" },
                    new HinhAnhSanPham { MaSanPham = 10, HinhAnh = "Seiko-SSC943P1-3.png" }
                );
                await _context.SaveChangesAsync();
            }
            if (!_context.BaiViets.Any())
            {
                await _context.BaiViets.AddRangeAsync(
                new BaiViet
                {
                    HinhAnh = "Blog_1.jpg",
                    TieuDe = "Hiểu Về Chuyển Động Của Đồng Hồ Cơ",
                    NoiDung = "Khám phá thế giới phức tạp của chuyển động cơ học và tìm hiểu điều gì làm nên sự khác biệt của chiếc đồng hồ của bạn."
                },
                new BaiViet
                {
                    HinhAnh = "Blog_Meo.jpg",
                    TieuDe = "Mẹo chăm sóc đồng hồ",
                    NoiDung = "Học cách chăm sóc đồng hồ của bạn để nó luôn hoạt động chính xác và bền lâu."
                },
                new BaiViet
                {
                    HinhAnh = "Blog_YN.jpg",
                    TieuDe = "Ý nghĩa của những chiếc đồng hồ đeo tay",
                    NoiDung = "Đồng hồ đeo tay là một vật dụng vô cùng quen thuộc với cả phái nam lẫn nữ, dù ở bất kì độ tuổi nào."
                },
                new BaiViet
                {
                    HinhAnh = "Blog_PC.jpg",
                    TieuDe = "Chọn Đồng Hồ Phù Hợp Với Phong Cách Của Bạn",
                    NoiDung = "Bạn đang tìm kiếm chiếc đồng hồ phù hợp với phong cách cá nhân? Hãy tham khảo những gợi ý dưới đây để chọn một chiếc đồng hồ hoàn hảo cho bạn."
                },
                new BaiViet
                {
                    HinhAnh = "Blog_Hublot.jpg",
                    TieuDe = "Các Loại Đồng Hồ Cơ Và Cách Chọn Lựa",
                    NoiDung = "Đồng hồ cơ được chia thành nhiều loại khác nhau. Hãy tìm hiểu các loại đồng hồ cơ phổ biến và cách chọn lựa một chiếc đồng hồ cơ phù hợp với nhu cầu của bạn."
                },
                new BaiViet
                {
                    HinhAnh = "Blog_Co.jpg",
                    TieuDe = "Top 10 Đồng Hồ Nam Chất Lượng Nhất 2025",
                    NoiDung = "Khám phá danh sách top 10 mẫu đồng hồ nam chất lượng nhất trong năm 2025. Những chiếc đồng hồ này không chỉ đẹp mắt mà còn sở hữu tính năng vượt trội."
                }
                );
                await _context.SaveChangesAsync();
            }
            if (!_context.HinhAnhBaiViets.Any())
            {
                await _context.HinhAnhBaiViets.AddRangeAsync
                (
                new  HinhAnhBaiViet { MaBaiViet = 1, NoiDung = "Hình ảnh chi tiết về cơ chế đồng hồ cơ", HinhAnh = "Blog_1_detail.jpg" },
                new HinhAnhBaiViet { MaBaiViet = 1, NoiDung = "Hình ảnh bộ máy đồng hồ cơ", HinhAnh = "Blog_1_mechanism.jpg" },
                new HinhAnhBaiViet { MaBaiViet = 2, NoiDung = "Hình ảnh các dụng cụ chăm sóc đồng hồ", HinhAnh = "Blog_Meo_tools.jpg" },
                new HinhAnhBaiViet { MaBaiViet = 2, NoiDung = "Hình ảnh quy trình chăm sóc đồng hồ", HinhAnh = "Blog_Meo_process.jpg" },
                new HinhAnhBaiViet { MaBaiViet = 3, NoiDung = "Hình ảnh đồng hồ đeo tay phổ biến", HinhAnh = "Blog_YN_watch.jpg" },
                new HinhAnhBaiViet { MaBaiViet = 3, NoiDung = "Hình ảnh lịch sử phát triển đồng hồ đeo tay", HinhAnh = "Blog_YN_history.jpg" },
                new HinhAnhBaiViet { MaBaiViet = 4, NoiDung = "Hình ảnh các mẫu đồng hồ phù hợp với phong cách", HinhAnh = "Blog_PC_style.jpg" },
                new HinhAnhBaiViet { MaBaiViet = 4, NoiDung = "Hình ảnh đồng hồ thời trang cho các dịp đặc biệt", HinhAnh = "Blog_PC_special.jpg" },
                new HinhAnhBaiViet { MaBaiViet = 5, NoiDung = "Hình ảnh các loại đồng hồ cơ phổ biến", HinhAnh = "Blog_Hublot_types.jpg" },
                new HinhAnhBaiViet { MaBaiViet = 5, NoiDung = "Hình ảnh chi tiết các bộ phận của đồng hồ cơ", HinhAnh = "Blog_Hublot_parts.jpg" },
                new HinhAnhBaiViet { MaBaiViet = 6, NoiDung = "Hình ảnh các mẫu đồng hồ nam cao cấp", HinhAnh = "Blog_Co_men_watch.jpg" },
                new HinhAnhBaiViet { MaBaiViet = 6, NoiDung = "Hình ảnh đồng hồ nam nổi bật năm 2025", HinhAnh = "Blog_Co_2025.jpg" }
                );
                await _context.SaveChangesAsync();
            }

            if (!_context.Footers.Any())
            {
                await _context.Footers.AddRangeAsync(
                    new Footer
                    {
                        Logo = "Logo.png",
                        MoTa = "ZZZ không chỉ là nơi để mua sắm, mà còn là một nơi để khám phá, tìm hiểu và đắm mình trong thế giới đồng hồ.",
                        DiaChi = "65 Đ. Huỳnh Thúc Kháng, Bến Nghé, Quận 1, Hồ Chí Minh",
                        Email = "contact@zzz.com",
                        SoDienThoai = "0123456789",
                        FacebookUrl = "https://www.facebook.com/ZZZWATCHESS/",
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
                    // Nhóm Tài Khoản (MaNhom = 2)
                    new FooterLink { TieuDe = "Tài Khoản Của Tôi", Url = "/Account/Index", MaNhom = 2, ThuTuHienThi = 1, TrangThai = true },
                    new FooterLink { TieuDe = "Yêu Thích", Url = "/Account/Favorite", MaNhom = 2, ThuTuHienThi = 2, TrangThai = true },
                    new FooterLink { TieuDe = "Lịch Sử Đơn Hàng", Url = "/Account/Order", MaNhom = 2, ThuTuHienThi = 3, TrangThai = true },
                    // Nhóm Danh Mục (MaNhom = 3)
                    new FooterLink { TieuDe = "Đồng Hồ Nam", Url = "/dong-ho-nam", MaNhom = 3, ThuTuHienThi = 1, TrangThai = true },
                    new FooterLink { TieuDe = "Đồng Hồ Nữ", Url = "/dong-ho-nu", MaNhom = 3, ThuTuHienThi = 2, TrangThai = true },
                    new FooterLink { TieuDe = "Đồng Hồ Cơ", Url = "/dong-ho-co", MaNhom = 3, ThuTuHienThi = 3, TrangThai = true },
                    new FooterLink { TieuDe = "Đồng Hồ Pin", Url = "/dong-ho-pin", MaNhom = 3, ThuTuHienThi = 4, TrangThai = true },
                    new FooterLink { TieuDe = "Đồng hồ Điện Tử", Url = "/dong-ho-dien-tu", MaNhom = 3, ThuTuHienThi = 5, TrangThai = true }
                );
                await _context.SaveChangesAsync();
            }

            if (!_context.Sliders.Any())
            {
                await _context.Sliders.AddRangeAsync
                (
                    new Slider { TieuDe = "Đồng Hồ Citizen", MoTa = "Sản Phẩm Nổi Bật", HinhAnh = "/HinhAnhs/ricky-kharawala-Yka2yhGJwjc-unsplash 1.png", Link = "/Product/ProductDetail/2", ThuTuHienThi = 1, TrangThai = true },
                    new Slider { TieuDe = "Đồng Hồ Citizen-Eco", MoTa = "Giảm giá đến 15%", HinhAnh = "/HinhAnhs/Artboard-1.jpg", Link = "/Product/ProductDetail/5", ThuTuHienThi = 2, TrangThai = true },
                    new Slider { TieuDe = "Đồng Hồ Doxa", MoTa = "Biểu tượng của đẳng cấp và phong cách", HinhAnh = "/HinhAnhs/default-large.jpg", Link = "/Product/ProductDetail/9", ThuTuHienThi = 3, TrangThai = true }
                );
                await _context.SaveChangesAsync();
            }

            if (!_context.GioiThieus.Any())
            {
                await _context.GioiThieus.AddRangeAsync
                (
                    new GioiThieu
                    {
                        NoiDung = @"
                        ZZZ WATCH không chỉ là nơi để mua sắm, mà còn là một nơi để khám phá, tìm hiểu và đắm mình trong thế giới đồng hồ.
                        <br />
                        ZZZ WATCH được xây dựng nhằm cung cấp cho khách hàng những sản phẩm đồng hồ đeo tay cao cấp, chất lượng, 
                        chính hãng cam kết mang đến cho khách hàng những mẫu đồng hồ hoàn hảo về cả thiết kế lẫn tính năng 
                        và hoàn thành sứ mệnh “Nơi An Tâm Mua Đồng Hồ Chính Hãng”. Đồng thời chúng tôi cũng hướng đến  những trải nghiệm dễ dàng, 
                        an toàn và nhanh chóng khi mua sắm trực tuyến thông qua hệ thống hỗ trợ thanh toán và vận hành vững mạnh.
                        ",
                        DiaChi = "65 Đ. Huỳnh Thúc Kháng, Bến Nghé, Quận 1, Hồ Chí Minh",
                        SoDienThoai = "0306221377",
                        Email = "0306221377@caothang.edu.vn"
                    }
                );
                await _context.SaveChangesAsync();
            }
            if (!_context.ChinhSachs.Any())
            {
                await _context.ChinhSachs.AddRangeAsync
                (
                    new ChinhSach
                    {
                        TieuDe = "Giao hàng nhanh",
                        NoiDung = @"Chúng tôi cam kết cung cấp dịch vụ giao hàng nhanh chóng và đáng tin cậy. Đơn hàng của bạn sẽ được xử lý và giao trong vòng 1-2 ngày làm việc, tùy thuộc vào địa chỉ giao hàng. 
                                Đặc biệt, đối với các đơn hàng trong khu vực nội thành, chúng tôi sẽ giao trong ngày nếu đơn hàng được đặt trước 12h00. 
                                Mọi chi phí giao hàng sẽ được hiển thị rõ ràng khi bạn thanh toán, và miễn phí vận chuyển cho đơn hàng có giá trị từ [số tiền cụ thể] trở lên. 
                                Chúng tôi luôn nỗ lực mang đến trải nghiệm giao hàng nhanh chóng, tiện lợi và không gây phiền phức cho khách hàng."
                    },
                    new ChinhSach
                    {
                        TieuDe = "Miễn phí giao hàng",
                        NoiDung = @"Cửa hàng sẽ miễn phí giao hàng cho tất cả các đơn hàng trong phạm vi nội thành.
                                Đối với các đơn hàng ở phạm vi ngoài thành phố thì sẽ được tính phí vận chuyển.
                                Thời gian nhận hàng sẽ từ 1-5 ngày tùy vào địa điểm nhận hàng.
                                Cửa hàng sẽ lựa chọn đối tác vận chuyển uy tín để đảm bảo đồng hồ được giao đến khách hàng một cách an toàn và đúng thời gian.
                                Trong quá trình vận chuyển, nếu sản phẩm bị hư hỏng hoặc thất lạc, cửa hàng sẽ chịu trách nhiệm hoàn toàn và có thể gửi lại sản phẩm mới hoặc hoàn tiền cho khách hàng.
                                Chính sách miễn phí giao hàng có thể không áp dụng cho các khu vực vùng sâu, vùng xa hoặc quốc tế, và trong trường hợp này, khách hàng sẽ được thông báo rõ ràng về các chi phí phát sinh."
                    },
                    new ChinhSach
                    {
                        TieuDe = "Cam kết chính hãng",
                        NoiDung = @"Cửa hàng cam kết tất cả đồng hồ bán ra đều là hàng chính hãng, được nhập khẩu hoặc phân phối trực tiếp từ nhà sản xuất hoặc đại lý ủy quyền.
                                Mỗi sản phẩm sẽ đi kèm với các giấy tờ chứng nhận chính hãng, bao gồm sổ bảo hành, hóa đơn mua hàng, và các giấy tờ liên quan khác.
                                Đồng hồ mua tại cửa hàng sẽ được bảo hành theo tiêu chuẩn của nhà sản xuất. Thời gian bảo hành và các dịch vụ đi kèm sẽ được thực hiện tại các trung tâm bảo hành ủy quyền.
                                Nếu khách hàng chứng minh được sản phẩm là hàng giả, cửa hàng cam kết hoàn trả toàn bộ số tiền đã thanh toán và có thể bồi thường thêm tùy theo chính sách cụ thể.
                                Cửa hàng sẽ cung cấp dịch vụ hậu mãi, bao gồm sửa chữa và bảo trì đồng hồ, với cam kết sử dụng linh kiện chính hãng.
                                Cửa hàng có thể áp dụng chính sách đổi trả linh hoạt nếu khách hàng phát hiện sản phẩm có lỗi sản xuất hoặc không đúng với mô tả ban đầu."
                    }
                );
                await _context.SaveChangesAsync();
            }
            if (!_context.VaiTros.Any())
            {
                await _context.VaiTros.AddRangeAsync
                (
                    new VaiTro { Loai = "User" },
                    new VaiTro { Loai = "Admin" }
                );
                await _context.SaveChangesAsync();
            }
            if (!_context.TaiKhoans.Any())
            {
                await _context.TaiKhoans.AddRangeAsync
                (
                    new TaiKhoan { TenDangNhap = "admin", MatKhau = "admin", MaVaiTro = 2, },
                    new TaiKhoan { TenDangNhap = "user1", MatKhau = "user1", MaVaiTro = 1 },
                    new TaiKhoan { TenDangNhap = "user2", MatKhau = "user2", MaVaiTro = 1, },
                    new TaiKhoan { TenDangNhap = "user3", MatKhau = "user3", MaVaiTro = 1, },
                    new TaiKhoan { TenDangNhap = "user4", MatKhau = "user4", MaVaiTro = 1 },
                    new TaiKhoan { TenDangNhap = "user5", MatKhau = "user5", MaVaiTro = 1, },
                    new TaiKhoan { TenDangNhap = "user6", MatKhau = "user6", MaVaiTro = 1 },
                    new TaiKhoan { TenDangNhap = "user7", MatKhau = "user7", MaVaiTro = 1, },
                    new TaiKhoan { TenDangNhap = "user8", MatKhau = "user8", MaVaiTro = 1 },
                    new TaiKhoan { TenDangNhap = "user9", MatKhau = "user9", MaVaiTro = 1, },
                    new TaiKhoan { TenDangNhap = "user10", MatKhau = "user10", MaVaiTro = 1 }
                );
                await _context.SaveChangesAsync();
            }
            if (!_context.KhachHangs.Any())
            {
                await _context.KhachHangs.AddRangeAsync
                (
                    new KhachHang { HoTen = "Nguyễn Văn A", SoDienThoai = "0123456789", DiaChi = "123 Đường ABC, Quận 1", Email = "vana@gmail.com", NgaySinh = DateOnly.ParseExact("1990-01-01", "yyyy-MM-dd"), GioiTinh = true, MaTaiKhoan = 2, TenHienThi = "user1" },

                    new KhachHang { HoTen = "Trần Thị B", SoDienThoai = "0987654321", DiaChi = "456 Đường DEF, Quận 2", Email = "btran@gmail.com", NgaySinh = DateOnly.ParseExact("1992-02-02", "yyyy-MM-dd"), GioiTinh = false, MaTaiKhoan = 3, TenHienThi = "user2" },

                    new KhachHang { HoTen = "Lê Văn C", SoDienThoai = "0123456780", DiaChi = "789 Đường GHI, Quận 3", Email = "cle@gmail.com", NgaySinh = DateOnly.ParseExact("1988-03-03", "yyyy-MM-dd"), GioiTinh = true, MaTaiKhoan = 4, TenHienThi = "user3" },

                    new KhachHang { HoTen = "Phạm Thị D", SoDienThoai = "0987654310", DiaChi = "321 Đường JKL, Quận 4", Email = "dpham@gmail.com", NgaySinh = DateOnly.ParseExact("1985-04-04", "yyyy-MM-dd"), GioiTinh = false, MaTaiKhoan = 5, TenHienThi = "user4" },

                    new KhachHang { HoTen = "Nguyễn Văn E", SoDienThoai = "0123456790", DiaChi = "654 Đường MNO, Quận 5", Email = "evan@gmail.com", NgaySinh = DateOnly.ParseExact("1995-05-05", "yyyy-MM-dd"), GioiTinh = true, MaTaiKhoan = 6, TenHienThi = "user5" },

                    new KhachHang { HoTen = "Trần Thị F", SoDienThoai = "0987654322", DiaChi = "987 Đường PQR, Quận 6", Email = "ftran@gmail.com", NgaySinh = DateOnly.ParseExact("1990-06-06", "yyyy-MM-dd"), GioiTinh = false, MaTaiKhoan = 7, TenHienThi = "user6" },

                    new KhachHang { HoTen = "Lê Văn G", SoDienThoai = "0123456781", DiaChi = "135 Đường STU, Quận 7", Email = "gle@gmail.com", NgaySinh = DateOnly.ParseExact("1982-07-07", "yyyy-MM-dd"), GioiTinh = true, MaTaiKhoan = 8, TenHienThi = "user7" },

                    new KhachHang { HoTen = "Phạm Thị H", SoDienThoai = "0987654311", DiaChi = "246 Đường VWX, Quận 8", Email = "hpham@gmail.com", NgaySinh = DateOnly.ParseExact("2000-07-07", "yyyy-MM-dd"), GioiTinh = true, MaTaiKhoan = 9, TenHienThi = "user8" },

                    new KhachHang { HoTen = "Nguyễn Văn I", SoDienThoai = "0123456791", DiaChi = "357 Đường YZ, Quận 9", Email = "ivan@gmail.com", NgaySinh = DateOnly.ParseExact("2002-08-30", "yyyy-MM-dd"), GioiTinh = true, MaTaiKhoan = 10, TenHienThi = "user9" },

                    new KhachHang { HoTen = "Trần Thị J", SoDienThoai = "0987654323", DiaChi = "468 Đường ABCD, Quận 10", Email = "jtran@gmail.com", NgaySinh = DateOnly.ParseExact("1996-01-11", "yyyy-MM-dd"), GioiTinh = true, MaTaiKhoan = 11, TenHienThi = "user10" }
                );
                await _context.SaveChangesAsync();
            }
            if (!_context.HoaDons.Any())
            {
                await _context.HoaDons.AddRangeAsync
                (
                    new HoaDon { MaKhachHang = 1, NgayDatHang = new DateTime(2021, 5, 15), HoTen = "Nguyễn Văn A", SoDienThoai = "0123456789", Email = "vana@gmail.com", DiaChi = "123 Đường ABC, Quận 1", Tinh = "TPHCM", Huyen = "Quận 1", Xa = "Phường 1", PhuongThucThanhToan = "Momo", TongTien = 15118000, TrangThai = 2 },

                    new HoaDon { MaKhachHang = 2, NgayDatHang = new DateTime(2021, 6, 20), HoTen = "Trần Thị B", SoDienThoai = "0987654321", Email = "btran@gmail.com", DiaChi = "456 Đường DEF, Quận 2", Tinh = "TPHCM", Huyen = "Quận 2", Xa = "Phường 2", PhuongThucThanhToan = "COD", TongTien = 32655000, TrangThai = 2 },

                    new HoaDon { MaKhachHang = 3, NgayDatHang = new DateTime(2022, 1, 10), HoTen = "Lê Văn C", SoDienThoai = "0123456780", Email = "cle@gmail.com", DiaChi = "789 Đường GHI, Quận 3", Tinh = "Hà Nội", Huyen = "Quận 3", Xa = "Phường 3", PhuongThucThanhToan = "Momo", TongTien = 32955000, TrangThai = 2 },

                    new HoaDon { MaKhachHang = 4, NgayDatHang = new DateTime(2022, 3, 15), HoTen = "Phạm Thị D", SoDienThoai = "0987654310", Email = "dpham@gmail.com", DiaChi = "321 Đường JKL, Quận 4", Tinh = "Đà Nẵng", Huyen = "Quận 4", Xa = "Phường 4", PhuongThucThanhToan = "COD", TongTien = 27830000, TrangThai = 2 },

                    new HoaDon { MaKhachHang = 5, NgayDatHang = new DateTime(2023, 2, 25), HoTen = "Nguyễn Văn E", SoDienThoai = "0123456790", Email = "evan@gmail.com", DiaChi = "654 Đường MNO, Quận 5", Tinh = "Hải Phòng", Huyen = "Quận 5", Xa = "Phường 5", PhuongThucThanhToan = "Momo", TongTien = 75090000, TrangThai = 2 },

                    new HoaDon { MaKhachHang = 6, NgayDatHang = new DateTime(2023, 4, 30), HoTen = "Trần Thị F", SoDienThoai = "0987654322", Email = "ftran@gmail.com", DiaChi = "987 Đường PQR, Quận 6", Tinh = "TPHCM", Huyen = "Quận 6", Xa = "Phường 6", PhuongThucThanhToan = "COD", TongTien = 51758000, TrangThai = 2 },
                            
                    new HoaDon { MaKhachHang = 7, NgayDatHang = new DateTime(2024, 7, 5), HoTen = "Lê Văn G", SoDienThoai = "0123456781", Email = "gle@gmail.com", DiaChi = "135 Đường STU, Quận 7", Tinh = "Hà Nội", Huyen = "Quận 7", Xa = "Phường 7", PhuongThucThanhToan = "Momo", TongTien = 30255000, TrangThai = 2 },

                    new HoaDon { MaKhachHang = 8, NgayDatHang = new DateTime(2024, 9, 10), HoTen = "Phạm Thị H", SoDienThoai = "0987654311", Email = "hpham@gmail.com", DiaChi = "246 Đường VWX, Quận 8", Tinh = "Đà Nẵng", Huyen = "Quận 8", Xa = "Phường 8", PhuongThucThanhToan = "COD", TongTien = 7585000, TrangThai = 2 },

                    new HoaDon { MaKhachHang = 9, NgayDatHang = new DateTime(2025, 1, 15), HoTen = "Nguyễn Văn I", SoDienThoai = "0123456791", Email = "ivan@gmail.com", DiaChi = "357 Đường YZ, Quận 9", Tinh = "Hải Phòng", Huyen = "Quận 9", Xa = "Phường 9", PhuongThucThanhToan = "Momo", TongTien = 63425000, TrangThai = 2 },

                    new HoaDon { MaKhachHang = 10, NgayDatHang = new DateTime(2025, 3, 20), HoTen = "Trần Thị J", SoDienThoai = "0987654323", Email = "jtran@gmail.com", DiaChi = "468 Đường ABCD, Quận 10", Tinh = "TPHCM", Huyen = "Quận 10", Xa = "Phường 10", PhuongThucThanhToan = "COD", TongTien = 11450000, TrangThai = 2 }

                );
                await _context.SaveChangesAsync();
            }
            if (!_context.ChiTietHoaDons.Any())
            {
                await _context.ChiTietHoaDons.AddRangeAsync
                (
                    new ChiTietHoaDon { MaHoaDon = 1, MaSanPham = 1, Gia = 2279000, SoLuong = 2, TongTien = 4558000 },

                    new ChiTietHoaDon { MaHoaDon = 1, MaSanPham = 2, Gia = 5280000, SoLuong = 2, TongTien = 10560000 },

                    new ChiTietHoaDon { MaHoaDon = 2, MaSanPham = 3, Gia = 12485000, SoLuong = 1, TongTien = 12485000 },

                    new ChiTietHoaDon { MaHoaDon = 2, MaSanPham = 4, Gia = 10085000, SoLuong = 2, TongTien = 20170000 },

                    new ChiTietHoaDon { MaHoaDon = 3, MaSanPham = 5, Gia = 7585000, SoLuong = 1, TongTien = 7585000 },

                    new ChiTietHoaDon { MaHoaDon = 3, MaSanPham = 6, Gia = 12685000, SoLuong = 2, TongTien = 25370000 },

                    new ChiTietHoaDon { MaHoaDon = 4, MaSanPham = 7, Gia = 23250000, SoLuong = 1, TongTien = 23250000 },

                    new ChiTietHoaDon { MaHoaDon = 4, MaSanPham = 8, Gia = 2290000, SoLuong = 2, TongTien = 4580000 },

                    new ChiTietHoaDon { MaHoaDon = 5, MaSanPham = 9, Gia = 25030000, SoLuong = 3, TongTien = 75090000 },

                    new ChiTietHoaDon { MaHoaDon = 6, MaSanPham = 10, Gia = 23600000, SoLuong = 2, TongTien = 47200000 },

                    new ChiTietHoaDon { MaHoaDon = 6, MaSanPham = 1, Gia = 2279000, SoLuong = 2, TongTien = 4558000 },

                    new ChiTietHoaDon { MaHoaDon = 7, MaSanPham = 4, Gia = 10085000, SoLuong = 3, TongTien = 30255000 },

                    new ChiTietHoaDon { MaHoaDon = 8, MaSanPham = 5, Gia = 7585000, SoLuong = 1, TongTien = 7585000 },

                    new ChiTietHoaDon { MaHoaDon = 9, MaSanPham = 6, Gia = 12685000, SoLuong = 5, TongTien = 63425000 },

                    new ChiTietHoaDon { MaHoaDon = 10, MaSanPham = 8, Gia = 2290000, SoLuong = 5, TongTien = 11450000 }

                );
                //await _context.SaveChangesAsync();
            }
        }

    }
}