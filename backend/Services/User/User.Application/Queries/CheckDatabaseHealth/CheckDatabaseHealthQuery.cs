using MediatR;

namespace FrogEdu.User.Application.Queries.CheckDatabaseHealth;

/// <summary>
/// Query to check database health status
/// </summary>
public sealed record CheckDatabaseHealthQuery : IRequest<DatabaseHealthDto>;
