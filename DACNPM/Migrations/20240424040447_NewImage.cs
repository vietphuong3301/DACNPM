using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DACNPM.Migrations
{
    public partial class NewImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductImage1",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductImage2",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductImage3",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductImage1",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductImage2",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductImage3",
                table: "Products");
        }
    }
}
