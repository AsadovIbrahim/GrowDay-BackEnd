using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrowDay.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class FixNotificationCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_UserHabits_UserHabitId",
                table: "Notifications");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_UserHabits_UserHabitId",
                table: "Notifications",
                column: "UserHabitId",
                principalTable: "UserHabits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_UserHabits_UserHabitId",
                table: "Notifications");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_UserHabits_UserHabitId",
                table: "Notifications",
                column: "UserHabitId",
                principalTable: "UserHabits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
