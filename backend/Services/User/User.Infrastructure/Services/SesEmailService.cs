using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using FrogEdu.Shared.Kernel;
using FrogEdu.User.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FrogEdu.User.Infrastructure.Services;

public class SesEmailService : IEmailService
{
    private readonly IAmazonSimpleEmailService _sesClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<SesEmailService> _logger;
    private readonly string _fromEmail;
    private readonly string _fromName;
    private readonly string _frontendBaseUrl;

    public SesEmailService(
        IAmazonSimpleEmailService sesClient,
        IConfiguration configuration,
        ILogger<SesEmailService> logger
    )
    {
        _sesClient = sesClient;
        _configuration = configuration;
        _logger = logger;
        _fromEmail = configuration["AWS:SES:FromEmail"] ?? "notifications@frogedu.org";
        _fromName = configuration["AWS:SES:FromName"] ?? "FrogEdu";
        _frontendBaseUrl = configuration["Frontend:BaseUrl"] ?? "https://www.frogedu.org";
    }

    public async Task<Result> SendVerificationEmailAsync(
        string toEmail,
        string toName,
        string verificationToken,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var verificationLink = $"{_frontendBaseUrl}/verify-email?token={verificationToken}";
            var htmlBody = await LoadTemplateAsync("EmailVerification.html");

            htmlBody = htmlBody
                .Replace("{{UserName}}", toName)
                .Replace("{{VerificationLink}}", verificationLink);

            var request = new SendEmailRequest
            {
                Source = $"{_fromName} <{_fromEmail}>",
                Destination = new Destination { ToAddresses = [toEmail] },
                Message = new Message
                {
                    Subject = new Content("Verify Your Email - FrogEdu"),
                    Body = new Body
                    {
                        Html = new Content { Charset = "UTF-8", Data = htmlBody },
                    },
                },
            };

            var response = await _sesClient.SendEmailAsync(request, cancellationToken);

            _logger.LogInformation(
                "Verification email sent to {Email}. MessageId: {MessageId}",
                toEmail,
                response.MessageId
            );

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send verification email to {Email}", toEmail);
            return Result.Failure("Failed to send verification email");
        }
    }

    public async Task<Result> SendPasswordResetEmailAsync(
        string toEmail,
        string toName,
        string resetToken,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var resetLink = $"{_frontendBaseUrl}/reset-password?token={resetToken}";
            var htmlBody = await LoadTemplateAsync("PasswordReset.html");

            htmlBody = htmlBody.Replace("{{UserName}}", toName).Replace("{{ResetLink}}", resetLink);

            var request = new SendEmailRequest
            {
                Source = $"{_fromName} <{_fromEmail}>",
                Destination = new Destination { ToAddresses = new List<string> { toEmail } },
                Message = new Message
                {
                    Subject = new Content("Reset Your Password - FrogEdu"),
                    Body = new Body
                    {
                        Html = new Content { Charset = "UTF-8", Data = htmlBody },
                    },
                },
            };

            var response = await _sesClient.SendEmailAsync(request, cancellationToken);

            _logger.LogInformation(
                "Password reset email sent to {Email}. MessageId: {MessageId}",
                toEmail,
                response.MessageId
            );

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send password reset email to {Email}", toEmail);
            return Result.Failure("Failed to send password reset email");
        }
    }

    public async Task<Result> SendWelcomeEmailAsync(
        string toEmail,
        string toName,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var appLink = $"{_frontendBaseUrl}/app";
            var helpLink = $"{_frontendBaseUrl}/help";
            var htmlBody = await LoadTemplateAsync("Welcome.html");

            htmlBody = htmlBody
                .Replace("{{UserName}}", toName)
                .Replace("{{AppLink}}", appLink)
                .Replace("{{HelpLink}}", helpLink);

            var request = new SendEmailRequest
            {
                Source = $"{_fromName} <{_fromEmail}>",
                Destination = new Destination { ToAddresses = new List<string> { toEmail } },
                Message = new Message
                {
                    Subject = new Content("Welcome to FrogEdu!"),
                    Body = new Body
                    {
                        Html = new Content { Charset = "UTF-8", Data = htmlBody },
                    },
                },
            };

            var response = await _sesClient.SendEmailAsync(request, cancellationToken);

            _logger.LogInformation(
                "Welcome email sent to {Email}. MessageId: {MessageId}",
                toEmail,
                response.MessageId
            );

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send welcome email to {Email}", toEmail);
            return Result.Failure("Failed to send welcome email");
        }
    }

    private async Task<string> LoadTemplateAsync(string templateName)
    {
        var templatePath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Templates",
            templateName
        );

        if (!File.Exists(templatePath))
        {
            _logger.LogError("Email template not found: {TemplatePath}", templatePath);
            throw new FileNotFoundException($"Email template not found: {templateName}");
        }

        return await File.ReadAllTextAsync(templatePath);
    }
}
