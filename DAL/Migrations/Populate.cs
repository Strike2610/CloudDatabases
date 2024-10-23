using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations {
    /// <inheritdoc />
    public partial class Populate : Migration {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.InsertData(
                table: "Customers",
                columns: ["Id", "Name", "Address"],
                values: new object[,] {
            { 1, "Anakin Skywalker", "Jedi Temple" },
            { 2, "Harry Potter", "Privet Drive 4" },
            { 3, "Bilbo Baggins", "Bag End" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: ["Id", "Name", "Price", "Thumbnail"],
                values: new object[,] {
            { 1, "Lightsaber", 200m, "bf4084e2-c63d-4a26-a1f8-29585d107b33.jpg" },
            { 2, "Wand", 5m, "b2ac5f77-a354-47f0-a69d-69a01c268385.jpg" },
            { 3, "Ring", 999m, "236eb394-4170-4e3a-ad68-20bd9c592ddf.jpg" }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: ["Id", "CustomerId", "ProductId", "OrderDate", "ShipDate", "OrderProcessed"],
                values: new object[,] {
                    { 1, 1, 3, DateTimeOffset.Now.Subtract(TimeSpan.FromDays(16)), null, null},
                    { 2, 2, 1, DateTimeOffset.Now.Subtract(TimeSpan.FromDays(10)), null, null},
                    { 3, 3, 2, DateTimeOffset.Now.Subtract(TimeSpan.FromDays(66)), null, null},
                    { 4, 1, 1, DateTimeOffset.Now.Subtract(TimeSpan.FromDays(51)), DateTimeOffset.Now.Subtract(TimeSpan.FromDays(23)), TimeSpan.FromDays(28) },
                    { 5, 2, 2, DateTimeOffset.Now.Subtract(TimeSpan.FromDays(92)), DateTimeOffset.Now.Subtract(TimeSpan.FromDays(47)), TimeSpan.FromDays(45) },
                    { 6, 3, 3, DateTimeOffset.Now.Subtract(TimeSpan.FromDays(94)), DateTimeOffset.Now.Subtract(TimeSpan.FromDays(58)), TimeSpan.FromDays(36) }
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: ["Id", "ProductId", "PostDate", "Content"],
                values: new object[,] {
                    { 1, 1, DateTimeOffset.Now.Subtract(TimeSpan.FromDays(42)), "Works great against younglings!"},
                    { 2, 2, DateTimeOffset.Now.Subtract(TimeSpan.FromDays(17)), "What a rip off, I expected the elder wand."},
                    { 3, 3, DateTimeOffset.Now.Subtract(TimeSpan.FromDays(89)), "My precious..."}
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DeleteData("Orders", "Id", [1, 2, 3, 4, 5, 6]);
            migrationBuilder.DeleteData("Customers", "Id", [1, 2, 3]);
            migrationBuilder.DeleteData("Products", "Id", [1, 2, 3]);
            migrationBuilder.DeleteData("Comments", "Id", [1, 2, 3]);
        }
    }
}
