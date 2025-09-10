using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrowDay.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class userhabittableupdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RequiredPoints",
                table: "UserTasks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StreakRequired",
                table: "UserTasks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalRequiredCompletions",
                table: "UserTasks",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequiredPoints",
                table: "UserTasks");

            migrationBuilder.DropColumn(
                name: "StreakRequired",
                table: "UserTasks");

            migrationBuilder.DropColumn(
                name: "TotalRequiredCompletions",
                table: "UserTasks");
        }
    }
}
