using Microsoft.EntityFrameworkCore.Migrations;

namespace ESkimo.DataLayer.Migrations
{
    public partial class desmember : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Location",
                table: "MemberLocations",
                newName: "location");

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "Members",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "zoom",
                table: "MemberLocations",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "description",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "zoom",
                table: "MemberLocations");

            migrationBuilder.RenameColumn(
                name: "location",
                table: "MemberLocations",
                newName: "Location");
        }
    }
}
