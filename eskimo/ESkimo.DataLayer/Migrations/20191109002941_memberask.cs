using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ESkimo.DataLayer.Migrations
{
    public partial class memberask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MemberAsks",
                columns: table => new
                {
                    memberAskId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(maxLength: 1000, nullable: false),
                    family = table.Column<string>(maxLength: 1000, nullable: false),
                    mobile = table.Column<string>(maxLength: 10, nullable: false),
                    email = table.Column<string>(maxLength: 1000, nullable: true),
                    description = table.Column<string>(maxLength: 4000, nullable: false),
                    answer = table.Column<string>(maxLength: 4000, nullable: true),
                    read = table.Column<bool>(nullable: false),
                    registerDate = table.Column<DateTime>(nullable: false),
                    type = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberAsks", x => x.memberAskId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberAsks");
        }
    }
}
