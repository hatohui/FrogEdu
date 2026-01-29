using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrogEdu.Class.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(name: "public");

            migrationBuilder.CreateTable(
                name: "ExamGenerations",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExamId = table.Column<Guid>(type: "uuid", nullable: false),
                    GeneratedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Prompt = table.Column<string>(
                        type: "character varying(4000)",
                        maxLength: 4000,
                        nullable: false
                    ),
                    ResultUri = table.Column<string>(
                        type: "character varying(1000)",
                        maxLength: 1000,
                        nullable: true
                    ),
                    Error = table.Column<string>(
                        type: "character varying(2000)",
                        maxLength: 2000,
                        nullable: true
                    ),
                    CreatedAt = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    UpdatedAt = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    CreatedBy = table.Column<string>(
                        type: "character varying(256)",
                        maxLength: 256,
                        nullable: true
                    ),
                    UpdatedBy = table.Column<string>(
                        type: "character varying(256)",
                        maxLength: 256,
                        nullable: true
                    ),
                    IsDeleted = table.Column<bool>(
                        type: "boolean",
                        nullable: false,
                        defaultValue: false
                    ),
                    RowVersion = table.Column<byte[]>(
                        type: "bytea",
                        rowVersion: true,
                        nullable: false
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamGenerations", x => x.Id);
                }
            );

            migrationBuilder.CreateTable(
                name: "ExamPapers",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: false
                    ),
                    MatrixEasyCount = table.Column<int>(type: "integer", nullable: false),
                    MatrixMediumCount = table.Column<int>(type: "integer", nullable: false),
                    MatrixHardCount = table.Column<int>(type: "integer", nullable: false),
                    MatrixEasyPoints = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    MatrixMediumPoints = table.Column<decimal>(
                        type: "numeric(5,2)",
                        nullable: false
                    ),
                    MatrixHardPoints = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    TextbookId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    DurationMinutes = table.Column<int>(type: "integer", nullable: false),
                    Instructions = table.Column<string>(
                        type: "character varying(2000)",
                        maxLength: 2000,
                        nullable: true
                    ),
                    ExamPdfS3Key = table.Column<string>(
                        type: "character varying(1000)",
                        maxLength: 1000,
                        nullable: true
                    ),
                    AnswerKeyPdfS3Key = table.Column<string>(
                        type: "character varying(1000)",
                        maxLength: 1000,
                        nullable: true
                    ),
                    Version = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    CreatedAt = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    UpdatedAt = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    CreatedBy = table.Column<string>(
                        type: "character varying(256)",
                        maxLength: 256,
                        nullable: true
                    ),
                    UpdatedBy = table.Column<string>(
                        type: "character varying(256)",
                        maxLength: 256,
                        nullable: true
                    ),
                    IsDeleted = table.Column<bool>(
                        type: "boolean",
                        nullable: false,
                        defaultValue: false
                    ),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamPapers", x => x.Id);
                }
            );

            migrationBuilder.CreateTable(
                name: "ExamSections",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExamId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: false
                    ),
                    Instructions = table.Column<string>(
                        type: "character varying(2000)",
                        maxLength: 2000,
                        nullable: true
                    ),
                    CreatedAt = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    UpdatedAt = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    CreatedBy = table.Column<string>(
                        type: "character varying(256)",
                        maxLength: 256,
                        nullable: true
                    ),
                    UpdatedBy = table.Column<string>(
                        type: "character varying(256)",
                        maxLength: 256,
                        nullable: true
                    ),
                    IsDeleted = table.Column<bool>(
                        type: "boolean",
                        nullable: false,
                        defaultValue: false
                    ),
                    RowVersion = table.Column<byte[]>(
                        type: "bytea",
                        rowVersion: true,
                        nullable: false
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamSections", x => x.Id);
                }
            );

            migrationBuilder.CreateTable(
                name: "QuestionBanks",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: false
                    ),
                    Description = table.Column<string>(
                        type: "character varying(2000)",
                        maxLength: 2000,
                        nullable: true
                    ),
                    IsPublic = table.Column<bool>(
                        type: "boolean",
                        nullable: false,
                        defaultValue: false
                    ),
                    CreatedAt = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    UpdatedAt = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    CreatedBy = table.Column<string>(
                        type: "character varying(256)",
                        maxLength: 256,
                        nullable: true
                    ),
                    UpdatedBy = table.Column<string>(
                        type: "character varying(256)",
                        maxLength: 256,
                        nullable: true
                    ),
                    IsDeleted = table.Column<bool>(
                        type: "boolean",
                        nullable: false,
                        defaultValue: false
                    ),
                    RowVersion = table.Column<byte[]>(
                        type: "bytea",
                        rowVersion: true,
                        nullable: false
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionBanks", x => x.Id);
                }
            );

            migrationBuilder.CreateTable(
                name: "Rubrics",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Criteria = table.Column<string>(type: "jsonb", nullable: false),
                    Description = table.Column<string>(
                        type: "character varying(2000)",
                        maxLength: 2000,
                        nullable: true
                    ),
                    CreatedAt = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    UpdatedAt = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    CreatedBy = table.Column<string>(
                        type: "character varying(256)",
                        maxLength: 256,
                        nullable: true
                    ),
                    UpdatedBy = table.Column<string>(
                        type: "character varying(256)",
                        maxLength: 256,
                        nullable: true
                    ),
                    IsDeleted = table.Column<bool>(
                        type: "boolean",
                        nullable: false,
                        defaultValue: false
                    ),
                    RowVersion = table.Column<byte[]>(
                        type: "bytea",
                        rowVersion: true,
                        nullable: false
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rubrics", x => x.Id);
                }
            );

            migrationBuilder.CreateTable(
                name: "Submissions",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExamId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartedAt = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    SubmittedAt = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                    Score = table.Column<decimal>(
                        type: "numeric(5,2)",
                        precision: 5,
                        scale: 2,
                        nullable: true
                    ),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    UpdatedAt = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    CreatedBy = table.Column<string>(
                        type: "character varying(256)",
                        maxLength: 256,
                        nullable: true
                    ),
                    UpdatedBy = table.Column<string>(
                        type: "character varying(256)",
                        maxLength: 256,
                        nullable: true
                    ),
                    IsDeleted = table.Column<bool>(
                        type: "boolean",
                        nullable: false,
                        defaultValue: false
                    ),
                    RowVersion = table.Column<byte[]>(
                        type: "bytea",
                        rowVersion: true,
                        nullable: false
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Submissions", x => x.Id);
                }
            );

            migrationBuilder.CreateTable(
                name: "Questions",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(
                        type: "character varying(4000)",
                        maxLength: 4000,
                        nullable: false
                    ),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Difficulty = table.Column<int>(type: "integer", nullable: false),
                    Points = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    TextbookId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChapterId = table.Column<Guid>(type: "uuid", nullable: true),
                    Explanation = table.Column<string>(
                        type: "character varying(2000)",
                        maxLength: 2000,
                        nullable: true
                    ),
                    ImageS3Key = table.Column<string>(
                        type: "character varying(1000)",
                        maxLength: 1000,
                        nullable: true
                    ),
                    LearningObjectives = table.Column<string>(
                        type: "character varying(1000)",
                        maxLength: 1000,
                        nullable: true
                    ),
                    QuestionBankId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    UpdatedAt = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    CreatedBy = table.Column<string>(
                        type: "character varying(256)",
                        maxLength: 256,
                        nullable: true
                    ),
                    UpdatedBy = table.Column<string>(
                        type: "character varying(256)",
                        maxLength: 256,
                        nullable: true
                    ),
                    IsDeleted = table.Column<bool>(
                        type: "boolean",
                        nullable: false,
                        defaultValue: false
                    ),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_QuestionBanks_QuestionBankId",
                        column: x => x.QuestionBankId,
                        principalSchema: "public",
                        principalTable: "QuestionBanks",
                        principalColumn: "Id"
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "Answers",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SubmissionId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uuid", nullable: false),
                    SelectedOptionIds = table.Column<Guid[]>(type: "uuid[]", nullable: true),
                    AnswerText = table.Column<string>(
                        type: "character varying(4000)",
                        maxLength: 4000,
                        nullable: true
                    ),
                    Score = table.Column<decimal>(
                        type: "numeric(5,2)",
                        precision: 5,
                        scale: 2,
                        nullable: true
                    ),
                    Feedback = table.Column<string>(
                        type: "character varying(2000)",
                        maxLength: 2000,
                        nullable: true
                    ),
                    CreatedAt = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    UpdatedAt = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    CreatedBy = table.Column<string>(
                        type: "character varying(256)",
                        maxLength: 256,
                        nullable: true
                    ),
                    UpdatedBy = table.Column<string>(
                        type: "character varying(256)",
                        maxLength: 256,
                        nullable: true
                    ),
                    IsDeleted = table.Column<bool>(
                        type: "boolean",
                        nullable: false,
                        defaultValue: false
                    ),
                    RowVersion = table.Column<byte[]>(
                        type: "bytea",
                        rowVersion: true,
                        nullable: false
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answers_Submissions_SubmissionId",
                        column: x => x.SubmissionId,
                        principalSchema: "public",
                        principalTable: "Submissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "ExamQuestions",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExamPaperId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false),
                    Points = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    ExamSectionId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    UpdatedAt = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    CreatedBy = table.Column<string>(
                        type: "character varying(256)",
                        maxLength: 256,
                        nullable: true
                    ),
                    UpdatedBy = table.Column<string>(
                        type: "character varying(256)",
                        maxLength: 256,
                        nullable: true
                    ),
                    IsDeleted = table.Column<bool>(
                        type: "boolean",
                        nullable: false,
                        defaultValue: false
                    ),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamQuestions_ExamPapers_ExamPaperId",
                        column: x => x.ExamPaperId,
                        principalSchema: "public",
                        principalTable: "ExamPapers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_ExamQuestions_ExamSections_ExamSectionId",
                        column: x => x.ExamSectionId,
                        principalSchema: "public",
                        principalTable: "ExamSections",
                        principalColumn: "Id"
                    );
                    table.ForeignKey(
                        name: "FK_ExamQuestions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalSchema: "public",
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "QuestionOptions",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OptionText = table.Column<string>(
                        type: "character varying(2000)",
                        maxLength: 2000,
                        nullable: false
                    ),
                    IsCorrect = table.Column<bool>(type: "boolean", nullable: false),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    UpdatedAt = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    CreatedBy = table.Column<string>(
                        type: "character varying(256)",
                        maxLength: 256,
                        nullable: true
                    ),
                    UpdatedBy = table.Column<string>(
                        type: "character varying(256)",
                        maxLength: 256,
                        nullable: true
                    ),
                    IsDeleted = table.Column<bool>(
                        type: "boolean",
                        nullable: false,
                        defaultValue: false
                    ),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionOptions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalSchema: "public",
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_Answers_IsDeleted",
                schema: "public",
                table: "Answers",
                column: "IsDeleted"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId",
                schema: "public",
                table: "Answers",
                column: "QuestionId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Answers_SubmissionId",
                schema: "public",
                table: "Answers",
                column: "SubmissionId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Answers_SubmissionId_QuestionId",
                schema: "public",
                table: "Answers",
                columns: new[] { "SubmissionId", "QuestionId" },
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_ExamGenerations_ExamId",
                schema: "public",
                table: "ExamGenerations",
                column: "ExamId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_ExamGenerations_GeneratedBy",
                schema: "public",
                table: "ExamGenerations",
                column: "GeneratedBy"
            );

            migrationBuilder.CreateIndex(
                name: "IX_ExamGenerations_IsDeleted",
                schema: "public",
                table: "ExamGenerations",
                column: "IsDeleted"
            );

            migrationBuilder.CreateIndex(
                name: "IX_ExamGenerations_Status",
                schema: "public",
                table: "ExamGenerations",
                column: "Status"
            );

            migrationBuilder.CreateIndex(
                name: "IX_ExamPapers_CreatedAt",
                schema: "public",
                table: "ExamPapers",
                column: "CreatedAt"
            );

            migrationBuilder.CreateIndex(
                name: "IX_ExamPapers_CreatedByUserId",
                schema: "public",
                table: "ExamPapers",
                column: "CreatedByUserId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_ExamPapers_IsDeleted",
                schema: "public",
                table: "ExamPapers",
                column: "IsDeleted"
            );

            migrationBuilder.CreateIndex(
                name: "IX_ExamPapers_TextbookId",
                schema: "public",
                table: "ExamPapers",
                column: "TextbookId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_ExamQuestions_ExamPaperId",
                schema: "public",
                table: "ExamQuestions",
                column: "ExamPaperId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_ExamQuestions_ExamPaperId_OrderIndex",
                schema: "public",
                table: "ExamQuestions",
                columns: new[] { "ExamPaperId", "OrderIndex" },
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_ExamQuestions_ExamPaperId_QuestionId",
                schema: "public",
                table: "ExamQuestions",
                columns: new[] { "ExamPaperId", "QuestionId" },
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_ExamQuestions_ExamSectionId",
                schema: "public",
                table: "ExamQuestions",
                column: "ExamSectionId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_ExamQuestions_IsDeleted",
                schema: "public",
                table: "ExamQuestions",
                column: "IsDeleted"
            );

            migrationBuilder.CreateIndex(
                name: "IX_ExamQuestions_QuestionId",
                schema: "public",
                table: "ExamQuestions",
                column: "QuestionId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_ExamSections_ExamId_OrderIndex",
                schema: "public",
                table: "ExamSections",
                columns: new[] { "ExamId", "OrderIndex" },
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_ExamSections_IsDeleted",
                schema: "public",
                table: "ExamSections",
                column: "IsDeleted"
            );

            migrationBuilder.CreateIndex(
                name: "IX_QuestionBanks_IsDeleted",
                schema: "public",
                table: "QuestionBanks",
                column: "IsDeleted"
            );

            migrationBuilder.CreateIndex(
                name: "IX_QuestionBanks_IsPublic",
                schema: "public",
                table: "QuestionBanks",
                column: "IsPublic"
            );

            migrationBuilder.CreateIndex(
                name: "IX_QuestionBanks_OwnerId",
                schema: "public",
                table: "QuestionBanks",
                column: "OwnerId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_QuestionOptions_IsDeleted",
                schema: "public",
                table: "QuestionOptions",
                column: "IsDeleted"
            );

            migrationBuilder.CreateIndex(
                name: "IX_QuestionOptions_QuestionId",
                schema: "public",
                table: "QuestionOptions",
                column: "QuestionId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_QuestionOptions_QuestionId_OrderIndex",
                schema: "public",
                table: "QuestionOptions",
                columns: new[] { "QuestionId", "OrderIndex" },
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ChapterId",
                schema: "public",
                table: "Questions",
                column: "ChapterId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Questions_Difficulty",
                schema: "public",
                table: "Questions",
                column: "Difficulty"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Questions_IsDeleted",
                schema: "public",
                table: "Questions",
                column: "IsDeleted"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuestionBankId",
                schema: "public",
                table: "Questions",
                column: "QuestionBankId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Questions_TextbookId",
                schema: "public",
                table: "Questions",
                column: "TextbookId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Questions_TextbookId_Difficulty",
                schema: "public",
                table: "Questions",
                columns: new[] { "TextbookId", "Difficulty" }
            );

            migrationBuilder.CreateIndex(
                name: "IX_Questions_Type",
                schema: "public",
                table: "Questions",
                column: "Type"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Rubrics_IsDeleted",
                schema: "public",
                table: "Rubrics",
                column: "IsDeleted"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Rubrics_QuestionId",
                schema: "public",
                table: "Rubrics",
                column: "QuestionId",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_ExamId",
                schema: "public",
                table: "Submissions",
                column: "ExamId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_ExamId_StudentId",
                schema: "public",
                table: "Submissions",
                columns: new[] { "ExamId", "StudentId" }
            );

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_IsDeleted",
                schema: "public",
                table: "Submissions",
                column: "IsDeleted"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_Status",
                schema: "public",
                table: "Submissions",
                column: "Status"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_StudentId",
                schema: "public",
                table: "Submissions",
                column: "StudentId"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Answers", schema: "public");

            migrationBuilder.DropTable(name: "ExamGenerations", schema: "public");

            migrationBuilder.DropTable(name: "ExamQuestions", schema: "public");

            migrationBuilder.DropTable(name: "QuestionOptions", schema: "public");

            migrationBuilder.DropTable(name: "Rubrics", schema: "public");

            migrationBuilder.DropTable(name: "Submissions", schema: "public");

            migrationBuilder.DropTable(name: "ExamPapers", schema: "public");

            migrationBuilder.DropTable(name: "ExamSections", schema: "public");

            migrationBuilder.DropTable(name: "Questions", schema: "public");

            migrationBuilder.DropTable(name: "QuestionBanks", schema: "public");
        }
    }
}
