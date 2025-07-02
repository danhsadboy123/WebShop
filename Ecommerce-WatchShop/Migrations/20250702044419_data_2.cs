using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce_WatchShop.Migrations
{
    /// <inheritdoc />
    public partial class data_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SanPhams_DanhMucs_DanhMucMaDanhMuc",
                table: "SanPhams");

            migrationBuilder.DropForeignKey(
                name: "FK_SanPhams_ThuongHieus_ThuongHieuMaThuongHieu",
                table: "SanPhams");

            migrationBuilder.DropIndex(
                name: "IX_SanPhams_DanhMucMaDanhMuc",
                table: "SanPhams");

            migrationBuilder.DropIndex(
                name: "IX_SanPhams_ThuongHieuMaThuongHieu",
                table: "SanPhams");

            migrationBuilder.DropColumn(
                name: "DanhMucMaDanhMuc",
                table: "SanPhams");

            migrationBuilder.DropColumn(
                name: "ThuongHieuMaThuongHieu",
                table: "SanPhams");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhams_MaDanhMuc",
                table: "SanPhams",
                column: "MaDanhMuc");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhams_MaThuongHieu",
                table: "SanPhams",
                column: "MaThuongHieu");

            migrationBuilder.AddForeignKey(
                name: "FK_SanPhams_DanhMucs_MaDanhMuc",
                table: "SanPhams",
                column: "MaDanhMuc",
                principalTable: "DanhMucs",
                principalColumn: "MaDanhMuc",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SanPhams_ThuongHieus_MaThuongHieu",
                table: "SanPhams",
                column: "MaThuongHieu",
                principalTable: "ThuongHieus",
                principalColumn: "MaThuongHieu",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SanPhams_DanhMucs_MaDanhMuc",
                table: "SanPhams");

            migrationBuilder.DropForeignKey(
                name: "FK_SanPhams_ThuongHieus_MaThuongHieu",
                table: "SanPhams");

            migrationBuilder.DropIndex(
                name: "IX_SanPhams_MaDanhMuc",
                table: "SanPhams");

            migrationBuilder.DropIndex(
                name: "IX_SanPhams_MaThuongHieu",
                table: "SanPhams");

            migrationBuilder.AddColumn<int>(
                name: "DanhMucMaDanhMuc",
                table: "SanPhams",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ThuongHieuMaThuongHieu",
                table: "SanPhams",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SanPhams_DanhMucMaDanhMuc",
                table: "SanPhams",
                column: "DanhMucMaDanhMuc");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhams_ThuongHieuMaThuongHieu",
                table: "SanPhams",
                column: "ThuongHieuMaThuongHieu");

            migrationBuilder.AddForeignKey(
                name: "FK_SanPhams_DanhMucs_DanhMucMaDanhMuc",
                table: "SanPhams",
                column: "DanhMucMaDanhMuc",
                principalTable: "DanhMucs",
                principalColumn: "MaDanhMuc");

            migrationBuilder.AddForeignKey(
                name: "FK_SanPhams_ThuongHieus_ThuongHieuMaThuongHieu",
                table: "SanPhams",
                column: "ThuongHieuMaThuongHieu",
                principalTable: "ThuongHieus",
                principalColumn: "MaThuongHieu");
        }
    }
}
