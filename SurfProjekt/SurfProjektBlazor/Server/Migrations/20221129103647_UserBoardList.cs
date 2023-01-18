using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurfProjektBlazor.Server.Migrations
{
    public partial class UserBoardList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Boards",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Boards_ApplicationUserId",
                table: "Boards",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Boards_AspNetUsers_ApplicationUserId",
                table: "Boards",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boards_AspNetUsers_ApplicationUserId",
                table: "Boards");

            migrationBuilder.DropIndex(
                name: "IX_Boards_ApplicationUserId",
                table: "Boards");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Boards");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");
        }
    }
}
