using Microsoft.EntityFrameworkCore.Migrations;

namespace MiddleAges.Migrations
{
    public partial class UnitBuildingLandLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LandId",
                table: "Units",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LandId",
                table: "Buildings",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Units_LandId",
                table: "Units",
                column: "LandId");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_LandId",
                table: "Buildings",
                column: "LandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Buildings_Lands_LandId",
                table: "Buildings",
                column: "LandId",
                principalTable: "Lands",
                principalColumn: "LandId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Lands_LandId",
                table: "Units",
                column: "LandId",
                principalTable: "Lands",
                principalColumn: "LandId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buildings_Lands_LandId",
                table: "Buildings");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_Lands_LandId",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Units_LandId",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Buildings_LandId",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "LandId",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "LandId",
                table: "Buildings");
        }
    }
}
