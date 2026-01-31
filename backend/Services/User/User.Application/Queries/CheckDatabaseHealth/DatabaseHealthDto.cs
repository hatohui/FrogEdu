namespace FrogEdu.User.Application.Queries.CheckDatabaseHealth;

public sealed record DatabaseHealthDto(
    bool IsHealthy,
    string Status,
    long ResponseTimeMs,
    string? Error = null
)
{
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
}
