using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace solara_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddTeam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FullArt = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FaceArt = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Element = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Price = table.Column<int>(type: "int", nullable: false),
                    BaseAttackStat = table.Column<int>(type: "int", nullable: false),
                    BaseSpeedStat = table.Column<int>(type: "int", nullable: false),
                    BaseCritRateStat = table.Column<float>(type: "float", nullable: false),
                    BaseCritDamageStat = table.Column<float>(type: "float", nullable: false),
                    BaseEnergyRechargeStat = table.Column<float>(type: "float", nullable: false),
                    BasicAttack = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SpecialAttack = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EquipmentInstances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ObtainedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentInstances", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CharacterInstances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CharacterId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Experience = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    AttackStat = table.Column<float>(type: "float", nullable: false),
                    SpeedStat = table.Column<float>(type: "float", nullable: false),
                    CritRateStat = table.Column<float>(type: "float", nullable: false),
                    CritDamageStat = table.Column<float>(type: "float", nullable: false),
                    EnergyRechargeStat = table.Column<float>(type: "float", nullable: false),
                    Team = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ObtainedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterInstances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterInstances_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProfilePicture = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OAuthProvider = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OAuthProviderUserId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Balance = table.Column<int>(type: "int", nullable: false),
                    Exp = table.Column<int>(type: "int", nullable: false),
                    Teamcharacter1Id = table.Column<int>(type: "int", nullable: true),
                    Teamcharacter2Id = table.Column<int>(type: "int", nullable: true),
                    Teamcharacter3Id = table.Column<int>(type: "int", nullable: true),
                    Teamcharacter4Id = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_CharacterInstances_Teamcharacter1Id",
                        column: x => x.Teamcharacter1Id,
                        principalTable: "CharacterInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Users_CharacterInstances_Teamcharacter2Id",
                        column: x => x.Teamcharacter2Id,
                        principalTable: "CharacterInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Users_CharacterInstances_Teamcharacter3Id",
                        column: x => x.Teamcharacter3Id,
                        principalTable: "CharacterInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Users_CharacterInstances_Teamcharacter4Id",
                        column: x => x.Teamcharacter4Id,
                        principalTable: "CharacterInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Quests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Deadline = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Difficulty = table.Column<int>(type: "int", nullable: false),
                    Important = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Complete = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Type = table.Column<string>(type: "varchar(13)", maxLength: 13, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EndDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Repetition = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Characters",
                columns: new[] { "Id", "BaseAttackStat", "BaseCritDamageStat", "BaseCritRateStat", "BaseEnergyRechargeStat", "BaseSpeedStat", "BasicAttack", "Description", "Element", "FaceArt", "FullArt", "Name", "Price", "SpecialAttack" },
                values: new object[,]
                {
                    { 1, 100, 1.5f, 0.1f, 1f, 20, "Sword Slash", "A laid-back wanderer drifting through Solara.", "Blue", "art/margana-face.png", "art/margana.png", "Margana", 5000, "Blazing Strike" },
                    { 2, 80, 1.4f, 0.12f, 1.2f, 15, "Water Blast", "A passionate artist known to get fired up.", "Magenta", "art/a-face.png", "art/a.png", "Artemisia", 5000, "Tsunami" },
                    { 3, 80, 1.4f, 0.12f, 1.2f, 15, "Water Blast", "A hardworking maid just trying her best.", "Magenta", "art/b-face.png", "art/b.png", "Powder", 5000, "Tsunami" },
                    { 4, 80, 1.4f, 0.12f, 1.2f, 15, "Water Blast", "A medical student currently enrolled at the University of Solara.", "Magenta", "art/e-face.png", "art/e.png", "Ann", 5000, "Tsunami" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterInstances_CharacterId",
                table: "CharacterInstances",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterInstances_UserId",
                table: "CharacterInstances",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Quests_UserId",
                table: "Quests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Teamcharacter1Id",
                table: "Users",
                column: "Teamcharacter1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Teamcharacter2Id",
                table: "Users",
                column: "Teamcharacter2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Teamcharacter3Id",
                table: "Users",
                column: "Teamcharacter3Id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Teamcharacter4Id",
                table: "Users",
                column: "Teamcharacter4Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterInstances_Users_UserId",
                table: "CharacterInstances",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CharacterInstances_Characters_CharacterId",
                table: "CharacterInstances");

            migrationBuilder.DropForeignKey(
                name: "FK_CharacterInstances_Users_UserId",
                table: "CharacterInstances");

            migrationBuilder.DropTable(
                name: "EquipmentInstances");

            migrationBuilder.DropTable(
                name: "Quests");

            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "CharacterInstances");
        }
    }
}
