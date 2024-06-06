using Microsoft.EntityFrameworkCore.Migrations;

namespace MiddleAges.Migrations
{
    public partial class LandTaxes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Taxes",
                table: "Lands",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Taxes",
                table: "Lands");
        }
    }
}
