using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrowDay.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class sometablesarechanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HabitRecords_Habits_HabitId",
                table: "HabitRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_UserId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_UserHabits_UserHabitId",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "HabitId",
                table: "HabitRecords",
                newName: "UserHabitId");

            migrationBuilder.RenameIndex(
                name: "IX_HabitRecords_HabitId",
                table: "HabitRecords",
                newName: "IX_HabitRecords_UserHabitId");

            migrationBuilder.AddForeignKey(
                name: "FK_HabitRecords_UserHabits_UserHabitId",
                table: "HabitRecords",
                column: "UserHabitId",
                principalTable: "UserHabits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_UserId",
                table: "Notifications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_UserHabits_UserHabitId",
                table: "Notifications",
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

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_UserId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_UserHabits_UserHabitId",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "UserHabitId",
                table: "HabitRecords",
                newName: "HabitId");

            migrationBuilder.RenameIndex(
                name: "IX_HabitRecords_UserHabitId",
                table: "HabitRecords",
                newName: "IX_HabitRecords_HabitId");

            migrationBuilder.AddForeignKey(
                name: "FK_HabitRecords_Habits_HabitId",
                table: "HabitRecords",
                column: "HabitId",
                principalTable: "Habits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_UserId",
                table: "Notifications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_UserHabits_UserHabitId",
                table: "Notifications",
                column: "UserHabitId",
                principalTable: "UserHabits",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
