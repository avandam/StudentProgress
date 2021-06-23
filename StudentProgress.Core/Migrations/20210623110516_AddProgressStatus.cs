using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentProgress.Core.Migrations
{
    public partial class AddProgressStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProgressStatus",
                table: "ProgressUpdate",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProgressStatus",
                table: "ProgressUpdate");
        }
    }
}
