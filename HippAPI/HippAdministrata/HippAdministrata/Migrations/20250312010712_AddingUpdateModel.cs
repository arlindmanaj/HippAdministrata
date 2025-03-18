using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HippAdministrata.Migrations
{
    /// <inheritdoc />
    public partial class AddingUpdateModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NewDeliveryDestination",
                table: "OrderRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NewProductId",
                table: "OrderRequests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NewQuantity",
                table: "OrderRequests",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewDeliveryDestination",
                table: "OrderRequests");

            migrationBuilder.DropColumn(
                name: "NewProductId",
                table: "OrderRequests");

            migrationBuilder.DropColumn(
                name: "NewQuantity",
                table: "OrderRequests");
        }
    }
}
