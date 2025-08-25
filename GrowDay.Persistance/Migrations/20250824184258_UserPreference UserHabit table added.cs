using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrowDay.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class UserPreferenceUserHabittableadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_UserHabit_UserHabitId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_UserHabit_AspNetUsers_UserId",
                table: "UserHabit");

            migrationBuilder.DropForeignKey(
                name: "FK_UserHabit_Habits_HabitId",
                table: "UserHabit");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserHabit",
                table: "UserHabit");

            migrationBuilder.RenameTable(
                name: "UserHabit",
                newName: "UserHabits");

            migrationBuilder.RenameIndex(
                name: "IX_UserHabit_UserId",
                table: "UserHabits",
                newName: "IX_UserHabits_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserHabit_HabitId",
                table: "UserHabits",
                newName: "IX_UserHabits_HabitId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserHabits",
                table: "UserHabits",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UserPreferences",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WakeUpTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    SleepTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    ProcrestinateFrequency = table.Column<int>(type: "int", nullable: false),
                    FocusDifficulty = table.Column<int>(type: "int", nullable: false),
                    MotivationalFactors = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPreferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPreferences_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPreferences_UserId",
                table: "UserPreferences",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_UserHabits_UserHabitId",
                table: "Notifications",
                column: "UserHabitId",
                principalTable: "UserHabits",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_UserHabits_AspNetUsers_UserId",
                table: "UserHabits",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_Notifications_UserHabits_UserHabitId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_UserHabits_AspNetUsers_UserId",
                table: "UserHabits");

            migrationBuilder.DropForeignKey(
                name: "FK_UserHabits_Habits_HabitId",
                table: "UserHabits");

            migrationBuilder.DropTable(
                name: "UserPreferences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserHabits",
                table: "UserHabits");

            migrationBuilder.RenameTable(
                name: "UserHabits",
                newName: "UserHabit");

            migrationBuilder.RenameIndex(
                name: "IX_UserHabits_UserId",
                table: "UserHabit",
                newName: "IX_UserHabit_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserHabits_HabitId",
                table: "UserHabit",
                newName: "IX_UserHabit_HabitId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserHabit",
                table: "UserHabit",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_UserHabit_UserHabitId",
                table: "Notifications",
                column: "UserHabitId",
                principalTable: "UserHabit",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_UserHabit_AspNetUsers_UserId",
                table: "UserHabit",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserHabit_Habits_HabitId",
                table: "UserHabit",
                column: "HabitId",
                principalTable: "Habits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
