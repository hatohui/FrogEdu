using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FrogEdu.Exam.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedMathTopics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                schema: "public",
                table: "Topics",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldMaxLength: 256);

            migrationBuilder.InsertData(
                schema: "public",
                table: "Topics",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Description", "IsCurriculum", "SubjectId", "Title", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Nhận biết và đọc số 0, 1, 2, 3, 4, 5. Tập viết số. Đếm số lượng đồ vật.", true, new Guid("00000000-0000-0000-0000-000000000001"), "Các số 0, 1, 2, 3, 4, 5", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000002"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Nhận biết và đọc số 6, 7, 8, 9, 10. Tập viết số.", true, new Guid("00000000-0000-0000-0000-000000000001"), "Các số 6, 7, 8, 9, 10", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000003"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "So sánh số lượng. Xác định: Nhiều hơn, ít hơn, bằng nhau.", true, new Guid("00000000-0000-0000-0000-000000000001"), "Nhiều hơn, ít hơn, bằng nhau", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000004"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dấu lớn hơn (>), bé hơn (<), bằng (=). Ví dụ: 4 > 3, 2 < 5, 4 = 4", true, new Guid("00000000-0000-0000-0000-000000000001"), "So sánh số", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000005"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Phân tích cấu tạo số. Ví dụ: 5 gồm 3 và 2", true, new Guid("00000000-0000-0000-0000-000000000001"), "Mấy và mấy", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000006"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn tập và luyện tập các số từ 0 đến 10", true, new Guid("00000000-0000-0000-0000-000000000001"), "Luyện tập chung - Các số từ 0 đến 10", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000007"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Nhận biết các hình phẳng cơ bản", true, new Guid("00000000-0000-0000-0000-000000000001"), "Hình vuông, hình tròn, hình tam giác, hình chữ nhật", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000008"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Thực hành với các hình phẳng", true, new Guid("00000000-0000-0000-0000-000000000001"), "Thực hành lắp ghép, xếp hình", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000009"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn tập các hình phẳng", true, new Guid("00000000-0000-0000-0000-000000000001"), "Luyện tập chung - Hình phẳng", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000010"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ý nghĩa: Gộp lại. Ví dụ: 3 + 2 = 5", true, new Guid("00000000-0000-0000-0000-000000000001"), "Phép cộng trong phạm vi 10", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000011"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ý nghĩa: Bớt đi. Ví dụ: 6 - 1 = 5", true, new Guid("00000000-0000-0000-0000-000000000001"), "Phép trừ trong phạm vi 10", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000012"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Học thuộc bảng cộng trừ trong phạm vi 10", true, new Guid("00000000-0000-0000-0000-000000000001"), "Bảng cộng, bảng trừ trong phạm vi 10", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000013"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn tập phép cộng, phép trừ", true, new Guid("00000000-0000-0000-0000-000000000001"), "Luyện tập chung - Cộng trừ trong phạm vi 10", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000014"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Nhận biết các hình khối cơ bản", true, new Guid("00000000-0000-0000-0000-000000000001"), "Khối lập phương, khối hộp chữ nhật", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000015"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Phải - Trái, Trên - Dưới, Trước - Sau", true, new Guid("00000000-0000-0000-0000-000000000001"), "Vị trí, định hướng trong không gian", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000016"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn tập hình khối và vị trí", true, new Guid("00000000-0000-0000-0000-000000000001"), "Luyện tập chung - Hình khối", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000017"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn tập tổng hợp các số 0-10", true, new Guid("00000000-0000-0000-0000-000000000001"), "Ôn tập các số trong phạm vi 10", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000018"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn tập các phép tính", true, new Guid("00000000-0000-0000-0000-000000000001"), "Ôn tập phép cộng, phép trừ", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000019"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn tập hình phẳng và hình khối", true, new Guid("00000000-0000-0000-0000-000000000001"), "Ôn tập hình học", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000020"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn tập tổng hợp học kì 1", true, new Guid("00000000-0000-0000-0000-000000000001"), "Ôn tập chung học kì 1", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000021"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Nhận biết Chục và Đơn vị. Cách đọc: 24 = hai mươi tư", true, new Guid("00000000-0000-0000-0000-000000000001"), "Số có hai chữ số", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000022"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "So sánh hàng chục trước. Ví dụ: 42 > 25", true, new Guid("00000000-0000-0000-0000-000000000001"), "So sánh số có hai chữ số", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000023"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Số 100 = một trăm. Làm quen với bảng số", true, new Guid("00000000-0000-0000-0000-000000000001"), "Bảng các số từ 1 đến 100", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000024"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn tập các số đến 100", true, new Guid("00000000-0000-0000-0000-000000000001"), "Luyện tập chung - Các số đến 100", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000025"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "So sánh độ dài của các vật", true, new Guid("00000000-0000-0000-0000-000000000001"), "Dài hơn, ngắn hơn", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000026"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Làm quen với đơn vị cm", true, new Guid("00000000-0000-0000-0000-000000000001"), "Đơn vị đo độ dài (cm)", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000027"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Thực hành đo đạc", true, new Guid("00000000-0000-0000-0000-000000000001"), "Thực hành ước lượng và đo độ dài", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000028"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn tập đo độ dài", true, new Guid("00000000-0000-0000-0000-000000000001"), "Luyện tập chung - Độ dài", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000029"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Cộng số có hai chữ số với một chữ số", true, new Guid("00000000-0000-0000-0000-000000000001"), "Phép cộng (2 chữ số + 1 chữ số)", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000030"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Cộng hai số có hai chữ số", true, new Guid("00000000-0000-0000-0000-000000000001"), "Phép cộng (2 chữ số + 2 chữ số)", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000031"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Trừ số có hai chữ số cho một chữ số", true, new Guid("00000000-0000-0000-0000-000000000001"), "Phép trừ (2 chữ số - 1 chữ số)", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000032"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Trừ hai số có hai chữ số", true, new Guid("00000000-0000-0000-0000-000000000001"), "Phép trừ (2 chữ số - 2 chữ số)", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000033"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn tập cộng trừ", true, new Guid("00000000-0000-0000-0000-000000000001"), "Luyện tập chung - Cộng trừ trong phạm vi 100", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000034"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Học cách xem giờ", true, new Guid("00000000-0000-0000-0000-000000000001"), "Xem giờ đúng trên đồng hồ", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000035"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Tên các ngày trong tuần", true, new Guid("00000000-0000-0000-0000-000000000001"), "Các ngày trong tuần", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000036"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Thực hành với lịch và đồng hồ", true, new Guid("00000000-0000-0000-0000-000000000001"), "Thực hành xem lịch và giờ", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000037"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn tập về thời gian", true, new Guid("00000000-0000-0000-0000-000000000001"), "Luyện tập chung - Thời gian", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000038"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn tập các số và phép tính trong phạm vi 10", true, new Guid("00000000-0000-0000-0000-000000000001"), "Ôn tập các số và phép tính (0-10)", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000039"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn tập các số và phép tính trong phạm vi 100", true, new Guid("00000000-0000-0000-0000-000000000001"), "Ôn tập các số và phép tính (0-100)", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000040"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn tập hình học, độ dài, thời gian", true, new Guid("00000000-0000-0000-0000-000000000001"), "Ôn tập hình học và đo lường", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000041"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn tập tổng hợp toàn bộ chương trình lớp 1", true, new Guid("00000000-0000-0000-0000-000000000001"), "Ôn tập chung cuối năm lớp 1", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000042"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn tập kiến thức lớp 1", true, new Guid("00000000-0000-0000-0000-000000000011"), "Ôn tập các số đến 100", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000043"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn tập các phép tính cơ bản", true, new Guid("00000000-0000-0000-0000-000000000011"), "Ôn tập phép cộng, phép trừ không nhớ", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000044"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Phương pháp: Tách số để tròn chục. Ví dụ: 9 + 5 = 9 + 1 + 4 = 14", true, new Guid("00000000-0000-0000-0000-000000000011"), "Phép cộng qua 10 trong phạm vi 20", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000045"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Thực hành phép cộng qua 10", true, new Guid("00000000-0000-0000-0000-000000000011"), "Luyện tập phép cộng qua 10", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000046"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Phương pháp: Trừ để tròn chục. Ví dụ: 12 - 5 = 12 - 2 - 3 = 7", true, new Guid("00000000-0000-0000-0000-000000000011"), "Phép trừ qua 10 trong phạm vi 20", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000047"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Thực hành phép trừ qua 10", true, new Guid("00000000-0000-0000-0000-000000000011"), "Luyện tập phép trừ qua 10", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000048"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn tập cộng trừ qua 10", true, new Guid("00000000-0000-0000-0000-000000000011"), "Luyện tập chung - Cộng trừ qua 10", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000049"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Khối trụ: Hai mặt đáy tròn (hộp sữa). Khối cầu: Tròn đều (quả bóng)", true, new Guid("00000000-0000-0000-0000-000000000011"), "Khối trụ, khối cầu", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000050"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Thực hành nhận biết khối trụ, khối cầu", true, new Guid("00000000-0000-0000-0000-000000000011"), "Luyện tập - Hình khối", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000051"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Phép cộng có nhớ trong phạm vi 100. Ví dụ: 38 + 25 = 63", true, new Guid("00000000-0000-0000-0000-000000000011"), "Phép cộng có nhớ", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000052"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Thực hành phép cộng có nhớ", true, new Guid("00000000-0000-0000-0000-000000000011"), "Luyện tập phép cộng có nhớ", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000053"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Phép trừ có nhớ trong phạm vi 100. Ví dụ: 52 - 27 = 25", true, new Guid("00000000-0000-0000-0000-000000000011"), "Phép trừ có nhớ", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000054"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Thực hành phép trừ có nhớ", true, new Guid("00000000-0000-0000-0000-000000000011"), "Luyện tập phép trừ có nhớ", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000055"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn tập cộng trừ có nhớ", true, new Guid("00000000-0000-0000-0000-000000000011"), "Luyện tập chung - Cộng trừ có nhớ", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000056"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Nhận biết các loại đường", true, new Guid("00000000-0000-0000-0000-000000000011"), "Đường thẳng, đường cong, đường gấp khúc", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000057"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Thực hành vẽ và nhận biết", true, new Guid("00000000-0000-0000-0000-000000000011"), "Luyện tập - Các loại đường", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000058"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "1 giờ = 60 phút. Chuyển đổi đơn vị thời gian", true, new Guid("00000000-0000-0000-0000-000000000011"), "Ngày - Giờ, Giờ - Phút", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000059"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Số ngày trong các tháng", true, new Guid("00000000-0000-0000-0000-000000000011"), "Ngày - Tháng", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000060"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn tập về thời gian", true, new Guid("00000000-0000-0000-0000-000000000011"), "Luyện tập - Thời gian", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000061"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn tập tổng hợp chương trình lớp 2", true, new Guid("00000000-0000-0000-0000-000000000011"), "Ôn tập cuối năm lớp 2", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000062"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Đọc, viết, so sánh số đến 1000", true, new Guid("00000000-0000-0000-0000-000000000021"), "Các số đến 1000", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000063"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Cộng trừ số có 3 chữ số", true, new Guid("00000000-0000-0000-0000-000000000021"), "Phép cộng, phép trừ trong phạm vi 1000", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000064"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn tập bảng nhân chia 2 và 5", true, new Guid("00000000-0000-0000-0000-000000000021"), "Bảng nhân 2, 5 và Bảng chia 2, 5", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000065"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Học thuộc bảng nhân 3, 4", true, new Guid("00000000-0000-0000-0000-000000000021"), "Bảng nhân 3, 4", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000066"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Học thuộc bảng nhân 6, 7, 8, 9", true, new Guid("00000000-0000-0000-0000-000000000021"), "Bảng nhân 6, 7, 8, 9", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000067"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Nhân số có 2-3 chữ số với 1 chữ số", true, new Guid("00000000-0000-0000-0000-000000000021"), "Phép nhân (2-3 chữ số × 1 chữ số)", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000068"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Chia số có 2-3 chữ số cho 1 chữ số", true, new Guid("00000000-0000-0000-0000-000000000021"), "Phép chia (2-3 chữ số ÷ 1 chữ số)", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000069"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Nhận biết và đo các loại góc", true, new Guid("00000000-0000-0000-0000-000000000021"), "Góc: Vuông, nhọn, tù", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000070"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Tính chất và nhận biết các hình", true, new Guid("00000000-0000-0000-0000-000000000021"), "Hình: Tam giác, tứ giác, chữ nhật, vuông, tròn", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000071"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Các đơn vị đo độ dài và chuyển đổi", true, new Guid("00000000-0000-0000-0000-000000000021"), "Đơn vị đo độ dài: mm, cm, dm, m, km", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000072"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Đơn vị đo khối lượng", true, new Guid("00000000-0000-0000-0000-000000000021"), "Khối lượng: g, kg", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000073"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Đơn vị đo dung tích", true, new Guid("00000000-0000-0000-0000-000000000021"), "Dung tích: ml, l", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000074"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Làm quen với thống kê cơ bản", true, new Guid("00000000-0000-0000-0000-000000000021"), "Thu thập và phân loại dữ liệu", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000075"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Làm quen với xác suất đơn giản", true, new Guid("00000000-0000-0000-0000-000000000021"), "Khả năng xảy ra của sự kiện", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000076"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Đọc, viết số có 5 chữ số", true, new Guid("00000000-0000-0000-0000-000000000021"), "Các số đến 10,000", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000077"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Đọc, viết số có 6 chữ số", true, new Guid("00000000-0000-0000-0000-000000000021"), "Các số đến 100,000", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000078"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Hàng và lớp của số", true, new Guid("00000000-0000-0000-0000-000000000021"), "Cấu tạo thập phân", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000079"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "So sánh các số lớn", true, new Guid("00000000-0000-0000-0000-000000000021"), "So sánh số trong phạm vi 100,000", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000080"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Cộng trừ số có nhiều chữ số", true, new Guid("00000000-0000-0000-0000-000000000021"), "Cộng, trừ có nhớ (số lớn)", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000081"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Nhân chia trong phạm vi 100,000", true, new Guid("00000000-0000-0000-0000-000000000021"), "Nhân, chia số lớn", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000082"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Thứ tự thực hiện phép tính", true, new Guid("00000000-0000-0000-0000-000000000021"), "Tính giá trị biểu thức", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000083"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Công thức tính chu vi các hình", true, new Guid("00000000-0000-0000-0000-000000000021"), "Chu vi: Tam giác, tứ giác, chữ nhật, vuông", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000084"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "S = dài × rộng, S = cạnh × cạnh", true, new Guid("00000000-0000-0000-0000-000000000021"), "Diện tích: Chữ nhật, vuông", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000085"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Đơn vị tiền tệ và quy đổi", true, new Guid("00000000-0000-0000-0000-000000000021"), "Tiền Việt Nam", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000086"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Đọc giờ chính xác", true, new Guid("00000000-0000-0000-0000-000000000021"), "Xem đồng hồ (chính xác đến phút)", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000087"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Năm nhuận, năm thường", true, new Guid("00000000-0000-0000-0000-000000000021"), "Tháng - Năm", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000088"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bài toán tìm số khi biết gấp/giảm", true, new Guid("00000000-0000-0000-0000-000000000021"), "Toán gấp, giảm số", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000089"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bài toán so sánh hai số", true, new Guid("00000000-0000-0000-0000-000000000021"), "Toán so sánh", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000090"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bài toán nhiều phép tính", true, new Guid("00000000-0000-0000-0000-000000000021"), "Toán hai bước tính", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000091"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn tập tổng hợp chương trình lớp 3", true, new Guid("00000000-0000-0000-0000-000000000021"), "Ôn tập cuối năm lớp 3", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000092"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Đọc, viết số có 6 chữ số", true, new Guid("00000000-0000-0000-0000-000000000031"), "Số có 6 chữ số", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000093"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Cấu tạo số theo hàng và lớp", true, new Guid("00000000-0000-0000-0000-000000000031"), "Hàng và lớp (triệu, nghìn, đơn vị)", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000094"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "So sánh số có 6 chữ số", true, new Guid("00000000-0000-0000-0000-000000000031"), "So sánh số tự nhiên lớn", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000095"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Làm tròn đến chục, trăm, nghìn", true, new Guid("00000000-0000-0000-0000-000000000031"), "Làm tròn số", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000096"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Cộng trừ số có 6 chữ số", true, new Guid("00000000-0000-0000-0000-000000000031"), "Cộng, trừ số lớn", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000097"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Thuật toán nhân số có 2 chữ số", true, new Guid("00000000-0000-0000-0000-000000000031"), "Nhân với số có 2 chữ số", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000098"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Thuật toán chia số lớn", true, new Guid("00000000-0000-0000-0000-000000000031"), "Chia số có nhiều chữ số", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000099"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Phân loại và đo góc", true, new Guid("00000000-0000-0000-0000-000000000031"), "Các loại góc: Vuông, nhọn, tù, bẹt", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000100"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Tính chất và cách vẽ", true, new Guid("00000000-0000-0000-0000-000000000031"), "Đường thẳng song song", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000101"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Tính chất và cách vẽ", true, new Guid("00000000-0000-0000-0000-000000000031"), "Đường thẳng vuông góc", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000102"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "1 yến = 10kg, 1 tạ = 100kg, 1 tấn = 1000kg", true, new Guid("00000000-0000-0000-0000-000000000031"), "Khối lượng: Yến, Tạ, Tấn", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000103"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Các đơn vị đo diện tích", true, new Guid("00000000-0000-0000-0000-000000000031"), "Diện tích: mm², cm², dm², m²", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000104"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Tử số, mẫu số, ý nghĩa phân số", true, new Guid("00000000-0000-0000-0000-000000000031"), "Khái niệm phân số", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000105"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Tính chất cơ bản của phân số", true, new Guid("00000000-0000-0000-0000-000000000031"), "Phân số bằng nhau", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000106"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Chia cả tử và mẫu cho ƯCLN", true, new Guid("00000000-0000-0000-0000-000000000031"), "Rút gọn phân số", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000107"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Nhân cả tử và mẫu với cùng một số", true, new Guid("00000000-0000-0000-0000-000000000031"), "Quy đồng mẫu số", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000108"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "So sánh phân số cùng mẫu, khác mẫu", true, new Guid("00000000-0000-0000-0000-000000000031"), "So sánh phân số", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000109"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Cộng phân số cùng mẫu và khác mẫu", true, new Guid("00000000-0000-0000-0000-000000000031"), "Cộng phân số", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000110"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Trừ phân số cùng mẫu và khác mẫu", true, new Guid("00000000-0000-0000-0000-000000000031"), "Trừ phân số", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000111"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Nhân tử với tử, mẫu với mẫu", true, new Guid("00000000-0000-0000-0000-000000000031"), "Nhân phân số", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000112"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Nhân với phân số nghịch đảo", true, new Guid("00000000-0000-0000-0000-000000000031"), "Chia phân số", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000113"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "S = đáy × cao", true, new Guid("00000000-0000-0000-0000-000000000031"), "Hình bình hành", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000114"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "S = (d₁ × d₂) / 2", true, new Guid("00000000-0000-0000-0000-000000000031"), "Hình thoi", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000115"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dạng toán tổng hiệu", true, new Guid("00000000-0000-0000-0000-000000000031"), "Tìm 2 số biết Tổng và Hiệu", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000116"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dạng toán tổng tỉ", true, new Guid("00000000-0000-0000-0000-000000000031"), "Tìm 2 số biết Tổng và Tỉ", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000117"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dạng toán hiệu tỉ", true, new Guid("00000000-0000-0000-0000-000000000031"), "Tìm 2 số biết Hiệu và Tỉ", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000118"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Đọc và vẽ biểu đồ cột", true, new Guid("00000000-0000-0000-0000-000000000031"), "Biểu đồ cột", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000119"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Đọc và vẽ biểu đồ tranh", true, new Guid("00000000-0000-0000-0000-000000000031"), "Biểu đồ tranh", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000120"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ôn tập tổng hợp chương trình lớp 4", true, new Guid("00000000-0000-0000-0000-000000000031"), "Ôn tập cuối năm lớp 4", null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000010"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000011"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000012"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000013"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000014"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000015"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000016"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000017"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000018"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000019"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000020"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000021"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000022"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000023"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000024"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000025"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000026"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000027"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000028"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000029"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000030"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000031"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000032"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000033"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000034"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000035"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000036"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000037"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000038"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000039"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000040"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000041"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000042"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000043"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000044"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000045"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000046"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000047"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000048"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000049"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000050"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000051"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000052"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000053"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000054"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000055"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000056"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000057"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000058"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000059"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000060"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000061"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000062"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000063"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000064"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000065"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000066"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000067"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000068"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000069"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000070"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000071"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000072"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000073"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000074"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000075"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000076"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000077"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000078"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000079"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000080"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000081"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000082"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000083"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000084"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000085"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000086"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000087"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000088"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000089"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000090"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000091"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000092"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000093"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000094"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000095"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000096"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000097"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000098"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000099"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000100"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000101"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000102"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000103"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000104"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000105"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000106"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000107"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000108"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000109"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000110"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000111"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000112"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000113"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000114"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000115"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000116"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000117"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000118"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000119"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000120"));

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                schema: "public",
                table: "Topics",
                type: "uuid",
                maxLength: 256,
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);
        }
    }
}
