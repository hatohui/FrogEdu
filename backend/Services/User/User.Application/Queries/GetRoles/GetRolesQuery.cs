using FrogEdu.User.Application.DTOs;
using MediatR;

namespace FrogEdu.User.Application.Queries.GetRoles;

public sealed record GetRolesQuery : IRequest<IReadOnlyList<RoleDto>>;
