using Microsoft.EntityFrameworkCore.Migrations;

namespace MiddleAges.Migrations
{
    public partial class LandBuildingKeyChangesV3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LandId",
                table: "LandBuildings",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LandBuildings_LandId",
                table: "LandBuildings",
                column: "LandId");

            migrationBuilder.AddForeignKey(
                name: "FK_LandBuildings_Lands_LandId",
                table: "LandBuildings",
                column: "LandId",
                principalTable: "Lands",
                principalColumn: "LandId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LandBuildings_Lands_LandId",
                table: "LandBuildings");

            migrationBuilder.DropIndex(
                name: "IX_LandBuildings_LandId",
                table: "LandBuildings");

            migrationBuilder.AlterColumn<string>(
                name: "LandId",
                table: "LandBuildings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
