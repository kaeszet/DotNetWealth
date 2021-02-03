using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetWMS.Migrations
{
    public partial class ItemChangeEmployeeToIdentityUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_AspNetUsers_WMSIdentityUserId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_WMSIdentityUserId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "WMSIdentityUserId",
                table: "Items");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Items",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_UserId",
                table: "Items",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_AspNetUsers_UserId",
                table: "Items",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_AspNetUsers_UserId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_UserId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Items");

            migrationBuilder.AddColumn<string>(
                name: "WMSIdentityUserId",
                table: "Items",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_WMSIdentityUserId",
                table: "Items",
                column: "WMSIdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_AspNetUsers_WMSIdentityUserId",
                table: "Items",
                column: "WMSIdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
