using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWeeklistProductRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WeeklistId",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Products_WeeklistId",
                table: "Products",
                column: "WeeklistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Weeklists_WeeklistId",
                table: "Products",
                column: "WeeklistId",
                principalTable: "Weeklists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Weeklists_WeeklistId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_WeeklistId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "WeeklistId",
                table: "Products");
        }
    }
}
