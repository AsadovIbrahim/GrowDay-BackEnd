using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrowDay.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class habittableanduserhabittableupdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentValue",
                table: "UserHabits",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TargetValue",
                table: "UserHabits",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "UserHabits",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TargetValue",
                table: "Habits",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "Habits",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentValue",
                table: "UserHabits");

            migrationBuilder.DropColumn(
                name: "TargetValue",
                table: "UserHabits");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "UserHabits");

            migrationBuilder.DropColumn(
                name: "TargetValue",
                table: "Habits");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "Habits");
        }
    }
}
