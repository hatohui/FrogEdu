using FrogEdu.Shared.Kernel;
using FrogEdu.User.Application.Interfaces;
using MediatR;

namespace FrogEdu.User.Application.Commands.DeleteRole;

public sealed class DeleteRoleCommandHandler(IRoleService roleService)
    : IRequestHandler<DeleteRoleCommand, Result>
{
    private readonly IRoleService _roleService = roleService;

    public async Task<Result> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleService.GetRoleEntityByIdAsync(request.Id, cancellationToken);

        if (role is null)
        {
            return Result.Failure("Role not found");
        }

        // Check if any users are using this role
        var hasUsers = await _roleService.HasUsersWithRoleAsync(request.Id, cancellationToken);
        if (hasUsers)
        {
            return Result.Failure("Cannot delete role that is assigned to users");
        }

        await _roleService.DeleteRoleAsync(role, cancellationToken);

        return Result.Success();
    }
}
