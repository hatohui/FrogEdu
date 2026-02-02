using FrogEdu.Shared.Kernel;
using FrogEdu.User.Application.Interfaces;
using FrogEdu.User.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FrogEdu.User.Application.Commands.SendVerificationEmail;

public sealed class SendVerificationEmailCommandHandler
    : IRequestHandler<SendVerificationEmailCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    private readonly ILogger<SendVerificationEmailCommandHandler> _logger;

    public SendVerificationEmailCommandHandler(
        IUserRepository userRepository,
        IEmailService emailService,
        ILogger<SendVerificationEmailCommandHandler> logger
    )
    {
        _userRepository = userRepository;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<Result> Handle(
        SendVerificationEmailCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            _logger.LogWarning("User with ID {UserId} not found", request.UserId);
            return Result.Failure("User not found");
        }

        if (user.IsEmailVerified)
        {
            _logger.LogInformation("User {UserId} email already verified", request.UserId);
            return Result.Failure("Email is already verified");
        }

        // Generate new verification token
        user.GenerateEmailVerificationToken();

        await _userRepository.UpdateAsync(user, cancellationToken);

        // Send verification email
        var emailResult = await _emailService.SendVerificationEmailAsync(
            user.Email.Value,
            $"{user.FirstName} {user.LastName}",
            user.EmailVerificationToken!,
            cancellationToken
        );

        if (emailResult.IsFailure)
        {
            _logger.LogError(
                "Failed to send verification email to user {UserId}: {Error}",
                request.UserId,
                emailResult.Error
            );
            return emailResult;
        }

        _logger.LogInformation(
            "Verification email sent successfully to user {UserId}",
            request.UserId
        );

        return Result.Success();
    }
}
