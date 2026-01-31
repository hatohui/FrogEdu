using FrogEdu.User.Application.Interfaces;
using MediatR;

namespace FrogEdu.User.Application.Queries.CheckDatabaseHealth;

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
