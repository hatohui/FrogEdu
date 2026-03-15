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

        // =================================================================
        //  GRADE 1
        // =================================================================

        // --- Grade 1 Math (ID=1) ---
        var g1Math = Guid.Parse("00000000-0000-0000-0000-000000000001");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Các số từ 0 đến 10", "Nhận biết, đọc, viết, đếm xuôi ngược các số 0-10. So sánh bằng >, <, =. Cấu tạo số (tách-gộp).", g1Math, createdDate),
            CreateTopic(topicId++, "Làm quen với một số hình phẳng", "Nhận biết hình vuông, hình tròn, hình tam giác, hình chữ nhật. Thực hành lắp ghép, xếp hình.", g1Math, createdDate),
            CreateTopic(topicId++, "Phép cộng, phép trừ trong phạm vi 10", "Phép cộng (gộp lại), phép trừ (bớt đi) trong phạm vi 10. Bảng cộng trừ, mối quan hệ cộng-trừ.", g1Math, createdDate),
            CreateTopic(topicId++, "Làm quen với một số hình khối", "Khối lập phương, khối hộp chữ nhật. Vị trí và định hướng trong không gian: trên-dưới, phải-trái, trước-sau.", g1Math, createdDate),
            CreateTopic(topicId++, "Ôn tập học kì 1", "Ôn tập: đếm, đọc, viết số và so sánh số đến 10; cộng trừ phạm vi 10; nhận biết hình phẳng, hình khối và vị trí.", g1Math, createdDate),
            CreateTopic(topicId++, "Các số đến 100", "Số có hai chữ số, hệ thập phân (chục và đơn vị). So sánh, sắp xếp số đến 100.", g1Math, createdDate),
            CreateTopic(topicId++, "Độ dài và đo độ dài", "Đơn vị xăng-ti-mét (cm). Sử dụng thước kẻ để đo và vẽ đoạn thẳng.", g1Math, createdDate),
            CreateTopic(topicId++, "Cộng, trừ trong phạm vi 100", "Phép cộng, trừ không nhớ trong phạm vi 100. Cộng trừ dọc.", g1Math, createdDate),
            CreateTopic(topicId++, "Thời gian, giờ và lịch", "Đọc giờ đúng trên đồng hồ. Ngày trong tuần, tờ lịch. Thời gian sinh hoạt hàng ngày.", g1Math, createdDate),
            CreateTopic(topicId++, "Ôn tập cuối năm", "Ôn tập tổng hợp: số đến 100, cộng trừ phạm vi 100, đo độ dài, đọc giờ, hình phẳng và hình khối.", g1Math, createdDate),
        });

        // --- Grade 1 Literature (ID=2) ---
        var g1Lit = Guid.Parse("00000000-0000-0000-0000-000000000002");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Học âm và chữ cái", "Phần 1: 25 bài học âm, chữ cái Tiếng Việt. Nhận diện mặt chữ, phát âm, tập viết.", g1Lit, createdDate),
            CreateTopic(topicId++, "Học vần", "Phần 2: Học vần trơn (ai, oi, ui...), vần có âm cuối (an, on, un, at, ot...). Ghép vần tạo từ.", g1Lit, createdDate),
            CreateTopic(topicId++, "Kể chuyện và luyện đọc", "Phần 3: Đọc bài khóa ngắn, kể chuyện theo tranh. Luyện kỹ năng nghe-nói-đọc-viết.", g1Lit, createdDate),
            CreateTopic(topicId++, "Tôi và các bạn", "Chủ điểm 1 Tập 2: 4 bài đọc về tình bạn, sẻ chia, giúp đỡ nhau.", g1Lit, createdDate),
            CreateTopic(topicId++, "Mái ấm gia đình", "Chủ điểm 2 Tập 2: 4 bài đọc về gia đình, yêu thương, chăm sóc lẫn nhau.", g1Lit, createdDate),
            CreateTopic(topicId++, "Mái trường thân yêu", "Chủ điểm 3 Tập 2: 4 bài đọc về trường học, thầy cô, bạn bè.", g1Lit, createdDate),
            CreateTopic(topicId++, "Thiên nhiên quanh ta", "Chủ điểm 4 Tập 2: 5 bài đọc về thiên nhiên, động vật, thực vật.", g1Lit, createdDate),
            CreateTopic(topicId++, "Đất nước và con người", "Chủ điểm 5 Tập 2: 5 bài đọc về quê hương, đất nước, danh lam thắng cảnh.", g1Lit, createdDate),
        });

        // --- Grade 1 English (ID=3) ---
        var g1Eng = Guid.Parse("00000000-0000-0000-0000-000000000003");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Unit 1-4: Letters Bb, Cc, Aa, Dd", "Phonics: /b/, /k/, /æ/, /d/. Từ vựng: bear, cat, apple, dog. Mẫu câu: This is a bear. I have an apple.", g1Eng, createdDate),
            CreateTopic(topicId++, "Unit 5-8: Letters Ff, Gg, Hh, Ii", "Phonics: /f/, /g/, /h/, /ɪ/. Từ vựng: fish, goat, hat, igloo. Mẫu câu: I like fish. Here's a hat.", g1Eng, createdDate),
            CreateTopic(topicId++, "Review 1 (Units 1-8)", "Ôn tập tổng hợp các âm A-I, từ vựng và mẫu câu đã học.", g1Eng, createdDate),
            CreateTopic(topicId++, "Unit 9-12: Letters Jj, Kk, Ll, Mm", "Phonics: /dʒ/, /k/, /l/, /m/. Từ vựng: jelly, kite, lion, monkey. Mẫu câu cơ bản.", g1Eng, createdDate),
            CreateTopic(topicId++, "Unit 13-16: Letters Nn, Oo, Pp, Qq", "Phonics: /n/, /ɒ/, /p/, /kw/. Từ vựng: nest, orange, pen, queen.", g1Eng, createdDate),
            CreateTopic(topicId++, "Review 2 (Units 9-16)", "Ôn tập tổng hợp các âm J-Q, từ vựng và mẫu câu đã học.", g1Eng, createdDate),
        });

        // --- Grade 1 Ethics (ID=4) ---
        var g1Ethics = Guid.Parse("00000000-0000-0000-0000-000000000004");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Yêu thương gia đình", "Bài 1-2: Nhận biết sự quan tâm, chăm sóc của các thành viên. Thể hiện tình yêu thương gia đình.", g1Ethics, createdDate),
            CreateTopic(topicId++, "Tự giác làm việc của mình", "Bài 3: Nhận biết các việc cá nhân cần tự làm. Rèn tính tự giác trong sinh hoạt hàng ngày.", g1Ethics, createdDate),
            CreateTopic(topicId++, "Thật thà", "Bài 4: Ý nghĩa của sự thật thà. Trung thực trong học tập và cuộc sống.", g1Ethics, createdDate),
            CreateTopic(topicId++, "Giữ gìn đồ dùng", "Bài 5-6: Giữ gìn đồ dùng cá nhân và đồ dùng chung. Sắp xếp ngăn nắp.", g1Ethics, createdDate),
            CreateTopic(topicId++, "Quan tâm hàng xóm", "Bài 7: Biết quan tâm, giúp đỡ láng giềng. Lễ phép, thân thiện với mọi người.", g1Ethics, createdDate),
            CreateTopic(topicId++, "Phòng tránh tai nạn", "Bài 8-9: Nhận diện nguy hiểm, phòng tránh tai nạn thương tích. An toàn ở nhà và ở trường.", g1Ethics, createdDate),
            CreateTopic(topicId++, "Sinh hoạt nền nếp", "Bài 10-11: Thực hiện nếp sống văn minh, đúng giờ, gọn gàng ngăn nắp.", g1Ethics, createdDate),
        });

        // --- Grade 1 Nature & Society (ID=5) ---
        var g1NatSoc = Guid.Parse("00000000-0000-0000-0000-000000000005");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Gia đình", "Bài 1-4: Các thành viên trong gia đình, ngôi nhà, an toàn khi ở nhà.", g1NatSoc, createdDate),
            CreateTopic(topicId++, "Trường học", "Bài 5-8: Đồ dùng học tập, các phòng trong trường, giữ sạch trường lớp.", g1NatSoc, createdDate),
            CreateTopic(topicId++, "Cộng đồng địa phương", "Bài 9-11: Đường đi an toàn, phương tiện giao thông, nơi công cộng.", g1NatSoc, createdDate),
            CreateTopic(topicId++, "Thực vật và động vật", "Bài 12-14: Nhận biết cây cối, con vật xung quanh. Bảo vệ cây xanh, động vật.", g1NatSoc, createdDate),
            CreateTopic(topicId++, "Con người và sức khỏe", "Bài 15-17: Các bộ phận cơ thể, giữ vệ sinh, ăn uống đủ chất.", g1NatSoc, createdDate),
            CreateTopic(topicId++, "Trái Đất và bầu trời", "Bài 18-19: Thời tiết (nắng, mưa, gió), bầu trời ngày và đêm.", g1NatSoc, createdDate),
        });

        // --- Grade 1 Art (ID=6) ---
        var g1Art = Guid.Parse("00000000-0000-0000-0000-000000000006");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Mĩ thuật trong nhà trường", "Bài 1-2: Làm quen với đồ dùng mĩ thuật, các hoạt động mĩ thuật cơ bản.", g1Art, createdDate),
            CreateTopic(topicId++, "Sự thú vị đến từ những chấm", "Bài 3-4: Nhận biết chấm trong tự nhiên và nghệ thuật. Tạo hình từ chấm.", g1Art, createdDate),
            CreateTopic(topicId++, "Sự quyến rũ của đường nét", "Bài 5-6: Các loại đường nét (thẳng, cong, gấp khúc). Sáng tạo với đường nét.", g1Art, createdDate),
            CreateTopic(topicId++, "Sáng tạo với hình và khối", "Bài 7-8: Hình phẳng và khối 3D cơ bản. Tạo sản phẩm từ hình khối.", g1Art, createdDate),
            CreateTopic(topicId++, "Màu sắc quanh em", "Bài 9-10: Nhận biết 3 màu cơ bản (Đỏ, Vàng, Xanh). Vẽ tranh với màu sắc.", g1Art, createdDate),
            CreateTopic(topicId++, "Thế giới mĩ thuật quanh em", "Bài 11-12: Mĩ thuật trong đời sống, vật liệu tái chế, trưng bày tác phẩm.", g1Art, createdDate),
        });

        // --- Grade 1 Music (ID=7) ---
        var g1Music = Guid.Parse("00000000-0000-0000-0000-000000000007");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Âm thanh kì diệu", "Làm quen nốt Đô-Rê-Mi. Phân biệt âm thanh cao-thấp, mạnh-nhẹ.", g1Music, createdDate),
            CreateTopic(topicId++, "Việt Nam yêu thương", "Trống: loại nhạc cụ gõ. Quốc ca Việt Nam. Cảm nhận nhịp hành khúc.", g1Music, createdDate),
            CreateTopic(topicId++, "Mái trường thân yêu", "Kí hiệu tay nốt Đô-Rê-Mi. Bài hát về trường lớp.", g1Music, createdDate),
            CreateTopic(topicId++, "Vòng tay bè bạn", "Thưởng thức: Hồ Thiên Nga (Tchaikovsky). Hát và vận động theo nhạc.", g1Music, createdDate),
            CreateTopic(topicId++, "Nhịp điệu mùa xuân", "Nốt Pha, Son. Nhạc phẩm thiếu nhi Mozart. Nhịp 2/4.", g1Music, createdDate),
            CreateTopic(topicId++, "Về miền dân ca", "Dân ca Việt Nam. Nhạc cụ dân tộc: thanh phách.", g1Music, createdDate),
            CreateTopic(topicId++, "Gia đình yêu thương", "Thang 5 âm (Đô-Rê-Mi-Pha-Son). Bài hát về gia đình.", g1Music, createdDate),
            CreateTopic(topicId++, "Vui đón hè", "Nhạc cụ: kèn Triangle. Ôn tập tổng hợp cuối năm.", g1Music, createdDate),
        });

        // --- Grade 1 Experiential Activities (ID=8) ---
        var g1Exp = Guid.Parse("00000000-0000-0000-0000-000000000008");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Em và trường tiểu học", "Tuần 1-4: Làm quen trường mới, bạn mới, thầy cô. Nội quy trường học.", g1Exp, createdDate),
            CreateTopic(topicId++, "Khám phá bản thân", "Tuần 5-8: Nhận biết đặc điểm, sở thích cá nhân. Tự tin giới thiệu bản thân.", g1Exp, createdDate),
            CreateTopic(topicId++, "Mừng ngày nhà giáo", "Tuần 9-12: Tri ân thầy cô nhân ngày 20/11. Làm thiệp, tập biểu diễn.", g1Exp, createdDate),
            CreateTopic(topicId++, "Tự phục vụ bản thân", "Tuần 13-16: Tự chuẩn bị sách vở, vệ sinh cá nhân, ăn uống đúng cách.", g1Exp, createdDate),
            CreateTopic(topicId++, "Gia đình yêu thương và Tết", "Tuần 17-20: Tình cảm gia đình, phong tục Tết, trò chơi dân gian.", g1Exp, createdDate),
            CreateTopic(topicId++, "Em với cộng đồng", "Tuần 21-24: Hàng xóm láng giềng, nghề nghiệp trong cộng đồng.", g1Exp, createdDate),
            CreateTopic(topicId++, "Em với môi trường", "Tuần 25-28: Giữ gìn vệ sinh, bảo vệ cây xanh, tiết kiệm nước.", g1Exp, createdDate),
            CreateTopic(topicId++, "Phòng tránh tai nạn", "Tuần 29-32: An toàn giao thông, phòng cháy, đuối nước.", g1Exp, createdDate),
            CreateTopic(topicId++, "Tổng kết năm học", "Tuần 33-35: Ôn tập, trưng bày sản phẩm, liên hoan cuối năm.", g1Exp, createdDate),
        });

        // =================================================================
        //  GRADE 2
        // =================================================================

        // --- Grade 2 Math (ID=9) ---
        var g2Math = Guid.Parse("00000000-0000-0000-0000-000000000009");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Ôn tập và bổ sung - Số đến 100", "Ôn tập số đến 100, cộng trừ không nhớ trong phạm vi 100. Bổ sung kiến thức.", g2Math, createdDate),
            CreateTopic(topicId++, "Phép cộng, phép trừ qua 10 trong phạm vi 20", "Cộng trừ có nhớ qua 10. Ví dụ: 9+5=14, 14-6=8.", g2Math, createdDate),
            CreateTopic(topicId++, "Làm quen với khối trụ, khối cầu", "Nhận biết khối trụ (lon nước), khối cầu (quả bóng). Phân loại hình khối.", g2Math, createdDate),
            CreateTopic(topicId++, "Phép cộng, phép trừ có nhớ trong phạm vi 100", "Cộng trừ có nhớ dạng cột dọc. Ví dụ: 47+25=72, 63-28=35.", g2Math, createdDate),
            CreateTopic(topicId++, "Hình phẳng - Đường thẳng, đoạn thẳng, đường gấp khúc", "Nhận biết và vẽ đường thẳng, đoạn thẳng, đường gấp khúc, ba điểm thẳng hàng.", g2Math, createdDate),
            CreateTopic(topicId++, "Thời gian - Giờ, ngày, tháng", "Đọc giờ đúng, giờ rưỡi. Ngày trong tuần, tháng trong năm.", g2Math, createdDate),
            CreateTopic(topicId++, "Ôn tập học kì 1", "Ôn tập: cộng trừ có nhớ phạm vi 100, nhận biết khối trụ/cầu, đường thẳng, giờ.", g2Math, createdDate),
            CreateTopic(topicId++, "Phép nhân và phép chia", "Ý nghĩa phép nhân (gom nhóm bằng nhau), phép chia (chia đều). Bảng nhân chia 2, 3, 4, 5.", g2Math, createdDate),
            CreateTopic(topicId++, "Các số đến 1000", "Số có 3 chữ số: hàng trăm, hàng chục, hàng đơn vị. Đọc, viết, so sánh.", g2Math, createdDate),
            CreateTopic(topicId++, "Độ dài và khối lượng", "Mét (m), ki-lô-mét (km), mi-li-mét (mm). Ki-lô-gam (kg). Ước lượng độ dài.", g2Math, createdDate),
            CreateTopic(topicId++, "Phép cộng, phép trừ trong phạm vi 1000", "Cộng trừ có nhớ trong phạm vi 1000. Tính nhẩm, tính viết.", g2Math, createdDate),
            CreateTopic(topicId++, "Thống kê và xác suất", "Thu thập dữ liệu, biểu đồ tranh đơn giản. Chắc chắn, có thể, không thể.", g2Math, createdDate),
            CreateTopic(topicId++, "Ôn tập cuối năm", "Ôn tập tổng hợp: số đến 1000, 4 phép tính, đo lường, thống kê, hình học.", g2Math, createdDate),
        });

        // --- Grade 2 Literature (ID=10) ---
        var g2Lit = Guid.Parse("00000000-0000-0000-0000-000000000010");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Em là búp măng non", "Chủ điểm 1 Tập 1: Đọc hiểu văn bản về tuổi thơ, tự hào bản thân. Tập viết chữ hoa A-E.", g2Lit, createdDate),
            CreateTopic(topicId++, "Em đi học", "Chủ điểm 2 Tập 1: Văn bản về ngày đầu đến trường, niềm vui học tập. Chữ hoa F-H.", g2Lit, createdDate),
            CreateTopic(topicId++, "Niềm vui và tình bạn", "Chủ điểm 3 Tập 1: Đọc về tình bạn, sẻ chia. Chữ hoa I-L.", g2Lit, createdDate),
            CreateTopic(topicId++, "Mái ấm gia đình", "Chủ điểm 4 Tập 1: Gia đình, ông bà, cha mẹ. Chữ hoa M.", g2Lit, createdDate),
            CreateTopic(topicId++, "Ôn tập học kì 1", "Ôn tập các chủ điểm 1-4, kiểm tra đọc hiểu, chính tả, viết đoạn văn.", g2Lit, createdDate),
            CreateTopic(topicId++, "Vẻ đẹp quanh em", "Chủ điểm 1 Tập 2: Thiên nhiên, cảnh vật đẹp. Chữ hoa N-P.", g2Lit, createdDate),
            CreateTopic(topicId++, "Hành tinh xanh của em", "Chủ điểm 2 Tập 2: Bảo vệ môi trường, yêu thiên nhiên. Chữ hoa Q-S.", g2Lit, createdDate),
            CreateTopic(topicId++, "Con người Việt Nam", "Chủ điểm 3 Tập 2: Truyền thống, phẩm chất người Việt. Chữ hoa T-V.", g2Lit, createdDate),
            CreateTopic(topicId++, "Quê hương em", "Chủ điểm 4 Tập 2: Quê hương đất nước. Chữ hoa X-Y.", g2Lit, createdDate),
            CreateTopic(topicId++, "Thế giới trong mắt em", "Chủ điểm 5 Tập 2: Mở rộng hiểu biết về thế giới. Tốc độ đọc 80-100 tiếng/phút.", g2Lit, createdDate),
        });

        // --- Grade 2 English (ID=11) ---
        var g2Eng = Guid.Parse("00000000-0000-0000-0000-000000000011");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Unit 1-5: Letters Pp-Uu", "Phonics: /p/, /r/, /s/, /t/, /ʌ/. Từ vựng: popcorn, riding, sea, tea, bus. Mẫu câu cơ bản.", g2Eng, createdDate),
            CreateTopic(topicId++, "Review 1 (Units 1-5)", "Ôn tập tổng hợp các âm P-U, từ vựng và mẫu câu đã học.", g2Eng, createdDate),
            CreateTopic(topicId++, "Unit 6-10: Letters Vv-Zz", "Phonics: /v/, /w/, /ks/, /j/, /z/. Từ vựng: van, water, fox, yams, zebra.", g2Eng, createdDate),
            CreateTopic(topicId++, "Review 2 (Units 6-10)", "Ôn tập tổng hợp các âm V-Z, từ vựng và mẫu câu đã học.", g2Eng, createdDate),
            CreateTopic(topicId++, "Unit 11-16: Diphthongs & Digraphs", "Phonics nâng cao: i_e /aɪ/, oa /əʊ/, short i /ɪ/, short o /ɒ/, ui/ou /uː/, a_e /eɪ/. Present continuous, Would you like...?", g2Eng, createdDate),
            CreateTopic(topicId++, "Review 3 (Units 11-16)", "Ôn tập tổng hợp diphthongs, digraphs, mẫu câu nâng cao.", g2Eng, createdDate),
        });

        // --- Grade 2 Ethics (ID=12) ---
        var g2Ethics = Guid.Parse("00000000-0000-0000-0000-000000000012");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Quý trọng thời gian", "Bài 1: Nhận thức thời gian là vô giá. Quản lý thời gian, lập thời gian biểu.", g2Ethics, createdDate),
            CreateTopic(topicId++, "Nhận lỗi và sửa lỗi", "Bài 2: Dũng cảm nhận lỗi, sửa sai. Trung thực và trách nhiệm.", g2Ethics, createdDate),
            CreateTopic(topicId++, "Bảo quản đồ dùng cá nhân và gia đình", "Bài 3-4: Giữ gìn, sắp xếp đồ dùng cá nhân và gia đình.", g2Ethics, createdDate),
            CreateTopic(topicId++, "Kính trọng thầy cô và yêu quý bạn bè", "Bài 5-6: Văn hóa chào hỏi, cây tình bạn, sẻ chia khi bạn gặp khó khăn.", g2Ethics, createdDate),
            CreateTopic(topicId++, "Quý trọng lao động", "Bài 7: Tôn trọng mọi nghề nghiệp, trân quý thành quả lao động.", g2Ethics, createdDate),
            CreateTopic(topicId++, "Gìn giữ cảnh quan thiên nhiên", "Bài 8: Bảo vệ môi trường, lối sống xanh.", g2Ethics, createdDate),
            CreateTopic(topicId++, "Tuân thủ quy định nơi công cộng", "Bài 9: Xếp hàng, giữ trật tự, văn hóa ứng xử nơi công cộng.", g2Ethics, createdDate),
            CreateTopic(topicId++, "Phòng tránh tai nạn, thương tích", "Bài 10: Nhận diện nguy hiểm, số điện thoại khẩn cấp (113, 114, 115), thoát hiểm.", g2Ethics, createdDate),
        });

        // --- Grade 2 Nature & Society (ID=13) ---
        var g2NatSoc = Guid.Parse("00000000-0000-0000-0000-000000000013");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Gia đình", "Nghề nghiệp người thân, phòng ngộ độc, an toàn khi sử dụng đồ dùng trong nhà.", g2NatSoc, createdDate),
            CreateTopic(topicId++, "Trường học", "Khu vực trường, phòng chức năng, sinh hoạt lớp, vệ sinh trường lớp.", g2NatSoc, createdDate),
            CreateTopic(topicId++, "Cộng đồng", "Di tích lịch sử địa phương, an toàn giao thông, giữ gìn cảnh quan.", g2NatSoc, createdDate),
            CreateTopic(topicId++, "Thực vật và động vật", "Các bộ phận cây (rễ, thân, lá). Môi trường sống động vật. Bảo vệ sinh vật.", g2NatSoc, createdDate),
            CreateTopic(topicId++, "Con người và sức khỏe", "Cơ quan vận động (cơ, xương), tiêu hóa, bài tiết. Phòng bệnh.", g2NatSoc, createdDate),
            CreateTopic(topicId++, "Trái Đất và bầu trời", "Các mùa trong năm, hiện tượng thời tiết, thiên tai và cách phòng tránh.", g2NatSoc, createdDate),
        });

        // --- Grade 2 Art (ID=14) ---
        var g2Art = Guid.Parse("00000000-0000-0000-0000-000000000014");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Mĩ thuật trong nhà trường", "Bài 1-2: Đồ dùng mĩ thuật, pha trộn màu cơ bản tạo màu nhị hợp (cam, lục, tím).", g2Art, createdDate),
            CreateTopic(topicId++, "Sự thú vị đến từ những hình khối", "Bài 3-4: Khối lập phương, khối cầu, khối trụ. Mô hình đồ vật từ hình khối.", g2Art, createdDate),
            CreateTopic(topicId++, "Sự kì diệu của đường nét", "Bài 5-6: Nét thẳng, cong, gấp khúc, xoắn ốc. Họa tiết trang trí.", g2Art, createdDate),
            CreateTopic(topicId++, "Sáng tạo với đất nặn", "Bài 7-8: Kỹ thuật xoay, nặn, ấn dẹt, ghép dính. Tạo hình con vật.", g2Art, createdDate),
            CreateTopic(topicId++, "Gia đình và bạn bè", "Bài 9-10: Vẽ chân dung người thân, tranh sinh hoạt vui chơi.", g2Art, createdDate),
            CreateTopic(topicId++, "Thiên nhiên xanh", "Bài 11-12: In độc bản từ lá cây, cắt dán tranh vườn hoa.", g2Art, createdDate),
            CreateTopic(topicId++, "Tìm hiểu mĩ thuật - Tranh dân gian Đông Hồ", "Bài 13-14: Tranh Gà đàn, Lợn ăn lá ráy, Đám cưới chuột. Đồ chơi dân gian.", g2Art, createdDate),
            CreateTopic(topicId++, "Giao thông và quê hương", "Bài 15-16: Vẽ phương tiện giao thông, tranh phong cảnh quê hương.", g2Art, createdDate),
        });

        // --- Grade 2 Music (ID=15) ---
        var g2Music = Guid.Parse("00000000-0000-0000-0000-000000000015");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Rộn ràng ngày mới", "Hát bài về ngày mới. Vỗ tay theo nhịp. Làm quen nhạc cụ Body Percussion.", g2Music, createdDate),
            CreateTopic(topicId++, "Nhịp điệu bạn bè", "Hát bài về tình bạn. Nhạc cụ Maracas. Chơi trò chơi âm nhạc.", g2Music, createdDate),
            CreateTopic(topicId++, "Vui đến trường", "Hát bài về trường lớp. Thực hành Body Percussion nâng cao.", g2Music, createdDate),
            CreateTopic(topicId++, "Đoàn kết yêu thương", "Hát bài về đoàn kết. Nghe nhạc cổ điển. Vận động theo nhạc.", g2Music, createdDate),
            CreateTopic(topicId++, "Mùa xuân", "Hát bài về mùa xuân, Tết. Nghe nhạc truyền thống.", g2Music, createdDate),
            CreateTopic(topicId++, "Gia đình", "Hát bài về gia đình. Ôn tập nốt nhạc. Biểu diễn nhóm.", g2Music, createdDate),
            CreateTopic(topicId++, "Thiên nhiên", "Hát bài về thiên nhiên. Nhận biết nhịp 2/4. Sáng tạo âm nhạc.", g2Music, createdDate),
            CreateTopic(topicId++, "Vui đón hè", "Ôn tập tổng kết năm học. Biểu diễn, liên hoan âm nhạc.", g2Music, createdDate),
        });

        // --- Grade 2 Experiential Activities (ID=16) ---
        var g2Exp = Guid.Parse("00000000-0000-0000-0000-000000000016");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Khám phá bản thân", "Tuần 1-4: Hình dáng, sở thích, cảm xúc, sự thay đổi so với lớp 1.", g2Exp, createdDate),
            CreateTopic(topicId++, "Rèn luyện bản thân", "Tuần 5-8: Ngăn nắp, quý trọng thời gian, tự chăm sóc, an toàn.", g2Exp, createdDate),
            CreateTopic(topicId++, "Xây dựng nhà trường", "Tuần 9-12: Truyền thống nhà trường, vệ sinh, xây dựng tình bạn, biết ơn thầy cô.", g2Exp, createdDate),
            CreateTopic(topicId++, "Em với gia đình", "Tuần 13-15: Gia đình yêu thương, quan tâm chăm sóc, truyền thống gia đình.", g2Exp, createdDate),
            CreateTopic(topicId++, "Em với cộng đồng", "Tuần 16-18: Hàng xóm, lễ hội địa phương, ngày Tết.", g2Exp, createdDate),
            CreateTopic(topicId++, "Em với môi trường", "Tuần 19-21: Bảo vệ môi trường, tiết kiệm năng lượng, trồng cây.", g2Exp, createdDate),
            CreateTopic(topicId++, "Nghề nghiệp quanh em", "Tuần 22-24: Tìm hiểu nghề nghiệp, đóng vai, ước mơ nghề nghiệp.", g2Exp, createdDate),
        });

        // =================================================================
        //  GRADE 3
        // =================================================================

        // --- Grade 3 Math (ID=17) ---
        var g3Math = Guid.Parse("00000000-0000-0000-0000-000000000017");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Ôn tập và bổ sung - Số đến 1000", "Ôn tập số đến 1000, cộng trừ có nhớ. Bổ sung kiến thức lớp 2.", g3Math, createdDate),
            CreateTopic(topicId++, "Bảng nhân và bảng chia", "Bảng nhân chia 6, 7, 8, 9. Nhân chia trong phạm vi 1000.", g3Math, createdDate),
            CreateTopic(topicId++, "Hình học và đo lường", "Góc vuông, góc không vuông. Hình tròn, tâm, bán kính. Đơn vị: mm, g, ml.", g3Math, createdDate),
            CreateTopic(topicId++, "Phép nhân và phép chia trong phạm vi 1000", "Nhân số có 2-3 chữ số với số có 1 chữ số. Chia số có 2-3 chữ số cho số có 1 chữ số.", g3Math, createdDate),
            CreateTopic(topicId++, "Thống kê và xác suất", "Bảng số liệu, biểu đồ tranh. Sự kiện chắc chắn, có thể, không thể.", g3Math, createdDate),
            CreateTopic(topicId++, "Ôn tập học kì 1", "Ôn tập: bảng nhân chia, hình học, đo lường, thống kê.", g3Math, createdDate),
            CreateTopic(topicId++, "Các số đến 100 000", "Số có 4-5 chữ số: hàng nghìn, hàng chục nghìn. Đọc, viết, so sánh.", g3Math, createdDate),
            CreateTopic(topicId++, "Phép cộng, phép trừ trong phạm vi 100 000", "Cộng trừ số có nhiều chữ số. Tính giá trị biểu thức.", g3Math, createdDate),
            CreateTopic(topicId++, "Chu vi và diện tích", "Chu vi, diện tích hình chữ nhật, hình vuông. Công thức tính.", g3Math, createdDate),
            CreateTopic(topicId++, "Phép nhân, phép chia trong phạm vi 100 000", "Nhân chia số có nhiều chữ số. Bài toán giải bằng hai phép tính.", g3Math, createdDate),
            CreateTopic(topicId++, "Tiền Việt Nam và thời gian", "Các loại tiền Việt Nam. Tháng, năm, thế kỷ. Bài toán liên quan đến tiền.", g3Math, createdDate),
            CreateTopic(topicId++, "Ôn tập cuối năm", "Ôn tập tổng hợp: số đến 100 000, 4 phép tính, chu vi-diện tích, tiền, thời gian.", g3Math, createdDate),
        });

        // --- Grade 3 Literature (ID=18) ---
        var g3Lit = Guid.Parse("00000000-0000-0000-0000-000000000018");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Trải nghiệm thú vị", "Chủ điểm 1 Tập 1: Đọc hiểu về du lịch, trải nghiệm. Tốc độ đọc 70-80 tiếng/phút.", g3Lit, createdDate),
            CreateTopic(topicId++, "Cổng trường rộng mở", "Chủ điểm 2 Tập 1: Trường lớp, thầy cô, bạn bè.", g3Lit, createdDate),
            CreateTopic(topicId++, "Mái ấm gia đình", "Chủ điểm 3 Tập 1: Gia đình, yêu thương, chăm sóc.", g3Lit, createdDate),
            CreateTopic(topicId++, "Mái nhà chung", "Chủ điểm 4 Tập 1: Cộng đồng, xã hội, đoàn kết.", g3Lit, createdDate),
            CreateTopic(topicId++, "Những câu chuyện thú vị", "Chủ điểm 5 Tập 1: Truyện cổ tích, truyện ngụ ngôn.", g3Lit, createdDate),
            CreateTopic(topicId++, "Ôn tập học kì 1", "Ôn tập: đọc hiểu, viết đoạn văn ngắn, chính tả, từ vựng HK1.", g3Lit, createdDate),
            CreateTopic(topicId++, "Nghệ thuật", "Chủ điểm 1 Tập 2: Các loại hình nghệ thuật, sáng tạo.", g3Lit, createdDate),
            CreateTopic(topicId++, "Quê hương", "Chủ điểm 2 Tập 2: Cảnh đẹp quê hương, phong tục.", g3Lit, createdDate),
            CreateTopic(topicId++, "Đất nước ngàn năm", "Chủ điểm 3 Tập 2: Lịch sử, truyền thống dân tộc.", g3Lit, createdDate),
            CreateTopic(topicId++, "Trái đất xanh", "Chủ điểm 4 Tập 2: Bảo vệ môi trường, thiên nhiên.", g3Lit, createdDate),
            CreateTopic(topicId++, "Thế giới trong mắt em", "Chủ điểm 5 Tập 2: Mở rộng hiểu biết về thế giới.", g3Lit, createdDate),
        });

        // --- Grade 3 English (ID=19) ---
        var g3Eng = Guid.Parse("00000000-0000-0000-0000-000000000019");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Unit 1-5: Greetings, School, Colours", "Hello/Goodbye, school things (pen, book, ruler), colours (red, blue, green), numbers 1-10.", g3Eng, createdDate),
            CreateTopic(topicId++, "Unit 6-10: Family, Jobs, Body", "Family members, jobs (teacher, doctor), body parts, pets. This is my..., He/She is a...", g3Eng, createdDate),
            CreateTopic(topicId++, "Review 1 (Units 1-10)", "Ôn tập từ vựng và mẫu câu chủ đề trường học, gia đình, nghề nghiệp.", g3Eng, createdDate),
            CreateTopic(topicId++, "Unit 11-15: House, Toys, Activities", "Rooms in house, toys, outdoor activities. Where is...? I have... Do you like...?", g3Eng, createdDate),
            CreateTopic(topicId++, "Unit 16-20: Animals, Food, Clothes", "Zoo animals, food, clothes, weather. What's the weather like? I'm wearing...", g3Eng, createdDate),
            CreateTopic(topicId++, "Review 2 (Units 11-20)", "Ôn tập tổng hợp từ vựng, mẫu câu, phonics toàn bộ 20 unit.", g3Eng, createdDate),
        });

        // --- Grade 3 Ethics (ID=20) ---
        var g3Ethics = Guid.Parse("00000000-0000-0000-0000-000000000020");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "An toàn giao thông", "Bài 1: Nhận diện biển báo, luật giao thông cơ bản, qua đường an toàn.", g3Ethics, createdDate),
            CreateTopic(topicId++, "Yêu Tổ quốc Việt Nam", "Bài 2: Tự hào về đất nước, cờ Tổ quốc, Quốc ca.", g3Ethics, createdDate),
            CreateTopic(topicId++, "Ham học hỏi", "Bài 3: Chăm chỉ học tập, ham tìm hiểu kiến thức mới.", g3Ethics, createdDate),
            CreateTopic(topicId++, "Giữ lời hứa", "Bài 4: Giá trị của lời hứa, trách nhiệm với lời nói.", g3Ethics, createdDate),
            CreateTopic(topicId++, "Truyền thống gia đình", "Bài 5: Giữ gìn và phát huy truyền thống tốt đẹp của gia đình.", g3Ethics, createdDate),
            CreateTopic(topicId++, "Quan tâm hàng xóm, láng giềng", "Bài 6: Tình làng nghĩa xóm, giúp đỡ láng giềng.", g3Ethics, createdDate),
            CreateTopic(topicId++, "Thấu hiểu và chia sẻ", "Bài 7: Đồng cảm với người khác, biết chia sẻ, giúp đỡ.", g3Ethics, createdDate),
            CreateTopic(topicId++, "Tôn trọng sự khác biệt", "Bài 8: Tôn trọng sự khác biệt về ngoại hình, dân tộc, vùng miền.", g3Ethics, createdDate),
            CreateTopic(topicId++, "Kiềm chế cảm xúc tiêu cực", "Bài 9-10: Nhận biết và kiểm soát cảm xúc giận dữ, buồn bã.", g3Ethics, createdDate),
        });

        // --- Grade 3 Nature & Society (ID=21) ---
        var g3NatSoc = Guid.Parse("00000000-0000-0000-0000-000000000021");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Gia đình", "Họ hàng, quan hệ gia đình mở rộng, phòng cháy chữa cháy trong gia đình.", g3NatSoc, createdDate),
            CreateTopic(topicId++, "Trường học", "Môi trường trường học, an toàn trường lớp, phòng thí nghiệm đơn giản.", g3NatSoc, createdDate),
            CreateTopic(topicId++, "Cộng đồng", "Các hoạt động cộng đồng, phong tục địa phương, nghề nghiệp.", g3NatSoc, createdDate),
            CreateTopic(topicId++, "Thực vật và động vật", "Bộ phận cây (rễ, thân, lá, hoa, quả). Các nhóm động vật, chuỗi thức ăn đơn giản.", g3NatSoc, createdDate),
            CreateTopic(topicId++, "Con người và sức khỏe", "Hệ tiêu hóa, tuần hoàn, thần kinh. Phòng bệnh truyền nhiễm.", g3NatSoc, createdDate),
            CreateTopic(topicId++, "Trái Đất và bầu trời", "Hệ Mặt Trời, Trái Đất quay. Các đới khí hậu (nhiệt đới, ôn đới, hàn đới).", g3NatSoc, createdDate),
        });

        // --- Grade 3 Art (ID=22) ---
        var g3Art = Guid.Parse("00000000-0000-0000-0000-000000000022");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Nét, hình và màu", "Bài 1-2: Kết hợp nét, hình, màu sắc. Tạo bức tranh tổng hợp.", g3Art, createdDate),
            CreateTopic(topicId++, "Trường học của em", "Bài 3-4: Vẽ tranh trường lớp, bạn bè, hoạt động trường học.", g3Art, createdDate),
            CreateTopic(topicId++, "Gia đình thân yêu", "Bài 5-6: Vẽ chân dung gia đình, sinh hoạt ngày Tết.", g3Art, createdDate),
            CreateTopic(topicId++, "Sáng tạo từ vật liệu tái chế", "Bài 7-8: Tạo sản phẩm mĩ thuật từ vật liệu tái chế.", g3Art, createdDate),
            CreateTopic(topicId++, "Đồ chơi dân gian", "Bài 9-10: Làm rối tay, diều giấy từ vật liệu đơn giản.", g3Art, createdDate),
            CreateTopic(topicId++, "Tranh dân gian", "Bài 11-12: Tìm hiểu, thưởng thức và vẽ lại tranh dân gian.", g3Art, createdDate),
            CreateTopic(topicId++, "Thiên nhiên tươi đẹp", "Bài 13-14: Vẽ tranh phong cảnh thiên nhiên.", g3Art, createdDate),
            CreateTopic(topicId++, "Tổng kết và trưng bày", "Bài 15-16: Ôn tập, trưng bày sản phẩm mĩ thuật cả năm.", g3Art, createdDate),
        });

        // --- Grade 3 Music (ID=23) ---
        var g3Music = Guid.Parse("00000000-0000-0000-0000-000000000023");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Vui đón xuân", "Hát bài về xuân, Tết. Ôn tập nốt nhạc Đô-Rê-Mi-Pha-Son-La-Si.", g3Music, createdDate),
            CreateTopic(topicId++, "Gia đình yêu thương", "Hát bài về gia đình. Thực hành cao độ nốt nhạc.", g3Music, createdDate),
            CreateTopic(topicId++, "Mái trường mến yêu", "Hát bài về trường lớp. Nhịp 2/4, 3/4.", g3Music, createdDate),
            CreateTopic(topicId++, "Vòng tay bạn bè", "Hát bài về tình bạn. Nghe nhạc cổ điển.", g3Music, createdDate),
            CreateTopic(topicId++, "Thiên nhiên quanh em", "Hát bài về thiên nhiên. Sáng tạo vận động.", g3Music, createdDate),
            CreateTopic(topicId++, "Quê hương", "Dân ca Việt Nam, nhạc cụ dân tộc.", g3Music, createdDate),
            CreateTopic(topicId++, "Thế giới âm nhạc", "Làm quen Piano/Organ. Nghe nhạc phương Tây.", g3Music, createdDate),
            CreateTopic(topicId++, "Vui đón hè", "Ôn tập, biểu diễn cuối năm. Thang 7 nốt Đô-Si.", g3Music, createdDate),
        });

        // --- Grade 3 Experiential Activities (ID=24) ---
        var g3Exp = Guid.Parse("00000000-0000-0000-0000-000000000024");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Em và trường tiểu học", "Tuần 1-4: Truyền thống nhà trường, nội quy, các hoạt động tập thể.", g3Exp, createdDate),
            CreateTopic(topicId++, "Khám phá bản thân", "Tuần 5-8: Điểm mạnh, điểm yếu, kế hoạch rèn luyện.", g3Exp, createdDate),
            CreateTopic(topicId++, "Trách nhiệm của em", "Tuần 9-12: Trách nhiệm với bản thân, gia đình, trường lớp.", g3Exp, createdDate),
            CreateTopic(topicId++, "Em với gia đình", "Tuần 13-16: Công việc nhà, giúp đỡ gia đình, ngày Tết.", g3Exp, createdDate),
            CreateTopic(topicId++, "Em với cộng đồng", "Tuần 17-20: Tham gia hoạt động cộng đồng, thiện nguyện.", g3Exp, createdDate),
            CreateTopic(topicId++, "Vệ sinh môi trường", "Tuần 21-24: Phân loại rác, tái chế, giảm thiểu ô nhiễm.", g3Exp, createdDate),
            CreateTopic(topicId++, "Phòng hỏa hoạn và thiên tai", "Tuần 25-28: Kỹ năng phòng cháy, thoát hiểm, phòng tránh thiên tai.", g3Exp, createdDate),
            CreateTopic(topicId++, "Môi trường xanh", "Tuần 29-35: Bảo vệ môi trường, trồng cây, tổng kết năm học.", g3Exp, createdDate),
        });

        // --- Grade 3 Information Technology (ID=25) ---
        var g3IT = Guid.Parse("00000000-0000-0000-0000-000000000025");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Máy tính và em", "Bài 1-2: Các bộ phận máy tính (CPU, màn hình, bàn phím, chuột). Bật/tắt máy tính.", g3IT, createdDate),
            CreateTopic(topicId++, "Mạng Internet", "Bài 3-4: Internet là gì? Trình duyệt web, truy cập website an toàn.", g3IT, createdDate),
            CreateTopic(topicId++, "Lưu trữ thông tin", "Bài 5-6: Tệp và thư mục. Tạo, đổi tên, xóa thư mục trên Desktop.", g3IT, createdDate),
            CreateTopic(topicId++, "Đạo đức số", "Bài 7-8: An toàn thông tin cá nhân, không chia sẻ mật khẩu, tôn trọng bản quyền.", g3IT, createdDate),
            CreateTopic(topicId++, "Ứng dụng: Bàn phím và chuột", "Bài 9-10: Tập gõ phím, sử dụng chuột thành thạo. Trò chơi luyện gõ.", g3IT, createdDate),
            CreateTopic(topicId++, "Giải quyết vấn đề với máy tính", "Bài 11-12: Phần mềm Paint, vẽ hình đơn giản. Thuật toán cơ bản (tuần tự).", g3IT, createdDate),
        });

        // --- Grade 3 Technology (ID=26) ---
        var g3Tech = Guid.Parse("00000000-0000-0000-0000-000000000026");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Công nghệ và đời sống", "Đèn, quạt, radio, tivi – nhận biết và sử dụng an toàn các thiết bị.", g3Tech, createdDate),
            CreateTopic(topicId++, "Trồng hoa trong chậu", "Chọn giống, gieo hạt, chăm sóc cây hoa trong chậu. Quan sát sinh trưởng.", g3Tech, createdDate),
            CreateTopic(topicId++, "Lắp ráp mô hình", "Lắp ráp xe đồ chơi, đèn giao thông từ bộ lắp ghép kỹ thuật.", g3Tech, createdDate),
        });

        // =================================================================
        //  GRADE 4
        // =================================================================

        // --- Grade 4 Math (ID=27) ---
        var g4Math = Guid.Parse("00000000-0000-0000-0000-000000000027");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Số có nhiều chữ số", "Số có 6 chữ số, hàng và lớp (đơn vị, nghìn, triệu). Đọc, viết, so sánh, sắp xếp.", g4Math, createdDate),
            CreateTopic(topicId++, "Làm tròn số và ước lượng", "Làm tròn số đến hàng nghìn, hàng chục nghìn. Ước lượng kết quả.", g4Math, createdDate),
            CreateTopic(topicId++, "Phép nhân", "Nhân số có nhiều chữ số với số có 2 chữ số. Tính chất giao hoán, kết hợp, phân phối.", g4Math, createdDate),
            CreateTopic(topicId++, "Phép chia", "Chia số có nhiều chữ số cho số có 2 chữ số. Chia có dư, kiểm tra phép chia.", g4Math, createdDate),
            CreateTopic(topicId++, "Hình học: Góc và đường thẳng", "Góc vuông, nhọn, tù, bẹt. Hai đường thẳng vuông góc, song song.", g4Math, createdDate),
            CreateTopic(topicId++, "Đo lường", "Yến, tạ, tấn. Diện tích: mm², cm², dm², m². Đổi đơn vị đo.", g4Math, createdDate),
            CreateTopic(topicId++, "Ôn tập học kì 1", "Ôn tập: số có nhiều chữ số, 4 phép tính, hình học, đo lường.", g4Math, createdDate),
            CreateTopic(topicId++, "Phân số", "Khái niệm phân số, rút gọn, quy đồng mẫu số. Cộng, trừ, nhân, chia phân số.", g4Math, createdDate),
            CreateTopic(topicId++, "Hình bình hành và hình thoi", "Đặc điểm, chu vi, diện tích hình bình hành và hình thoi.", g4Math, createdDate),
            CreateTopic(topicId++, "Tỉ số", "Tỉ số của hai số. Bài toán tìm hai số khi biết tổng-hiệu, tổng-tỉ, hiệu-tỉ.", g4Math, createdDate),
            CreateTopic(topicId++, "Biểu đồ", "Biểu đồ cột và biểu đồ tranh. Đọc, vẽ, phân tích biểu đồ.", g4Math, createdDate),
            CreateTopic(topicId++, "Ôn tập cuối năm", "Ôn tập tổng hợp: phân số, hình bình hành/thoi, tỉ số, biểu đồ.", g4Math, createdDate),
        });

        // --- Grade 4 Literature (ID=28) ---
        var g4Lit = Guid.Parse("00000000-0000-0000-0000-000000000028");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Mỗi người một vẻ", "Chủ điểm 1 Tập 1: Danh từ. Đọc hiểu về cá tính, sự độc đáo mỗi người.", g4Lit, createdDate),
            CreateTopic(topicId++, "Trải nghiệm và khám phá", "Chủ điểm 2 Tập 1: Động từ. Du lịch, trải nghiệm thiên nhiên.", g4Lit, createdDate),
            CreateTopic(topicId++, "Kho tàng tri thức", "Chủ điểm 3 Tập 1: Tính từ. Sách, kiến thức, ham đọc sách.", g4Lit, createdDate),
            CreateTopic(topicId++, "Chắp cánh ước mơ", "Chủ điểm 4 Tập 1: Biện pháp nhân hóa. Ước mơ, hoài bão.", g4Lit, createdDate),
            CreateTopic(topicId++, "Ôn tập học kì 1", "Ôn tập: danh từ, động từ, tính từ, nhân hóa. Viết đoạn văn tả người, kể chuyện.", g4Lit, createdDate),
            CreateTopic(topicId++, "Sống để yêu thương", "Chủ điểm 1 Tập 2: Câu và thành phần câu (CN-VN). Tình yêu thương.", g4Lit, createdDate),
            CreateTopic(topicId++, "Uống nước nhớ nguồn", "Chủ điểm 2 Tập 2: Trạng ngữ. Biết ơn, truyền thống.", g4Lit, createdDate),
            CreateTopic(topicId++, "Quê hương trong tôi", "Chủ điểm 3 Tập 2: Dấu ngoặc kép. Quê hương, đất nước.", g4Lit, createdDate),
            CreateTopic(topicId++, "Vì một thế giới bình yên", "Chủ điểm 4 Tập 2: Từ ngữ, câu tưởng tượng. Hòa bình, nhân đạo.", g4Lit, createdDate),
            CreateTopic(topicId++, "Ôn tập cuối năm", "Ôn tập: câu, trạng ngữ, dấu câu. Viết thư, giấy mời, tả cây cối.", g4Lit, createdDate),
        });

        // --- Grade 4 English (ID=29) ---
        var g4Eng = Guid.Parse("00000000-0000-0000-0000-000000000029");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Unit 1-2: Countries & Time", "Countries, nationalities (Where are you from?). Telling time (What time is it?).", g4Eng, createdDate),
            CreateTopic(topicId++, "Unit 3-4: Days & Birthdays", "Days of the week (What day is it today?). Birthdays, months, ordinal numbers.", g4Eng, createdDate),
            CreateTopic(topicId++, "Unit 5-6: Abilities & School", "Can/Can't for abilities. School facilities (library, gym, canteen).", g4Eng, createdDate),
            CreateTopic(topicId++, "Unit 7-8: Timetable & Subjects", "School timetable, subjects (I have Maths on Monday). Favourite subjects.", g4Eng, createdDate),
            CreateTopic(topicId++, "Unit 9-10: Sports & Holidays", "Sports activities (play football, swim). Summer holidays, past activities.", g4Eng, createdDate),
            CreateTopic(topicId++, "Review 1 (Units 1-10)", "Ôn tập học kì 1: quốc gia, thời gian, trường học, thể thao.", g4Eng, createdDate),
            CreateTopic(topicId++, "Unit 11-12: Family Jobs & Workplaces", "Jobs (doctor, farmer, teacher). Workplaces (hospital, school, office).", g4Eng, createdDate),
            CreateTopic(topicId++, "Unit 13-14: Appearance & Daily Activities", "Describing people (tall, short, hair). Daily activities (get up, go to school).", g4Eng, createdDate),
            CreateTopic(topicId++, "Unit 15-16: Weekend & Weather", "Weekend activities (What do you do at the weekend?). Weather (sunny, rainy, cloudy).", g4Eng, createdDate),
            CreateTopic(topicId++, "Unit 17-18: City & Shopping", "City places (park, cinema). Shopping (How much is it? It's...).", g4Eng, createdDate),
            CreateTopic(topicId++, "Unit 19-20: Animals & Summer", "Zoo animals, describing animals. Summer holiday plans.", g4Eng, createdDate),
            CreateTopic(topicId++, "Review 2 (Units 11-20)", "Ôn tập học kì 2: nghề nghiệp, hoạt động, thời tiết, mua sắm.", g4Eng, createdDate),
        });

        // --- Grade 4 Ethics (ID=30) ---
        var g4Ethics = Guid.Parse("00000000-0000-0000-0000-000000000030");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Biết ơn người có công với đất nước", "Bài 1-2: Anh hùng dân tộc, kính trọng người có công.", g4Ethics, createdDate),
            CreateTopic(topicId++, "Cảm thông và giúp đỡ người khó khăn", "Bài 3-4: Đồng cảm, tương trợ, hoạt động nhân đạo.", g4Ethics, createdDate),
            CreateTopic(topicId++, "Yêu lao động", "Bài 5: Chăm chỉ lao động, quý trọng sản phẩm lao động.", g4Ethics, createdDate),
            CreateTopic(topicId++, "Tôn trọng tài sản của người khác", "Bài 6-7: Không lấy đồ không thuộc về mình, bảo vệ tài sản chung.", g4Ethics, createdDate),
            CreateTopic(topicId++, "Quan hệ bạn bè", "Bài 8-9: Tình bạn lành mạnh, giải quyết mâu thuẫn hòa bình.", g4Ethics, createdDate),
            CreateTopic(topicId++, "Quyền và bổn phận trẻ em", "Bài 10: Quyền được học tập, vui chơi, bảo vệ. Bổn phận đối với gia đình, xã hội.", g4Ethics, createdDate),
        });

        // --- Grade 4 Science (ID=31) ---
        var g4Science = Guid.Parse("00000000-0000-0000-0000-000000000031");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Chất", "Nước: 3 thể (rắn, lỏng, khí), chuyển thể (bay hơi, ngưng tụ, đông đặc, nóng chảy). Không khí.", g4Science, createdDate),
            CreateTopic(topicId++, "Năng lượng", "Ánh sáng (truyền thẳng, phản xạ), âm thanh (rung động, cao-thấp), nhiệt (truyền nhiệt, cách nhiệt).", g4Science, createdDate),
            CreateTopic(topicId++, "Thực vật và động vật", "Trao đổi chất ở thực vật, động vật. Hô hấp, quang hợp. Thích nghi môi trường.", g4Science, createdDate),
            CreateTopic(topicId++, "Nấm và vi khuẩn", "Vi khuẩn có ích, vi khuẩn gây bệnh. Nấm men, nấm mốc. Bảo quản thực phẩm.", g4Science, createdDate),
            CreateTopic(topicId++, "Con người và sức khỏe", "Dinh dưỡng cân bằng, vitamin, khoáng chất. Vệ sinh an toàn thực phẩm.", g4Science, createdDate),
            CreateTopic(topicId++, "Sinh vật và môi trường", "Hệ sinh thái, chuỗi thức ăn (sinh vật sản xuất, tiêu thụ, phân hủy). Bảo vệ đa dạng sinh học.", g4Science, createdDate),
        });

        // --- Grade 4 Art (ID=32) ---
        var g4Art = Guid.Parse("00000000-0000-0000-0000-000000000032");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Màu sắc trong nghệ thuật", "Gam màu nóng, lạnh, trung tính. Hòa sắc, tương phản.", g4Art, createdDate),
            CreateTopic(topicId++, "Di sản văn hóa", "Trống đồng Đông Sơn, gốm sứ Bát Tràng. Vẽ họa tiết truyền thống.", g4Art, createdDate),
            CreateTopic(topicId++, "Chân dung và con người", "Kỹ thuật vẽ chân dung, tỉ lệ khuôn mặt, biểu cảm.", g4Art, createdDate),
            CreateTopic(topicId++, "Thiên nhiên và môi trường", "Vẽ tranh phong cảnh, bảo vệ môi trường.", g4Art, createdDate),
            CreateTopic(topicId++, "Kiến trúc", "Nhận biết kiến trúc truyền thống Việt Nam (đình, chùa). Vẽ phối cảnh đơn giản.", g4Art, createdDate),
            CreateTopic(topicId++, "Cuộc sống quanh em", "Vẽ tranh đề tài cuộc sống, sinh hoạt hàng ngày.", g4Art, createdDate),
            CreateTopic(topicId++, "Thưởng thức mĩ thuật", "Tranh của Tô Ngọc Vân, Van Gogh. Phân tích bố cục, màu sắc.", g4Art, createdDate),
        });

        // --- Grade 4 Music (ID=33) ---
        var g4Music = Guid.Parse("00000000-0000-0000-0000-000000000033");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Khởi đầu mới", "Khuông nhạc, khóa Son. Nốt nhạc trên khuông.", g4Music, createdDate),
            CreateTopic(topicId++, "Giai điệu quê hương", "Dân ca các vùng miền. Nhạc cụ: Đàn Bầu, Đàn Tranh.", g4Music, createdDate),
            CreateTopic(topicId++, "Thầy cô và mái trường", "Hát bài về thầy cô. Thực hành đọc nhạc.", g4Music, createdDate),
            CreateTopic(topicId++, "Tuổi thơ và ước mơ", "Mozart, Beethoven – thần đồng âm nhạc. Thưởng thức nhạc cổ điển.", g4Music, createdDate),
            CreateTopic(topicId++, "Ước mơ bay xa", "Sáo Recorder hoặc Melodica. Thực hành chơi nhạc cụ.", g4Music, createdDate),
            CreateTopic(topicId++, "Vui đón xuân", "Hát bài Tết, xuân. Ôn tập tổng kết năm học.", g4Music, createdDate),
        });

        // --- Grade 4 Experiential Activities (ID=34) ---
        var g4Exp = Guid.Parse("00000000-0000-0000-0000-000000000034");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Nhà trường", "Xây dựng lớp học hạnh phúc, tự quản, hoạt động Đội.", g4Exp, createdDate),
            CreateTopic(topicId++, "Cộng đồng", "Hoạt động thiện nguyện, tham gia sự kiện cộng đồng.", g4Exp, createdDate),
            CreateTopic(topicId++, "Thiên nhiên", "Khám phá thiên nhiên, bảo vệ môi trường sống.", g4Exp, createdDate),
            CreateTopic(topicId++, "Kỹ năng tự phục vụ", "Quản lý chi tiêu đơn giản, phòng chống đuối nước.", g4Exp, createdDate),
            CreateTopic(topicId++, "Nghề nghiệp", "Tìm hiểu nghề nghiệp, đóng vai, lập kế hoạch tương lai.", g4Exp, createdDate),
        });

        // --- Grade 4 Information Technology (ID=35) ---
        var g4IT = Guid.Parse("00000000-0000-0000-0000-000000000035");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Phần cứng và phần mềm", "Phân biệt phần cứng (hardware) và phần mềm (software). Bộ nhớ.", g4IT, createdDate),
            CreateTopic(topicId++, "Internet và tìm kiếm", "Sử dụng trình duyệt, tìm kiếm Google, đánh giá nguồn thông tin.", g4IT, createdDate),
            CreateTopic(topicId++, "Tệp và thư mục nâng cao", "Quản lý tệp, sao chép, di chuyển, nén/giải nén.", g4IT, createdDate),
            CreateTopic(topicId++, "Bản quyền số", "Tôn trọng bản quyền phần mềm, hình ảnh, nhạc. Quy tắc sử dụng.", g4IT, createdDate),
            CreateTopic(topicId++, "Soạn thảo văn bản", "Microsoft Word: nhập văn bản, định dạng chữ, chèn hình ảnh.", g4IT, createdDate),
            CreateTopic(topicId++, "Lập trình Scratch", "Giao diện Scratch, khối lệnh, lập trình hoạt hình và trò chơi đơn giản.", g4IT, createdDate),
        });

        // --- Grade 4 Technology (ID=36) ---
        var g4Tech = Guid.Parse("00000000-0000-0000-0000-000000000036");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Trồng hoa và cây cảnh", "Kỹ thuật gieo hạt, trồng, chăm sóc cây hoa và cây cảnh.", g4Tech, createdDate),
            CreateTopic(topicId++, "Lắp ráp mô hình kỹ thuật", "Lắp ráp cái đu, rô-bốt, bập bênh, đồ chơi dân gian từ bộ lắp ghép.", g4Tech, createdDate),
        });

        // --- Grade 4 History & Geography (ID=37) ---
        var g4HistGeo = Guid.Parse("00000000-0000-0000-0000-000000000037");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Phương tiện học tập Lịch sử và Địa lí", "Bản đồ, lược đồ, biểu đồ. Cách sử dụng bản đồ để học địa lí.", g4HistGeo, createdDate),
            CreateTopic(topicId++, "Địa phương em", "Đặc điểm tự nhiên, dân cư, kinh tế địa phương.", g4HistGeo, createdDate),
            CreateTopic(topicId++, "Trung du và miền núi Bắc Bộ", "Đặc điểm địa hình, khí hậu, dân tộc. Đền Hùng, truyền thuyết Hùng Vương.", g4HistGeo, createdDate),
            CreateTopic(topicId++, "Đồng bằng Bắc Bộ", "Đồng bằng sông Hồng, kinh thành Thăng Long, Văn Miếu – Quốc Tử Giám.", g4HistGeo, createdDate),
            CreateTopic(topicId++, "Duyên hải miền Trung", "Đặc điểm bờ biển, kinh tế. Di sản: Cố đô Huế, phố cổ Hội An.", g4HistGeo, createdDate),
            CreateTopic(topicId++, "Tây Nguyên và Nam Bộ", "Tây Nguyên: cồng chiêng, cà phê. Nam Bộ: TP.HCM, địa đạo Củ Chi, đồng bằng sông Cửu Long.", g4HistGeo, createdDate),
        });

        // =================================================================
        //  GRADE 5
        // =================================================================

        // --- Grade 5 Math (ID=38) ---
        var g5Math = Guid.Parse("00000000-0000-0000-0000-000000000038");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Ôn tập phân số và số thập phân", "Ôn tập phân số. Khái niệm số thập phân, chuyển đổi phân số ↔ số thập phân.", g5Math, createdDate),
            CreateTopic(topicId++, "Bốn phép tính với số thập phân", "Cộng, trừ, nhân, chia số thập phân. Tính giá trị biểu thức.", g5Math, createdDate),
            CreateTopic(topicId++, "Hình học", "Tam giác, hình thang (chu vi, diện tích). Hình tròn (chu vi, diện tích). Hình hộp chữ nhật, lập phương (diện tích xung quanh, toàn phần, thể tích).", g5Math, createdDate),
            CreateTopic(topicId++, "Tỉ số phần trăm", "Khái niệm phần trăm (%). Tìm tỉ số phần trăm, tìm giá trị theo %. Bài toán lãi suất cơ bản.", g5Math, createdDate),
            CreateTopic(topicId++, "Đo lường và chuyển động đều", "Đổi đơn vị đo. Vận tốc, quãng đường, thời gian: s = v × t.", g5Math, createdDate),
            CreateTopic(topicId++, "Thống kê và xác suất", "Biểu đồ hình quạt (đọc, nhận xét). Xác suất thực nghiệm.", g5Math, createdDate),
            CreateTopic(topicId++, "Ôn tập cuối năm", "Ôn tập tổng hợp chuẩn bị chuyển cấp: số thập phân, phân số, hình học, đo lường, tỉ số %.", g5Math, createdDate),
        });

        // --- Grade 5 Literature (ID=39) ---
        var g5Lit = Guid.Parse("00000000-0000-0000-0000-000000000039");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Thế giới tuổi thơ", "Chủ điểm 1: Từ đồng nghĩa. Tả phong cảnh. Đọc hiểu về tuổi thơ.", g5Lit, createdDate),
            CreateTopic(topicId++, "Cộng đồng", "Chủ điểm 2: Từ trái nghĩa. Văn bản về cộng đồng, sẻ chia.", g5Lit, createdDate),
            CreateTopic(topicId++, "Quê hương", "Chủ điểm 3: Từ đồng âm, từ nhiều nghĩa. Tả người. Văn bản về quê hương.", g5Lit, createdDate),
            CreateTopic(topicId++, "Ôn tập học kì 1", "Ôn tập: từ đồng nghĩa, trái nghĩa, đồng âm, nhiều nghĩa. Viết bài tả phong cảnh, tả người.", g5Lit, createdDate),
            CreateTopic(topicId++, "Bảo vệ môi trường", "Chủ điểm 4: Đại từ, quan hệ từ. Kể chuyện sáng tạo. Bảo vệ môi trường.", g5Lit, createdDate),
            CreateTopic(topicId++, "Chủ nhân tương lai", "Chủ điểm 5: Câu đơn, câu ghép. Văn bản về trẻ em, tương lai.", g5Lit, createdDate),
            CreateTopic(topicId++, "Vững bước vào cấp hai", "Chủ điểm 6: Ôn tập tổng hợp chuyển cấp. Hệ thống ngữ pháp, từ vựng.", g5Lit, createdDate),
        });

        // --- Grade 5 English (ID=40) ---
        var g5Eng = Guid.Parse("00000000-0000-0000-0000-000000000040");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Me & My Friends", "Addresses, daily routines, hobbies. Where do you live? What do you do every day?", g5Eng, createdDate),
            CreateTopic(topicId++, "My School", "School subjects, events, school rules. Present continuous tense.", g5Eng, createdDate),
            CreateTopic(topicId++, "The World Around Us", "Holidays, transport, folk tales. Past Simple tense.", g5Eng, createdDate),
            CreateTopic(topicId++, "Review 1 (Themes 1-3)", "Ôn tập: daily routines, school, holidays. Grammar: present simple/continuous, past simple.", g5Eng, createdDate),
            CreateTopic(topicId++, "The Future", "Jobs, health & safety. Future Simple (will), Should for advice.", g5Eng, createdDate),
            CreateTopic(topicId++, "Festivals", "Vietnamese and world festivals. Descriptions, traditions, food.", g5Eng, createdDate),
            CreateTopic(topicId++, "Review 2 (Themes 4-5)", "Ôn tập: jobs, health, festivals. Grammar: future simple, should, overall review.", g5Eng, createdDate),
        });

        // --- Grade 5 Ethics (ID=41) ---
        var g5Ethics = Guid.Parse("00000000-0000-0000-0000-000000000041");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Biết ơn người có công với đất nước", "Anh hùng dân tộc, đền đáp công ơn, ngày thương binh liệt sĩ 27/7.", g5Ethics, createdDate),
            CreateTopic(topicId++, "Tôn trọng sự khác biệt", "Tôn trọng giới tính, dân tộc, tín ngưỡng. Không phân biệt đối xử.", g5Ethics, createdDate),
            CreateTopic(topicId++, "Vượt qua khó khăn", "Ý chí, nghị lực vượt khó. Gương người thành công từ hoàn cảnh khó khăn.", g5Ethics, createdDate),
            CreateTopic(topicId++, "Bảo vệ môi trường", "Trách nhiệm bảo vệ môi trường, ứng phó biến đổi khí hậu.", g5Ethics, createdDate),
            CreateTopic(topicId++, "Quyền và bổn phận trẻ em", "Công ước Liên Hợp Quốc về quyền trẻ em. Bổn phận của trẻ em.", g5Ethics, createdDate),
            CreateTopic(topicId++, "Tình hữu nghị quốc tế", "Hòa bình thế giới, hữu nghị giữa các dân tộc.", g5Ethics, createdDate),
        });

        // --- Grade 5 Science (ID=42) ---
        var g5Science = Guid.Parse("00000000-0000-0000-0000-000000000042");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Chất và biến đổi hóa học", "Hỗn hợp, dung dịch. Biến đổi lý học, hóa học. Lọc, lắng, bay hơi.", g5Science, createdDate),
            CreateTopic(topicId++, "Năng lượng", "Mạch điện đơn giản (pin, bóng đèn, công tắc). Năng lượng tái tạo (mặt trời, gió).", g5Science, createdDate),
            CreateTopic(topicId++, "Con người", "Sinh sản ở người. Tuổi dậy thì. Vệ sinh cơ thể.", g5Science, createdDate),
            CreateTopic(topicId++, "Thực vật và động vật", "Sinh sản hữu tính, vô tính ở thực vật và động vật.", g5Science, createdDate),
            CreateTopic(topicId++, "Vi khuẩn và nấm nâng cao", "Ứng dụng vi sinh vật: lên men (sữa chua, dưa muối). Nấm có ích và nấm độc.", g5Science, createdDate),
            CreateTopic(topicId++, "Môi trường và tài nguyên", "Ô nhiễm (không khí, nước, đất). Bảo tồn tài nguyên, phát triển bền vững.", g5Science, createdDate),
        });

        // --- Grade 5 Art (ID=43) ---
        var g5Art = Guid.Parse("00000000-0000-0000-0000-000000000043");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Màu sắc trang trí", "Gam màu nóng, lạnh trong thiết kế. Trang trí họa tiết.", g5Art, createdDate),
            CreateTopic(topicId++, "Di sản văn hóa", "Múa rối nước, nghệ thuật truyền thống Việt Nam.", g5Art, createdDate),
            CreateTopic(topicId++, "Tạo hình dáng người", "Kỹ thuật vẽ dáng người chuyển động, tỉ lệ cơ thể.", g5Art, createdDate),
            CreateTopic(topicId++, "Thiết kế đồ họa", "Thiết kế bao bì sản phẩm, typography cơ bản.", g5Art, createdDate),
            CreateTopic(topicId++, "Kiến trúc và không gian", "Luật xa gần (phối cảnh), vẽ phong cảnh có chiều sâu.", g5Art, createdDate),
            CreateTopic(topicId++, "Cuộc sống quanh em", "Tranh cổ động, tranh đề tài xã hội. Trưng bày cuối năm.", g5Art, createdDate),
        });

        // --- Grade 5 Music (ID=44) ---
        var g5Music = Guid.Parse("00000000-0000-0000-0000-000000000044");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Reo vang bình minh", "Nhịp 2/4. Hát bài về buổi sáng. Lý thuyết nhịp.", g5Music, createdDate),
            CreateTopic(topicId++, "Giai điệu quê hương", "Hát Xoan, Quan họ. Nhạc cụ: Đàn Nguyệt, Đàn Tì Bà.", g5Music, createdDate),
            CreateTopic(topicId++, "Lòng biết ơn", "Dấu luyến, dấu nối trong bài hát. Bài hát về thầy cô, cha mẹ.", g5Music, createdDate),
            CreateTopic(topicId++, "Âm nhạc năm châu", "Chopin, Tchaikovsky. Nghe nhạc cổ điển phương Tây.", g5Music, createdDate),
            CreateTopic(topicId++, "Khát vọng hòa bình", "Sáo Recorder nâng cao. Hát bè 2 bè. Bài hát về hòa bình.", g5Music, createdDate),
            CreateTopic(topicId++, "Nhớ ơn Bác Hồ", "Bài hát về Bác Hồ. Ôn tập tổng kết năm học.", g5Music, createdDate),
        });

        // --- Grade 5 Experiential Activities (ID=45) ---
        var g5Exp = Guid.Parse("00000000-0000-0000-0000-000000000045");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Nhà trường", "Tự quản lớp học, xây dựng video kỉ niệm, hoạt động Đội nâng cao.", g5Exp, createdDate),
            CreateTopic(topicId++, "Cộng đồng", "Hành trình về nguồn, góc cộng đồng xanh, hoạt động thiện nguyện.", g5Exp, createdDate),
            CreateTopic(topicId++, "Thiên nhiên", "Biến đổi khí hậu, ứng phó thiên tai, bảo vệ đa dạng sinh học.", g5Exp, createdDate),
            CreateTopic(topicId++, "Kỹ năng sống", "An toàn trên mạng Internet, quản lý chi tiêu, hội chợ mini.", g5Exp, createdDate),
            CreateTopic(topicId++, "Nghề nghiệp và chuẩn bị lên lớp 6", "Tìm hiểu nghề nghiệp, chuẩn bị kỹ năng chuyển cấp lên THCS.", g5Exp, createdDate),
        });

        // --- Grade 5 Information Technology (ID=46) ---
        var g5IT = Guid.Parse("00000000-0000-0000-0000-000000000046");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Tìm kiếm nâng cao", "Kỹ thuật tìm kiếm nâng cao trên Google, đánh giá độ tin cậy nguồn thông tin.", g5IT, createdDate),
            CreateTopic(topicId++, "Email", "Tạo và sử dụng email. Gửi, nhận, trả lời thư. An toàn email.", g5IT, createdDate),
            CreateTopic(topicId++, "Lưu trữ đám mây", "Google Drive: tải lên, chia sẻ tập tin, cộng tác trực tuyến.", g5IT, createdDate),
            CreateTopic(topicId++, "Công dân số", "Bắt nạt trên mạng (cyberbullying), quyền riêng tư, ứng xử văn minh trên mạng.", g5IT, createdDate),
            CreateTopic(topicId++, "Đa phương tiện và trình chiếu", "PowerPoint: tạo bài trình chiếu, chèn văn bản, hình ảnh, hiệu ứng.", g5IT, createdDate),
            CreateTopic(topicId++, "Lập trình Scratch nâng cao", "Vòng lặp, câu lệnh điều kiện, biến số. Lập trình trò chơi và ứng dụng.", g5IT, createdDate),
        });

        // --- Grade 5 Technology (ID=47) ---
        var g5Tech = Guid.Parse("00000000-0000-0000-0000-000000000047");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Công nghệ và đời sống", "Vai trò công nghệ trong cuộc sống. Thiết bị công nghệ thông dụng.", g5Tech, createdDate),
            CreateTopic(topicId++, "Thiết kế kỹ thuật tổng quát", "Quy trình thiết kế: ý tưởng → bản vẽ → chế tạo → thử nghiệm.", g5Tech, createdDate),
            CreateTopic(topicId++, "Lắp ráp mô hình điện", "Lắp ráp xe chạy điện, máy bay mô hình từ bộ lắp ghép.", g5Tech, createdDate),
            CreateTopic(topicId++, "Nuôi cá cảnh", "Chuẩn bị bể cá, chọn giống, chăm sóc cá cảnh.", g5Tech, createdDate),
            CreateTopic(topicId++, "Thiết kế đồ họa số", "Sử dụng phần mềm vẽ để thiết kế logo, poster đơn giản.", g5Tech, createdDate),
            CreateTopic(topicId++, "Công nghệ và nghề nghiệp", "Vai trò công nghệ trong các nghề. Định hướng nghề nghiệp.", g5Tech, createdDate),
        });

        // --- Grade 5 History & Geography (ID=48) ---
        var g5HistGeo = Guid.Parse("00000000-0000-0000-0000-000000000048");
        topics.AddRange(new[]
        {
            CreateTopic(topicId++, "Đất nước Việt Nam", "Địa lí: Vị trí, hình dạng đất nước. Biển đảo, Hoàng Sa, Trường Sa.", g5HistGeo, createdDate),
            CreateTopic(topicId++, "Thiên nhiên Việt Nam", "Địa hình, khí hậu, sông ngòi. Đặc điểm thiên nhiên các vùng miền.", g5HistGeo, createdDate),
            CreateTopic(topicId++, "Các châu lục và đại dương", "5 châu lục, 4 đại dương. Đặc điểm chính của các châu lục.", g5HistGeo, createdDate),
            CreateTopic(topicId++, "Việt Nam đầu thế kỷ XX - 1945", "Lịch sử: Phong trào yêu nước, Đảng Cộng sản Việt Nam ra đời, Cách mạng Tháng Tám 1945.", g5HistGeo, createdDate),
            CreateTopic(topicId++, "Kháng chiến chống Pháp", "Chiến thắng Điện Biên Phủ 1954, Hiệp định Giơ-ne-vơ.", g5HistGeo, createdDate),
            CreateTopic(topicId++, "Kháng chiến chống Mỹ", "Chiến dịch Hồ Chí Minh, giải phóng miền Nam 30/4/1975, thống nhất đất nước.", g5HistGeo, createdDate),
            CreateTopic(topicId++, "Xây dựng và đổi mới", "Thời kỳ đổi mới từ 1986, hội nhập quốc tế, thành tựu phát triển.", g5HistGeo, createdDate),
        });

        builder.HasData(topics);
    }

    private static object CreateTopic(
        int id,
        string title,
        string description,
        Guid subjectId,
        DateTime createdAt,
        Guid? createdBy = null
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
