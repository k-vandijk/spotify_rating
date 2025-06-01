using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace spotify_rating.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserPlaylistAiSuggestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isDismissed",
                table: "UserTracks",
                newName: "IsDismissed");

            migrationBuilder.RenameColumn(
                name: "isAiSuggestion",
                table: "UserTracks",
                newName: "IsAiSuggestion");

            migrationBuilder.AddColumn<DateTime>(
                name: "DismissedAtUtc",
                table: "UserPlaylists",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAiSuggestion",
                table: "UserPlaylists",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDismissed",
                table: "UserPlaylists",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_UserPlaylists_PlaylistId",
                table: "UserPlaylists",
                column: "PlaylistId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPlaylists_Playlists_PlaylistId",
                table: "UserPlaylists",
                column: "PlaylistId",
                principalTable: "Playlists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPlaylists_Playlists_PlaylistId",
                table: "UserPlaylists");

            migrationBuilder.DropIndex(
                name: "IX_UserPlaylists_PlaylistId",
                table: "UserPlaylists");

            migrationBuilder.DropColumn(
                name: "DismissedAtUtc",
                table: "UserPlaylists");

            migrationBuilder.DropColumn(
                name: "IsAiSuggestion",
                table: "UserPlaylists");

            migrationBuilder.DropColumn(
                name: "IsDismissed",
                table: "UserPlaylists");

            migrationBuilder.RenameColumn(
                name: "IsDismissed",
                table: "UserTracks",
                newName: "isDismissed");

            migrationBuilder.RenameColumn(
                name: "IsAiSuggestion",
                table: "UserTracks",
                newName: "isAiSuggestion");
        }
    }
}
