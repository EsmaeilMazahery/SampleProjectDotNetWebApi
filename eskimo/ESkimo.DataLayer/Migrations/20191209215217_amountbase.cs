using Microsoft.EntityFrameworkCore.Migrations;

namespace ESkimo.DataLayer.Migrations
{
    public partial class amountbase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "amountBase",
                table: "ProductPrices");

            migrationBuilder.DropColumn(
                name: "name",
                table: "ProductPrices");

            migrationBuilder.AddColumn<decimal>(
                name: "amountBase",
                table: "Products",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "amountBase",
                table: "Products");

            migrationBuilder.AddColumn<decimal>(
                name: "amountBase",
                table: "ProductPrices",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "ProductPrices",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");
        }
    }
}
