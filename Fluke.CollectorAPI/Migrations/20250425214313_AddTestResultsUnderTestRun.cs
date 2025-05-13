using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlukeCollectorAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddTestResultsUnderTestRun : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TestResults_TestRunId",
                table: "TestResults",
                column: "TestRunId");

            migrationBuilder.AddForeignKey(
                name: "FK_TestResults_TestRuns_TestRunId",
                table: "TestResults",
                column: "TestRunId",
                principalTable: "TestRuns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestResults_TestRuns_TestRunId",
                table: "TestResults");

            migrationBuilder.DropIndex(
                name: "IX_TestResults_TestRunId",
                table: "TestResults");
        }
    }
}
