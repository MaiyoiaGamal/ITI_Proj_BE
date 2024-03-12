using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace proj2.Migrations
{
    /// <inheritdoc />
    public partial class removeemployeesettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Settings_SettingId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_SettingId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "SettingId",
                table: "Employees");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SettingId",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_SettingId",
                table: "Employees",
                column: "SettingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Settings_SettingId",
                table: "Employees",
                column: "SettingId",
                principalTable: "Settings",
                principalColumn: "Id");
        }
    }
}
