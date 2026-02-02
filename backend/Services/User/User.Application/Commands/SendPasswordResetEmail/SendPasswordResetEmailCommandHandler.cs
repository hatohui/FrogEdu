using FrogEdu.Shared.Kernel;
using FrogEdu.User.Application.Interfaces;
using FrogEdu.User.Domain.Repositories;
using FrogEdu.User.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FrogEdu.User.Application.Commands.SendPasswordResetEmail;

public sealed class SendPasswordResetEmailCommandHandler
    : IRequestHandler<SendPasswordResetEmailCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    private readonly IApplicationConfiguration _configuration;
    private readonly ILogger<SendPasswordResetEmailCommandHandler> _logger;

    public SendPasswordResetEmailCommandHandler(
        IUserRepository userRepository,
        IEmailService emailService,
        IApplicationConfiguration configuration,
        ILogger<SendPasswordResetEmailCommandHandler> logger
    )
    {
        _userRepository = userRepository;
        _emailService = emailService;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<Result> Handle(
        SendPasswordResetEmailCommand request,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(
            "Processing password reset request for email: {Email}",
            request.Email
        );

        // Get user by email
        var emailValueObject = Email.Create(request.Email);
        var user = await _userRepository.GetByEmailAsync(emailValueObject.Value, cancellationToken);

        if (user == null)
        {
            // Don't reveal whether the email exists or not for security
            _logger.LogWarning(
                "Password reset requested for non-existent email: {Email}",
                request.Email
            );
            return Result.Success();
        }

        // Generate password reset token
        user.GeneratePasswordResetToken();
        await _userRepository.UpdateAsync(user, cancellationToken);

        // Build reset link
        var frontendBaseUrl = _configuration.GetFrontendBaseUrl();
        var resetLink =
            $"{frontendBaseUrl}/reset-password?token={Uri.EscapeDataString(user.PasswordResetToken!)}";

        // Send password reset email
        var emailResult = await _emailService.SendPasswordResetEmailAsync(
            user.Email.Value,
            user.FirstName,
            resetLink,
            cancellationToken
        );

        if (emailResult.IsFailure)
        {
            _logger.LogError(
                "Failed to send password reset email to {Email}: {Error}",
                user.Email.Value,
                emailResult.Error
            );
            return Result.Failure("Failed to send password reset email");
        }

        _logger.LogInformation(
            "Password reset email sent successfully to {Email}",
            user.Email.Value
        );

        return Result.Success();
    }
}
