using MediatR;

namespace FrogEdu.Class.Application.Queries.CheckDatabaseHealth;

/// <summary>
/// Query to check database health status
/// </summary>
public sealed record CheckDatabaseHealthQuery : IRequest<DatabaseHealthDto>;
