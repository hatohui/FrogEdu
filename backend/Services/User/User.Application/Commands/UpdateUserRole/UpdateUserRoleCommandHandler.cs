using FrogEdu.Shared.Kernel;
using FrogEdu.User.Application.Interfaces;
using FrogEdu.User.Domain.Repositories;
using MediatR;

namespace FrogEdu.User.Application.Commands.UpdateUserRole;

public sealed class UpdateUserRoleCommandHandler(
    IUserRepository userRepository,
    IRoleService roleService
) : IRequestHandler<UpdateUserRoleCommand, Result>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IRoleService _roleService = roleService;

    public async Task<Result> Handle(
        UpdateUserRoleCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure("User not found");
        }

        // Verify role exists
        var role = await _roleService.GetRoleByIdAsync(request.RoleId, cancellationToken);
        if (role is null)
        {
            return Result.Failure("Role not found");
        }

        user.ChangeRole(request.RoleId);

        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
