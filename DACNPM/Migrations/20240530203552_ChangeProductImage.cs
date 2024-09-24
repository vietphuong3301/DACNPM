using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DACNPM.Migrations
{
    public partial class ChangeProductImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductImageURL",
                table: "Products",
                newName: "ProductImage");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductImage",
                table: "Products",
                newName: "ProductImageURL");
        }
    }
}
