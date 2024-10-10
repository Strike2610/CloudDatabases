﻿// <auto-generated />
using System;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DAL.Migrations {
    [DbContext(typeof(CloudContext))]
    [Migration("Populate")]
    partial class Populate {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder) {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Domain.Customer", b => {
                b.Property<int?>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                b.Property<string>("Address")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.HasKey("Id");

                b.ToTable("Customers");
            });

            modelBuilder.Entity("Domain.Order", b => {
                b.Property<int?>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                b.Property<int?>("CustomerId")
                    .IsRequired()
                    .HasColumnType("int");

                b.Property<DateTimeOffset>("OrderDate")
                    .HasColumnType("datetimeoffset");

                b.Property<TimeSpan>("OrderProcessed")
                    .HasColumnType("time");

                b.Property<int?>("ProductId")
                    .IsRequired()
                    .HasColumnType("int");

                b.Property<DateTimeOffset>("ShipDate")
                    .HasColumnType("datetimeoffset");

                b.HasKey("Id");

                b.HasIndex("CustomerId");

                b.HasIndex("ProductId");

                b.ToTable("Orders");
            });

            modelBuilder.Entity("Domain.Product", b => {
                b.Property<int?>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                b.Property<string>("Name")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<decimal>("Price")
                    .HasPrecision(8, 2)
                    .HasColumnType("decimal(8,2)");

                b.Property<string>("Thumbnail")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.HasKey("Id");

                b.ToTable("Products");
            });

            modelBuilder.Entity("Domain.Order", b => {
                b.HasOne("Domain.Customer", "Customer")
                    .WithMany("Orders")
                    .HasForeignKey("CustomerId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("Domain.Product", "Product")
                    .WithMany()
                    .HasForeignKey("ProductId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Customer");

                b.Navigation("Product");
            });

            modelBuilder.Entity("Domain.Customer", b => {
                b.Navigation("Orders");
            });
#pragma warning restore 612, 618
        }
    }
}
