using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PixelPulse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Addcategoryinproductspecification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "ProductSpecifications",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "ProductSpecifications");
        }
    }
}
