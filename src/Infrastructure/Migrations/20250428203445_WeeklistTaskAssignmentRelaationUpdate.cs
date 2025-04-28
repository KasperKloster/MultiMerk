using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class WeeklistTaskAssignmentRelaationUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeeklistTaskAssignments_AspNetUsers_ApplicationUserId",
                table: "WeeklistTaskAssignments");

            migrationBuilder.DropIndex(
                name: "IX_WeeklistTaskAssignments_ApplicationUserId",
                table: "WeeklistTaskAssignments");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "WeeklistTaskAssignments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "WeeklistTaskAssignments",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "WeeklistTaskAssignments",
                keyColumn: "Id",
                keyValue: 1,
                column: "ApplicationUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "WeeklistTaskAssignments",
                keyColumn: "Id",
                keyValue: 2,
                column: "ApplicationUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "WeeklistTaskAssignments",
                keyColumn: "Id",
                keyValue: 3,
                column: "ApplicationUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "WeeklistTaskAssignments",
                keyColumn: "Id",
                keyValue: 4,
                column: "ApplicationUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "WeeklistTaskAssignments",
                keyColumn: "Id",
                keyValue: 5,
                column: "ApplicationUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "WeeklistTaskAssignments",
                keyColumn: "Id",
                keyValue: 6,
                column: "ApplicationUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "WeeklistTaskAssignments",
                keyColumn: "Id",
                keyValue: 7,
                column: "ApplicationUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "WeeklistTaskAssignments",
                keyColumn: "Id",
                keyValue: 8,
                column: "ApplicationUserId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_WeeklistTaskAssignments_ApplicationUserId",
                table: "WeeklistTaskAssignments",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WeeklistTaskAssignments_AspNetUsers_ApplicationUserId",
                table: "WeeklistTaskAssignments",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
