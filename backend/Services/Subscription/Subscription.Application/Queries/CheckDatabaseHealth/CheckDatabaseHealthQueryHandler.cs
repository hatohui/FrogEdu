using FrogEdu.Subscription.Application.Interfaces;
using MediatR;

namespace FrogEdu.Subscription.Application.Queries.CheckDatabaseHealth;

/// <summary>
/// Handler for CheckDatabaseHealthQuery
/// </summary>
public sealed class CheckDatabaseHealthQueryHandler
    : IRequestHandler<CheckDatabaseHealthQuery, DatabaseHealthDto>
{
    private readonly IDatabaseHealthService _healthService;

    public CheckDatabaseHealthQueryHandler(IDatabaseHealthService healthService)
    {
        _healthService = healthService;
    }

    public async Task<DatabaseHealthDto> Handle(
        CheckDatabaseHealthQuery request,
        CancellationToken cancellationToken
    )
    {
        return await _healthService.CheckHealthAsync(cancellationToken);
    }
}
