using FrogEdu.Shared.Kernel;

namespace FrogEdu.User.Application.Interfaces;

public interface IEmailService
{
    /// <summary>
    /// Send a verification email to the user with a token
    /// </summary>
    /// <param name="toEmail">Recipient email address</param>
    /// <param name="toName">Recipient full name</param>
    /// <param name="verificationToken">Verification token to embed in email</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result indicating success or failure</returns>
    Task<Result> SendVerificationEmailAsync(
        string toEmail,
        string toName,
        string verificationToken,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Send a password reset email to the user with a reset link
    /// </summary>
    /// <param name="toEmail">Recipient email address</param>
    /// <param name="toName">Recipient full name</param>
    /// <param name="resetToken">Password reset token</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result indicating success or failure</returns>
    Task<Result> SendPasswordResetEmailAsync(
        string toEmail,
        string toName,
        string resetToken,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Send a welcome email to the user after successful registration
    /// </summary>
    /// <param name="toEmail">Recipient email address</param>
    /// <param name="toName">Recipient full name</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result indicating success or failure</returns>
    Task<Result> SendWelcomeEmailAsync(
        string toEmail,
        string toName,
        CancellationToken cancellationToken = default
    );
}
