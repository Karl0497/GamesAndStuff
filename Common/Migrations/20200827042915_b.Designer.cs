﻿// <auto-generated />
using System;
using Common.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Common.Migrations
{
    [DbContext(typeof(GamesAndStuffContext))]
    [Migration("20200827042915_b")]
    partial class b
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Common.Context.BasicModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("BasicModel");

                    b.HasDiscriminator<string>("Discriminator").HasValue("BasicModel");
                });

            modelBuilder.Entity("Common.Context.AccountBalance", b =>
                {
                    b.HasBaseType("Common.Context.BasicModel");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.HasDiscriminator().HasValue("AccountBalance");
                });

            modelBuilder.Entity("Common.Context.DiscordUser", b =>
                {
                    b.HasBaseType("Common.Context.BasicModel");

                    b.Property<int?>("AccountBalance")
                        .HasColumnType("int");

                    b.Property<decimal>("DiscordId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<string>("DisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.HasIndex("AccountBalance")
                        .IsUnique()
                        .HasFilter("[AccountBalance] IS NOT NULL");

                    b.HasDiscriminator().HasValue("DiscordUser");
                });

            modelBuilder.Entity("Common.Context.ScheduledJob", b =>
                {
                    b.HasBaseType("Common.Context.BasicModel");

                    b.Property<DateTime>("ExecutionTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("OwnerId")
                        .HasColumnType("int");

                    b.HasIndex("OwnerId");

                    b.HasDiscriminator().HasValue("ScheduledJob");
                });

            modelBuilder.Entity("Common.Context.DiscordUser", b =>
                {
                    b.HasOne("Common.Context.AccountBalance", "Balance")
                        .WithOne("Owner")
                        .HasForeignKey("Common.Context.DiscordUser", "AccountBalance");
                });

            modelBuilder.Entity("Common.Context.ScheduledJob", b =>
                {
                    b.HasOne("Common.Context.BasicModel", "Owner")
                        .WithMany("Jobs")
                        .HasForeignKey("OwnerId");
                });
#pragma warning restore 612, 618
        }
    }
}
