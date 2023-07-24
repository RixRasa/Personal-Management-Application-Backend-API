﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TransactionsAPI.Database;

#nullable disable

namespace TransactionsAPI.Migrations
{
    [DbContext(typeof(TransDbContext))]
    partial class TransDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TransactionsAPI.Database.Entities.CategoryEntity", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Parent_code")
                        .HasColumnType("text");

                    b.HasKey("Code");

                    b.ToTable("categories", (string)null);
                });

            modelBuilder.Entity("TransactionsAPI.Database.Entities.SplitTransactionEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("TransactionId")
                        .HasColumnType("text");

                    b.Property<double>("amount")
                        .HasColumnType("double precision");

                    b.Property<string>("catcode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("TransactionId");

                    b.ToTable("splitsOfTransaction", (string)null);
                });

            modelBuilder.Entity("TransactionsAPI.Database.Entities.TransactionEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<double>("Amount")
                        .HasColumnType("double precision");

                    b.Property<string>("Beneficiary_name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CategoryId")
                        .HasColumnType("text");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Direction")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Kind")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Mcc")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("transactions", (string)null);
                });

            modelBuilder.Entity("TransactionsAPI.Database.Entities.SplitTransactionEntity", b =>
                {
                    b.HasOne("TransactionsAPI.Database.Entities.TransactionEntity", "Transaction")
                        .WithMany("SplitTransactions")
                        .HasForeignKey("TransactionId");

                    b.Navigation("Transaction");
                });

            modelBuilder.Entity("TransactionsAPI.Database.Entities.TransactionEntity", b =>
                {
                    b.HasOne("TransactionsAPI.Database.Entities.CategoryEntity", "Category")
                        .WithMany("Transactions")
                        .HasForeignKey("CategoryId");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("TransactionsAPI.Database.Entities.CategoryEntity", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("TransactionsAPI.Database.Entities.TransactionEntity", b =>
                {
                    b.Navigation("SplitTransactions");
                });
#pragma warning restore 612, 618
        }
    }
}
