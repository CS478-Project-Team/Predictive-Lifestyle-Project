using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Predictive_Lifestyle_Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLifestyleColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "A1c",
                table: "HealthEntries");

            migrationBuilder.DropColumn(
                name: "DiastolicBp",
                table: "HealthEntries");

            migrationBuilder.DropColumn(
                name: "Hdl",
                table: "HealthEntries");

            migrationBuilder.DropColumn(
                name: "Ldl",
                table: "HealthEntries");

            migrationBuilder.RenameColumn(
                name: "Triglycerides",
                table: "HealthEntries",
                newName: "AlcoholicDrinksPerWeek");

            migrationBuilder.AddColumn<string>(
                name: "SmokeOrVape",
                table: "HealthEntries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SmokeOrVape",
                table: "HealthEntries");

            migrationBuilder.RenameColumn(
                name: "AlcoholicDrinksPerWeek",
                table: "HealthEntries",
                newName: "Triglycerides");

            migrationBuilder.AddColumn<decimal>(
                name: "A1c",
                table: "HealthEntries",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DiastolicBp",
                table: "HealthEntries",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Hdl",
                table: "HealthEntries",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Ldl",
                table: "HealthEntries",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
