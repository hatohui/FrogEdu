using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrogEdu.Exam.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(name: "public");

            migrationBuilder.CreateTable(
                name: "Exams",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(
                        type: "character varying(256)",
                        maxLength: 256,
                        nullable: false
                    ),
                    Duration = table.Column<int>(type: "integer", nullable: false),
                    AccessCode = table.Column<string>(
                        type: "character varying(50)",
                        maxLength: 50,
                        nullable: true
                    ),
                    PassScore = table.Column<int>(type: "integer", nullable: false),
                    MaxAttempts = table.Column<int>(type: "integer", nullable: false),
                    StartTime = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    EndTime = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    ShouldShuffleQuestions = table.Column<bool>(
                        type: "boolean",
                        nullable: false,
                        defaultValue: false
                    ),
                    ShouldShuffleAnswerOptions = table.Column<bool>(
                        type: "boolean",
                        nullable: false,
                        defaultValue: false
                    ),
                    IsDraft = table.Column<bool>(
                        type: "boolean",
                        nullable: false,
                        defaultValue: true
                    ),
                    IsActive = table.Column<bool>(
                        type: "boolean",
                        nullable: false,
                        defaultValue: false
                    ),
                    TopicId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    CreatedBy = table.Column<Guid>(type: "uuid", maxLength: 256, nullable: false),
                    UpdatedAt = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                    UpdatedBy = table.Column<Guid>(type: "uuid", maxLength: 256, nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exams", x => x.Id);
                }
            );

            migrationBuilder.CreateTable(
                name: "Subjects",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SubjectCode = table.Column<string>(
                        type: "character varying(50)",
                        maxLength: 50,
                        nullable: false
                    ),
                    Name = table.Column<string>(
                        type: "character varying(256)",
                        maxLength: 256,
                        nullable: false
                    ),
                    Description = table.Column<string>(
                        type: "character varying(1000)",
                        maxLength: 1000,
                        nullable: false
                    ),
                    ImageUrl = table.Column<string>(
                        type: "character varying(1000)",
                        maxLength: 1000,
                        nullable: true
                    ),
                    Grade = table.Column<int>(type: "integer", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                }
            );

            migrationBuilder.CreateTable(
                name: "Matrices",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExamId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    CreatedBy = table.Column<Guid>(type: "uuid", maxLength: 256, nullable: false),
                    UpdatedAt = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                    UpdatedBy = table.Column<Guid>(type: "uuid", maxLength: 256, nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matrices_Exams_ExamId",
                        column: x => x.ExamId,
                        principalSchema: "public",
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "Topics",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(
                        type: "character varying(256)",
                        maxLength: 256,
                        nullable: false
                    ),
                    Description = table.Column<string>(
                        type: "character varying(2000)",
                        maxLength: 2000,
                        nullable: false
                    ),
                    IsCurriculum = table.Column<bool>(
                        type: "boolean",
                        nullable: false,
                        defaultValue: false
                    ),
                    SubjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    CreatedBy = table.Column<Guid>(type: "uuid", maxLength: 256, nullable: false),
                    UpdatedAt = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                    UpdatedBy = table.Column<Guid>(type: "uuid", maxLength: 256, nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Topics_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalSchema: "public",
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "MatrixTopics",
                schema: "public",
                columns: table => new
                {
                    MatrixId = table.Column<Guid>(type: "uuid", nullable: false),
                    TopicId = table.Column<Guid>(type: "uuid", nullable: false),
                    CognitiveLevel = table.Column<string>(
                        type: "character varying(50)",
                        maxLength: 50,
                        nullable: false
                    ),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "PK_MatrixTopics",
                        x => new
                        {
                            x.MatrixId,
                            x.TopicId,
                            x.CognitiveLevel,
                        }
                    );
                    table.ForeignKey(
                        name: "FK_MatrixTopics_Matrices_MatrixId",
                        column: x => x.MatrixId,
                        principalSchema: "public",
                        principalTable: "Matrices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_MatrixTopics_Topics_TopicId",
                        column: x => x.TopicId,
                        principalSchema: "public",
                        principalTable: "Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "Questions",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(
                        type: "character varying(5000)",
                        maxLength: 5000,
                        nullable: false
                    ),
                    Point = table.Column<double>(
                        type: "double precision",
                        precision: 5,
                        scale: 2,
                        nullable: false
                    ),
                    Type = table.Column<string>(
                        type: "character varying(50)",
                        maxLength: 50,
                        nullable: false
                    ),
                    MediaUrl = table.Column<string>(
                        type: "character varying(1000)",
                        maxLength: 1000,
                        nullable: true
                    ),
                    CognitiveLevel = table.Column<string>(
                        type: "character varying(50)",
                        maxLength: 50,
                        nullable: false
                    ),
                    IsPublic = table.Column<bool>(
                        type: "boolean",
                        nullable: false,
                        defaultValue: false
                    ),
                    Source = table.Column<string>(
                        type: "character varying(50)",
                        maxLength: 50,
                        nullable: false
                    ),
                    TopicId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false,
                        defaultValueSql: "CURRENT_TIMESTAMP"
                    ),
                    CreatedBy = table.Column<Guid>(type: "uuid", maxLength: 256, nullable: false),
                    UpdatedAt = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                    UpdatedBy = table.Column<Guid>(type: "uuid", maxLength: 256, nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Topics_TopicId",
                        column: x => x.TopicId,
                        principalSchema: "public",
                        principalTable: "Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "Answers",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(
                        type: "character varying(2000)",
                        maxLength: 2000,
                        nullable: false
                    ),
                    IsCorrect = table.Column<bool>(
                        type: "boolean",
                        nullable: false,
                        defaultValue: false
                    ),
                    Explanation = table.Column<string>(
                        type: "character varying(2000)",
                        maxLength: 2000,
                        nullable: true
                    ),
                    QuestionId = table.Column<Guid>(type: "uuid", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalSchema: "public",
                        principalTable: "Questions",
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
                    ExamId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uuid", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamQuestions", x => new { x.ExamId, x.QuestionId });
                    table.ForeignKey(
                        name: "FK_ExamQuestions_Exams_ExamId",
                        column: x => x.ExamId,
                        principalSchema: "public",
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
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

            migrationBuilder.CreateIndex(
                name: "IX_Answers_IsCorrect",
                schema: "public",
                table: "Answers",
                column: "IsCorrect"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId",
                schema: "public",
                table: "Answers",
                column: "QuestionId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_ExamQuestions_ExamId",
                schema: "public",
                table: "ExamQuestions",
                column: "ExamId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_ExamQuestions_QuestionId",
                schema: "public",
                table: "ExamQuestions",
                column: "QuestionId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Exams_AccessCode",
                schema: "public",
                table: "Exams",
                column: "AccessCode",
                unique: true,
                filter: "\"AccessCode\" IS NOT NULL"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Exams_CreatedBy",
                schema: "public",
                table: "Exams",
                column: "CreatedBy"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Exams_IsActive",
                schema: "public",
                table: "Exams",
                column: "IsActive"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Exams_IsDraft",
                schema: "public",
                table: "Exams",
                column: "IsDraft"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Exams_TopicId",
                schema: "public",
                table: "Exams",
                column: "TopicId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Matrices_ExamId",
                schema: "public",
                table: "Matrices",
                column: "ExamId",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_MatrixTopics_MatrixId",
                schema: "public",
                table: "MatrixTopics",
                column: "MatrixId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_MatrixTopics_TopicId",
                schema: "public",
                table: "MatrixTopics",
                column: "TopicId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Questions_CognitiveLevel",
                schema: "public",
                table: "Questions",
                column: "CognitiveLevel"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Questions_CreatedBy",
                schema: "public",
                table: "Questions",
                column: "CreatedBy"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Questions_IsPublic",
                schema: "public",
                table: "Questions",
                column: "IsPublic"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Questions_TopicId",
                schema: "public",
                table: "Questions",
                column: "TopicId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Questions_Type",
                schema: "public",
                table: "Questions",
                column: "Type"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_Grade",
                schema: "public",
                table: "Subjects",
                column: "Grade"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_SubjectCode",
                schema: "public",
                table: "Subjects",
                column: "SubjectCode",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_Topics_IsCurriculum",
                schema: "public",
                table: "Topics",
                column: "IsCurriculum"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Topics_SubjectId",
                schema: "public",
                table: "Topics",
                column: "SubjectId"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Answers", schema: "public");

            migrationBuilder.DropTable(name: "ExamQuestions", schema: "public");

            migrationBuilder.DropTable(name: "MatrixTopics", schema: "public");

            migrationBuilder.DropTable(name: "Questions", schema: "public");

            migrationBuilder.DropTable(name: "Matrices", schema: "public");

            migrationBuilder.DropTable(name: "Topics", schema: "public");

            migrationBuilder.DropTable(name: "Exams", schema: "public");

            migrationBuilder.DropTable(name: "Subjects", schema: "public");
        }
    }
}
