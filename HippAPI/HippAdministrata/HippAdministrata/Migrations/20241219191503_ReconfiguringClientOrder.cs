using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HippAdministrata.Migrations
{
    /// <inheritdoc />
    public partial class ReconfiguringClientOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientOrderRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    DeliveryDestination = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SalesPersonId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientOrderRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientOrderRequests_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientOrderRequests_SalesPersons_SalesPersonId",
                        column: x => x.SalesPersonId,
                        principalTable: "SalesPersons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientOrderRequests_ClientId",
                table: "ClientOrderRequests",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientOrderRequests_SalesPersonId",
                table: "ClientOrderRequests",
                column: "SalesPersonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientOrderRequests");
        }
    }
}
