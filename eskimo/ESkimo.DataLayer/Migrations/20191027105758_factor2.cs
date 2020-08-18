using Microsoft.EntityFrameworkCore.Migrations;

namespace ESkimo.DataLayer.Migrations
{
    public partial class factor2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "memberOrderPeriodId",
                table: "Factors",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "memberOrderPeriodId",
                table: "Factors",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
