using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HippAdministrata.Migrations
{
    /// <inheritdoc />
    public partial class ConfiguringSalesPersonClientTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "SalesPersonClients",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeliveryDestination",
                table: "SalesPersonClients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "SalesPersonClients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "SalesPersonClients",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "SalesPersonClients");

            migrationBuilder.DropColumn(
                name: "DeliveryDestination",
                table: "SalesPersonClients");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "SalesPersonClients");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "SalesPersonClients");
        }
    }
}
