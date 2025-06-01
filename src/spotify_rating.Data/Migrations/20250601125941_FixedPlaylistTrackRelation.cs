using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace spotify_rating.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixedPlaylistTrackRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tracks_Playlists_PlaylistId",
                table: "Tracks");

            migrationBuilder.DropIndex(
                name: "IX_Tracks_PlaylistId",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "PlaylistId",
                table: "Tracks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PlaylistId",
                table: "Tracks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_PlaylistId",
                table: "Tracks",
                column: "PlaylistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tracks_Playlists_PlaylistId",
                table: "Tracks",
                column: "PlaylistId",
                principalTable: "Playlists",
                principalColumn: "Id");
        }
    }
}
