using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IdentityService.Migrations
{
    /// <inheritdoc />
    public partial class AddRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1b1cca47-82c2-4cb6-8952-4f4484fbe9c5", null, "Buyer", "BUYER" },
                    { "3ba4db97-a9b1-4f3e-a5b8-1af0c4eb63e9", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1b1cca47-82c2-4cb6-8952-4f4484fbe9c5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3ba4db97-a9b1-4f3e-a5b8-1af0c4eb63e9");
        }
    }
}
