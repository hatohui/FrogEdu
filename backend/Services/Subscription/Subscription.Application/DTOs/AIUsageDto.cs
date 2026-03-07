namespace FrogEdu.Subscription.Application.DTOs;

public sealed record AIUsageDto
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public string ActionType { get; init; } = null!;
    public DateTime UsedAt { get; init; }
    public string? Metadata { get; init; }
}

public sealed record AIUsageLimitDto
{
    public Guid UserId { get; init; }
    public string Plan { get; init; } = null!;
    public int UsedCount { get; init; }
    public int? MaxAllowed { get; init; } // null = unlimited
    public int Remaining { get; init; }
    public bool CanUseAI { get; init; }
    public bool IsUnlimited { get; init; }
}
