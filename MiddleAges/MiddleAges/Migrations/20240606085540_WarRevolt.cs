using Microsoft.EntityFrameworkCore.Migrations;

namespace MiddleAges.Migrations
{
    public partial class WarRevolt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRevolt",
                table: "Wars",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RebelId",
                table: "Wars",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wars_RebelId",
                table: "Wars",
                column: "RebelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wars_AspNetUsers_RebelId",
                table: "Wars",
                column: "RebelId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wars_AspNetUsers_RebelId",
                table: "Wars");

            migrationBuilder.DropIndex(
                name: "IX_Wars_RebelId",
                table: "Wars");

            migrationBuilder.DropColumn(
                name: "IsRevolt",
                table: "Wars");

            migrationBuilder.DropColumn(
                name: "RebelId",
                table: "Wars");
        }
    }
}
