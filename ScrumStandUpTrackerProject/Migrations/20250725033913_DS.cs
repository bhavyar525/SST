using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScrumStandUpTrackerProject.Migrations
{
    /// <inheritdoc />
    public partial class DS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DailyStatuses_DeveloperId",
                table: "DailyStatuses",
                column: "DeveloperId");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyStatuses_Developers_DeveloperId",
                table: "DailyStatuses",
                column: "DeveloperId",
                principalTable: "Developers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyStatuses_Developers_DeveloperId",
                table: "DailyStatuses");

            migrationBuilder.DropIndex(
                name: "IX_DailyStatuses_DeveloperId",
                table: "DailyStatuses");
        }
    }
}
