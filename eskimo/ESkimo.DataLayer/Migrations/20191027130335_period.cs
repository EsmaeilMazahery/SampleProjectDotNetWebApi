using Microsoft.EntityFrameworkCore.Migrations;

namespace ESkimo.DataLayer.Migrations
{
    public partial class period : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "memberId",
                table: "MemberOrderPeriods",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MemberOrderPeriods_memberId",
                table: "MemberOrderPeriods",
                column: "memberId");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberOrderPeriods_Members_memberId",
                table: "MemberOrderPeriods",
                column: "memberId",
                principalTable: "Members",
                principalColumn: "memberId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberOrderPeriods_Members_memberId",
                table: "MemberOrderPeriods");

            migrationBuilder.DropIndex(
                name: "IX_MemberOrderPeriods_memberId",
                table: "MemberOrderPeriods");

            migrationBuilder.DropColumn(
                name: "memberId",
                table: "MemberOrderPeriods");
        }
    }
}
