using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce_CaFeShop.Migrations
{
    /// <inheritdoc />
    public partial class data3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NgaySinh",
                table: "KhachHangs");

            migrationBuilder.AddColumn<string>(
                name: "LyDoHuy",
                table: "HoaDons",
                type: "nvarchar(500)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LyDoHuy",
                table: "HoaDons");

            migrationBuilder.AddColumn<DateOnly>(
                name: "NgaySinh",
                table: "KhachHangs",
                type: "date",
                nullable: true);
        }
    }
}
