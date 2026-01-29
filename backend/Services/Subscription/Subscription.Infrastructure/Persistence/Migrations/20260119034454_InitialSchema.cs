using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrogEdu.Subscription.Infrastructure.Persistence.Migrations
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
                name: "PromptTemplates",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Template = table.Column<string>(type: "character varying(8000)", maxLength: 8000, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromptTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SessionArtifacts",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SessionId = table.Column<Guid>(type: "uuid", nullable: false),
                    ArtifactType = table.Column<int>(type: "integer", nullable: false),
                    Uri = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Metadata = table.Column<string>(type: "jsonb", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionArtifacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SessionEvents",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SessionId = table.Column<Guid>(type: "uuid", nullable: false),
                    EventType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Payload = table.Column<string>(type: "jsonb", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TutorSessions",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    GradeLevel = table.Column<int>(type: "integer", nullable: false),
                    TextbookId = table.Column<Guid>(type: "uuid", nullable: true),
                    ChapterId = table.Column<Guid>(type: "uuid", nullable: true),
                    TotalTokensUsed = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    LastActivityAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TutorSessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConversationMessages",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TutorSessionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Content = table.Column<string>(type: "character varying(8000)", maxLength: 8000, nullable: false),
                    TokenCount = table.Column<int>(type: "integer", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversationMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConversationMessages_TutorSessions_TutorSessionId",
                        column: x => x.TutorSessionId,
                        principalSchema: "public",
                        principalTable: "TutorSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConversationMessages_IsDeleted",
                schema: "public",
                table: "ConversationMessages",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ConversationMessages_Timestamp",
                schema: "public",
                table: "ConversationMessages",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_ConversationMessages_TutorSessionId",
                schema: "public",
                table: "ConversationMessages",
                column: "TutorSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_PromptTemplates_IsActive",
                schema: "public",
                table: "PromptTemplates",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_PromptTemplates_IsDeleted",
                schema: "public",
                table: "PromptTemplates",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_PromptTemplates_Name",
                schema: "public",
                table: "PromptTemplates",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PromptTemplates_Version",
                schema: "public",
                table: "PromptTemplates",
                column: "Version");

            migrationBuilder.CreateIndex(
                name: "IX_SessionArtifacts_ArtifactType",
                schema: "public",
                table: "SessionArtifacts",
                column: "ArtifactType");

            migrationBuilder.CreateIndex(
                name: "IX_SessionArtifacts_IsDeleted",
                schema: "public",
                table: "SessionArtifacts",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_SessionArtifacts_SessionId",
                schema: "public",
                table: "SessionArtifacts",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionEvents_CreatedAt",
                schema: "public",
                table: "SessionEvents",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_SessionEvents_EventType",
                schema: "public",
                table: "SessionEvents",
                column: "EventType");

            migrationBuilder.CreateIndex(
                name: "IX_SessionEvents_IsDeleted",
                schema: "public",
                table: "SessionEvents",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_SessionEvents_SessionId",
                schema: "public",
                table: "SessionEvents",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorSessions_IsActive_ExpiresAt",
                schema: "public",
                table: "TutorSessions",
                columns: new[] { "IsActive", "ExpiresAt" });

            migrationBuilder.CreateIndex(
                name: "IX_TutorSessions_IsDeleted",
                schema: "public",
                table: "TutorSessions",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TutorSessions_LastActivityAt",
                schema: "public",
                table: "TutorSessions",
                column: "LastActivityAt");

            migrationBuilder.CreateIndex(
                name: "IX_TutorSessions_StudentId",
                schema: "public",
                table: "TutorSessions",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConversationMessages",
                schema: "public");

            migrationBuilder.DropTable(
                name: "PromptTemplates",
                schema: "public");

            migrationBuilder.DropTable(
                name: "SessionArtifacts",
                schema: "public");

            migrationBuilder.DropTable(
                name: "SessionEvents",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TutorSessions",
                schema: "public");
        }
    }
}


