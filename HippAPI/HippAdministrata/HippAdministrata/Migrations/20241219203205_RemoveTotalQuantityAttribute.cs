using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HippAdministrata.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTotalQuantityAttribute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalQuantity",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalQuantity",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
