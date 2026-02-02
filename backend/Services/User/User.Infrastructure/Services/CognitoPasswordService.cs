using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using FrogEdu.Shared.Kernel;
using FrogEdu.User.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FrogEdu.User.Infrastructure.Services;

public sealed class CognitoPasswordService : IPasswordService
{
    private readonly IAmazonCognitoIdentityProvider _cognitoClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<CognitoPasswordService> _logger;

    public CognitoPasswordService(
        IAmazonCognitoIdentityProvider cognitoClient,
        IConfiguration configuration,
        ILogger<CognitoPasswordService> logger
    )
    {
        _cognitoClient = cognitoClient;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<Result> UpdatePasswordAsync(
        string cognitoId,
        string newPassword,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var userPoolId = _configuration["AWS:Cognito:UserPoolId"];

            if (string.IsNullOrWhiteSpace(userPoolId))
            {
                _logger.LogError("AWS Cognito UserPoolId is not configured");
                return Result.Failure("Password service is not properly configured");
            }

            var setPasswordRequest = new AdminSetUserPasswordRequest
            {
                UserPoolId = userPoolId,
                Username = cognitoId,
                Password = newPassword,
                Permanent = true,
            };

            await _cognitoClient.AdminSetUserPasswordAsync(setPasswordRequest, cancellationToken);

            _logger.LogInformation(
                "Password updated successfully for Cognito user {CognitoId}",
                cognitoId
            );
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to update password for Cognito user {CognitoId}",
                cognitoId
            );
            return Result.Failure("Failed to update password");
        }
    }
}
