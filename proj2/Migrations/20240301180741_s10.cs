using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace proj2.Migrations
{
    /// <inheritdoc />
    public partial class s10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeesAttndens_Employees_empID",
                table: "EmployeesAttndens");

            migrationBuilder.DropIndex(
                name: "IX_EmployeesAttndens_empID",
                table: "EmployeesAttndens");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "EmployeesAttndens",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeesAttndens_EmployeeId",
                table: "EmployeesAttndens",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeesAttndens_Employees_EmployeeId",
                table: "EmployeesAttndens",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeesAttndens_Employees_EmployeeId",
                table: "EmployeesAttndens");

            migrationBuilder.DropIndex(
                name: "IX_EmployeesAttndens_EmployeeId",
                table: "EmployeesAttndens");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "EmployeesAttndens");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeesAttndens_empID",
                table: "EmployeesAttndens",
                column: "empID");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeesAttndens_Employees_empID",
                table: "EmployeesAttndens",
                column: "empID",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
