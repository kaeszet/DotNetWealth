using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetWMS.Migrations
{
    public partial class AddressInLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Items_Employees_EmployeeId",
            //    table: "Items");

            //migrationBuilder.DropTable(
            //    name: "Employees");

            //migrationBuilder.DropIndex(
            //    name: "IX_Items_EmployeeId",
            //    table: "Items");

            //migrationBuilder.DropColumn(
            //    name: "EmployeeId",
            //    table: "Items");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Locations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Locations");

            //migrationBuilder.AddColumn<int>(
            //    name: "EmployeeId",
            //    table: "Items",
            //    type: "int",
            //    nullable: true);

            //migrationBuilder.CreateTable(
            //    name: "Employees",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        City = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
            //        DepartmentId = table.Column<int>(type: "int", nullable: true),
            //        Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
            //        Pesel = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
            //        Street = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
            //        Surname = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
            //        ZipCode = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Employees", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Employees_Departments_DepartmentId",
            //            column: x => x.DepartmentId,
            //            principalTable: "Departments",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Items_EmployeeId",
            //    table: "Items",
            //    column: "EmployeeId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Employees_DepartmentId",
            //    table: "Employees",
            //    column: "DepartmentId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Items_Employees_EmployeeId",
            //    table: "Items",
            //    column: "EmployeeId",
            //    principalTable: "Employees",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }
    }
}
