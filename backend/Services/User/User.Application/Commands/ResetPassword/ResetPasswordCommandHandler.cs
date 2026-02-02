using FrogEdu.Shared.Kernel;
using FrogEdu.User.Application.Interfaces;
using FrogEdu.User.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FrogEdu.User.Application.Commands.ResetPassword;

public sealed class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly ILogger<ResetPasswordCommandHandler> _logger;

    public ResetPasswordCommandHandler(
        IUserRepository userRepository,
        IPasswordService passwordService,
        ILogger<ResetPasswordCommandHandler> logger
    )
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _logger = logger;
    }

    public async Task<Result> Handle(
        ResetPasswordCommand request,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation("Processing password reset request");

        // Find user by reset token
        var user = await _userRepository.GetByPasswordResetTokenAsync(
            request.Token,
            cancellationToken
        );

        if (user == null)
        {
            _logger.LogWarning("Invalid or expired password reset token");
            return Result.Failure("Invalid or expired password reset token");
        }

        // Validate token
        if (!user.IsPasswordResetTokenValid(request.Token))
        {
            _logger.LogWarning("Password reset token validation failed for user {UserId}", user.Id);
            return Result.Failure("Invalid or expired password reset token");
        }

        // Update password
        var passwordResult = await _passwordService.UpdatePasswordAsync(
            user.CognitoId.Value,
            request.NewPassword,
            cancellationToken
        );

        if (passwordResult.IsFailure)
        {
            _logger.LogError(
                "Failed to update password for user {UserId}: {Error}",
                user.Id,
                passwordResult.Error
            );
            return Result.Failure(passwordResult.Error);
        }

        _logger.LogInformation("Password updated successfully for user {UserId}", user.Id);

        // Clear the reset token
        user.ClearPasswordResetToken();
        await _userRepository.UpdateAsync(user, cancellationToken);

        _logger.LogInformation("Password reset successfully for user {UserId}", user.Id);

        return Result.Success();
    }
}
