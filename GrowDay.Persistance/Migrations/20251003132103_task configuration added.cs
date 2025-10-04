using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrowDay.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class taskconfigurationadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Habits_HabitId",
                table: "Tasks");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Habits_HabitId",
                table: "Tasks",
                column: "HabitId",
                principalTable: "Habits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Habits_HabitId",
                table: "Tasks");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Habits_HabitId",
                table: "Tasks",
                column: "HabitId",
                principalTable: "Habits",
                principalColumn: "Id");
        }
    }
}
