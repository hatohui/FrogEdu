using FrogEdu.Shared.Kernel;

namespace FrogEdu.Class.Domain.ValueObjects;

/// <summary>
/// Exam Matrix value object defining question distribution requirements
/// </summary>
public class ExamMatrix : ValueObject
{
    public int EasyCount { get; private set; }
    public int MediumCount { get; private set; }
    public int HardCount { get; private set; }
    public decimal EasyPoints { get; private set; }
    public decimal MediumPoints { get; private set; }
    public decimal HardPoints { get; private set; }

    public int TotalQuestions => EasyCount + MediumCount + HardCount;
    public decimal TotalPoints => EasyPoints + MediumPoints + HardPoints;

    private ExamMatrix() { } // For EF Core

    private ExamMatrix(
        int easyCount,
        int mediumCount,
        int hardCount,
        decimal easyPoints,
        decimal mediumPoints,
        decimal hardPoints
    )
    {
        if (easyCount < 0 || mediumCount < 0 || hardCount < 0)
            throw new ArgumentException("Question counts cannot be negative");

        if (easyPoints < 0 || mediumPoints < 0 || hardPoints < 0)
            throw new ArgumentException("Points cannot be negative");

        if (easyCount + mediumCount + hardCount == 0)
            throw new ArgumentException("Exam must have at least one question");

        EasyCount = easyCount;
        MediumCount = mediumCount;
        HardCount = hardCount;
        EasyPoints = easyPoints;
        MediumPoints = mediumPoints;
        HardPoints = hardPoints;
    }

    public static ExamMatrix Create(
        int easyCount,
        int mediumCount,
        int hardCount,
        decimal easyPoints,
        decimal mediumPoints,
        decimal hardPoints
    )
    {
        return new ExamMatrix(
            easyCount,
            mediumCount,
            hardCount,
            easyPoints,
            mediumPoints,
            hardPoints
        );
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return EasyCount;
        yield return MediumCount;
        yield return HardCount;
        yield return EasyPoints;
        yield return MediumPoints;
        yield return HardPoints;
    }

    public override string ToString() =>
        $"Matrix: Easy={EasyCount}({EasyPoints}pts), Medium={MediumCount}({MediumPoints}pts), Hard={HardCount}({HardPoints}pts)";
}
