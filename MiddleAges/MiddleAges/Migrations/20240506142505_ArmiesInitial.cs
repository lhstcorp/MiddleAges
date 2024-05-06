using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MiddleAges.Migrations
{
    public partial class ArmiesInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Armies",
                columns: table => new
                {
                    ArmyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlayerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SoldiersCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Armies", x => x.ArmyId);
                    table.ForeignKey(
                        name: "FK_Armies_AspNetUsers_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArmiesInBattle",
                columns: table => new
                {
                    ArmyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlayerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    WarId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LandId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SoldiersCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArmiesInBattle", x => x.ArmyId);
                    table.ForeignKey(
                        name: "FK_ArmiesInBattle_AspNetUsers_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArmiesInBattle_Lands_LandId",
                        column: x => x.LandId,
                        principalTable: "Lands",
                        principalColumn: "LandId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArmiesInBattle_Wars_WarId",
                        column: x => x.WarId,
                        principalTable: "Wars",
                        principalColumn: "WarId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Armies_PlayerId",
                table: "Armies",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_ArmiesInBattle_LandId",
                table: "ArmiesInBattle",
                column: "LandId");

            migrationBuilder.CreateIndex(
                name: "IX_ArmiesInBattle_PlayerId",
                table: "ArmiesInBattle",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_ArmiesInBattle_WarId",
                table: "ArmiesInBattle",
                column: "WarId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Armies");

            migrationBuilder.DropTable(
                name: "ArmiesInBattle");
        }
    }
}
