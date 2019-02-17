using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MitchRankChecker.EntityFramework.Migrations
{
    public partial class initialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RankCheckRequests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SearchUrl = table.Column<string>(nullable: false),
                    MaximumRecords = table.Column<int>(nullable: false),
                    StatusId = table.Column<int>(nullable: false),
                    ErrorMessage = table.Column<string>(nullable: true),
                    TermToSearch = table.Column<string>(nullable: false),
                    WebsiteUrl = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RankCheckRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SearchEntries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Rank = table.Column<int>(nullable: false),
                    Url = table.Column<string>(nullable: false),
                    RankCheckRequestId = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SearchEntries_RankCheckRequests_RankCheckRequestId",
                        column: x => x.RankCheckRequestId,
                        principalTable: "RankCheckRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SearchEntries_RankCheckRequestId",
                table: "SearchEntries",
                column: "RankCheckRequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SearchEntries");

            migrationBuilder.DropTable(
                name: "RankCheckRequests");
        }
    }
}
