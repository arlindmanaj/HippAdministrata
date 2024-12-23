using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HippAdministrata.Migrations
{
    /// <inheritdoc />
    public partial class ChangingModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orderHistories_Employees_UpdatedByEmployeeId",
                table: "orderHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_orderHistories_Orders_OrderId",
                table: "orderHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_orderHistories_OrderHistoryId1",
                table: "Orders");

           

            migrationBuilder.DropIndex(
                name: "IX_Orders_OrderHistoryId1",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_orderHistories",
                table: "orderHistories");

           
           
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderHistoryId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderHistoryId1",
                table: "Orders");

            migrationBuilder.RenameTable(
                name: "orderHistories",
                newName: "OrderHistory");

            migrationBuilder.RenameIndex(
                name: "IX_orderHistories_UpdatedByEmployeeId",
                table: "OrderHistory",
                newName: "IX_OrderHistory_UpdatedByEmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_orderHistories_OrderId",
                table: "OrderHistory",
                newName: "IX_OrderHistory_OrderId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdated",
                table: "Orders",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DriverId",
                table: "Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderHistory",
                table: "OrderHistory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHistory_Employees_UpdatedByEmployeeId",
                table: "OrderHistory",
                column: "UpdatedByEmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHistory_Orders_OrderId",
                table: "OrderHistory",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderHistory_Employees_UpdatedByEmployeeId",
                table: "OrderHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderHistory_Orders_OrderId",
                table: "OrderHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderHistory",
                table: "OrderHistory");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Orders");

            migrationBuilder.RenameTable(
                name: "OrderHistory",
                newName: "orderHistories");

            migrationBuilder.RenameIndex(
                name: "IX_OrderHistory_UpdatedByEmployeeId",
                table: "orderHistories",
                newName: "IX_orderHistories_UpdatedByEmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderHistory_OrderId",
                table: "orderHistories",
                newName: "IX_orderHistories_OrderId");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalQuantity",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdated",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DriverId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderHistoryId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrderHistoryId1",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_orderHistories",
                table: "orderHistories",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "SalesPersonClients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    SalesPersonId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesPersonClients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesPersonClients_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesPersonClients_SalesPersons_SalesPersonId",
                        column: x => x.SalesPersonId,
                        principalTable: "SalesPersons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderHistoryId1",
                table: "Orders",
                column: "OrderHistoryId1");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPersonClients_ClientId",
                table: "SalesPersonClients",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPersonClients_SalesPersonId",
                table: "SalesPersonClients",
                column: "SalesPersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_orderHistories_Employees_UpdatedByEmployeeId",
                table: "orderHistories",
                column: "UpdatedByEmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_orderHistories_Orders_OrderId",
                table: "orderHistories",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_orderHistories_OrderHistoryId1",
                table: "Orders",
                column: "OrderHistoryId1",
                principalTable: "orderHistories",
                principalColumn: "Id");
        }
    }
}
