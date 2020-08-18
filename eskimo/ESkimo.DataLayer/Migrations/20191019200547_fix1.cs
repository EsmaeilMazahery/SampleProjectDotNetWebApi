using Microsoft.EntityFrameworkCore.Migrations;

namespace ESkimo.DataLayer.Migrations
{
    public partial class fix1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "_maxRegisterDate",
                table: "DiscountFactors");

            migrationBuilder.RenameColumn(
                name: "precent",
                table: "DiscountFactors",
                newName: "percent");

            migrationBuilder.RenameColumn(
                name: "minAmount",
                table: "DiscountFactors",
                newName: "minPrice");

            migrationBuilder.AddColumn<int>(
                name: "maxRegisterDate",
                table: "DiscountFactors",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "maxRegisterDate",
                table: "DiscountFactors");

            migrationBuilder.RenameColumn(
                name: "percent",
                table: "DiscountFactors",
                newName: "precent");

            migrationBuilder.RenameColumn(
                name: "minPrice",
                table: "DiscountFactors",
                newName: "minAmount");

            migrationBuilder.AddColumn<long>(
                name: "_maxRegisterDate",
                table: "DiscountFactors",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
