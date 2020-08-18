using Microsoft.EntityFrameworkCore.Migrations;

namespace ESkimo.DataLayer.Migrations
{
    public partial class areaday : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "sendDay",
                table: "Areas");

            migrationBuilder.AddColumn<string>(
                name: "sendDaies",
                table: "Areas",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "sendDaies",
                table: "Areas");

            migrationBuilder.AddColumn<int>(
                name: "sendDay",
                table: "Areas",
                nullable: false,
                defaultValue: 0);
        }
    }
}
