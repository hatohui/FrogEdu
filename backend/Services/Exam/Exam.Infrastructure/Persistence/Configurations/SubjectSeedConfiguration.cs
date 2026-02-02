using FrogEdu.Exam.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrogEdu.Exam.Infrastructure.Persistence.Configurations;

public class SubjectSeedConfiguration : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        var subjects = new List<object>();

        var subjectDefinitions = new[]
        {
            new
            {
                Code = "math",
                Name = "Mathematics",
                Description = "Study of numbers, quantities, and shapes",
            },
            new
            {
                Code = "history_geography",
                Name = "History & Geography",
                Description = "Study of past events and Earth's features",
            },
            new
            {
                Code = "english",
                Name = "English",
                Description = "English language and literature",
            },
            new
            {
                Code = "literature",
                Name = "Literature",
                Description = "Study of written works and literary analysis",
            },
            new
            {
                Code = "it",
                Name = "Information Technology",
                Description = "Computer science and digital technology",
            },
            new
            {
                Code = "technology",
                Name = "Technology",
                Description = "Applied science and engineering",
            },
            new
            {
                Code = "art",
                Name = "Art",
                Description = "Visual arts and creative expression",
            },
            new
            {
                Code = "science",
                Name = "Science",
                Description = "Study of natural world and physical phenomena",
            },
            new
            {
                Code = "music",
                Name = "Music",
                Description = "Musical theory and performance",
            },
            new
            {
                Code = "experiential_activities",
                Name = "Experiential Activities",
                Description = "Hands-on learning and practical experiences",
            },
        };

        int idCounter = 1;
        for (int grade = 1; grade <= 5; grade++)
        {
            foreach (var subject in subjectDefinitions)
            {
                subjects.Add(
                    new
                    {
                        Id = Guid.Parse($"00000000-0000-0000-0000-{idCounter:D12}"),
                        SubjectCode = subject.Code,
                        Name = subject.Name,
                        Description = subject.Description,
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
