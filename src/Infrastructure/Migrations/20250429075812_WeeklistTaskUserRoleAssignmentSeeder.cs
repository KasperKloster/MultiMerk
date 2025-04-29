using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class WeeklistTaskUserRoleAssignmentSeeder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserRole",
                table: "WeeklistTaskUserRoleAssignments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "WeeklistTaskUserRoleAssignments",
                keyColumn: "Id",
                keyValue: 1,
                column: "UserRole",
                value: "Admin");

            migrationBuilder.UpdateData(
                table: "WeeklistTaskUserRoleAssignments",
                keyColumn: "Id",
                keyValue: 2,
                column: "UserRole",
                value: "Writer");

            migrationBuilder.UpdateData(
                table: "WeeklistTaskUserRoleAssignments",
                keyColumn: "Id",
                keyValue: 3,
                column: "UserRole",
                value: "WarehouseWorker");

            migrationBuilder.UpdateData(
                table: "WeeklistTaskUserRoleAssignments",
                keyColumn: "Id",
                keyValue: 4,
                column: "UserRole",
                value: "WarehouseWorker");

            migrationBuilder.UpdateData(
                table: "WeeklistTaskUserRoleAssignments",
                keyColumn: "Id",
                keyValue: 5,
                column: "UserRole",
                value: "Writer");

            migrationBuilder.UpdateData(
                table: "WeeklistTaskUserRoleAssignments",
                keyColumn: "Id",
                keyValue: 6,
                column: "UserRole",
                value: "Admin");

            migrationBuilder.UpdateData(
                table: "WeeklistTaskUserRoleAssignments",
                keyColumn: "Id",
                keyValue: 7,
                column: "UserRole",
                value: "Admin");

            migrationBuilder.UpdateData(
                table: "WeeklistTaskUserRoleAssignments",
                keyColumn: "Id",
                keyValue: 8,
                column: "UserRole",
                value: "Admin");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserRole",
                table: "WeeklistTaskUserRoleAssignments");
        }
    }
}
