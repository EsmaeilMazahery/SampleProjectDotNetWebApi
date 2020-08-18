using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ESkimo.DataLayer.Migrations
{
    public partial class factorstatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "dateTime",
                table: "PocketPosts",
                newName: "sendDateTime");

            migrationBuilder.AddColumn<int>(
                name: "count",
                table: "PocketPosts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "PocketPosts",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "registerDateTime",
                table: "PocketPosts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "userSenderId",
                table: "PocketPosts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte>(
                name: "status",
                table: "Factors",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateIndex(
                name: "IX_PocketPosts_userSenderId",
                table: "PocketPosts",
                column: "userSenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_PocketPosts_Users_userSenderId",
                table: "PocketPosts",
                column: "userSenderId",
                principalTable: "Users",
                principalColumn: "userId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PocketPosts_Users_userSenderId",
                table: "PocketPosts");

            migrationBuilder.DropIndex(
                name: "IX_PocketPosts_userSenderId",
                table: "PocketPosts");

            migrationBuilder.DropColumn(
                name: "count",
                table: "PocketPosts");

            migrationBuilder.DropColumn(
                name: "description",
                table: "PocketPosts");

            migrationBuilder.DropColumn(
                name: "registerDateTime",
                table: "PocketPosts");

            migrationBuilder.DropColumn(
                name: "userSenderId",
                table: "PocketPosts");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Factors");

            migrationBuilder.RenameColumn(
                name: "sendDateTime",
                table: "PocketPosts",
                newName: "dateTime");
        }
    }
}
