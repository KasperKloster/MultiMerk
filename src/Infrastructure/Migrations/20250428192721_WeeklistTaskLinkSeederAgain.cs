using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class WeeklistTaskLinkSeederAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "WeeklistTaskLinks",
                columns: new[] { "WeeklistId", "WeeklistTaskId", "AssignedUserId", "WeeklistTaskStatusId" },
                values: new object[,]
                {
                    { 1, 1, null, 2 },
                    { 1, 2, null, 1 },
                    { 1, 3, null, 1 },
                    { 1, 4, null, 1 },
                    { 1, 5, null, 1 },
                    { 1, 6, null, 1 },
                    { 1, 7, null, 1 },
                    { 1, 8, null, 1 },
                    { 2, 1, null, 4 },
                    { 2, 2, null, 4 },
                    { 2, 3, null, 4 },
                    { 2, 4, null, 4 },
                    { 2, 5, null, 4 },
                    { 2, 6, null, 4 },
                    { 2, 7, null, 3 },
                    { 2, 8, null, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WeeklistTaskLinks",
                keyColumns: new[] { "WeeklistId", "WeeklistTaskId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "WeeklistTaskLinks",
                keyColumns: new[] { "WeeklistId", "WeeklistTaskId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "WeeklistTaskLinks",
                keyColumns: new[] { "WeeklistId", "WeeklistTaskId" },
                keyValues: new object[] { 1, 3 });

            migrationBuilder.DeleteData(
                table: "WeeklistTaskLinks",
                keyColumns: new[] { "WeeklistId", "WeeklistTaskId" },
                keyValues: new object[] { 1, 4 });

            migrationBuilder.DeleteData(
                table: "WeeklistTaskLinks",
                keyColumns: new[] { "WeeklistId", "WeeklistTaskId" },
                keyValues: new object[] { 1, 5 });

            migrationBuilder.DeleteData(
                table: "WeeklistTaskLinks",
                keyColumns: new[] { "WeeklistId", "WeeklistTaskId" },
                keyValues: new object[] { 1, 6 });

            migrationBuilder.DeleteData(
                table: "WeeklistTaskLinks",
                keyColumns: new[] { "WeeklistId", "WeeklistTaskId" },
                keyValues: new object[] { 1, 7 });

            migrationBuilder.DeleteData(
                table: "WeeklistTaskLinks",
                keyColumns: new[] { "WeeklistId", "WeeklistTaskId" },
                keyValues: new object[] { 1, 8 });

            migrationBuilder.DeleteData(
                table: "WeeklistTaskLinks",
                keyColumns: new[] { "WeeklistId", "WeeklistTaskId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "WeeklistTaskLinks",
                keyColumns: new[] { "WeeklistId", "WeeklistTaskId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "WeeklistTaskLinks",
                keyColumns: new[] { "WeeklistId", "WeeklistTaskId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "WeeklistTaskLinks",
                keyColumns: new[] { "WeeklistId", "WeeklistTaskId" },
                keyValues: new object[] { 2, 4 });

            migrationBuilder.DeleteData(
                table: "WeeklistTaskLinks",
                keyColumns: new[] { "WeeklistId", "WeeklistTaskId" },
                keyValues: new object[] { 2, 5 });

            migrationBuilder.DeleteData(
                table: "WeeklistTaskLinks",
                keyColumns: new[] { "WeeklistId", "WeeklistTaskId" },
                keyValues: new object[] { 2, 6 });

            migrationBuilder.DeleteData(
                table: "WeeklistTaskLinks",
                keyColumns: new[] { "WeeklistId", "WeeklistTaskId" },
                keyValues: new object[] { 2, 7 });

            migrationBuilder.DeleteData(
                table: "WeeklistTaskLinks",
                keyColumns: new[] { "WeeklistId", "WeeklistTaskId" },
                keyValues: new object[] { 2, 8 });
        }
    }
}
