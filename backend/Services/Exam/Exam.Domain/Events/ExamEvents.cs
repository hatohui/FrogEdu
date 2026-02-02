using FrogEdu.Shared.Kernel;

namespace FrogEdu.Exam.Domain.Events;

public sealed record ExamCreatedDomainEvent(Guid ExamId, string Title) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record ExamPublishedDomainEvent(Guid ExamId, string Title, string AccessCode)
    : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record ExamArchivedDomainEvent(Guid ExamId, string Title) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
