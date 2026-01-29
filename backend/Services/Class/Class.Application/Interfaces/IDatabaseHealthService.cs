using FrogEdu.Class.Application.Queries.CheckDatabaseHealth;

namespace FrogEdu.Class.Application.Interfaces;

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
