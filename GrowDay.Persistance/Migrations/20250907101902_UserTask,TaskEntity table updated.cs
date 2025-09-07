using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrowDay.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class UserTaskTaskEntitytableupdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HabitId",
                table: "Tasks",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_HabitId",
                table: "Tasks",
                column: "HabitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Habits_HabitId",
                table: "Tasks",
                column: "HabitId",
                principalTable: "Habits",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Habits_HabitId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_HabitId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "HabitId",
                table: "Tasks");
        }
    }
}
