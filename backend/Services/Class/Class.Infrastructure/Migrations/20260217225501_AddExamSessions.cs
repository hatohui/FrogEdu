using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrogEdu.Class.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExamSessions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExamSessions",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClassId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExamId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RetryTimes = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    IsRetryable = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    ShouldShuffleQuestions = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    ShouldShuffleAnswers = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    AllowPartialScoring = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamSessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentExamAttempts",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExamSessionId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Score = table.Column<double>(type: "double precision", precision: 10, scale: 2, nullable: false, defaultValue: 0.0),
                    TotalPoints = table.Column<double>(type: "double precision", precision: 10, scale: 2, nullable: false, defaultValue: 0.0),
                    AttemptNumber = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentExamAttempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentExamAttempts_ExamSessions_ExamSessionId",
                        column: x => x.ExamSessionId,
                        principalSchema: "public",
                        principalTable: "ExamSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentAnswers",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AttemptId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uuid", nullable: false),
                    SelectedAnswerIds = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Score = table.Column<double>(type: "double precision", precision: 10, scale: 2, nullable: false, defaultValue: 0.0),
                    IsCorrect = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsPartiallyCorrect = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentAnswers_StudentExamAttempts_AttemptId",
                        column: x => x.AttemptId,
                        principalSchema: "public",
                        principalTable: "StudentExamAttempts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamSessions_ClassId",
                schema: "public",
                table: "ExamSessions",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSessions_ClassId_ExamId",
                schema: "public",
                table: "ExamSessions",
                columns: new[] { "ClassId", "ExamId" });

            migrationBuilder.CreateIndex(
                name: "IX_ExamSessions_EndTime",
                schema: "public",
                table: "ExamSessions",
                column: "EndTime");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSessions_ExamId",
                schema: "public",
                table: "ExamSessions",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSessions_IsActive",
                schema: "public",
                table: "ExamSessions",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSessions_StartTime",
                schema: "public",
                table: "ExamSessions",
                column: "StartTime");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAnswers_AttemptId",
                schema: "public",
                table: "StudentAnswers",
                column: "AttemptId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAnswers_AttemptId_QuestionId",
                schema: "public",
                table: "StudentAnswers",
                columns: new[] { "AttemptId", "QuestionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentExamAttempts_ExamSessionId",
                schema: "public",
                table: "StudentExamAttempts",
                column: "ExamSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentExamAttempts_SessionId_StudentId",
                schema: "public",
                table: "StudentExamAttempts",
                columns: new[] { "ExamSessionId", "StudentId" });

            migrationBuilder.CreateIndex(
                name: "IX_StudentExamAttempts_Status",
                schema: "public",
                table: "StudentExamAttempts",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_StudentExamAttempts_StudentId",
                schema: "public",
                table: "StudentExamAttempts",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentAnswers",
                schema: "public");

            migrationBuilder.DropTable(
                name: "StudentExamAttempts",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ExamSessions",
                schema: "public");
        }
    }
}
