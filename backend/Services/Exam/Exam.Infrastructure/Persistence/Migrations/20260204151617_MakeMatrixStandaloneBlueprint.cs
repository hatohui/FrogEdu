using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrogEdu.Exam.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MakeMatrixStandaloneBlueprint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matrices_Exams_ExamId",
                schema: "public",
                table: "Matrices");

            migrationBuilder.DropIndex(
                name: "IX_Matrices_ExamId",
                schema: "public",
                table: "Matrices");

            migrationBuilder.RenameColumn(
                name: "ExamId",
                schema: "public",
                table: "Matrices",
                newName: "SubjectId");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "public",
                table: "Matrices",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Grade",
                schema: "public",
                table: "Matrices",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "public",
                table: "Matrices",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "MatrixId",
                schema: "public",
                table: "Exams",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Matrices_CreatedBy",
                schema: "public",
                table: "Matrices",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Matrices_Grade",
                schema: "public",
                table: "Matrices",
                column: "Grade");

            migrationBuilder.CreateIndex(
                name: "IX_Matrices_SubjectId",
                schema: "public",
                table: "Matrices",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_MatrixId",
                schema: "public",
                table: "Exams",
                column: "MatrixId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Matrices_MatrixId",
                schema: "public",
                table: "Exams",
                column: "MatrixId",
                principalSchema: "public",
                principalTable: "Matrices",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Matrices_MatrixId",
                schema: "public",
                table: "Exams");

            migrationBuilder.DropIndex(
                name: "IX_Matrices_CreatedBy",
                schema: "public",
                table: "Matrices");

            migrationBuilder.DropIndex(
                name: "IX_Matrices_Grade",
                schema: "public",
                table: "Matrices");

            migrationBuilder.DropIndex(
                name: "IX_Matrices_SubjectId",
                schema: "public",
                table: "Matrices");

            migrationBuilder.DropIndex(
                name: "IX_Exams_MatrixId",
                schema: "public",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "public",
                table: "Matrices");

            migrationBuilder.DropColumn(
                name: "Grade",
                schema: "public",
                table: "Matrices");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "public",
                table: "Matrices");

            migrationBuilder.DropColumn(
                name: "MatrixId",
                schema: "public",
                table: "Exams");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                schema: "public",
                table: "Matrices",
                newName: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_Matrices_ExamId",
                schema: "public",
                table: "Matrices",
                column: "ExamId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Matrices_Exams_ExamId",
                schema: "public",
                table: "Matrices",
                column: "ExamId",
                principalSchema: "public",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
