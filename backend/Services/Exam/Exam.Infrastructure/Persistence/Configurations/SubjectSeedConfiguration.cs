using FrogEdu.Exam.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.Exam.Infrastructure.Persistence.Configurations;

public class SubjectSeedConfiguration : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        var subjects = new List<object>();

        // Subjects available per grade based on "Kết nối tri thức với cuộc sống" curriculum
        var allSubjects = new Dictionary<string, (string Name, string Description)>
        {
            ["math"] = ("Mathematics", "Toán học – Nghiên cứu về số, phép tính và hình học"),
            ["literature"] = (
                "Literature",
                "Tiếng Việt – Đọc, viết, nghe, nói và kiến thức ngôn ngữ"
            ),
            ["english"] = ("English", "Tiếng Anh – Ngôn ngữ và giao tiếp quốc tế"),
            ["ethics"] = ("Ethics", "Đạo đức – Giáo dục giá trị sống và kỹ năng ứng xử"),
            ["nature_and_society"] = (
                "Nature & Society",
                "Tự nhiên và Xã hội – Khám phá thế giới tự nhiên và đời sống xã hội"
            ),
            ["art"] = ("Art", "Mĩ thuật – Nghệ thuật tạo hình và thẩm mỹ"),
            ["music"] = ("Music", "Âm nhạc – Lý thuyết âm nhạc và biểu diễn"),
            ["experiential_activities"] = (
                "Experiential Activities",
                "Hoạt động trải nghiệm – Học tập thực hành và kỹ năng sống"
            ),
            ["information_technology"] = (
                "Information Technology",
                "Tin học – Khoa học máy tính và công nghệ số"
            ),
            ["technology"] = ("Technology", "Công nghệ – Khoa học ứng dụng và kỹ thuật"),
            ["science"] = ("Science", "Khoa học – Khám phá thế giới tự nhiên và hiện tượng vật lý"),
            ["history_geography"] = (
                "History & Geography",
                "Lịch sử và Địa lí – Nghiên cứu lịch sử và địa lý Việt Nam"
            ),
        };

        // Grade 1-2: art, english, ethics, experiential_activities, literature, math, music, nature_and_society
        var grade1_2 = new[]
        {
            "math",
            "literature",
            "english",
            "ethics",
            "nature_and_society",
            "art",
            "music",
            "experiential_activities",
        };
        // Grade 3: adds information_technology, technology
        var grade3 = new[]
        {
            "math",
            "literature",
            "english",
            "ethics",
            "nature_and_society",
            "art",
            "music",
            "experiential_activities",
            "information_technology",
            "technology",
        };
        // Grade 4-5: replaces nature_and_society with science, adds history_geography
        var grade4_5 = new[]
        {
            "math",
            "literature",
            "english",
            "ethics",
            "science",
            "art",
            "music",
            "experiential_activities",
            "information_technology",
            "technology",
            "history_geography",
        };

        var gradeSubjects = new Dictionary<int, string[]>
        {
            [1] = grade1_2,
            [2] = grade1_2,
            [3] = grade3,
            [4] = grade4_5,
            [5] = grade4_5,
        };

        int idCounter = 1;
        for (int grade = 1; grade <= 5; grade++)
        {
            foreach (var code in gradeSubjects[grade])
            {
                var (name, desc) = allSubjects[code];
                subjects.Add(
                    new
                    {
                        Id = Guid.Parse($"00000000-0000-0000-0000-{idCounter:D12}"),
                        SubjectCode = code,
                        Name = name,
                        Description = desc,
                        Grade = grade,
                        ImageUrl = (string?)null,
                        CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                        UpdatedAt = (DateTime?)null,
                        IsDeleted = false,
                    }
                );
                idCounter++;
            }
        }

        builder.HasData(subjects);
    }
}
