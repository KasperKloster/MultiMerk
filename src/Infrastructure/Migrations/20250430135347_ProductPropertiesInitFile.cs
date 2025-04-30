using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ProductPropertiesInitFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CategoryId",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Cost",
                table: "Products",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EAN",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MainImage",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Material",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Products",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Qty",
                table: "Products",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Series",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupplierSku",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Weight",
                table: "Products",
                type: "real",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CategoryId", "Color", "Cost", "Description", "EAN", "MainImage", "Material", "Price", "Qty", "Series", "SupplierSku", "Title", "Weight" },
                values: new object[] { null, null, null, null, null, null, null, null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CategoryId", "Color", "Cost", "Description", "EAN", "MainImage", "Material", "Price", "Qty", "Series", "SupplierSku", "Title", "Weight" },
                values: new object[] { null, null, null, null, null, null, null, null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CategoryId", "Color", "Cost", "Description", "EAN", "MainImage", "Material", "Price", "Qty", "Series", "SupplierSku", "Title", "Weight" },
                values: new object[] { null, null, null, null, null, null, null, null, null, null, null, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Cost",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "EAN",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MainImage",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Material",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Qty",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Series",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SupplierSku",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Products");
        }
    }
}
