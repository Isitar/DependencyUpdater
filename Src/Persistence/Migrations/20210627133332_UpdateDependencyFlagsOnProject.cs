using Microsoft.EntityFrameworkCore.Migrations;

namespace Isitar.DependencyUpdater.Persistence.Migrations
{
    public partial class UpdateDependencyFlagsOnProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CheckRequested",
                table: "Projects",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsChecking",
                table: "Projects",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckRequested",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IsChecking",
                table: "Projects");
        }
    }
}
