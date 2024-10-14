using Microsoft.EntityFrameworkCore.Migrations;

namespace MiddleAges.Migrations
{
    public partial class LandGovernor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GovernorId",
                table: "Lands",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lands_GovernorId",
                table: "Lands",
                column: "GovernorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lands_AspNetUsers_GovernorId",
                table: "Lands",
                column: "GovernorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lands_AspNetUsers_GovernorId",
                table: "Lands");

            migrationBuilder.DropIndex(
                name: "IX_Lands_GovernorId",
                table: "Lands");

            migrationBuilder.DropColumn(
                name: "GovernorId",
                table: "Lands");
        }
    }
}
