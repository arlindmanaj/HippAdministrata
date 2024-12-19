using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HippAdministrata.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderProductEmployeeDriverLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeeId1",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SalesPersonId1",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderStatus",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EmployeeProductLabel",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    LabeledQuantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeProductLabel", x => new { x.EmployeeId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_EmployeeProductLabel_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeProductLabel_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderProduct",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderProduct", x => new { x.OrderId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_OrderProduct_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderProduct_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_EmployeeId1",
                table: "Orders",
                column: "EmployeeId1");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_SalesPersonId1",
                table: "Orders",
                column: "SalesPersonId1");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeProductLabel_ProductId",
                table: "EmployeeProductLabel",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProduct_ProductId",
                table: "OrderProduct",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Employees_EmployeeId1",
                table: "Orders",
                column: "EmployeeId1",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_SalesPersons_SalesPersonId1",
                table: "Orders",
                column: "SalesPersonId1",
                principalTable: "SalesPersons",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Employees_EmployeeId1",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_SalesPersons_SalesPersonId1",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "EmployeeProductLabel");

            migrationBuilder.DropTable(
                name: "OrderProduct");

            migrationBuilder.DropIndex(
                name: "IX_Orders_EmployeeId1",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_SalesPersonId1",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "EmployeeId1",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "SalesPersonId1",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderStatus",
                table: "Drivers");
        }
    }
}
