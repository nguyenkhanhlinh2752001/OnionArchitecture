using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class updateDbv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ImageProductDetails",
                table: "ImageProductDetails");

            migrationBuilder.RenameTable(
                name: "ImageProductDetails",
                newName: "ImageProducts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ImageProducts",
                table: "ImageProducts",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ImageProducts",
                table: "ImageProducts");

            migrationBuilder.RenameTable(
                name: "ImageProducts",
                newName: "ImageProductDetails");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ImageProductDetails",
                table: "ImageProductDetails",
                column: "Id");
        }
    }
}
