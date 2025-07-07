using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce_CaFeShop.Migrations
{
    /// <inheritdoc />
    public partial class dat3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietHoaDons_HoaDons_HoaDonMaHoaDon",
                table: "ChiTietHoaDons");

            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietHoaDons_SanPhams_SanPhamMaSanPham",
                table: "ChiTietHoaDons");

            migrationBuilder.DropIndex(
                name: "IX_ChiTietHoaDons_HoaDonMaHoaDon",
                table: "ChiTietHoaDons");

            migrationBuilder.DropIndex(
                name: "IX_ChiTietHoaDons_SanPhamMaSanPham",
                table: "ChiTietHoaDons");

            migrationBuilder.DropColumn(
                name: "HoaDonMaHoaDon",
                table: "ChiTietHoaDons");

            migrationBuilder.DropColumn(
                name: "SanPhamMaSanPham",
                table: "ChiTietHoaDons");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDons_MaHoaDon",
                table: "ChiTietHoaDons",
                column: "MaHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDons_MaSanPham",
                table: "ChiTietHoaDons",
                column: "MaSanPham");

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietHoaDons_HoaDons",
                table: "ChiTietHoaDons",
                column: "MaHoaDon",
                principalTable: "HoaDons",
                principalColumn: "MaHoaDon",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietHoaDons_SanPhams",
                table: "ChiTietHoaDons",
                column: "MaSanPham",
                principalTable: "SanPhams",
                principalColumn: "MaSanPham",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietHoaDons_HoaDons",
                table: "ChiTietHoaDons");

            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietHoaDons_SanPhams",
                table: "ChiTietHoaDons");

            migrationBuilder.DropIndex(
                name: "IX_ChiTietHoaDons_MaHoaDon",
                table: "ChiTietHoaDons");

            migrationBuilder.DropIndex(
                name: "IX_ChiTietHoaDons_MaSanPham",
                table: "ChiTietHoaDons");

            migrationBuilder.AddColumn<int>(
                name: "HoaDonMaHoaDon",
                table: "ChiTietHoaDons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SanPhamMaSanPham",
                table: "ChiTietHoaDons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDons_HoaDonMaHoaDon",
                table: "ChiTietHoaDons",
                column: "HoaDonMaHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDons_SanPhamMaSanPham",
                table: "ChiTietHoaDons",
                column: "SanPhamMaSanPham");

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietHoaDons_HoaDons_HoaDonMaHoaDon",
                table: "ChiTietHoaDons",
                column: "HoaDonMaHoaDon",
                principalTable: "HoaDons",
                principalColumn: "MaHoaDon",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietHoaDons_SanPhams_SanPhamMaSanPham",
                table: "ChiTietHoaDons",
                column: "SanPhamMaSanPham",
                principalTable: "SanPhams",
                principalColumn: "MaSanPham",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
