using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace proj2.Migrations
{
    /// <inheritdoc />
    public partial class employeesettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Employees",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);
        }
    }
}
