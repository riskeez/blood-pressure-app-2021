﻿// <auto-generated />
using System;
using BPLog.API.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BPLog.API.Domain.Migrations
{
    [DbContext(typeof(BPLogDbContext))]
    [Migration("20210821120315_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.9");

            modelBuilder.Entity("BPLog.API.Domain.BloodPressure", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateUTC")
                        .HasColumnType("TEXT");

                    b.Property<int>("Diastolic")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Systolic")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "UserId", "DateUTC" }, "IX_UserID_DateUTC");

                    b.ToTable("BloodPressure");
                });

            modelBuilder.Entity("BPLog.API.Domain.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Login")
                        .IsUnique();

                    b.ToTable("User");
                });

            modelBuilder.Entity("BPLog.API.Domain.BloodPressure", b =>
                {
                    b.HasOne("BPLog.API.Domain.User", "User")
                        .WithMany("BloodPressures")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("BPLog.API.Domain.User", b =>
                {
                    b.Navigation("BloodPressures");
                });
#pragma warning restore 612, 618
        }
    }
}
