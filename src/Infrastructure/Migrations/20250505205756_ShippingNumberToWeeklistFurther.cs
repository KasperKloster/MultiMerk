using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ShippingNumberToWeeklistFurther : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Weeklists",
                keyColumn: "Id",
                keyValue: 1,
                column: "ShippingNumber",
                value: "Shipment101");

            migrationBuilder.UpdateData(
                table: "Weeklists",
                keyColumn: "Id",
                keyValue: 2,
                column: "ShippingNumber",
                value: "Shipment102");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
