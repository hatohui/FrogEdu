namespace FrogEdu.Exam.Application.Queries.CheckDatabaseHealth;

/// <summary>
/// DTO representing database health status
/// </summary>
public sealed record DatabaseHealthDto(
    bool IsHealthy,
    string Status,
    long ResponseTimeMs,
    string? Error = null,
    DateTime Timestamp = default
)
{
    public DatabaseHealthDto(
        bool IsHealthy,
        string Status,
        long ResponseTimeMs,
        string? Error = null
    )
        : this(IsHealthy, Status, ResponseTimeMs, Error, DateTime.UtcNow) { }
}
