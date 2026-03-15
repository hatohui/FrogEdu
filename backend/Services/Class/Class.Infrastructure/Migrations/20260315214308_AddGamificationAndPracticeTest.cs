using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrogEdu.Class.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGamificationAndPracticeTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPractice",
                schema: "public",
                table: "ExamSessions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Badges",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    IconUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    BadgeType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RequiredScore = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Badges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentBadges",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    BadgeId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClassId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExamSessionId = table.Column<Guid>(type: "uuid", nullable: true),
                    AwardedByTeacherId = table.Column<Guid>(type: "uuid", nullable: true),
                    CustomPraise = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    AwardedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentBadges", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Badges_BadgeType",
                schema: "public",
                table: "Badges",
                column: "BadgeType");

            migrationBuilder.CreateIndex(
                name: "IX_Badges_IsActive",
                schema: "public",
                table: "Badges",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_StudentBadges_BadgeId",
                schema: "public",
                table: "StudentBadges",
                column: "BadgeId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentBadges_ClassId",
                schema: "public",
                table: "StudentBadges",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentBadges_StudentId",
                schema: "public",
                table: "StudentBadges",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentBadges_StudentId_ClassId",
                schema: "public",
                table: "StudentBadges",
                columns: new[] { "StudentId", "ClassId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Badges",
                schema: "public");

            migrationBuilder.DropTable(
                name: "StudentBadges",
                schema: "public");

            migrationBuilder.DropColumn(
                name: "IsPractice",
                schema: "public",
                table: "ExamSessions");
        }
    }
}
