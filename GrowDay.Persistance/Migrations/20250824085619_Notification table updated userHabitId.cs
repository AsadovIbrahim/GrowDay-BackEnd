using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrowDay.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class NotificationtableupdateduserHabitId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Habits_HabitId",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "HabitId",
                table: "Notifications",
                newName: "UserHabitId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_HabitId",
                table: "Notifications",
                newName: "IX_Notifications_UserHabitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_UserHabit_UserHabitId",
                table: "Notifications",
                column: "UserHabitId",
                principalTable: "UserHabit",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_UserHabit_UserHabitId",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "UserHabitId",
                table: "Notifications",
                newName: "HabitId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_UserHabitId",
                table: "Notifications",
                newName: "IX_Notifications_HabitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Habits_HabitId",
                table: "Notifications",
                column: "HabitId",
                principalTable: "Habits",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
