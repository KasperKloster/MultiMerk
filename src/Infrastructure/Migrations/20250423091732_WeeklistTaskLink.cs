using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class WeeklistTaskLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeeklistTaskLinks",
                columns: table => new
                {
                    WeeklistId = table.Column<int>(type: "integer", nullable: false),
                    WeeklistTaskId = table.Column<int>(type: "integer", nullable: false),
                    WeeklistTaskStatusId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeklistTaskLinks", x => new { x.WeeklistId, x.WeeklistTaskId });
                    table.ForeignKey(
                        name: "FK_WeeklistTaskLinks_WeeklistTaskStatus_WeeklistTaskStatusId",
                        column: x => x.WeeklistTaskStatusId,
                        principalTable: "WeeklistTaskStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WeeklistTaskLinks_WeeklistTasks_WeeklistTaskId",
                        column: x => x.WeeklistTaskId,
                        principalTable: "WeeklistTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WeeklistTaskLinks_Weeklists_WeeklistId",
                        column: x => x.WeeklistId,
                        principalTable: "Weeklists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "WeeklistTaskStatus",
                keyColumn: "Id",
                keyValue: 2,
                column: "Status",
                value: "Ready");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklistTaskLinks_WeeklistTaskId",
                table: "WeeklistTaskLinks",
                column: "WeeklistTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklistTaskLinks_WeeklistTaskStatusId",
                table: "WeeklistTaskLinks",
                column: "WeeklistTaskStatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeeklistTaskLinks");

            migrationBuilder.UpdateData(
                table: "WeeklistTaskStatus",
                keyColumn: "Id",
                keyValue: 2,
                column: "Status",
                value: "Not started");
        }
    }
}
