using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyNet6Demo.Infrastructure.Migrations
{
    public partial class imageContentType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageContentType",
                table: "Albums",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageContentType",
                table: "Albums");
        }
    }
}
