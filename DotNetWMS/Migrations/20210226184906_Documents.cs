using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetWMS.Migrations
{
    public partial class Documents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DocumentId",
                table: "Infoboxes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentId",
                table: "Infoboxes");
        }
    }
}
