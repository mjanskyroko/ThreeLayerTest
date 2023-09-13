﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TestWebApp.Infrastructure.Database.Mssql.Internal;

#nullable disable

namespace TestWebApp.Infrastructure.Database.Mssql.Migrations
{
    [DbContext(typeof(MssqlDbContext))]
    [Migration("20230913141517_AccountOnDelete")]
    partial class AccountOnDelete
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TestWebApp.Domain.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Balance")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Accounts", (string)null);
                });

            modelBuilder.Entity("TestWebApp.Domain.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Amount")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("FromId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ToId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("FromId");

                    b.HasIndex("ToId");

                    b.ToTable("Transactions", (string)null);
                });

            modelBuilder.Entity("TestWebApp.Domain.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(48)
                        .HasColumnType("nvarchar(48)");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("TestWebApp.Domain.Account", b =>
                {
                    b.HasOne("TestWebApp.Domain.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("TestWebApp.Domain.Transaction", b =>
                {
                    b.HasOne("TestWebApp.Domain.Account", "From")
                        .WithMany()
                        .HasForeignKey("FromId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("TestWebApp.Domain.Account", "To")
                        .WithMany()
                        .HasForeignKey("ToId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("From");

                    b.Navigation("To");
                });
#pragma warning restore 612, 618
        }
    }
}
