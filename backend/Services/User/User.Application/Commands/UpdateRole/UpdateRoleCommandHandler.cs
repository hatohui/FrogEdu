using FrogEdu.Shared.Kernel.Primitives;
using FrogEdu.User.Application.Interfaces;
using FrogEdu.User.Domain.Enums;
using MediatR;

namespace FrogEdu.User.Application.Commands.UpdateRole;

public sealed class UpdateRoleCommandHandler(IRoleService roleService)
    : IRequestHandler<UpdateRoleCommand, Result>
{
    private readonly IRoleService _roleService = roleService;

    public async Task<Result> Handle(
        UpdateRoleCommand request,
        CancellationToken cancellationToken
    )
    {
        var role = await _roleService.GetRoleEntityByIdAsync(request.Id, cancellationToken);

        if (role is null)
        {
            return Result.Failure(new Error("Role.NotFound", "Role not found"));
        }

        // Parse the role name to enum
        if (!Enum.TryParse<UserRole>(request.Name, true, out var roleEnum))
        {
            return Result.Failure(
                new Error("Role.InvalidName", "Invalid role name provided")
            );
        }

        role.Update(roleEnum, request.Description);

        await _roleService.UpdateRoleAsync(role, cancellationToken);

        return Result.Success();
    }
}
