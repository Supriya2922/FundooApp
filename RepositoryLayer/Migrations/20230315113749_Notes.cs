using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryLayer.Migrations
{
    public partial class Notes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "Notes");

            migrationBuilder.AddColumn<bool>(
                name: "trash",
                table: "Notes",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "trash",
                table: "Notes");

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "Notes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
