using FrogEdu.Shared.Kernel;
using FrogEdu.User.Application.Interfaces;
using FrogEdu.User.Domain.Enums;
using MediatR;

namespace FrogEdu.User.Application.Commands.CreateRole;

public sealed class CreateRoleCommandHandler(IRoleService roleService)
    : IRequestHandler<CreateRoleCommand, Result<Guid>>
{
    private readonly IRoleService _roleService = roleService;

    public async Task<Result<Guid>> Handle(
        CreateRoleCommand request,
        CancellationToken cancellationToken
    )
    {
        // Check if role with this name already exists
        var existingRole = await _roleService.GetRoleByNameAsync(request.Name, cancellationToken);

        if (existingRole is not null)
        {
            return Result<Guid>.Failure("Role already exists");
        }

        // Parse the role name to enum
        if (!Enum.TryParse<UserRole>(request.Name, true, out var roleEnum))
        {
            return Result<Guid>.Failure("Invalid role name provided");
        }

        var role = Domain.Entities.Role.Create(roleEnum, request.Description);

        await _roleService.CreateRoleAsync(role, cancellationToken);

        return Result<Guid>.Success(role.Id);
    }
}
