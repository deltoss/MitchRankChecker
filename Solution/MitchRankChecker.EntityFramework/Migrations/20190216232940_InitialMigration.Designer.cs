﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MitchRankChecker.EntityFramework;

namespace MitchRankChecker.EntityFramework.Migrations
{
    [DbContext(typeof(RankCheckerDbContext))]
    [Migration("20190216232940_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034");

            modelBuilder.Entity("MitchRankChecker.Model.Enumerations.RankCheckRequestStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("RankCheckRequestStatus");
                });

            modelBuilder.Entity("MitchRankChecker.Model.RankCheckRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreatedAt")
                        .IsConcurrencyToken();

                    b.Property<DateTime?>("LastUpdatedAt")
                        .IsConcurrencyToken();

                    b.Property<int>("MaximumRecords");

                    b.Property<string>("SearchUrl")
                        .IsRequired();

                    b.Property<int>("StatusId");

                    b.Property<string>("TermToSearch")
                        .IsRequired();

                    b.Property<string>("WebsiteUrl")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("StatusId");

                    b.ToTable("RankCheckRequests");
                });

            modelBuilder.Entity("MitchRankChecker.Model.SearchEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreatedAt")
                        .IsConcurrencyToken();

                    b.Property<DateTime?>("LastUpdatedAt")
                        .IsConcurrencyToken();

                    b.Property<int>("Rank");

                    b.Property<int>("RankCheckRequestId");

                    b.Property<string>("Url")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RankCheckRequestId");

                    b.ToTable("SearchEntries");
                });

            modelBuilder.Entity("MitchRankChecker.Model.RankCheckRequest", b =>
                {
                    b.HasOne("MitchRankChecker.Model.Enumerations.RankCheckRequestStatus", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MitchRankChecker.Model.SearchEntry", b =>
                {
                    b.HasOne("MitchRankChecker.Model.RankCheckRequest", "RankCheckRequest")
                        .WithMany("SearchEntries")
                        .HasForeignKey("RankCheckRequestId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
