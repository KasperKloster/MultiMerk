using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserToWeeklistTaskLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssignedUserId",
                table: "WeeklistTaskLinks",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WeeklistTaskLinks_AssignedUserId",
                table: "WeeklistTaskLinks",
                column: "AssignedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WeeklistTaskLinks_AspNetUsers_AssignedUserId",
                table: "WeeklistTaskLinks",
                column: "AssignedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeeklistTaskLinks_AspNetUsers_AssignedUserId",
                table: "WeeklistTaskLinks");

            migrationBuilder.DropIndex(
                name: "IX_WeeklistTaskLinks_AssignedUserId",
                table: "WeeklistTaskLinks");

            migrationBuilder.DropColumn(
                name: "AssignedUserId",
                table: "WeeklistTaskLinks");
        }
    }
}
