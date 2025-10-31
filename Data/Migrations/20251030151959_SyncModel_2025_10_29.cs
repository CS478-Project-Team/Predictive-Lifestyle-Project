using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Predictive_Lifestyle_Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class SyncModel_2025_10_29 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HealthEntries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    SexAtBirth = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HeightIn = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WeightLbs = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DailyCal = table.Column<int>(type: "int", nullable: true),
                    DiastolicBp = table.Column<int>(type: "int", nullable: true),
                    RestingHr = table.Column<int>(type: "int", nullable: true),
                    StepsPerDay = table.Column<int>(type: "int", nullable: true),
                    SleepHours = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Hdl = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Ldl = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Triglycerides = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    A1c = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Predictions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HealthEntryId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModelName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Score = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DetailsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Predictions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Predictions_HealthEntries_HealthEntryId",
                        column: x => x.HealthEntryId,
                        principalTable: "HealthEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Predictions_HealthEntryId",
                table: "Predictions",
                column: "HealthEntryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Predictions");

            migrationBuilder.DropTable(
                name: "HealthEntries");
        }
    }
}
