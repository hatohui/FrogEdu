namespace FrogEdu.Subscription.Application.Queries.CheckDatabaseHealth;

/// <summary>
/// DTO representing database health status
/// </summary>
public sealed record DatabaseHealthDto(
    bool IsHealthy,
    string Status,
    long ResponseTimeMs,
    string? Error = null
)
{
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
}
