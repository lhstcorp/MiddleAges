using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MiddleAges.Migrations
{
    public partial class PlayerInformation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlayerInformations",
                columns: table => new
                {
                    PlayerInformationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlayerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Vk = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telegram = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discord = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Facebook = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerInformations", x => x.PlayerInformationId);
                    table.ForeignKey(
                        name: "FK_PlayerInformations_AspNetUsers_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerInformations_PlayerId",
                table: "PlayerInformations",
                column: "PlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerInformations");
        }
    }
}
