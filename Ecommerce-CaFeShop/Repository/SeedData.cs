using Ecommerce_CaFeShop.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

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
                ThuongHieu Deva = new ThuongHieu { TenThuongHieu = "Deva", Slug = "deva" };

                await _context.ThuongHieus.AddRangeAsync(TrungNguyen, HighlandsCoffee, TheCoffeeHouse, G7, Deva);
                await _context.SaveChangesAsync();
            }

            if (!_context.DanhMucs.Any())
            {
                DanhMuc cafeHatRang = new DanhMuc { TenDanhMuc = "Cà phê hạt rang", MaDanhMucCha = null, Slug = "ca-phe-hat-rang" };
                DanhMuc cafeXay = new DanhMuc { TenDanhMuc = "Cà phê xay", MaDanhMucCha = null, Slug = "ca-phe-xay" };
                DanhMuc cafePhinTruyenThong = new DanhMuc { TenDanhMuc = "Cà phê phin truyền thống", MaDanhMucCha = null, Slug = "ca-phe-phin-truyen-thong" };
                DanhMuc cafeHoaTan = new DanhMuc { TenDanhMuc = "Cà phê hòa tan", MaDanhMucCha = null, Slug = "ca-phe-hoa-tan" };

                await _context.DanhMucs.AddRangeAsync(cafeHatRang, cafeXay, cafePhinTruyenThong, cafeHoaTan);
                await _context.SaveChangesAsync();
            }

            if (!_context.SanPhams.Any())
            {
                var cafeXay = _context.DanhMucs.FirstOrDefault(c => c.TenDanhMuc == "Cà phê xay")?.MaDanhMuc ?? 0;
                var cafeHatRang = _context.DanhMucs.FirstOrDefault(c => c.TenDanhMuc == "Cà phê hạt rang")?.MaDanhMuc ?? 0;
                var cafePhinTruyenThong = _context.DanhMucs.FirstOrDefault(c => c.TenDanhMuc == "Cà phê phin truyền thống")?.MaDanhMuc ?? 0;
                var cafeHoaTan = _context.DanhMucs.FirstOrDefault(c => c.TenDanhMuc == "Cà phê hòa tan")?.MaDanhMuc ?? 0;

                var TrungNguyen = _context.ThuongHieus.FirstOrDefault(b => b.TenThuongHieu == "Trung Nguyên")?.MaThuongHieu ?? 0;
                var HighlandsCoffee = _context.ThuongHieus.FirstOrDefault(b => b.TenThuongHieu == "Highlands Coffee")?.MaThuongHieu ?? 0;
                var TheCoffeeHouse = _context.ThuongHieus.FirstOrDefault(b => b.TenThuongHieu == "The Coffee House")?.MaThuongHieu ?? 0;
                var G7 = _context.ThuongHieus.FirstOrDefault(b => b.TenThuongHieu == "G7")?.MaThuongHieu ?? 0;
                var Deva = _context.ThuongHieus.FirstOrDefault(b => b.TenThuongHieu == "Deva")?.MaThuongHieu ?? 0;

                await _context.SanPhams.AddRangeAsync(
                    new SanPham
                    {
                        MaSanPhamCode = "001",
                        TenSanPham = "Cà phê G7 3in1 - Bịch 100 sticks 16gr",
                        HinhAnh = "/images/f3c215c1-ef5f-4616-8fc2-94d180578d8d.jpg",
                        MaDanhMuc = cafeHoaTan,
                        MaThuongHieu = G7,
                        Gia = (double)365000m,
                        GiaKhuyenMai = 300000,
                        MoTaNgan = "Thành phần:Cà phê - sữa - đường.",
                        MoTa = "G7 là sản phẩm cà phê hòa tan duy nhất được chọn phục vụ các nguyên thủ quốc gia tại APEC, ASEM 5",
                        SoLuong = 1000,
                        TrangThai = 1,
                        DaXoa = 0,
                        NgayTao = new DateTime(2025, 7, 8, 2, 58, 25, 611),
                        NgayCapNhat = new DateTime(2025, 7, 8, 2, 59, 13, 556),
                        Slug = "ca-phe-g7-3in1-bich-100-sticks-16gr"
                    },
                    new SanPham
                    {
                        MaSanPhamCode = "002",
                        TenSanPham = "Trung Nguyên Legend Special Edition hộp 18 sticks",
                        HinhAnh = "/images/e3139945-6ff1-439e-8753-ed3c2476c825.jpg",
                        MaDanhMuc = cafeHoaTan,
                        MaThuongHieu = TrungNguyen,
                        Gia = (double)131000m,
                        GiaKhuyenMai = 120000,
                        MoTaNgan = "Cà phê sữa hòa tan (cà phê, đường, sữa)",
                        MoTa = "2 vùng nguyên liệu tốt nhất tại Việt Nam: Arabica Cầu Đất + Robusta Buôn Ma Thuột. Rang đậm theo kiểu rang truyền thống. Huyền bí phương Đông không thể sao chép (công thức riêng của Trung Nguyên). Công nghệ Nano giữ hương vị cà phê tươi nguyên trọn vẹn.",
                        SoLuong = 1000,
                        TrangThai = 1,
                        DaXoa = 0,
                        NgayTao = new DateTime(2025, 7, 8, 3, 1, 6, 885),
                        NgayCapNhat = new DateTime(2025, 7, 8, 3, 1, 6, 885),
                        Slug = "trung-nguyen-legend-special-edition-hop-18-sticks"
                    },
                    new SanPham
                    {
                        MaSanPhamCode = "003",
                        TenSanPham = "Trung Nguyên Legend Success 3 - Thùng 12 lon",
                        HinhAnh = "/images/cd14e4ac-3a1d-4036-bdbe-ab88314bed59.jpg",
                        MaDanhMuc = cafeHatRang,
                        MaThuongHieu = TrungNguyen,
                        Gia = (double)3447000m,
                        GiaKhuyenMai = 2930000,
                        MoTaNgan = "Là loại cà phê siêu hạng có hương vị độc đáo và đầy thử thách.",
                        MoTa = "Đặc tính: Thể chất mạnh, Vị cân bằng. Thành phần: Cà phê (Arabica 90%, Robusta 10%). Hướng dẫn sử dụng: Xay cà phê trước khi pha chế, sản phẩm phù hợp tất cả các kiểu pha",
                        SoLuong = 1000,
                        TrangThai = 1,
                        DaXoa = 0,
                        NgayTao = new DateTime(2025, 7, 8, 3, 3, 55, 466),
                        NgayCapNhat = new DateTime(2025, 7, 8, 3, 3, 55, 466),
                        Slug = "trung-nguyen-legend-success-3-thung-12-lon"
                    },
                    new SanPham
                    {
                        MaSanPhamCode = "004",
                        TenSanPham = "Cà phê Drip - Arabica Robusta hạt số 2 - 250gr",
                        HinhAnh = "/images/8386bc9f-b49b-42f8-b9cc-5a2476e8b3a1.jpg",
                        MaDanhMuc = cafeHatRang,
                        MaThuongHieu = Deva,
                        Gia = (double)85000.01m,
                        GiaKhuyenMai = null,
                        MoTaNgan = "Cà phê hạt xay Drip - Arabica Robusta khi pha có màu đen nhạt",
                        MoTa = "Với tinh thần đam mê sống chết với cà phê, Trung Nguyên luôn tạo ra sản phẩm tuyệt hảo nhất, được sản xuất trên công nghệ hàng đầu và bí quyết không thể sao chép. Cà phê hạt xay Drip - Arabica Robusta mang những ly cà phê với mùi vị đặc trưng cân bằng giữa vị chua nhẹ của hạt Arabica và vị đắng gắt của Robusta.",
                        SoLuong = 1000,
                        TrangThai = 1,
                        DaXoa = 0,
                        NgayTao = new DateTime(2025, 7, 8, 3, 6, 12, 835),
                        NgayCapNhat = new DateTime(2025, 7, 8, 3, 6, 12, 835),
                        Slug = "ca-phe-drip-arabica-robusta-hat-so-2-250gr"
                    },
                    new SanPham
                    {
                        MaSanPhamCode = "0051",
                        TenSanPham = "1Kg Cà phê Robusta Truyền thống nguyên chất rang xay",
                        HinhAnh = "/images/7a2104a2-6b9e-46ad-8fb9-6f7f31ae9816.png",
                        MaDanhMuc = cafeXay,
                        MaThuongHieu = Deva,
                        Gia = (double)290000m,
                        GiaKhuyenMai = 248000,
                        MoTaNgan = "Thành Phẩm bột pha phin",
                        MoTa = "Cà Phê Robusta Thượng Hạng Nam Nguyên Coffee. Trải nghiệm hương vị cà phê Robusta đích thực với Cà Phê Robusta Thượng Hạng Nam Nguyên Coffee - sự lựa chọn hoàn hảo cho những người yêu thích vị đắng đậm.",
                        SoLuong = 1000,
                        TrangThai = 1,
                        DaXoa = 0,
                        NgayTao = new DateTime(2025, 7, 8, 13, 11, 1, 27),
                        NgayCapNhat = new DateTime(2025, 7, 8, 13, 11, 1, 27),
                        Slug = "1kg-ca-phe-robusta-truyen-thong-nguyen-chat-rang-xay"
                    },
                    new SanPham
                    {
                        MaSanPhamCode = "00s1",
                        TenSanPham = "Cà Phê Chế Phin 3",
                        HinhAnh = "/images/482763d8-992d-40e5-8be4-90d4542c7cc3.png",
                        MaDanhMuc = cafePhinTruyenThong,
                        MaThuongHieu = TrungNguyen,
                        Gia = (double)207000m,
                        GiaKhuyenMai = null,
                        MoTaNgan = "Là những sản phẩm được phối trộn từ những hạt cà phê ngon nhất.",
                        MoTa = "Nhà sản xuất:Trung Nguyên. Thành phần:Chế biến từ những hạt cà phê Arabica sẻ. Đặc tính: Nước pha màu nâu cánh gián nhạt. Mùi thơm nồng. Vị đắng hơi chua, thể chất nhẹ vừa phải. Hàm lượng Caffeine: khoảng 1.7%. Ngon nhất khi uống với sữa. Khối lượng: Bịch 500gr.",
                        SoLuong = 1000,
                        TrangThai = 1,
                        DaXoa = 0,
                        NgayTao = new DateTime(2025, 7, 8, 13, 15, 53, 32),
                        NgayCapNhat = new DateTime(2025, 7, 8, 13, 16, 2, 747),
                        Slug = "ca-phe-che-phin-3"
                    },
                    new SanPham
                    {
                        MaSanPhamCode = "009",
                        TenSanPham = "G7 GOLD PICASSO",
                        HinhAnh = "/images/907ae832-9569-472b-a355-c016612141b9.jpg",
                        MaDanhMuc = cafeHoaTan,
                        MaThuongHieu = G7,
                        Gia = (double)110000m,
                        GiaKhuyenMai = 89000,
                        MoTaNgan = "Mang phong vị của Ly cà phê Picasso Latte tại không gian Thế giới Cà phê Trung Nguyên Legend",
                        MoTa = "Thêm thông tin Mã sản phẩm 5000984 Thương hiệu Trung Nguyên Legend Đơn vị tính Hộp Quy cách đóng thùng 12 hộp / thùng Hạn sử dụng 2 năm kể từ ngày sản xuất",
                        SoLuong = 1000,
                        TrangThai = 1,
                        DaXoa = 0,
                        NgayTao = new DateTime(2025, 7, 8, 18, 29, 27, 405),
                        NgayCapNhat = new DateTime(2025, 7, 8, 18, 29, 27, 405),
                        Slug = "g7-gold-picasso"
                    },
                    new SanPham
                    {
                        MaSanPhamCode = "00s6",
                        TenSanPham = "Hộp NÂU - THE COFFEE HOUSE 220g / 10 gói - CÀ PHÊ SỮA ĐÁ HÒA TAN / 3 in 1 Instant Milk Coffee",
                        HinhAnh = "/images/8cea43b3-d374-48be-90a9-1f60768dc365.webp",
                        MaDanhMuc = cafeHoaTan,
                        MaThuongHieu = Deva,
                        Gia = (double)70000m,
                        GiaKhuyenMai = 52000,
                        MoTaNgan = "Thương hiệu: The Coffee House",
                        MoTa = "Thương hiệu The Coffee House Xuất xứ Việt Nam Dạng đồ uống",
                        SoLuong = 1000,
                        TrangThai = 0,
                        DaXoa = 1,
                        NgayTao = new DateTime(2025, 7, 8, 18, 37, 34, 667),
                        NgayCapNhat = new DateTime(2025, 7, 9, 16, 25, 38, 986),
                        Slug = "hop-nau-the-coffee-house-220g-10-goi-ca-phe-sua-da-hoa-tan-3-in-1-instant-milk-coffee"
                    },
                    new SanPham
                    {
                        MaSanPhamCode = "sp1",
                        TenSanPham = "Hộp NÂU - THE COFFEE HOUSE 220g / 10 gói - CÀ PHÊ SỮA ĐÁ HÒA TAN / 3 in 1 Instant Milk Coffee",
                        HinhAnh = "/images/08f3c57d-6aba-48d3-af9f-5d849d5c348c.webp",
                        MaDanhMuc = cafeHoaTan,
                        MaThuongHieu = TheCoffeeHouse,
                        Gia = (double)110000m,
                        GiaKhuyenMai = 90000,
                        MoTaNgan = "Thương hiệu The Coffee House",
                        MoTa = "Xuất xứ Việt Nam Dạng đồ uống 3 in 1 & hòa tan Hạn sử dụng 12 tháng. Thương hiệu: The Coffee House. Xuất xứ: Việt Nam. Trọng lượng: 220g (10 gói x 22g). Hướng dẫn sử dụng: dùng pha thức uống. Hạn sử dụng: 12 tháng kể từ ngày sản xuất. Bảo quản: nơi khô ráo, thoáng mát, tránh ánh nắng mặt trời",
                        SoLuong = 1000,
                        TrangThai = 1,
                        DaXoa = 0,
                        NgayTao = new DateTime(2025, 7, 8, 18, 49, 22, 276),
                        NgayCapNhat = new DateTime(2025, 7, 9, 16, 8, 44, 481),
                        Slug = "hop-nau-the-coffee-house-220g-10-goi-ca-phe-sua-da-hoa-tan-3-in-1-instant-milk-coffee-1"
                    },
                    new SanPham
                    {
                        MaSanPhamCode = "344",
                        TenSanPham = "cà phê chữ I",
                        HinhAnh = "/images/8fbef523-a07a-4190-a7a7-a71ddf30fad3.jpg",
                        MaDanhMuc = cafePhinTruyenThong,
                        MaThuongHieu = TrungNguyen,
                        Gia = (double)107000m,
                        GiaKhuyenMai = 100000,
                        MoTaNgan = "Loại: Cà phê phin",
                        MoTa = "Tên: Cà phê Khát Vọng chữ I – 500gr. Loại: Cà phê phin. Đặc tính nổi bật: Vị êm nhẹ, ít đắng, mùi thơm đặc trưng với nước pha có màu nâu cánh gián đậm. Khối lượng tịnh hộp: 500gr/gói. Thành phần chính: Arabica, Robusta, Excelsa, Catimor. Hàm lượng Caffeine: 2.0%.",
                        SoLuong = 1000,
                        TrangThai = 1,
                        DaXoa = 0,
                        NgayTao = new DateTime(2025, 7, 9, 9, 36, 4, 239),
                        NgayCapNhat = new DateTime(2025, 7, 9, 9, 36, 4, 239),
                        Slug = "ca-phe-chu-i"
                    },
                    new SanPham
                    {
                        MaSanPhamCode = "fa3",
                        TenSanPham = "Cà phê LEGEND",
                        HinhAnh = "/images/f1928900-dea7-4e89-b81d-61a77f11895d.jpg",
                        MaDanhMuc = cafePhinTruyenThong,
                        MaThuongHieu = TrungNguyen,
                        Gia = (double)1300000m,
                        GiaKhuyenMai = null,
                        MoTaNgan = "Nhà sản xuất: Trung Nguyên",
                        MoTa = "Nhà sản xuất: Trung Nguyên. Thành phần: Arabica, Robusta, Excelsa. Đặc điểm: LEGEND là cà phê chồn được sản xuất bằng phương pháp “Lên men sinh học”, sản phẩm chỉ có duy nhất ở Trung Nguyên. Hạn sử dụng: 2 năm. Khối lượng: Hộp 225gr.",
                        SoLuong = 1000,
                        TrangThai = 1,
                        DaXoa = 0,
                        NgayTao = new DateTime(2025, 7, 9, 10, 0, 12, 23),
                        NgayCapNhat = new DateTime(2025, 7, 9, 10, 0, 12, 23),
                        Slug = "ca-phe-legend"
                    },
                    new SanPham
                    {
                        MaSanPhamCode = "gf2",
                        TenSanPham = "Cà phê S (Chinh phục)",
                        HinhAnh = "/images/9b8ff18d-9713-40fe-bef0-f1060022a5dc.jpg",
                        MaDanhMuc = cafePhinTruyenThong,
                        MaThuongHieu = TrungNguyen,
                        Gia = (double)100000m,
                        GiaKhuyenMai = 90000,
                        MoTaNgan = "Nhà sản xuất:Trung Nguyên",
                        MoTa = "Hương vị cà phê đặc trưng với sự kết hợp của 4 loại hạt cà phê: Arabica, Robusta, Excelsa, Catimor theo tỉ lệ phối trộn đặc biệt. Cà phê Trung Nguyên S có màu nước nâu sánh, hương thơm đầy, vị đậm đà. Khối lượng: Bịch 500gr. Phù hợp với mọi cách uống.",
                        SoLuong = 1000,
                        TrangThai = 1,
                        DaXoa = 0,
                        NgayTao = new DateTime(2025, 7, 9, 10, 3, 10, 848),
                        NgayCapNhat = new DateTime(2025, 7, 9, 10, 3, 55, 381),
                        Slug = "ca-phe-s-chinh-phuc"
                    },
                    new SanPham
                    {
                        MaSanPhamCode = "ge3",
                        TenSanPham = "Cà Phê Rang Xay Original 1 250G",
                        HinhAnh = "/images/2680fdf6-2181-4f67-9955-b259f1d22707.jpg",
                        MaDanhMuc = cafeXay,
                        MaThuongHieu = TheCoffeeHouse,
                        Gia = (double)82000m,
                        GiaKhuyenMai = null,
                        MoTaNgan = "Cà phê Original 1 của The Coffee House với thành phần chính cà phê Robusta Đắk Lắk",
                        MoTa = "Cà phê Original 1 của The Coffee House với thành phần chính cà phê Robusta Đắk Lắk, vùng trồng cà phê nổi tiếng nhất Việt Nam. Bằng cách áp dụng kỹ thuật rang xay hiện đại, Cà phê Original 1 mang đến trải nghiệm tuyệt vời khi uống cà phê tại nhà với hương vị đậm đà truyền thống hợp khẩu vị của giới trẻ sành cà phê.",
                        SoLuong = 1000,
                        TrangThai = 1,
                        DaXoa = 0,
                        NgayTao = new DateTime(2025, 7, 9, 13, 48, 57, 314),
                        NgayCapNhat = new DateTime(2025, 7, 9, 13, 49, 15, 545),
                        Slug = "ca-phe-rang-xay-original-1-250g"
                    },
                    new SanPham
                    {
                        MaSanPhamCode = "CPS001",
                        TenSanPham = "Cà Phê Sữa Đá Hòa Tan Túi 25x22G",
                        HinhAnh = "/images/a6950e93-e2b5-4edf-ba8a-71efcf51e656.png",
                        MaDanhMuc = cafeHoaTan,
                        MaThuongHieu = TheCoffeeHouse,
                        Gia = (double)127000m,
                        GiaKhuyenMai = null,
                        MoTaNgan = "Sản phẩm chất lượng hàng đầu",
                        MoTa = "Thật dễ dàng để bắt đầu ngày mới với tách cà phê sữa đá sóng sánh, thơm ngon như cà phê pha phin. Vị đắng thanh của cà phê hòa quyện với vị ngọt béo của sữa, giúp bạn luôn tỉnh táo và hứng khởi cho ngày làm việc thật hiệu quả.",
                        SoLuong = 100,
                        TrangThai = 1,
                        DaXoa = 0,
                        NgayTao = new DateTime(2025, 7, 9, 16, 35, 56, 539),
                        NgayCapNhat = new DateTime(2025, 7, 9, 16, 35, 56, 539),
                        Slug = "ca-phe-sua-da-hoa-tan-tui-25x22g"
                    },
                    new SanPham
                    {
                        MaSanPhamCode = "PVN28",
                        TenSanPham = "Cà Phê Bột Truyền Thống Highlands Coffee 1kg",
                        HinhAnh = "/images/1f067951-a498-4f4b-8a84-8fdb2e237628.jpg",
                        MaDanhMuc = cafeXay,
                        MaThuongHieu = HighlandsCoffee,
                        Gia = (double)396000m,
                        GiaKhuyenMai = null,
                        MoTaNgan = "sản phẩm chất lượng",
                        MoTa = "Những hạt cà phê thượng hạng trồng ở vùng cao nguyên của Việt Nam được rang xay và phối trộn theo công thức độc đáo tại Highlands Coffee.",
                        SoLuong = 1000,
                        TrangThai = 1,
                        DaXoa = 0,
                        NgayTao = new DateTime(2025, 7, 9, 16, 39, 7, 230),
                        NgayCapNhat = new DateTime(2025, 7, 9, 16, 39, 20, 901),
                        Slug = "ca-phe-bot-truyen-thong-highlands-coffee-1kg"
                    },
                    new SanPham
                    {
                        MaSanPhamCode = "0014",
                        TenSanPham = "Cà Phê Hạt Full City Roast Highlands Coffee 1kg",
                        HinhAnh = "/images/2edabb65-577f-4414-bec5-811c38aee4e1.jpg",
                        MaDanhMuc = cafeHatRang,
                        MaThuongHieu = HighlandsCoffee,
                        Gia = (double)300000m,
                        GiaKhuyenMai = null,
                        MoTaNgan = "Khám phá sự tinh tế trong từng hạt cà phê, mang đến trải nghiệm cà phê đậm đà, thơm ngon.",
                        MoTa = "Thông Số Sản Phẩm: Thương hiệu: Highlands Coffee Xuất xứ thương hiệu: Việt Nam Trọng lượng: 1 kg Loại: Cà phê Hạt",
                        SoLuong = 100,
                        TrangThai = 1,
                        DaXoa = 0,
                        NgayTao = new DateTime(2025, 7, 9, 16, 41, 55, 287),
                        NgayCapNhat = new DateTime(2025, 7, 9, 16, 41, 55, 287),
                        Slug = "ca-phe-hat-full-city-roast-highlands-coffee-1kg"
                    },
                    new SanPham
                    {
                        MaSanPhamCode = "CFRBT001",
                        TenSanPham = "Cà phê Robusta rang đậm xay sẵn Traditional 250g",
                        HinhAnh = "/images/6464a51c-08d0-4c9b-bcc6-d17400a2b9a4.jpg",
                        MaDanhMuc = cafeHatRang,
                        MaThuongHieu = Deva,
                        Gia = (double)120000m,
                        GiaKhuyenMai = null,
                        MoTaNgan = "xuất xứ: Việt Nam",
                        MoTa = "Mô tả chung: Cà phê Robusta rang đậm xay sẵn Traditional 250g 'Tôn vinh hương vị 'Cà phê phin' thuần Việt với Traditional Phin Blend, chúng tôi chọn rang những hạt Robusta đặc biệt, giúp làm nổi bật hương vị ca cao và vị hạt rang tạo cảm vị sánh mịn trong khoang miệng.",
                        SoLuong = 123,
                        TrangThai = 1,
                        DaXoa = 0,
                        NgayTao = new DateTime(2025, 7, 9, 16, 44, 6, 891),
                        NgayCapNhat = new DateTime(2025, 7, 9, 16, 44, 6, 891),
                        Slug = "ca-phe-robusta-rang-dam-xay-san-traditional-250g"
                    },
                    new SanPham
                    {
                        MaSanPhamCode = "CF002",
                        TenSanPham = "Cà Phê Phin Giấy Trung Nguyên Legend VietNamese Blend",
                        HinhAnh = "/images/9def7db5-7490-4fa5-8464-540bcbfd3a30.png",
                        MaDanhMuc = cafeXay,
                        MaThuongHieu = TrungNguyen,
                        Gia = (double)150000m,
                        GiaKhuyenMai = null,
                        MoTaNgan = "Sản phẩm Việt Nam Chất Lượng Cao",
                        MoTa = "Tuyệt phẩm Cà Phê Phin giấy Trung Nguyên Legend Vietnamese Blend với phong vị đậm đà, hương thơm đặc trưng của cà phê Việt Nam truyền thống sẽ mang đến bạn sự tỉnh thức mạnh mẽ cho những ý tưởng sáng tạo đột phá.",
                        SoLuong = 2000,
                        TrangThai = 1,
                        DaXoa = 0,
                        NgayTao = new DateTime(2025, 7, 9, 16, 46, 13, 385),
                        NgayCapNhat = new DateTime(2025, 7, 9, 16, 46, 13, 385),
                        Slug = "ca-phe-phin-giay-trung-nguyen-legend-vietnamese-blend"
                    },
                    new SanPham
                    {
                        MaSanPhamCode = "CPS003",
                        TenSanPham = "Legend Special Edition – Hộp 18 Gói",
                        HinhAnh = "/images/875085aa-051a-41ef-b7e6-8d6999de4519.png",
                        MaDanhMuc = cafeHoaTan,
                        MaThuongHieu = TrungNguyen,
                        Gia = (double)145000m,
                        GiaKhuyenMai = null,
                        MoTaNgan = "Sản phẩm chất lượng",
                        MoTa = "2 vùng nguyên liệu tốt nhất tại Việt Nam: Arabica Cầu Đất + Robusta Buôn Ma Thuột. Rang đậm theo kiểu rang truyền thống. Huyền bí phương Đông không thể sao chép (công thức riêng của Trung Nguyên). Công nghệ Nano giữ hương vị cà phê tươi nguyên trọn vẹn. Sữa đặc sấy phun được phát triển riêng cho Trung Nguyên. Hỗn hợp cà phê (cà phê hòa tan và cà phê rang xay mịn) 20%.",
                        SoLuong = 1400,
                        TrangThai = 0,
                        DaXoa = 1,
                        NgayTao = new DateTime(2025, 7, 9, 16, 47, 40, 883),
                        NgayCapNhat = new DateTime(2025, 7, 9, 18, 9, 45, 913),
                        Slug = "legend-special-edition-hop-18-goi"
                    },
                    new SanPham
                    {
                        MaSanPhamCode = "CPS004",
                        TenSanPham = "Cà phê Robusta PREMIUM",
                        HinhAnh = "/images/31766e69-d978-4252-8b1c-c75ca51d8225.jpg",
                        MaDanhMuc = cafePhinTruyenThong,
                        MaThuongHieu = Deva,
                        Gia = (double)265000m,
                        GiaKhuyenMai = null,
                        MoTaNgan = "Sản phẩm đóng gói tiện lợi",
                        MoTa = "Cà phê Robusta PREMIUM rang xay lại đặc biệt của Deva có hàm lượng cafein cao, giá thành vừa túi tiền và loại cà phê yêu thích, gắn bó với người uống bao năm qua. Cà phê Robusta PREMIUM rang xay của Deva dưới bàn tay lành nghề của người thợ rang có vị đậm đà đặc trưng, mang lại cảm xúc mạnh mẽ cùng hương cà phê nhẹ nhè.",
                        SoLuong = 300,
                        TrangThai = 0,
                        DaXoa = 1,
                        NgayTao = new DateTime(2025, 7, 9, 16, 50, 23, 679),
                        NgayCapNhat = new DateTime(2025, 7, 9, 18, 9, 32, 116),
                        Slug = "ca-phe-robusta-premium"
                    }
                );
                await _context.SaveChangesAsync();

                await _context.HinhAnhSanPhams.AddRangeAsync(
                    new HinhAnhSanPham { MaSanPham = 1, HinhAnh = "/images/f3c215c1-ef5f-4616-8fc2-94d180578d8d.jpg" },
                    new HinhAnhSanPham { MaSanPham = 2, HinhAnh = "/images/e3139945-6ff1-439e-8753-ed3c2476c825.jpg" },
                    new HinhAnhSanPham { MaSanPham = 3, HinhAnh = "/images/cd14e4ac-3a1d-4036-bdbe-ab88314bed59.jpg" },
                    new HinhAnhSanPham { MaSanPham = 4, HinhAnh = "/images/8386bc9f-b49b-42f8-b9cc-5a2476e8b3a1.jpg" },
                    new HinhAnhSanPham { MaSanPham = 5, HinhAnh = "/images/7a2104a2-6b9e-46ad-8fb9-6f7f31ae9816.png" },
                    new HinhAnhSanPham { MaSanPham = 6, HinhAnh = "/images/482763d8-992d-40e5-8be4-90d4542c7cc3.png" },
                    new HinhAnhSanPham { MaSanPham = 7, HinhAnh = "/images/907ae832-9569-472b-a355-c016612141b9.jpg" },
                    new HinhAnhSanPham { MaSanPham = 8, HinhAnh = "/images/8cea43b3-d374-48be-90a9-1f60768dc365.webp" },
                    new HinhAnhSanPham { MaSanPham = 9, HinhAnh = "/images/08f3c57d-6aba-48d3-af9f-5d849d5c348c.webp" },
                    new HinhAnhSanPham { MaSanPham = 10, HinhAnh = "/images/8fbef523-a07a-4190-a7a7-a71ddf30fad3.jpg" },
                    new HinhAnhSanPham { MaSanPham = 11, HinhAnh = "/images/f1928900-dea7-4e89-b81d-61a77f11895d.jpg" },
                    new HinhAnhSanPham { MaSanPham = 12, HinhAnh = "/images/9b8ff18d-9713-40fe-bef0-f1060022a5dc.jpg" },
                    new HinhAnhSanPham { MaSanPham = 13, HinhAnh = "/images/2680fdf6-2181-4f67-9955-b259f1d22707.jpg" },
                    new HinhAnhSanPham { MaSanPham = 14, HinhAnh = "/images/a6950e93-e2b5-4edf-ba8a-71efcf51e656.png" },
                    new HinhAnhSanPham { MaSanPham = 15, HinhAnh = "/images/1f067951-a498-4f4b-8a84-8fdb2e237628.jpg" },
                    new HinhAnhSanPham { MaSanPham = 16, HinhAnh = "/images/2edabb65-577f-4414-bec5-811c38aee4e1.jpg" },
                    new HinhAnhSanPham { MaSanPham = 17, HinhAnh = "/images/6464a51c-08d0-4c9b-bcc6-d17400a2b9a4.jpg" },
                    new HinhAnhSanPham { MaSanPham = 18, HinhAnh = "/images/9def7db5-7490-4fa5-8464-540bcbfd3a30.png" },
                    new HinhAnhSanPham { MaSanPham = 19, HinhAnh = "/images/875085aa-051a-41ef-b7e6-8d6999de4519.png" },
                    new HinhAnhSanPham { MaSanPham = 20, HinhAnh = "/images/31766e69-d978-4252-8b1c-c75ca51d8225.jpg" }
                );
                await _context.SaveChangesAsync();
            }
            if (!_context.Sliders.Any())
            {
                Slider slider1 = new Slider { HinhAnh = "/images/banner1.png", TieuDe = "Ưu Đãi Đặc Biệt", TrangThai = true };
                Slider slider2 = new Slider { HinhAnh = "/images/banner2.png", TieuDe = "Sản Phẩm Mới",TrangThai = true };

                await _context.Sliders.AddRangeAsync(slider1, slider2);
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
                    new FooterLink { TieuDe = "Cà Phê Phin Truyền Thống", Url = "/ca-phe-phin-truyen-thong", MaNhom = 3, ThuTuHienThi = 3, TrangThai = true },
                    new FooterLink { TieuDe = "Cà Phê Hòa Tan", Url = "/ca-phe-hoa-tan", MaNhom = 3, ThuTuHienThi = 4, TrangThai = true }
                );
                await _context.SaveChangesAsync();
            }

            if (!_context.Sliders.Any())
            {
                await _context.Sliders.AddRangeAsync(
                    new Slider { HinhAnh = "slider1.jpg", TrangThai = true }, // Removed Url, TieuDe2, NoiDung
                    new Slider { HinhAnh = "slider2.jpg", TrangThai = true }   // Removed Url, TieuDe2, NoiDung
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
                    new TaiKhoan { TenDangNhap = "admin", MatKhau = "admin@123", MaVaiTro = 2 },
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
                    new KhachHang
                    {
                        HoTen = "Nguyễn Văn An",
                        SoDienThoai = "0901234567",
                        DiaChi = "123 Đường Lê Lợi, Phường Bến Nghé",
                        Tinh = "Hồ Chí Minh",
                        Huyen = "Quận 1",
                        Xa = "Phường Bến Nghé",
                        Email = "nguyenvanan@gmail.com",
                        TenHienThi = "An Nguyễn",
                        NgaySinh = new DateOnly(1990, 5, 15),
                        GioiTinh = true,
                        HinhDaiDien = "default-avatar.png",
                        MaTaiKhoan = 2
                    },
                    new KhachHang
                    {
                        HoTen = "Trần Thị Bình",
                        SoDienThoai = "0907654321",
                        DiaChi = "456 Đường Nguyễn Huệ, Phường Đakao",
                        Tinh = "Hồ Chí Minh",
                        Huyen = "Quận 1",
                        Xa = "Phường Đakao",
                        Email = "tranthibinh@gmail.com",
                        TenHienThi = "Bình Trần",
                        NgaySinh = new DateOnly(1988, 8, 22),
                        GioiTinh = false,
                        HinhDaiDien = "default-avatar.png",
                        MaTaiKhoan = 3
                    },
                    new KhachHang
                    {
                        HoTen = "Lê Minh Cường",
                        SoDienThoai = "0912345678",
                        DiaChi = "789 Đường Pasteur, Phường 6",
                        Tinh = "Hồ Chí Minh",
                        Huyen = "Quận 3",
                        Xa = "Phường 6",
                        Email = "leminhcuong@gmail.com",
                        TenHienThi = "Cường Lê",
                        NgaySinh = new DateOnly(1995, 3, 10),
                        GioiTinh = true,
                        HinhDaiDien = "default-avatar.png",
                        MaTaiKhoan = 4
                    },
                    new KhachHang
                    {
                        HoTen = "Phạm Thị Dung",
                        SoDienThoai = "0923456789",
                        DiaChi = "321 Đường Cách Mạng Tháng 8, Phường 10",
                        Tinh = "Hồ Chí Minh",
                        Huyen = "Quận 3",
                        Xa = "Phường 10",
                        Email = "phamthidung@gmail.com",
                        TenHienThi = "Dung Phạm",
                        NgaySinh = new DateOnly(1992, 12, 5),
                        GioiTinh = false,
                        HinhDaiDien = "default-avatar.png",
                        MaTaiKhoan = 5
                    },
                    new KhachHang
                    {
                        HoTen = "Hoàng Văn Em",
                        SoDienThoai = "0934567890",
                        DiaChi = "654 Đường Võ Văn Tần, Phường 6",
                        Tinh = "Hồ Chí Minh",
                        Huyen = "Quận 3",
                        Xa = "Phường 6",
                        Email = "hoangvanem@gmail.com",
                        TenHienThi = "Em Hoàng",
                        NgaySinh = new DateOnly(1993, 7, 18),
                        GioiTinh = true,
                        HinhDaiDien = "default-avatar.png",
                        MaTaiKhoan = 6
                    },
                    new KhachHang
                    {
                        HoTen = "Đỗ Thị Phương",
                        SoDienThoai = "0945678901",
                        DiaChi = "987 Đường Điện Biên Phủ, Phường 25",
                        Tinh = "Hồ Chí Minh",
                        Huyen = "Quận Bình Thạnh",
                        Xa = "Phường 25",
                        Email = "dothiphuong@gmail.com",
                        TenHienThi = "Phương Đỗ",
                        NgaySinh = new DateOnly(1991, 4, 25),
                        GioiTinh = false,
                        HinhDaiDien = "default-avatar.png",
                        MaTaiKhoan = 7
                    },
                    new KhachHang
                    {
                        HoTen = "Vũ Minh Giang",
                        SoDienThoai = "0956789012",
                        DiaChi = "147 Đường Phan Xích Long, Phường 2",
                        Tinh = "Hồ Chí Minh",
                        Huyen = "Quận Phú Nhuận",
                        Xa = "Phường 2",
                        Email = "vuminhgiang@gmail.com",
                        TenHienThi = "Giang Vũ",
                        NgaySinh = new DateOnly(1989, 11, 30),
                        GioiTinh = true,
                        HinhDaiDien = "default-avatar.png",
                        MaTaiKhoan = 8
                    },
                    new KhachHang
                    {
                        HoTen = "Bùi Thị Hương",
                        SoDienThoai = "0967890123",
                        DiaChi = "258 Đường Lý Thường Kiệt, Phường 14",
                        Tinh = "Hồ Chí Minh",
                        Huyen = "Quận 10",
                        Xa = "Phường 14",
                        Email = "buithihuong@gmail.com",
                        TenHienThi = "Hương Bùi",
                        NgaySinh = new DateOnly(1994, 6, 8),
                        GioiTinh = false,
                        HinhDaiDien = "default-avatar.png",
                        MaTaiKhoan = 9
                    },
                    new KhachHang
                    {
                        HoTen = "Ngô Văn Inh",
                        SoDienThoai = "0978901234",
                        DiaChi = "369 Đường Nguyễn Thị Minh Khai, Phường Đa Kao",
                        Tinh = "Hồ Chí Minh",
                        Huyen = "Quận 1",
                        Xa = "Phường Đa Kao",
                        Email = "ngovaninh@gmail.com",
                        TenHienThi = "Inh Ngô",
                        NgaySinh = new DateOnly(1987, 9, 12),
                        GioiTinh = true,
                        HinhDaiDien = "default-avatar.png",
                        MaTaiKhoan = 10
                    },
                    new KhachHang
                    {
                        HoTen = "Đinh Thị Kiều",
                        SoDienThoai = "0989012345",
                        DiaChi = "741 Đường Trần Hưng Đạo, Phường 1",
                        Tinh = "Hồ Chí Minh",
                        Huyen = "Quận 5",
                        Xa = "Phường 1",
                        Email = "dinhthikieu@gmail.com",
                        TenHienThi = "Kiều Đinh",
                        NgaySinh = new DateOnly(1996, 1, 20),
                        GioiTinh = false,
                        HinhDaiDien = "default-avatar.png",
                        MaTaiKhoan = 11
                    }
                );
                await _context.SaveChangesAsync();
            }
            if (!_context.HoaDons.Any())
            {
                var hoaDons = new List<HoaDon>
    {
        new HoaDon
        {
            MaKhachHang = 3,
            NgayDatHang = new DateTime(2025, 7, 10, 9, 30, 0),
            HoTen = "Nguyễn Văn A",
            SoDienThoai = "0123456789",
            Email = "a@example.com",
            DiaChi = "123 Đường ABC",
            Tinh = "Hà Nội",
            Huyen = "Đống Đa",
            Xa = "Phường Trung Liệt",
            PhuongThucThanhToan = "COD",
            TongTien = 1250000,
            TrangThai = 10
        },
        new HoaDon
        {
            MaKhachHang = 4,
            NgayDatHang = new DateTime(2025, 7, 11, 14, 15, 0),
            HoTen = "Trần Thị B",
            SoDienThoai = "0987654321",
            Email = "b@example.com",
            DiaChi = "456 Đường DEF",
            Tinh = "Hồ Chí Minh",
            Huyen = "Quận 1",
            Xa = "Phường Bến Nghé",
            PhuongThucThanhToan = "COD",
            TongTien = 900000,
            TrangThai = 10
        },
        new HoaDon
        {
            MaKhachHang = 5,
            NgayDatHang = new DateTime(2025, 7, 12, 19, 45, 0),
            HoTen = "Lê Văn C",
            SoDienThoai = "0111222333",
            Email = "c@example.com",
            DiaChi = "789 Đường XYZ",
            Tinh = "Đà Nẵng",
            Huyen = "Hải Châu",
            Xa = "Phường Hòa Cường",
            PhuongThucThanhToan = "COD",
            TongTien = 420000,
            TrangThai = 10
        }
    };

                await _context.HoaDons.AddRangeAsync(hoaDons);
                await _context.SaveChangesAsync();
            }

            if (!_context.ChiTietHoaDons.Any())
            {
                var hoaDonList = _context.HoaDons.OrderBy(h => h.NgayDatHang).ToList();

                await _context.ChiTietHoaDons.AddRangeAsync(
                    // Hóa đơn 1
                    new ChiTietHoaDon
                    {
                        MaHoaDon = hoaDonList[0].MaHoaDon,
                        MaSanPham = 1,
                        SoLuong = 50,
                        Gia = 25000,
                        TongTien = 50 * 25000
                    },
                    // Hóa đơn 2
                    new ChiTietHoaDon
                    {
                        MaHoaDon = hoaDonList[1].MaHoaDon,
                        MaSanPham = 2,
                        SoLuong = 30,
                        Gia = 30000,
                        TongTien = 30 * 30000
                    },
                    // Hóa đơn 3
                    new ChiTietHoaDon
                    {
                        MaHoaDon = hoaDonList[2].MaHoaDon,
                        MaSanPham = 3,
                        SoLuong = 21,
                        Gia = 20000,
                        TongTien = 21 * 20000
                    }
                );

                await _context.SaveChangesAsync();
            }
        }
    }
}