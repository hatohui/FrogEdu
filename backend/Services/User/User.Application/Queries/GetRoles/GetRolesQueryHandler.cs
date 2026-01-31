using FrogEdu.User.Application.DTOs;
using FrogEdu.User.Application.Interfaces;
using MediatR;

namespace FrogEdu.User.Application.Queries.GetRoles;

public sealed class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, IReadOnlyList<RoleDto>>
{
    private readonly IRoleService _roleService;

    public GetRolesQueryHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public Task<IReadOnlyList<RoleDto>> Handle(
        GetRolesQuery request,
        CancellationToken cancellationToken
    )
    {
        return _roleService.GetAllRolesAsync(cancellationToken);
    }
}
