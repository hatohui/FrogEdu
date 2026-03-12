using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrogEdu.Class.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEssayFeedbackToStudentAnswer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EssayFeedback",
                schema: "public",
                table: "StudentAnswers",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EssayFeedback",
                schema: "public",
                table: "StudentAnswers");
        }
    }
}
