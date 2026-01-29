using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrogEdu.Exam.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "Assets",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LessonId = table.Column<Guid>(type: "uuid", nullable: true),
                    Kind = table.Column<int>(type: "integer", nullable: false),
                    Uri = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    MimeType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ByteSize = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lessons",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ChapterId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Summary = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    DurationMinutes = table.Column<int>(type: "integer", nullable: true),
                    LearningObjectives = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lessons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Textbooks",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Subject_Code = table.Column<string>(type: "text", nullable: false),
                    Subject = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Subject_Grade = table.Column<short>(type: "smallint", nullable: false),
                    Subject_Description = table.Column<string>(type: "text", nullable: true),
                    Subject_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Subject_CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Subject_UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Subject_CreatedBy = table.Column<string>(type: "text", nullable: true),
                    Subject_UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    Subject_IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Subject_RowVersion = table.Column<byte[]>(type: "bytea", nullable: false),
                    GradeLevel = table.Column<int>(type: "integer", nullable: false),
                    Publisher = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    PublicationYear = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CoverImageS3Key = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Textbooks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LessonTags",
                schema: "public",
                columns: table => new
                {
                    LessonId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonTags", x => new { x.LessonId, x.TagId });
                    table.ForeignKey(
                        name: "FK_LessonTags_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalSchema: "public",
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonTags_Tags_TagId",
                        column: x => x.TagId,
                        principalSchema: "public",
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Chapters",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ChapterNumber = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    TextbookId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chapters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chapters_Textbooks_TextbookId",
                        column: x => x.TextbookId,
                        principalSchema: "public",
                        principalTable: "Textbooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pages",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PageNumber = table.Column<int>(type: "integer", nullable: false),
                    S3Key = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ChapterId = table.Column<Guid>(type: "uuid", nullable: false),
                    LessonId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pages_Chapters_ChapterId",
                        column: x => x.ChapterId,
                        principalSchema: "public",
                        principalTable: "Chapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pages_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalSchema: "public",
                        principalTable: "Lessons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assets_IsDeleted",
                schema: "public",
                table: "Assets",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_Kind",
                schema: "public",
                table: "Assets",
                column: "Kind");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_LessonId",
                schema: "public",
                table: "Assets",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_Chapters_IsDeleted",
                schema: "public",
                table: "Chapters",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Chapters_TextbookId",
                schema: "public",
                table: "Chapters",
                column: "TextbookId");

            migrationBuilder.CreateIndex(
                name: "IX_Chapters_TextbookId_ChapterNumber",
                schema: "public",
                table: "Chapters",
                columns: new[] { "TextbookId", "ChapterNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_ChapterId_OrderIndex",
                schema: "public",
                table: "Lessons",
                columns: new[] { "ChapterId", "OrderIndex" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_IsDeleted",
                schema: "public",
                table: "Lessons",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_LessonTags_TagId",
                schema: "public",
                table: "LessonTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_ChapterId",
                schema: "public",
                table: "Pages",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_ChapterId_PageNumber",
                schema: "public",
                table: "Pages",
                columns: new[] { "ChapterId", "PageNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pages_IsDeleted",
                schema: "public",
                table: "Pages",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_LessonId",
                schema: "public",
                table: "Pages",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_IsDeleted",
                schema: "public",
                table: "Tags",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Name",
                schema: "public",
                table: "Tags",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Textbooks_GradeLevel_Subject",
                schema: "public",
                table: "Textbooks",
                column: "GradeLevel")
                .Annotation("Npgsql:IndexInclude", new[] { "Title", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Textbooks_IsDeleted",
                schema: "public",
                table: "Textbooks",
                column: "IsDeleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assets",
                schema: "public");

            migrationBuilder.DropTable(
                name: "LessonTags",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Pages",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Tags",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Chapters",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Lessons",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Textbooks",
                schema: "public");
        }
    }
}


