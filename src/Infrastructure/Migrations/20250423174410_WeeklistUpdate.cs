using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class WeeklistUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "WeeklistTasks",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Assign EAN");

            migrationBuilder.UpdateData(
                table: "WeeklistTasks",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Create AI content list");

            migrationBuilder.InsertData(
                table: "WeeklistTasks",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 3, "Assign location" },
                    { 4, "Assign correct quantity" },
                    { 5, "Upload AI content" },
                    { 6, "Create final list" },
                    { 7, "Import product list" },
                    { 8, "Create translations" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WeeklistTasks",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "WeeklistTasks",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "WeeklistTasks",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "WeeklistTasks",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "WeeklistTasks",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "WeeklistTasks",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.UpdateData(
                table: "WeeklistTasks",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Give EAN");

            migrationBuilder.UpdateData(
                table: "WeeklistTasks",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Prepare for AI");
        }
    }
}
