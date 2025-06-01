using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace spotify_rating.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangedPlaylistVariableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlaylistDescription",
                table: "Playlists");

            migrationBuilder.RenameColumn(
                name: "PlaylistName",
                table: "Playlists",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Playlists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Genre",
                table: "Playlists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Playlists");

            migrationBuilder.DropColumn(
                name: "Genre",
                table: "Playlists");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Playlists",
                newName: "PlaylistName");

            migrationBuilder.AddColumn<string>(
                name: "PlaylistDescription",
                table: "Playlists",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
