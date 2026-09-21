using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PracticalHiring.Migrations
{
    /// <inheritdoc />
    public partial class SeedEmployees : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "department",
                table: "Employees",
                newName: "Department");

            migrationBuilder.RenameColumn(
                name: "code",
                table: "Employees",
                newName: "Code");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Department",
                table: "Employees",
                newName: "department");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Employees",
                newName: "code");
        }
    }
}
