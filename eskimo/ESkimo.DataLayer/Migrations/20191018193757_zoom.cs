using GeoAPI.Geometries;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ESkimo.DataLayer.Migrations
{
    public partial class zoom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Location",
                table: "Areas",
                newName: "location");

            migrationBuilder.AlterColumn<IPoint>(
                name: "location",
                table: "Areas",
                type: "geography",
                nullable: true,
                oldClrType: typeof(IPoint),
                oldNullable: true);

            migrationBuilder.AddColumn<float>(
                name: "zoom",
                table: "Areas",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "zoom",
                table: "Areas");

            migrationBuilder.RenameColumn(
                name: "location",
                table: "Areas",
                newName: "Location");

            migrationBuilder.AlterColumn<IPoint>(
                name: "Location",
                table: "Areas",
                nullable: true,
                oldClrType: typeof(IPoint),
                oldType: "geography (point)",
                oldNullable: true);
        }
    }
}
