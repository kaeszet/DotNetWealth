using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetWMS.Migrations
{
    public partial class extendDoc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalFromName",
                table: "Doc_Assignments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExternalToName",
                table: "Doc_Assignments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserFromName",
                table: "Doc_Assignments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserToName",
                table: "Doc_Assignments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WarehouseFromName",
                table: "Doc_Assignments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WarehouseToName",
                table: "Doc_Assignments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalFromName",
                table: "Doc_Assignments");

            migrationBuilder.DropColumn(
                name: "ExternalToName",
                table: "Doc_Assignments");

            migrationBuilder.DropColumn(
                name: "UserFromName",
                table: "Doc_Assignments");

            migrationBuilder.DropColumn(
                name: "UserToName",
                table: "Doc_Assignments");

            migrationBuilder.DropColumn(
                name: "WarehouseFromName",
                table: "Doc_Assignments");

            migrationBuilder.DropColumn(
                name: "WarehouseToName",
                table: "Doc_Assignments");
        }
    }
}
