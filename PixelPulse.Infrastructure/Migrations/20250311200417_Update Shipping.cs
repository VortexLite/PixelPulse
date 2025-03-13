using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PixelPulse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateShipping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shipping_Address_AddressId",
                table: "Shipping");

            migrationBuilder.AlterColumn<int>(
                name: "AddressId",
                table: "Shipping",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Shipping",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "Shipping",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Shipping_Address_AddressId",
                table: "Shipping",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shipping_Address_AddressId",
                table: "Shipping");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Shipping");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "Shipping");

            migrationBuilder.AlterColumn<int>(
                name: "AddressId",
                table: "Shipping",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Shipping_Address_AddressId",
                table: "Shipping",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
