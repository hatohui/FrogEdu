using FrogEdu.Shared.Kernel;

namespace FrogEdu.Exam.Domain.Events;

/// <summary>
/// Domain event raised when a textbook is created
/// </summary>
public record TextbookCreatedEvent(Guid TextbookId, string Title, int GradeLevel, string Subject)
    : DomainEvent;

/// <summary>
/// Domain event raised when a textbook is updated
/// </summary>
public record TextbookUpdatedEvent(
    Guid TextbookId,
    string Title,
    int GradeLevel,
    string Subject,
    bool IsDeleted
) : DomainEvent;


