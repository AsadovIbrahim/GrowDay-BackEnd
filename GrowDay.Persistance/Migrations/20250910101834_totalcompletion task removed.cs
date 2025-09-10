using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrowDay.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class totalcompletiontaskremoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalCompletionTask",
                table: "UserTaskCompletions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalCompletionTask",
                table: "UserTaskCompletions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
