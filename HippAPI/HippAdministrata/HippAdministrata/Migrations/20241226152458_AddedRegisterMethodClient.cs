using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HippAdministrata.Migrations
{
    /// <inheritdoc />
    public partial class AddedRegisterMethodClient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Clients");
        }
    }
}
