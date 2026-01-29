using FrogEdu.Subscription.Application.Queries.CheckDatabaseHealth;

namespace FrogEdu.Subscription.Application.Interfaces;

/// <summary>
/// Interface for database health checks
/// </summary>
public interface IDatabaseHealthService
{
    /// <summary>
    /// Checks the health of the database connection
    /// </summary>
    Task<DatabaseHealthDto> CheckHealthAsync(CancellationToken cancellationToken = default);
}
