﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Oracle.EntityFrameworkCore.Metadata;
using financeAPI;
using financeAPI.Data;

#nullable disable

namespace financeAPI.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230427231210_MainEntities")]
    partial class MainEntities
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            OracleModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("financeAPI.Models.BankAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccountNumber")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int>("Agency")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("BankName")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<int>("BankNumber")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("Name")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("Observation")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.HasKey("Id");

                    b.ToTable("BankAccount");
                });

            modelBuilder.Entity("financeAPI.Models.ChartOfAccounts", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<int>("ParentAccountId")
                        .HasColumnType("NUMBER(10)");

                    b.Property<bool>("ShowIncomeStatement")
                        .HasColumnType("NUMBER(1)");

                    b.HasKey("Id");

                    b.ToTable("ChartOfAccounts");
                });

            modelBuilder.Entity("financeAPI.Models.CostCenter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.HasKey("Id");

                    b.ToTable("CostCenter");
                });

            modelBuilder.Entity("financeAPI.Models.Supplier", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("RAW(16)");

                    b.Property<string>("Address")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("City")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("Country")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("County")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("Document")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("Name")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("Phone")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("PostalCode")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("State")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.HasKey("Guid");

                    b.ToTable("Supplier");
                });

            modelBuilder.Entity("financeAPI.Models.Transaction", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("RAW(16)");

                    b.Property<int>("AccountId")
                        .HasColumnType("NUMBER(10)");

                    b.Property<DateTime>("AccrualDate")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<int>("BankAccountId")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int>("CostCenterId")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("Description")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<decimal>("PenaltyAndInterestOrDiscount")
                        .HasPrecision(18, 4)
                        .HasColumnType("DECIMAL(18,4)");

                    b.Property<string>("Status")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<Guid>("SupplierGuid")
                        .HasColumnType("RAW(16)");

                    b.Property<string>("Type")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<decimal>("Value")
                        .HasPrecision(18, 4)
                        .HasColumnType("DECIMAL(18,4)");

                    b.HasKey("Guid");

                    b.ToTable("Transaction");
                });
#pragma warning restore 612, 618
        }
    }
}
