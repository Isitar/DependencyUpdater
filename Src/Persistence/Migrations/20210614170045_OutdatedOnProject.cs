using Microsoft.EntityFrameworkCore.Migrations;

namespace Isitar.DependencyUpdater.Persistence.Migrations
{
    public partial class OutdatedOnProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOutdated",
                table: "Projects",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOutdated",
                table: "Projects");
        }
    }
}
