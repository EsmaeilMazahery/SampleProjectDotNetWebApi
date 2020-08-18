using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ESkimo.DataLayer.Migrations
{
    public partial class blog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Members",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "family",
                table: "Members",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 1000);

            migrationBuilder.AddColumn<bool>(
                name: "verifyMobile",
                table: "Members",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "blogCategories",
                columns: table => new
                {
                    blogCategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    title = table.Column<string>(maxLength: 2500, nullable: true),
                    description = table.Column<string>(maxLength: 4000, nullable: true),
                    parentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blogCategories", x => x.blogCategoryId);
                    table.ForeignKey(
                        name: "FK_blogCategories_blogCategories_parentId",
                        column: x => x.parentId,
                        principalTable: "blogCategories",
                        principalColumn: "blogCategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "blogPosts",
                columns: table => new
                {
                    blogPostId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    title = table.Column<string>(maxLength: 2500, nullable: false),
                    userId = table.Column<int>(nullable: false),
                    image = table.Column<string>(maxLength: 1000, nullable: true),
                    content = table.Column<string>(nullable: true),
                    publishDate = table.Column<DateTime>(nullable: false),
                    registerDateTime = table.Column<DateTime>(nullable: false),
                    url = table.Column<string>(maxLength: 1000, nullable: true),
                    enable = table.Column<bool>(nullable: false),
                    enableComment = table.Column<bool>(nullable: false),
                    blogCategoryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blogPosts", x => x.blogPostId);
                    table.ForeignKey(
                        name: "FK_blogPosts_blogCategories_blogCategoryId",
                        column: x => x.blogCategoryId,
                        principalTable: "blogCategories",
                        principalColumn: "blogCategoryId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_blogPosts_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "blogComments",
                columns: table => new
                {
                    blogCommentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    body = table.Column<string>(maxLength: 4000, nullable: false),
                    registerDate = table.Column<DateTime>(nullable: false),
                    enable = table.Column<bool>(nullable: false),
                    memberName = table.Column<string>(maxLength: 1000, nullable: true),
                    memberEmail = table.Column<string>(maxLength: 1000, nullable: true),
                    memberMobile = table.Column<string>(maxLength: 1000, nullable: true),
                    memberId = table.Column<int>(nullable: true),
                    userId = table.Column<int>(nullable: true),
                    blogPostId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blogComments", x => x.blogCommentId);
                    table.ForeignKey(
                        name: "FK_blogComments_blogPosts_blogPostId",
                        column: x => x.blogPostId,
                        principalTable: "blogPosts",
                        principalColumn: "blogPostId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_blogComments_Members_memberId",
                        column: x => x.memberId,
                        principalTable: "Members",
                        principalColumn: "memberId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_blogComments_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_blogCategories_parentId",
                table: "blogCategories",
                column: "parentId");

            migrationBuilder.CreateIndex(
                name: "IX_blogComments_blogPostId",
                table: "blogComments",
                column: "blogPostId");

            migrationBuilder.CreateIndex(
                name: "IX_blogComments_memberId",
                table: "blogComments",
                column: "memberId");

            migrationBuilder.CreateIndex(
                name: "IX_blogComments_userId",
                table: "blogComments",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_blogPosts_blogCategoryId",
                table: "blogPosts",
                column: "blogCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_blogPosts_userId",
                table: "blogPosts",
                column: "userId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "blogComments");

            migrationBuilder.DropTable(
                name: "blogPosts");

            migrationBuilder.DropTable(
                name: "blogCategories");

            migrationBuilder.DropColumn(
                name: "verifyMobile",
                table: "Members");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Members",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "family",
                table: "Members",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 1000,
                oldNullable: true);
        }
    }
}
