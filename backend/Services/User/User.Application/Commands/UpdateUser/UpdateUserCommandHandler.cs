using FrogEdu.Shared.Kernel;
using FrogEdu.User.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FrogEdu.User.Application.Commands.UpdateUser;

public sealed class UpdateUserCommandHandler(
    IUserRepository userRepository,
    ILogger<UpdateUserCommandHandler> logger
) : IRequestHandler<UpdateUserCommand, Result>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ILogger<UpdateUserCommandHandler> _logger = logger;

    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure("User not found");
        }

        // Update user properties
        if (
            !string.IsNullOrWhiteSpace(request.FirstName)
            || !string.IsNullOrWhiteSpace(request.LastName)
        )
        {
            user.UpdateProfile(
                request.FirstName ?? user.FirstName,
                request.LastName ?? user.LastName
            );
        }

        if (request.RoleId.HasValue && request.RoleId.Value != user.RoleId)
        {
            // Update role - using reflection since there's no direct method
            var roleIdProperty = user.GetType().GetProperty(nameof(user.RoleId));
            roleIdProperty?.SetValue(user, request.RoleId.Value);
        }

        await _userRepository.UpdateAsync(user, cancellationToken);

        _logger.LogInformation("Updated user {UserId}", request.UserId);

        return Result.Success();
    }
}
