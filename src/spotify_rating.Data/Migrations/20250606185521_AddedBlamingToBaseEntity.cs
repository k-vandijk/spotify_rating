using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace spotify_rating.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedBlamingToBaseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtUtc",
                table: "UserTracks",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "UserTracks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeactivatedAtUtc",
                table: "UserTracks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeactivatedBy",
                table: "UserTracks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "UserTracks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtUtc",
                table: "UserPlaylists",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "UserPlaylists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeactivatedAtUtc",
                table: "UserPlaylists",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeactivatedBy",
                table: "UserPlaylists",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "UserPlaylists",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtUtc",
                table: "Tracks",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Tracks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeactivatedAtUtc",
                table: "Tracks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeactivatedBy",
                table: "Tracks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Tracks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtUtc",
                table: "PlaylistTracks",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "PlaylistTracks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeactivatedAtUtc",
                table: "PlaylistTracks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeactivatedBy",
                table: "PlaylistTracks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "PlaylistTracks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtUtc",
                table: "Playlists",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Playlists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeactivatedAtUtc",
                table: "Playlists",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeactivatedBy",
                table: "Playlists",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Playlists",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistTracks_TrackId",
                table: "PlaylistTracks",
                column: "TrackId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistTracks_Tracks_TrackId",
                table: "PlaylistTracks",
                column: "TrackId",
                principalTable: "Tracks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistTracks_Tracks_TrackId",
                table: "PlaylistTracks");

            migrationBuilder.DropIndex(
                name: "IX_PlaylistTracks_TrackId",
                table: "PlaylistTracks");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "UserTracks");

            migrationBuilder.DropColumn(
                name: "DeactivatedAtUtc",
                table: "UserTracks");

            migrationBuilder.DropColumn(
                name: "DeactivatedBy",
                table: "UserTracks");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "UserTracks");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "UserPlaylists");

            migrationBuilder.DropColumn(
                name: "DeactivatedAtUtc",
                table: "UserPlaylists");

            migrationBuilder.DropColumn(
                name: "DeactivatedBy",
                table: "UserPlaylists");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "UserPlaylists");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "DeactivatedAtUtc",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "DeactivatedBy",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "PlaylistTracks");

            migrationBuilder.DropColumn(
                name: "DeactivatedAtUtc",
                table: "PlaylistTracks");

            migrationBuilder.DropColumn(
                name: "DeactivatedBy",
                table: "PlaylistTracks");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "PlaylistTracks");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Playlists");

            migrationBuilder.DropColumn(
                name: "DeactivatedAtUtc",
                table: "Playlists");

            migrationBuilder.DropColumn(
                name: "DeactivatedBy",
                table: "Playlists");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Playlists");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtUtc",
                table: "UserTracks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtUtc",
                table: "UserPlaylists",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtUtc",
                table: "Tracks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtUtc",
                table: "PlaylistTracks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtUtc",
                table: "Playlists",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
