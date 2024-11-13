using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MiddleAges.Migrations
{
    public partial class LandDevelopmentShareInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LandDevelopmentShares",
                columns: table => new
                {
                    LandDevelopmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LandId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    InfrastructureShare = table.Column<double>(type: "float", nullable: false),
                    MarketShare = table.Column<double>(type: "float", nullable: false),
                    FortificationShare = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandDevelopmentShares", x => x.LandDevelopmentId);
                    table.ForeignKey(
                        name: "FK_LandDevelopmentShares_Lands_LandId",
                        column: x => x.LandId,
                        principalTable: "Lands",
                        principalColumn: "LandId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LandDevelopmentShares_LandId",
                table: "LandDevelopmentShares",
                column: "LandId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LandDevelopmentShares");
        }
    }
}
