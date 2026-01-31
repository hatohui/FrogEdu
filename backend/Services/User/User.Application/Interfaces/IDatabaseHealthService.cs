using FrogEdu.User.Application.Queries.CheckDatabaseHealth;

namespace FrogEdu.User.Application.Interfaces;

public interface IDatabaseHealthService
{
    Task<DatabaseHealthDto> CheckHealthAsync(CancellationToken cancellationToken = default);
}
