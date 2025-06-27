using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Seeder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "WeeklistTaskStatus",
                columns: new[] { "Id", "Status" },
                values: new object[,]
                {
                    { 1, "Awaiting" },
                    { 2, "Ready" },
                    { 3, "In Progress" },
                    { 4, "Done" }
                });

            migrationBuilder.InsertData(
                table: "WeeklistTasks",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Assign EAN" },
                    { 2, "Insert out of stock" },
                    { 3, "Get AI content list" },
                    { 4, "Upload AI content" },
                    { 5, "Create Checklist" },
                    { 6, "Insert warehouse list" },
                    { 7, "Import product list" }
                });

            migrationBuilder.InsertData(
                table: "Weeklists",
                columns: new[] { "Id", "Number", "OrderNumber", "ShippingNumber", "Supplier" },
                values: new object[] { 1, 101, "E123", "Shipment101", "TVC" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "AfterLastDash", "CategoryId", "Color", "Cost", "Description", "EAN", "Location", "MainImage", "Material", "Price", "Qty", "Series", "Sku", "SupplierSku", "TemplateDescription", "TemplateId", "TemplateTitle", "Title", "WeeklistId", "Weight" },
                values: new object[,]
                {
                    { 1, "", null, null, null, null, null, null, null, null, null, 0, null, "LC01-1001-1", null, null, null, null, "Product One", 1, null },
                    { 2, "", null, null, null, null, null, null, null, null, null, 3, null, "LC01-1001-2", null, null, null, null, "Product Two", 1, null }
                });

            migrationBuilder.InsertData(
                table: "WeeklistTaskLinks",
                columns: new[] { "WeeklistId", "WeeklistTaskId", "AssignedUserId", "WeeklistTaskStatusId" },
                values: new object[,]
                {
                    { 1, 1, "00000000-0000-0000-0000-000000000001", 2 },
                    { 1, 2, "00000000-0000-0000-0000-000000000001", 2 },
                    { 1, 3, "00000000-0000-0000-0000-000000000004", 2 },
                    { 1, 4, "00000000-0000-0000-0000-000000000004", 1 },
                    { 1, 5, "00000000-0000-0000-0000-000000000005", 1 },
                    { 1, 6, "00000000-0000-0000-0000-000000000006", 1 },
                    { 1, 7, "00000000-0000-0000-0000-000000000001", 1 }
                });

            migrationBuilder.InsertData(
                table: "WeeklistTaskUserRoleAssignments",
                columns: new[] { "Id", "UserRole", "WeeklistTaskId" },
                values: new object[,]
                {
                    { 1, "Admin", 1 },
                    { 2, "Admin", 2 },
                    { 3, "Writer", 3 },
                    { 4, "Writer", 4 },
                    { 5, "WarehouseWorker", 5 },
                    { 6, "WarehouseManager", 6 },
                    { 7, "Admin", 7 }
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
                table: "WeeklistTaskStatus",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "WeeklistTaskStatus",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "WeeklistTaskUserRoleAssignments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "WeeklistTaskUserRoleAssignments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "WeeklistTaskUserRoleAssignments",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "WeeklistTaskUserRoleAssignments",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "WeeklistTaskUserRoleAssignments",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "WeeklistTaskUserRoleAssignments",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "WeeklistTaskUserRoleAssignments",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "WeeklistTaskStatus",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "WeeklistTaskStatus",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "WeeklistTasks",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "WeeklistTasks",
                keyColumn: "Id",
                keyValue: 2);

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
                table: "Weeklists",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
