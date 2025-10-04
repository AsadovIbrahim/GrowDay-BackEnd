using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrowDay.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class userhabitconfigurationupdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserHabits_Habits_HabitId",
                table: "UserHabits");

            migrationBuilder.AddForeignKey(
                name: "FK_UserHabits_Habits_HabitId",
                table: "UserHabits",
                column: "HabitId",
                principalTable: "Habits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserHabits_Habits_HabitId",
                table: "UserHabits");

            migrationBuilder.AddForeignKey(
                name: "FK_UserHabits_Habits_HabitId",
                table: "UserHabits",
                column: "HabitId",
                principalTable: "Habits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
