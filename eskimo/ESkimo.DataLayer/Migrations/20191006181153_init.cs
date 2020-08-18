using System;
using GeoAPI.Geometries;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ESkimo.DataLayer.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    brandId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(maxLength: 1000, nullable: false),
                    image = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.brandId);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    categoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(maxLength: 1000, nullable: false),
                    parentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.categoryId);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_parentId",
                        column: x => x.parentId,
                        principalTable: "Categories",
                        principalColumn: "categoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DiscountFactors",
                columns: table => new
                {
                    discountFactorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    minAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    precent = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    maxDiscount = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    discount = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    startDate = table.Column<DateTime>(nullable: true),
                    endDate = table.Column<DateTime>(nullable: true),
                    _maxRegisterDate = table.Column<long>(nullable: false),
                    enable = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountFactors", x => x.discountFactorId);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    memberId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(maxLength: 1000, nullable: false),
                    family = table.Column<string>(maxLength: 1000, nullable: false),
                    password = table.Column<string>(maxLength: 1000, nullable: false),
                    mobile = table.Column<string>(maxLength: 10, nullable: false),
                    email = table.Column<string>(maxLength: 1000, nullable: true),
                    amount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    sumPayment = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    sumFactors = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    enable = table.Column<bool>(nullable: false),
                    registerDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.memberId);
                });

            migrationBuilder.CreateTable(
                name: "PeriodTypes",
                columns: table => new
                {
                    periodTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(maxLength: 1000, nullable: false),
                    day = table.Column<int>(nullable: false),
                    month = table.Column<int>(nullable: false),
                    percentDiscount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    maxDiscount = table.Column<decimal>(type: "decimal(18,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeriodTypes", x => x.periodTypeId);
                });

            migrationBuilder.CreateTable(
                name: "ProductTypes",
                columns: table => new
                {
                    productTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTypes", x => x.productTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    roleKey = table.Column<byte>(nullable: false),
                    name = table.Column<string>(maxLength: 1000, nullable: false),
                    description = table.Column<string>(maxLength: 4000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.roleKey);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    SettingType = table.Column<byte>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.SettingType);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    userId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    username = table.Column<string>(maxLength: 1000, nullable: false),
                    name = table.Column<string>(maxLength: 1000, nullable: false),
                    family = table.Column<string>(maxLength: 1000, nullable: false),
                    image = table.Column<string>(maxLength: 1000, nullable: true),
                    password = table.Column<string>(maxLength: 1000, nullable: false),
                    newPassword = table.Column<string>(maxLength: 1000, nullable: true),
                    mobile = table.Column<string>(maxLength: 10, nullable: false),
                    email = table.Column<string>(maxLength: 1000, nullable: true),
                    enable = table.Column<bool>(nullable: false),
                    registerDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.userId);
                });

            migrationBuilder.CreateTable(
                name: "Rel_CategoryBrand",
                columns: table => new
                {
                    categoryId = table.Column<int>(nullable: false),
                    brandId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rel_CategoryBrand", x => new { x.brandId, x.categoryId });
                    table.ForeignKey(
                        name: "FK_Rel_CategoryBrand_Brands_brandId",
                        column: x => x.brandId,
                        principalTable: "Brands",
                        principalColumn: "brandId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rel_CategoryBrand_Categories_categoryId",
                        column: x => x.categoryId,
                        principalTable: "Categories",
                        principalColumn: "categoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiscountCodes",
                columns: table => new
                {
                    discountCodeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(maxLength: 1000, nullable: false),
                    code = table.Column<string>(maxLength: 1000, nullable: false),
                    countSell = table.Column<int>(nullable: false),
                    minPrice = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    maxDiscount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    percent = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    discount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    startDate = table.Column<DateTime>(nullable: true),
                    endDate = table.Column<DateTime>(nullable: true),
                    countUse = table.Column<int>(nullable: true),
                    _maxRegisterDate = table.Column<long>(nullable: false),
                    activeAlone = table.Column<bool>(nullable: false),
                    enable = table.Column<bool>(nullable: false),
                    memberId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountCodes", x => x.discountCodeId);
                    table.ForeignKey(
                        name: "FK_DiscountCodes_Members_memberId",
                        column: x => x.memberId,
                        principalTable: "Members",
                        principalColumn: "memberId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    areaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(maxLength: 1000, nullable: false),
                    address = table.Column<string>(maxLength: 1000, nullable: true),
                    Location = table.Column<IPoint>(nullable: true),
                    amountSend = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    sendDay = table.Column<int>(nullable: false),
                    periodTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.areaId);
                    table.ForeignKey(
                        name: "FK_Areas_PeriodTypes_periodTypeId",
                        column: x => x.periodTypeId,
                        principalTable: "PeriodTypes",
                        principalColumn: "periodTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    productId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(maxLength: 1000, nullable: false),
                    imageAddress = table.Column<string>(maxLength: 1000, nullable: true),
                    enable = table.Column<bool>(nullable: false),
                    attributes = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    brandId = table.Column<int>(nullable: false),
                    categoryId = table.Column<int>(nullable: false),
                    productTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.productId);
                    table.ForeignKey(
                        name: "FK_Products_Brands_brandId",
                        column: x => x.brandId,
                        principalTable: "Brands",
                        principalColumn: "brandId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Categories_categoryId",
                        column: x => x.categoryId,
                        principalTable: "Categories",
                        principalColumn: "categoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_ProductTypes_productTypeId",
                        column: x => x.productTypeId,
                        principalTable: "ProductTypes",
                        principalColumn: "productTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Rel_CategoryProductType",
                columns: table => new
                {
                    categoryId = table.Column<int>(nullable: false),
                    productTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rel_CategoryProductType", x => new { x.categoryId, x.productTypeId });
                    table.ForeignKey(
                        name: "FK_Rel_CategoryProductType_Categories_categoryId",
                        column: x => x.categoryId,
                        principalTable: "Categories",
                        principalColumn: "categoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rel_CategoryProductType_ProductTypes_productTypeId",
                        column: x => x.productTypeId,
                        principalTable: "ProductTypes",
                        principalColumn: "productTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PocketPosts",
                columns: table => new
                {
                    pocketPostId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    dateTime = table.Column<DateTime>(nullable: false),
                    amount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    userId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PocketPosts", x => x.pocketPostId);
                    table.ForeignKey(
                        name: "FK_PocketPosts_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Rel_RoleUser",
                columns: table => new
                {
                    userId = table.Column<int>(nullable: false),
                    roleId = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rel_RoleUser", x => new { x.userId, x.roleId });
                    table.UniqueConstraint("AK_Rel_RoleUser_roleId_userId", x => new { x.roleId, x.userId });
                    table.ForeignKey(
                        name: "FK_Rel_RoleUser_Roles_roleId",
                        column: x => x.roleId,
                        principalTable: "Roles",
                        principalColumn: "roleKey",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rel_RoleUser_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSessionUpdates",
                columns: table => new
                {
                    userSessionUpdateId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    userId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSessionUpdates", x => x.userSessionUpdateId);
                    table.ForeignKey(
                        name: "FK_UserSessionUpdates_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSmsMessages",
                columns: table => new
                {
                    userSmsMessageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    registerDateTime = table.Column<DateTime>(nullable: false),
                    text = table.Column<string>(maxLength: 1000, nullable: false),
                    sendType = table.Column<byte>(nullable: false),
                    refId = table.Column<string>(maxLength: 100, nullable: true),
                    sendStatus = table.Column<byte>(nullable: false),
                    senderNumber = table.Column<string>(maxLength: 100, nullable: true),
                    receiverNumber = table.Column<string>(maxLength: 10, nullable: true),
                    tryCount = table.Column<byte>(nullable: false),
                    userId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSmsMessages", x => x.userSmsMessageId);
                    table.ForeignKey(
                        name: "FK_UserSmsMessages_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rel_DiscountCodeBrand",
                columns: table => new
                {
                    discountCodeId = table.Column<int>(nullable: false),
                    brandId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rel_DiscountCodeBrand", x => new { x.discountCodeId, x.brandId });
                    table.UniqueConstraint("AK_Rel_DiscountCodeBrand_brandId_discountCodeId", x => new { x.brandId, x.discountCodeId });
                    table.ForeignKey(
                        name: "FK_Rel_DiscountCodeBrand_Brands_brandId",
                        column: x => x.brandId,
                        principalTable: "Brands",
                        principalColumn: "brandId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rel_DiscountCodeBrand_DiscountCodes_discountCodeId",
                        column: x => x.discountCodeId,
                        principalTable: "DiscountCodes",
                        principalColumn: "discountCodeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rel_DiscountCodeCategory",
                columns: table => new
                {
                    discountCodeId = table.Column<int>(nullable: false),
                    categoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rel_DiscountCodeCategory", x => new { x.discountCodeId, x.categoryId });
                    table.UniqueConstraint("AK_Rel_DiscountCodeCategory_categoryId_discountCodeId", x => new { x.categoryId, x.discountCodeId });
                    table.ForeignKey(
                        name: "FK_Rel_DiscountCodeCategory_Categories_categoryId",
                        column: x => x.categoryId,
                        principalTable: "Categories",
                        principalColumn: "categoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rel_DiscountCodeCategory_DiscountCodes_discountCodeId",
                        column: x => x.discountCodeId,
                        principalTable: "DiscountCodes",
                        principalColumn: "discountCodeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemberLocations",
                columns: table => new
                {
                    memberLocationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(maxLength: 1000, nullable: false),
                    address = table.Column<string>(maxLength: 1000, nullable: false),
                    Location = table.Column<IPoint>(nullable: true),
                    memberId = table.Column<int>(nullable: false),
                    areaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberLocations", x => x.memberLocationId);
                    table.ForeignKey(
                        name: "FK_MemberLocations_Areas_areaId",
                        column: x => x.areaId,
                        principalTable: "Areas",
                        principalColumn: "areaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberLocations_Members_memberId",
                        column: x => x.memberId,
                        principalTable: "Members",
                        principalColumn: "memberId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    commentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    dateTime = table.Column<DateTime>(nullable: false),
                    text = table.Column<string>(maxLength: 4000, nullable: false),
                    productId = table.Column<int>(nullable: false),
                    memberId = table.Column<int>(nullable: false),
                    confirm = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.commentId);
                    table.ForeignKey(
                        name: "FK_Comments_Members_memberId",
                        column: x => x.memberId,
                        principalTable: "Members",
                        principalColumn: "memberId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Products_productId",
                        column: x => x.productId,
                        principalTable: "Products",
                        principalColumn: "productId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductPrices",
                columns: table => new
                {
                    productPriceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(maxLength: 1000, nullable: false),
                    productId = table.Column<int>(nullable: false),
                    amountBase = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    count = table.Column<int>(nullable: false),
                    minCountSell = table.Column<int>(nullable: false),
                    enable = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPrices", x => x.productPriceId);
                    table.ForeignKey(
                        name: "FK_ProductPrices_Products_productId",
                        column: x => x.productId,
                        principalTable: "Products",
                        principalColumn: "productId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Factors",
                columns: table => new
                {
                    factorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    amount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    amountSend = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    tax = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    discountOfFactor = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    discountOfCode = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    discountOfPeriod = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    dateTime = table.Column<DateTime>(nullable: false),
                    sendDateTime = table.Column<DateTime>(nullable: false),
                    sent = table.Column<bool>(nullable: false),
                    periodTypeId = table.Column<int>(nullable: true),
                    memberId = table.Column<int>(nullable: false),
                    discountFactorId = table.Column<int>(nullable: true),
                    discountCodeName = table.Column<string>(maxLength: 1000, nullable: false),
                    discountCodeId = table.Column<int>(nullable: true),
                    memberLocationId = table.Column<int>(nullable: false),
                    pocketPostId = table.Column<int>(nullable: true),
                    paymentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factors", x => x.factorId);
                    table.ForeignKey(
                        name: "FK_Factors_DiscountCodes_discountCodeId",
                        column: x => x.discountCodeId,
                        principalTable: "DiscountCodes",
                        principalColumn: "discountCodeId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Factors_DiscountFactors_discountFactorId",
                        column: x => x.discountFactorId,
                        principalTable: "DiscountFactors",
                        principalColumn: "discountFactorId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Factors_Members_memberId",
                        column: x => x.memberId,
                        principalTable: "Members",
                        principalColumn: "memberId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Factors_MemberLocations_memberLocationId",
                        column: x => x.memberLocationId,
                        principalTable: "MemberLocations",
                        principalColumn: "memberLocationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Factors_PeriodTypes_periodTypeId",
                        column: x => x.periodTypeId,
                        principalTable: "PeriodTypes",
                        principalColumn: "periodTypeId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Factors_PocketPosts_pocketPostId",
                        column: x => x.pocketPostId,
                        principalTable: "PocketPosts",
                        principalColumn: "pocketPostId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "FactorItems",
                columns: table => new
                {
                    factorItemId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(maxLength: 1000, nullable: false),
                    amountBasePerItem = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    amountPerItem = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    count = table.Column<int>(nullable: false),
                    amount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    productPriceId = table.Column<int>(nullable: true),
                    factorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactorItems", x => x.factorItemId);
                    table.ForeignKey(
                        name: "FK_FactorItems_Factors_factorId",
                        column: x => x.factorId,
                        principalTable: "Factors",
                        principalColumn: "factorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FactorItems_ProductPrices_productPriceId",
                        column: x => x.productPriceId,
                        principalTable: "ProductPrices",
                        principalColumn: "productPriceId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "MemberOrderPeriods",
                columns: table => new
                {
                    memberOrderPeriodId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    payType = table.Column<int>(nullable: false),
                    paymentId = table.Column<int>(nullable: true),
                    targetFactorId = table.Column<int>(nullable: false),
                    periodTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberOrderPeriods", x => x.memberOrderPeriodId);
                    table.ForeignKey(
                        name: "FK_MemberOrderPeriods_PeriodTypes_periodTypeId",
                        column: x => x.periodTypeId,
                        principalTable: "PeriodTypes",
                        principalColumn: "periodTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberOrderPeriods_Factors_targetFactorId",
                        column: x => x.targetFactorId,
                        principalTable: "Factors",
                        principalColumn: "factorId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    paymentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    description = table.Column<string>(maxLength: 4000, nullable: true),
                    paymentType = table.Column<byte>(nullable: false),
                    success = table.Column<bool>(nullable: false),
                    trackingCode = table.Column<string>(maxLength: 1000, nullable: true),
                    amount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    dateTime = table.Column<DateTime>(nullable: false),
                    memberId = table.Column<int>(nullable: false),
                    factorId = table.Column<int>(nullable: true),
                    memberOrderPeriodId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.paymentId);
                    table.ForeignKey(
                        name: "FK_Payments_Factors_factorId",
                        column: x => x.factorId,
                        principalTable: "Factors",
                        principalColumn: "factorId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Payments_Members_memberId",
                        column: x => x.memberId,
                        principalTable: "Members",
                        principalColumn: "memberId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_MemberOrderPeriods_memberOrderPeriodId",
                        column: x => x.memberOrderPeriodId,
                        principalTable: "MemberOrderPeriods",
                        principalColumn: "memberOrderPeriodId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Areas_periodTypeId",
                table: "Areas",
                column: "periodTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_parentId",
                table: "Categories",
                column: "parentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_memberId",
                table: "Comments",
                column: "memberId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_productId",
                table: "Comments",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountCodes_memberId",
                table: "DiscountCodes",
                column: "memberId");

            migrationBuilder.CreateIndex(
                name: "IX_FactorItems_factorId",
                table: "FactorItems",
                column: "factorId");

            migrationBuilder.CreateIndex(
                name: "IX_FactorItems_productPriceId",
                table: "FactorItems",
                column: "productPriceId");

            migrationBuilder.CreateIndex(
                name: "IX_Factors_discountCodeId",
                table: "Factors",
                column: "discountCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Factors_discountFactorId",
                table: "Factors",
                column: "discountFactorId");

            migrationBuilder.CreateIndex(
                name: "IX_Factors_memberId",
                table: "Factors",
                column: "memberId");

            migrationBuilder.CreateIndex(
                name: "IX_Factors_memberLocationId",
                table: "Factors",
                column: "memberLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Factors_periodTypeId",
                table: "Factors",
                column: "periodTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Factors_pocketPostId",
                table: "Factors",
                column: "pocketPostId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberLocations_areaId",
                table: "MemberLocations",
                column: "areaId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberLocations_memberId",
                table: "MemberLocations",
                column: "memberId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberOrderPeriods_periodTypeId",
                table: "MemberOrderPeriods",
                column: "periodTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberOrderPeriods_targetFactorId",
                table: "MemberOrderPeriods",
                column: "targetFactorId");

            migrationBuilder.CreateIndex(
                name: "UI_Email",
                table: "Members",
                column: "email",
                unique: true,
                filter: "[email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UI_Mobile",
                table: "Members",
                column: "mobile",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_factorId",
                table: "Payments",
                column: "factorId",
                unique: true,
                filter: "[factorId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_memberId",
                table: "Payments",
                column: "memberId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_memberOrderPeriodId",
                table: "Payments",
                column: "memberOrderPeriodId",
                unique: true,
                filter: "[memberOrderPeriodId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PocketPosts_userId",
                table: "PocketPosts",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPrices_productId",
                table: "ProductPrices",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_brandId",
                table: "Products",
                column: "brandId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_categoryId",
                table: "Products",
                column: "categoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_productTypeId",
                table: "Products",
                column: "productTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Rel_CategoryBrand_categoryId",
                table: "Rel_CategoryBrand",
                column: "categoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Rel_CategoryProductType_productTypeId",
                table: "Rel_CategoryProductType",
                column: "productTypeId");

            migrationBuilder.CreateIndex(
                name: "UI_Email",
                table: "Users",
                column: "email",
                unique: true,
                filter: "[email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UI_Mobile",
                table: "Users",
                column: "mobile",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UI_Username",
                table: "Users",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSessionUpdates_userId",
                table: "UserSessionUpdates",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSmsMessages_userId",
                table: "UserSmsMessages",
                column: "userId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "FactorItems");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Rel_CategoryBrand");

            migrationBuilder.DropTable(
                name: "Rel_CategoryProductType");

            migrationBuilder.DropTable(
                name: "Rel_DiscountCodeBrand");

            migrationBuilder.DropTable(
                name: "Rel_DiscountCodeCategory");

            migrationBuilder.DropTable(
                name: "Rel_RoleUser");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "UserSessionUpdates");

            migrationBuilder.DropTable(
                name: "UserSmsMessages");

            migrationBuilder.DropTable(
                name: "ProductPrices");

            migrationBuilder.DropTable(
                name: "MemberOrderPeriods");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Factors");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "ProductTypes");

            migrationBuilder.DropTable(
                name: "DiscountCodes");

            migrationBuilder.DropTable(
                name: "DiscountFactors");

            migrationBuilder.DropTable(
                name: "MemberLocations");

            migrationBuilder.DropTable(
                name: "PocketPosts");

            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "PeriodTypes");
        }
    }
}
