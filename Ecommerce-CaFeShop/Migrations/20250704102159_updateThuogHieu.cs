using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce_CaFeShop.Migrations
{
    /// <inheritdoc />
    public partial class updateThuogHieu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaDanhMuc",
                table: "ThuongHieus",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MoTa",
                table: "ThuongHieus",
                type: "nvarchar(500)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrangThai",
                table: "ThuongHieus",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ThuongHieus_MaDanhMuc",
                table: "ThuongHieus",
                column: "MaDanhMuc");

            migrationBuilder.AddForeignKey(
                name: "FK_ThuongHieus_DanhMucs_MaDanhMuc",
                table: "ThuongHieus",
                column: "MaDanhMuc",
                principalTable: "DanhMucs",
                principalColumn: "MaDanhMuc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ThuongHieus_DanhMucs_MaDanhMuc",
                table: "ThuongHieus");

            migrationBuilder.DropIndex(
                name: "IX_ThuongHieus_MaDanhMuc",
                table: "ThuongHieus");

            migrationBuilder.DropColumn(
                name: "MaDanhMuc",
                table: "ThuongHieus");

            migrationBuilder.DropColumn(
                name: "MoTa",
                table: "ThuongHieus");

            migrationBuilder.DropColumn(
                name: "TrangThai",
                table: "ThuongHieus");
        }
    }
}
