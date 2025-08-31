using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrowDay.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class UserHabitConfigurationupdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HabitRecords_UserHabits_UserHabitId",
                table: "HabitRecords");

            migrationBuilder.AddForeignKey(
                name: "FK_HabitRecords_UserHabits_UserHabitId",
                table: "HabitRecords",
                column: "UserHabitId",
                principalTable: "UserHabits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HabitRecords_UserHabits_UserHabitId",
                table: "HabitRecords");

            migrationBuilder.AddForeignKey(
                name: "FK_HabitRecords_UserHabits_UserHabitId",
                table: "HabitRecords",
                column: "UserHabitId",
                principalTable: "UserHabits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
