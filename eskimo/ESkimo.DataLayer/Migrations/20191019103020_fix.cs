using Microsoft.EntityFrameworkCore.Migrations;

namespace ESkimo.DataLayer.Migrations
{
    public partial class fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "_maxRegisterDate",
                table: "DiscountCodes");

            migrationBuilder.RenameColumn(
                name: "countSell",
                table: "DiscountCodes",
                newName: "maxRegisterDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "maxRegisterDate",
                table: "DiscountCodes",
                newName: "countSell");

            migrationBuilder.AddColumn<long>(
                name: "_maxRegisterDate",
                table: "DiscountCodes",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
