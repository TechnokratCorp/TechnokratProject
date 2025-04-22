using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TehnokratProject.Migrations
{
    /// <inheritdoc />
    public partial class added_quanitity_to_product : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "quantity",
                table: "products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "quantity",
                table: "products");
        }
    }
}
