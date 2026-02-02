using FrogEdu.Shared.Kernel.Primitives;

namespace FrogEdu.Exam.Domain.Entities;

public sealed class Subject : Entity
{
    public string SubjectCode { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string? ImageUrl { get; private set; }
    public int Grade { get; private set; }

    private readonly List<Topic> _topics = [];
    public IReadOnlyCollection<Topic> Topics => _topics.AsReadOnly();

    private Subject() { }

    private Subject(
        string subjectCode,
        string name,
        string description,
        int grade,
        string? imageUrl = null
    )
    {
        SubjectCode = subjectCode;
        Name = name;
        Description = description;
        Grade = grade;
        ImageUrl = imageUrl;
    }

    public static Subject Create(
        string subjectCode,
        string name,
        string description,
        int grade,
        string? imageUrl = null
    )
    {
        if (string.IsNullOrWhiteSpace(subjectCode))
            throw new ArgumentException("Subject code cannot be empty", nameof(subjectCode));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));
        if (grade < 1 || grade > 5)
            throw new ArgumentException(
                "Grade must be between 1 and 5 (primary school)",
                nameof(grade)
            );

        return new Subject(subjectCode, name, description, grade, imageUrl);
    }

    public void UpdateProfile(string name, string description, string? imageUrl = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        Name = name;
        Description = description;
        ImageUrl = imageUrl;
    }

    public void UpdateImage(string imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
            throw new ArgumentException("Image URL cannot be empty", nameof(imageUrl));

        ImageUrl = imageUrl;
    }
}
