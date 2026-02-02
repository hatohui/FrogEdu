using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrogEdu.Exam.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixCreatedByAuditing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"),
                column: "Title",
                value: "1.1 - Các số 0, 1, 2, 3, 4, 5");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"),
                column: "Title",
                value: "1.2 - Các số 6, 7, 8, 9, 10");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"),
                column: "Title",
                value: "1.3 - Nhiều hơn, ít hơn, bằng nhau");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"),
                column: "Title",
                value: "1.4 - So sánh số");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000005"),
                column: "Title",
                value: "1.5 - Mấy và mấy");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000006"),
                column: "Title",
                value: "1.6 - Luyện tập chung");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000007"),
                column: "Title",
                value: "2.1 - Hình vuông, hình tròn, hình tam giác, hình chữ nhật");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000008"),
                column: "Title",
                value: "2.2 - Thực hành lắp ghép, xếp hình");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000009"),
                column: "Title",
                value: "2.3 - Luyện tập chung");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000010"),
                column: "Title",
                value: "3.1 - Phép cộng trong phạm vi 10");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000011"),
                column: "Title",
                value: "3.2 - Phép trừ trong phạm vi 10");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000012"),
                column: "Title",
                value: "3.3 - Bảng cộng, bảng trừ trong phạm vi 10");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000013"),
                column: "Title",
                value: "3.4 - Luyện tập chung");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000014"),
                column: "Title",
                value: "4.1 - Khối lập phương, khối hộp chữ nhật");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000015"),
                column: "Title",
                value: "4.2 - Vị trí, định hướng trong không gian");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000016"),
                column: "Title",
                value: "4.3 - Luyện tập chung");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000017"),
                column: "Title",
                value: "5.1 - Ôn tập các số trong phạm vi 10");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000018"),
                column: "Title",
                value: "5.2 - Ôn tập phép cộng, phép trừ");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000019"),
                column: "Title",
                value: "5.3 - Ôn tập hình học");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000020"),
                column: "Title",
                value: "5.4 - Ôn tập chung học kì 1");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000021"),
                column: "Title",
                value: "6.1 - Số có hai chữ số");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000022"),
                column: "Title",
                value: "6.2 - So sánh số có hai chữ số");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000023"),
                column: "Title",
                value: "6.3 - Bảng các số từ 1 đến 100");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000024"),
                column: "Title",
                value: "6.4 - Luyện tập chung");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000025"),
                column: "Title",
                value: "7.1 - Dài hơn, ngắn hơn");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000026"),
                column: "Title",
                value: "7.2 - Đơn vị đo độ dài (cm)");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000027"),
                column: "Title",
                value: "7.3 - Thực hành ước lượng và đo độ dài");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000028"),
                column: "Title",
                value: "7.4 - Luyện tập chung");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000029"),
                column: "Title",
                value: "8.1 - Phép cộng (2 chữ số + 1 chữ số)");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000030"),
                column: "Title",
                value: "8.2 - Phép cộng (2 chữ số + 2 chữ số)");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000031"),
                column: "Title",
                value: "8.3 - Phép trừ (2 chữ số - 1 chữ số)");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000032"),
                column: "Title",
                value: "8.4 - Phép trừ (2 chữ số - 2 chữ số)");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000033"),
                column: "Title",
                value: "8.5 - Luyện tập chung");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000034"),
                column: "Title",
                value: "9.1 - Xem giờ đúng trên đồng hồ");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000035"),
                column: "Title",
                value: "9.2 - Các ngày trong tuần");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000036"),
                column: "Title",
                value: "9.3 - Thực hành xem lịch và giờ");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000037"),
                column: "Title",
                value: "9.4 - Luyện tập chung");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000038"),
                column: "Title",
                value: "10.1 - Ôn tập các số và phép tính (0-10)");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000039"),
                column: "Title",
                value: "10.2 - Ôn tập các số và phép tính (0-100)");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000040"),
                column: "Title",
                value: "10.3 - Ôn tập hình học và đo lường");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000041"),
                column: "Title",
                value: "10.4 - Ôn tập chung cuối năm");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000042"),
                column: "Title",
                value: "1.1 - Ôn tập các số đến 100");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000043"),
                column: "Title",
                value: "1.2 - Ôn tập phép cộng, phép trừ không nhớ");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000044"),
                column: "Title",
                value: "2.1 - Phép cộng qua 10 trong phạm vi 20");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000045"),
                column: "Title",
                value: "2.2 - Luyện tập phép cộng qua 10");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000046"),
                column: "Title",
                value: "2.3 - Phép trừ qua 10 trong phạm vi 20");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000047"),
                column: "Title",
                value: "2.4 - Luyện tập phép trừ qua 10");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000048"),
                column: "Title",
                value: "2.5 - Luyện tập chung - Cộng trừ qua 10");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000049"),
                column: "Title",
                value: "3.1 - Khối trụ, khối cầu");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000050"),
                column: "Title",
                value: "3.2 - Luyện tập - Hình khối");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000051"),
                column: "Title",
                value: "4.1 - Phép cộng có nhớ");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000052"),
                column: "Title",
                value: "4.2 - Luyện tập phép cộng có nhớ");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000053"),
                column: "Title",
                value: "4.3 - Phép trừ có nhớ");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000054"),
                column: "Title",
                value: "4.4 - Luyện tập phép trừ có nhớ");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000055"),
                column: "Title",
                value: "4.5 - Luyện tập chung - Cộng trừ có nhớ");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000056"),
                column: "Title",
                value: "5.1 - Đường thẳng, đường cong, đường gấp khúc");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000057"),
                column: "Title",
                value: "5.2 - Luyện tập - Các loại đường");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000058"),
                column: "Title",
                value: "6.1 - Ngày - Giờ, Giờ - Phút");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000059"),
                column: "Title",
                value: "6.2 - Ngày - Tháng");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000060"),
                column: "Title",
                value: "6.3 - Luyện tập - Thời gian");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000061"),
                column: "Title",
                value: "6.4 - Ôn tập cuối năm lớp 2");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000062"),
                column: "Title",
                value: "1.1 - Các số đến 1000");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000063"),
                column: "Title",
                value: "1.2 - Phép cộng, phép trừ trong phạm vi 1000");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000064"),
                column: "Title",
                value: "1.3 - Bảng nhân 2, 5 và Bảng chia 2, 5");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000065"),
                column: "Title",
                value: "2.1 - Bảng nhân 3, 4");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000066"),
                column: "Title",
                value: "2.2 - Bảng nhân 6, 7, 8, 9");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000067"),
                column: "Title",
                value: "2.3 - Phép nhân (2-3 chữ số x 1 chữ số)");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000068"),
                column: "Title",
                value: "2.4 - Phép chia (2-3 chữ số ÷ 1 chữ số)");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000069"),
                column: "Title",
                value: "3.1 - Góc: Vuông, nhọn, tù");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000070"),
                column: "Title",
                value: "3.2 - Hình: Tam giác, tứ giác, chữ nhật, vuông, tròn");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000071"),
                column: "Title",
                value: "3.3 - Đơn vị đo độ dài: mm, cm, dm, m, km");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000072"),
                column: "Title",
                value: "3.4 - Khối lượng: g, kg");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000073"),
                column: "Title",
                value: "3.5 - Dung tích: ml, l");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000074"),
                column: "Title",
                value: "4.1 - Thu thập và phân loại dữ liệu");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000075"),
                column: "Title",
                value: "4.2 - Khả năng xảy ra của sự kiện");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000076"),
                column: "Title",
                value: "5.1 - Các số đến 10,000");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000077"),
                column: "Title",
                value: "5.2 - Các số đến 100,000");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000078"),
                column: "Title",
                value: "5.3 - Cấu tạo thập phân");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000079"),
                column: "Title",
                value: "5.4 - So sánh số trong phạm vi 100,000");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000080"),
                column: "Title",
                value: "6.1 - Cộng, trừ có nhớ (số lớn)");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000081"),
                column: "Title",
                value: "6.2 - Nhân, chia số lớn");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000082"),
                column: "Title",
                value: "6.3 - Tính giá trị biểu thức");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000083"),
                column: "Title",
                value: "7.1 - Chu vi: Tam giác, tứ giác, chữ nhật, vuông");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000084"),
                columns: new[] { "Description", "Title" },
                values: new object[] { "S = dài x rộng, S = cạnh x cạnh", "7.2 - Diện tích: Chữ nhật, vuông" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000085"),
                column: "Title",
                value: "7.3 - Tiền Việt Nam");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000086"),
                column: "Title",
                value: "8.1 - Xem đồng hồ (chính xác đến phút)");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000087"),
                column: "Title",
                value: "8.2 - Tháng - Năm");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000088"),
                column: "Title",
                value: "9.1 - Toán gấp, giảm số");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000089"),
                column: "Title",
                value: "9.2 - Toán so sánh");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000090"),
                column: "Title",
                value: "9.3 - Toán hai bước tính");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000091"),
                column: "Title",
                value: "9.4 - Ôn tập cuối năm lớp 3");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000092"),
                column: "Title",
                value: "1.1 - Số có 6 chữ số");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000093"),
                column: "Title",
                value: "1.2 - Hàng và lớp (triệu, nghìn, đơn vị)");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000094"),
                column: "Title",
                value: "1.3 - So sánh số tự nhiên lớn");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000095"),
                column: "Title",
                value: "1.4 - Làm tròn số");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000096"),
                column: "Title",
                value: "2.1 - Cộng, trừ số lớn");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000097"),
                column: "Title",
                value: "2.2 - Nhân với số có 2 chữ số");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000098"),
                column: "Title",
                value: "2.3 - Chia số có nhiều chữ số");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000099"),
                column: "Title",
                value: "3.1 - Các loại góc: Vuông, nhọn, tù, bẹt");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000100"),
                column: "Title",
                value: "3.2 - Đường thẳng song song");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000101"),
                column: "Title",
                value: "3.3 - Đường thẳng vuông góc");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000102"),
                column: "Title",
                value: "4.1 - Khối lượng: Yến, Tạ, Tấn");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000103"),
                column: "Title",
                value: "4.2 - Diện tích: mm², cm², dm², m²");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000104"),
                column: "Title",
                value: "5.1 - Khái niệm phân số");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000105"),
                column: "Title",
                value: "5.2 - Phân số bằng nhau");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000106"),
                column: "Title",
                value: "5.3 - Rút gọn phân số");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000107"),
                column: "Title",
                value: "5.4 - Quy đồng mẫu số");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000108"),
                column: "Title",
                value: "5.5 - So sánh phân số");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000109"),
                column: "Title",
                value: "6.1 - Cộng phân số");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000110"),
                column: "Title",
                value: "6.2 - Trừ phân số");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000111"),
                column: "Title",
                value: "6.3 - Nhân phân số");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000112"),
                column: "Title",
                value: "6.4 - Chia phân số");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000113"),
                columns: new[] { "Description", "Title" },
                values: new object[] { "S = đáy x cao", "7.1 - Hình bình hành" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000114"),
                columns: new[] { "Description", "Title" },
                values: new object[] { "S = (d₁ x d₂) / 2", "7.2 - Hình thoi" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000115"),
                column: "Title",
                value: "8.1 - Tìm 2 số biết Tổng và Hiệu");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000116"),
                column: "Title",
                value: "8.2 - Tìm 2 số biết Tổng và Tỉ");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000117"),
                column: "Title",
                value: "8.3 - Tìm 2 số biết Hiệu và Tỉ");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000118"),
                column: "Title",
                value: "9.1 - Biểu đồ cột");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000119"),
                column: "Title",
                value: "9.2 - Biểu đồ tranh");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000120"),
                column: "Title",
                value: "9.3 - Ôn tập cuối năm lớp 4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"),
                column: "Title",
                value: "Các số 0, 1, 2, 3, 4, 5");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"),
                column: "Title",
                value: "Các số 6, 7, 8, 9, 10");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"),
                column: "Title",
                value: "Nhiều hơn, ít hơn, bằng nhau");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"),
                column: "Title",
                value: "So sánh số");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000005"),
                column: "Title",
                value: "Mấy và mấy");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000006"),
                column: "Title",
                value: "Luyện tập chung - Các số từ 0 đến 10");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000007"),
                column: "Title",
                value: "Hình vuông, hình tròn, hình tam giác, hình chữ nhật");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000008"),
                column: "Title",
                value: "Thực hành lắp ghép, xếp hình");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000009"),
                column: "Title",
                value: "Luyện tập chung - Hình phẳng");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000010"),
                column: "Title",
                value: "Phép cộng trong phạm vi 10");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000011"),
                column: "Title",
                value: "Phép trừ trong phạm vi 10");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000012"),
                column: "Title",
                value: "Bảng cộng, bảng trừ trong phạm vi 10");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000013"),
                column: "Title",
                value: "Luyện tập chung - Cộng trừ trong phạm vi 10");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000014"),
                column: "Title",
                value: "Khối lập phương, khối hộp chữ nhật");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000015"),
                column: "Title",
                value: "Vị trí, định hướng trong không gian");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000016"),
                column: "Title",
                value: "Luyện tập chung - Hình khối");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000017"),
                column: "Title",
                value: "Ôn tập các số trong phạm vi 10");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000018"),
                column: "Title",
                value: "Ôn tập phép cộng, phép trừ");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000019"),
                column: "Title",
                value: "Ôn tập hình học");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000020"),
                column: "Title",
                value: "Ôn tập chung học kì 1");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000021"),
                column: "Title",
                value: "Số có hai chữ số");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000022"),
                column: "Title",
                value: "So sánh số có hai chữ số");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000023"),
                column: "Title",
                value: "Bảng các số từ 1 đến 100");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000024"),
                column: "Title",
                value: "Luyện tập chung - Các số đến 100");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000025"),
                column: "Title",
                value: "Dài hơn, ngắn hơn");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000026"),
                column: "Title",
                value: "Đơn vị đo độ dài (cm)");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000027"),
                column: "Title",
                value: "Thực hành ước lượng và đo độ dài");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000028"),
                column: "Title",
                value: "Luyện tập chung - Độ dài");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000029"),
                column: "Title",
                value: "Phép cộng (2 chữ số + 1 chữ số)");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000030"),
                column: "Title",
                value: "Phép cộng (2 chữ số + 2 chữ số)");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000031"),
                column: "Title",
                value: "Phép trừ (2 chữ số - 1 chữ số)");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000032"),
                column: "Title",
                value: "Phép trừ (2 chữ số - 2 chữ số)");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000033"),
                column: "Title",
                value: "Luyện tập chung - Cộng trừ trong phạm vi 100");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000034"),
                column: "Title",
                value: "Xem giờ đúng trên đồng hồ");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000035"),
                column: "Title",
                value: "Các ngày trong tuần");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000036"),
                column: "Title",
                value: "Thực hành xem lịch và giờ");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000037"),
                column: "Title",
                value: "Luyện tập chung - Thời gian");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000038"),
                column: "Title",
                value: "Ôn tập các số và phép tính (0-10)");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000039"),
                column: "Title",
                value: "Ôn tập các số và phép tính (0-100)");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000040"),
                column: "Title",
                value: "Ôn tập hình học và đo lường");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000041"),
                column: "Title",
                value: "Ôn tập chung cuối năm lớp 1");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000042"),
                column: "Title",
                value: "Ôn tập các số đến 100");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000043"),
                column: "Title",
                value: "Ôn tập phép cộng, phép trừ không nhớ");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000044"),
                column: "Title",
                value: "Phép cộng qua 10 trong phạm vi 20");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000045"),
                column: "Title",
                value: "Luyện tập phép cộng qua 10");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000046"),
                column: "Title",
                value: "Phép trừ qua 10 trong phạm vi 20");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000047"),
                column: "Title",
                value: "Luyện tập phép trừ qua 10");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000048"),
                column: "Title",
                value: "Luyện tập chung - Cộng trừ qua 10");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000049"),
                column: "Title",
                value: "Khối trụ, khối cầu");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000050"),
                column: "Title",
                value: "Luyện tập - Hình khối");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000051"),
                column: "Title",
                value: "Phép cộng có nhớ");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000052"),
                column: "Title",
                value: "Luyện tập phép cộng có nhớ");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000053"),
                column: "Title",
                value: "Phép trừ có nhớ");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000054"),
                column: "Title",
                value: "Luyện tập phép trừ có nhớ");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000055"),
                column: "Title",
                value: "Luyện tập chung - Cộng trừ có nhớ");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000056"),
                column: "Title",
                value: "Đường thẳng, đường cong, đường gấp khúc");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000057"),
                column: "Title",
                value: "Luyện tập - Các loại đường");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000058"),
                column: "Title",
                value: "Ngày - Giờ, Giờ - Phút");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000059"),
                column: "Title",
                value: "Ngày - Tháng");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000060"),
                column: "Title",
                value: "Luyện tập - Thời gian");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000061"),
                column: "Title",
                value: "Ôn tập cuối năm lớp 2");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000062"),
                column: "Title",
                value: "Các số đến 1000");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000063"),
                column: "Title",
                value: "Phép cộng, phép trừ trong phạm vi 1000");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000064"),
                column: "Title",
                value: "Bảng nhân 2, 5 và Bảng chia 2, 5");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000065"),
                column: "Title",
                value: "Bảng nhân 3, 4");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000066"),
                column: "Title",
                value: "Bảng nhân 6, 7, 8, 9");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000067"),
                column: "Title",
                value: "Phép nhân (2-3 chữ số × 1 chữ số)");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000068"),
                column: "Title",
                value: "Phép chia (2-3 chữ số ÷ 1 chữ số)");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000069"),
                column: "Title",
                value: "Góc: Vuông, nhọn, tù");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000070"),
                column: "Title",
                value: "Hình: Tam giác, tứ giác, chữ nhật, vuông, tròn");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000071"),
                column: "Title",
                value: "Đơn vị đo độ dài: mm, cm, dm, m, km");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000072"),
                column: "Title",
                value: "Khối lượng: g, kg");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000073"),
                column: "Title",
                value: "Dung tích: ml, l");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000074"),
                column: "Title",
                value: "Thu thập và phân loại dữ liệu");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000075"),
                column: "Title",
                value: "Khả năng xảy ra của sự kiện");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000076"),
                column: "Title",
                value: "Các số đến 10,000");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000077"),
                column: "Title",
                value: "Các số đến 100,000");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000078"),
                column: "Title",
                value: "Cấu tạo thập phân");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000079"),
                column: "Title",
                value: "So sánh số trong phạm vi 100,000");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000080"),
                column: "Title",
                value: "Cộng, trừ có nhớ (số lớn)");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000081"),
                column: "Title",
                value: "Nhân, chia số lớn");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000082"),
                column: "Title",
                value: "Tính giá trị biểu thức");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000083"),
                column: "Title",
                value: "Chu vi: Tam giác, tứ giác, chữ nhật, vuông");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000084"),
                columns: new[] { "Description", "Title" },
                values: new object[] { "S = dài × rộng, S = cạnh × cạnh", "Diện tích: Chữ nhật, vuông" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000085"),
                column: "Title",
                value: "Tiền Việt Nam");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000086"),
                column: "Title",
                value: "Xem đồng hồ (chính xác đến phút)");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000087"),
                column: "Title",
                value: "Tháng - Năm");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000088"),
                column: "Title",
                value: "Toán gấp, giảm số");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000089"),
                column: "Title",
                value: "Toán so sánh");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000090"),
                column: "Title",
                value: "Toán hai bước tính");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000091"),
                column: "Title",
                value: "Ôn tập cuối năm lớp 3");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000092"),
                column: "Title",
                value: "Số có 6 chữ số");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000093"),
                column: "Title",
                value: "Hàng và lớp (triệu, nghìn, đơn vị)");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000094"),
                column: "Title",
                value: "So sánh số tự nhiên lớn");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000095"),
                column: "Title",
                value: "Làm tròn số");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000096"),
                column: "Title",
                value: "Cộng, trừ số lớn");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000097"),
                column: "Title",
                value: "Nhân với số có 2 chữ số");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000098"),
                column: "Title",
                value: "Chia số có nhiều chữ số");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000099"),
                column: "Title",
                value: "Các loại góc: Vuông, nhọn, tù, bẹt");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000100"),
                column: "Title",
                value: "Đường thẳng song song");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000101"),
                column: "Title",
                value: "Đường thẳng vuông góc");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000102"),
                column: "Title",
                value: "Khối lượng: Yến, Tạ, Tấn");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000103"),
                column: "Title",
                value: "Diện tích: mm², cm², dm², m²");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000104"),
                column: "Title",
                value: "Khái niệm phân số");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000105"),
                column: "Title",
                value: "Phân số bằng nhau");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000106"),
                column: "Title",
                value: "Rút gọn phân số");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000107"),
                column: "Title",
                value: "Quy đồng mẫu số");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000108"),
                column: "Title",
                value: "So sánh phân số");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000109"),
                column: "Title",
                value: "Cộng phân số");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000110"),
                column: "Title",
                value: "Trừ phân số");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000111"),
                column: "Title",
                value: "Nhân phân số");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000112"),
                column: "Title",
                value: "Chia phân số");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000113"),
                columns: new[] { "Description", "Title" },
                values: new object[] { "S = đáy × cao", "Hình bình hành" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000114"),
                columns: new[] { "Description", "Title" },
                values: new object[] { "S = (d₁ × d₂) / 2", "Hình thoi" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000115"),
                column: "Title",
                value: "Tìm 2 số biết Tổng và Hiệu");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000116"),
                column: "Title",
                value: "Tìm 2 số biết Tổng và Tỉ");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000117"),
                column: "Title",
                value: "Tìm 2 số biết Hiệu và Tỉ");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000118"),
                column: "Title",
                value: "Biểu đồ cột");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000119"),
                column: "Title",
                value: "Biểu đồ tranh");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000120"),
                column: "Title",
                value: "Ôn tập cuối năm lớp 4");
        }
    }
}
