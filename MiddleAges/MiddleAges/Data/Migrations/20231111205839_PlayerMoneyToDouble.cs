using Microsoft.EntityFrameworkCore.Migrations;

namespace MiddleAges.Data.Migrations
{
    public partial class PlayerMoneyToDouble : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Money",
                table: "players",
                type: "float",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "Money",
                table: "players",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
