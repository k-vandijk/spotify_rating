using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace spotify_rating.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserTrackAiSuggestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistTracks_Playlists_PlaylistId",
                table: "PlaylistTracks");

            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistTracks_Tracks_TrackId",
                table: "PlaylistTracks");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPlaylists_Playlists_PlaylistId",
                table: "UserPlaylists");

            migrationBuilder.DropIndex(
                name: "IX_UserPlaylists_PlaylistId",
                table: "UserPlaylists");

            migrationBuilder.DropIndex(
                name: "IX_PlaylistTracks_PlaylistId",
                table: "PlaylistTracks");

            migrationBuilder.DropIndex(
                name: "IX_PlaylistTracks_TrackId",
                table: "PlaylistTracks");

            migrationBuilder.AddColumn<DateTime>(
                name: "DismissedAtUtc",
                table: "UserTracks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isAiSuggestion",
                table: "UserTracks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isDismissed",
                table: "UserTracks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DismissedAtUtc",
                table: "UserTracks");

            migrationBuilder.DropColumn(
                name: "isAiSuggestion",
                table: "UserTracks");

            migrationBuilder.DropColumn(
                name: "isDismissed",
                table: "UserTracks");

            migrationBuilder.CreateIndex(
                name: "IX_UserPlaylists_PlaylistId",
                table: "UserPlaylists",
                column: "PlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistTracks_PlaylistId",
                table: "PlaylistTracks",
                column: "PlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistTracks_TrackId",
                table: "PlaylistTracks",
                column: "TrackId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistTracks_Playlists_PlaylistId",
                table: "PlaylistTracks",
                column: "PlaylistId",
                principalTable: "Playlists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistTracks_Tracks_TrackId",
                table: "PlaylistTracks",
                column: "TrackId",
                principalTable: "Tracks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPlaylists_Playlists_PlaylistId",
                table: "UserPlaylists",
                column: "PlaylistId",
                principalTable: "Playlists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
