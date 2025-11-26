using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Predictive_Lifestyle_Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixPredictionScoreType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Score",
                table: "Predictions",
                type: "decimal(18,4)",   // or decimal(5,4) if you only need 0..1
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
