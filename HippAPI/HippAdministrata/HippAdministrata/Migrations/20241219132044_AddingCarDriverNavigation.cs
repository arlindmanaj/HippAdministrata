using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HippAdministrata.Migrations
{
    /// <inheritdoc />
    public partial class AddingCarDriverNavigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DriverId1",
                table: "CarDrivers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CarDrivers_DriverId1",
                table: "CarDrivers",
                column: "DriverId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CarDrivers_Drivers_DriverId1",
                table: "CarDrivers",
                column: "DriverId1",
                principalTable: "Drivers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarDrivers_Drivers_DriverId1",
                table: "CarDrivers");

            migrationBuilder.DropIndex(
                name: "IX_CarDrivers_DriverId1",
                table: "CarDrivers");

            migrationBuilder.DropColumn(
                name: "DriverId1",
                table: "CarDrivers");
        }
    }
}
