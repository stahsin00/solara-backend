using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace solara_backend.Migrations
{
    /// <inheritdoc />
    public partial class GameTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hours",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Minutes",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Seconds",
                table: "Games");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "RemainingTime",
                table: "Games",
                type: "time(6)",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RemainingTime",
                table: "Games");

            migrationBuilder.AddColumn<int>(
                name: "Hours",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Minutes",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Seconds",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
