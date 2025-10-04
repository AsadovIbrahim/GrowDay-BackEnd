using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrowDay.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class achievementconfigurationadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Achievements_Habits_HabitId",
                table: "Achievements");

            migrationBuilder.AddForeignKey(
                name: "FK_Achievements_Habits_HabitId",
                table: "Achievements",
                column: "HabitId",
                principalTable: "Habits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Achievements_Habits_HabitId",
                table: "Achievements");

            migrationBuilder.AddForeignKey(
                name: "FK_Achievements_Habits_HabitId",
                table: "Achievements",
                column: "HabitId",
                principalTable: "Habits",
                principalColumn: "Id");
        }
    }
}
