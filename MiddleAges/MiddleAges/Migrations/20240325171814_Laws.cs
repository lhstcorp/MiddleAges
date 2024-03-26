using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MiddleAges.Migrations
{
    public partial class Laws : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CurrentLand",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Law",
                columns: table => new
                {
                    LawId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PublishingDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Law", x => x.LawId);
                    table.ForeignKey(
                        name: "FK_Law_AspNetUsers_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Law_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CurrentLand",
                table: "AspNetUsers",
                column: "CurrentLand");

            migrationBuilder.CreateIndex(
                name: "IX_Law_CountryId",
                table: "Law",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Law_PlayerId",
                table: "Law",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Lands_CurrentLand",
                table: "AspNetUsers",
                column: "CurrentLand",
                principalTable: "Lands",
                principalColumn: "LandId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Lands_CurrentLand",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Law");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CurrentLand",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "CurrentLand",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
