using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce_WatchShop.Migrations
{
    /// <inheritdoc />
    public partial class data_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_YeuThichs_KhachHangs_KhachHangMaKhachHang",
                table: "YeuThichs");

            migrationBuilder.DropForeignKey(
                name: "FK_YeuThichs_SanPhams_SanPhamMaSanPham",
                table: "YeuThichs");

            migrationBuilder.DropIndex(
                name: "IX_YeuThichs_KhachHangMaKhachHang",
                table: "YeuThichs");

            migrationBuilder.DropIndex(
                name: "IX_YeuThichs_SanPhamMaSanPham",
                table: "YeuThichs");

            migrationBuilder.DropColumn(
                name: "KhachHangMaKhachHang",
                table: "YeuThichs");

            migrationBuilder.DropColumn(
                name: "SanPhamMaSanPham",
                table: "YeuThichs");

            migrationBuilder.CreateIndex(
                name: "IX_YeuThichs_MaKhachHang",
                table: "YeuThichs",
                column: "MaKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_YeuThichs_MaSanPham",
                table: "YeuThichs",
                column: "MaSanPham");

            migrationBuilder.AddForeignKey(
                name: "FK_YeuThichs_KhachHangs_MaKhachHang",
                table: "YeuThichs",
                column: "MaKhachHang",
                principalTable: "KhachHangs",
                principalColumn: "MaKhachHang",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_YeuThichs_SanPhams_MaSanPham",
                table: "YeuThichs",
                column: "MaSanPham",
                principalTable: "SanPhams",
                principalColumn: "MaSanPham",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_YeuThichs_KhachHangs_MaKhachHang",
                table: "YeuThichs");

            migrationBuilder.DropForeignKey(
                name: "FK_YeuThichs_SanPhams_MaSanPham",
                table: "YeuThichs");

            migrationBuilder.DropIndex(
                name: "IX_YeuThichs_MaKhachHang",
                table: "YeuThichs");

            migrationBuilder.DropIndex(
                name: "IX_YeuThichs_MaSanPham",
                table: "YeuThichs");

            migrationBuilder.AddColumn<int>(
                name: "KhachHangMaKhachHang",
                table: "YeuThichs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SanPhamMaSanPham",
                table: "YeuThichs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_YeuThichs_KhachHangMaKhachHang",
                table: "YeuThichs",
                column: "KhachHangMaKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_YeuThichs_SanPhamMaSanPham",
                table: "YeuThichs",
                column: "SanPhamMaSanPham");

            migrationBuilder.AddForeignKey(
                name: "FK_YeuThichs_KhachHangs_KhachHangMaKhachHang",
                table: "YeuThichs",
                column: "KhachHangMaKhachHang",
                principalTable: "KhachHangs",
                principalColumn: "MaKhachHang");

            migrationBuilder.AddForeignKey(
                name: "FK_YeuThichs_SanPhams_SanPhamMaSanPham",
                table: "YeuThichs",
                column: "SanPhamMaSanPham",
                principalTable: "SanPhams",
                principalColumn: "MaSanPham",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
