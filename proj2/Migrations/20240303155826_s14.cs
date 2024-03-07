using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace proj2.Migrations
{
    /// <inheritdoc />
    public partial class s14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "EmployeesAttndens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "name",
                table: "EmployeesAttndens");
        }
    }
}
