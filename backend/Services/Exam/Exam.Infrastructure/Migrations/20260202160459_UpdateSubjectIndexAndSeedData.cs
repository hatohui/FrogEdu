using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FrogEdu.Exam.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSubjectIndexAndSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Subjects_SubjectCode",
                schema: "public",
                table: "Subjects");

            migrationBuilder.InsertData(
                schema: "public",
                table: "Subjects",
                columns: new[] { "Id", "Description", "Grade", "ImageUrl", "Name", "SubjectCode" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), "Study of numbers, quantities, and shapes", 1, null, "Mathematics", "math" },
                    { new Guid("00000000-0000-0000-0000-000000000002"), "Study of past events and Earth's features", 1, null, "History & Geography", "history_geography" },
                    { new Guid("00000000-0000-0000-0000-000000000003"), "English language and literature", 1, null, "English", "english" },
                    { new Guid("00000000-0000-0000-0000-000000000004"), "Study of written works and literary analysis", 1, null, "Literature", "literature" },
                    { new Guid("00000000-0000-0000-0000-000000000005"), "Computer science and digital technology", 1, null, "Information Technology", "it" },
                    { new Guid("00000000-0000-0000-0000-000000000006"), "Applied science and engineering", 1, null, "Technology", "technology" },
                    { new Guid("00000000-0000-0000-0000-000000000007"), "Visual arts and creative expression", 1, null, "Art", "art" },
                    { new Guid("00000000-0000-0000-0000-000000000008"), "Study of natural world and physical phenomena", 1, null, "Science", "science" },
                    { new Guid("00000000-0000-0000-0000-000000000009"), "Musical theory and performance", 1, null, "Music", "music" },
                    { new Guid("00000000-0000-0000-0000-000000000010"), "Hands-on learning and practical experiences", 1, null, "Experiential Activities", "experiential_activities" },
                    { new Guid("00000000-0000-0000-0000-000000000011"), "Study of numbers, quantities, and shapes", 2, null, "Mathematics", "math" },
                    { new Guid("00000000-0000-0000-0000-000000000012"), "Study of past events and Earth's features", 2, null, "History & Geography", "history_geography" },
                    { new Guid("00000000-0000-0000-0000-000000000013"), "English language and literature", 2, null, "English", "english" },
                    { new Guid("00000000-0000-0000-0000-000000000014"), "Study of written works and literary analysis", 2, null, "Literature", "literature" },
                    { new Guid("00000000-0000-0000-0000-000000000015"), "Computer science and digital technology", 2, null, "Information Technology", "it" },
                    { new Guid("00000000-0000-0000-0000-000000000016"), "Applied science and engineering", 2, null, "Technology", "technology" },
                    { new Guid("00000000-0000-0000-0000-000000000017"), "Visual arts and creative expression", 2, null, "Art", "art" },
                    { new Guid("00000000-0000-0000-0000-000000000018"), "Study of natural world and physical phenomena", 2, null, "Science", "science" },
                    { new Guid("00000000-0000-0000-0000-000000000019"), "Musical theory and performance", 2, null, "Music", "music" },
                    { new Guid("00000000-0000-0000-0000-000000000020"), "Hands-on learning and practical experiences", 2, null, "Experiential Activities", "experiential_activities" },
                    { new Guid("00000000-0000-0000-0000-000000000021"), "Study of numbers, quantities, and shapes", 3, null, "Mathematics", "math" },
                    { new Guid("00000000-0000-0000-0000-000000000022"), "Study of past events and Earth's features", 3, null, "History & Geography", "history_geography" },
                    { new Guid("00000000-0000-0000-0000-000000000023"), "English language and literature", 3, null, "English", "english" },
                    { new Guid("00000000-0000-0000-0000-000000000024"), "Study of written works and literary analysis", 3, null, "Literature", "literature" },
                    { new Guid("00000000-0000-0000-0000-000000000025"), "Computer science and digital technology", 3, null, "Information Technology", "it" },
                    { new Guid("00000000-0000-0000-0000-000000000026"), "Applied science and engineering", 3, null, "Technology", "technology" },
                    { new Guid("00000000-0000-0000-0000-000000000027"), "Visual arts and creative expression", 3, null, "Art", "art" },
                    { new Guid("00000000-0000-0000-0000-000000000028"), "Study of natural world and physical phenomena", 3, null, "Science", "science" },
                    { new Guid("00000000-0000-0000-0000-000000000029"), "Musical theory and performance", 3, null, "Music", "music" },
                    { new Guid("00000000-0000-0000-0000-000000000030"), "Hands-on learning and practical experiences", 3, null, "Experiential Activities", "experiential_activities" },
                    { new Guid("00000000-0000-0000-0000-000000000031"), "Study of numbers, quantities, and shapes", 4, null, "Mathematics", "math" },
                    { new Guid("00000000-0000-0000-0000-000000000032"), "Study of past events and Earth's features", 4, null, "History & Geography", "history_geography" },
                    { new Guid("00000000-0000-0000-0000-000000000033"), "English language and literature", 4, null, "English", "english" },
                    { new Guid("00000000-0000-0000-0000-000000000034"), "Study of written works and literary analysis", 4, null, "Literature", "literature" },
                    { new Guid("00000000-0000-0000-0000-000000000035"), "Computer science and digital technology", 4, null, "Information Technology", "it" },
                    { new Guid("00000000-0000-0000-0000-000000000036"), "Applied science and engineering", 4, null, "Technology", "technology" },
                    { new Guid("00000000-0000-0000-0000-000000000037"), "Visual arts and creative expression", 4, null, "Art", "art" },
                    { new Guid("00000000-0000-0000-0000-000000000038"), "Study of natural world and physical phenomena", 4, null, "Science", "science" },
                    { new Guid("00000000-0000-0000-0000-000000000039"), "Musical theory and performance", 4, null, "Music", "music" },
                    { new Guid("00000000-0000-0000-0000-000000000040"), "Hands-on learning and practical experiences", 4, null, "Experiential Activities", "experiential_activities" },
                    { new Guid("00000000-0000-0000-0000-000000000041"), "Study of numbers, quantities, and shapes", 5, null, "Mathematics", "math" },
                    { new Guid("00000000-0000-0000-0000-000000000042"), "Study of past events and Earth's features", 5, null, "History & Geography", "history_geography" },
                    { new Guid("00000000-0000-0000-0000-000000000043"), "English language and literature", 5, null, "English", "english" },
                    { new Guid("00000000-0000-0000-0000-000000000044"), "Study of written works and literary analysis", 5, null, "Literature", "literature" },
                    { new Guid("00000000-0000-0000-0000-000000000045"), "Computer science and digital technology", 5, null, "Information Technology", "it" },
                    { new Guid("00000000-0000-0000-0000-000000000046"), "Applied science and engineering", 5, null, "Technology", "technology" },
                    { new Guid("00000000-0000-0000-0000-000000000047"), "Visual arts and creative expression", 5, null, "Art", "art" },
                    { new Guid("00000000-0000-0000-0000-000000000048"), "Study of natural world and physical phenomena", 5, null, "Science", "science" },
                    { new Guid("00000000-0000-0000-0000-000000000049"), "Musical theory and performance", 5, null, "Music", "music" },
                    { new Guid("00000000-0000-0000-0000-000000000050"), "Hands-on learning and practical experiences", 5, null, "Experiential Activities", "experiential_activities" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_SubjectCode_Grade",
                schema: "public",
                table: "Subjects",
                columns: new[] { "SubjectCode", "Grade" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Subjects_SubjectCode_Grade",
                schema: "public",
                table: "Subjects");

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000010"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000011"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000012"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000013"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000014"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000015"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000016"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000017"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000018"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000019"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000020"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000021"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000022"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000023"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000024"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000025"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000026"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000027"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000028"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000029"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000030"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000031"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000032"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000033"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000034"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000035"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000036"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000037"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000038"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000039"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000040"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000041"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000042"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000043"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000044"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000045"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000046"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000047"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000048"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000049"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000050"));

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_SubjectCode",
                schema: "public",
                table: "Subjects",
                column: "SubjectCode",
                unique: true);
        }
    }
}
