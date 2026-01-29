using MediatR;

namespace FrogEdu.Exam.Application.Queries.CheckDatabaseHealth;

/// <summary>
/// Query to check database health status
/// </summary>
public sealed record CheckDatabaseHealthQuery : IRequest<DatabaseHealthDto>;
