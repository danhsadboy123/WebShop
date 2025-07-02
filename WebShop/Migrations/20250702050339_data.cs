using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.Migrations
{
    /// <inheritdoc />
    public partial class data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attributes",
                columns: table => new
                {
                    AttributeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name_EN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ordering = table.Column<int>(type: "int", nullable: true),
                    IsActivated = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attributes", x => x.AttributeID);
                });

            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    BrandID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Thumb = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.BrandID);
                });

            migrationBuilder.CreateTable(
                name: "CardTemplates",
                columns: table => new
                {
                    CardTemplateID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HtmlContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardTemplates", x => x.CardTemplateID);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CatID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CatName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CatName_EN = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description_EN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentID = table.Column<int>(type: "int", nullable: true),
                    Levels = table.Column<int>(type: "int", nullable: true),
                    Outstanding = table.Column<bool>(type: "bit", nullable: false),
                    Ordering = table.Column<int>(type: "int", nullable: true),
                    Published = table.Column<bool>(type: "bit", nullable: false),
                    Thumb = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Title_EN = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Alias = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    MetaDesc = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    MetaDesc_EN = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    MetaKey = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    MetaKey_EN = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Cover = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SchemaMarkup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BrandShow = table.Column<bool>(type: "bit", nullable: true),
                    ThumbShow = table.Column<bool>(type: "bit", nullable: true),
                    BannerThumb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameBrand = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CatID);
                    table.ForeignKey(
                        name: "FK_Categories_Categories",
                        column: x => x.ParentID,
                        principalTable: "Categories",
                        principalColumn: "CatID");
                });

            migrationBuilder.CreateTable(
                name: "CODStatus",
                columns: table => new
                {
                    CodStatusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CODStatus_1", x => x.CodStatusID);
                });

            migrationBuilder.CreateTable(
                name: "CustomerBrand",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YearOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    YearOff = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true),
                    Link = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerBrand", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerProject",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YearOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    YearOff = table.Column<DateTime>(type: "datetime", nullable: true),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerProject", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Birthday = table.Column<DateTime>(type: "datetime", nullable: true),
                    Avatar = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Email = table.Column<string>(type: "nchar(150)", fixedLength: true, maxLength: 150, nullable: true),
                    SoDienThoai = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Salt = table.Column<string>(type: "nchar(8)", fixedLength: true, maxLength: 8, nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActivated = table.Column<bool>(type: "bit", nullable: false),
                    Gender = table.Column<bool>(type: "bit", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerID);
                });

            migrationBuilder.CreateTable(
                name: "CustomerSupplier",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoDienThoai = table.Column<int>(type: "int", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YearAdd = table.Column<DateTime>(type: "datetime", nullable: true),
                    Lever = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerSupplier", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryStatus",
                columns: table => new
                {
                    DeliveryStatusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactStatus", x => x.DeliveryStatusID);
                });

            migrationBuilder.CreateTable(
                name: "Discount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gift = table.Column<bool>(type: "bit", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    TimeOff = table.Column<DateTime>(type: "datetime", nullable: true),
                    ShowWeb = table.Column<bool>(type: "bit", nullable: false),
                    MaxApply = table.Column<int>(type: "int", nullable: true),
                    Startus = table.Column<bool>(type: "bit", nullable: false),
                    Discount = table.Column<int>(type: "int", nullable: true),
                    ConditionCheck = table.Column<bool>(type: "bit", nullable: false),
                    Money = table.Column<int>(type: "int", nullable: true),
                    ProductCount = table.Column<int>(type: "int", nullable: true),
                    Sum = table.Column<int>(type: "int", nullable: true),
                    ApplyGift = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FacebookPage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Userid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    token_account = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name_group = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Avartar_group = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GroupId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facebook", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Guests",
                columns: table => new
                {
                    GuestID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SoDienThoai = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: true),
                    Email = table.Column<string>(type: "nchar(150)", fixedLength: true, maxLength: 150, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guests", x => x.GuestID);
                });

            migrationBuilder.CreateTable(
                name: "ImageServer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateUpdate = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageServer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeverCustommerPTT",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameLever = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeverCustommerPTT", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PageInfos",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Domain = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MetaDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Robots = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GoogleSiteVerification = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FacebookAppId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FacebookPage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GoogleTracking = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoDienThoai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeaderCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FoodterCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Profile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OgImage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageInfos", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Pages",
                columns: table => new
                {
                    PageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PageName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Contents = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Thumb = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Published = table.Column<bool>(type: "bit", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    MetaDesc = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    MetaKey = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Alias = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Ordering = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pages", x => x.PageID);
                });

            migrationBuilder.CreateTable(
                name: "PaymentStatus",
                columns: table => new
                {
                    PaymentStatusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentStatus", x => x.PaymentStatusID);
                });

            migrationBuilder.CreateTable(
                name: "PostCategory",
                columns: table => new
                {
                    PostCatID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostCatName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostCategory", x => x.PostCatID);
                });

            migrationBuilder.CreateTable(
                name: "ProductFacebook",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CeateAdd = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductFacebook", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductGift",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Alias = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDift", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Provinces",
                columns: table => new
                {
                    ProvinceID = table.Column<int>(type: "int", nullable: false),
                    ProvinceName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Province__FD0A6FA3F17E00B8", x => x.ProvinceID);
                });

            migrationBuilder.CreateTable(
                name: "QuangCaos",
                columns: table => new
                {
                    QuangCaoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubTitle = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ImageBG = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ImageProduct = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UrlLink = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    IsActivated = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuangCaos", x => x.QuangCaoID);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Shippers",
                columns: table => new
                {
                    ShipperID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShipperName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    SoDienThoai = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true),
                    Company = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ShipDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shippers", x => x.ShipperID);
                });

            migrationBuilder.CreateTable(
                name: "SystemWeb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Server = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailSend = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Post = table.Column<int>(type: "int", nullable: true),
                    EmailSMTP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PassSMTP = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemWeb", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BrandGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandId = table.Column<int>(type: "int", nullable: true),
                    CatId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrandGroup_Brands",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "BrandID");
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductName_EN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortDesc_EN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description_EN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConfigInformation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConfigInformation_EN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gift = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gift_EN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Warranty = table.Column<int>(type: "int", nullable: true),
                    WarrantyNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WarrantyNote_EN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<int>(type: "int", nullable: true),
                    SalePrice = table.Column<int>(type: "int", nullable: true),
                    BrandID = table.Column<int>(type: "int", nullable: true),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Video = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    BestSellers = table.Column<bool>(type: "bit", nullable: false),
                    HomeFlag = table.Column<bool>(type: "bit", nullable: false),
                    IsActivated = table.Column<bool>(type: "bit", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title_EN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Alias = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaDesc_EN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaKey_EN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitsInStock = table.Column<int>(type: "int", nullable: true),
                    ProductOption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BrandGroup = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_Products_Brands",
                        column: x => x.BrandID,
                        principalTable: "Brands",
                        principalColumn: "BrandID");
                });

            migrationBuilder.CreateTable(
                name: "Video",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Video = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Show = table.Column<bool>(type: "bit", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Video", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Video_Brands",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "BrandID");
                });

            migrationBuilder.CreateTable(
                name: "Banner",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CatId = table.Column<int>(type: "int", nullable: false),
                    Banner = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banner", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Banner_Categories",
                        column: x => x.CatId,
                        principalTable: "Categories",
                        principalColumn: "CatID");
                });

            migrationBuilder.CreateTable(
                name: "CategoryAttributes",
                columns: table => new
                {
                    CategoryAttributeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CatID = table.Column<int>(type: "int", nullable: true),
                    AttributeID = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActivated = table.Column<bool>(type: "bit", nullable: true),
                    Show = table.Column<bool>(type: "bit", nullable: true),
                    Odering = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryAttributes", x => x.CategoryAttributeID);
                    table.ForeignKey(
                        name: "FK_CategoryAttributes_Attributes",
                        column: x => x.AttributeID,
                        principalTable: "Attributes",
                        principalColumn: "AttributeID");
                    table.ForeignKey(
                        name: "FK_CategoryAttributes_Categories",
                        column: x => x.CatID,
                        principalTable: "Categories",
                        principalColumn: "CatID");
                });

            migrationBuilder.CreateTable(
                name: "CategoryBrands",
                columns: table => new
                {
                    CategoryBrandID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CatID = table.Column<int>(type: "int", nullable: true),
                    BrandID = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActivated = table.Column<bool>(type: "bit", nullable: true),
                    Topbrand = table.Column<bool>(type: "bit", nullable: true),
                    BrandProduct = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryBrands", x => x.CategoryBrandID);
                    table.ForeignKey(
                        name: "FK_CategoryBrands_Brands",
                        column: x => x.BrandID,
                        principalTable: "Brands",
                        principalColumn: "BrandID");
                    table.ForeignKey(
                        name: "FK_CategoryBrands_Categories",
                        column: x => x.CatID,
                        principalTable: "Categories",
                        principalColumn: "CatID");
                });

            migrationBuilder.CreateTable(
                name: "Slides",
                columns: table => new
                {
                    SlideID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CatID = table.Column<int>(type: "int", nullable: true),
                    Thumb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Alias = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SlideName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActivated = table.Column<bool>(type: "bit", nullable: false),
                    Right = table.Column<bool>(type: "bit", nullable: false),
                    Bottom = table.Column<bool>(type: "bit", nullable: false),
                    HomeFlag = table.Column<bool>(type: "bit", nullable: false),
                    Ordering = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Table_1", x => x.SlideID);
                    table.ForeignKey(
                        name: "FK_Slides_Categories",
                        column: x => x.CatID,
                        principalTable: "Categories",
                        principalColumn: "CatID");
                });

            migrationBuilder.CreateTable(
                name: "EmailAttributes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustumerId = table.Column<int>(type: "int", nullable: false),
                    body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeSend = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailAttributes_1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailAttributes_Customers",
                        column: x => x.CustumerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerID");
                });

            migrationBuilder.CreateTable(
                name: "Quotations",
                columns: table => new
                {
                    QuotationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<int>(type: "int", nullable: true),
                    TotalMoney = table.Column<int>(type: "int", nullable: true),
                    VAT = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Confirmed = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quotations", x => x.QuotationID);
                    table.ForeignKey(
                        name: "FK_Quotations_Customers",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID");
                });

            migrationBuilder.CreateTable(
                name: "DiscountAddCustomer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    DiscountId = table.Column<int>(type: "int", nullable: true),
                    CheckDiscount = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountAddCustomer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscountAddCustomer_Customers",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerID");
                    table.ForeignKey(
                        name: "FK_DiscountAddCustomer_Discount",
                        column: x => x.DiscountId,
                        principalTable: "Discount",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HistoryDiscount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscountId = table.Column<int>(type: "int", nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeCreate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryDiscount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoryDiscount_Discount",
                        column: x => x.DiscountId,
                        principalTable: "Discount",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomerPotentail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CompannyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoDienThoai = table.Column<int>(type: "int", nullable: true),
                    Checked = table.Column<int>(type: "int", nullable: true),
                    LeverId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerPotentail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerPotentail_LeverCustommerPTT",
                        column: x => x.LeverId,
                        principalTable: "LeverCustommerPTT",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<int>(type: "int", nullable: true),
                    GuestID = table.Column<int>(type: "int", nullable: true),
                    SoDienThoai = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ShipDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeliveryStatusID = table.Column<int>(type: "int", nullable: false),
                    PaymentStatusID = table.Column<int>(type: "int", nullable: false),
                    CODstatusID = table.Column<int>(type: "int", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    TotalMoney = table.Column<int>(type: "int", nullable: false),
                    CheckEmail = table.Column<bool>(type: "bit", nullable: true),
                    PaymentID = table.Column<int>(type: "int", nullable: true),
                    Confirmed = table.Column<bool>(type: "bit", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Draft = table.Column<bool>(type: "bit", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NotePay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NodeCus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discount = table.Column<int>(type: "int", nullable: true),
                    RedBill = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK_Orders_CODStatus",
                        column: x => x.CODstatusID,
                        principalTable: "CODStatus",
                        principalColumn: "CodStatusID");
                    table.ForeignKey(
                        name: "FK_Orders_Customers",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID");
                    table.ForeignKey(
                        name: "FK_Orders_DeliveryStatus",
                        column: x => x.DeliveryStatusID,
                        principalTable: "DeliveryStatus",
                        principalColumn: "DeliveryStatusID");
                    table.ForeignKey(
                        name: "FK_Orders_Guests",
                        column: x => x.GuestID,
                        principalTable: "Guests",
                        principalColumn: "GuestID");
                    table.ForeignKey(
                        name: "FK_Orders_PaymentStatus",
                        column: x => x.PaymentStatusID,
                        principalTable: "PaymentStatus",
                        principalColumn: "PaymentStatusID");
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    PostID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Title_EN = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SContents = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SContents_EN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Contents = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Contents_EN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Thumb = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Published = table.Column<bool>(type: "bit", nullable: false),
                    Alias = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Alias_EN = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Author = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AccountId = table.Column<int>(type: "int", nullable: true),
                    PostCatID = table.Column<int>(type: "int", nullable: true),
                    isHot = table.Column<bool>(type: "bit", nullable: false),
                    isNewfeed = table.Column<bool>(type: "bit", nullable: false),
                    TitleSEO = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TitleSEO_EN = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MetaKey = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MetaKey_EN = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MetaDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaDesc_EN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Views = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblTinTucs", x => x.PostID);
                    table.ForeignKey(
                        name: "FK_Posts_PostCategory",
                        column: x => x.PostCatID,
                        principalTable: "PostCategory",
                        principalColumn: "PostCatID");
                });

            migrationBuilder.CreateTable(
                name: "GitAttributes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscountId = table.Column<int>(type: "int", nullable: true),
                    ProductGiftId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GitAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GitAttributes_Discount",
                        column: x => x.DiscountId,
                        principalTable: "Discount",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GitAttributes_ProductGift",
                        column: x => x.ProductGiftId,
                        principalTable: "ProductGift",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Districts",
                columns: table => new
                {
                    DistrictID = table.Column<int>(type: "int", nullable: false),
                    ProvinceID = table.Column<int>(type: "int", nullable: true),
                    DistrictName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__District__85FDA4A66FD0CA46", x => x.DistrictID);
                    table.ForeignKey(
                        name: "FK_Districts_Provinces",
                        column: x => x.ProvinceID,
                        principalTable: "Provinces",
                        principalColumn: "ProvinceID");
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneNumber = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Salt = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true),
                    IsActivated = table.Column<bool>(type: "bit", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_Accounts_Roles",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId");
                });

            migrationBuilder.CreateTable(
                name: "AttributesPrices",
                columns: table => new
                {
                    AttributesPriceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttributeID = table.Column<int>(type: "int", nullable: true),
                    ProductID = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price_EN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActivated = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributesPrices", x => x.AttributesPriceID);
                    table.ForeignKey(
                        name: "FK_AttributesPrices_Attributes",
                        column: x => x.AttributeID,
                        principalTable: "Attributes",
                        principalColumn: "AttributeID");
                    table.ForeignKey(
                        name: "FK_AttributesPrices_Products",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID");
                });

            migrationBuilder.CreateTable(
                name: "DiscountAddProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    DiscountId = table.Column<int>(type: "int", nullable: true),
                    CheckDiscount = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountAddProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscountAddProducts_Discount",
                        column: x => x.DiscountId,
                        principalTable: "Discount",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DiscountAddProducts_Products",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductID");
                });

            migrationBuilder.CreateTable(
                name: "ProductAddCusPro",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    Stock = table.Column<int>(type: "int", nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAddCusPro", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductAddCusPro_CustomerSupplier",
                        column: x => x.CustomerId,
                        principalTable: "CustomerSupplier",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductAddCusPro_Products",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductID");
                });

            migrationBuilder.CreateTable(
                name: "ProductCategories",
                columns: table => new
                {
                    ProductCatID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CatID = table.Column<int>(type: "int", nullable: true),
                    ProductID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => x.ProductCatID);
                    table.ForeignKey(
                        name: "FK_ProductCategories_Categories",
                        column: x => x.CatID,
                        principalTable: "Categories",
                        principalColumn: "CatID");
                    table.ForeignKey(
                        name: "FK_ProductCategories_Products",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID");
                });

            migrationBuilder.CreateTable(
                name: "ProductThumbs",
                columns: table => new
                {
                    ImageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductID = table.Column<int>(type: "int", nullable: true),
                    Alias = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Ordering = table.Column<int>(type: "int", nullable: true),
                    IsMain = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Thumb", x => x.ImageID);
                    table.ForeignKey(
                        name: "FK_ProductThumbs_Products",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID");
                });

            migrationBuilder.CreateTable(
                name: "QuotationDetails",
                columns: table => new
                {
                    QuotationDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuotationID = table.Column<int>(type: "int", nullable: true),
                    ProductID = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<int>(type: "int", nullable: true),
                    Discount = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuotationDetails", x => x.QuotationDetailID);
                    table.ForeignKey(
                        name: "FK_QuotationDetails_Products",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID");
                    table.ForeignKey(
                        name: "FK_QuotationDetails_Quotations",
                        column: x => x.QuotationID,
                        principalTable: "Quotations",
                        principalColumn: "QuotationID");
                });

            migrationBuilder.CreateTable(
                name: "AttributesPotentail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PotentailId = table.Column<int>(type: "int", nullable: true),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeSend = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributesPotentail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttributesPotentail_CustomerPotentail",
                        column: x => x.PotentailId,
                        principalTable: "CustomerPotentail",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    OrderDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: true),
                    ProductID = table.Column<int>(type: "int", nullable: true),
                    OrderNumber = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<int>(type: "int", nullable: true),
                    Discount = table.Column<int>(type: "int", nullable: true),
                    TotalMoney = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    Price = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.OrderDetailID);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID");
                    table.ForeignKey(
                        name: "FK_OrderDetails_Products",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID");
                });

            migrationBuilder.CreateTable(
                name: "Wards",
                columns: table => new
                {
                    WardID = table.Column<int>(type: "int", nullable: false),
                    DistrictID = table.Column<int>(type: "int", nullable: true),
                    WardName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Wards__C6BD9BEAAFC290EC", x => x.WardID);
                    table.ForeignKey(
                        name: "FK_Wards_Districts",
                        column: x => x.DistrictID,
                        principalTable: "Districts",
                        principalColumn: "DistrictID");
                });

            migrationBuilder.CreateTable(
                name: "AccountAddress",
                columns: table => new
                {
                    AddressID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<int>(type: "int", nullable: true),
                    GuestID = table.Column<int>(type: "int", nullable: true),
                    SoDienThoai = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ProvinceID = table.Column<int>(type: "int", nullable: true),
                    DistrictID = table.Column<int>(type: "int", nullable: true),
                    WardID = table.Column<int>(type: "int", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AccountA__091C2A1B7AAC9A99", x => x.AddressID);
                    table.ForeignKey(
                        name: "FK_AccountAddress_Customers",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID");
                    table.ForeignKey(
                        name: "FK_AccountAddress_Districts",
                        column: x => x.DistrictID,
                        principalTable: "Districts",
                        principalColumn: "DistrictID");
                    table.ForeignKey(
                        name: "FK_AccountAddress_Guests",
                        column: x => x.GuestID,
                        principalTable: "Guests",
                        principalColumn: "GuestID");
                    table.ForeignKey(
                        name: "FK_AccountAddress_Provinces",
                        column: x => x.ProvinceID,
                        principalTable: "Provinces",
                        principalColumn: "ProvinceID");
                    table.ForeignKey(
                        name: "FK_AccountAddress_Wards",
                        column: x => x.WardID,
                        principalTable: "Wards",
                        principalColumn: "WardID");
                });

            migrationBuilder.CreateTable(
                name: "ShippingAddress",
                columns: table => new
                {
                    ShippingAdressID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SoDienThoai = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ProvinceID = table.Column<int>(type: "int", nullable: true),
                    DistrictID = table.Column<int>(type: "int", nullable: true),
                    WardID = table.Column<int>(type: "int", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingAddress", x => x.ShippingAdressID);
                    table.ForeignKey(
                        name: "FK_ShippingAddress_Districts",
                        column: x => x.DistrictID,
                        principalTable: "Districts",
                        principalColumn: "DistrictID");
                    table.ForeignKey(
                        name: "FK_ShippingAddress_Orders",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID");
                    table.ForeignKey(
                        name: "FK_ShippingAddress_Provinces",
                        column: x => x.ProvinceID,
                        principalTable: "Provinces",
                        principalColumn: "ProvinceID");
                    table.ForeignKey(
                        name: "FK_ShippingAddress_Wards",
                        column: x => x.WardID,
                        principalTable: "Wards",
                        principalColumn: "WardID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountAddress_CustomerID",
                table: "AccountAddress",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_AccountAddress_DistrictID",
                table: "AccountAddress",
                column: "DistrictID");

            migrationBuilder.CreateIndex(
                name: "IX_AccountAddress_GuestID",
                table: "AccountAddress",
                column: "GuestID");

            migrationBuilder.CreateIndex(
                name: "IX_AccountAddress_ProvinceID",
                table: "AccountAddress",
                column: "ProvinceID");

            migrationBuilder.CreateIndex(
                name: "IX_AccountAddress_WardID",
                table: "AccountAddress",
                column: "WardID");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_RoleId",
                table: "Accounts",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AttributesPotentail_PotentailId",
                table: "AttributesPotentail",
                column: "PotentailId");

            migrationBuilder.CreateIndex(
                name: "IX_AttributesPrices_AttributeID",
                table: "AttributesPrices",
                column: "AttributeID");

            migrationBuilder.CreateIndex(
                name: "IX_AttributesPrices_ProductID",
                table: "AttributesPrices",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Banner_CatId",
                table: "Banner",
                column: "CatId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandGroups_BrandId",
                table: "BrandGroups",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentID",
                table: "Categories",
                column: "ParentID");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryAttributes_AttributeID",
                table: "CategoryAttributes",
                column: "AttributeID");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryAttributes_CatID",
                table: "CategoryAttributes",
                column: "CatID");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryBrands_BrandID",
                table: "CategoryBrands",
                column: "BrandID");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryBrands_CatID",
                table: "CategoryBrands",
                column: "CatID");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPotentail_LeverId",
                table: "CustomerPotentail",
                column: "LeverId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountAddCustomer_CustomerId",
                table: "DiscountAddCustomer",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountAddCustomer_DiscountId",
                table: "DiscountAddCustomer",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountAddProducts_DiscountId",
                table: "DiscountAddProducts",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountAddProducts_ProductId",
                table: "DiscountAddProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_ProvinceID",
                table: "Districts",
                column: "ProvinceID");

            migrationBuilder.CreateIndex(
                name: "IX_EmailAttributes_CustumerId",
                table: "EmailAttributes",
                column: "CustumerId");

            migrationBuilder.CreateIndex(
                name: "IX_GitAttributes_DiscountId",
                table: "GitAttributes",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_GitAttributes_ProductGiftId",
                table: "GitAttributes",
                column: "ProductGiftId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoryDiscount_DiscountId",
                table: "HistoryDiscount",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderID",
                table: "OrderDetails",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductID",
                table: "OrderDetails",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CODstatusID",
                table: "Orders",
                column: "CODstatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerID",
                table: "Orders",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DeliveryStatusID",
                table: "Orders",
                column: "DeliveryStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_GuestID",
                table: "Orders",
                column: "GuestID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PaymentStatusID",
                table: "Orders",
                column: "PaymentStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_PostCatID",
                table: "Posts",
                column: "PostCatID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAddCusPro_CustomerId",
                table: "ProductAddCusPro",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAddCusPro_ProductId",
                table: "ProductAddCusPro",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_CatID",
                table: "ProductCategories",
                column: "CatID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_ProductID",
                table: "ProductCategories",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandID",
                table: "Products",
                column: "BrandID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductThumbs_ProductID",
                table: "ProductThumbs",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationDetails_ProductID",
                table: "QuotationDetails",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationDetails_QuotationID",
                table: "QuotationDetails",
                column: "QuotationID");

            migrationBuilder.CreateIndex(
                name: "IX_Quotations_CustomerID",
                table: "Quotations",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingAddress_DistrictID",
                table: "ShippingAddress",
                column: "DistrictID");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingAddress_OrderID",
                table: "ShippingAddress",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingAddress_ProvinceID",
                table: "ShippingAddress",
                column: "ProvinceID");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingAddress_WardID",
                table: "ShippingAddress",
                column: "WardID");

            migrationBuilder.CreateIndex(
                name: "IX_Slides_CatID",
                table: "Slides",
                column: "CatID");

            migrationBuilder.CreateIndex(
                name: "IX_Video_BrandId",
                table: "Video",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Wards_DistrictID",
                table: "Wards",
                column: "DistrictID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountAddress");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "AttributesPotentail");

            migrationBuilder.DropTable(
                name: "AttributesPrices");

            migrationBuilder.DropTable(
                name: "Banner");

            migrationBuilder.DropTable(
                name: "BrandGroups");

            migrationBuilder.DropTable(
                name: "CardTemplates");

            migrationBuilder.DropTable(
                name: "CategoryAttributes");

            migrationBuilder.DropTable(
                name: "CategoryBrands");

            migrationBuilder.DropTable(
                name: "CustomerBrand");

            migrationBuilder.DropTable(
                name: "CustomerProject");

            migrationBuilder.DropTable(
                name: "DiscountAddCustomer");

            migrationBuilder.DropTable(
                name: "DiscountAddProducts");

            migrationBuilder.DropTable(
                name: "EmailAttributes");

            migrationBuilder.DropTable(
                name: "FacebookPage");

            migrationBuilder.DropTable(
                name: "GitAttributes");

            migrationBuilder.DropTable(
                name: "HistoryDiscount");

            migrationBuilder.DropTable(
                name: "ImageServer");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "PageInfos");

            migrationBuilder.DropTable(
                name: "Pages");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "ProductAddCusPro");

            migrationBuilder.DropTable(
                name: "ProductCategories");

            migrationBuilder.DropTable(
                name: "ProductFacebook");

            migrationBuilder.DropTable(
                name: "ProductThumbs");

            migrationBuilder.DropTable(
                name: "QuangCaos");

            migrationBuilder.DropTable(
                name: "QuotationDetails");

            migrationBuilder.DropTable(
                name: "Shippers");

            migrationBuilder.DropTable(
                name: "ShippingAddress");

            migrationBuilder.DropTable(
                name: "Slides");

            migrationBuilder.DropTable(
                name: "SystemWeb");

            migrationBuilder.DropTable(
                name: "Video");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "CustomerPotentail");

            migrationBuilder.DropTable(
                name: "Attributes");

            migrationBuilder.DropTable(
                name: "ProductGift");

            migrationBuilder.DropTable(
                name: "Discount");

            migrationBuilder.DropTable(
                name: "PostCategory");

            migrationBuilder.DropTable(
                name: "CustomerSupplier");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Quotations");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Wards");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "LeverCustommerPTT");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "CODStatus");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "DeliveryStatus");

            migrationBuilder.DropTable(
                name: "Guests");

            migrationBuilder.DropTable(
                name: "PaymentStatus");

            migrationBuilder.DropTable(
                name: "Districts");

            migrationBuilder.DropTable(
                name: "Provinces");
        }
    }
}
