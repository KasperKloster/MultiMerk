using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ShippingNumberToWeeklist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShippingNumber",
                table: "Weeklists",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Weeklists",
                keyColumn: "Id",
                keyValue: 1,
                column: "ShippingNumber",
                value: "");

            migrationBuilder.UpdateData(
                table: "Weeklists",
                keyColumn: "Id",
                keyValue: 2,
                column: "ShippingNumber",
                value: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShippingNumber",
                table: "Weeklists");
        }
    }
}
