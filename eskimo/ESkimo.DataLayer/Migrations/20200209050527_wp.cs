using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ESkimo.DataLayer.Migrations
{
    public partial class wp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductPriceWholesales",
                columns: table => new
                {
                    productPriceWholesaleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    productId = table.Column<int>(nullable: false),
                    amount = table.Column<decimal>(nullable: false),
                    count = table.Column<int>(nullable: false),
                    minCountSell = table.Column<int>(nullable: false),
                    enable = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPriceWholesales", x => x.productPriceWholesaleId);
                    table.ForeignKey(
                        name: "FK_ProductPriceWholesales_Products_productId",
                        column: x => x.productId,
                        principalTable: "Products",
                        principalColumn: "productId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductPriceWholesales_productId",
                table: "ProductPriceWholesales",
                column: "productId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductPriceWholesales");
        }
    }
}
