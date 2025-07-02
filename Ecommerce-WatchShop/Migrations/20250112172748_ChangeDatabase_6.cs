using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce_WatchShop.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDatabase_6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "Abouts");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Abouts",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Abouts",
                newName: "Email");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Abouts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Policies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Policies", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Policies");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Abouts");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Abouts",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Abouts",
                newName: "Title");

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "Abouts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
