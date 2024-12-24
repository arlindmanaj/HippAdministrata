using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HippAdministrata.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTotalQuantityAndImgUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalQuantity",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Products");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalQuantity",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }

    }
}
