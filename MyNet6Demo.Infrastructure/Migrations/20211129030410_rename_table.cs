using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyNet6Demo.Infrastructure.Migrations
{
    public partial class rename_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Song_Albums_AlbumId",
                table: "Song");

            migrationBuilder.DropForeignKey(
                name: "FK_SongArtist_Artist_ArtistId",
                table: "SongArtist");

            migrationBuilder.DropForeignKey(
                name: "FK_SongArtist_Song_SongId",
                table: "SongArtist");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SongArtist",
                table: "SongArtist");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Song",
                table: "Song");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Artist",
                table: "Artist");

            migrationBuilder.RenameTable(
                name: "SongArtist",
                newName: "SongArtists");

            migrationBuilder.RenameTable(
                name: "Song",
                newName: "Songs");

            migrationBuilder.RenameTable(
                name: "Artist",
                newName: "Artists");

            migrationBuilder.RenameIndex(
                name: "IX_SongArtist_SongId",
                table: "SongArtists",
                newName: "IX_SongArtists_SongId");

            migrationBuilder.RenameIndex(
                name: "IX_SongArtist_ArtistId",
                table: "SongArtists",
                newName: "IX_SongArtists_ArtistId");

            migrationBuilder.RenameIndex(
                name: "IX_Song_Id",
                table: "Songs",
                newName: "IX_Songs_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Song_AlbumId",
                table: "Songs",
                newName: "IX_Songs_AlbumId");

            migrationBuilder.RenameIndex(
                name: "IX_Artist_Id",
                table: "Artists",
                newName: "IX_Artists_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SongArtists",
                table: "SongArtists",
                columns: new[] { "ArtistId", "SongId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Songs",
                table: "Songs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Artists",
                table: "Artists",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SongArtists_Artists_ArtistId",
                table: "SongArtists",
                column: "ArtistId",
                principalTable: "Artists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SongArtists_Songs_SongId",
                table: "SongArtists",
                column: "SongId",
                principalTable: "Songs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_Albums_AlbumId",
                table: "Songs",
                column: "AlbumId",
                principalTable: "Albums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SongArtists_Artists_ArtistId",
                table: "SongArtists");

            migrationBuilder.DropForeignKey(
                name: "FK_SongArtists_Songs_SongId",
                table: "SongArtists");

            migrationBuilder.DropForeignKey(
                name: "FK_Songs_Albums_AlbumId",
                table: "Songs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Songs",
                table: "Songs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SongArtists",
                table: "SongArtists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Artists",
                table: "Artists");

            migrationBuilder.RenameTable(
                name: "Songs",
                newName: "Song");

            migrationBuilder.RenameTable(
                name: "SongArtists",
                newName: "SongArtist");

            migrationBuilder.RenameTable(
                name: "Artists",
                newName: "Artist");

            migrationBuilder.RenameIndex(
                name: "IX_Songs_Id",
                table: "Song",
                newName: "IX_Song_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Songs_AlbumId",
                table: "Song",
                newName: "IX_Song_AlbumId");

            migrationBuilder.RenameIndex(
                name: "IX_SongArtists_SongId",
                table: "SongArtist",
                newName: "IX_SongArtist_SongId");

            migrationBuilder.RenameIndex(
                name: "IX_SongArtists_ArtistId",
                table: "SongArtist",
                newName: "IX_SongArtist_ArtistId");

            migrationBuilder.RenameIndex(
                name: "IX_Artists_Id",
                table: "Artist",
                newName: "IX_Artist_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Song",
                table: "Song",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SongArtist",
                table: "SongArtist",
                columns: new[] { "ArtistId", "SongId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Artist",
                table: "Artist",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Song_Albums_AlbumId",
                table: "Song",
                column: "AlbumId",
                principalTable: "Albums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SongArtist_Artist_ArtistId",
                table: "SongArtist",
                column: "ArtistId",
                principalTable: "Artist",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SongArtist_Song_SongId",
                table: "SongArtist",
                column: "SongId",
                principalTable: "Song",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
