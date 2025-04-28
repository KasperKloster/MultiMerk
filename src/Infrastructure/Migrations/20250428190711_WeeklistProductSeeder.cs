using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class WeeklistProductSeeder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Weeklists",
                columns: new[] { "Id", "Number", "OrderNumber", "Supplier" },
                values: new object[,]
                {
                    { 1, 101, "E123", "TVC" },
                    { 2, 102, "E321", "TVC" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Sku", "WeeklistId" },
                values: new object[,]
                {
                    { 1, "LC01-1001-1", 1 },
                    { 2, "LC01-1001-2", 1 },
                    { 3, "LC02-2002-1", 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Weeklists",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Weeklists",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
