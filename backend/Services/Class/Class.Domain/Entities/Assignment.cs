using FrogEdu.Shared.Kernel.Primitives;

namespace FrogEdu.Class.Domain.Entities;

public sealed class Assignment : Entity
{
    public Guid ClassId { get; private set; }
    public Guid ExamId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime DueDate { get; private set; }
    public bool IsMandatory { get; private set; }
    public int Weight { get; private set; } // Weight for grading (0-100)

    private Assignment() { }

    private Assignment(
        Guid classId,
        Guid examId,
        DateTime startDate,
        DateTime dueDate,
        bool isMandatory,
        int weight
    )
    {
        ClassId = classId;
        ExamId = examId;
        StartDate = startDate;
        DueDate = dueDate;
        IsMandatory = isMandatory;
        Weight = weight;
    }

    public static Assignment Create(
        Guid classId,
        Guid examId,
        DateTime startDate,
        DateTime dueDate,
        bool isMandatory = true,
        int weight = 100
    )
    {
        if (classId == Guid.Empty)
            throw new ArgumentException("Class ID cannot be empty", nameof(classId));
        if (examId == Guid.Empty)
            throw new ArgumentException("Exam ID cannot be empty", nameof(examId));
        if (startDate >= dueDate)
            throw new ArgumentException("Start date must be before due date", nameof(startDate));
        if (weight < 0 || weight > 100)
            throw new ArgumentException("Weight must be between 0 and 100", nameof(weight));

        return new Assignment(classId, examId, startDate, dueDate, isMandatory, weight);
    }

    public void Update(DateTime startDate, DateTime dueDate, bool isMandatory, int weight)
    {
        if (startDate >= dueDate)
            throw new ArgumentException("Start date must be before due date", nameof(startDate));
        if (weight < 0 || weight > 100)
            throw new ArgumentException("Weight must be between 0 and 100", nameof(weight));

        StartDate = startDate;
        DueDate = dueDate;
        IsMandatory = isMandatory;
        Weight = weight;
    }

    public bool IsOverdue()
    {
        return DateTime.UtcNow > DueDate;
    }

    public bool IsActive()
    {
        var now = DateTime.UtcNow;
        return now >= StartDate && now <= DueDate;
    }
}
