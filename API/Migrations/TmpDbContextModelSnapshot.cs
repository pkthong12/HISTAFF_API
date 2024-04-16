﻿// <auto-generated />
using System;
using API.All.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace API.Migrations
{
    [DbContext(typeof(TmpDbContext))]
    partial class TmpDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("API.Entities.SYS_MUTATION_LOG", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ID"));

                    b.Property<string>("AFTER")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AFTER1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AFTER2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AFTER3")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BEFORE")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BEFORE1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BEFORE2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BEFORE3")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CREATED_BY")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CREATED_DATE")
                        .HasColumnType("datetime2");

                    b.Property<string>("SYS_ACTION_CODE")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SYS_FUNCTION_CODE")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UPDATED_BY")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UPDATED_DATE")
                        .HasColumnType("datetime2");

                    b.Property<string>("USERNAME")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("SYS_MUTATION_LOG");
                });
#pragma warning restore 612, 618
        }
    }
}
