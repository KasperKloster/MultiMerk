using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class WeeklistTasksAndStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeeklistTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeklistTasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeeklistTaskStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeklistTaskStatus", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "WeeklistTaskStatus",
                columns: new[] { "Id", "Status" },
                values: new object[,]
                {
                    { 1, "Awaiting" },
                    { 2, "Not started" },
                    { 3, "In Progress" },
                    { 4, "Done" }
                });

            migrationBuilder.InsertData(
                table: "WeeklistTasks",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Give EAN" },
                    { 2, "Prepare for AI" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeeklistTasks");

            migrationBuilder.DropTable(
                name: "WeeklistTaskStatus");
        }
    }
}
