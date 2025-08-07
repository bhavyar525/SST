using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScrumStandUpTrackerProject.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Developers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Developers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DailyStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeveloperName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TaskDetails = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DidYesterday = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DoingToday = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Blockers = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    SubmissionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeveloperId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyStatuses_Developers_DeveloperId",
                        column: x => x.DeveloperId,
                        principalTable: "Developers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyStatuses_DeveloperId",
                table: "DailyStatuses",
                column: "DeveloperId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyStatuses");

            migrationBuilder.DropTable(
                name: "Developers");
        }
    }
}
