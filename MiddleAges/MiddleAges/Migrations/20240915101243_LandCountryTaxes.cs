using Microsoft.EntityFrameworkCore.Migrations;

namespace MiddleAges.Migrations
{
    public partial class LandCountryTaxes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Taxes",
                table: "Lands",
                newName: "LandTax");

            migrationBuilder.AddColumn<int>(
                name: "CountryTax",
                table: "Lands",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountryTax",
                table: "Lands");

            migrationBuilder.RenameColumn(
                name: "LandTax",
                table: "Lands",
                newName: "Taxes");
        }
    }
}
