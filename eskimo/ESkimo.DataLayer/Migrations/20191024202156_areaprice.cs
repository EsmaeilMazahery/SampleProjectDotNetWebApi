using Microsoft.EntityFrameworkCore.Migrations;

namespace ESkimo.DataLayer.Migrations
{
    public partial class areaprice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Areas_PeriodTypes_periodTypeId",
                table: "Areas");

            migrationBuilder.DropIndex(
                name: "IX_Areas_periodTypeId",
                table: "Areas");

            migrationBuilder.DropColumn(
                name: "amountSend",
                table: "Areas");

            migrationBuilder.DropColumn(
                name: "periodTypeId",
                table: "Areas");

            migrationBuilder.CreateTable(
                name: "AreaPrices",
                columns: table => new
                {
                    areaId = table.Column<int>(nullable: false),
                    periodTypeId = table.Column<int>(nullable: false),
                    amountSend = table.Column<decimal>(type: "decimal(18,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreaPrices", x => new { x.areaId, x.periodTypeId });
                    table.ForeignKey(
                        name: "FK_AreaPrices_Areas_areaId",
                        column: x => x.areaId,
                        principalTable: "Areas",
                        principalColumn: "areaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AreaPrices_PeriodTypes_periodTypeId",
                        column: x => x.periodTypeId,
                        principalTable: "PeriodTypes",
                        principalColumn: "periodTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AreaPrices_periodTypeId",
                table: "AreaPrices",
                column: "periodTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AreaPrices");

            migrationBuilder.AddColumn<decimal>(
                name: "amountSend",
                table: "Areas",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "periodTypeId",
                table: "Areas",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Areas_periodTypeId",
                table: "Areas",
                column: "periodTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Areas_PeriodTypes_periodTypeId",
                table: "Areas",
                column: "periodTypeId",
                principalTable: "PeriodTypes",
                principalColumn: "periodTypeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
