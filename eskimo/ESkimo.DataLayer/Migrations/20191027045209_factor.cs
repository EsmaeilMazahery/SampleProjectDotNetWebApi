using Microsoft.EntityFrameworkCore.Migrations;

namespace ESkimo.DataLayer.Migrations
{
    public partial class factor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberOrderPeriods_Factors_targetFactorId",
                table: "MemberOrderPeriods");

            migrationBuilder.DropIndex(
                name: "IX_Payments_memberOrderPeriodId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_MemberOrderPeriods_targetFactorId",
                table: "MemberOrderPeriods");

            migrationBuilder.DropColumn(
                name: "paymentId",
                table: "MemberOrderPeriods");

            migrationBuilder.DropColumn(
                name: "targetFactorId",
                table: "MemberOrderPeriods");

            migrationBuilder.AlterColumn<decimal>(
                name: "maxDiscount",
                table: "PeriodTypes",
                type: "decimal(18,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.AddColumn<int>(
                name: "memberOrderPeriodId",
                table: "Factors",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "percent",
                table: "DiscountCodes",
                type: "decimal(18,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.AlterColumn<int>(
                name: "maxRegisterDate",
                table: "DiscountCodes",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<decimal>(
                name: "maxDiscount",
                table: "DiscountCodes",
                type: "decimal(18,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "discount",
                table: "DiscountCodes",
                type: "decimal(18,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_memberOrderPeriodId",
                table: "Payments",
                column: "memberOrderPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_Factors_memberOrderPeriodId",
                table: "Factors",
                column: "memberOrderPeriodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Factors_MemberOrderPeriods_memberOrderPeriodId",
                table: "Factors",
                column: "memberOrderPeriodId",
                principalTable: "MemberOrderPeriods",
                principalColumn: "memberOrderPeriodId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Factors_MemberOrderPeriods_memberOrderPeriodId",
                table: "Factors");

            migrationBuilder.DropIndex(
                name: "IX_Payments_memberOrderPeriodId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Factors_memberOrderPeriodId",
                table: "Factors");

            migrationBuilder.DropColumn(
                name: "memberOrderPeriodId",
                table: "Factors");

            migrationBuilder.AlterColumn<decimal>(
                name: "maxDiscount",
                table: "PeriodTypes",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "paymentId",
                table: "MemberOrderPeriods",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "targetFactorId",
                table: "MemberOrderPeriods",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "percent",
                table: "DiscountCodes",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "maxRegisterDate",
                table: "DiscountCodes",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "maxDiscount",
                table: "DiscountCodes",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "discount",
                table: "DiscountCodes",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_memberOrderPeriodId",
                table: "Payments",
                column: "memberOrderPeriodId",
                unique: true,
                filter: "[memberOrderPeriodId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MemberOrderPeriods_targetFactorId",
                table: "MemberOrderPeriods",
                column: "targetFactorId");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberOrderPeriods_Factors_targetFactorId",
                table: "MemberOrderPeriods",
                column: "targetFactorId",
                principalTable: "Factors",
                principalColumn: "factorId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
