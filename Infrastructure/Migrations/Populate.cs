#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations {
    /// <inheritdoc />
    public partial class Populate : Migration {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) {
            var customer1 = Guid.NewGuid();
            var customer2 = Guid.NewGuid();
            var customer3 = Guid.NewGuid();
            var product1 = Guid.NewGuid();
            var product2 = Guid.NewGuid();
            var product3 = Guid.NewGuid();
            migrationBuilder.InsertData(
                table: "Customers",
                columns: ["Id", "Name", "Address"],
                values: new object[,] {
            { customer1, "Anakin Skywalker", "Jedi Temple" },
            { customer2, "Harry Potter", "Privet Drive 4" },
            { customer3, "Bilbo Baggins", "Bag End" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: ["Id", "Name", "Price", "Thumbnail"],
                values: new object[,] {
            { product1, "Lightsaber", 200m, "bf4084e2-c63d-4a26-a1f8-29585d107b33.jpg" },
            { product2, "Wand", 5m, "b2ac5f77-a354-47f0-a69d-69a01c268385.jpg" },
            { product3, "Ring", 999m, "236eb394-4170-4e3a-ad68-20bd9c592ddf.jpg" }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: ["Id", "CustomerId", "ProductId", "OrderDate", "ShipDate", "OrderProcessed"],
                values: new object[,] {
                    { Guid.NewGuid(), customer1, product3, DateTimeOffset.Now.Subtract(TimeSpan.FromDays(16)), null, null},
                    { Guid.NewGuid(), customer2, product1, DateTimeOffset.Now.Subtract(TimeSpan.FromDays(10)), null, null},
                    { Guid.NewGuid(), customer3, product2, DateTimeOffset.Now.Subtract(TimeSpan.FromDays(66)), null, null},
                    { Guid.NewGuid(), customer1, product1, DateTimeOffset.Now.Subtract(TimeSpan.FromDays(51)), DateTimeOffset.Now.Subtract(TimeSpan.FromDays(23)), TimeSpan.FromDays(28) },
                    { Guid.NewGuid(), customer2, product2, DateTimeOffset.Now.Subtract(TimeSpan.FromDays(92)), DateTimeOffset.Now.Subtract(TimeSpan.FromDays(47)), TimeSpan.FromDays(45) },
                    { Guid.NewGuid(), customer3, product3, DateTimeOffset.Now.Subtract(TimeSpan.FromDays(94)), DateTimeOffset.Now.Subtract(TimeSpan.FromDays(58)), TimeSpan.FromDays(36) }
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: ["Id", "ProductId", "PostDate", "Content"],
                values: new object[,] {
                    { Guid.NewGuid(), product1, DateTimeOffset.Now.Subtract(TimeSpan.FromDays(42)), "Works great against younglings!"},
                    { Guid.NewGuid(), product2, DateTimeOffset.Now.Subtract(TimeSpan.FromDays(17)), "What a rip off, I expected the elder wand."},
                    { Guid.NewGuid(), product3, DateTimeOffset.Now.Subtract(TimeSpan.FromDays(89)), "My precious..."}
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