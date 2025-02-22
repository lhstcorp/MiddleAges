using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MiddleAges.Migrations
{
    public partial class WarLog_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WarLogs",
                columns: table => new
                {
                    WarLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WarId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttackEfficiency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DefenceEfficiency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttackLosses = table.Column<int>(type: "int", nullable: false),
                    DefenceLosses = table.Column<int>(type: "int", nullable: false),
                    CalculationTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarLogs", x => x.WarLogId);
                    table.ForeignKey(
                        name: "FK_WarLogs_Wars_WarId",
                        column: x => x.WarId,
                        principalTable: "Wars",
                        principalColumn: "WarId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WarLogs_WarId",
                table: "WarLogs",
                column: "WarId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WarLogs");
        }
    }
}
