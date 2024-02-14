using Microsoft.EntityFrameworkCore.Migrations;

namespace MiddleAges.Migrations
{
    public partial class BorderLandsKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BorderLands_Lands_LandId",
                table: "BorderLands");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BorderLands",
                table: "BorderLands");

            migrationBuilder.DropIndex(
                name: "IX_BorderLands_LandId",
                table: "BorderLands");

            migrationBuilder.AlterColumn<string>(
                name: "LandId",
                table: "BorderLands",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BorderLands",
                table: "BorderLands",
                columns: new[] { "LandId", "BorderLandId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BorderLands_Lands_LandId",
                table: "BorderLands",
                column: "LandId",
                principalTable: "Lands",
                principalColumn: "LandId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BorderLands_Lands_LandId",
                table: "BorderLands");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BorderLands",
                table: "BorderLands");

            migrationBuilder.AlterColumn<string>(
                name: "LandId",
                table: "BorderLands",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BorderLands",
                table: "BorderLands",
                column: "BorderLandId");

            migrationBuilder.CreateIndex(
                name: "IX_BorderLands_LandId",
                table: "BorderLands",
                column: "LandId");

            migrationBuilder.AddForeignKey(
                name: "FK_BorderLands_Lands_LandId",
                table: "BorderLands",
                column: "LandId",
                principalTable: "Lands",
                principalColumn: "LandId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
