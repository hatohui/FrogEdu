using FrogEdu.Shared.Kernel;

namespace FrogEdu.Class.Domain.Events;

public sealed record ClassRoomCreatedDomainEvent(Guid ClassRoomId, string Name, string InviteCode)
    : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record InviteCodeRegeneratedDomainEvent(Guid ClassRoomId, string NewInviteCode)
    : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record StudentEnrolledDomainEvent(Guid ClassRoomId, Guid StudentId) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record StudentKickedDomainEvent(Guid ClassRoomId, Guid StudentId) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
