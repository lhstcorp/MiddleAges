using Microsoft.EntityFrameworkCore.Migrations;

namespace MiddleAges.Data.Migrations
{
    public partial class Unit_column_fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SoldiersCount",
                table: "units",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "PeasantsCount",
                table: "units",
                newName: "Lvl");

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "units",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "units");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "units",
                newName: "SoldiersCount");

            migrationBuilder.RenameColumn(
                name: "Lvl",
                table: "units",
                newName: "PeasantsCount");
        }
    }
}
