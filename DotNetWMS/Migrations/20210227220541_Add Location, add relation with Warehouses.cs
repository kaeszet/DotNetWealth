using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetWMS.Migrations
{
    public partial class AddLocationaddrelationwithWarehouses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Warehouses",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Longitude = table.Column<string>(nullable: true),
                    Latitude = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_LocationId",
                table: "Warehouses",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouses_Locations_LocationId",
                table: "Warehouses",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Warehouses_Locations_LocationId",
                table: "Warehouses");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Warehouses_LocationId",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Warehouses");
        }
    }
}
