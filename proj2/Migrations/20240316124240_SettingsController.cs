using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace proj2.Migrations
{
    /// <inheritdoc />
    public partial class SettingsController : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Late",
                table: "Settings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Plus",
                table: "Settings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Late",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "Plus",
                table: "Settings");
        }
    }
}
