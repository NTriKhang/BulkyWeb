using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bulky.Migrations
{
    /// <inheritdoc />
    public partial class addCountToShoppingCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "shoppingCartsFixed",
                type: "int",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
