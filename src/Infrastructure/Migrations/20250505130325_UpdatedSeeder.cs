using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedSeeder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "WeeklistTasks",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Get AI content list");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "WeeklistTasks",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Create AI content list");
        }
    }
}
