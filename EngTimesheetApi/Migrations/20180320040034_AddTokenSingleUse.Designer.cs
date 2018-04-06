﻿// <auto-generated />
using EngTimesheetApi.Data;
using EngTimesheetApi.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace EngTimesheetApi.Migrations
{
    [DbContext(typeof(TimesheetContext))]
    [Migration("20180320040034_AddTokenSingleUse")]
    partial class AddTokenSingleUse
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EngTimesheetApi.Models.Login", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("PasswordHash");

                    b.Property<byte[]>("PasswordSalt");

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Logins");
                });

            modelBuilder.Entity("EngTimesheetApi.Models.Token", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Expired");

                    b.Property<bool>("SingleUse");

                    b.Property<int>("Type");

                    b.Property<int?>("UserId");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Tokens");
                });

            modelBuilder.Entity("EngTimesheetApi.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("Deactivated");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<int>("Group");

                    b.Property<string>("LastName");

                    b.Property<bool>("Manager");

                    b.Property<DateTime?>("Registered");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("EngTimesheetApi.Models.Login", b =>
                {
                    b.HasOne("EngTimesheetApi.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("EngTimesheetApi.Models.Token", b =>
                {
                    b.HasOne("EngTimesheetApi.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}
