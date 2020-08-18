using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ESkimo.DataLayer.Migrations
{
    public partial class smslog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "enable",
                table: "blogCategories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "SmsLogs",
                columns: table => new
                {
                    smsLogId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    message = table.Column<string>(nullable: true),
                    receptor = table.Column<string>(nullable: true),
                    dateTime = table.Column<DateTime>(nullable: false),
                    status = table.Column<int>(nullable: false),
                    memberId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsLogs", x => x.smsLogId);
                    table.ForeignKey(
                        name: "FK_SmsLogs_Members_memberId",
                        column: x => x.memberId,
                        principalTable: "Members",
                        principalColumn: "memberId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SmsLogs_memberId",
                table: "SmsLogs",
                column: "memberId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SmsLogs");

            migrationBuilder.DropColumn(
                name: "enable",
                table: "blogCategories");
        }
    }
}
