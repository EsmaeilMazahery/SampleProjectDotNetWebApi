﻿// <auto-generated />
using System;
using ESkimo.DataLayer.Context;
using GeoAPI.Geometries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ESkimo.DataLayer.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20191019103020_fix")]
    partial class fix
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ESkimo.DomainLayer.Models.Area", b =>
                {
                    b.Property<int>("areaId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("address")
                        .HasMaxLength(1000);

                    b.Property<decimal>("amountSend")
                        .HasColumnType("decimal(18,4)");

                    b.Property<IPoint>("location")
                        .HasColumnType("geography");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<int>("periodTypeId");

                    b.Property<int>("sendDay");

                    b.Property<float>("zoom");

                    b.HasKey("areaId");

                    b.HasIndex("periodTypeId");

                    b.ToTable("Areas");
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.Brand", b =>
                {
                    b.Property<int>("brandId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("image")
                        .HasMaxLength(1000);

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.HasKey("brandId");

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.Category", b =>
                {
                    b.Property<int>("categoryId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<int?>("parentId");

                    b.HasKey("categoryId");

                    b.HasIndex("parentId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.Comment", b =>
                {
                    b.Property<int>("commentId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool?>("confirm");

                    b.Property<DateTime>("dateTime");

                    b.Property<int>("memberId");

                    b.Property<int>("productId");

                    b.Property<string>("text")
                        .IsRequired()
                        .HasMaxLength(4000);

                    b.HasKey("commentId");

                    b.HasIndex("memberId");

                    b.HasIndex("productId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.DiscountCode", b =>
                {
                    b.Property<int>("discountCodeId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("activeAlone");

                    b.Property<string>("code")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<int?>("countUse");

                    b.Property<decimal>("discount")
                        .HasColumnType("decimal(18,4)");

                    b.Property<bool>("enable");

                    b.Property<DateTime?>("endDate");

                    b.Property<decimal>("maxDiscount")
                        .HasColumnType("decimal(18,4)");

                    b.Property<int>("maxRegisterDate");

                    b.Property<int?>("memberId");

                    b.Property<decimal>("minPrice")
                        .HasColumnType("decimal(18,4)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<decimal>("percent")
                        .HasColumnType("decimal(18,4)");

                    b.Property<DateTime?>("startDate");

                    b.HasKey("discountCodeId");

                    b.HasIndex("memberId");

                    b.ToTable("DiscountCodes");
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.DiscountFactor", b =>
                {
                    b.Property<int>("discountFactorId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("_maxRegisterDate");

                    b.Property<decimal?>("discount")
                        .HasColumnType("decimal(18,4)");

                    b.Property<bool>("enable");

                    b.Property<DateTime?>("endDate");

                    b.Property<decimal?>("maxDiscount")
                        .HasColumnType("decimal(18,4)");

                    b.Property<decimal?>("minAmount")
                        .HasColumnType("decimal(18,4)");

                    b.Property<decimal?>("precent")
                        .HasColumnType("decimal(18,4)");

                    b.Property<DateTime?>("startDate");

                    b.HasKey("discountFactorId");

                    b.ToTable("DiscountFactors");
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.Factor", b =>
                {
                    b.Property<int>("factorId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("amount")
                        .HasColumnType("decimal(18,4)");

                    b.Property<decimal>("amountSend")
                        .HasColumnType("decimal(18,4)");

                    b.Property<DateTime>("dateTime");

                    b.Property<int?>("discountCodeId");

                    b.Property<string>("discountCodeName")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<int?>("discountFactorId");

                    b.Property<decimal>("discountOfCode")
                        .HasColumnType("decimal(18,4)");

                    b.Property<decimal>("discountOfFactor")
                        .HasColumnType("decimal(18,4)");

                    b.Property<decimal>("discountOfPeriod")
                        .HasColumnType("decimal(18,4)");

                    b.Property<int>("memberId");

                    b.Property<int>("memberLocationId");

                    b.Property<int?>("paymentId");

                    b.Property<int?>("periodTypeId");

                    b.Property<int?>("pocketPostId");

                    b.Property<DateTime>("sendDateTime");

                    b.Property<bool>("sent");

                    b.Property<decimal>("tax")
                        .HasColumnType("decimal(18,4)");

                    b.HasKey("factorId");

                    b.HasIndex("discountCodeId");

                    b.HasIndex("discountFactorId");

                    b.HasIndex("memberId");

                    b.HasIndex("memberLocationId");

                    b.HasIndex("periodTypeId");

                    b.HasIndex("pocketPostId");

                    b.ToTable("Factors");
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.FactorItem", b =>
                {
                    b.Property<int>("factorItemId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("amount")
                        .HasColumnType("decimal(18,4)");

                    b.Property<decimal>("amountBasePerItem")
                        .HasColumnType("decimal(18,4)");

                    b.Property<decimal>("amountPerItem")
                        .HasColumnType("decimal(18,4)");

                    b.Property<int>("count");

                    b.Property<int>("factorId");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<int?>("productPriceId");

                    b.HasKey("factorItemId");

                    b.HasIndex("factorId");

                    b.HasIndex("productPriceId");

                    b.ToTable("FactorItems");
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.Member", b =>
                {
                    b.Property<int>("memberId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("amount")
                        .HasColumnType("decimal(18,4)");

                    b.Property<string>("email")
                        .HasMaxLength(1000);

                    b.Property<bool>("enable");

                    b.Property<string>("family")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<string>("mobile")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<string>("password")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<DateTime>("registerDate");

                    b.Property<decimal>("sumFactors")
                        .HasColumnType("decimal(18,4)");

                    b.Property<decimal>("sumPayment")
                        .HasColumnType("decimal(18,4)");

                    b.HasKey("memberId");

                    b.HasIndex("email")
                        .IsUnique()
                        .HasName("UI_Email")
                        .HasFilter("[email] IS NOT NULL");

                    b.HasIndex("mobile")
                        .IsUnique()
                        .HasName("UI_Mobile");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.MemberLocation", b =>
                {
                    b.Property<int>("memberLocationId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<IPoint>("Location");

                    b.Property<string>("address")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<int>("areaId");

                    b.Property<int>("memberId");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.HasKey("memberLocationId");

                    b.HasIndex("areaId");

                    b.HasIndex("memberId");

                    b.ToTable("MemberLocations");
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.MemberOrderPeriod", b =>
                {
                    b.Property<int>("memberOrderPeriodId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("payType");

                    b.Property<int?>("paymentId");

                    b.Property<int>("periodTypeId");

                    b.Property<int>("targetFactorId");

                    b.HasKey("memberOrderPeriodId");

                    b.HasIndex("periodTypeId");

                    b.HasIndex("targetFactorId");

                    b.ToTable("MemberOrderPeriods");
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.Payment", b =>
                {
                    b.Property<int>("paymentId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("amount")
                        .HasColumnType("decimal(18,4)");

                    b.Property<DateTime>("dateTime");

                    b.Property<string>("description")
                        .HasMaxLength(4000);

                    b.Property<int?>("factorId");

                    b.Property<int>("memberId");

                    b.Property<int?>("memberOrderPeriodId");

                    b.Property<byte>("paymentType");

                    b.Property<bool>("success");

                    b.Property<string>("trackingCode")
                        .HasMaxLength(1000);

                    b.HasKey("paymentId");

                    b.HasIndex("factorId")
                        .IsUnique()
                        .HasFilter("[factorId] IS NOT NULL");

                    b.HasIndex("memberId");

                    b.HasIndex("memberOrderPeriodId")
                        .IsUnique()
                        .HasFilter("[memberOrderPeriodId] IS NOT NULL");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.PeriodType", b =>
                {
                    b.Property<int>("periodTypeId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("day");

                    b.Property<decimal>("maxDiscount")
                        .HasColumnType("decimal(18,4)");

                    b.Property<int>("month");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<decimal>("percentDiscount")
                        .HasColumnType("decimal(18,4)");

                    b.HasKey("periodTypeId");

                    b.ToTable("PeriodTypes");
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.PocketPost", b =>
                {
                    b.Property<int>("pocketPostId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("amount")
                        .HasColumnType("decimal(18,4)");

                    b.Property<DateTime>("dateTime");

                    b.Property<int>("userId");

                    b.HasKey("pocketPostId");

                    b.HasIndex("userId");

                    b.ToTable("PocketPosts");
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.Product", b =>
                {
                    b.Property<int>("productId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("attributes");

                    b.Property<int>("brandId");

                    b.Property<int>("categoryId");

                    b.Property<string>("description");

                    b.Property<bool>("enable");

                    b.Property<string>("imageAddress")
                        .HasMaxLength(1000);

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<int>("productTypeId");

                    b.HasKey("productId");

                    b.HasIndex("brandId");

                    b.HasIndex("categoryId");

                    b.HasIndex("productTypeId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.ProductPrice", b =>
                {
                    b.Property<int>("productPriceId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("amount")
                        .HasColumnType("decimal(18,4)");

                    b.Property<decimal>("amountBase")
                        .HasColumnType("decimal(18,4)");

                    b.Property<int>("count");

                    b.Property<bool>("enable");

                    b.Property<int>("minCountSell");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<int>("productId");

                    b.HasKey("productPriceId");

                    b.HasIndex("productId");

                    b.ToTable("ProductPrices");
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.ProductType", b =>
                {
                    b.Property<int>("productTypeId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.HasKey("productTypeId");

                    b.ToTable("ProductTypes");
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.Rel_CategoryBrand", b =>
                {
                    b.Property<int>("brandId");

                    b.Property<int>("categoryId");

                    b.HasKey("brandId", "categoryId");

                    b.HasIndex("categoryId");

                    b.ToTable("Rel_CategoryBrand");
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.Rel_CategoryProductType", b =>
                {
                    b.Property<int>("categoryId");

                    b.Property<int>("productTypeId");

                    b.HasKey("categoryId", "productTypeId");

                    b.HasIndex("productTypeId");

                    b.ToTable("Rel_CategoryProductType");
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.Rel_DiscountCodeBrand", b =>
                {
                    b.Property<int>("discountCodeId");

                    b.Property<int>("brandId");

                    b.HasKey("discountCodeId", "brandId");

                    b.HasAlternateKey("brandId", "discountCodeId");

                    b.ToTable("Rel_DiscountCodeBrand");
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.Rel_DiscountCodeCategory", b =>
                {
                    b.Property<int>("discountCodeId");

                    b.Property<int>("categoryId");

                    b.HasKey("discountCodeId", "categoryId");

                    b.HasAlternateKey("categoryId", "discountCodeId");

                    b.ToTable("Rel_DiscountCodeCategory");
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.Rel_RoleUser", b =>
                {
                    b.Property<int>("userId");

                    b.Property<byte>("roleId");

                    b.HasKey("userId", "roleId");

                    b.HasAlternateKey("roleId", "userId");

                    b.ToTable("Rel_RoleUser");
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.Role", b =>
                {
                    b.Property<byte>("roleKey");

                    b.Property<string>("description")
                        .HasMaxLength(4000);

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.HasKey("roleKey");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.Setting", b =>
                {
                    b.Property<byte>("SettingType");

                    b.Property<string>("Value");

                    b.HasKey("SettingType");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.User", b =>
                {
                    b.Property<int>("userId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("email")
                        .HasMaxLength(1000);

                    b.Property<bool>("enable");

                    b.Property<string>("family")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<string>("image")
                        .HasMaxLength(1000);

                    b.Property<string>("mobile")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<string>("newPassword")
                        .HasMaxLength(1000);

                    b.Property<string>("password")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<DateTime>("registerDate");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.HasKey("userId");

                    b.HasIndex("email")
                        .IsUnique()
                        .HasName("UI_Email")
                        .HasFilter("[email] IS NOT NULL");

                    b.HasIndex("mobile")
                        .IsUnique()
                        .HasName("UI_Mobile");

                    b.HasIndex("username")
                        .IsUnique()
                        .HasName("UI_Username");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.UserSessionUpdate", b =>
                {
                    b.Property<int>("userSessionUpdateId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("userId");

                    b.HasKey("userSessionUpdateId");

                    b.HasIndex("userId");

                    b.ToTable("UserSessionUpdates");
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.UserSmsMessage", b =>
                {
                    b.Property<int>("userSmsMessageId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("receiverNumber")
                        .HasMaxLength(10);

                    b.Property<string>("refId")
                        .HasMaxLength(100);

                    b.Property<DateTime>("registerDateTime");

                    b.Property<byte>("sendStatus");

                    b.Property<byte>("sendType");

                    b.Property<string>("senderNumber")
                        .HasMaxLength(100);

                    b.Property<string>("text")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<byte>("tryCount");

                    b.Property<int>("userId");

                    b.HasKey("userSmsMessageId");

                    b.HasIndex("userId");

                    b.ToTable("UserSmsMessages");
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.Area", b =>
                {
                    b.HasOne("ESkimo.DomainLayer.Models.PeriodType", "periodType")
                        .WithMany("areas")
                        .HasForeignKey("periodTypeId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.Category", b =>
                {
                    b.HasOne("ESkimo.DomainLayer.Models.Category", "parent")
                        .WithMany("children")
                        .HasForeignKey("parentId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.Comment", b =>
                {
                    b.HasOne("ESkimo.DomainLayer.Models.Member", "member")
                        .WithMany("comments")
                        .HasForeignKey("memberId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ESkimo.DomainLayer.Models.Product", "product")
                        .WithMany("comments")
                        .HasForeignKey("productId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.DiscountCode", b =>
                {
                    b.HasOne("ESkimo.DomainLayer.Models.Member", "member")
                        .WithMany("discountCodes")
                        .HasForeignKey("memberId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.Factor", b =>
                {
                    b.HasOne("ESkimo.DomainLayer.Models.DiscountCode", "discountCode")
                        .WithMany("factors")
                        .HasForeignKey("discountCodeId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("ESkimo.DomainLayer.Models.DiscountFactor", "discountFactor")
                        .WithMany("factors")
                        .HasForeignKey("discountFactorId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("ESkimo.DomainLayer.Models.Member", "member")
                        .WithMany("factors")
                        .HasForeignKey("memberId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ESkimo.DomainLayer.Models.MemberLocation", "memberLocation")
                        .WithMany("factors")
                        .HasForeignKey("memberLocationId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ESkimo.DomainLayer.Models.PeriodType", "periodType")
                        .WithMany("factors")
                        .HasForeignKey("periodTypeId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("ESkimo.DomainLayer.Models.PocketPost", "pocketPost")
                        .WithMany("factors")
                        .HasForeignKey("pocketPostId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.FactorItem", b =>
                {
                    b.HasOne("ESkimo.DomainLayer.Models.Factor", "factor")
                        .WithMany("factorItems")
                        .HasForeignKey("factorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ESkimo.DomainLayer.Models.ProductPrice", "productPrice")
                        .WithMany("factorItems")
                        .HasForeignKey("productPriceId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.MemberLocation", b =>
                {
                    b.HasOne("ESkimo.DomainLayer.Models.Area", "area")
                        .WithMany("memberLocations")
                        .HasForeignKey("areaId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ESkimo.DomainLayer.Models.Member", "member")
                        .WithMany("memberLocations")
                        .HasForeignKey("memberId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.MemberOrderPeriod", b =>
                {
                    b.HasOne("ESkimo.DomainLayer.Models.PeriodType", "periodType")
                        .WithMany("memberOrderPeriods")
                        .HasForeignKey("periodTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ESkimo.DomainLayer.Models.Factor", "targetFactor")
                        .WithMany("memberOrderPeriods")
                        .HasForeignKey("targetFactorId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.Payment", b =>
                {
                    b.HasOne("ESkimo.DomainLayer.Models.Factor", "factor")
                        .WithOne("payment")
                        .HasForeignKey("ESkimo.DomainLayer.Models.Payment", "factorId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("ESkimo.DomainLayer.Models.Member", "member")
                        .WithMany("payments")
                        .HasForeignKey("memberId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ESkimo.DomainLayer.Models.MemberOrderPeriod", "memberOrderPeriod")
                        .WithOne("payment")
                        .HasForeignKey("ESkimo.DomainLayer.Models.Payment", "memberOrderPeriodId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.PocketPost", b =>
                {
                    b.HasOne("ESkimo.DomainLayer.Models.User", "user")
                        .WithMany("pocketPosts")
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.Product", b =>
                {
                    b.HasOne("ESkimo.DomainLayer.Models.Brand", "brand")
                        .WithMany("products")
                        .HasForeignKey("brandId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ESkimo.DomainLayer.Models.Category", "category")
                        .WithMany("products")
                        .HasForeignKey("categoryId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ESkimo.DomainLayer.Models.ProductType", "productType")
                        .WithMany("products")
                        .HasForeignKey("productTypeId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.ProductPrice", b =>
                {
                    b.HasOne("ESkimo.DomainLayer.Models.Product", "product")
                        .WithMany("productPrices")
                        .HasForeignKey("productId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.Rel_CategoryBrand", b =>
                {
                    b.HasOne("ESkimo.DomainLayer.Models.Brand", "brand")
                        .WithMany("categories")
                        .HasForeignKey("brandId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ESkimo.DomainLayer.Models.Category", "category")
                        .WithMany("brands")
                        .HasForeignKey("categoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.Rel_CategoryProductType", b =>
                {
                    b.HasOne("ESkimo.DomainLayer.Models.Category", "category")
                        .WithMany("productTypes")
                        .HasForeignKey("categoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ESkimo.DomainLayer.Models.ProductType", "productType")
                        .WithMany("categories")
                        .HasForeignKey("productTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.Rel_DiscountCodeBrand", b =>
                {
                    b.HasOne("ESkimo.DomainLayer.Models.Brand", "brand")
                        .WithMany("discountCodes")
                        .HasForeignKey("brandId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ESkimo.DomainLayer.Models.DiscountCode", "discountCode")
                        .WithMany("brands")
                        .HasForeignKey("discountCodeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.Rel_DiscountCodeCategory", b =>
                {
                    b.HasOne("ESkimo.DomainLayer.Models.Category", "category")
                        .WithMany("discountCodes")
                        .HasForeignKey("categoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ESkimo.DomainLayer.Models.DiscountCode", "discountCode")
                        .WithMany("categories")
                        .HasForeignKey("discountCodeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.Rel_RoleUser", b =>
                {
                    b.HasOne("ESkimo.DomainLayer.Models.Role", "role")
                        .WithMany("users")
                        .HasForeignKey("roleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ESkimo.DomainLayer.Models.User", "user")
                        .WithMany("roles")
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.UserSessionUpdate", b =>
                {
                    b.HasOne("ESkimo.DomainLayer.Models.User", "user")
                        .WithMany()
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ESkimo.DomainLayer.Models.UserSmsMessage", b =>
                {
                    b.HasOne("ESkimo.DomainLayer.Models.User", "user")
                        .WithMany()
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
