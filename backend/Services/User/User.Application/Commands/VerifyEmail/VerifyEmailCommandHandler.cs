using FrogEdu.Shared.Kernel;
using FrogEdu.User.Application.Interfaces;
using FrogEdu.User.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FrogEdu.User.Application.Commands.VerifyEmail;

public sealed class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    private readonly ILogger<VerifyEmailCommandHandler> _logger;

    public VerifyEmailCommandHandler(
        IUserRepository userRepository,
        IEmailService emailService,
        ILogger<VerifyEmailCommandHandler> logger
    )
    {
        _userRepository = userRepository;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<Result> Handle(
        VerifyEmailCommand request,
        CancellationToken cancellationToken
    )
    {
        if (string.IsNullOrWhiteSpace(request.Token))
        {
            _logger.LogWarning("Empty verification token received");
            return Result.Failure("Invalid verification token");
        }

        var user = await _userRepository.GetByVerificationTokenAsync(
            request.Token,
            cancellationToken
        );

        if (user is null)
        {
            _logger.LogWarning("User with token not found");
            return Result.Failure("Invalid or expired verification token");
        }

        if (!user.IsEmailVerificationTokenValid(request.Token))
        {
            _logger.LogWarning("Invalid or expired token for user {UserId}", user.Id);
            return Result.Failure("Verification token has expired");
        }

        user.VerifyEmail();

        await _userRepository.UpdateAsync(user, cancellationToken);

        var emailResult = await _emailService.SendWelcomeEmailAsync(
            user.Email.Value,
            $"{user.FirstName} {user.LastName}",
            cancellationToken
        );

        if (emailResult.IsFailure)
        {
            _logger.LogWarning(
                "Email verified but failed to send welcome email to user {UserId}: {Error}",
                user.Id,
                emailResult.Error
            );
        }

        _logger.LogInformation("Email verified successfully for user {UserId}", user.Id);

        return Result.Success();
    }
}
