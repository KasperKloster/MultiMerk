using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class WeeklistTaskAssignmentUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "WeeklistTaskAssignments",
                columns: new[] { "Id", "ApplicationUserId", "WeeklistTaskId" },
                values: new object[,]
                {
                    { 1, null, 1 },
                    { 2, null, 2 },
                    { 3, null, 3 },
                    { 4, null, 4 },
                    { 5, null, 5 },
                    { 6, null, 6 },
                    { 7, null, 7 },
                    { 8, null, 8 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WeeklistTaskAssignments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "WeeklistTaskAssignments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "WeeklistTaskAssignments",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "WeeklistTaskAssignments",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "WeeklistTaskAssignments",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "WeeklistTaskAssignments",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "WeeklistTaskAssignments",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "WeeklistTaskAssignments",
                keyColumn: "Id",
                keyValue: 8);
        }
    }
}
