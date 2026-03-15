using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FrogEdu.Exam.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCurriculumSeeds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "Description",
                value: "Toán học – Nghiên cứu về số, phép tính và hình học");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Tiếng Việt – Đọc, viết, nghe, nói và kiến thức ngôn ngữ", "Literature", "literature" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                column: "Description",
                value: "Tiếng Anh – Ngôn ngữ và giao tiếp quốc tế");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000004"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Đạo đức – Giáo dục giá trị sống và kỹ năng ứng xử", "Ethics", "ethics" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000005"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Tự nhiên và Xã hội – Khám phá thế giới tự nhiên và đời sống xã hội", "Nature & Society", "nature_and_society" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000006"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Mĩ thuật – Nghệ thuật tạo hình và thẩm mỹ", "Art", "art" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000007"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Âm nhạc – Lý thuyết âm nhạc và biểu diễn", "Music", "music" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000008"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Hoạt động trải nghiệm – Học tập thực hành và kỹ năng sống", "Experiential Activities", "experiential_activities" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000009"),
                columns: new[] { "Description", "Grade", "Name", "SubjectCode" },
                values: new object[] { "Toán học – Nghiên cứu về số, phép tính và hình học", 2, "Mathematics", "math" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000010"),
                columns: new[] { "Description", "Grade", "Name", "SubjectCode" },
                values: new object[] { "Tiếng Việt – Đọc, viết, nghe, nói và kiến thức ngôn ngữ", 2, "Literature", "literature" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000011"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Tiếng Anh – Ngôn ngữ và giao tiếp quốc tế", "English", "english" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000012"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Đạo đức – Giáo dục giá trị sống và kỹ năng ứng xử", "Ethics", "ethics" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000013"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Tự nhiên và Xã hội – Khám phá thế giới tự nhiên và đời sống xã hội", "Nature & Society", "nature_and_society" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000014"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Mĩ thuật – Nghệ thuật tạo hình và thẩm mỹ", "Art", "art" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000015"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Âm nhạc – Lý thuyết âm nhạc và biểu diễn", "Music", "music" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000016"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Hoạt động trải nghiệm – Học tập thực hành và kỹ năng sống", "Experiential Activities", "experiential_activities" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000017"),
                columns: new[] { "Description", "Grade", "Name", "SubjectCode" },
                values: new object[] { "Toán học – Nghiên cứu về số, phép tính và hình học", 3, "Mathematics", "math" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000018"),
                columns: new[] { "Description", "Grade", "Name", "SubjectCode" },
                values: new object[] { "Tiếng Việt – Đọc, viết, nghe, nói và kiến thức ngôn ngữ", 3, "Literature", "literature" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000019"),
                columns: new[] { "Description", "Grade", "Name", "SubjectCode" },
                values: new object[] { "Tiếng Anh – Ngôn ngữ và giao tiếp quốc tế", 3, "English", "english" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000020"),
                columns: new[] { "Description", "Grade", "Name", "SubjectCode" },
                values: new object[] { "Đạo đức – Giáo dục giá trị sống và kỹ năng ứng xử", 3, "Ethics", "ethics" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000021"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Tự nhiên và Xã hội – Khám phá thế giới tự nhiên và đời sống xã hội", "Nature & Society", "nature_and_society" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000022"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Mĩ thuật – Nghệ thuật tạo hình và thẩm mỹ", "Art", "art" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000023"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Âm nhạc – Lý thuyết âm nhạc và biểu diễn", "Music", "music" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000024"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Hoạt động trải nghiệm – Học tập thực hành và kỹ năng sống", "Experiential Activities", "experiential_activities" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000025"),
                columns: new[] { "Description", "SubjectCode" },
                values: new object[] { "Tin học – Khoa học máy tính và công nghệ số", "information_technology" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000026"),
                column: "Description",
                value: "Công nghệ – Khoa học ứng dụng và kỹ thuật");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000027"),
                columns: new[] { "Description", "Grade", "Name", "SubjectCode" },
                values: new object[] { "Toán học – Nghiên cứu về số, phép tính và hình học", 4, "Mathematics", "math" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000028"),
                columns: new[] { "Description", "Grade", "Name", "SubjectCode" },
                values: new object[] { "Tiếng Việt – Đọc, viết, nghe, nói và kiến thức ngôn ngữ", 4, "Literature", "literature" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000029"),
                columns: new[] { "Description", "Grade", "Name", "SubjectCode" },
                values: new object[] { "Tiếng Anh – Ngôn ngữ và giao tiếp quốc tế", 4, "English", "english" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000030"),
                columns: new[] { "Description", "Grade", "Name", "SubjectCode" },
                values: new object[] { "Đạo đức – Giáo dục giá trị sống và kỹ năng ứng xử", 4, "Ethics", "ethics" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000031"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Khoa học – Khám phá thế giới tự nhiên và hiện tượng vật lý", "Science", "science" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000032"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Mĩ thuật – Nghệ thuật tạo hình và thẩm mỹ", "Art", "art" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000033"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Âm nhạc – Lý thuyết âm nhạc và biểu diễn", "Music", "music" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000034"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Hoạt động trải nghiệm – Học tập thực hành và kỹ năng sống", "Experiential Activities", "experiential_activities" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000035"),
                columns: new[] { "Description", "SubjectCode" },
                values: new object[] { "Tin học – Khoa học máy tính và công nghệ số", "information_technology" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000036"),
                column: "Description",
                value: "Công nghệ – Khoa học ứng dụng và kỹ thuật");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000037"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Lịch sử và Địa lí – Nghiên cứu lịch sử và địa lý Việt Nam", "History & Geography", "history_geography" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000038"),
                columns: new[] { "Description", "Grade", "Name", "SubjectCode" },
                values: new object[] { "Toán học – Nghiên cứu về số, phép tính và hình học", 5, "Mathematics", "math" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000039"),
                columns: new[] { "Description", "Grade", "Name", "SubjectCode" },
                values: new object[] { "Tiếng Việt – Đọc, viết, nghe, nói và kiến thức ngôn ngữ", 5, "Literature", "literature" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000040"),
                columns: new[] { "Description", "Grade", "Name", "SubjectCode" },
                values: new object[] { "Tiếng Anh – Ngôn ngữ và giao tiếp quốc tế", 5, "English", "english" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000041"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Đạo đức – Giáo dục giá trị sống và kỹ năng ứng xử", "Ethics", "ethics" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000042"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Khoa học – Khám phá thế giới tự nhiên và hiện tượng vật lý", "Science", "science" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000043"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Mĩ thuật – Nghệ thuật tạo hình và thẩm mỹ", "Art", "art" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000044"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Âm nhạc – Lý thuyết âm nhạc và biểu diễn", "Music", "music" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000045"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Hoạt động trải nghiệm – Học tập thực hành và kỹ năng sống", "Experiential Activities", "experiential_activities" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000046"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Tin học – Khoa học máy tính và công nghệ số", "Information Technology", "information_technology" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000047"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Công nghệ – Khoa học ứng dụng và kỹ thuật", "Technology", "technology" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000048"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Lịch sử và Địa lí – Nghiên cứu lịch sử và địa lý Việt Nam", "History & Geography", "history_geography" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"),
                columns: new[] { "Description", "Title" },
                values: new object[] { "Nhận biết, đọc, viết, đếm xuôi ngược các số 0-10. So sánh bằng >, <, =. Cấu tạo số (tách-gộp).", "Các số từ 0 đến 10" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"),
                columns: new[] { "Description", "Title" },
                values: new object[] { "Nhận biết hình vuông, hình tròn, hình tam giác, hình chữ nhật. Thực hành lắp ghép, xếp hình.", "Làm quen với một số hình phẳng" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"),
                columns: new[] { "Description", "Title" },
                values: new object[] { "Phép cộng (gộp lại), phép trừ (bớt đi) trong phạm vi 10. Bảng cộng trừ, mối quan hệ cộng-trừ.", "Phép cộng, phép trừ trong phạm vi 10" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"),
                columns: new[] { "Description", "Title" },
                values: new object[] { "Khối lập phương, khối hộp chữ nhật. Vị trí và định hướng trong không gian: trên-dưới, phải-trái, trước-sau.", "Làm quen với một số hình khối" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000005"),
                columns: new[] { "Description", "Title" },
                values: new object[] { "Ôn tập: đếm, đọc, viết số và so sánh số đến 10; cộng trừ phạm vi 10; nhận biết hình phẳng, hình khối và vị trí.", "Ôn tập học kì 1" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000006"),
                columns: new[] { "Description", "Title" },
                values: new object[] { "Số có hai chữ số, hệ thập phân (chục và đơn vị). So sánh, sắp xếp số đến 100.", "Các số đến 100" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000007"),
                columns: new[] { "Description", "Title" },
                values: new object[] { "Đơn vị xăng-ti-mét (cm). Sử dụng thước kẻ để đo và vẽ đoạn thẳng.", "Độ dài và đo độ dài" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000008"),
                columns: new[] { "Description", "Title" },
                values: new object[] { "Phép cộng, trừ không nhớ trong phạm vi 100. Cộng trừ dọc.", "Cộng, trừ trong phạm vi 100" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000009"),
                columns: new[] { "Description", "Title" },
                values: new object[] { "Đọc giờ đúng trên đồng hồ. Ngày trong tuần, tờ lịch. Thời gian sinh hoạt hàng ngày.", "Thời gian, giờ và lịch" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000010"),
                columns: new[] { "Description", "Title" },
                values: new object[] { "Ôn tập tổng hợp: số đến 100, cộng trừ phạm vi 100, đo độ dài, đọc giờ, hình phẳng và hình khối.", "Ôn tập cuối năm" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000011"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Phần 1: 25 bài học âm, chữ cái Tiếng Việt. Nhận diện mặt chữ, phát âm, tập viết.", new Guid("00000000-0000-0000-0000-000000000002"), "Học âm và chữ cái" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000012"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Phần 2: Học vần trơn (ai, oi, ui...), vần có âm cuối (an, on, un, at, ot...). Ghép vần tạo từ.", new Guid("00000000-0000-0000-0000-000000000002"), "Học vần" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000013"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Phần 3: Đọc bài khóa ngắn, kể chuyện theo tranh. Luyện kỹ năng nghe-nói-đọc-viết.", new Guid("00000000-0000-0000-0000-000000000002"), "Kể chuyện và luyện đọc" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000014"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Chủ điểm 1 Tập 2: 4 bài đọc về tình bạn, sẻ chia, giúp đỡ nhau.", new Guid("00000000-0000-0000-0000-000000000002"), "Tôi và các bạn" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000015"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Chủ điểm 2 Tập 2: 4 bài đọc về gia đình, yêu thương, chăm sóc lẫn nhau.", new Guid("00000000-0000-0000-0000-000000000002"), "Mái ấm gia đình" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000016"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Chủ điểm 3 Tập 2: 4 bài đọc về trường học, thầy cô, bạn bè.", new Guid("00000000-0000-0000-0000-000000000002"), "Mái trường thân yêu" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000017"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Chủ điểm 4 Tập 2: 5 bài đọc về thiên nhiên, động vật, thực vật.", new Guid("00000000-0000-0000-0000-000000000002"), "Thiên nhiên quanh ta" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000018"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Chủ điểm 5 Tập 2: 5 bài đọc về quê hương, đất nước, danh lam thắng cảnh.", new Guid("00000000-0000-0000-0000-000000000002"), "Đất nước và con người" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000019"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Phonics: /b/, /k/, /æ/, /d/. Từ vựng: bear, cat, apple, dog. Mẫu câu: This is a bear. I have an apple.", new Guid("00000000-0000-0000-0000-000000000003"), "Unit 1-4: Letters Bb, Cc, Aa, Dd" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000020"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Phonics: /f/, /g/, /h/, /ɪ/. Từ vựng: fish, goat, hat, igloo. Mẫu câu: I like fish. Here's a hat.", new Guid("00000000-0000-0000-0000-000000000003"), "Unit 5-8: Letters Ff, Gg, Hh, Ii" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000021"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Ôn tập tổng hợp các âm A-I, từ vựng và mẫu câu đã học.", new Guid("00000000-0000-0000-0000-000000000003"), "Review 1 (Units 1-8)" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000022"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Phonics: /dʒ/, /k/, /l/, /m/. Từ vựng: jelly, kite, lion, monkey. Mẫu câu cơ bản.", new Guid("00000000-0000-0000-0000-000000000003"), "Unit 9-12: Letters Jj, Kk, Ll, Mm" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000023"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Phonics: /n/, /ɒ/, /p/, /kw/. Từ vựng: nest, orange, pen, queen.", new Guid("00000000-0000-0000-0000-000000000003"), "Unit 13-16: Letters Nn, Oo, Pp, Qq" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000024"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Ôn tập tổng hợp các âm J-Q, từ vựng và mẫu câu đã học.", new Guid("00000000-0000-0000-0000-000000000003"), "Review 2 (Units 9-16)" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000025"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Bài 1-2: Nhận biết sự quan tâm, chăm sóc của các thành viên. Thể hiện tình yêu thương gia đình.", new Guid("00000000-0000-0000-0000-000000000004"), "Yêu thương gia đình" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000026"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Bài 3: Nhận biết các việc cá nhân cần tự làm. Rèn tính tự giác trong sinh hoạt hàng ngày.", new Guid("00000000-0000-0000-0000-000000000004"), "Tự giác làm việc của mình" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000027"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Bài 4: Ý nghĩa của sự thật thà. Trung thực trong học tập và cuộc sống.", new Guid("00000000-0000-0000-0000-000000000004"), "Thật thà" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000028"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Bài 5-6: Giữ gìn đồ dùng cá nhân và đồ dùng chung. Sắp xếp ngăn nắp.", new Guid("00000000-0000-0000-0000-000000000004"), "Giữ gìn đồ dùng" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000029"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Bài 7: Biết quan tâm, giúp đỡ láng giềng. Lễ phép, thân thiện với mọi người.", new Guid("00000000-0000-0000-0000-000000000004"), "Quan tâm hàng xóm" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000030"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Bài 8-9: Nhận diện nguy hiểm, phòng tránh tai nạn thương tích. An toàn ở nhà và ở trường.", new Guid("00000000-0000-0000-0000-000000000004"), "Phòng tránh tai nạn" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000031"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Bài 10-11: Thực hiện nếp sống văn minh, đúng giờ, gọn gàng ngăn nắp.", new Guid("00000000-0000-0000-0000-000000000004"), "Sinh hoạt nền nếp" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000032"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Bài 1-4: Các thành viên trong gia đình, ngôi nhà, an toàn khi ở nhà.", new Guid("00000000-0000-0000-0000-000000000005"), "Gia đình" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000033"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Bài 5-8: Đồ dùng học tập, các phòng trong trường, giữ sạch trường lớp.", new Guid("00000000-0000-0000-0000-000000000005"), "Trường học" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000034"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Bài 9-11: Đường đi an toàn, phương tiện giao thông, nơi công cộng.", new Guid("00000000-0000-0000-0000-000000000005"), "Cộng đồng địa phương" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000035"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Bài 12-14: Nhận biết cây cối, con vật xung quanh. Bảo vệ cây xanh, động vật.", new Guid("00000000-0000-0000-0000-000000000005"), "Thực vật và động vật" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000036"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Bài 15-17: Các bộ phận cơ thể, giữ vệ sinh, ăn uống đủ chất.", new Guid("00000000-0000-0000-0000-000000000005"), "Con người và sức khỏe" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000037"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Bài 18-19: Thời tiết (nắng, mưa, gió), bầu trời ngày và đêm.", new Guid("00000000-0000-0000-0000-000000000005"), "Trái Đất và bầu trời" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000038"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Bài 1-2: Làm quen với đồ dùng mĩ thuật, các hoạt động mĩ thuật cơ bản.", new Guid("00000000-0000-0000-0000-000000000006"), "Mĩ thuật trong nhà trường" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000039"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Bài 3-4: Nhận biết chấm trong tự nhiên và nghệ thuật. Tạo hình từ chấm.", new Guid("00000000-0000-0000-0000-000000000006"), "Sự thú vị đến từ những chấm" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000040"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Bài 5-6: Các loại đường nét (thẳng, cong, gấp khúc). Sáng tạo với đường nét.", new Guid("00000000-0000-0000-0000-000000000006"), "Sự quyến rũ của đường nét" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000041"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Bài 7-8: Hình phẳng và khối 3D cơ bản. Tạo sản phẩm từ hình khối.", new Guid("00000000-0000-0000-0000-000000000006"), "Sáng tạo với hình và khối" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000042"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Bài 9-10: Nhận biết 3 màu cơ bản (Đỏ, Vàng, Xanh). Vẽ tranh với màu sắc.", new Guid("00000000-0000-0000-0000-000000000006"), "Màu sắc quanh em" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000043"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Bài 11-12: Mĩ thuật trong đời sống, vật liệu tái chế, trưng bày tác phẩm.", new Guid("00000000-0000-0000-0000-000000000006"), "Thế giới mĩ thuật quanh em" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000044"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Làm quen nốt Đô-Rê-Mi. Phân biệt âm thanh cao-thấp, mạnh-nhẹ.", new Guid("00000000-0000-0000-0000-000000000007"), "Âm thanh kì diệu" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000045"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Trống: loại nhạc cụ gõ. Quốc ca Việt Nam. Cảm nhận nhịp hành khúc.", new Guid("00000000-0000-0000-0000-000000000007"), "Việt Nam yêu thương" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000046"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Kí hiệu tay nốt Đô-Rê-Mi. Bài hát về trường lớp.", new Guid("00000000-0000-0000-0000-000000000007"), "Mái trường thân yêu" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000047"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Thưởng thức: Hồ Thiên Nga (Tchaikovsky). Hát và vận động theo nhạc.", new Guid("00000000-0000-0000-0000-000000000007"), "Vòng tay bè bạn" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000048"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Nốt Pha, Son. Nhạc phẩm thiếu nhi Mozart. Nhịp 2/4.", new Guid("00000000-0000-0000-0000-000000000007"), "Nhịp điệu mùa xuân" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000049"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Dân ca Việt Nam. Nhạc cụ dân tộc: thanh phách.", new Guid("00000000-0000-0000-0000-000000000007"), "Về miền dân ca" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000050"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Thang 5 âm (Đô-Rê-Mi-Pha-Son). Bài hát về gia đình.", new Guid("00000000-0000-0000-0000-000000000007"), "Gia đình yêu thương" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000051"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Nhạc cụ: kèn Triangle. Ôn tập tổng hợp cuối năm.", new Guid("00000000-0000-0000-0000-000000000007"), "Vui đón hè" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000052"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Tuần 1-4: Làm quen trường mới, bạn mới, thầy cô. Nội quy trường học.", new Guid("00000000-0000-0000-0000-000000000008"), "Em và trường tiểu học" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000053"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Tuần 5-8: Nhận biết đặc điểm, sở thích cá nhân. Tự tin giới thiệu bản thân.", new Guid("00000000-0000-0000-0000-000000000008"), "Khám phá bản thân" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000054"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Tuần 9-12: Tri ân thầy cô nhân ngày 20/11. Làm thiệp, tập biểu diễn.", new Guid("00000000-0000-0000-0000-000000000008"), "Mừng ngày nhà giáo" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000055"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Tuần 13-16: Tự chuẩn bị sách vở, vệ sinh cá nhân, ăn uống đúng cách.", new Guid("00000000-0000-0000-0000-000000000008"), "Tự phục vụ bản thân" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000056"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Tuần 17-20: Tình cảm gia đình, phong tục Tết, trò chơi dân gian.", new Guid("00000000-0000-0000-0000-000000000008"), "Gia đình yêu thương và Tết" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000057"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Tuần 21-24: Hàng xóm láng giềng, nghề nghiệp trong cộng đồng.", new Guid("00000000-0000-0000-0000-000000000008"), "Em với cộng đồng" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000058"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Tuần 25-28: Giữ gìn vệ sinh, bảo vệ cây xanh, tiết kiệm nước.", new Guid("00000000-0000-0000-0000-000000000008"), "Em với môi trường" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000059"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Tuần 29-32: An toàn giao thông, phòng cháy, đuối nước.", new Guid("00000000-0000-0000-0000-000000000008"), "Phòng tránh tai nạn" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000060"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Tuần 33-35: Ôn tập, trưng bày sản phẩm, liên hoan cuối năm.", new Guid("00000000-0000-0000-0000-000000000008"), "Tổng kết năm học" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000061"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Ôn tập số đến 100, cộng trừ không nhớ trong phạm vi 100. Bổ sung kiến thức.", new Guid("00000000-0000-0000-0000-000000000009"), "Ôn tập và bổ sung - Số đến 100" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000062"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Cộng trừ có nhớ qua 10. Ví dụ: 9+5=14, 14-6=8.", new Guid("00000000-0000-0000-0000-000000000009"), "Phép cộng, phép trừ qua 10 trong phạm vi 20" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000063"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Nhận biết khối trụ (lon nước), khối cầu (quả bóng). Phân loại hình khối.", new Guid("00000000-0000-0000-0000-000000000009"), "Làm quen với khối trụ, khối cầu" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000064"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Cộng trừ có nhớ dạng cột dọc. Ví dụ: 47+25=72, 63-28=35.", new Guid("00000000-0000-0000-0000-000000000009"), "Phép cộng, phép trừ có nhớ trong phạm vi 100" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000065"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Nhận biết và vẽ đường thẳng, đoạn thẳng, đường gấp khúc, ba điểm thẳng hàng.", new Guid("00000000-0000-0000-0000-000000000009"), "Hình phẳng - Đường thẳng, đoạn thẳng, đường gấp khúc" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000066"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Đọc giờ đúng, giờ rưỡi. Ngày trong tuần, tháng trong năm.", new Guid("00000000-0000-0000-0000-000000000009"), "Thời gian - Giờ, ngày, tháng" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000067"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Ôn tập: cộng trừ có nhớ phạm vi 100, nhận biết khối trụ/cầu, đường thẳng, giờ.", new Guid("00000000-0000-0000-0000-000000000009"), "Ôn tập học kì 1" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000068"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Ý nghĩa phép nhân (gom nhóm bằng nhau), phép chia (chia đều). Bảng nhân chia 2, 3, 4, 5.", new Guid("00000000-0000-0000-0000-000000000009"), "Phép nhân và phép chia" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000069"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Số có 3 chữ số: hàng trăm, hàng chục, hàng đơn vị. Đọc, viết, so sánh.", new Guid("00000000-0000-0000-0000-000000000009"), "Các số đến 1000" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000070"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Mét (m), ki-lô-mét (km), mi-li-mét (mm). Ki-lô-gam (kg). Ước lượng độ dài.", new Guid("00000000-0000-0000-0000-000000000009"), "Độ dài và khối lượng" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000071"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Cộng trừ có nhớ trong phạm vi 1000. Tính nhẩm, tính viết.", new Guid("00000000-0000-0000-0000-000000000009"), "Phép cộng, phép trừ trong phạm vi 1000" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000072"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Thu thập dữ liệu, biểu đồ tranh đơn giản. Chắc chắn, có thể, không thể.", new Guid("00000000-0000-0000-0000-000000000009"), "Thống kê và xác suất" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000073"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Ôn tập tổng hợp: số đến 1000, 4 phép tính, đo lường, thống kê, hình học.", new Guid("00000000-0000-0000-0000-000000000009"), "Ôn tập cuối năm" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000074"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Chủ điểm 1 Tập 1: Đọc hiểu văn bản về tuổi thơ, tự hào bản thân. Tập viết chữ hoa A-E.", new Guid("00000000-0000-0000-0000-000000000010"), "Em là búp măng non" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000075"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Chủ điểm 2 Tập 1: Văn bản về ngày đầu đến trường, niềm vui học tập. Chữ hoa F-H.", new Guid("00000000-0000-0000-0000-000000000010"), "Em đi học" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000076"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Chủ điểm 3 Tập 1: Đọc về tình bạn, sẻ chia. Chữ hoa I-L.", new Guid("00000000-0000-0000-0000-000000000010"), "Niềm vui và tình bạn" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000077"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Chủ điểm 4 Tập 1: Gia đình, ông bà, cha mẹ. Chữ hoa M.", new Guid("00000000-0000-0000-0000-000000000010"), "Mái ấm gia đình" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000078"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Ôn tập các chủ điểm 1-4, kiểm tra đọc hiểu, chính tả, viết đoạn văn.", new Guid("00000000-0000-0000-0000-000000000010"), "Ôn tập học kì 1" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000079"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Chủ điểm 1 Tập 2: Thiên nhiên, cảnh vật đẹp. Chữ hoa N-P.", new Guid("00000000-0000-0000-0000-000000000010"), "Vẻ đẹp quanh em" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000080"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Chủ điểm 2 Tập 2: Bảo vệ môi trường, yêu thiên nhiên. Chữ hoa Q-S.", new Guid("00000000-0000-0000-0000-000000000010"), "Hành tinh xanh của em" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000081"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Chủ điểm 3 Tập 2: Truyền thống, phẩm chất người Việt. Chữ hoa T-V.", new Guid("00000000-0000-0000-0000-000000000010"), "Con người Việt Nam" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000082"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Chủ điểm 4 Tập 2: Quê hương đất nước. Chữ hoa X-Y.", new Guid("00000000-0000-0000-0000-000000000010"), "Quê hương em" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000083"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Chủ điểm 5 Tập 2: Mở rộng hiểu biết về thế giới. Tốc độ đọc 80-100 tiếng/phút.", new Guid("00000000-0000-0000-0000-000000000010"), "Thế giới trong mắt em" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000084"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Phonics: /p/, /r/, /s/, /t/, /ʌ/. Từ vựng: popcorn, riding, sea, tea, bus. Mẫu câu cơ bản.", new Guid("00000000-0000-0000-0000-000000000011"), "Unit 1-5: Letters Pp-Uu" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000085"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Ôn tập tổng hợp các âm P-U, từ vựng và mẫu câu đã học.", new Guid("00000000-0000-0000-0000-000000000011"), "Review 1 (Units 1-5)" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000086"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Phonics: /v/, /w/, /ks/, /j/, /z/. Từ vựng: van, water, fox, yams, zebra.", new Guid("00000000-0000-0000-0000-000000000011"), "Unit 6-10: Letters Vv-Zz" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000087"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Ôn tập tổng hợp các âm V-Z, từ vựng và mẫu câu đã học.", new Guid("00000000-0000-0000-0000-000000000011"), "Review 2 (Units 6-10)" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000088"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Phonics nâng cao: i_e /aɪ/, oa /əʊ/, short i /ɪ/, short o /ɒ/, ui/ou /uː/, a_e /eɪ/. Present continuous, Would you like...?", new Guid("00000000-0000-0000-0000-000000000011"), "Unit 11-16: Diphthongs & Digraphs" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000089"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Ôn tập tổng hợp diphthongs, digraphs, mẫu câu nâng cao.", new Guid("00000000-0000-0000-0000-000000000011"), "Review 3 (Units 11-16)" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000090"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Bài 1: Nhận thức thời gian là vô giá. Quản lý thời gian, lập thời gian biểu.", new Guid("00000000-0000-0000-0000-000000000012"), "Quý trọng thời gian" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000091"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Bài 2: Dũng cảm nhận lỗi, sửa sai. Trung thực và trách nhiệm.", new Guid("00000000-0000-0000-0000-000000000012"), "Nhận lỗi và sửa lỗi" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000092"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Bài 3-4: Giữ gìn, sắp xếp đồ dùng cá nhân và gia đình.", new Guid("00000000-0000-0000-0000-000000000012"), "Bảo quản đồ dùng cá nhân và gia đình" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000093"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Bài 5-6: Văn hóa chào hỏi, cây tình bạn, sẻ chia khi bạn gặp khó khăn.", new Guid("00000000-0000-0000-0000-000000000012"), "Kính trọng thầy cô và yêu quý bạn bè" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000094"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Bài 7: Tôn trọng mọi nghề nghiệp, trân quý thành quả lao động.", new Guid("00000000-0000-0000-0000-000000000012"), "Quý trọng lao động" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000095"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Bài 8: Bảo vệ môi trường, lối sống xanh.", new Guid("00000000-0000-0000-0000-000000000012"), "Gìn giữ cảnh quan thiên nhiên" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000096"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Bài 9: Xếp hàng, giữ trật tự, văn hóa ứng xử nơi công cộng.", new Guid("00000000-0000-0000-0000-000000000012"), "Tuân thủ quy định nơi công cộng" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000097"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Bài 10: Nhận diện nguy hiểm, số điện thoại khẩn cấp (113, 114, 115), thoát hiểm.", new Guid("00000000-0000-0000-0000-000000000012"), "Phòng tránh tai nạn, thương tích" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000098"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Nghề nghiệp người thân, phòng ngộ độc, an toàn khi sử dụng đồ dùng trong nhà.", new Guid("00000000-0000-0000-0000-000000000013"), "Gia đình" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000099"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Khu vực trường, phòng chức năng, sinh hoạt lớp, vệ sinh trường lớp.", new Guid("00000000-0000-0000-0000-000000000013"), "Trường học" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000100"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Di tích lịch sử địa phương, an toàn giao thông, giữ gìn cảnh quan.", new Guid("00000000-0000-0000-0000-000000000013"), "Cộng đồng" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000101"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Các bộ phận cây (rễ, thân, lá). Môi trường sống động vật. Bảo vệ sinh vật.", new Guid("00000000-0000-0000-0000-000000000013"), "Thực vật và động vật" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000102"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Cơ quan vận động (cơ, xương), tiêu hóa, bài tiết. Phòng bệnh.", new Guid("00000000-0000-0000-0000-000000000013"), "Con người và sức khỏe" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000103"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Các mùa trong năm, hiện tượng thời tiết, thiên tai và cách phòng tránh.", new Guid("00000000-0000-0000-0000-000000000013"), "Trái Đất và bầu trời" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000104"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Bài 1-2: Đồ dùng mĩ thuật, pha trộn màu cơ bản tạo màu nhị hợp (cam, lục, tím).", new Guid("00000000-0000-0000-0000-000000000014"), "Mĩ thuật trong nhà trường" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000105"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Bài 3-4: Khối lập phương, khối cầu, khối trụ. Mô hình đồ vật từ hình khối.", new Guid("00000000-0000-0000-0000-000000000014"), "Sự thú vị đến từ những hình khối" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000106"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Bài 5-6: Nét thẳng, cong, gấp khúc, xoắn ốc. Họa tiết trang trí.", new Guid("00000000-0000-0000-0000-000000000014"), "Sự kì diệu của đường nét" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000107"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Bài 7-8: Kỹ thuật xoay, nặn, ấn dẹt, ghép dính. Tạo hình con vật.", new Guid("00000000-0000-0000-0000-000000000014"), "Sáng tạo với đất nặn" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000108"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Bài 9-10: Vẽ chân dung người thân, tranh sinh hoạt vui chơi.", new Guid("00000000-0000-0000-0000-000000000014"), "Gia đình và bạn bè" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000109"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Bài 11-12: In độc bản từ lá cây, cắt dán tranh vườn hoa.", new Guid("00000000-0000-0000-0000-000000000014"), "Thiên nhiên xanh" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000110"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Bài 13-14: Tranh Gà đàn, Lợn ăn lá ráy, Đám cưới chuột. Đồ chơi dân gian.", new Guid("00000000-0000-0000-0000-000000000014"), "Tìm hiểu mĩ thuật - Tranh dân gian Đông Hồ" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000111"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Bài 15-16: Vẽ phương tiện giao thông, tranh phong cảnh quê hương.", new Guid("00000000-0000-0000-0000-000000000014"), "Giao thông và quê hương" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000112"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Hát bài về ngày mới. Vỗ tay theo nhịp. Làm quen nhạc cụ Body Percussion.", new Guid("00000000-0000-0000-0000-000000000015"), "Rộn ràng ngày mới" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000113"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Hát bài về tình bạn. Nhạc cụ Maracas. Chơi trò chơi âm nhạc.", new Guid("00000000-0000-0000-0000-000000000015"), "Nhịp điệu bạn bè" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000114"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Hát bài về trường lớp. Thực hành Body Percussion nâng cao.", new Guid("00000000-0000-0000-0000-000000000015"), "Vui đến trường" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000115"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Hát bài về đoàn kết. Nghe nhạc cổ điển. Vận động theo nhạc.", new Guid("00000000-0000-0000-0000-000000000015"), "Đoàn kết yêu thương" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000116"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Hát bài về mùa xuân, Tết. Nghe nhạc truyền thống.", new Guid("00000000-0000-0000-0000-000000000015"), "Mùa xuân" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000117"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Hát bài về gia đình. Ôn tập nốt nhạc. Biểu diễn nhóm.", new Guid("00000000-0000-0000-0000-000000000015"), "Gia đình" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000118"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Hát bài về thiên nhiên. Nhận biết nhịp 2/4. Sáng tạo âm nhạc.", new Guid("00000000-0000-0000-0000-000000000015"), "Thiên nhiên" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000119"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Ôn tập tổng kết năm học. Biểu diễn, liên hoan âm nhạc.", new Guid("00000000-0000-0000-0000-000000000015"), "Vui đón hè" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000120"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Tuần 1-4: Hình dáng, sở thích, cảm xúc, sự thay đổi so với lớp 1.", new Guid("00000000-0000-0000-0000-000000000016"), "Khám phá bản thân" });

            migrationBuilder.InsertData(
                schema: "public",
                table: "Topics",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Description", "IsCurriculum", "SubjectId", "Title", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000121"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Tuần 5-8: Ngăn nắp, quý trọng thời gian, tự chăm sóc, an toàn.", true, new Guid("00000000-0000-0000-0000-000000000016"), "Rèn luyện bản thân", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000122"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Tuần 9-12: Truyền thống nhà trường, vệ sinh, xây dựng tình bạn, biết ơn thầy cô.", true, new Guid("00000000-0000-0000-0000-000000000016"), "Xây dựng nhà trường", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000123"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Tuần 13-15: Gia đình yêu thương, quan tâm chăm sóc, truyền thống gia đình.", true, new Guid("00000000-0000-0000-0000-000000000016"), "Em với gia đình", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000124"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Tuần 16-18: Hàng xóm, lễ hội địa phương, ngày Tết.", true, new Guid("00000000-0000-0000-0000-000000000016"), "Em với cộng đồng", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000125"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Tuần 19-21: Bảo vệ môi trường, tiết kiệm năng lượng, trồng cây.", true, new Guid("00000000-0000-0000-0000-000000000016"), "Em với môi trường", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000126"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Tuần 22-24: Tìm hiểu nghề nghiệp, đóng vai, ước mơ nghề nghiệp.", true, new Guid("00000000-0000-0000-0000-000000000016"), "Nghề nghiệp quanh em", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000127"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn tập số đến 1000, cộng trừ có nhớ. Bổ sung kiến thức lớp 2.", true, new Guid("00000000-0000-0000-0000-000000000017"), "Ôn tập và bổ sung - Số đến 1000", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000128"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bảng nhân chia 6, 7, 8, 9. Nhân chia trong phạm vi 1000.", true, new Guid("00000000-0000-0000-0000-000000000017"), "Bảng nhân và bảng chia", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000129"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Góc vuông, góc không vuông. Hình tròn, tâm, bán kính. Đơn vị: mm, g, ml.", true, new Guid("00000000-0000-0000-0000-000000000017"), "Hình học và đo lường", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000130"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Nhân số có 2-3 chữ số với số có 1 chữ số. Chia số có 2-3 chữ số cho số có 1 chữ số.", true, new Guid("00000000-0000-0000-0000-000000000017"), "Phép nhân và phép chia trong phạm vi 1000", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000131"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bảng số liệu, biểu đồ tranh. Sự kiện chắc chắn, có thể, không thể.", true, new Guid("00000000-0000-0000-0000-000000000017"), "Thống kê và xác suất", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000132"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn tập: bảng nhân chia, hình học, đo lường, thống kê.", true, new Guid("00000000-0000-0000-0000-000000000017"), "Ôn tập học kì 1", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000133"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Số có 4-5 chữ số: hàng nghìn, hàng chục nghìn. Đọc, viết, so sánh.", true, new Guid("00000000-0000-0000-0000-000000000017"), "Các số đến 100 000", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000134"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Cộng trừ số có nhiều chữ số. Tính giá trị biểu thức.", true, new Guid("00000000-0000-0000-0000-000000000017"), "Phép cộng, phép trừ trong phạm vi 100 000", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000135"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Chu vi, diện tích hình chữ nhật, hình vuông. Công thức tính.", true, new Guid("00000000-0000-0000-0000-000000000017"), "Chu vi và diện tích", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000136"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Nhân chia số có nhiều chữ số. Bài toán giải bằng hai phép tính.", true, new Guid("00000000-0000-0000-0000-000000000017"), "Phép nhân, phép chia trong phạm vi 100 000", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000137"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Các loại tiền Việt Nam. Tháng, năm, thế kỷ. Bài toán liên quan đến tiền.", true, new Guid("00000000-0000-0000-0000-000000000017"), "Tiền Việt Nam và thời gian", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000138"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn tập tổng hợp: số đến 100 000, 4 phép tính, chu vi-diện tích, tiền, thời gian.", true, new Guid("00000000-0000-0000-0000-000000000017"), "Ôn tập cuối năm", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000139"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Chủ điểm 1 Tập 1: Đọc hiểu về du lịch, trải nghiệm. Tốc độ đọc 70-80 tiếng/phút.", true, new Guid("00000000-0000-0000-0000-000000000018"), "Trải nghiệm thú vị", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000140"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Chủ điểm 2 Tập 1: Trường lớp, thầy cô, bạn bè.", true, new Guid("00000000-0000-0000-0000-000000000018"), "Cổng trường rộng mở", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000141"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Chủ điểm 3 Tập 1: Gia đình, yêu thương, chăm sóc.", true, new Guid("00000000-0000-0000-0000-000000000018"), "Mái ấm gia đình", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000142"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Chủ điểm 4 Tập 1: Cộng đồng, xã hội, đoàn kết.", true, new Guid("00000000-0000-0000-0000-000000000018"), "Mái nhà chung", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000143"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Chủ điểm 5 Tập 1: Truyện cổ tích, truyện ngụ ngôn.", true, new Guid("00000000-0000-0000-0000-000000000018"), "Những câu chuyện thú vị", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000144"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn tập: đọc hiểu, viết đoạn văn ngắn, chính tả, từ vựng HK1.", true, new Guid("00000000-0000-0000-0000-000000000018"), "Ôn tập học kì 1", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000145"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Chủ điểm 1 Tập 2: Các loại hình nghệ thuật, sáng tạo.", true, new Guid("00000000-0000-0000-0000-000000000018"), "Nghệ thuật", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000146"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Chủ điểm 2 Tập 2: Cảnh đẹp quê hương, phong tục.", true, new Guid("00000000-0000-0000-0000-000000000018"), "Quê hương", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000147"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Chủ điểm 3 Tập 2: Lịch sử, truyền thống dân tộc.", true, new Guid("00000000-0000-0000-0000-000000000018"), "Đất nước ngàn năm", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000148"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Chủ điểm 4 Tập 2: Bảo vệ môi trường, thiên nhiên.", true, new Guid("00000000-0000-0000-0000-000000000018"), "Trái đất xanh", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000149"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Chủ điểm 5 Tập 2: Mở rộng hiểu biết về thế giới.", true, new Guid("00000000-0000-0000-0000-000000000018"), "Thế giới trong mắt em", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000150"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Hello/Goodbye, school things (pen, book, ruler), colours (red, blue, green), numbers 1-10.", true, new Guid("00000000-0000-0000-0000-000000000019"), "Unit 1-5: Greetings, School, Colours", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000151"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Family members, jobs (teacher, doctor), body parts, pets. This is my..., He/She is a...", true, new Guid("00000000-0000-0000-0000-000000000019"), "Unit 6-10: Family, Jobs, Body", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000152"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn tập từ vựng và mẫu câu chủ đề trường học, gia đình, nghề nghiệp.", true, new Guid("00000000-0000-0000-0000-000000000019"), "Review 1 (Units 1-10)", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000153"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Rooms in house, toys, outdoor activities. Where is...? I have... Do you like...?", true, new Guid("00000000-0000-0000-0000-000000000019"), "Unit 11-15: House, Toys, Activities", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000154"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Zoo animals, food, clothes, weather. What's the weather like? I'm wearing...", true, new Guid("00000000-0000-0000-0000-000000000019"), "Unit 16-20: Animals, Food, Clothes", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000155"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn tập tổng hợp từ vựng, mẫu câu, phonics toàn bộ 20 unit.", true, new Guid("00000000-0000-0000-0000-000000000019"), "Review 2 (Units 11-20)", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000156"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bài 1: Nhận diện biển báo, luật giao thông cơ bản, qua đường an toàn.", true, new Guid("00000000-0000-0000-0000-000000000020"), "An toàn giao thông", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000157"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bài 2: Tự hào về đất nước, cờ Tổ quốc, Quốc ca.", true, new Guid("00000000-0000-0000-0000-000000000020"), "Yêu Tổ quốc Việt Nam", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000158"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bài 3: Chăm chỉ học tập, ham tìm hiểu kiến thức mới.", true, new Guid("00000000-0000-0000-0000-000000000020"), "Ham học hỏi", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000159"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bài 4: Giá trị của lời hứa, trách nhiệm với lời nói.", true, new Guid("00000000-0000-0000-0000-000000000020"), "Giữ lời hứa", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000160"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bài 5: Giữ gìn và phát huy truyền thống tốt đẹp của gia đình.", true, new Guid("00000000-0000-0000-0000-000000000020"), "Truyền thống gia đình", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000161"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bài 6: Tình làng nghĩa xóm, giúp đỡ láng giềng.", true, new Guid("00000000-0000-0000-0000-000000000020"), "Quan tâm hàng xóm, láng giềng", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000162"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bài 7: Đồng cảm với người khác, biết chia sẻ, giúp đỡ.", true, new Guid("00000000-0000-0000-0000-000000000020"), "Thấu hiểu và chia sẻ", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000163"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bài 8: Tôn trọng sự khác biệt về ngoại hình, dân tộc, vùng miền.", true, new Guid("00000000-0000-0000-0000-000000000020"), "Tôn trọng sự khác biệt", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000164"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bài 9-10: Nhận biết và kiểm soát cảm xúc giận dữ, buồn bã.", true, new Guid("00000000-0000-0000-0000-000000000020"), "Kiềm chế cảm xúc tiêu cực", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000165"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Họ hàng, quan hệ gia đình mở rộng, phòng cháy chữa cháy trong gia đình.", true, new Guid("00000000-0000-0000-0000-000000000021"), "Gia đình", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000166"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Môi trường trường học, an toàn trường lớp, phòng thí nghiệm đơn giản.", true, new Guid("00000000-0000-0000-0000-000000000021"), "Trường học", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000167"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Các hoạt động cộng đồng, phong tục địa phương, nghề nghiệp.", true, new Guid("00000000-0000-0000-0000-000000000021"), "Cộng đồng", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000168"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bộ phận cây (rễ, thân, lá, hoa, quả). Các nhóm động vật, chuỗi thức ăn đơn giản.", true, new Guid("00000000-0000-0000-0000-000000000021"), "Thực vật và động vật", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000169"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Hệ tiêu hóa, tuần hoàn, thần kinh. Phòng bệnh truyền nhiễm.", true, new Guid("00000000-0000-0000-0000-000000000021"), "Con người và sức khỏe", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000170"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Hệ Mặt Trời, Trái Đất quay. Các đới khí hậu (nhiệt đới, ôn đới, hàn đới).", true, new Guid("00000000-0000-0000-0000-000000000021"), "Trái Đất và bầu trời", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000171"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bài 1-2: Kết hợp nét, hình, màu sắc. Tạo bức tranh tổng hợp.", true, new Guid("00000000-0000-0000-0000-000000000022"), "Nét, hình và màu", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000172"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bài 3-4: Vẽ tranh trường lớp, bạn bè, hoạt động trường học.", true, new Guid("00000000-0000-0000-0000-000000000022"), "Trường học của em", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000173"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bài 5-6: Vẽ chân dung gia đình, sinh hoạt ngày Tết.", true, new Guid("00000000-0000-0000-0000-000000000022"), "Gia đình thân yêu", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000174"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bài 7-8: Tạo sản phẩm mĩ thuật từ vật liệu tái chế.", true, new Guid("00000000-0000-0000-0000-000000000022"), "Sáng tạo từ vật liệu tái chế", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000175"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bài 9-10: Làm rối tay, diều giấy từ vật liệu đơn giản.", true, new Guid("00000000-0000-0000-0000-000000000022"), "Đồ chơi dân gian", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000176"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bài 11-12: Tìm hiểu, thưởng thức và vẽ lại tranh dân gian.", true, new Guid("00000000-0000-0000-0000-000000000022"), "Tranh dân gian", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000177"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bài 13-14: Vẽ tranh phong cảnh thiên nhiên.", true, new Guid("00000000-0000-0000-0000-000000000022"), "Thiên nhiên tươi đẹp", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000178"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bài 15-16: Ôn tập, trưng bày sản phẩm mĩ thuật cả năm.", true, new Guid("00000000-0000-0000-0000-000000000022"), "Tổng kết và trưng bày", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000179"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Hát bài về xuân, Tết. Ôn tập nốt nhạc Đô-Rê-Mi-Pha-Son-La-Si.", true, new Guid("00000000-0000-0000-0000-000000000023"), "Vui đón xuân", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000180"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Hát bài về gia đình. Thực hành cao độ nốt nhạc.", true, new Guid("00000000-0000-0000-0000-000000000023"), "Gia đình yêu thương", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000181"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Hát bài về trường lớp. Nhịp 2/4, 3/4.", true, new Guid("00000000-0000-0000-0000-000000000023"), "Mái trường mến yêu", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000182"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Hát bài về tình bạn. Nghe nhạc cổ điển.", true, new Guid("00000000-0000-0000-0000-000000000023"), "Vòng tay bạn bè", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000183"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Hát bài về thiên nhiên. Sáng tạo vận động.", true, new Guid("00000000-0000-0000-0000-000000000023"), "Thiên nhiên quanh em", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000184"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dân ca Việt Nam, nhạc cụ dân tộc.", true, new Guid("00000000-0000-0000-0000-000000000023"), "Quê hương", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000185"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Làm quen Piano/Organ. Nghe nhạc phương Tây.", true, new Guid("00000000-0000-0000-0000-000000000023"), "Thế giới âm nhạc", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000186"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn tập, biểu diễn cuối năm. Thang 7 nốt Đô-Si.", true, new Guid("00000000-0000-0000-0000-000000000023"), "Vui đón hè", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000187"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Tuần 1-4: Truyền thống nhà trường, nội quy, các hoạt động tập thể.", true, new Guid("00000000-0000-0000-0000-000000000024"), "Em và trường tiểu học", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000188"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Tuần 5-8: Điểm mạnh, điểm yếu, kế hoạch rèn luyện.", true, new Guid("00000000-0000-0000-0000-000000000024"), "Khám phá bản thân", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000189"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Tuần 9-12: Trách nhiệm với bản thân, gia đình, trường lớp.", true, new Guid("00000000-0000-0000-0000-000000000024"), "Trách nhiệm của em", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000190"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Tuần 13-16: Công việc nhà, giúp đỡ gia đình, ngày Tết.", true, new Guid("00000000-0000-0000-0000-000000000024"), "Em với gia đình", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000191"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Tuần 17-20: Tham gia hoạt động cộng đồng, thiện nguyện.", true, new Guid("00000000-0000-0000-0000-000000000024"), "Em với cộng đồng", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000192"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Tuần 21-24: Phân loại rác, tái chế, giảm thiểu ô nhiễm.", true, new Guid("00000000-0000-0000-0000-000000000024"), "Vệ sinh môi trường", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000193"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Tuần 25-28: Kỹ năng phòng cháy, thoát hiểm, phòng tránh thiên tai.", true, new Guid("00000000-0000-0000-0000-000000000024"), "Phòng hỏa hoạn và thiên tai", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000194"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Tuần 29-35: Bảo vệ môi trường, trồng cây, tổng kết năm học.", true, new Guid("00000000-0000-0000-0000-000000000024"), "Môi trường xanh", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000195"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bài 1-2: Các bộ phận máy tính (CPU, màn hình, bàn phím, chuột). Bật/tắt máy tính.", true, new Guid("00000000-0000-0000-0000-000000000025"), "Máy tính và em", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000196"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bài 3-4: Internet là gì? Trình duyệt web, truy cập website an toàn.", true, new Guid("00000000-0000-0000-0000-000000000025"), "Mạng Internet", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000197"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bài 5-6: Tệp và thư mục. Tạo, đổi tên, xóa thư mục trên Desktop.", true, new Guid("00000000-0000-0000-0000-000000000025"), "Lưu trữ thông tin", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000198"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bài 7-8: An toàn thông tin cá nhân, không chia sẻ mật khẩu, tôn trọng bản quyền.", true, new Guid("00000000-0000-0000-0000-000000000025"), "Đạo đức số", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000199"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bài 9-10: Tập gõ phím, sử dụng chuột thành thạo. Trò chơi luyện gõ.", true, new Guid("00000000-0000-0000-0000-000000000025"), "Ứng dụng: Bàn phím và chuột", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000200"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bài 11-12: Phần mềm Paint, vẽ hình đơn giản. Thuật toán cơ bản (tuần tự).", true, new Guid("00000000-0000-0000-0000-000000000025"), "Giải quyết vấn đề với máy tính", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000201"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Đèn, quạt, radio, tivi – nhận biết và sử dụng an toàn các thiết bị.", true, new Guid("00000000-0000-0000-0000-000000000026"), "Công nghệ và đời sống", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000202"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Chọn giống, gieo hạt, chăm sóc cây hoa trong chậu. Quan sát sinh trưởng.", true, new Guid("00000000-0000-0000-0000-000000000026"), "Trồng hoa trong chậu", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000203"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Lắp ráp xe đồ chơi, đèn giao thông từ bộ lắp ghép kỹ thuật.", true, new Guid("00000000-0000-0000-0000-000000000026"), "Lắp ráp mô hình", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000204"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Số có 6 chữ số, hàng và lớp (đơn vị, nghìn, triệu). Đọc, viết, so sánh, sắp xếp.", true, new Guid("00000000-0000-0000-0000-000000000027"), "Số có nhiều chữ số", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000205"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Làm tròn số đến hàng nghìn, hàng chục nghìn. Ước lượng kết quả.", true, new Guid("00000000-0000-0000-0000-000000000027"), "Làm tròn số và ước lượng", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000206"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Nhân số có nhiều chữ số với số có 2 chữ số. Tính chất giao hoán, kết hợp, phân phối.", true, new Guid("00000000-0000-0000-0000-000000000027"), "Phép nhân", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000207"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Chia số có nhiều chữ số cho số có 2 chữ số. Chia có dư, kiểm tra phép chia.", true, new Guid("00000000-0000-0000-0000-000000000027"), "Phép chia", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000208"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Góc vuông, nhọn, tù, bẹt. Hai đường thẳng vuông góc, song song.", true, new Guid("00000000-0000-0000-0000-000000000027"), "Hình học: Góc và đường thẳng", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000209"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Yến, tạ, tấn. Diện tích: mm², cm², dm², m². Đổi đơn vị đo.", true, new Guid("00000000-0000-0000-0000-000000000027"), "Đo lường", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000210"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn tập: số có nhiều chữ số, 4 phép tính, hình học, đo lường.", true, new Guid("00000000-0000-0000-0000-000000000027"), "Ôn tập học kì 1", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000211"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Khái niệm phân số, rút gọn, quy đồng mẫu số. Cộng, trừ, nhân, chia phân số.", true, new Guid("00000000-0000-0000-0000-000000000027"), "Phân số", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000212"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Đặc điểm, chu vi, diện tích hình bình hành và hình thoi.", true, new Guid("00000000-0000-0000-0000-000000000027"), "Hình bình hành và hình thoi", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000213"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Tỉ số của hai số. Bài toán tìm hai số khi biết tổng-hiệu, tổng-tỉ, hiệu-tỉ.", true, new Guid("00000000-0000-0000-0000-000000000027"), "Tỉ số", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000214"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Biểu đồ cột và biểu đồ tranh. Đọc, vẽ, phân tích biểu đồ.", true, new Guid("00000000-0000-0000-0000-000000000027"), "Biểu đồ", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000215"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn tập tổng hợp: phân số, hình bình hành/thoi, tỉ số, biểu đồ.", true, new Guid("00000000-0000-0000-0000-000000000027"), "Ôn tập cuối năm", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000216"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Chủ điểm 1 Tập 1: Danh từ. Đọc hiểu về cá tính, sự độc đáo mỗi người.", true, new Guid("00000000-0000-0000-0000-000000000028"), "Mỗi người một vẻ", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000217"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Chủ điểm 2 Tập 1: Động từ. Du lịch, trải nghiệm thiên nhiên.", true, new Guid("00000000-0000-0000-0000-000000000028"), "Trải nghiệm và khám phá", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000218"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Chủ điểm 3 Tập 1: Tính từ. Sách, kiến thức, ham đọc sách.", true, new Guid("00000000-0000-0000-0000-000000000028"), "Kho tàng tri thức", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000219"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Chủ điểm 4 Tập 1: Biện pháp nhân hóa. Ước mơ, hoài bão.", true, new Guid("00000000-0000-0000-0000-000000000028"), "Chắp cánh ước mơ", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000220"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn tập: danh từ, động từ, tính từ, nhân hóa. Viết đoạn văn tả người, kể chuyện.", true, new Guid("00000000-0000-0000-0000-000000000028"), "Ôn tập học kì 1", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000221"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Chủ điểm 1 Tập 2: Câu và thành phần câu (CN-VN). Tình yêu thương.", true, new Guid("00000000-0000-0000-0000-000000000028"), "Sống để yêu thương", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000222"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Chủ điểm 2 Tập 2: Trạng ngữ. Biết ơn, truyền thống.", true, new Guid("00000000-0000-0000-0000-000000000028"), "Uống nước nhớ nguồn", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000223"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Chủ điểm 3 Tập 2: Dấu ngoặc kép. Quê hương, đất nước.", true, new Guid("00000000-0000-0000-0000-000000000028"), "Quê hương trong tôi", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000224"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Chủ điểm 4 Tập 2: Từ ngữ, câu tưởng tượng. Hòa bình, nhân đạo.", true, new Guid("00000000-0000-0000-0000-000000000028"), "Vì một thế giới bình yên", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000225"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn tập: câu, trạng ngữ, dấu câu. Viết thư, giấy mời, tả cây cối.", true, new Guid("00000000-0000-0000-0000-000000000028"), "Ôn tập cuối năm", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000226"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Countries, nationalities (Where are you from?). Telling time (What time is it?).", true, new Guid("00000000-0000-0000-0000-000000000029"), "Unit 1-2: Countries & Time", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000227"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Days of the week (What day is it today?). Birthdays, months, ordinal numbers.", true, new Guid("00000000-0000-0000-0000-000000000029"), "Unit 3-4: Days & Birthdays", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000228"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Can/Can't for abilities. School facilities (library, gym, canteen).", true, new Guid("00000000-0000-0000-0000-000000000029"), "Unit 5-6: Abilities & School", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000229"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "School timetable, subjects (I have Maths on Monday). Favourite subjects.", true, new Guid("00000000-0000-0000-0000-000000000029"), "Unit 7-8: Timetable & Subjects", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000230"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Sports activities (play football, swim). Summer holidays, past activities.", true, new Guid("00000000-0000-0000-0000-000000000029"), "Unit 9-10: Sports & Holidays", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000231"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn tập học kì 1: quốc gia, thời gian, trường học, thể thao.", true, new Guid("00000000-0000-0000-0000-000000000029"), "Review 1 (Units 1-10)", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000232"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Jobs (doctor, farmer, teacher). Workplaces (hospital, school, office).", true, new Guid("00000000-0000-0000-0000-000000000029"), "Unit 11-12: Family Jobs & Workplaces", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000233"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Describing people (tall, short, hair). Daily activities (get up, go to school).", true, new Guid("00000000-0000-0000-0000-000000000029"), "Unit 13-14: Appearance & Daily Activities", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000234"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Weekend activities (What do you do at the weekend?). Weather (sunny, rainy, cloudy).", true, new Guid("00000000-0000-0000-0000-000000000029"), "Unit 15-16: Weekend & Weather", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000235"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "City places (park, cinema). Shopping (How much is it? It's...).", true, new Guid("00000000-0000-0000-0000-000000000029"), "Unit 17-18: City & Shopping", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000236"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Zoo animals, describing animals. Summer holiday plans.", true, new Guid("00000000-0000-0000-0000-000000000029"), "Unit 19-20: Animals & Summer", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000237"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn tập học kì 2: nghề nghiệp, hoạt động, thời tiết, mua sắm.", true, new Guid("00000000-0000-0000-0000-000000000029"), "Review 2 (Units 11-20)", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000238"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bài 1-2: Anh hùng dân tộc, kính trọng người có công.", true, new Guid("00000000-0000-0000-0000-000000000030"), "Biết ơn người có công với đất nước", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000239"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bài 3-4: Đồng cảm, tương trợ, hoạt động nhân đạo.", true, new Guid("00000000-0000-0000-0000-000000000030"), "Cảm thông và giúp đỡ người khó khăn", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000240"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bài 5: Chăm chỉ lao động, quý trọng sản phẩm lao động.", true, new Guid("00000000-0000-0000-0000-000000000030"), "Yêu lao động", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000241"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bài 6-7: Không lấy đồ không thuộc về mình, bảo vệ tài sản chung.", true, new Guid("00000000-0000-0000-0000-000000000030"), "Tôn trọng tài sản của người khác", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000242"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bài 8-9: Tình bạn lành mạnh, giải quyết mâu thuẫn hòa bình.", true, new Guid("00000000-0000-0000-0000-000000000030"), "Quan hệ bạn bè", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000243"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bài 10: Quyền được học tập, vui chơi, bảo vệ. Bổn phận đối với gia đình, xã hội.", true, new Guid("00000000-0000-0000-0000-000000000030"), "Quyền và bổn phận trẻ em", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000244"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Nước: 3 thể (rắn, lỏng, khí), chuyển thể (bay hơi, ngưng tụ, đông đặc, nóng chảy). Không khí.", true, new Guid("00000000-0000-0000-0000-000000000031"), "Chất", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000245"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ánh sáng (truyền thẳng, phản xạ), âm thanh (rung động, cao-thấp), nhiệt (truyền nhiệt, cách nhiệt).", true, new Guid("00000000-0000-0000-0000-000000000031"), "Năng lượng", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000246"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Trao đổi chất ở thực vật, động vật. Hô hấp, quang hợp. Thích nghi môi trường.", true, new Guid("00000000-0000-0000-0000-000000000031"), "Thực vật và động vật", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000247"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Vi khuẩn có ích, vi khuẩn gây bệnh. Nấm men, nấm mốc. Bảo quản thực phẩm.", true, new Guid("00000000-0000-0000-0000-000000000031"), "Nấm và vi khuẩn", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000248"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dinh dưỡng cân bằng, vitamin, khoáng chất. Vệ sinh an toàn thực phẩm.", true, new Guid("00000000-0000-0000-0000-000000000031"), "Con người và sức khỏe", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000249"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Hệ sinh thái, chuỗi thức ăn (sinh vật sản xuất, tiêu thụ, phân hủy). Bảo vệ đa dạng sinh học.", true, new Guid("00000000-0000-0000-0000-000000000031"), "Sinh vật và môi trường", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000250"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Gam màu nóng, lạnh, trung tính. Hòa sắc, tương phản.", true, new Guid("00000000-0000-0000-0000-000000000032"), "Màu sắc trong nghệ thuật", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000251"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Trống đồng Đông Sơn, gốm sứ Bát Tràng. Vẽ họa tiết truyền thống.", true, new Guid("00000000-0000-0000-0000-000000000032"), "Di sản văn hóa", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000252"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Kỹ thuật vẽ chân dung, tỉ lệ khuôn mặt, biểu cảm.", true, new Guid("00000000-0000-0000-0000-000000000032"), "Chân dung và con người", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000253"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Vẽ tranh phong cảnh, bảo vệ môi trường.", true, new Guid("00000000-0000-0000-0000-000000000032"), "Thiên nhiên và môi trường", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000254"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Nhận biết kiến trúc truyền thống Việt Nam (đình, chùa). Vẽ phối cảnh đơn giản.", true, new Guid("00000000-0000-0000-0000-000000000032"), "Kiến trúc", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000255"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Vẽ tranh đề tài cuộc sống, sinh hoạt hàng ngày.", true, new Guid("00000000-0000-0000-0000-000000000032"), "Cuộc sống quanh em", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000256"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Tranh của Tô Ngọc Vân, Van Gogh. Phân tích bố cục, màu sắc.", true, new Guid("00000000-0000-0000-0000-000000000032"), "Thưởng thức mĩ thuật", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000257"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Khuông nhạc, khóa Son. Nốt nhạc trên khuông.", true, new Guid("00000000-0000-0000-0000-000000000033"), "Khởi đầu mới", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000258"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dân ca các vùng miền. Nhạc cụ: Đàn Bầu, Đàn Tranh.", true, new Guid("00000000-0000-0000-0000-000000000033"), "Giai điệu quê hương", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000259"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Hát bài về thầy cô. Thực hành đọc nhạc.", true, new Guid("00000000-0000-0000-0000-000000000033"), "Thầy cô và mái trường", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000260"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Mozart, Beethoven – thần đồng âm nhạc. Thưởng thức nhạc cổ điển.", true, new Guid("00000000-0000-0000-0000-000000000033"), "Tuổi thơ và ước mơ", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000261"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Sáo Recorder hoặc Melodica. Thực hành chơi nhạc cụ.", true, new Guid("00000000-0000-0000-0000-000000000033"), "Ước mơ bay xa", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000262"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Hát bài Tết, xuân. Ôn tập tổng kết năm học.", true, new Guid("00000000-0000-0000-0000-000000000033"), "Vui đón xuân", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000263"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Xây dựng lớp học hạnh phúc, tự quản, hoạt động Đội.", true, new Guid("00000000-0000-0000-0000-000000000034"), "Nhà trường", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000264"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Hoạt động thiện nguyện, tham gia sự kiện cộng đồng.", true, new Guid("00000000-0000-0000-0000-000000000034"), "Cộng đồng", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000265"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Khám phá thiên nhiên, bảo vệ môi trường sống.", true, new Guid("00000000-0000-0000-0000-000000000034"), "Thiên nhiên", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000266"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Quản lý chi tiêu đơn giản, phòng chống đuối nước.", true, new Guid("00000000-0000-0000-0000-000000000034"), "Kỹ năng tự phục vụ", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000267"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Tìm hiểu nghề nghiệp, đóng vai, lập kế hoạch tương lai.", true, new Guid("00000000-0000-0000-0000-000000000034"), "Nghề nghiệp", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000268"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Phân biệt phần cứng (hardware) và phần mềm (software). Bộ nhớ.", true, new Guid("00000000-0000-0000-0000-000000000035"), "Phần cứng và phần mềm", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000269"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Sử dụng trình duyệt, tìm kiếm Google, đánh giá nguồn thông tin.", true, new Guid("00000000-0000-0000-0000-000000000035"), "Internet và tìm kiếm", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000270"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Quản lý tệp, sao chép, di chuyển, nén/giải nén.", true, new Guid("00000000-0000-0000-0000-000000000035"), "Tệp và thư mục nâng cao", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000271"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Tôn trọng bản quyền phần mềm, hình ảnh, nhạc. Quy tắc sử dụng.", true, new Guid("00000000-0000-0000-0000-000000000035"), "Bản quyền số", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000272"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Microsoft Word: nhập văn bản, định dạng chữ, chèn hình ảnh.", true, new Guid("00000000-0000-0000-0000-000000000035"), "Soạn thảo văn bản", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000273"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Giao diện Scratch, khối lệnh, lập trình hoạt hình và trò chơi đơn giản.", true, new Guid("00000000-0000-0000-0000-000000000035"), "Lập trình Scratch", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000274"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Kỹ thuật gieo hạt, trồng, chăm sóc cây hoa và cây cảnh.", true, new Guid("00000000-0000-0000-0000-000000000036"), "Trồng hoa và cây cảnh", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000275"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Lắp ráp cái đu, rô-bốt, bập bênh, đồ chơi dân gian từ bộ lắp ghép.", true, new Guid("00000000-0000-0000-0000-000000000036"), "Lắp ráp mô hình kỹ thuật", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000276"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bản đồ, lược đồ, biểu đồ. Cách sử dụng bản đồ để học địa lí.", true, new Guid("00000000-0000-0000-0000-000000000037"), "Phương tiện học tập Lịch sử và Địa lí", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000277"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Đặc điểm tự nhiên, dân cư, kinh tế địa phương.", true, new Guid("00000000-0000-0000-0000-000000000037"), "Địa phương em", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000278"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Đặc điểm địa hình, khí hậu, dân tộc. Đền Hùng, truyền thuyết Hùng Vương.", true, new Guid("00000000-0000-0000-0000-000000000037"), "Trung du và miền núi Bắc Bộ", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000279"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Đồng bằng sông Hồng, kinh thành Thăng Long, Văn Miếu – Quốc Tử Giám.", true, new Guid("00000000-0000-0000-0000-000000000037"), "Đồng bằng Bắc Bộ", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000280"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Đặc điểm bờ biển, kinh tế. Di sản: Cố đô Huế, phố cổ Hội An.", true, new Guid("00000000-0000-0000-0000-000000000037"), "Duyên hải miền Trung", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000281"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Tây Nguyên: cồng chiêng, cà phê. Nam Bộ: TP.HCM, địa đạo Củ Chi, đồng bằng sông Cửu Long.", true, new Guid("00000000-0000-0000-0000-000000000037"), "Tây Nguyên và Nam Bộ", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000282"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn tập phân số. Khái niệm số thập phân, chuyển đổi phân số ↔ số thập phân.", true, new Guid("00000000-0000-0000-0000-000000000038"), "Ôn tập phân số và số thập phân", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000283"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Cộng, trừ, nhân, chia số thập phân. Tính giá trị biểu thức.", true, new Guid("00000000-0000-0000-0000-000000000038"), "Bốn phép tính với số thập phân", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000284"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Tam giác, hình thang (chu vi, diện tích). Hình tròn (chu vi, diện tích). Hình hộp chữ nhật, lập phương (diện tích xung quanh, toàn phần, thể tích).", true, new Guid("00000000-0000-0000-0000-000000000038"), "Hình học", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000285"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Khái niệm phần trăm (%). Tìm tỉ số phần trăm, tìm giá trị theo %. Bài toán lãi suất cơ bản.", true, new Guid("00000000-0000-0000-0000-000000000038"), "Tỉ số phần trăm", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000286"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Đổi đơn vị đo. Vận tốc, quãng đường, thời gian: s = v × t.", true, new Guid("00000000-0000-0000-0000-000000000038"), "Đo lường và chuyển động đều", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000287"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Biểu đồ hình quạt (đọc, nhận xét). Xác suất thực nghiệm.", true, new Guid("00000000-0000-0000-0000-000000000038"), "Thống kê và xác suất", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000288"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn tập tổng hợp chuẩn bị chuyển cấp: số thập phân, phân số, hình học, đo lường, tỉ số %.", true, new Guid("00000000-0000-0000-0000-000000000038"), "Ôn tập cuối năm", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000289"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Chủ điểm 1: Từ đồng nghĩa. Tả phong cảnh. Đọc hiểu về tuổi thơ.", true, new Guid("00000000-0000-0000-0000-000000000039"), "Thế giới tuổi thơ", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000290"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Chủ điểm 2: Từ trái nghĩa. Văn bản về cộng đồng, sẻ chia.", true, new Guid("00000000-0000-0000-0000-000000000039"), "Cộng đồng", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000291"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Chủ điểm 3: Từ đồng âm, từ nhiều nghĩa. Tả người. Văn bản về quê hương.", true, new Guid("00000000-0000-0000-0000-000000000039"), "Quê hương", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000292"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn tập: từ đồng nghĩa, trái nghĩa, đồng âm, nhiều nghĩa. Viết bài tả phong cảnh, tả người.", true, new Guid("00000000-0000-0000-0000-000000000039"), "Ôn tập học kì 1", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000293"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Chủ điểm 4: Đại từ, quan hệ từ. Kể chuyện sáng tạo. Bảo vệ môi trường.", true, new Guid("00000000-0000-0000-0000-000000000039"), "Bảo vệ môi trường", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000294"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Chủ điểm 5: Câu đơn, câu ghép. Văn bản về trẻ em, tương lai.", true, new Guid("00000000-0000-0000-0000-000000000039"), "Chủ nhân tương lai", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000295"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Chủ điểm 6: Ôn tập tổng hợp chuyển cấp. Hệ thống ngữ pháp, từ vựng.", true, new Guid("00000000-0000-0000-0000-000000000039"), "Vững bước vào cấp hai", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000296"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Addresses, daily routines, hobbies. Where do you live? What do you do every day?", true, new Guid("00000000-0000-0000-0000-000000000040"), "Me & My Friends", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000297"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "School subjects, events, school rules. Present continuous tense.", true, new Guid("00000000-0000-0000-0000-000000000040"), "My School", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000298"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Holidays, transport, folk tales. Past Simple tense.", true, new Guid("00000000-0000-0000-0000-000000000040"), "The World Around Us", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000299"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn tập: daily routines, school, holidays. Grammar: present simple/continuous, past simple.", true, new Guid("00000000-0000-0000-0000-000000000040"), "Review 1 (Themes 1-3)", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000300"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Jobs, health & safety. Future Simple (will), Should for advice.", true, new Guid("00000000-0000-0000-0000-000000000040"), "The Future", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000301"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Vietnamese and world festivals. Descriptions, traditions, food.", true, new Guid("00000000-0000-0000-0000-000000000040"), "Festivals", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000302"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn tập: jobs, health, festivals. Grammar: future simple, should, overall review.", true, new Guid("00000000-0000-0000-0000-000000000040"), "Review 2 (Themes 4-5)", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000303"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Anh hùng dân tộc, đền đáp công ơn, ngày thương binh liệt sĩ 27/7.", true, new Guid("00000000-0000-0000-0000-000000000041"), "Biết ơn người có công với đất nước", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000304"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Tôn trọng giới tính, dân tộc, tín ngưỡng. Không phân biệt đối xử.", true, new Guid("00000000-0000-0000-0000-000000000041"), "Tôn trọng sự khác biệt", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000305"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ý chí, nghị lực vượt khó. Gương người thành công từ hoàn cảnh khó khăn.", true, new Guid("00000000-0000-0000-0000-000000000041"), "Vượt qua khó khăn", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000306"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Trách nhiệm bảo vệ môi trường, ứng phó biến đổi khí hậu.", true, new Guid("00000000-0000-0000-0000-000000000041"), "Bảo vệ môi trường", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000307"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Công ước Liên Hợp Quốc về quyền trẻ em. Bổn phận của trẻ em.", true, new Guid("00000000-0000-0000-0000-000000000041"), "Quyền và bổn phận trẻ em", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000308"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Hòa bình thế giới, hữu nghị giữa các dân tộc.", true, new Guid("00000000-0000-0000-0000-000000000041"), "Tình hữu nghị quốc tế", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000309"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Hỗn hợp, dung dịch. Biến đổi lý học, hóa học. Lọc, lắng, bay hơi.", true, new Guid("00000000-0000-0000-0000-000000000042"), "Chất và biến đổi hóa học", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000310"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Mạch điện đơn giản (pin, bóng đèn, công tắc). Năng lượng tái tạo (mặt trời, gió).", true, new Guid("00000000-0000-0000-0000-000000000042"), "Năng lượng", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000311"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Sinh sản ở người. Tuổi dậy thì. Vệ sinh cơ thể.", true, new Guid("00000000-0000-0000-0000-000000000042"), "Con người", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000312"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Sinh sản hữu tính, vô tính ở thực vật và động vật.", true, new Guid("00000000-0000-0000-0000-000000000042"), "Thực vật và động vật", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000313"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ứng dụng vi sinh vật: lên men (sữa chua, dưa muối). Nấm có ích và nấm độc.", true, new Guid("00000000-0000-0000-0000-000000000042"), "Vi khuẩn và nấm nâng cao", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000314"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ô nhiễm (không khí, nước, đất). Bảo tồn tài nguyên, phát triển bền vững.", true, new Guid("00000000-0000-0000-0000-000000000042"), "Môi trường và tài nguyên", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000315"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Gam màu nóng, lạnh trong thiết kế. Trang trí họa tiết.", true, new Guid("00000000-0000-0000-0000-000000000043"), "Màu sắc trang trí", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000316"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Múa rối nước, nghệ thuật truyền thống Việt Nam.", true, new Guid("00000000-0000-0000-0000-000000000043"), "Di sản văn hóa", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000317"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Kỹ thuật vẽ dáng người chuyển động, tỉ lệ cơ thể.", true, new Guid("00000000-0000-0000-0000-000000000043"), "Tạo hình dáng người", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000318"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Thiết kế bao bì sản phẩm, typography cơ bản.", true, new Guid("00000000-0000-0000-0000-000000000043"), "Thiết kế đồ họa", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000319"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Luật xa gần (phối cảnh), vẽ phong cảnh có chiều sâu.", true, new Guid("00000000-0000-0000-0000-000000000043"), "Kiến trúc và không gian", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000320"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Tranh cổ động, tranh đề tài xã hội. Trưng bày cuối năm.", true, new Guid("00000000-0000-0000-0000-000000000043"), "Cuộc sống quanh em", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000321"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Nhịp 2/4. Hát bài về buổi sáng. Lý thuyết nhịp.", true, new Guid("00000000-0000-0000-0000-000000000044"), "Reo vang bình minh", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000322"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Hát Xoan, Quan họ. Nhạc cụ: Đàn Nguyệt, Đàn Tì Bà.", true, new Guid("00000000-0000-0000-0000-000000000044"), "Giai điệu quê hương", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000323"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dấu luyến, dấu nối trong bài hát. Bài hát về thầy cô, cha mẹ.", true, new Guid("00000000-0000-0000-0000-000000000044"), "Lòng biết ơn", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000324"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Chopin, Tchaikovsky. Nghe nhạc cổ điển phương Tây.", true, new Guid("00000000-0000-0000-0000-000000000044"), "Âm nhạc năm châu", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000325"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Sáo Recorder nâng cao. Hát bè 2 bè. Bài hát về hòa bình.", true, new Guid("00000000-0000-0000-0000-000000000044"), "Khát vọng hòa bình", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000326"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bài hát về Bác Hồ. Ôn tập tổng kết năm học.", true, new Guid("00000000-0000-0000-0000-000000000044"), "Nhớ ơn Bác Hồ", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000327"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Tự quản lớp học, xây dựng video kỉ niệm, hoạt động Đội nâng cao.", true, new Guid("00000000-0000-0000-0000-000000000045"), "Nhà trường", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000328"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Hành trình về nguồn, góc cộng đồng xanh, hoạt động thiện nguyện.", true, new Guid("00000000-0000-0000-0000-000000000045"), "Cộng đồng", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000329"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Biến đổi khí hậu, ứng phó thiên tai, bảo vệ đa dạng sinh học.", true, new Guid("00000000-0000-0000-0000-000000000045"), "Thiên nhiên", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000330"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "An toàn trên mạng Internet, quản lý chi tiêu, hội chợ mini.", true, new Guid("00000000-0000-0000-0000-000000000045"), "Kỹ năng sống", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000331"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Tìm hiểu nghề nghiệp, chuẩn bị kỹ năng chuyển cấp lên THCS.", true, new Guid("00000000-0000-0000-0000-000000000045"), "Nghề nghiệp và chuẩn bị lên lớp 6", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000332"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Kỹ thuật tìm kiếm nâng cao trên Google, đánh giá độ tin cậy nguồn thông tin.", true, new Guid("00000000-0000-0000-0000-000000000046"), "Tìm kiếm nâng cao", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000333"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Tạo và sử dụng email. Gửi, nhận, trả lời thư. An toàn email.", true, new Guid("00000000-0000-0000-0000-000000000046"), "Email", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000334"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Google Drive: tải lên, chia sẻ tập tin, cộng tác trực tuyến.", true, new Guid("00000000-0000-0000-0000-000000000046"), "Lưu trữ đám mây", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000335"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bắt nạt trên mạng (cyberbullying), quyền riêng tư, ứng xử văn minh trên mạng.", true, new Guid("00000000-0000-0000-0000-000000000046"), "Công dân số", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000336"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "PowerPoint: tạo bài trình chiếu, chèn văn bản, hình ảnh, hiệu ứng.", true, new Guid("00000000-0000-0000-0000-000000000046"), "Đa phương tiện và trình chiếu", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000337"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Vòng lặp, câu lệnh điều kiện, biến số. Lập trình trò chơi và ứng dụng.", true, new Guid("00000000-0000-0000-0000-000000000046"), "Lập trình Scratch nâng cao", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000338"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Vai trò công nghệ trong cuộc sống. Thiết bị công nghệ thông dụng.", true, new Guid("00000000-0000-0000-0000-000000000047"), "Công nghệ và đời sống", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000339"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Quy trình thiết kế: ý tưởng → bản vẽ → chế tạo → thử nghiệm.", true, new Guid("00000000-0000-0000-0000-000000000047"), "Thiết kế kỹ thuật tổng quát", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000340"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Lắp ráp xe chạy điện, máy bay mô hình từ bộ lắp ghép.", true, new Guid("00000000-0000-0000-0000-000000000047"), "Lắp ráp mô hình điện", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000341"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Chuẩn bị bể cá, chọn giống, chăm sóc cá cảnh.", true, new Guid("00000000-0000-0000-0000-000000000047"), "Nuôi cá cảnh", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000342"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Sử dụng phần mềm vẽ để thiết kế logo, poster đơn giản.", true, new Guid("00000000-0000-0000-0000-000000000047"), "Thiết kế đồ họa số", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000343"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Vai trò công nghệ trong các nghề. Định hướng nghề nghiệp.", true, new Guid("00000000-0000-0000-0000-000000000047"), "Công nghệ và nghề nghiệp", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000344"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Địa lí: Vị trí, hình dạng đất nước. Biển đảo, Hoàng Sa, Trường Sa.", true, new Guid("00000000-0000-0000-0000-000000000048"), "Đất nước Việt Nam", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000345"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Địa hình, khí hậu, sông ngòi. Đặc điểm thiên nhiên các vùng miền.", true, new Guid("00000000-0000-0000-0000-000000000048"), "Thiên nhiên Việt Nam", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000346"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "5 châu lục, 4 đại dương. Đặc điểm chính của các châu lục.", true, new Guid("00000000-0000-0000-0000-000000000048"), "Các châu lục và đại dương", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000347"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Lịch sử: Phong trào yêu nước, Đảng Cộng sản Việt Nam ra đời, Cách mạng Tháng Tám 1945.", true, new Guid("00000000-0000-0000-0000-000000000048"), "Việt Nam đầu thế kỷ XX - 1945", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000348"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Chiến thắng Điện Biên Phủ 1954, Hiệp định Giơ-ne-vơ.", true, new Guid("00000000-0000-0000-0000-000000000048"), "Kháng chiến chống Pháp", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000349"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Chiến dịch Hồ Chí Minh, giải phóng miền Nam 30/4/1975, thống nhất đất nước.", true, new Guid("00000000-0000-0000-0000-000000000048"), "Kháng chiến chống Mỹ", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000350"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Thời kỳ đổi mới từ 1986, hội nhập quốc tế, thành tựu phát triển.", true, new Guid("00000000-0000-0000-0000-000000000048"), "Xây dựng và đổi mới", null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000121"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000122"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000123"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000124"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000125"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000126"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000127"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000128"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000129"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000130"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000131"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000132"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000133"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000134"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000135"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000136"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000137"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000138"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000139"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000140"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000141"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000142"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000143"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000144"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000145"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000146"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000147"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000148"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000149"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000150"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000151"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000152"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000153"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000154"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000155"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000156"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000157"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000158"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000159"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000160"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000161"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000162"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000163"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000164"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000165"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000166"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000167"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000168"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000169"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000170"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000171"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000172"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000173"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000174"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000175"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000176"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000177"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000178"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000179"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000180"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000181"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000182"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000183"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000184"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000185"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000186"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000187"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000188"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000189"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000190"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000191"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000192"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000193"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000194"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000195"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000196"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000197"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000198"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000199"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000200"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000201"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000202"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000203"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000204"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000205"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000206"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000207"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000208"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000209"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000210"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000211"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000212"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000213"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000214"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000215"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000216"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000217"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000218"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000219"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000220"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000221"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000222"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000223"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000224"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000225"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000226"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000227"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000228"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000229"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000230"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000231"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000232"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000233"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000234"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000235"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000236"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000237"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000238"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000239"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000240"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000241"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000242"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000243"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000244"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000245"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000246"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000247"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000248"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000249"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000250"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000251"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000252"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000253"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000254"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000255"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000256"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000257"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000258"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000259"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000260"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000261"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000262"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000263"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000264"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000265"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000266"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000267"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000268"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000269"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000270"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000271"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000272"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000273"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000274"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000275"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000276"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000277"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000278"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000279"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000280"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000281"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000282"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000283"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000284"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000285"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000286"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000287"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000288"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000289"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000290"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000291"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000292"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000293"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000294"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000295"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000296"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000297"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000298"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000299"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000300"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000301"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000302"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000303"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000304"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000305"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000306"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000307"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000308"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000309"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000310"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000311"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000312"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000313"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000314"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000315"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000316"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000317"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000318"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000319"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000320"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000321"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000322"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000323"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000324"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000325"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000326"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000327"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000328"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000329"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000330"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000331"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000332"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000333"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000334"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000335"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000336"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000337"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000338"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000339"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000340"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000341"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000342"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000343"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000344"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000345"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000346"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000347"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000348"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000349"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000350"));

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "Description",
                value: "Study of numbers, quantities, and shapes");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Study of past events and Earth's features", "History & Geography", "history_geography" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                column: "Description",
                value: "English language and literature");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000004"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Study of written works and literary analysis", "Literature", "literature" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000005"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Computer science and digital technology", "Information Technology", "it" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000006"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Applied science and engineering", "Technology", "technology" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000007"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Visual arts and creative expression", "Art", "art" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000008"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Study of natural world and physical phenomena", "Science", "science" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000009"),
                columns: new[] { "Description", "Grade", "Name", "SubjectCode" },
                values: new object[] { "Musical theory and performance", 1, "Music", "music" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000010"),
                columns: new[] { "Description", "Grade", "Name", "SubjectCode" },
                values: new object[] { "Hands-on learning and practical experiences", 1, "Experiential Activities", "experiential_activities" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000011"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Study of numbers, quantities, and shapes", "Mathematics", "math" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000012"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Study of past events and Earth's features", "History & Geography", "history_geography" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000013"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "English language and literature", "English", "english" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000014"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Study of written works and literary analysis", "Literature", "literature" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000015"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Computer science and digital technology", "Information Technology", "it" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000016"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Applied science and engineering", "Technology", "technology" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000017"),
                columns: new[] { "Description", "Grade", "Name", "SubjectCode" },
                values: new object[] { "Visual arts and creative expression", 2, "Art", "art" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000018"),
                columns: new[] { "Description", "Grade", "Name", "SubjectCode" },
                values: new object[] { "Study of natural world and physical phenomena", 2, "Science", "science" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000019"),
                columns: new[] { "Description", "Grade", "Name", "SubjectCode" },
                values: new object[] { "Musical theory and performance", 2, "Music", "music" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000020"),
                columns: new[] { "Description", "Grade", "Name", "SubjectCode" },
                values: new object[] { "Hands-on learning and practical experiences", 2, "Experiential Activities", "experiential_activities" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000021"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Study of numbers, quantities, and shapes", "Mathematics", "math" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000022"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Study of past events and Earth's features", "History & Geography", "history_geography" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000023"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "English language and literature", "English", "english" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000024"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Study of written works and literary analysis", "Literature", "literature" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000025"),
                columns: new[] { "Description", "SubjectCode" },
                values: new object[] { "Computer science and digital technology", "it" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000026"),
                column: "Description",
                value: "Applied science and engineering");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000027"),
                columns: new[] { "Description", "Grade", "Name", "SubjectCode" },
                values: new object[] { "Visual arts and creative expression", 3, "Art", "art" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000028"),
                columns: new[] { "Description", "Grade", "Name", "SubjectCode" },
                values: new object[] { "Study of natural world and physical phenomena", 3, "Science", "science" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000029"),
                columns: new[] { "Description", "Grade", "Name", "SubjectCode" },
                values: new object[] { "Musical theory and performance", 3, "Music", "music" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000030"),
                columns: new[] { "Description", "Grade", "Name", "SubjectCode" },
                values: new object[] { "Hands-on learning and practical experiences", 3, "Experiential Activities", "experiential_activities" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000031"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Study of numbers, quantities, and shapes", "Mathematics", "math" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000032"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Study of past events and Earth's features", "History & Geography", "history_geography" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000033"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "English language and literature", "English", "english" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000034"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Study of written works and literary analysis", "Literature", "literature" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000035"),
                columns: new[] { "Description", "SubjectCode" },
                values: new object[] { "Computer science and digital technology", "it" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000036"),
                column: "Description",
                value: "Applied science and engineering");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000037"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Visual arts and creative expression", "Art", "art" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000038"),
                columns: new[] { "Description", "Grade", "Name", "SubjectCode" },
                values: new object[] { "Study of natural world and physical phenomena", 4, "Science", "science" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000039"),
                columns: new[] { "Description", "Grade", "Name", "SubjectCode" },
                values: new object[] { "Musical theory and performance", 4, "Music", "music" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000040"),
                columns: new[] { "Description", "Grade", "Name", "SubjectCode" },
                values: new object[] { "Hands-on learning and practical experiences", 4, "Experiential Activities", "experiential_activities" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000041"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Study of numbers, quantities, and shapes", "Mathematics", "math" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000042"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Study of past events and Earth's features", "History & Geography", "history_geography" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000043"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "English language and literature", "English", "english" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000044"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Study of written works and literary analysis", "Literature", "literature" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000045"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Computer science and digital technology", "Information Technology", "it" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000046"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Applied science and engineering", "Technology", "technology" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000047"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Visual arts and creative expression", "Art", "art" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000048"),
                columns: new[] { "Description", "Name", "SubjectCode" },
                values: new object[] { "Study of natural world and physical phenomena", "Science", "science" });

            migrationBuilder.InsertData(
                schema: "public",
                table: "Subjects",
                columns: new[] { "Id", "Description", "Grade", "ImageUrl", "Name", "SubjectCode" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000049"), "Musical theory and performance", 5, null, "Music", "music" },
                    { new Guid("00000000-0000-0000-0000-000000000050"), "Hands-on learning and practical experiences", 5, null, "Experiential Activities", "experiential_activities" }
                });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"),
                columns: new[] { "Description", "Title" },
                values: new object[] { "Nhận biết và đọc số 0, 1, 2, 3, 4, 5. Tập viết số. Đếm số lượng đồ vật.", "1.1 - Các số 0, 1, 2, 3, 4, 5" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"),
                columns: new[] { "Description", "Title" },
                values: new object[] { "Nhận biết và đọc số 6, 7, 8, 9, 10. Tập viết số.", "1.2 - Các số 6, 7, 8, 9, 10" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"),
                columns: new[] { "Description", "Title" },
                values: new object[] { "So sánh số lượng. Xác định: Nhiều hơn, ít hơn, bằng nhau.", "1.3 - Nhiều hơn, ít hơn, bằng nhau" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"),
                columns: new[] { "Description", "Title" },
                values: new object[] { "Dấu lớn hơn (>), bé hơn (<), bằng (=). Ví dụ: 4 > 3, 2 < 5, 4 = 4", "1.4 - So sánh số" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000005"),
                columns: new[] { "Description", "Title" },
                values: new object[] { "Phân tích cấu tạo số. Ví dụ: 5 gồm 3 và 2", "1.5 - Mấy và mấy" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000006"),
                columns: new[] { "Description", "Title" },
                values: new object[] { "Ôn tập và luyện tập các số từ 0 đến 10", "1.6 - Luyện tập chung" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000007"),
                columns: new[] { "Description", "Title" },
                values: new object[] { "Nhận biết các hình phẳng cơ bản", "2.1 - Hình vuông, hình tròn, hình tam giác, hình chữ nhật" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000008"),
                columns: new[] { "Description", "Title" },
                values: new object[] { "Thực hành với các hình phẳng", "2.2 - Thực hành lắp ghép, xếp hình" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000009"),
                columns: new[] { "Description", "Title" },
                values: new object[] { "Ôn tập các hình phẳng", "2.3 - Luyện tập chung" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000010"),
                columns: new[] { "Description", "Title" },
                values: new object[] { "Ý nghĩa: Gộp lại. Ví dụ: 3 + 2 = 5", "3.1 - Phép cộng trong phạm vi 10" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000011"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Ý nghĩa: Bớt đi. Ví dụ: 6 - 1 = 5", new Guid("00000000-0000-0000-0000-000000000001"), "3.2 - Phép trừ trong phạm vi 10" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000012"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Học thuộc bảng cộng trừ trong phạm vi 10", new Guid("00000000-0000-0000-0000-000000000001"), "3.3 - Bảng cộng, bảng trừ trong phạm vi 10" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000013"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Ôn tập phép cộng, phép trừ", new Guid("00000000-0000-0000-0000-000000000001"), "3.4 - Luyện tập chung" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000014"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Nhận biết các hình khối cơ bản", new Guid("00000000-0000-0000-0000-000000000001"), "4.1 - Khối lập phương, khối hộp chữ nhật" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000015"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Phải - Trái, Trên - Dưới, Trước - Sau", new Guid("00000000-0000-0000-0000-000000000001"), "4.2 - Vị trí, định hướng trong không gian" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000016"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Ôn tập hình khối và vị trí", new Guid("00000000-0000-0000-0000-000000000001"), "4.3 - Luyện tập chung" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000017"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Ôn tập tổng hợp các số 0-10", new Guid("00000000-0000-0000-0000-000000000001"), "5.1 - Ôn tập các số trong phạm vi 10" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000018"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Ôn tập các phép tính", new Guid("00000000-0000-0000-0000-000000000001"), "5.2 - Ôn tập phép cộng, phép trừ" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000019"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Ôn tập hình phẳng và hình khối", new Guid("00000000-0000-0000-0000-000000000001"), "5.3 - Ôn tập hình học" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000020"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Ôn tập tổng hợp học kì 1", new Guid("00000000-0000-0000-0000-000000000001"), "5.4 - Ôn tập chung học kì 1" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000021"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Nhận biết Chục và Đơn vị. Cách đọc: 24 = hai mươi tư", new Guid("00000000-0000-0000-0000-000000000001"), "6.1 - Số có hai chữ số" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000022"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "So sánh hàng chục trước. Ví dụ: 42 > 25", new Guid("00000000-0000-0000-0000-000000000001"), "6.2 - So sánh số có hai chữ số" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000023"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Số 100 = một trăm. Làm quen với bảng số", new Guid("00000000-0000-0000-0000-000000000001"), "6.3 - Bảng các số từ 1 đến 100" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000024"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Ôn tập các số đến 100", new Guid("00000000-0000-0000-0000-000000000001"), "6.4 - Luyện tập chung" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000025"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "So sánh độ dài của các vật", new Guid("00000000-0000-0000-0000-000000000001"), "7.1 - Dài hơn, ngắn hơn" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000026"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Làm quen với đơn vị cm", new Guid("00000000-0000-0000-0000-000000000001"), "7.2 - Đơn vị đo độ dài (cm)" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000027"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Thực hành đo đạc", new Guid("00000000-0000-0000-0000-000000000001"), "7.3 - Thực hành ước lượng và đo độ dài" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000028"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Ôn tập đo độ dài", new Guid("00000000-0000-0000-0000-000000000001"), "7.4 - Luyện tập chung" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000029"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Cộng số có hai chữ số với một chữ số", new Guid("00000000-0000-0000-0000-000000000001"), "8.1 - Phép cộng (2 chữ số + 1 chữ số)" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000030"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Cộng hai số có hai chữ số", new Guid("00000000-0000-0000-0000-000000000001"), "8.2 - Phép cộng (2 chữ số + 2 chữ số)" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000031"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Trừ số có hai chữ số cho một chữ số", new Guid("00000000-0000-0000-0000-000000000001"), "8.3 - Phép trừ (2 chữ số - 1 chữ số)" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000032"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Trừ hai số có hai chữ số", new Guid("00000000-0000-0000-0000-000000000001"), "8.4 - Phép trừ (2 chữ số - 2 chữ số)" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000033"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Ôn tập cộng trừ", new Guid("00000000-0000-0000-0000-000000000001"), "8.5 - Luyện tập chung" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000034"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Học cách xem giờ", new Guid("00000000-0000-0000-0000-000000000001"), "9.1 - Xem giờ đúng trên đồng hồ" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000035"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Tên các ngày trong tuần", new Guid("00000000-0000-0000-0000-000000000001"), "9.2 - Các ngày trong tuần" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000036"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Thực hành với lịch và đồng hồ", new Guid("00000000-0000-0000-0000-000000000001"), "9.3 - Thực hành xem lịch và giờ" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000037"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Ôn tập về thời gian", new Guid("00000000-0000-0000-0000-000000000001"), "9.4 - Luyện tập chung" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000038"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Ôn tập các số và phép tính trong phạm vi 10", new Guid("00000000-0000-0000-0000-000000000001"), "10.1 - Ôn tập các số và phép tính (0-10)" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000039"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Ôn tập các số và phép tính trong phạm vi 100", new Guid("00000000-0000-0000-0000-000000000001"), "10.2 - Ôn tập các số và phép tính (0-100)" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000040"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Ôn tập hình học, độ dài, thời gian", new Guid("00000000-0000-0000-0000-000000000001"), "10.3 - Ôn tập hình học và đo lường" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000041"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Ôn tập tổng hợp toàn bộ chương trình lớp 1", new Guid("00000000-0000-0000-0000-000000000001"), "10.4 - Ôn tập chung cuối năm" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000042"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Ôn tập kiến thức lớp 1", new Guid("00000000-0000-0000-0000-000000000011"), "1.1 - Ôn tập các số đến 100" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000043"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Ôn tập các phép tính cơ bản", new Guid("00000000-0000-0000-0000-000000000011"), "1.2 - Ôn tập phép cộng, phép trừ không nhớ" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000044"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Phương pháp: Tách số để tròn chục. Ví dụ: 9 + 5 = 9 + 1 + 4 = 14", new Guid("00000000-0000-0000-0000-000000000011"), "2.1 - Phép cộng qua 10 trong phạm vi 20" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000045"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Thực hành phép cộng qua 10", new Guid("00000000-0000-0000-0000-000000000011"), "2.2 - Luyện tập phép cộng qua 10" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000046"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Phương pháp: Trừ để tròn chục. Ví dụ: 12 - 5 = 12 - 2 - 3 = 7", new Guid("00000000-0000-0000-0000-000000000011"), "2.3 - Phép trừ qua 10 trong phạm vi 20" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000047"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Thực hành phép trừ qua 10", new Guid("00000000-0000-0000-0000-000000000011"), "2.4 - Luyện tập phép trừ qua 10" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000048"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Ôn tập cộng trừ qua 10", new Guid("00000000-0000-0000-0000-000000000011"), "2.5 - Luyện tập chung - Cộng trừ qua 10" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000049"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Khối trụ: Hai mặt đáy tròn (hộp sữa). Khối cầu: Tròn đều (quả bóng)", new Guid("00000000-0000-0000-0000-000000000011"), "3.1 - Khối trụ, khối cầu" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000050"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Thực hành nhận biết khối trụ, khối cầu", new Guid("00000000-0000-0000-0000-000000000011"), "3.2 - Luyện tập - Hình khối" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000051"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Phép cộng có nhớ trong phạm vi 100. Ví dụ: 38 + 25 = 63", new Guid("00000000-0000-0000-0000-000000000011"), "4.1 - Phép cộng có nhớ" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000052"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Thực hành phép cộng có nhớ", new Guid("00000000-0000-0000-0000-000000000011"), "4.2 - Luyện tập phép cộng có nhớ" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000053"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Phép trừ có nhớ trong phạm vi 100. Ví dụ: 52 - 27 = 25", new Guid("00000000-0000-0000-0000-000000000011"), "4.3 - Phép trừ có nhớ" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000054"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Thực hành phép trừ có nhớ", new Guid("00000000-0000-0000-0000-000000000011"), "4.4 - Luyện tập phép trừ có nhớ" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000055"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Ôn tập cộng trừ có nhớ", new Guid("00000000-0000-0000-0000-000000000011"), "4.5 - Luyện tập chung - Cộng trừ có nhớ" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000056"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Nhận biết các loại đường", new Guid("00000000-0000-0000-0000-000000000011"), "5.1 - Đường thẳng, đường cong, đường gấp khúc" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000057"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Thực hành vẽ và nhận biết", new Guid("00000000-0000-0000-0000-000000000011"), "5.2 - Luyện tập - Các loại đường" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000058"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "1 giờ = 60 phút. Chuyển đổi đơn vị thời gian", new Guid("00000000-0000-0000-0000-000000000011"), "6.1 - Ngày - Giờ, Giờ - Phút" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000059"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Số ngày trong các tháng", new Guid("00000000-0000-0000-0000-000000000011"), "6.2 - Ngày - Tháng" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000060"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Ôn tập về thời gian", new Guid("00000000-0000-0000-0000-000000000011"), "6.3 - Luyện tập - Thời gian" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000061"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Ôn tập tổng hợp chương trình lớp 2", new Guid("00000000-0000-0000-0000-000000000011"), "6.4 - Ôn tập cuối năm lớp 2" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000062"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Đọc, viết, so sánh số đến 1000", new Guid("00000000-0000-0000-0000-000000000021"), "1.1 - Các số đến 1000" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000063"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Cộng trừ số có 3 chữ số", new Guid("00000000-0000-0000-0000-000000000021"), "1.2 - Phép cộng, phép trừ trong phạm vi 1000" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000064"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Ôn tập bảng nhân chia 2 và 5", new Guid("00000000-0000-0000-0000-000000000021"), "1.3 - Bảng nhân 2, 5 và Bảng chia 2, 5" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000065"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Học thuộc bảng nhân 3, 4", new Guid("00000000-0000-0000-0000-000000000021"), "2.1 - Bảng nhân 3, 4" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000066"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Học thuộc bảng nhân 6, 7, 8, 9", new Guid("00000000-0000-0000-0000-000000000021"), "2.2 - Bảng nhân 6, 7, 8, 9" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000067"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Nhân số có 2-3 chữ số với 1 chữ số", new Guid("00000000-0000-0000-0000-000000000021"), "2.3 - Phép nhân (2-3 chữ số x 1 chữ số)" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000068"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Chia số có 2-3 chữ số cho 1 chữ số", new Guid("00000000-0000-0000-0000-000000000021"), "2.4 - Phép chia (2-3 chữ số ÷ 1 chữ số)" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000069"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Nhận biết và đo các loại góc", new Guid("00000000-0000-0000-0000-000000000021"), "3.1 - Góc: Vuông, nhọn, tù" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000070"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Tính chất và nhận biết các hình", new Guid("00000000-0000-0000-0000-000000000021"), "3.2 - Hình: Tam giác, tứ giác, chữ nhật, vuông, tròn" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000071"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Các đơn vị đo độ dài và chuyển đổi", new Guid("00000000-0000-0000-0000-000000000021"), "3.3 - Đơn vị đo độ dài: mm, cm, dm, m, km" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000072"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Đơn vị đo khối lượng", new Guid("00000000-0000-0000-0000-000000000021"), "3.4 - Khối lượng: g, kg" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000073"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Đơn vị đo dung tích", new Guid("00000000-0000-0000-0000-000000000021"), "3.5 - Dung tích: ml, l" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000074"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Làm quen với thống kê cơ bản", new Guid("00000000-0000-0000-0000-000000000021"), "4.1 - Thu thập và phân loại dữ liệu" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000075"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Làm quen với xác suất đơn giản", new Guid("00000000-0000-0000-0000-000000000021"), "4.2 - Khả năng xảy ra của sự kiện" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000076"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Đọc, viết số có 5 chữ số", new Guid("00000000-0000-0000-0000-000000000021"), "5.1 - Các số đến 10,000" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000077"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Đọc, viết số có 6 chữ số", new Guid("00000000-0000-0000-0000-000000000021"), "5.2 - Các số đến 100,000" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000078"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Hàng và lớp của số", new Guid("00000000-0000-0000-0000-000000000021"), "5.3 - Cấu tạo thập phân" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000079"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "So sánh các số lớn", new Guid("00000000-0000-0000-0000-000000000021"), "5.4 - So sánh số trong phạm vi 100,000" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000080"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Cộng trừ số có nhiều chữ số", new Guid("00000000-0000-0000-0000-000000000021"), "6.1 - Cộng, trừ có nhớ (số lớn)" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000081"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Nhân chia trong phạm vi 100,000", new Guid("00000000-0000-0000-0000-000000000021"), "6.2 - Nhân, chia số lớn" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000082"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Thứ tự thực hiện phép tính", new Guid("00000000-0000-0000-0000-000000000021"), "6.3 - Tính giá trị biểu thức" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000083"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Công thức tính chu vi các hình", new Guid("00000000-0000-0000-0000-000000000021"), "7.1 - Chu vi: Tam giác, tứ giác, chữ nhật, vuông" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000084"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "S = dài x rộng, S = cạnh x cạnh", new Guid("00000000-0000-0000-0000-000000000021"), "7.2 - Diện tích: Chữ nhật, vuông" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000085"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Đơn vị tiền tệ và quy đổi", new Guid("00000000-0000-0000-0000-000000000021"), "7.3 - Tiền Việt Nam" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000086"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Đọc giờ chính xác", new Guid("00000000-0000-0000-0000-000000000021"), "8.1 - Xem đồng hồ (chính xác đến phút)" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000087"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Năm nhuận, năm thường", new Guid("00000000-0000-0000-0000-000000000021"), "8.2 - Tháng - Năm" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000088"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Bài toán tìm số khi biết gấp/giảm", new Guid("00000000-0000-0000-0000-000000000021"), "9.1 - Toán gấp, giảm số" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000089"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Bài toán so sánh hai số", new Guid("00000000-0000-0000-0000-000000000021"), "9.2 - Toán so sánh" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000090"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Bài toán nhiều phép tính", new Guid("00000000-0000-0000-0000-000000000021"), "9.3 - Toán hai bước tính" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000091"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Ôn tập tổng hợp chương trình lớp 3", new Guid("00000000-0000-0000-0000-000000000021"), "9.4 - Ôn tập cuối năm lớp 3" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000092"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Đọc, viết số có 6 chữ số", new Guid("00000000-0000-0000-0000-000000000031"), "1.1 - Số có 6 chữ số" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000093"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Cấu tạo số theo hàng và lớp", new Guid("00000000-0000-0000-0000-000000000031"), "1.2 - Hàng và lớp (triệu, nghìn, đơn vị)" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000094"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "So sánh số có 6 chữ số", new Guid("00000000-0000-0000-0000-000000000031"), "1.3 - So sánh số tự nhiên lớn" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000095"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Làm tròn đến chục, trăm, nghìn", new Guid("00000000-0000-0000-0000-000000000031"), "1.4 - Làm tròn số" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000096"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Cộng trừ số có 6 chữ số", new Guid("00000000-0000-0000-0000-000000000031"), "2.1 - Cộng, trừ số lớn" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000097"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Thuật toán nhân số có 2 chữ số", new Guid("00000000-0000-0000-0000-000000000031"), "2.2 - Nhân với số có 2 chữ số" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000098"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Thuật toán chia số lớn", new Guid("00000000-0000-0000-0000-000000000031"), "2.3 - Chia số có nhiều chữ số" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000099"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Phân loại và đo góc", new Guid("00000000-0000-0000-0000-000000000031"), "3.1 - Các loại góc: Vuông, nhọn, tù, bẹt" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000100"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Tính chất và cách vẽ", new Guid("00000000-0000-0000-0000-000000000031"), "3.2 - Đường thẳng song song" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000101"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Tính chất và cách vẽ", new Guid("00000000-0000-0000-0000-000000000031"), "3.3 - Đường thẳng vuông góc" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000102"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "1 yến = 10kg, 1 tạ = 100kg, 1 tấn = 1000kg", new Guid("00000000-0000-0000-0000-000000000031"), "4.1 - Khối lượng: Yến, Tạ, Tấn" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000103"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Các đơn vị đo diện tích", new Guid("00000000-0000-0000-0000-000000000031"), "4.2 - Diện tích: mm², cm², dm², m²" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000104"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Tử số, mẫu số, ý nghĩa phân số", new Guid("00000000-0000-0000-0000-000000000031"), "5.1 - Khái niệm phân số" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000105"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Tính chất cơ bản của phân số", new Guid("00000000-0000-0000-0000-000000000031"), "5.2 - Phân số bằng nhau" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000106"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Chia cả tử và mẫu cho ƯCLN", new Guid("00000000-0000-0000-0000-000000000031"), "5.3 - Rút gọn phân số" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000107"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Nhân cả tử và mẫu với cùng một số", new Guid("00000000-0000-0000-0000-000000000031"), "5.4 - Quy đồng mẫu số" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000108"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "So sánh phân số cùng mẫu, khác mẫu", new Guid("00000000-0000-0000-0000-000000000031"), "5.5 - So sánh phân số" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000109"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Cộng phân số cùng mẫu và khác mẫu", new Guid("00000000-0000-0000-0000-000000000031"), "6.1 - Cộng phân số" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000110"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Trừ phân số cùng mẫu và khác mẫu", new Guid("00000000-0000-0000-0000-000000000031"), "6.2 - Trừ phân số" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000111"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Nhân tử với tử, mẫu với mẫu", new Guid("00000000-0000-0000-0000-000000000031"), "6.3 - Nhân phân số" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000112"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Nhân với phân số nghịch đảo", new Guid("00000000-0000-0000-0000-000000000031"), "6.4 - Chia phân số" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000113"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "S = đáy x cao", new Guid("00000000-0000-0000-0000-000000000031"), "7.1 - Hình bình hành" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000114"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "S = (d₁ x d₂) / 2", new Guid("00000000-0000-0000-0000-000000000031"), "7.2 - Hình thoi" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000115"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Dạng toán tổng hiệu", new Guid("00000000-0000-0000-0000-000000000031"), "8.1 - Tìm 2 số biết Tổng và Hiệu" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000116"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Dạng toán tổng tỉ", new Guid("00000000-0000-0000-0000-000000000031"), "8.2 - Tìm 2 số biết Tổng và Tỉ" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000117"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Dạng toán hiệu tỉ", new Guid("00000000-0000-0000-0000-000000000031"), "8.3 - Tìm 2 số biết Hiệu và Tỉ" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000118"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Đọc và vẽ biểu đồ cột", new Guid("00000000-0000-0000-0000-000000000031"), "9.1 - Biểu đồ cột" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000119"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Đọc và vẽ biểu đồ tranh", new Guid("00000000-0000-0000-0000-000000000031"), "9.2 - Biểu đồ tranh" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000120"),
                columns: new[] { "Description", "SubjectId", "Title" },
                values: new object[] { "Ôn tập tổng hợp chương trình lớp 4", new Guid("00000000-0000-0000-0000-000000000031"), "9.3 - Ôn tập cuối năm lớp 4" });
        }
    }
}
