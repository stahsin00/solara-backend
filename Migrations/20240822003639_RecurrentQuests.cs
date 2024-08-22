using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace solara_backend.Migrations
{
    /// <inheritdoc />
    public partial class RecurrentQuests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quests_Users_UserId",
                table: "Quests");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Quests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Deadline",
                table: "Quests",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Difficulty",
                table: "Quests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Quests",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Important",
                table: "Quests",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Repetition",
                table: "Quests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Quests",
                type: "varchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Quests_Users_UserId",
                table: "Quests",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quests_Users_UserId",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "Deadline",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "Difficulty",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "Important",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "Repetition",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Quests");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Quests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Quests_Users_UserId",
                table: "Quests",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
