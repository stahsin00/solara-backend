using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace solara_backend.Migrations
{
    /// <inheritdoc />
    public partial class CharacterDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeamCharacter1Id",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TeamCharacter2Id",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TeamCharacter3Id",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TeamCharacter4Id",
                table: "Users");

            migrationBuilder.AddColumn<float>(
                name: "AttackStat",
                table: "CharacterInstances",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "CritDamageStat",
                table: "CharacterInstances",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "CritRateStat",
                table: "CharacterInstances",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "EnergyRechargeStat",
                table: "CharacterInstances",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "Experience",
                table: "CharacterInstances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "CharacterInstances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ObtainedAt",
                table: "CharacterInstances",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<float>(
                name: "SpeedStat",
                table: "CharacterInstances",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<bool>(
                name: "Team",
                table: "CharacterInstances",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

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

            migrationBuilder.CreateIndex(
                name: "IX_CharacterInstances_CharacterId",
                table: "CharacterInstances",
                column: "CharacterId");

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterInstances_Characters_CharacterId",
                table: "CharacterInstances",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CharacterInstances_Characters_CharacterId",
                table: "CharacterInstances");

            migrationBuilder.DropTable(
                name: "EquipmentInstances");

            migrationBuilder.DropIndex(
                name: "IX_CharacterInstances_CharacterId",
                table: "CharacterInstances");

            migrationBuilder.DropColumn(
                name: "AttackStat",
                table: "CharacterInstances");

            migrationBuilder.DropColumn(
                name: "CritDamageStat",
                table: "CharacterInstances");

            migrationBuilder.DropColumn(
                name: "CritRateStat",
                table: "CharacterInstances");

            migrationBuilder.DropColumn(
                name: "EnergyRechargeStat",
                table: "CharacterInstances");

            migrationBuilder.DropColumn(
                name: "Experience",
                table: "CharacterInstances");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "CharacterInstances");

            migrationBuilder.DropColumn(
                name: "ObtainedAt",
                table: "CharacterInstances");

            migrationBuilder.DropColumn(
                name: "SpeedStat",
                table: "CharacterInstances");

            migrationBuilder.DropColumn(
                name: "Team",
                table: "CharacterInstances");

            migrationBuilder.AddColumn<int>(
                name: "TeamCharacter1Id",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeamCharacter2Id",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeamCharacter3Id",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeamCharacter4Id",
                table: "Users",
                type: "int",
                nullable: true);
        }
    }
}
