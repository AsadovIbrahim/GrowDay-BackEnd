using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrowDay.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class userhabitandhabittableincrementvalueadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IncrementValue",
                table: "UserHabits",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IncrementValue",
                table: "Habits",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IncrementValue",
                table: "UserHabits");

            migrationBuilder.DropColumn(
                name: "IncrementValue",
                table: "Habits");
        }
    }
}
