using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurfProjekt.Migrations
{
    public partial class IsPremiumPropertyAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPremium",
                table: "Boards",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPremium",
                table: "Boards");
        }
    }
}
