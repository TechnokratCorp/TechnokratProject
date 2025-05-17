using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TehnokratProject.Migrations
{
    /// <inheritdoc />
    public partial class added_state_and_brands_to_product : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "brand",
                table: "products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "state",
                table: "products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "brand",
                table: "products");

            migrationBuilder.DropColumn(
                name: "state",
                table: "products");
        }
    }
}
