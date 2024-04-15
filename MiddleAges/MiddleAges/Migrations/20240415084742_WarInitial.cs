using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MiddleAges.Migrations
{
    public partial class WarInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Wars",
                columns: table => new
                {
                    WarId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LandIdFrom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LandIdTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsEnded = table.Column<bool>(type: "bit", nullable: false),
                    WarResult = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wars", x => x.WarId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Wars");
        }
    }
}
