﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mk.DemoC.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Mk.DemoC.Migrations
{
    [DbContext(typeof(DemoCHttpApiHostMigrationsDbContext))]
    [Migration("20200912055027_AddDocId")]
    partial class AddDocId
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("_Abp_DatabaseProvider", EfCoreDatabaseProvider.MySql)
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Mk.DemoC.SearchDocumentMgr.Entities.ProductSpuDoc", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("char(36)");

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasColumnName("brand")
                        .HasColumnType("varchar(24) CHARACTER SET utf8mb4")
                        .HasMaxLength(24);

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnName("concurrency_stamp")
                        .HasColumnType("varchar(40) CHARACTER SET utf8mb4")
                        .HasMaxLength(40);

                    b.Property<DateTime>("CreationTime")
                        .HasColumnName("creation_time")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("CreatorId")
                        .HasColumnName("creator_id")
                        .HasColumnType("char(36)");

                    b.Property<string>("Currency")
                        .HasColumnName("currency")
                        .HasColumnType("varchar(8) CHARACTER SET utf8mb4")
                        .HasMaxLength(8);

                    b.Property<string>("DocId")
                        .HasColumnName("doc_id")
                        .HasColumnType("varchar(64) CHARACTER SET utf8mb4")
                        .HasMaxLength(64);

                    b.Property<string>("ExtraProperties")
                        .HasColumnName("extra_properties")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<decimal>("MaxPrice")
                        .HasColumnName("max_price")
                        .HasColumnType("decimal(18,6)");

                    b.Property<decimal>("MinPrice")
                        .HasColumnName("min_price")
                        .HasColumnType("decimal(18,6)");

                    b.Property<string>("SpuCode")
                        .IsRequired()
                        .HasColumnName("spu_code")
                        .HasColumnType("varchar(24) CHARACTER SET utf8mb4")
                        .HasMaxLength(24);

                    b.Property<string>("SpuKeywords")
                        .IsRequired()
                        .HasColumnName("spu_keywords")
                        .HasColumnType("varchar(64) CHARACTER SET utf8mb4")
                        .HasMaxLength(64);

                    b.Property<string>("SpuName")
                        .IsRequired()
                        .HasColumnName("spu_name")
                        .HasColumnType("varchar(64) CHARACTER SET utf8mb4")
                        .HasMaxLength(64);

                    b.Property<string>("SumSkuCode")
                        .HasColumnName("sum_sku_code")
                        .HasColumnType("varchar(240) CHARACTER SET utf8mb4")
                        .HasMaxLength(240);

                    b.Property<string>("SumSkuKeywords")
                        .HasColumnName("sum_sku_keywords")
                        .HasColumnType("varchar(640) CHARACTER SET utf8mb4")
                        .HasMaxLength(640);

                    b.HasKey("Id");

                    b.ToTable("product_spu_doc");
                });
#pragma warning restore 612, 618
        }
    }
}
