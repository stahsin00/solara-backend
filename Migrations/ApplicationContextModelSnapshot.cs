﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Solara.Data;

#nullable disable

namespace solara_backend.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("Solara.Models.Character", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BaseAttackStat")
                        .HasColumnType("int");

                    b.Property<float>("BaseCritDamageStat")
                        .HasColumnType("float");

                    b.Property<float>("BaseCritRateStat")
                        .HasColumnType("float");

                    b.Property<float>("BaseEnergyRechargeStat")
                        .HasColumnType("float");

                    b.Property<int>("BaseSpeedStat")
                        .HasColumnType("int");

                    b.Property<string>("BasicAttack")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Element")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("FaceArt")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("FullArt")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<string>("SpecialAttack")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Characters");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BaseAttackStat = 100,
                            BaseCritDamageStat = 1.5f,
                            BaseCritRateStat = 0.1f,
                            BaseEnergyRechargeStat = 1f,
                            BaseSpeedStat = 20,
                            BasicAttack = "Sword Slash",
                            Description = "A laid-back wanderer drifting through Solara.",
                            Element = "Blue",
                            FaceArt = "art/margana-face.png",
                            FullArt = "art/margana.png",
                            Name = "Margana",
                            Price = 5000,
                            SpecialAttack = "Blazing Strike"
                        },
                        new
                        {
                            Id = 2,
                            BaseAttackStat = 80,
                            BaseCritDamageStat = 1.4f,
                            BaseCritRateStat = 0.12f,
                            BaseEnergyRechargeStat = 1.2f,
                            BaseSpeedStat = 15,
                            BasicAttack = "Water Blast",
                            Description = "A passionate artist known to get fired up.",
                            Element = "Magenta",
                            FaceArt = "art/a-face.png",
                            FullArt = "art/a.png",
                            Name = "Artemisia",
                            Price = 5000,
                            SpecialAttack = "Tsunami"
                        },
                        new
                        {
                            Id = 3,
                            BaseAttackStat = 80,
                            BaseCritDamageStat = 1.4f,
                            BaseCritRateStat = 0.12f,
                            BaseEnergyRechargeStat = 1.2f,
                            BaseSpeedStat = 15,
                            BasicAttack = "Water Blast",
                            Description = "A hardworking maid just trying her best.",
                            Element = "Magenta",
                            FaceArt = "art/b-face.png",
                            FullArt = "art/b.png",
                            Name = "Powder",
                            Price = 5000,
                            SpecialAttack = "Tsunami"
                        },
                        new
                        {
                            Id = 4,
                            BaseAttackStat = 80,
                            BaseCritDamageStat = 1.4f,
                            BaseCritRateStat = 0.12f,
                            BaseEnergyRechargeStat = 1.2f,
                            BaseSpeedStat = 15,
                            BasicAttack = "Water Blast",
                            Description = "A medical student currently enrolled at the University of Solara.",
                            Element = "Magenta",
                            FaceArt = "art/e-face.png",
                            FullArt = "art/e.png",
                            Name = "Ann",
                            Price = 5000,
                            SpecialAttack = "Tsunami"
                        });
                });

            modelBuilder.Entity("Solara.Models.CharacterInstance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<float>("AttackStat")
                        .HasColumnType("float");

                    b.Property<int>("CharacterId")
                        .HasColumnType("int");

                    b.Property<float>("CritDamageStat")
                        .HasColumnType("float");

                    b.Property<float>("CritRateStat")
                        .HasColumnType("float");

                    b.Property<float>("EnergyRechargeStat")
                        .HasColumnType("float");

                    b.Property<int>("Experience")
                        .HasColumnType("int");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<DateTime>("ObtainedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<float>("SpeedStat")
                        .HasColumnType("float");

                    b.Property<bool>("Team")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CharacterId");

                    b.HasIndex("UserId");

                    b.ToTable("CharacterInstances");
                });

            modelBuilder.Entity("Solara.Models.EquipmentInstance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ObtainedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("EquipmentInstances");
                });

            modelBuilder.Entity("Solara.Models.Quest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Complete")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Quests");
                });

            modelBuilder.Entity("Solara.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Balance")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Exp")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastLogin")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("OAuthProvider")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("OAuthProviderUserId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ProfilePicture")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Solara.Models.CharacterInstance", b =>
                {
                    b.HasOne("Solara.Models.Character", "Character")
                        .WithMany()
                        .HasForeignKey("CharacterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Solara.Models.User", null)
                        .WithMany("Characters")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Character");
                });

            modelBuilder.Entity("Solara.Models.Quest", b =>
                {
                    b.HasOne("Solara.Models.User", null)
                        .WithMany("Quests")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Solara.Models.User", b =>
                {
                    b.Navigation("Characters");

                    b.Navigation("Quests");
                });
#pragma warning restore 612, 618
        }
    }
}
