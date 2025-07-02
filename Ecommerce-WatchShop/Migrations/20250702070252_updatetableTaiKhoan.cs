using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce_WatchShop.Migrations
{
    /// <inheritdoc />
    public partial class updatetableTaiKhoan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KhachHangs_TaiKhoans_TaiKhoanMaTaiKhoan",
                table: "KhachHangs");

            migrationBuilder.DropIndex(
                name: "IX_KhachHangs_TaiKhoanMaTaiKhoan",
                table: "KhachHangs");

            migrationBuilder.DropColumn(
                name: "TaiKhoanMaTaiKhoan",
                table: "KhachHangs");

            migrationBuilder.CreateIndex(
                name: "IX_KhachHangs_MaTaiKhoan",
                table: "KhachHangs",
                column: "MaTaiKhoan",
                unique: true,
                filter: "[MaTaiKhoan] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_KhachHangs_TaiKhoans_MaTaiKhoan",
                table: "KhachHangs",
                column: "MaTaiKhoan",
                principalTable: "TaiKhoans",
                principalColumn: "MaTaiKhoan");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KhachHangs_TaiKhoans_MaTaiKhoan",
                table: "KhachHangs");

            migrationBuilder.DropIndex(
                name: "IX_KhachHangs_MaTaiKhoan",
                table: "KhachHangs");

            migrationBuilder.AddColumn<int>(
                name: "TaiKhoanMaTaiKhoan",
                table: "KhachHangs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_KhachHangs_TaiKhoanMaTaiKhoan",
                table: "KhachHangs",
                column: "TaiKhoanMaTaiKhoan");

            migrationBuilder.AddForeignKey(
                name: "FK_KhachHangs_TaiKhoans_TaiKhoanMaTaiKhoan",
                table: "KhachHangs",
                column: "TaiKhoanMaTaiKhoan",
                principalTable: "TaiKhoans",
                principalColumn: "MaTaiKhoan");
        }
    }
}
