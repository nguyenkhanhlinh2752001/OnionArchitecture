using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class updateAuditTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "OrderDetails");

            migrationBuilder.RenameColumn(
                name: "LastEditBy",
                table: "Orders",
                newName: "LastModifiedBy");

            migrationBuilder.RenameColumn(
                name: "DeleledOn",
                table: "Orders",
                newName: "LastModifiedOn");

            migrationBuilder.RenameColumn(
                name: "LastEditBy",
                table: "OrderDetails",
                newName: "LastModifiedBy");

            migrationBuilder.RenameColumn(
                name: "DeleledOn",
                table: "OrderDetails",
                newName: "LastModifiedOn");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastModifiedOn",
                table: "Orders",
                newName: "DeleledOn");

            migrationBuilder.RenameColumn(
                name: "LastModifiedBy",
                table: "Orders",
                newName: "LastEditBy");

            migrationBuilder.RenameColumn(
                name: "LastModifiedOn",
                table: "OrderDetails",
                newName: "DeleledOn");

            migrationBuilder.RenameColumn(
                name: "LastModifiedBy",
                table: "OrderDetails",
                newName: "LastEditBy");

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "OrderDetails",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
