using Ecommerce_CaFeShop.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_CaFeShop
{
    public class SeedData
    {
        public static async Task SeedingData(CaFeContext _context)
        {
            await _context.Database.MigrateAsync();
            if (!_context.ThuongHieus.Any())
            {
                ThuongHieu TrungNguyen = new ThuongHieu { TenThuongHieu = "Trung Nguyên", Slug = "trung-nguyen" };
                ThuongHieu HighlandsCoffee = new ThuongHieu { TenThuongHieu = "Highlands Coffee", Slug = "highlands-coffee" };
                ThuongHieu TheCoffeeHouse = new ThuongHieu { TenThuongHieu = "The Coffee House", Slug = "the-coffee-house" };
                ThuongHieu G7 = new ThuongHieu { TenThuongHieu = "G7", Slug = "g7" };

                await _context.ThuongHieus.AddRangeAsync(TrungNguyen, HighlandsCoffee, TheCoffeeHouse, G7);
                await _context.SaveChangesAsync();
            }

            if (!_context.DanhMucs.Any())
            {
                DanhMuc cafeHatRang = new DanhMuc { TenDanhMuc = "Cà phê hạt rang", MaDanhMucCha = null, Slug = "ca-phe-hat-rang" };
                DanhMuc cafeXay = new DanhMuc { TenDanhMuc = "Cà phê xay", MaDanhMucCha = null, Slug = "ca-phe-xay" };
                DanhMuc cafePhinTruyenThong = new DanhMuc { TenDanhMuc = "Cà phê phin truyền thống", MaDanhMucCha = null, Slug = "ca-phe-phin-truyen-thong" };

                await _context.DanhMucs.AddRangeAsync(cafeHatRang, cafeXay, cafePhinTruyenThong);
                await _context.SaveChangesAsync();
            }

            if (!_context.SanPhams.Any())
            {
                var cafeXay = _context.DanhMucs.FirstOrDefault(c => c.TenDanhMuc == "Cà phê xay");
                var cafeHatRang = _context.DanhMucs.FirstOrDefault(c => c.TenDanhMuc == "Cà phê hạt rang");
                var cafePhinTruyenThong = _context.DanhMucs.FirstOrDefault(c => c.TenDanhMuc == "Cà phê phin truyền thống");

                var TrungNguyen = _context.ThuongHieus.FirstOrDefault(b => b.TenThuongHieu == "Trung Nguyên");
                var HighlandsCoffee = _context.ThuongHieus.FirstOrDefault(b => b.TenThuongHieu == "Highlands Coffee");
                var TheCoffeeHouse = _context.ThuongHieus.FirstOrDefault(b => b.TenThuongHieu == "The Coffee House");
                var G7 = _context.ThuongHieus.FirstOrDefault(b => b.TenThuongHieu == "G7");

                await _context.SanPhams.AddRangeAsync(
                    new SanPham
                    {
                        HinhAnh = "TheCoffeeHouseKashmirBlend.png",
                        TenSanPham = "The Coffee House Kashmir Blend",
                        MaDanhMuc = cafeHatRang.MaDanhMuc,
                        MaThuongHieu = TheCoffeeHouse.MaThuongHieu,                       
                        Gia = 150000,
                        MoTaNgan = "Hương vị đậm đà, thơm nồng của cà phê hạt rang nguyên chất",
                        MoTa = "The Coffee House Kashmir Blend là sự kết hợp hoàn hảo giữa hạt cà phê Robusta và Arabica, mang lại hương vị đậm đà, hậu ngọt, phù hợp cho pha phin hoặc máy.",
                        SoLuong = 50,
                        TrangThai = 1,
                        LuotXem = 1000,
                        NgayTao = DateTime.Now,
                        NgayCapNhat = null,
                        DaXoa = 0,
                        Slug = "the-coffee-house-kashmir-blend"
                    },
                    new SanPham
                    {
                        HinhAnh = "trungnguyensantao1.png",
                        TenSanPham = "Trung Nguyên Sáng Tạo 1",
                        MaDanhMuc = cafeXay.MaDanhMuc,
                        MaThuongHieu = TrungNguyen.MaThuongHieu,
                        Gia = 120000,
                        MoTaNgan = "Cà phê xay đậm đà, phong cách Việt Nam truyền thống",
                        MoTa = "Trung Nguyên Sáng Tạo 1 là dòng cà phê xay với hương vị mạnh mẽ, đậm chất Việt, thích hợp cho pha phin truyền thống.",
                        SoLuong = 40,
                        TrangThai = 1,
                        LuotXem = 800,
                        NgayTao = DateTime.Now,
                        NgayCapNhat = null,
                        DaXoa = 0,
                        Slug = "trung-nguyen-sang-tao-1"
                    },
                    new SanPham
                    {
                        HinhAnh = "trungnguyenclassic.png",
                        TenSanPham = "Trung Nguyên Legend Classic",
                        MaDanhMuc = cafeHatRang.MaDanhMuc,
                        MaThuongHieu = TrungNguyen.MaThuongHieu,
                        Gia = 200000,
                        MoTaNgan = "Hạt cà phê rang cao cấp với hương vị cân bằng",
                        MoTa = "Trung Nguyên Legend Classic là sự lựa chọn hoàn hảo cho những ai yêu thích cà phê đậm đà, thơm nồng, với hậu vị ngọt ngào.",
                        SoLuong = 30,
                        TrangThai = 1,
                        LuotXem = 600,
                        NgayTao = DateTime.Now,
                        NgayCapNhat = null,
                        DaXoa = 0,
                        Slug = "trung-nguyen-legend-classic"
                    },
                    new SanPham
                    {
                        HinhAnh = "culirobusta.png",
                        TenSanPham = "Trung Nguyên Culi Robusta",
                        MaDanhMuc = cafeHatRang.MaDanhMuc,
                        MaThuongHieu = TrungNguyen.MaThuongHieu,
                        Gia = 180000,
                        MoTaNgan = "Cà phê Culi Robusta với hương vị đậm đà, mạnh mẽ",
                        MoTa = "Trung Nguyên Culi Robusta được chọn lọc từ những hạt cà phê Culi chất lượng cao, mang đến hương vị đậm đà, mạnh mẽ và hậu vị kéo dài.",
                        SoLuong = 25,
                        TrangThai = 1,
                        LuotXem = 500,
                        NgayTao = DateTime.Now,
                        NgayCapNhat = null,
                        DaXoa = 0,
                        Slug = "trung-nguyen-culi-robusta"
                    },
                    new SanPham
                    {
                        HinhAnh = "HighlandsCoffeePhinsuada.png",
                        TenSanPham = "Highlands Coffee Phin Sữa Đá",
                        MaDanhMuc = cafePhinTruyenThong.MaDanhMuc,
                        MaThuongHieu = HighlandsCoffee.MaThuongHieu,
                        Gia = 100000,
                        MoTaNgan = "Cà phê phin truyền thống đậm đà, hòa quyện sữa đặc",
                        MoTa = "Highlands Coffee Phin Sữa Đá mang đậm phong cách Việt Nam với vị cà phê đậm, hòa quyện cùng sữa đặc thơm béo, thích hợp cho mọi thời điểm trong ngày.",
                        SoLuong = 60,
                        TrangThai = 2,
                        LuotXem = 1200,
                        NgayTao = DateTime.Now,
                        NgayCapNhat = null,
                        DaXoa = 0,
                        Slug = "highlands-coffee-phin-sua-da"
                    },
                    new SanPham
                    {
                        HinhAnh = "HighlandsCoffeeArabicaBlend.png",
                        TenSanPham = "Highlands Coffee Arabica Blend",
                        MaDanhMuc = cafeXay.MaDanhMuc,
                        MaThuongHieu = HighlandsCoffee.MaThuongHieu,
                        Gia = 160000,
                        MoTaNgan = "Cà phê xay Arabica với hương thơm nhẹ nhàng",
                        MoTa = "Highlands Coffee Arabica Blend mang đến hương vị nhẹ nhàng, thơm ngát với chút chua thanh, phù hợp cho những ai yêu thích cà phê nhẹ.",
                        SoLuong = 45,
                        TrangThai = 2,
                        LuotXem = 900,
                        NgayTao = DateTime.Now,
                        NgayCapNhat = null,
                        DaXoa = 0,
                        Slug = "highlands-coffee-arabica-blend"
                    },
                    new SanPham
                    {
                        HinhAnh = "G73in1Instant Coffee.png",
                        TenSanPham = "G7 3in1 Instant Coffee",
                        MaDanhMuc = cafePhinTruyenThong.MaDanhMuc,
                        MaThuongHieu = G7.MaThuongHieu,
                        Gia = 90000,
                        MoTaNgan = "Cà phê hòa tan 3in1 tiện lợi, đậm vị",
                        MoTa = "G7 3in1 Instant Coffee mang đến sự tiện lợi với hương vị đậm đà, hòa quyện giữa cà phê, sữa và đường, phù hợp cho người bận rộn.",
                        SoLuong = 100,
                        TrangThai = 3,
                        LuotXem = 1500,
                        NgayTao = DateTime.Now,
                        NgayCapNhat = null,
                        DaXoa = 0,
                        Slug = "g7-3in1-instant-coffee"
                    },
                    new SanPham
                    {
                        HinhAnh = "TheCoffeeHouseHouseBlend.png",
                        TenSanPham = "The Coffee House House Blend",
                        MaDanhMuc = cafeXay.MaDanhMuc,
                        MaThuongHieu = TheCoffeeHouse.MaThuongHieu,
                        Gia = 140000,
                        MoTaNgan = "Cà phê xay với hương vị cân bằng, dễ uống",
                        MoTa = "The Coffee House House Blend là sự kết hợp tinh tế giữa Arabica và Robusta, mang đến hương vị cân bằng, dễ uống, phù hợp cho mọi cách pha chế.",
                        SoLuong = 35,
                        TrangThai = 3,
                        LuotXem = 700,
                        NgayTao = DateTime.Now,
                        NgayCapNhat = null,
                        DaXoa = 0,
                        Slug = "the-coffee-house-house-blend"
                    },
                    new SanPham
                    {
                        HinhAnh = "HighlandsCoffeeMokaBlend.png",
                        TenSanPham = "Highlands Coffee Moka Blend",
                        MaDanhMuc = cafeHatRang.MaDanhMuc,
                        MaThuongHieu = HighlandsCoffee.MaThuongHieu,
                        Gia = 170000,
                        MoTaNgan = "Cà phê hạt rang Moka với hương vị độc đáo",
                        MoTa = "Highlands Coffee Moka Blend mang đến hương vị đặc trưng của hạt Moka, với vị chua nhẹ và hương thơm quyến rũ, lý tưởng cho pha máy.",
                        SoLuong = 50,
                        TrangThai = 3,
                        LuotXem = 850,
                        NgayTao = DateTime.Now,
                        NgayCapNhat = null,
                        DaXoa = 0,
                        Slug = "highlands-coffee-moka-blend"
                    },
                    new SanPham
                    {
                        HinhAnh = "G7BlackInstantCoffee.png",
                        TenSanPham = "G7 Black Instant Coffee",
                        MaDanhMuc = cafePhinTruyenThong.MaDanhMuc,
                        MaThuongHieu = G7.MaThuongHieu,
                        Gia = 80000,
                        MoTaNgan = "Cà phê hòa tan đen nguyên chất, đậm vị",
                        MoTa = "G7 Black Instant Coffee mang đến hương vị cà phê đen nguyên chất, đậm đà, không đường, dành cho những ai yêu thích sự mạnh mẽ.",
                        SoLuong = 80,
                        TrangThai = 3,
                        LuotXem = 1100,
                        NgayTao = DateTime.Now,
                        NgayCapNhat = null,
                        DaXoa = 0,
                        Slug = "g7-black-instant-coffee"
                    }
                );
                await _context.SaveChangesAsync();

                await _context.HinhAnhSanPhams.AddRangeAsync(
                    new HinhAnhSanPham { MaSanPham = 1, HinhAnh = "culirobusta.png" },
                    new HinhAnhSanPham { MaSanPham = 1, HinhAnh = "G73in1Instant Coffee.png" },
                    new HinhAnhSanPham { MaSanPham = 2, HinhAnh = "G7BlackInstantCoffee.png" },
                    new HinhAnhSanPham { MaSanPham = 2, HinhAnh = "HighlandsCoffeeArabicaBlend.png" },
                    new HinhAnhSanPham { MaSanPham = 3, HinhAnh = "HighlandsCoffeeMokaBlend.png" },
                    new HinhAnhSanPham { MaSanPham = 3, HinhAnh = "HighlandsCoffeePhinsuada.png" },
                    new HinhAnhSanPham { MaSanPham = 4, HinhAnh = "TheCoffeeHouseHouseBlend.png" },
                    new HinhAnhSanPham { MaSanPham = 4, HinhAnh = "TheCoffeeHouseKashmirBlend.png" },
                    new HinhAnhSanPham { MaSanPham = 5, HinhAnh = "trungnguyenclassic.png" },
                    new HinhAnhSanPham { MaSanPham = 5, HinhAnh = "trungnguyensantao1.png" },
                    new HinhAnhSanPham { MaSanPham = 6, HinhAnh = "culirobusta.png" },
                    new HinhAnhSanPham { MaSanPham = 6, HinhAnh = "G73in1Instant Coffee.png" },
                    new HinhAnhSanPham { MaSanPham = 7, HinhAnh = "G7BlackInstantCoffee.png" },
                    new HinhAnhSanPham { MaSanPham = 7, HinhAnh = "HighlandsCoffeeArabicaBlend.png" },
                    new HinhAnhSanPham { MaSanPham = 8, HinhAnh = "HighlandsCoffeeMokaBlend.png" },
                    new HinhAnhSanPham { MaSanPham = 8, HinhAnh = "HighlandsCoffeePhinsuada.png" },
                    new HinhAnhSanPham { MaSanPham = 9, HinhAnh = "TheCoffeeHouseHouseBlend.png" },
                    new HinhAnhSanPham { MaSanPham = 9, HinhAnh = "TheCoffeeHouseKashmirBlend.png" },
                    new HinhAnhSanPham { MaSanPham = 10, HinhAnh = "trungnguyenclassic.png" },
                    new HinhAnhSanPham { MaSanPham = 10, HinhAnh = "trungnguyensantao1.png" }
                );
                await _context.SaveChangesAsync();
            }

            if (!_context.BaiViets.Any())
            {
                await _context.BaiViets.AddRangeAsync(
                    new BaiViet
                    {
                        HinhAnh = "hieubietvequytrinhrangcafe.jpg",
                        TieuDe = "Hiểu Biết Về Quy Trình Rang Cà Phê",
                        NoiDung = "Khám phá quy trình rang cà phê và những yếu tố tạo nên hương vị độc đáo của từng loại cà phê."
                    },
                    new BaiViet
                    {
                        HinhAnh = "meophacaphetainha.jpg",
                        TieuDe = "Mẹo Pha Cà Phê Ngon Tại Nhà",
                        NoiDung = "Học cách pha cà phê ngon tại nhà với các mẹo đơn giản từ chọn hạt, rang xay đến pha chế."
                    },
                    new BaiViet
                    {
                        HinhAnh = "Lscfvn.jpg",
                        TieuDe = "Lịch Sử Cà Phê Việt Nam",
                        NoiDung = "Tìm hiểu về hành trình của cà phê Việt Nam, từ khi du nhập đến vị thế hàng đầu thế giới."
                    },
                    new BaiViet
                    {
                        HinhAnh = "chonloaicfphuhop.jpg",
                        TieuDe = "Chọn Loại Cà Phê Phù Hợp Với Bạn",
                        NoiDung = "Bạn đang tìm kiếm loại cà phê phù hợp với khẩu vị? Hãy tham khảo những gợi ý dưới đây để chọn cà phê hoàn hảo."
                    },
                    new BaiViet
                    {
                        HinhAnh = "cacloaihatphobien.jpg",
                        TieuDe = "Các Loại Cà Phê Hạt Rang Phổ Biến",
                        NoiDung = "Tìm hiểu các loại cà phê hạt rang phổ biến như Robusta, Arabica, Moka và cách chọn lựa phù hợp."
                    },
                    new BaiViet
                    {
                        HinhAnh = "topcafe.jpg",
                        TieuDe = "Top 10 Loại Cà Phê Ngon Nhất 2025",
                        NoiDung = "Khám phá danh sách 10 loại cà phê ngon nhất trong năm 2025, từ hương vị truyền thống đến hiện đại."
                    }
                );
                await _context.SaveChangesAsync();
            }

            if (!_context.HinhAnhBaiViets.Any())
            {
                await _context.HinhAnhBaiViets.AddRangeAsync(
                    new HinhAnhBaiViet { MaBaiViet = 1, NoiDung = "Hình ảnh quy trình rang cà phê", HinhAnh = "hieubietvequytrinhrangcafe.png.jpg" },
                    new HinhAnhBaiViet { MaBaiViet = 1, NoiDung = "Hình ảnh hạt cà phê rang", HinhAnh = "cacloaihatphobien.jpg" },
                    new HinhAnhBaiViet { MaBaiViet = 2, NoiDung = "Hình ảnh dụng cụ pha cà phê", HinhAnh = "dungcucafe.jpg" },
                    new HinhAnhBaiViet { MaBaiViet = 2, NoiDung = "Hình ảnh quy trình pha phin", HinhAnh = "quytrinhphacafep.jpg" },
                    new HinhAnhBaiViet { MaBaiViet = 3, NoiDung = "Hình ảnh lịch sử cà phê Việt", HinhAnh = "Lscfvn.jpg" },
                    new HinhAnhBaiViet { MaBaiViet = 3, NoiDung = "Hình ảnh đồn điền cà phê", HinhAnh = "dondiencafe.png" },
                    new HinhAnhBaiViet { MaBaiViet = 4, NoiDung = "Hình ảnh các loại cà phê", HinhAnh = "cacloaicafe.jpg" },
                    new HinhAnhBaiViet { MaBaiViet = 4, NoiDung = "Hình ảnh cà phê cho từng khẩu vị", HinhAnh = "tungkhauvi.jpg" },
                    new HinhAnhBaiViet { MaBaiViet = 5, NoiDung = "Hình ảnh cà phê hạt rang", HinhAnh = "cacloaihatphobien.jpg" },
                    new HinhAnhBaiViet { MaBaiViet = 5, NoiDung = "Hình ảnh chi tiết hạt cà phê", HinhAnh = "cacloaihatphobien.jpg" },
                    new HinhAnhBaiViet { MaBaiViet = 6, NoiDung = "Hình ảnh cà phê cao cấp", HinhAnh = "HighlandsCoffeePhinsuada.png" },
                    new HinhAnhBaiViet { MaBaiViet = 6, NoiDung = "Hình ảnh cà phê nổi bật 2025", HinhAnh = "topcafe.jpg" }
                );
                await _context.SaveChangesAsync();
            }

            if (!_context.Footers.Any())
            {
                await _context.Footers.AddRangeAsync(
                    new Footer
                    {
                        Logo = "logo.png",
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

            if (!_context.Sliders.Any())
            {
                await _context.Sliders.AddRangeAsync(
                    new Slider { TieuDe = "Trung Nguyên Legend", MoTa = "Sản Phẩm Nổi Bật", HinhAnh = "~/images/ca-phe-chon-gia-bao-nhieu-va-ban-o-dau-202109281143187496.jpg", Link = "/Product/ProductDetail/3", ThuTuHienThi = 1, TrangThai = true },
                    new Slider { TieuDe = "Highlands Coffee Phin", MoTa = "Giảm giá đến 15%", HinhAnh = "~/images/HighlandsCoffeePhinsuada.png", Link = "/Product/ProductDetail/5", ThuTuHienThi = 2, TrangThai = true },
                    new Slider { TieuDe = "The Coffee House Blend", MoTa = "Hương vị cân bằng, dễ uống", HinhAnh = "~/images/cafexay.png", Link = "/Product/ProductDetail/8", ThuTuHienThi = 3, TrangThai = true }
                );
                await _context.SaveChangesAsync();
            }

            if (!_context.GioiThieus.Any())
            {
                await _context.GioiThieus.AddRangeAsync(
                    new GioiThieu
                    {
                        NoiDung = @"
                         Coffeeshop không chỉ là nơi để mua sắm, mà còn là một nơi để khám phá, tìm hiểu và đắm mình trong thế giới cà phê Việt Nam.
                        <br />
                         Coffeeshop được xây dựng nhằm cung cấp cho khách hàng những sản phẩm cà phê chất lượng cao, chính gốc, 
                        cam kết mang đến những ly cà phê hoàn hảo về cả hương vị lẫn trải nghiệm. 
                        Đồng thời chúng tôi cũng hướng đến những trải nghiệm dễ dàng, an toàn và nhanh chóng khi mua sắm trực tuyến thông qua hệ thống hỗ trợ thanh toán và vận hành vững mạnh.
                        ",
                        DiaChi = "65 Đ. Huỳnh Thúc Kháng, Bến Nghé, Quận 1, Hồ Chí Minh",
                        SoDienThoai = "0306221302",
                        Email = "0306221302@caothang.edu.vn"
                    }
                );
                await _context.SaveChangesAsync();
            }

            if (!_context.ChinhSachs.Any())
            {
                await _context.ChinhSachs.AddRangeAsync(
                    new ChinhSach
                    {
                        TieuDe = "Giao hàng nhanh",
                        NoiDung = @"Chúng tôi cam kết cung cấp dịch vụ giao hàng nhanh chóng và đáng tin cậy. Đơn hàng của bạn sẽ được xử lý và giao trong vòng 1-2 ngày làm việc, tùy thuộc vào địa chỉ giao hàng. 
                                Đặc biệt, đối với các đơn hàng trong khu vực nội thành, chúng tôi sẽ giao trong ngày nếu đơn hàng được đặt trước 12h00. 
                                Mọi chi phí giao hàng sẽ được hiển thị rõ ràng khi bạn thanh toán, và miễn phí vận chuyển cho đơn hàng có giá trị từ 500,000 VNĐ trở lên. 
                                Chúng tôi luôn nỗ lực mang đến trải nghiệm giao hàng nhanh chóng, tiện lợi và không gây phiền phức cho khách hàng."
                    },
                    new ChinhSach
                    {
                        TieuDe = "Miễn phí giao hàng",
                        NoiDung = @"Cửa hàng sẽ miễn phí giao hàng cho tất cả các đơn hàng trong phạm vi nội thành.
                                Đối với các đơn hàng ở phạm vi ngoài thành phố thì sẽ được tính phí vận chuyển.
                                Thời gian nhận hàng sẽ từ 1-5 ngày tùy vào địa điểm nhận hàng.
                                Cửa hàng sẽ lựa chọn đối tác vận chuyển uy tín để đảm bảo sản phẩm được giao đến khách hàng một cách an toàn và đúng thời gian.
                                Trong quá trình vận chuyển, nếu sản phẩm bị hư hỏng hoặc thất lạc, cửa hàng sẽ chịu trách nhiệm hoàn toàn và có thể gửi lại sản phẩm mới hoặc hoàn tiền cho khách hàng.
                                Chính sách miễn phí giao hàng có thể không áp dụng cho các khu vực vùng sâu, vùng xa hoặc quốc tế, và trong trường hợp này, khách hàng sẽ được thông báo rõ ràng về các chi phí phát sinh."
                    },
                    new ChinhSach
                    {
                        TieuDe = "Cam kết chất lượng",
                        NoiDung = @"Cửa hàng cam kết tất cả cà phê bán ra đều là sản phẩm chất lượng cao, được chọn lọc từ các nguồn cung cấp uy tín.
                                Mỗi sản phẩm sẽ đi kèm với thông tin nguồn gốc rõ ràng và được đóng gói cẩn thận để bảo đảm chất lượng.
                                Nếu khách hàng chứng minh được sản phẩm không đạt chất lượng hoặc không đúng mô tả, cửa hàng cam kết hoàn trả toàn bộ số tiền đã thanh toán.
                                Cửa hàng cung cấp dịch vụ hậu mãi, bao gồm tư vấn cách bảo quản và pha chế cà phê để đạt hương vị tốt nhất.
                                Chính sách đổi trả linh hoạt được áp dụng nếu khách hàng phát hiện sản phẩm có lỗi từ nhà sản xuất."
                    }
                );
                await _context.SaveChangesAsync();
            }

            if (!_context.VaiTros.Any())
            {
                await _context.VaiTros.AddRangeAsync(
                    new VaiTro { Loai = "User" },
                    new VaiTro { Loai = "Admin" }
                );
                await _context.SaveChangesAsync();
            }

            if (!_context.TaiKhoans.Any())
            {
                await _context.TaiKhoans.AddRangeAsync(
                    new TaiKhoan { TenDangNhap = "admin", MatKhau = "admin", MaVaiTro = 2 },
                    new TaiKhoan { TenDangNhap = "user1", MatKhau = "user1", MaVaiTro = 1 },
                    new TaiKhoan { TenDangNhap = "user2", MatKhau = "user2", MaVaiTro = 1 },
                    new TaiKhoan { TenDangNhap = "user3", MatKhau = "user3", MaVaiTro = 1 },
                    new TaiKhoan { TenDangNhap = "user4", MatKhau = "user4", MaVaiTro = 1 },
                    new TaiKhoan { TenDangNhap = "user5", MatKhau = "user5", MaVaiTro = 1 },
                    new TaiKhoan { TenDangNhap = "user6", MatKhau = "user6", MaVaiTro = 1 },
                    new TaiKhoan { TenDangNhap = "user7", MatKhau = "user7", MaVaiTro = 1 },
                    new TaiKhoan { TenDangNhap = "user8", MatKhau = "user8", MaVaiTro = 1 },
                    new TaiKhoan { TenDangNhap = "user9", MatKhau = "user9", MaVaiTro = 1 },
                    new TaiKhoan { TenDangNhap = "user10", MatKhau = "user10", MaVaiTro = 1 }
                );
                await _context.SaveChangesAsync();
            }

            if (!_context.KhachHangs.Any())
            {
                await _context.KhachHangs.AddRangeAsync(
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
                await _context.HoaDons.AddRangeAsync(
                    new HoaDon { MaKhachHang = 1, NgayDatHang = new DateTime(2021, 5, 15), HoTen = "Nguyễn Văn A", SoDienThoai = "0123456789", Email = "vana@gmail.com", DiaChi = "123 Đường ABC, Quận 1", Tinh = "TPHCM", Huyen = "Quận 1", Xa = "Phường 1", PhuongThucThanhToan = "Momo", TongTien = 390000, TrangThai = 2 },
                    new HoaDon { MaKhachHang = 2, NgayDatHang = new DateTime(2021, 6, 20), HoTen = "Trần Thị B", SoDienThoai = "0987654321", Email = "btran@gmail.com", DiaChi = "456 Đường DEF, Quận 2", Tinh = "TPHCM", Huyen = "Quận 2", Xa = "Phường 2", PhuongThucThanhToan = "COD", TongTien = 560000, TrangThai = 2 },
                    new HoaDon { MaKhachHang = 3, NgayDatHang = new DateTime(2022, 1, 10), HoTen = "Lê Văn C", SoDienThoai = "0123456780", Email = "cle@gmail.com", DiaChi = "789 Đường GHI, Quận 3", Tinh = "Hà Nội", Huyen = "Quận 3", Xa = "Phường 3", PhuongThucThanhToan = "Momo", TongTien = 380000, TrangThai = 2 },
                    new HoaDon { MaKhachHang = 4, NgayDatHang = new DateTime(2022, 3, 15), HoTen = "Phạm Thị D", SoDienThoai = "0987654310", Email = "dpham@gmail.com", DiaChi = "321 Đường JKL, Quận 4", Tinh = "Đà Nẵng", Huyen = "Quận 4", Xa = "Phường 4", PhuongThucThanhToan = "COD", TongTien = 450000, TrangThai = 2 },
                    new HoaDon { MaKhachHang = 5, NgayDatHang = new DateTime(2023, 2, 25), HoTen = "Nguyễn Văn E", SoDienThoai = "0123456790", Email = "evan@gmail.com", DiaChi = "654 Đường MNO, Quận 5", Tinh = "Hải Phòng", Huyen = "Quận 5", Xa = "Phường 5", PhuongThucThanhToan = "Momo", TongTien = 600000, TrangThai = 2 },
                    new HoaDon { MaKhachHang = 6, NgayDatHang = new DateTime(2023, 4, 30), HoTen = "Trần Thị F", SoDienThoai = "0987654322", Email = "ftran@gmail.com", DiaChi = "987 Đường PQR, Quận 6", Tinh = "TPHCM", Huyen = "Quận 6", Xa = "Phường 6", PhuongThucThanhToan = "COD", TongTien = 420000, TrangThai = 2 },
                    new HoaDon { MaKhachHang = 7, NgayDatHang = new DateTime(2024, 7, 5), HoTen = "Lê Văn G", SoDienThoai = "0123456781", Email = "gle@gmail.com", DiaChi = "135 Đường STU, Quận 7", Tinh = "Hà Nội", Huyen = "Quận 7", Xa = "Phường 7", PhuongThucThanhToan = "Momo", TongTien = 540000, TrangThai = 2 },
                    new HoaDon { MaKhachHang = 8, NgayDatHang = new DateTime(2024, 9, 10), HoTen = "Phạm Thị H", SoDienThoai = "0987654311", Email = "hpham@gmail.com", DiaChi = "246 Đường VWX, Quận 8", Tinh = "Đà Nẵng", Huyen = "Quận 8", Xa = "Phường 8", PhuongThucThanhToan = "COD", TongTien = 240000, TrangThai = 2 },
                    new HoaDon { MaKhachHang = 9, NgayDatHang = new DateTime(2025, 1, 15), HoTen = "Nguyễn Văn I", SoDienThoai = "0123456791", Email = "ivan@gmail.com", DiaChi = "357 Đường YZ, Quận 9", Tinh = "Hải Phòng", Huyen = "Quận 9", Xa = "Phường 9", PhuongThucThanhToan = "Momo", TongTien = 700000, TrangThai = 2 },
                    new HoaDon { MaKhachHang = 10, NgayDatHang = new DateTime(2025, 3, 20), HoTen = "Trần Thị J", SoDienThoai = "0987654323", Email = "jtran@gmail.com", DiaChi = "468 Đường ABCD, Quận 10", Tinh = "TPHCM", Huyen = "Quận 10", Xa = "Phường 10", PhuongThucThanhToan = "COD", TongTien = 360000, TrangThai = 2 }
                );
                await _context.SaveChangesAsync();
            }

            if (!_context.ChiTietHoaDons.Any())
            {
                await _context.ChiTietHoaDons.AddRangeAsync(
                    new ChiTietHoaDon { MaHoaDon = 1, MaSanPham = 1, Gia = 150000, SoLuong = 2, TongTien = 300000 },
                    new ChiTietHoaDon { MaHoaDon = 1, MaSanPham = 7, Gia = 90000, SoLuong = 1, TongTien = 90000 },
                    new ChiTietHoaDon { MaHoaDon = 2, MaSanPham = 3, Gia = 200000, SoLuong = 2, TongTien = 400000 },
                    new ChiTietHoaDon { MaHoaDon = 2, MaSanPham = 8, Gia = 140000, SoLuong = 1, TongTien = 160000 },
                    new ChiTietHoaDon { MaHoaDon = 3, MaSanPham = 2, Gia = 120000, SoLuong = 2, TongTien = 240000 },
                    new ChiTietHoaDon { MaHoaDon = 3, MaSanPham = 10, Gia = 80000, SoLuong = 2, TongTien = 160000 },
                    new ChiTietHoaDon { MaHoaDon = 4, MaSanPham = 5, Gia = 100000, SoLuong = 3, TongTien = 300000 },
                    new ChiTietHoaDon { MaHoaDon = 4, MaSanPham = 9, Gia = 150000, SoLuong = 1, TongTien = 150000 },
                    new ChiTietHoaDon { MaHoaDon = 5, MaSanPham = 4, Gia = 180000, SoLuong = 2, TongTien = 360000 },
                    new ChiTietHoaDon { MaHoaDon = 5, MaSanPham = 6, Gia = 160000, SoLuong = 1, TongTien = 160000 },
                    new ChiTietHoaDon { MaHoaDon = 6, MaSanPham = 7, Gia = 90000, SoLuong = 3, TongTien = 270000 },
                    new ChiTietHoaDon { MaHoaDon = 6, MaSanPham = 1, Gia = 150000, SoLuong = 1, TongTien = 150000 },
                    new ChiTietHoaDon { MaHoaDon = 7, MaSanPham = 3, Gia = 200000, SoLuong = 2, TongTien = 400000 },
                    new ChiTietHoaDon { MaHoaDon = 7, MaSanPham = 8, Gia = 140000, SoLuong = 1, TongTien = 140000 },
                    new ChiTietHoaDon { MaHoaDon = 8, MaSanPham = 2, Gia = 120000, SoLuong = 2, TongTien = 240000 },
                    new ChiTietHoaDon { MaHoaDon = 9, MaSanPham = 9, Gia = 170000, SoLuong = 3, TongTien = 510000 },
                    new ChiTietHoaDon { MaHoaDon = 9, MaSanPham = 10, Gia = 80000, SoLuong = 2, TongTien = 160000 },
                    new ChiTietHoaDon { MaHoaDon = 10, MaSanPham = 7, Gia = 90000, SoLuong = 4, TongTien = 360000 }
                );
                //await _context.SaveChangesAsync();
            }
        }
    }
}