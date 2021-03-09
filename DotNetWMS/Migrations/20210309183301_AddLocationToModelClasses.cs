using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetWMS.Migrations
{
    public partial class AddLocationToModelClasses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Externals",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Externals_LocationId",
                table: "Externals",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_LocationId",
                table: "AspNetUsers",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Locations_LocationId",
                table: "AspNetUsers",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Externals_Locations_LocationId",
                table: "Externals",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Locations_LocationId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Externals_Locations_LocationId",
                table: "Externals");

            migrationBuilder.DropIndex(
                name: "IX_Externals_LocationId",
                table: "Externals");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_LocationId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Externals");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "AspNetUsers");
        }
    }
}
