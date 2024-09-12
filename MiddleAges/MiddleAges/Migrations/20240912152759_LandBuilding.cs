using Microsoft.EntityFrameworkCore.Migrations;

namespace MiddleAges.Migrations
{
    public partial class LandBuilding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LandBuildings",
                columns: table => new
                {
                    BuildingId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LandId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    BuildingType = table.Column<int>(type: "int", nullable: false),
                    Lvl = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandBuildings", x => x.BuildingId);
                    table.ForeignKey(
                        name: "FK_LandBuildings_Lands_LandId",
                        column: x => x.LandId,
                        principalTable: "Lands",
                        principalColumn: "LandId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LandBuildings_LandId",
                table: "LandBuildings",
                column: "LandId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LandBuildings");
        }
    }
}
