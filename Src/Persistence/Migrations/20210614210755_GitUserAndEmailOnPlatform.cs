using Microsoft.EntityFrameworkCore.Migrations;

namespace Isitar.DependencyUpdater.Persistence.Migrations
{
    public partial class GitUserAndEmailOnPlatform : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GitUserEmail",
                table: "Platforms",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GitUserName",
                table: "Platforms",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GitUserEmail",
                table: "Platforms");

            migrationBuilder.DropColumn(
                name: "GitUserName",
                table: "Platforms");
        }
    }
}
