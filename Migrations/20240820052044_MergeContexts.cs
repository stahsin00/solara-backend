using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace solara_backend.Migrations
{
    /// <inheritdoc />
    public partial class MergeContexts : Migration
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
                    TeamCharacter1Id = table.Column<int>(type: "int", nullable: true),
                    TeamCharacter2Id = table.Column<int>(type: "int", nullable: true),
                    TeamCharacter3Id = table.Column<int>(type: "int", nullable: true),
                    TeamCharacter4Id = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CharacterInstances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CharacterId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterInstances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterInstances_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Quest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quest_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
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
                name: "IX_CharacterInstances_UserId",
                table: "CharacterInstances",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Quest_UserId",
                table: "Quest",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterInstances");

            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "Quest");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
