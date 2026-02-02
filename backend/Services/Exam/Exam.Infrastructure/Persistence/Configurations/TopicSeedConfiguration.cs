using FrogEdu.Exam.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.Exam.Infrastructure.Persistence.Configurations;

public class TopicSeedConfiguration : IEntityTypeConfiguration<Topic>
{
    public void Configure(EntityTypeBuilder<Topic> builder)
    {
        var topics = new List<object>();
        var createdDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        int topicId = 1;

        // Grade 1 Math Topics
        var grade1MathSubjectId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        topics.AddRange([
            // Chủ đề 1: Các số từ 0 đến 10
            CreateTopic(
                topicId++,
                "1.1 - Các số 0, 1, 2, 3, 4, 5",
                "Nhận biết và đọc số 0, 1, 2, 3, 4, 5. Tập viết số. Đếm số lượng đồ vật.",
                grade1MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "1.2 - Các số 6, 7, 8, 9, 10",
                "Nhận biết và đọc số 6, 7, 8, 9, 10. Tập viết số.",
                grade1MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "1.3 - Nhiều hơn, ít hơn, bằng nhau",
                "So sánh số lượng. Xác định: Nhiều hơn, ít hơn, bằng nhau.",
                grade1MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "1.4 - So sánh số",
                "Dấu lớn hơn (>), bé hơn (<), bằng (=). Ví dụ: 4 > 3, 2 < 5, 4 = 4",
                grade1MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "1.5 - Mấy và mấy",
                "Phân tích cấu tạo số. Ví dụ: 5 gồm 3 và 2",
                grade1MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "1.6 - Luyện tập chung",
                "Ôn tập và luyện tập các số từ 0 đến 10",
                grade1MathSubjectId,
                createdDate,
                null
            ),
            // Chủ đề 2: Làm quen với một số hình phẳng
            CreateTopic(
                topicId++,
                "2.1 - Hình vuông, hình tròn, hình tam giác, hình chữ nhật",
                "Nhận biết các hình phẳng cơ bản",
                grade1MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "2.2 - Thực hành lắp ghép, xếp hình",
                "Thực hành với các hình phẳng",
                grade1MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "2.3 - Luyện tập chung",
                "Ôn tập các hình phẳng",
                grade1MathSubjectId,
                createdDate,
                null
            ),
            // Chủ đề 3: Phép cộng, phép trừ trong phạm vi 10
            CreateTopic(
                topicId++,
                "3.1 - Phép cộng trong phạm vi 10",
                "Ý nghĩa: Gộp lại. Ví dụ: 3 + 2 = 5",
                grade1MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "3.2 - Phép trừ trong phạm vi 10",
                "Ý nghĩa: Bớt đi. Ví dụ: 6 - 1 = 5",
                grade1MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "3.3 - Bảng cộng, bảng trừ trong phạm vi 10",
                "Học thuộc bảng cộng trừ trong phạm vi 10",
                grade1MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "3.4 - Luyện tập chung",
                "Ôn tập phép cộng, phép trừ",
                grade1MathSubjectId,
                createdDate,
                null
            ),
            // Chủ đề 4: Làm quen với một số hình khối
            CreateTopic(
                topicId++,
                "4.1 - Khối lập phương, khối hộp chữ nhật",
                "Nhận biết các hình khối cơ bản",
                grade1MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "4.2 - Vị trí, định hướng trong không gian",
                "Phải - Trái, Trên - Dưới, Trước - Sau",
                grade1MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "4.3 - Luyện tập chung",
                "Ôn tập hình khối và vị trí",
                grade1MathSubjectId,
                createdDate,
                null
            ),
            // Chủ đề 5: Ôn tập học kì 1
            CreateTopic(
                topicId++,
                "5.1 - Ôn tập các số trong phạm vi 10",
                "Ôn tập tổng hợp các số 0-10",
                grade1MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "5.2 - Ôn tập phép cộng, phép trừ",
                "Ôn tập các phép tính",
                grade1MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "5.3 - Ôn tập hình học",
                "Ôn tập hình phẳng và hình khối",
                grade1MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "5.4 - Ôn tập chung học kì 1",
                "Ôn tập tổng hợp học kì 1",
                grade1MathSubjectId,
                createdDate,
                null
            ),
        ]);

        // Học kì 2
        topics.AddRange(
            new[]
            {
                // Chủ đề 6: Các số đến 100
                CreateTopic(
                    topicId++,
                    "6.1 - Số có hai chữ số",
                    "Nhận biết Chục và Đơn vị. Cách đọc: 24 = hai mươi tư",
                    grade1MathSubjectId,
                    createdDate,
                    null
                ),
                CreateTopic(
                    topicId++,
                    "6.2 - So sánh số có hai chữ số",
                    "So sánh hàng chục trước. Ví dụ: 42 > 25",
                    grade1MathSubjectId,
                    createdDate,
                    null
                ),
                CreateTopic(
                    topicId++,
                    "6.3 - Bảng các số từ 1 đến 100",
                    "Số 100 = một trăm. Làm quen với bảng số",
                    grade1MathSubjectId,
                    createdDate,
                    null
                ),
                CreateTopic(
                    topicId++,
                    "6.4 - Luyện tập chung",
                    "Ôn tập các số đến 100",
                    grade1MathSubjectId,
                    createdDate,
                    null
                ),
                // Chủ đề 7: Độ dài và đo độ dài
                CreateTopic(
                    topicId++,
                    "7.1 - Dài hơn, ngắn hơn",
                    "So sánh độ dài của các vật",
                    grade1MathSubjectId,
                    createdDate,
                    null
                ),
                CreateTopic(
                    topicId++,
                    "7.2 - Đơn vị đo độ dài (cm)",
                    "Làm quen với đơn vị cm",
                    grade1MathSubjectId,
                    createdDate,
                    null
                ),
                CreateTopic(
                    topicId++,
                    "7.3 - Thực hành ước lượng và đo độ dài",
                    "Thực hành đo đạc",
                    grade1MathSubjectId,
                    createdDate,
                    null
                ),
                CreateTopic(
                    topicId++,
                    "7.4 - Luyện tập chung",
                    "Ôn tập đo độ dài",
                    grade1MathSubjectId,
                    createdDate,
                    null
                ),
                // Chủ đề 8: Phép cộng, phép trừ trong phạm vi 100
                CreateTopic(
                    topicId++,
                    "8.1 - Phép cộng (2 chữ số + 1 chữ số)",
                    "Cộng số có hai chữ số với một chữ số",
                    grade1MathSubjectId,
                    createdDate,
                    null
                ),
                CreateTopic(
                    topicId++,
                    "8.2 - Phép cộng (2 chữ số + 2 chữ số)",
                    "Cộng hai số có hai chữ số",
                    grade1MathSubjectId,
                    createdDate,
                    null
                ),
                CreateTopic(
                    topicId++,
                    "8.3 - Phép trừ (2 chữ số - 1 chữ số)",
                    "Trừ số có hai chữ số cho một chữ số",
                    grade1MathSubjectId,
                    createdDate,
                    null
                ),
                CreateTopic(
                    topicId++,
                    "8.4 - Phép trừ (2 chữ số - 2 chữ số)",
                    "Trừ hai số có hai chữ số",
                    grade1MathSubjectId,
                    createdDate,
                    null
                ),
                CreateTopic(
                    topicId++,
                    "8.5 - Luyện tập chung",
                    "Ôn tập cộng trừ",
                    grade1MathSubjectId,
                    createdDate,
                    null
                ),
                // Chủ đề 9: Thời gian, giờ và lịch
                CreateTopic(
                    topicId++,
                    "9.1 - Xem giờ đúng trên đồng hồ",
                    "Học cách xem giờ",
                    grade1MathSubjectId,
                    createdDate,
                    null
                ),
                CreateTopic(
                    topicId++,
                    "9.2 - Các ngày trong tuần",
                    "Tên các ngày trong tuần",
                    grade1MathSubjectId,
                    createdDate,
                    null
                ),
                CreateTopic(
                    topicId++,
                    "9.3 - Thực hành xem lịch và giờ",
                    "Thực hành với lịch và đồng hồ",
                    grade1MathSubjectId,
                    createdDate,
                    null
                ),
                CreateTopic(
                    topicId++,
                    "9.4 - Luyện tập chung",
                    "Ôn tập về thời gian",
                    grade1MathSubjectId,
                    createdDate,
                    null
                ),
                // Chủ đề 10: Ôn tập cuối năm
                CreateTopic(
                    topicId++,
                    "10.1 - Ôn tập các số và phép tính (0-10)",
                    "Ôn tập các số và phép tính trong phạm vi 10",
                    grade1MathSubjectId,
                    createdDate,
                    null
                ),
                CreateTopic(
                    topicId++,
                    "10.2 - Ôn tập các số và phép tính (0-100)",
                    "Ôn tập các số và phép tính trong phạm vi 100",
                    grade1MathSubjectId,
                    createdDate,
                    null
                ),
                CreateTopic(
                    topicId++,
                    "10.3 - Ôn tập hình học và đo lường",
                    "Ôn tập hình học, độ dài, thời gian",
                    grade1MathSubjectId,
                    createdDate,
                    null
                ),
                CreateTopic(
                    topicId++,
                    "10.4 - Ôn tập chung cuối năm",
                    "Ôn tập tổng hợp toàn bộ chương trình lớp 1",
                    grade1MathSubjectId,
                    createdDate,
                    null
                ),
            }
        );

        // Grade 2 Math Topics
        var grade2MathSubjectId = Guid.Parse("00000000-0000-0000-0000-000000000011");

        topics.AddRange([
            // Chủ đề 1: Ôn tập và bổ sung
            CreateTopic(
                topicId++,
                "1.1 - Ôn tập các số đến 100",
                "Ôn tập kiến thức lớp 1",
                grade2MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "1.2 - Ôn tập phép cộng, phép trừ không nhớ",
                "Ôn tập các phép tính cơ bản",
                grade2MathSubjectId,
                createdDate,
                null
            ),
            // Chủ đề 2: Phép cộng, phép trừ qua 10 (phạm vi 20)
            CreateTopic(
                topicId++,
                "2.1 - Phép cộng qua 10 trong phạm vi 20",
                "Phương pháp: Tách số để tròn chục. Ví dụ: 9 + 5 = 9 + 1 + 4 = 14",
                grade2MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "2.2 - Luyện tập phép cộng qua 10",
                "Thực hành phép cộng qua 10",
                grade2MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "2.3 - Phép trừ qua 10 trong phạm vi 20",
                "Phương pháp: Trừ để tròn chục. Ví dụ: 12 - 5 = 12 - 2 - 3 = 7",
                grade2MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "2.4 - Luyện tập phép trừ qua 10",
                "Thực hành phép trừ qua 10",
                grade2MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "2.5 - Luyện tập chung - Cộng trừ qua 10",
                "Ôn tập cộng trừ qua 10",
                grade2MathSubjectId,
                createdDate,
                null
            ),
            // Chủ đề 3: Làm quen với khối trụ, khối cầu
            CreateTopic(
                topicId++,
                "3.1 - Khối trụ, khối cầu",
                "Khối trụ: Hai mặt đáy tròn (hộp sữa). Khối cầu: Tròn đều (quả bóng)",
                grade2MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "3.2 - Luyện tập - Hình khối",
                "Thực hành nhận biết khối trụ, khối cầu",
                grade2MathSubjectId,
                createdDate,
                null
            ),
            // Chủ đề 4: Phép cộng, phép trừ có nhớ (phạm vi 100)
            CreateTopic(
                topicId++,
                "4.1 - Phép cộng có nhớ",
                "Phép cộng có nhớ trong phạm vi 100. Ví dụ: 38 + 25 = 63",
                grade2MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "4.2 - Luyện tập phép cộng có nhớ",
                "Thực hành phép cộng có nhớ",
                grade2MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "4.3 - Phép trừ có nhớ",
                "Phép trừ có nhớ trong phạm vi 100. Ví dụ: 52 - 27 = 25",
                grade2MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "4.4 - Luyện tập phép trừ có nhớ",
                "Thực hành phép trừ có nhớ",
                grade2MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "4.5 - Luyện tập chung - Cộng trừ có nhớ",
                "Ôn tập cộng trừ có nhớ",
                grade2MathSubjectId,
                createdDate,
                null
            ),
            // Chủ đề 5: Làm quen với hình phẳng
            CreateTopic(
                topicId++,
                "5.1 - Đường thẳng, đường cong, đường gấp khúc",
                "Nhận biết các loại đường",
                grade2MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "5.2 - Luyện tập - Các loại đường",
                "Thực hành vẽ và nhận biết",
                grade2MathSubjectId,
                createdDate,
                null
            ),
            // Chủ đề 6: Thời gian
            CreateTopic(
                topicId++,
                "6.1 - Ngày - Giờ, Giờ - Phút",
                "1 giờ = 60 phút. Chuyển đổi đơn vị thời gian",
                grade2MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "6.2 - Ngày - Tháng",
                "Số ngày trong các tháng",
                grade2MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "6.3 - Luyện tập - Thời gian",
                "Ôn tập về thời gian",
                grade2MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "6.4 - Ôn tập cuối năm lớp 2",
                "Ôn tập tổng hợp chương trình lớp 2",
                grade2MathSubjectId,
                createdDate,
                null
            ),
        ]);

        // Grade 3 Math Topics
        var grade3MathSubjectId = Guid.Parse("00000000-0000-0000-0000-000000000021");

        topics.AddRange([
            // Học kì 1
            // Phần 1: Ôn tập và bổ sung
            CreateTopic(
                topicId++,
                "1.1 - Các số đến 1000",
                "Đọc, viết, so sánh số đến 1000",
                grade3MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "1.2 - Phép cộng, phép trừ trong phạm vi 1000",
                "Cộng trừ số có 3 chữ số",
                grade3MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "1.3 - Bảng nhân 2, 5 và Bảng chia 2, 5",
                "Ôn tập bảng nhân chia 2 và 5",
                grade3MathSubjectId,
                createdDate,
                null
            ),
            // Phần 2: Bảng nhân và Bảng chia mới
            CreateTopic(
                topicId++,
                "2.1 - Bảng nhân 3, 4",
                "Học thuộc bảng nhân 3, 4",
                grade3MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "2.2 - Bảng nhân 6, 7, 8, 9",
                "Học thuộc bảng nhân 6, 7, 8, 9",
                grade3MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "2.3 - Phép nhân (2-3 chữ số x 1 chữ số)",
                "Nhân số có 2-3 chữ số với 1 chữ số",
                grade3MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "2.4 - Phép chia (2-3 chữ số ÷ 1 chữ số)",
                "Chia số có 2-3 chữ số cho 1 chữ số",
                grade3MathSubjectId,
                createdDate,
                null
            ),
            // Phần 3: Hình học và Đo lường
            CreateTopic(
                topicId++,
                "3.1 - Góc: Vuông, nhọn, tù",
                "Nhận biết và đo các loại góc",
                grade3MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "3.2 - Hình: Tam giác, tứ giác, chữ nhật, vuông, tròn",
                "Tính chất và nhận biết các hình",
                grade3MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "3.3 - Đơn vị đo độ dài: mm, cm, dm, m, km",
                "Các đơn vị đo độ dài và chuyển đổi",
                grade3MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "3.4 - Khối lượng: g, kg",
                "Đơn vị đo khối lượng",
                grade3MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "3.5 - Dung tích: ml, l",
                "Đơn vị đo dung tích",
                grade3MathSubjectId,
                createdDate,
                null
            ),
            // Phần 4: Thống kê và xác suất
            CreateTopic(
                topicId++,
                "4.1 - Thu thập và phân loại dữ liệu",
                "Làm quen với thống kê cơ bản",
                grade3MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "4.2 - Khả năng xảy ra của sự kiện",
                "Làm quen với xác suất đơn giản",
                grade3MathSubjectId,
                createdDate,
                null
            ),
            // Học kì 2
            // Phần 5: Các số đến 10,000 và 100,000
            CreateTopic(
                topicId++,
                "5.1 - Các số đến 10,000",
                "Đọc, viết số có 5 chữ số",
                grade3MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "5.2 - Các số đến 100,000",
                "Đọc, viết số có 6 chữ số",
                grade3MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "5.3 - Cấu tạo thập phân",
                "Hàng và lớp của số",
                grade3MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "5.4 - So sánh số trong phạm vi 100,000",
                "So sánh các số lớn",
                grade3MathSubjectId,
                createdDate,
                null
            ),
            // Phần 6: Phép tính trong phạm vi 100,000
            CreateTopic(
                topicId++,
                "6.1 - Cộng, trừ có nhớ (số lớn)",
                "Cộng trừ số có nhiều chữ số",
                grade3MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "6.2 - Nhân, chia số lớn",
                "Nhân chia trong phạm vi 100,000",
                grade3MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "6.3 - Tính giá trị biểu thức",
                "Thứ tự thực hiện phép tính",
                grade3MathSubjectId,
                createdDate,
                null
            ),
            // Phần 7: Hình học nâng cao
            CreateTopic(
                topicId++,
                "7.1 - Chu vi: Tam giác, tứ giác, chữ nhật, vuông",
                "Công thức tính chu vi các hình",
                grade3MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "7.2 - Diện tích: Chữ nhật, vuông",
                "S = dài x rộng, S = cạnh x cạnh",
                grade3MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "7.3 - Tiền Việt Nam",
                "Đơn vị tiền tệ và quy đổi",
                grade3MathSubjectId,
                createdDate,
                null
            ),
            // Phần 8: Thời gian
            CreateTopic(
                topicId++,
                "8.1 - Xem đồng hồ (chính xác đến phút)",
                "Đọc giờ chính xác",
                grade3MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "8.2 - Tháng - Năm",
                "Năm nhuận, năm thường",
                grade3MathSubjectId,
                createdDate,
                null
            ),
            // Dạng toán tư duy
            CreateTopic(
                topicId++,
                "9.1 - Toán gấp, giảm số",
                "Bài toán tìm số khi biết gấp/giảm",
                grade3MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "9.2 - Toán so sánh",
                "Bài toán so sánh hai số",
                grade3MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "9.3 - Toán hai bước tính",
                "Bài toán nhiều phép tính",
                grade3MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "9.4 - Ôn tập cuối năm lớp 3",
                "Ôn tập tổng hợp chương trình lớp 3",
                grade3MathSubjectId,
                createdDate,
                null
            ),
        ]);

        // Grade 4 Math Topics
        var grade4MathSubjectId = Guid.Parse("00000000-0000-0000-0000-000000000031");

        topics.AddRange([
            // Học kì 1
            // Phần 1: Số tự nhiên
            CreateTopic(
                topicId++,
                "1.1 - Số có 6 chữ số",
                "Đọc, viết số có 6 chữ số",
                grade4MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "1.2 - Hàng và lớp (triệu, nghìn, đơn vị)",
                "Cấu tạo số theo hàng và lớp",
                grade4MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "1.3 - So sánh số tự nhiên lớn",
                "So sánh số có 6 chữ số",
                grade4MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "1.4 - Làm tròn số",
                "Làm tròn đến chục, trăm, nghìn",
                grade4MathSubjectId,
                createdDate,
                null
            ),
            // Phần 2: Phép tính
            CreateTopic(
                topicId++,
                "2.1 - Cộng, trừ số lớn",
                "Cộng trừ số có 6 chữ số",
                grade4MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "2.2 - Nhân với số có 2 chữ số",
                "Thuật toán nhân số có 2 chữ số",
                grade4MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "2.3 - Chia số có nhiều chữ số",
                "Thuật toán chia số lớn",
                grade4MathSubjectId,
                createdDate,
                null
            ),
            // Phần 3: Hình học
            CreateTopic(
                topicId++,
                "3.1 - Các loại góc: Vuông, nhọn, tù, bẹt",
                "Phân loại và đo góc",
                grade4MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "3.2 - Đường thẳng song song",
                "Tính chất và cách vẽ",
                grade4MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "3.3 - Đường thẳng vuông góc",
                "Tính chất và cách vẽ",
                grade4MathSubjectId,
                createdDate,
                null
            ),
            // Phần 4: Đo lường
            CreateTopic(
                topicId++,
                "4.1 - Khối lượng: Yến, Tạ, Tấn",
                "1 yến = 10kg, 1 tạ = 100kg, 1 tấn = 1000kg",
                grade4MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "4.2 - Diện tích: mm², cm², dm², m²",
                "Các đơn vị đo diện tích",
                grade4MathSubjectId,
                createdDate,
                null
            ),
            // Học kì 2
            // Phần 1: Phân số
            CreateTopic(
                topicId++,
                "5.1 - Khái niệm phân số",
                "Tử số, mẫu số, ý nghĩa phân số",
                grade4MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "5.2 - Phân số bằng nhau",
                "Tính chất cơ bản của phân số",
                grade4MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "5.3 - Rút gọn phân số",
                "Chia cả tử và mẫu cho ƯCLN",
                grade4MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "5.4 - Quy đồng mẫu số",
                "Nhân cả tử và mẫu với cùng một số",
                grade4MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "5.5 - So sánh phân số",
                "So sánh phân số cùng mẫu, khác mẫu",
                grade4MathSubjectId,
                createdDate,
                null
            ),
            // Phần 2: Phép tính với phân số
            CreateTopic(
                topicId++,
                "6.1 - Cộng phân số",
                "Cộng phân số cùng mẫu và khác mẫu",
                grade4MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "6.2 - Trừ phân số",
                "Trừ phân số cùng mẫu và khác mẫu",
                grade4MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "6.3 - Nhân phân số",
                "Nhân tử với tử, mẫu với mẫu",
                grade4MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "6.4 - Chia phân số",
                "Nhân với phân số nghịch đảo",
                grade4MathSubjectId,
                createdDate,
                null
            ),
            // Phần 3: Hình học nâng cao
            CreateTopic(
                topicId++,
                "7.1 - Hình bình hành",
                "S = đáy x cao",
                grade4MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "7.2 - Hình thoi",
                "S = (d₁ x d₂) / 2",
                grade4MathSubjectId,
                createdDate,
                null
            ),
            // Phần 4: Tỉ số và bài toán
            CreateTopic(
                topicId++,
                "8.1 - Tìm 2 số biết Tổng và Hiệu",
                "Dạng toán tổng hiệu",
                grade4MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "8.2 - Tìm 2 số biết Tổng và Tỉ",
                "Dạng toán tổng tỉ",
                grade4MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "8.3 - Tìm 2 số biết Hiệu và Tỉ",
                "Dạng toán hiệu tỉ",
                grade4MathSubjectId,
                createdDate,
                null
            ),
            // Phần 5: Thống kê
            CreateTopic(
                topicId++,
                "9.1 - Biểu đồ cột",
                "Đọc và vẽ biểu đồ cột",
                grade4MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "9.2 - Biểu đồ tranh",
                "Đọc và vẽ biểu đồ tranh",
                grade4MathSubjectId,
                createdDate,
                null
            ),
            CreateTopic(
                topicId++,
                "9.3 - Ôn tập cuối năm lớp 4",
                "Ôn tập tổng hợp chương trình lớp 4",
                grade4MathSubjectId,
                createdDate,
                null
            ),
        ]);

        builder.HasData(topics);
    }

    private static object CreateTopic(
        int id,
        string title,
        string description,
        Guid subjectId,
        DateTime createdAt,
        Guid? createdBy
    )
    {
        return new
        {
            Id = Guid.Parse($"10000000-0000-0000-0000-{id:D12}"),
            Title = title,
            Description = description,
            IsCurriculum = true,
            SubjectId = subjectId,
            CreatedAt = createdAt,
            CreatedBy = createdBy,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (Guid?)null,
            IsDeleted = false,
        };
    }
}
