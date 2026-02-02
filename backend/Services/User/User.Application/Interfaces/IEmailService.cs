using FrogEdu.Shared.Kernel;

namespace FrogEdu.User.Application.Interfaces;

public interface IEmailService
{
    Task<Result> SendVerificationEmailAsync(
        string toEmail,
        string toName,
        string verificationToken,
        CancellationToken cancellationToken = default
    );

    Task<Result> SendPasswordResetEmailAsync(
        string toEmail,
        string toName,
        string resetToken,
        CancellationToken cancellationToken = default
    );
    Task<Result> SendWelcomeEmailAsync(
        string toEmail,
        string toName,
        CancellationToken cancellationToken = default
    );
}
