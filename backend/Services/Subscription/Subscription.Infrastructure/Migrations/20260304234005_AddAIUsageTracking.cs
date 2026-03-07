using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrogEdu.Subscription.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAIUsageTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AIUsageRecords",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ActionType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    UsedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Metadata = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AIUsageRecords", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AIUsageRecords_UserId",
                schema: "public",
                table: "AIUsageRecords",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AIUsageRecords_UserId_UsedAt",
                schema: "public",
                table: "AIUsageRecords",
                columns: new[] { "UserId", "UsedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AIUsageRecords",
                schema: "public");
        }
    }
}
