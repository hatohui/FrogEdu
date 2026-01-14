using FrogEdu.Shared.Kernel;

namespace FrogEdu.Content.Domain.ValueObjects;

/// <summary>
/// Subject value object representing Vietnamese primary school subjects
/// </summary>
public class Subject : ValueObject
{
    public string Name { get; private set; }

    private static readonly string[] ValidSubjects = new[]
    {
        "Toán", // Math
        "Tiếng Việt", // Vietnamese
        "Tiếng Anh", // English
        "Khoa học", // Science
        "Đạo đức", // Ethics/Morality
        "Âm nhạc", // Music
        "Mỹ thuật", // Arts
        "Thể dục", // Physical Education
        "Tin học", // Computer Science
        "Tự nhiên và Xã hội", // Nature and Society
    };

    private Subject(string name)
    {
        Name = name;
    }

    public static Subject Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Subject name cannot be empty", nameof(name));

        if (!ValidSubjects.Contains(name, StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException(
                $"Invalid subject: {name}. Valid subjects are: {string.Join(", ", ValidSubjects)}",
                nameof(name)
            );

        return new Subject(name);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Name;
    }

    public override string ToString() => Name;

    // Predefined subjects for convenience
    public static Subject Math => Create("Toán");
    public static Subject Vietnamese => Create("Tiếng Việt");
    public static Subject English => Create("Tiếng Anh");
    public static Subject Science => Create("Khoa học");
    public static Subject Ethics => Create("Đạo đức");
}
