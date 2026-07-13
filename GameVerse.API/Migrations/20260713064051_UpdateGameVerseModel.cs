using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameVerse.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGameVerseModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFavorite",
                table: "UserGames",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFavorite",
                table: "UserGames");
        }
    }
}
