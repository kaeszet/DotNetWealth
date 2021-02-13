using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetWMS.Migrations
{
    public partial class DocAssignmentModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Doc_Assignments",
                columns: table => new
                {
                    DocumentId = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    UserFrom = table.Column<string>(nullable: true),
                    UserTo = table.Column<string>(nullable: true),
                    WarehouseFrom = table.Column<int>(nullable: true),
                    WarehouseTo = table.Column<int>(nullable: true),
                    ExternalFrom = table.Column<int>(nullable: true),
                    ExternalTo = table.Column<int>(nullable: true),
                    IsConfirmed = table.Column<bool>(nullable: false),
                    ConfirmationDate = table.Column<DateTime>(nullable: true),
                    ItemsToString = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doc_Assignments", x => x.DocumentId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Doc_Assignments");
        }
    }
}
