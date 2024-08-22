using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace solara_backend.Migrations
{
    /// <inheritdoc />
    public partial class TeamPosition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Team",
                table: "CharacterInstances");

            migrationBuilder.AddColumn<int>(
                name: "TeamPos",
                table: "CharacterInstances",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeamPos",
                table: "CharacterInstances");

            migrationBuilder.AddColumn<bool>(
                name: "Team",
                table: "CharacterInstances",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
