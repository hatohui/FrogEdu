using MediatR;

namespace FrogEdu.User.Application.Queries.CheckDatabaseHealth;

public sealed record CheckDatabaseHealthQuery : IRequest<DatabaseHealthDto>;
