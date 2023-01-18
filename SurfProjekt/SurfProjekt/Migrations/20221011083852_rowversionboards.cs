using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurfProjekt.Migrations
{
    public partial class rowversionboards : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersionBoards",
                table: "Boards",
                type: "rowversion",
                rowVersion: true,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersionBoards",
                table: "Boards");
        }
    }
}
