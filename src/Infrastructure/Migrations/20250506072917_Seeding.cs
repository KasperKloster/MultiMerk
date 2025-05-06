using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Seeding : Migration
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
                    { 2, "Get AI content list" },
                    { 3, "Upload AI content" },
                    { 4, "Create Checklist" },
                    { 5, "Create final list" },
                    { 6, "Import product list" },
                    { 7, "Create translations" }
                });

            migrationBuilder.InsertData(
                table: "Weeklists",
                columns: new[] { "Id", "Number", "OrderNumber", "ShippingNumber", "Supplier" },
                values: new object[,]
                {
                    { 1, 101, "E123", "Shipment101", "TVC" },
                    { 2, 102, "E321", "Shipment102", "TVC" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Color", "Cost", "Description", "EAN", "Location", "MainImage", "Material", "Price", "Qty", "Series", "Sku", "SupplierSku", "TemplateId", "Title", "WeeklistId", "Weight" },
                values: new object[,]
                {
                    { 1, null, null, null, null, null, null, null, null, null, null, null, "LC01-1001-1", null, null, null, 1, null },
                    { 2, null, null, null, null, null, null, null, null, null, null, null, "LC01-1001-2", null, null, null, 1, null },
                    { 3, null, null, null, null, null, null, null, null, null, null, null, "LC02-2002-1", null, null, null, 2, null }
                });

            migrationBuilder.InsertData(
                table: "WeeklistTaskLinks",
                columns: new[] { "WeeklistId", "WeeklistTaskId", "AssignedUserId", "WeeklistTaskStatusId" },
                values: new object[,]
                {
                    { 1, 1, "00000000-0000-0000-0000-000000000001", 4 },
                    { 1, 2, "00000000-0000-0000-0000-000000000004", 4 },
                    { 1, 3, "00000000-0000-0000-0000-000000000004", 1 },
                    { 1, 4, "00000000-0000-0000-0000-000000000005", 2 },
                    { 1, 5, "00000000-0000-0000-0000-000000000001", 2 },
                    { 1, 6, "00000000-0000-0000-0000-000000000001", 1 },
                    { 1, 7, "00000000-0000-0000-0000-000000000001", 1 },
                    { 2, 1, "00000000-0000-0000-0000-000000000001", 2 },
                    { 2, 2, "00000000-0000-0000-0000-000000000004", 2 },
                    { 2, 3, "00000000-0000-0000-0000-000000000004", 1 },
                    { 2, 4, "00000000-0000-0000-0000-000000000005", 2 },
                    { 2, 5, "00000000-0000-0000-0000-000000000001", 1 },
                    { 2, 6, "00000000-0000-0000-0000-000000000001", 1 },
                    { 2, 7, "00000000-0000-0000-0000-000000000001", 1 }
                });

            migrationBuilder.InsertData(
                table: "WeeklistTaskUserRoleAssignments",
                columns: new[] { "Id", "UserRole", "WeeklistTaskId" },
                values: new object[,]
                {
                    { 1, "Admin", 1 },
                    { 2, "Writer", 2 },
                    { 3, "Writer", 3 },
                    { 4, "WarehouseWorker", 4 },
                    { 5, "Admin", 5 },
                    { 6, "Admin", 6 },
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
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

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
                table: "WeeklistTaskStatus",
                keyColumn: "Id",
                keyValue: 3);

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
                table: "WeeklistTaskStatus",
                keyColumn: "Id",
                keyValue: 4);

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

            migrationBuilder.DeleteData(
                table: "Weeklists",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
