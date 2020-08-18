using Microsoft.EntityFrameworkCore.Migrations;

namespace ESkimo.DataLayer.Migrations
{
    public partial class address : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "phone",
                table: "MemberLocations",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "postalCode",
                table: "MemberLocations",
                maxLength: 1000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "phone",
                table: "MemberLocations");

            migrationBuilder.DropColumn(
                name: "postalCode",
                table: "MemberLocations");
        }
    }
}
