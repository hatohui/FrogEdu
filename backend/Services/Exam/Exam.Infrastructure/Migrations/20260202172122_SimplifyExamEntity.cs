using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrogEdu.Exam.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SimplifyExamEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Exams_AccessCode",
                schema: "public",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "AccessCode",
                schema: "public",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "Duration",
                schema: "public",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "EndTime",
                schema: "public",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "MaxAttempts",
                schema: "public",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "ShouldShuffleAnswerOptions",
                schema: "public",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "ShouldShuffleQuestions",
                schema: "public",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "StartTime",
                schema: "public",
                table: "Exams");

            migrationBuilder.RenameColumn(
                name: "Title",
                schema: "public",
                table: "Exams",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "PassScore",
                schema: "public",
                table: "Exams",
                newName: "Grade");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "public",
                table: "Exams",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "SubjectId",
                schema: "public",
                table: "Exams",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Exams_Grade",
                schema: "public",
                table: "Exams",
                column: "Grade");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_SubjectId",
                schema: "public",
                table: "Exams",
                column: "SubjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Exams_Grade",
                schema: "public",
                table: "Exams");

            migrationBuilder.DropIndex(
                name: "IX_Exams_SubjectId",
                schema: "public",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "public",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                schema: "public",
                table: "Exams");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "public",
                table: "Exams",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Grade",
                schema: "public",
                table: "Exams",
                newName: "PassScore");

            migrationBuilder.AddColumn<string>(
                name: "AccessCode",
                schema: "public",
                table: "Exams",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                schema: "public",
                table: "Exams",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                schema: "public",
                table: "Exams",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "MaxAttempts",
                schema: "public",
                table: "Exams",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "ShouldShuffleAnswerOptions",
                schema: "public",
                table: "Exams",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShouldShuffleQuestions",
                schema: "public",
                table: "Exams",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                schema: "public",
                table: "Exams",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Exams_AccessCode",
                schema: "public",
                table: "Exams",
                column: "AccessCode",
                unique: true,
                filter: "[AccessCode] IS NOT NULL");
        }
    }
}
