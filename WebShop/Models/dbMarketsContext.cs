using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace WebShop.Models;

public partial class DbMarketsContext : DbContext
{
    public DbMarketsContext()
    {
    }

    public DbMarketsContext(DbContextOptions<DbMarketsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

    public virtual DbSet<DiaChiTaiKhoan> DiaChiTaiKhoans { get; set; }

    public virtual DbSet<ThuocTinh> Attributes { get; set; }

    public virtual DbSet<ThuocTinhTiemNang> AttributesPotentails { get; set; }

    public virtual DbSet<GiaThuocTinh> AttributesPrices { get; set; }

    public virtual DbSet<BangQuangCao> Banners { get; set; }

    public virtual DbSet<ThuongHieu> Brands { get; set; }

    public virtual DbSet<NhomThuongHieu> BrandGroups { get; set; }

    public virtual DbSet<MauThe> CardTemplates { get; set; }

    public virtual DbSet<DanhMuc> Categories { get; set; }

    public virtual DbSet<DanhMucThuocTinh> CategoryAttributes { get; set; }

    public virtual DbSet<DanhMucThuongHieu> CategoryBrands { get; set; }

    public virtual DbSet<TrangThaiThanhToan> Codstatuses { get; set; }

    public virtual DbSet<KhachHang> Customers { get; set; }

    public virtual DbSet<KhachHangThuongHieu> CustomerBrands { get; set; }

    public virtual DbSet<KhachHangTiemNang> CustomerPotentails { get; set; }

    public virtual DbSet<DuAnKhachHang> CustomerProjects { get; set; }

    public virtual DbSet<NhaCungCapKhachHang> CustomerSuppliers { get; set; }

    public virtual DbSet<TrangThaiGiaoHang> DeliveryStatuses { get; set; }

    public virtual DbSet<KhuyenMai> Discounts { get; set; }

    public virtual DbSet<KhuyenMaiThemKhachHang> DiscountAddCustomers { get; set; }

    public virtual DbSet<KhuyenMaiThemSanPham> DiscountAddProducts { get; set; }

    public virtual DbSet<Huyen> Districts { get; set; }

    public virtual DbSet<ThuocTinhEmail> EmailAttributes { get; set; }

    public virtual DbSet<EmailTiepThi> EmailMakettings { get; set; }

    public virtual DbSet<TrangFacebook> FacebookPages { get; set; }

    public virtual DbSet<ThuocTinhQuaTang> GitAttributes { get; set; }

    public virtual DbSet<KhachVangLai> Guests { get; set; }

    public virtual DbSet<LichSuKhuyenMai> HistoryDiscounts { get; set; }

    public virtual DbSet<MayChuHinhAnh> ImageServers { get; set; }

    public virtual DbSet<CapDoKhachHang> LeverCustommerPtts { get; set; }

    public virtual DbSet<DonHang> Orders { get; set; }

    public virtual DbSet<ChiTietDonHang> OrderDetails { get; set; }

    public virtual DbSet<TrangWeb> Pages { get; set; }

    public virtual DbSet<ThongTinTrang> PageInfos { get; set; }

    public virtual DbSet<TrangThaiThanhToan> PaymentStatuses { get; set; }

    public virtual DbSet<BaiViet> Posts { get; set; }

    public virtual DbSet<DanhMucBaiViet> PostCategories { get; set; }

    public virtual DbSet<SanPham> Products { get; set; }

    public virtual DbSet<SanPhamThemKhachHang> ProductAddCusPros { get; set; }

    public virtual DbSet<DanhMucSanPham> ProductCategories { get; set; }

    public virtual DbSet<SanPhamFacebook> ProductFacebooks { get; set; }

    public virtual DbSet<QuaTangSanPham> ProductGifts { get; set; }

    public virtual DbSet<AnhSanPham> ProductThumbs { get; set; }

    public virtual DbSet<Tinh> Provinces { get; set; }

    public virtual DbSet<QuangCao> QuangCaos { get; set; }

    public virtual DbSet<BaoGia> Quotations { get; set; }

    public virtual DbSet<ChiTietBaoGia> QuotationDetails { get; set; }

    public virtual DbSet<VaiTro> VaiTros { get; set; }

    public virtual DbSet<NguoiGiaoHang> Shippers { get; set; }

    public virtual DbSet<DiaChiGiaoHang> ShippingAddresses { get; set; }

    public virtual DbSet<TrangTrinhBay> Slides { get; set; }

    public virtual DbSet<HeThongWeb> SystemWebs { get; set; }

    public virtual DbSet<PhimAnh> Videos { get; set; }

    public virtual DbSet<Xa> Wards { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("NovazoneConnectionString"));
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.Property(e => e.MaTaiKhoan).HasColumnName("MaTaiKhoan");
            entity.Property(e => e.NgayTao).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.HoTen).HasMaxLength(150);
            entity.Property(e => e.LanDangNhapCuoi).HasColumnType("datetime");
            entity.Property(e => e.MatKhau).HasMaxLength(50);
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.MaVaiTro).HasColumnName("MaVaiTro");
            entity.Property(e => e.Salt)
                .HasMaxLength(10)
                .IsFixedLength();

            entity.HasOne(d => d.VaiTro).WithMany(p => p.TaiKhoans)
                .HasForeignKey(d => d.MaVaiTro)
                .HasConstraintName("FK_Accounts_Roles");
        });

        modelBuilder.Entity<DiaChiTaiKhoan>(entity =>
        {
            entity.HasKey(e => e.MaDiaChi).HasName("PK__AccountA__091C2A1B7AAC9A99");

            entity.ToTable("AccountAddress");

            entity.Property(e => e.MaDiaChi).HasColumnName("AddressID");
            entity.Property(e => e.NoiDung).HasMaxLength(50);
            entity.Property(e => e.MaKhachHang).HasColumnName("CustomerID");
            entity.Property(e => e.MaHuyen).HasColumnName("DistrictID");
            entity.Property(e => e.MaKhachVangLai).HasColumnName("GuestID");
            entity.Property(e => e.SoDienThoai).HasMaxLength(10);
            entity.Property(e => e.MaTinh).HasColumnName("ProvinceID");
            entity.Property(e => e.TenNguoiDung).HasMaxLength(20);
            entity.Property(e => e.MaXa).HasColumnName("WardID");

            entity.HasOne(d => d.KhachHang).WithMany(p => p.AccountAddresses)
                .HasForeignKey(d => d.MaKhachHang)
                .HasConstraintName("FK_AccountAddress_Customers");

            entity.HasOne(d => d.Tinh).WithMany(p => p.AccountAddresses)
                .HasForeignKey(d => d.MaTinh)
                .HasConstraintName("FK_AccountAddress_Districts");

            entity.HasOne(d => d.KhachVangLai).WithMany(p => p.AccountAddresses)
                .HasForeignKey(d => d.MaKhachVangLai)
                .HasConstraintName("FK_AccountAddress_Guests");

            entity.HasOne(d => d.Tinh).WithMany(p => p.AccountAddresses)
                .HasForeignKey(d => d.MaTinh)
                .HasConstraintName("FK_AccountAddress_Provinces");

            entity.HasOne(d => d.Xa).WithMany(p => p.AccountAddresses)
                .HasForeignKey(d => d.MaXa)
                .HasConstraintName("FK_AccountAddress_Wards");
        });

        modelBuilder.Entity<ThuocTinh>(entity =>
        {
            entity.Property(e => e.AttributeId).HasColumnName("AttributeID");
            entity.Property(e => e.NameEn).HasColumnName("Name_EN");
        });

        modelBuilder.Entity<ThuocTinhTiemNang>(entity =>
        {
            entity.ToTable("AttributesPotentail");

            entity.Property(e => e.TimeSend).HasColumnType("datetime");

            entity.HasOne(d => d.Potentail).WithMany(p => p.AttributesPotentails)
                .HasForeignKey(d => d.PotentailId)
                .HasConstraintName("FK_AttributesPotentail_CustomerPotentail");
        });

        modelBuilder.Entity<GiaThuocTinh>(entity =>
        {
            entity.Property(e => e.AttributesPriceId).HasColumnName("AttributesPriceID");
            entity.Property(e => e.AttributeId).HasColumnName("AttributeID");
            entity.Property(e => e.PriceEn).HasColumnName("Price_EN");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Attribute).WithMany(p => p.AttributesPrices)
                .HasForeignKey(d => d.AttributeId)
                .HasConstraintName("FK_AttributesPrices_Attributes");

            entity.HasOne(d => d.SanPham).WithMany(p => p.AttributesPrices)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_AttributesPrices_Products");
        });

        modelBuilder.Entity<BangQuangCao>(entity =>
        {
            entity.ToTable("BangQuangCao");

            entity.Property(e => e.Banner1).HasColumnName("BangQuangCao");

            entity.HasOne(d => d.Cat).WithMany(p => p.Banners)
                .HasForeignKey(d => d.CatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Banner_Categories");
        });

        modelBuilder.Entity<ThuongHieu>(entity =>
        {
            entity.Property(e => e.BrandId).HasColumnName("BrandID");
            entity.Property(e => e.BrandName).HasMaxLength(50);
        });

        modelBuilder.Entity<NhomThuongHieu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_BrandGroup");

            entity.HasOne(d => d.Brand).WithMany(p => p.BrandGroups)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("FK_BrandGroup_Brands");
        });

        modelBuilder.Entity<MauThe>(entity =>
        {
            entity.Property(e => e.CardTemplateId).HasColumnName("CardTemplateID");
            entity.Property(e => e.NgayTao).HasColumnType("datetime");
            entity.Property(e => e.DateModified).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<DanhMuc>(entity =>
        {
            entity.HasKey(e => e.CatId);

            entity.Property(e => e.CatId).HasColumnName("CatID");
            entity.Property(e => e.Alias).HasMaxLength(250);
            entity.Property(e => e.CatName).HasMaxLength(250);
            entity.Property(e => e.CatNameEn)
                .HasMaxLength(250)
                .HasColumnName("CatName_EN");
            entity.Property(e => e.Cover).HasMaxLength(255);
            entity.Property(e => e.DescriptionEn).HasColumnName("Description_EN");
            entity.Property(e => e.MetaDesc).HasMaxLength(250);
            entity.Property(e => e.MetaDescEn)
                .HasMaxLength(250)
                .HasColumnName("MetaDesc_EN");
            entity.Property(e => e.MetaKey).HasMaxLength(250);
            entity.Property(e => e.MetaKeyEn)
                .HasMaxLength(250)
                .HasColumnName("MetaKey_EN");
            entity.Property(e => e.ParentId).HasColumnName("ParentID");
            entity.Property(e => e.Thumb).HasMaxLength(250);
            entity.Property(e => e.Title).HasMaxLength(250);
            entity.Property(e => e.TitleEn)
                .HasMaxLength(250)
                .HasColumnName("Title_EN");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK_Categories_Categories");
        });

        modelBuilder.Entity<DanhMucThuocTinh>(entity =>
        {
            entity.Property(e => e.CategoryAttributeId).HasColumnName("CategoryAttributeID");
            entity.Property(e => e.AttributeId).HasColumnName("AttributeID");
            entity.Property(e => e.CatId).HasColumnName("CatID");
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Attribute).WithMany(p => p.CategoryAttributes)
                .HasForeignKey(d => d.AttributeId)
                .HasConstraintName("FK_CategoryAttributes_Attributes");

            entity.HasOne(d => d.Cat).WithMany(p => p.CategoryAttributes)
                .HasForeignKey(d => d.CatId)
                .HasConstraintName("FK_CategoryAttributes_Categories");
        });

        modelBuilder.Entity<DanhMucThuongHieu>(entity =>
        {
            entity.Property(e => e.CategoryBrandId).HasColumnName("CategoryBrandID");
            entity.Property(e => e.BrandId).HasColumnName("BrandID");
            entity.Property(e => e.CatId).HasColumnName("CatID");
            entity.Property(e => e.MoTa).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Brand).WithMany(p => p.CategoryBrands)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("FK_CategoryBrands_Brands");

            entity.HasOne(d => d.Cat).WithMany(p => p.CategoryBrands)
                .HasForeignKey(d => d.CatId)
                .HasConstraintName("FK_CategoryBrands_Categories");
        });

        modelBuilder.Entity<TrangThaiThanhToan>(entity =>
        {
            entity.HasKey(e => e.CodStatusId).HasName("PK_CODStatus_1");

            entity.ToTable("CODStatus");

            entity.Property(e => e.CodStatusId).HasColumnName("CodStatusID");
            entity.Property(e => e.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<KhachHang>(entity =>
        {
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Avatar).HasMaxLength(255);
            entity.Property(e => e.Birthday).HasColumnType("datetime");
            entity.Property(e => e.NgayTao).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsFixedLength();
            entity.Property(e => e.HoTen).HasMaxLength(255);
            entity.Property(e => e.LanDangNhapCuoi).HasColumnType("datetime");
            entity.Property(e => e.MatKhau).HasMaxLength(50);
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Salt)
                .HasMaxLength(8)
                .IsFixedLength();
        });

        modelBuilder.Entity<KhachHangThuongHieu>(entity =>
        {
            entity.ToTable("CustomerBrand");

            entity.Property(e => e.Link)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.YearOff)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.YearOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<KhachHangTiemNang>(entity =>
        {
            entity.ToTable("CustomerPotentail");

            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Lever).WithMany(p => p.CustomerPotentails)
                .HasForeignKey(d => d.LeverId)
                .HasConstraintName("FK_CustomerPotentail_LeverCustommerPTT");
        });

        modelBuilder.Entity<DuAnKhachHang>(entity =>
        {
            entity.ToTable("CustomerProject");

            entity.Property(e => e.YearOff).HasColumnType("datetime");
            entity.Property(e => e.YearOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<NhaCungCapKhachHang>(entity =>
        {
            entity.ToTable("CustomerSupplier");

            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.YearAdd).HasColumnType("datetime");
        });

        modelBuilder.Entity<TrangThaiGiaoHang>(entity =>
        {
            entity.HasKey(e => e.DeliveryStatusId).HasName("PK_TransactStatus");

            entity.ToTable("DeliveryStatus");

            entity.Property(e => e.DeliveryStatusId).HasColumnName("DeliveryStatusID");
            entity.Property(e => e.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<KhuyenMai>(entity =>
        {
            entity.ToTable("Discount");

            entity.Property(e => e.Discount1).HasColumnName("Discount");
            entity.Property(e => e.TimeOff).HasColumnType("datetime");
            entity.Property(e => e.TimeOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<KhuyenMaiThemKhachHang>(entity =>
        {
            entity.ToTable("DiscountAddCustomer");

            entity.HasOne(d => d.Customer).WithMany(p => p.DiscountAddCustomers)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_DiscountAddCustomer_Customers");

            entity.HasOne(d => d.Discount).WithMany(p => p.DiscountAddCustomers)
                .HasForeignKey(d => d.DiscountId)
                .HasConstraintName("FK_DiscountAddCustomer_Discount");
        });

        modelBuilder.Entity<KhuyenMaiThemSanPham>(entity =>
        {
            entity.HasOne(d => d.Discount).WithMany(p => p.DiscountAddProducts)
                .HasForeignKey(d => d.DiscountId)
                .HasConstraintName("FK_DiscountAddProducts_Discount");

            entity.HasOne(d => d.SanPham).WithMany(p => p.DiscountAddProducts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_DiscountAddProducts_Products");
        });

        modelBuilder.Entity<Huyen>(entity =>
        {
            entity.HasKey(e => e.DistrictId).HasName("PK__District__85FDA4A66FD0CA46");

            entity.Property(e => e.DistrictId)
                .ValueGeneratedNever()
                .HasColumnName("DistrictID");
            entity.Property(e => e.DistrictName).HasMaxLength(50);
            entity.Property(e => e.ProvinceId).HasColumnName("ProvinceID");
            entity.Property(e => e.Type).HasMaxLength(20);

            entity.HasOne(d => d.Province).WithMany(p => p.Districts)
                .HasForeignKey(d => d.ProvinceId)
                .HasConstraintName("FK_Districts_Provinces");
        });

        modelBuilder.Entity<ThuocTinhEmail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_EmailAttributes_1");

            entity.Property(e => e.Body).HasColumnName("body");
            entity.Property(e => e.TimeSend).HasColumnType("datetime");

            entity.HasOne(d => d.Custumer).WithMany(p => p.EmailAttributes)
                .HasForeignKey(d => d.CustumerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmailAttributes_Customers");
        });

        modelBuilder.Entity<EmailTiepThi>(entity =>
        {
            entity.HasKey(e => e.EmailId);

            entity.ToTable("EmailMaketting");

            entity.Property(e => e.EmailId).HasColumnName("EmailID");
            entity.Property(e => e.AcountId).HasColumnName("AcountID");
            entity.Property(e => e.NgayTao).HasColumnType("datetime");
            entity.Property(e => e.CustomDate).HasColumnType("datetime");

            entity.HasOne(d => d.Acount).WithMany(p => p.EmailMakettings)
                .HasForeignKey(d => d.AcountId)
                .HasConstraintName("FK_EmailMaketting_Accounts");
        });

        modelBuilder.Entity<TrangFacebook>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Facebook");

            entity.ToTable("FacebookPage");

            entity.Property(e => e.AvartarGroup).HasColumnName("Avartar_group");
            entity.Property(e => e.NameGroup).HasColumnName("Name_group");
            entity.Property(e => e.TokenAccount).HasColumnName("token_account");
        });

        modelBuilder.Entity<ThuocTinhQuaTang>(entity =>
        {
            entity.HasOne(d => d.Discount).WithMany(p => p.GitAttributes)
                .HasForeignKey(d => d.DiscountId)
                .HasConstraintName("FK_GitAttributes_Discount");

            entity.HasOne(d => d.QuaTangSanPham).WithMany(p => p.GitAttributes)
                .HasForeignKey(d => d.ProductGiftId)
                .HasConstraintName("FK_GitAttributes_ProductGift");
        });

        modelBuilder.Entity<KhachVangLai>(entity =>
        {
            entity.Property(e => e.GuestId).HasColumnName("GuestID");
            entity.Property(e => e.NgayTao).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsFixedLength();
            entity.Property(e => e.HoTen).HasMaxLength(255);
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(12)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LichSuKhuyenMai>(entity =>
        {
            entity.ToTable("HistoryDiscount");

            entity.Property(e => e.TimeCreate).HasColumnType("datetime");

            entity.HasOne(d => d.Discount).WithMany(p => p.HistoryDiscounts)
                .HasForeignKey(d => d.DiscountId)
                .HasConstraintName("FK_HistoryDiscount_Discount");
        });

        modelBuilder.Entity<MayChuHinhAnh>(entity =>
        {
            entity.ToTable("ImageServer");
        });

        modelBuilder.Entity<CapDoKhachHang>(entity =>
        {
            entity.ToTable("LeverCustommerPTT");
        });

        modelBuilder.Entity<DonHang>(entity =>
        {
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.CodstatusId).HasColumnName("CODstatusID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.DeliveryStatusId).HasColumnName("DeliveryStatusID");
            entity.Property(e => e.GuestId).HasColumnName("GuestID");
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.PaymentStatusId).HasColumnName("PaymentStatusID");
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.ShipDate).HasColumnType("datetime");

            entity.HasOne(d => d.Codstatus).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CodstatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_CODStatus");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Orders_Customers");

            entity.HasOne(d => d.DeliveryStatus).WithMany(p => p.Orders)
                .HasForeignKey(d => d.DeliveryStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_DeliveryStatus");

            entity.HasOne(d => d.Guest).WithMany(p => p.Orders)
                .HasForeignKey(d => d.GuestId)
                .HasConstraintName("FK_Orders_Guests");

            entity.HasOne(d => d.TrangThaiThanhToan).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PaymentStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_PaymentStatus");
        });

        modelBuilder.Entity<ChiTietDonHang>(entity =>
        {
            entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");
            entity.Property(e => e.NgayTao).HasColumnType("datetime");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_OrderDetails_Orders");

            entity.HasOne(d => d.SanPham).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_OrderDetails_Products");
        });

        modelBuilder.Entity<TrangWeb>(entity =>
        {
            entity.Property(e => e.PageId).HasColumnName("PageID");
            entity.Property(e => e.Alias).HasMaxLength(250);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.MetaDesc).HasMaxLength(250);
            entity.Property(e => e.MetaKey).HasMaxLength(250);
            entity.Property(e => e.PageName).HasMaxLength(250);
            entity.Property(e => e.Thumb).HasMaxLength(250);
            entity.Property(e => e.Title).HasMaxLength(250);
        });

        modelBuilder.Entity<ThongTinTrang>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Domain).HasMaxLength(50);
            entity.Property(e => e.FacebookAppId).HasMaxLength(50);
            entity.Property(e => e.FacebookPage).HasMaxLength(50);
            entity.Property(e => e.GoogleSiteVerification).HasMaxLength(50);
            entity.Property(e => e.SoDienThoai).HasMaxLength(50);
            entity.Property(e => e.Robots).HasMaxLength(50);
        });

        modelBuilder.Entity<TrangThaiThanhToan>(entity =>
        {
            entity.ToTable("TrangThaiThanhToan");

            entity.Property(e => e.PaymentStatusId).HasColumnName("PaymentStatusID");
            entity.Property(e => e.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<BaiViet>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PK_tblTinTucs");

            entity.Property(e => e.PostId).HasColumnName("PostID");
            entity.Property(e => e.MaTaiKhoan).HasColumnName("MaTaiKhoan");
            entity.Property(e => e.Alias).HasMaxLength(255);
            entity.Property(e => e.AliasEn)
                .HasMaxLength(255)
                .HasColumnName("Alias_EN");
            entity.Property(e => e.Author).HasMaxLength(255);
            entity.Property(e => e.ContentsEn).HasColumnName("Contents_EN");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.IsHot).HasColumnName("isHot");
            entity.Property(e => e.IsNewfeed).HasColumnName("isNewfeed");
            entity.Property(e => e.MetaDescEn).HasColumnName("MetaDesc_EN");
            entity.Property(e => e.MetaKey).HasMaxLength(255);
            entity.Property(e => e.MetaKeyEn)
                .HasMaxLength(255)
                .HasColumnName("MetaKey_EN");
            entity.Property(e => e.PostCatId).HasColumnName("PostCatID");
            entity.Property(e => e.Scontents).HasColumnName("SContents");
            entity.Property(e => e.ScontentsEn).HasColumnName("SContents_EN");
            entity.Property(e => e.Thumb).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.TitleEn)
                .HasMaxLength(255)
                .HasColumnName("Title_EN");
            entity.Property(e => e.TitleSeo)
                .HasMaxLength(255)
                .HasColumnName("TitleSEO");
            entity.Property(e => e.TitleSeoEn)
                .HasMaxLength(255)
                .HasColumnName("TitleSEO_EN");

            entity.HasOne(d => d.PostCat).WithMany(p => p.Posts)
                .HasForeignKey(d => d.PostCatId)
                .HasConstraintName("FK_Posts_PostCategory");
        });

        modelBuilder.Entity<DanhMucBaiViet>(entity =>
        {
            entity.HasKey(e => e.PostCatId);

            entity.ToTable("DanhMucBaiViet");

            entity.Property(e => e.PostCatId).HasColumnName("PostCatID");
            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.DateModified).HasColumnType("datetime");
            entity.Property(e => e.PostCatName).HasMaxLength(255);
        });

        modelBuilder.Entity<SanPham>(entity =>
        {
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.BrandId).HasColumnName("BrandID");
            entity.Property(e => e.ConfigInformationEn).HasColumnName("ConfigInformation_EN");
            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.DateModified).HasColumnType("datetime");
            entity.Property(e => e.DescriptionEn).HasColumnName("Description_EN");
            entity.Property(e => e.GiftEn).HasColumnName("Gift_EN");
            entity.Property(e => e.MetaDescEn).HasColumnName("MetaDesc_EN");
            entity.Property(e => e.MetaKeyEn).HasColumnName("MetaKey_EN");
            entity.Property(e => e.ProductCode).IsRequired();
            entity.Property(e => e.ProductName).IsRequired();
            entity.Property(e => e.ProductNameEn).HasColumnName("ProductName_EN");
            entity.Property(e => e.ShortDescEn).HasColumnName("ShortDesc_EN");
            entity.Property(e => e.TitleEn).HasColumnName("Title_EN");
            entity.Property(e => e.WarrantyNoteEn).HasColumnName("WarrantyNote_EN");

            entity.HasOne(d => d.Brand).WithMany(p => p.Products)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("FK_Products_Brands");
        });

        modelBuilder.Entity<SanPhamThemKhachHang>(entity =>
        {
            entity.ToTable("SanPhamThemKhachHang");

            entity.HasOne(d => d.Customer).WithMany(p => p.ProductAddCusPros)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_ProductAddCusPro_CustomerSupplier");

            entity.HasOne(d => d.SanPham).WithMany(p => p.ProductAddCusPros)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_ProductAddCusPro_Products");
        });

        modelBuilder.Entity<DanhMucSanPham>(entity =>
        {
            entity.HasKey(e => e.ProductCatId);

            entity.Property(e => e.ProductCatId).HasColumnName("ProductCatID");
            entity.Property(e => e.CatId).HasColumnName("CatID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Cat).WithMany(p => p.ProductCategories)
                .HasForeignKey(d => d.CatId)
                .HasConstraintName("FK_ProductCategories_Categories");

            entity.HasOne(d => d.SanPham).WithMany(p => p.ProductCategories)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_ProductCategories_Products");
        });

        modelBuilder.Entity<SanPhamFacebook>(entity =>
        {
            entity.ToTable("SanPhamFacebook");

            entity.Property(e => e.CeateAdd).HasColumnType("datetime");
        });

        modelBuilder.Entity<QuaTangSanPham>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ProductDift");

            entity.ToTable("QuaTangSanPham");
        });

        modelBuilder.Entity<AnhSanPham>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK_Product_Thumb");

            entity.Property(e => e.ImageId).HasColumnName("ImageID");
            entity.Property(e => e.Alias).HasMaxLength(250);
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.SanPham).WithMany(p => p.ProductThumbs)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_ProductThumbs_Products");
        });

        modelBuilder.Entity<Tinh>(entity =>
        {
            entity.HasKey(e => e.ProvinceId).HasName("PK__Province__FD0A6FA3F17E00B8");

            entity.Property(e => e.ProvinceId)
                .ValueGeneratedNever()
                .HasColumnName("ProvinceID");
            entity.Property(e => e.ProvinceName).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(20);
        });

        modelBuilder.Entity<QuangCao>(entity =>
        {
            entity.Property(e => e.QuangCaoId).HasColumnName("QuangCaoID");
            entity.Property(e => e.NgayTao).HasColumnType("datetime");
            entity.Property(e => e.ImageBg)
                .HasMaxLength(250)
                .HasColumnName("ImageBG");
            entity.Property(e => e.ImageProduct).HasMaxLength(250);
            entity.Property(e => e.SubTitle).HasMaxLength(150);
            entity.Property(e => e.Title).HasMaxLength(150);
            entity.Property(e => e.UrlLink).HasMaxLength(250);
        });

        modelBuilder.Entity<BaoGia>(entity =>
        {
            entity.Property(e => e.QuotationId).HasColumnName("QuotationID");
            entity.Property(e => e.NgayTao).HasColumnType("datetime");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Vat)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("VAT");

            entity.HasOne(d => d.Customer).WithMany(p => p.Quotations)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Quotations_Customers");
        });

        modelBuilder.Entity<ChiTietBaoGia>(entity =>
        {
            entity.Property(e => e.QuotationDetailId).HasColumnName("QuotationDetailID");
            entity.Property(e => e.NgayTao).HasColumnType("datetime");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.QuotationId).HasColumnName("QuotationID");

            entity.HasOne(d => d.SanPham).WithMany(p => p.QuotationDetails)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_QuotationDetails_Products");

            entity.HasOne(d => d.BaoGia).WithMany(p => p.QuotationDetails)
                .HasForeignKey(d => d.QuotationId)
                .HasConstraintName("FK_QuotationDetails_Quotations");
        });

        modelBuilder.Entity<VaiTro>(entity =>
        {
            entity.Property(e => e.MaVaiTro).HasColumnName("MaVaiTro");
            entity.Property(e => e.MoTa).HasMaxLength(50);
            entity.Property(e => e.TenVaiTro).HasMaxLength(50);
        });

        modelBuilder.Entity<NguoiGiaoHang>(entity =>
        {
            entity.Property(e => e.ShipperId).HasColumnName("ShipperID");
            entity.Property(e => e.Company).HasMaxLength(150);
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.ShipDate).HasColumnType("datetime");
            entity.Property(e => e.ShipperName).HasMaxLength(150);
        });

        modelBuilder.Entity<DiaChiGiaoHang>(entity =>
        {
            entity.HasKey(e => e.ShippingAdressId);

            entity.ToTable("DiaChiGiaoHang");

            entity.Property(e => e.ShippingAdressId).HasColumnName("ShippingAdressID");
            entity.Property(e => e.DistrictId).HasColumnName("DistrictID");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.SoDienThoai).HasMaxLength(255);
            entity.Property(e => e.ProvinceId).HasColumnName("ProvinceID");
            entity.Property(e => e.WardId).HasColumnName("WardID");

            entity.HasOne(d => d.District).WithMany(p => p.ShippingAddresses)
                .HasForeignKey(d => d.DistrictId)
                .HasConstraintName("FK_ShippingAddress_Districts");

            entity.HasOne(d => d.Order).WithMany(p => p.ShippingAddresses)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_ShippingAddress_Orders");

            entity.HasOne(d => d.Province).WithMany(p => p.ShippingAddresses)
                .HasForeignKey(d => d.ProvinceId)
                .HasConstraintName("FK_ShippingAddress_Provinces");

            entity.HasOne(d => d.Ward).WithMany(p => p.ShippingAddresses)
                .HasForeignKey(d => d.WardId)
                .HasConstraintName("FK_ShippingAddress_Wards");
        });

        modelBuilder.Entity<TrangTrinhBay>(entity =>
        {
            entity.HasKey(e => e.SlideId).HasName("PK_Table_1");

            entity.Property(e => e.SlideId).HasColumnName("SlideID");
            entity.Property(e => e.CatId).HasColumnName("CatID");

            entity.HasOne(d => d.Cat).WithMany(p => p.Slides)
                .HasForeignKey(d => d.CatId)
                .HasConstraintName("FK_Slides_Categories");
        });

        modelBuilder.Entity<HeThongWeb>(entity =>
        {
            entity.ToTable("HeThongWeb");

            entity.Property(e => e.EmailSmtp).HasColumnName("EmailSMTP");
            entity.Property(e => e.PassSmtp).HasColumnName("PassSMTP");
        });

        modelBuilder.Entity<PhimAnh>(entity =>
        {
            entity.ToTable("PhimAnh");

            entity.Property(e => e.Video1).HasColumnName("PhimAnh");

            entity.HasOne(d => d.Brand).WithMany(p => p.Videos)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Video_Brands");
        });

        modelBuilder.Entity<Xa>(entity =>
        {
            entity.HasKey(e => e.WardId).HasName("PK__Wards__C6BD9BEAAFC290EC");

            entity.Property(e => e.WardId)
                .ValueGeneratedNever()
                .HasColumnName("WardID");
            entity.Property(e => e.DistrictId).HasColumnName("DistrictID");
            entity.Property(e => e.Type).HasMaxLength(20);
            entity.Property(e => e.WardName).HasMaxLength(50);

            entity.HasOne(d => d.District).WithMany(p => p.Wards)
                .HasForeignKey(d => d.DistrictId)
                .HasConstraintName("FK_Wards_Districts");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
