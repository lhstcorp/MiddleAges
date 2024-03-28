using Microsoft.EntityFrameworkCore.Migrations;

namespace MiddleAges.Migrations
{
    public partial class LawsUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Law_AspNetUsers_PlayerId",
                table: "Law");

            migrationBuilder.DropForeignKey(
                name: "FK_Law_Countries_CountryId",
                table: "Law");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Law",
                table: "Law");

            migrationBuilder.RenameTable(
                name: "Law",
                newName: "Laws");

            migrationBuilder.RenameIndex(
                name: "IX_Law_PlayerId",
                table: "Laws",
                newName: "IX_Laws_PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_Law_CountryId",
                table: "Laws",
                newName: "IX_Laws_CountryId");

            migrationBuilder.AddColumn<string>(
                name: "Value1",
                table: "Laws",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value2",
                table: "Laws",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Laws",
                table: "Laws",
                column: "LawId");

            migrationBuilder.AddForeignKey(
                name: "FK_Laws_AspNetUsers_PlayerId",
                table: "Laws",
                column: "PlayerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Laws_Countries_CountryId",
                table: "Laws",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Laws_AspNetUsers_PlayerId",
                table: "Laws");

            migrationBuilder.DropForeignKey(
                name: "FK_Laws_Countries_CountryId",
                table: "Laws");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Laws",
                table: "Laws");

            migrationBuilder.DropColumn(
                name: "Value1",
                table: "Laws");

            migrationBuilder.DropColumn(
                name: "Value2",
                table: "Laws");

            migrationBuilder.RenameTable(
                name: "Laws",
                newName: "Law");

            migrationBuilder.RenameIndex(
                name: "IX_Laws_PlayerId",
                table: "Law",
                newName: "IX_Law_PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_Laws_CountryId",
                table: "Law",
                newName: "IX_Law_CountryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Law",
                table: "Law",
                column: "LawId");

            migrationBuilder.AddForeignKey(
                name: "FK_Law_AspNetUsers_PlayerId",
                table: "Law",
                column: "PlayerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Law_Countries_CountryId",
                table: "Law",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
