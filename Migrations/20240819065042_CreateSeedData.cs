using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace solara_backend.Migrations
{
    /// <inheritdoc />
    public partial class CreateSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Characters");
        }
    }
}
