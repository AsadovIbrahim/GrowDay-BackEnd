using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrowDay.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class tasktableaddedtotalrequirementandrequiredpointsadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RequiredPoints",
                table: "Tasks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalRequiredCompletions",
                table: "Tasks",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequiredPoints",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "TotalRequiredCompletions",
                table: "Tasks");
        }
    }
}
