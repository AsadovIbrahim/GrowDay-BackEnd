using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrowDay.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class achievementtableaddedHabitId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HabitId",
                table: "Achievements",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Achievements_HabitId",
                table: "Achievements",
                column: "HabitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Achievements_Habits_HabitId",
                table: "Achievements",
                column: "HabitId",
                principalTable: "Habits",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Achievements_Habits_HabitId",
                table: "Achievements");

            migrationBuilder.DropIndex(
                name: "IX_Achievements_HabitId",
                table: "Achievements");

            migrationBuilder.DropColumn(
                name: "HabitId",
                table: "Achievements");
        }
    }
}
