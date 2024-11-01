﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MiddleAges.Data;

namespace MiddleAges.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241101135143_LandBuildingKeyChangesV2")]
    partial class LandBuildingKeyChangesV2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("MiddleAges.Entities.Army", b =>
                {
                    b.Property<Guid>("ArmyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LandId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PlayerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Side")
                        .HasColumnType("int");

                    b.Property<int>("SoldiersCount")
                        .HasColumnType("int");

                    b.Property<int>("SoldiersKilled")
                        .HasColumnType("int");

                    b.Property<int>("SoldiersLost")
                        .HasColumnType("int");

                    b.Property<Guid>("WarId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ArmyId");

                    b.HasIndex("LandId");

                    b.HasIndex("PlayerId");

                    b.HasIndex("WarId");

                    b.ToTable("Armies");
                });

            modelBuilder.Entity("MiddleAges.Entities.BorderLand", b =>
                {
                    b.Property<string>("LandId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BorderLandId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LandId", "BorderLandId");

                    b.ToTable("BorderLands");
                });

            modelBuilder.Entity("MiddleAges.Entities.Building", b =>
                {
                    b.Property<Guid>("BuildingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LandId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Lvl")
                        .HasColumnType("int");

                    b.Property<string>("PlayerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("BuildingId");

                    b.HasIndex("LandId");

                    b.HasIndex("PlayerId");

                    b.ToTable("Buildings");
                });

            modelBuilder.Entity("MiddleAges.Entities.ChatMessage", b =>
                {
                    b.Property<Guid>("MessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ChatRoomId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ChatRoomType")
                        .HasColumnType("int");

                    b.Property<string>("MessageValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PlayerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("PublishingDateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("MessageId");

                    b.HasIndex("PlayerId");

                    b.ToTable("ChatMessages");
                });

            modelBuilder.Entity("MiddleAges.Entities.Country", b =>
                {
                    b.Property<Guid>("CountryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CapitalId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Color")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Money")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RulerId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("CountryId");

                    b.HasIndex("RulerId");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("MiddleAges.Entities.Land", b =>
                {
                    b.Property<string>("LandId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid?>("CountryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("CountryTax")
                        .HasColumnType("int");

                    b.Property<string>("GovernorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("LandTax")
                        .HasColumnType("int");

                    b.Property<double>("Money")
                        .HasColumnType("float");

                    b.Property<double>("ProductionLimit")
                        .HasColumnType("float");

                    b.HasKey("LandId");

                    b.HasIndex("CountryId");

                    b.HasIndex("GovernorId");

                    b.ToTable("Lands");
                });

            modelBuilder.Entity("MiddleAges.Entities.LandBuilding", b =>
                {
                    b.Property<Guid>("BuildingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("BuildingType")
                        .HasColumnType("int");

                    b.Property<string>("LandId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Lvl")
                        .HasColumnType("int");

                    b.HasKey("BuildingId");

                    b.ToTable("LandBuildings");
                });

            modelBuilder.Entity("MiddleAges.Entities.Law", b =>
                {
                    b.Property<Guid>("LawId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CountryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PlayerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("PublishingDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<string>("Value1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value2")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LawId");

                    b.HasIndex("CountryId");

                    b.HasIndex("PlayerId");

                    b.ToTable("Laws");
                });

            modelBuilder.Entity("MiddleAges.Entities.Player", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CurrentLand")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<DateTime>("EndDateTimeProduction")
                        .HasColumnType("datetime2");

                    b.Property<long>("Exp")
                        .HasColumnType("bigint");

                    b.Property<string>("ImageURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("Lvl")
                        .HasColumnType("int");

                    b.Property<double>("Money")
                        .HasColumnType("float");

                    b.Property<double>("MoneyProduced")
                        .HasColumnType("float");

                    b.Property<double>("MoneySpent")
                        .HasColumnType("float");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<int>("RecruitAmount")
                        .HasColumnType("int");

                    b.Property<string>("ResidenceLand")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("CurrentLand");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("MiddleAges.Entities.PlayerAttribute", b =>
                {
                    b.Property<Guid>("AttributeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Leadership")
                        .HasColumnType("int");

                    b.Property<int>("Management")
                        .HasColumnType("int");

                    b.Property<string>("PlayerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Warfare")
                        .HasColumnType("int");

                    b.HasKey("AttributeId");

                    b.HasIndex("PlayerId");

                    b.ToTable("PlayerAttributes");
                });

            modelBuilder.Entity("MiddleAges.Entities.PlayerInformation", b =>
                {
                    b.Property<Guid>("PlayerInformationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discord")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Facebook")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PlayerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Telegram")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Vk")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PlayerInformationId");

                    b.HasIndex("PlayerId");

                    b.ToTable("PlayerInformations");
                });

            modelBuilder.Entity("MiddleAges.Entities.PlayerLocalEvent", b =>
                {
                    b.Property<Guid>("LocalEventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("AssignedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("EventId")
                        .HasColumnType("int");

                    b.Property<string>("PlayerId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LocalEventId");

                    b.HasIndex("PlayerId");

                    b.ToTable("PlayerLocalEvents");
                });

            modelBuilder.Entity("MiddleAges.Entities.PlayerStatistics", b =>
                {
                    b.Property<Guid>("PlayerStatisticsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PlayerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("SoldiersKilled")
                        .HasColumnType("int");

                    b.Property<int>("SoldiersLost")
                        .HasColumnType("int");

                    b.HasKey("PlayerStatisticsId");

                    b.HasIndex("PlayerId");

                    b.ToTable("PlayerStatistics");
                });

            modelBuilder.Entity("MiddleAges.Entities.Rating", b =>
                {
                    b.Property<Guid>("RatingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ExpPlace")
                        .HasColumnType("int");

                    b.Property<int>("MoneyPlace")
                        .HasColumnType("int");

                    b.Property<string>("PlayerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("TotalPlace")
                        .HasColumnType("int");

                    b.Property<int>("WarPowerPlace")
                        .HasColumnType("int");

                    b.HasKey("RatingId");

                    b.HasIndex("PlayerId");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("MiddleAges.Entities.Unit", b =>
                {
                    b.Property<Guid>("UnitId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<string>("LandId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Lvl")
                        .HasColumnType("int");

                    b.Property<string>("PlayerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("UnitId");

                    b.HasIndex("LandId");

                    b.HasIndex("PlayerId");

                    b.ToTable("Units");
                });

            modelBuilder.Entity("MiddleAges.Entities.War", b =>
                {
                    b.Property<Guid>("WarId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsEnded")
                        .HasColumnType("bit");

                    b.Property<bool>("IsRevolt")
                        .HasColumnType("bit");

                    b.Property<string>("LandIdFrom")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LandIdTo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RebelId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("StartDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("WarResult")
                        .HasColumnType("int");

                    b.HasKey("WarId");

                    b.HasIndex("RebelId");

                    b.ToTable("Wars");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("MiddleAges.Entities.Player", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("MiddleAges.Entities.Player", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MiddleAges.Entities.Player", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("MiddleAges.Entities.Player", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MiddleAges.Entities.Army", b =>
                {
                    b.HasOne("MiddleAges.Entities.Land", "Land")
                        .WithMany()
                        .HasForeignKey("LandId");

                    b.HasOne("MiddleAges.Entities.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId");

                    b.HasOne("MiddleAges.Entities.War", "War")
                        .WithMany()
                        .HasForeignKey("WarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Land");

                    b.Navigation("Player");

                    b.Navigation("War");
                });

            modelBuilder.Entity("MiddleAges.Entities.BorderLand", b =>
                {
                    b.HasOne("MiddleAges.Entities.Land", "Land")
                        .WithMany()
                        .HasForeignKey("LandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Land");
                });

            modelBuilder.Entity("MiddleAges.Entities.Building", b =>
                {
                    b.HasOne("MiddleAges.Entities.Land", "Land")
                        .WithMany()
                        .HasForeignKey("LandId");

                    b.HasOne("MiddleAges.Entities.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId");

                    b.Navigation("Land");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("MiddleAges.Entities.ChatMessage", b =>
                {
                    b.HasOne("MiddleAges.Entities.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("MiddleAges.Entities.Country", b =>
                {
                    b.HasOne("MiddleAges.Entities.Player", "Ruler")
                        .WithMany()
                        .HasForeignKey("RulerId");

                    b.Navigation("Ruler");
                });

            modelBuilder.Entity("MiddleAges.Entities.Land", b =>
                {
                    b.HasOne("MiddleAges.Entities.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId");

                    b.HasOne("MiddleAges.Entities.Player", "Governor")
                        .WithMany()
                        .HasForeignKey("GovernorId");

                    b.Navigation("Country");

                    b.Navigation("Governor");
                });

            modelBuilder.Entity("MiddleAges.Entities.Law", b =>
                {
                    b.HasOne("MiddleAges.Entities.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MiddleAges.Entities.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId");

                    b.Navigation("Country");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("MiddleAges.Entities.Player", b =>
                {
                    b.HasOne("MiddleAges.Entities.Land", "Land")
                        .WithMany()
                        .HasForeignKey("CurrentLand");

                    b.Navigation("Land");
                });

            modelBuilder.Entity("MiddleAges.Entities.PlayerAttribute", b =>
                {
                    b.HasOne("MiddleAges.Entities.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("MiddleAges.Entities.PlayerInformation", b =>
                {
                    b.HasOne("MiddleAges.Entities.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("MiddleAges.Entities.PlayerLocalEvent", b =>
                {
                    b.HasOne("MiddleAges.Entities.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("MiddleAges.Entities.PlayerStatistics", b =>
                {
                    b.HasOne("MiddleAges.Entities.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("MiddleAges.Entities.Rating", b =>
                {
                    b.HasOne("MiddleAges.Entities.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("MiddleAges.Entities.Unit", b =>
                {
                    b.HasOne("MiddleAges.Entities.Land", "Land")
                        .WithMany()
                        .HasForeignKey("LandId");

                    b.HasOne("MiddleAges.Entities.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId");

                    b.Navigation("Land");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("MiddleAges.Entities.War", b =>
                {
                    b.HasOne("MiddleAges.Entities.Player", "Player")
                        .WithMany()
                        .HasForeignKey("RebelId");

                    b.Navigation("Player");
                });
#pragma warning restore 612, 618
        }
    }
}
