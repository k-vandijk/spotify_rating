using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace spotify_rating.Web.Migrations
{
    /// <inheritdoc />
    public partial class RecordMoreData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Records",
                newName: "SpotifyUserId");

            migrationBuilder.RenameColumn(
                name: "AlbumCoverUrl",
                table: "Records",
                newName: "SpotifyUri");

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Records",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SpotifyAlbumCoverUrl",
                table: "Records",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SpotifyTrackId",
                table: "Records",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "SpotifyAlbumCoverUrl",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "SpotifyTrackId",
                table: "Records");

            migrationBuilder.RenameColumn(
                name: "SpotifyUserId",
                table: "Records",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "SpotifyUri",
                table: "Records",
                newName: "AlbumCoverUrl");
        }
    }
}
