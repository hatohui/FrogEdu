using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using FrogEdu.Shared.Kernel;
using FrogEdu.User.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FrogEdu.User.Infrastructure.Services;

/// <summary>
/// Service for managing Cognito user attributes
/// </summary>
public sealed class CognitoAttributeService : ICognitoAttributeService
{
    private readonly IAmazonCognitoIdentityProvider? _cognitoClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<CognitoAttributeService> _logger;
    private readonly bool _isConfigured;

    public CognitoAttributeService(
        IConfiguration configuration,
        ILogger<CognitoAttributeService> logger,
        IAmazonCognitoIdentityProvider? cognitoClient = null
    )
    {
        _configuration = configuration;
        _logger = logger;
        _cognitoClient = cognitoClient;

        // Check if Cognito is properly configured
        var userPoolId = configuration["AWS:Cognito:UserPoolId"];
        var accessKeyId = configuration["AWS:Cognito:AccessKeyId"];
        var secretAccessKey = configuration["AWS:Cognito:SecretAccessKey"];

        _isConfigured =
            !string.IsNullOrWhiteSpace(userPoolId)
            && !string.IsNullOrWhiteSpace(accessKeyId)
            && !string.IsNullOrWhiteSpace(secretAccessKey)
            && cognitoClient != null;
    }

    public async Task<Result> SyncRoleAttributeAsync(
        string cognitoId,
        string roleName,
        CancellationToken cancellationToken = default
    )
    {
        // Early check: if Cognito is not configured, skip sync gracefully
        if (!_isConfigured)
        {
            _logger.LogWarning(
                "Cognito is not configured - skipping role sync for user {CognitoId}. Configure AWS Cognito credentials to enable this feature.",
                cognitoId
            );
            return Result.Failure("Cognito service is not available");
        }

        try
        {
            var userPoolId = _configuration["AWS:Cognito:UserPoolId"]!;

            var updateRequest = new AdminUpdateUserAttributesRequest
            {
                UserPoolId = userPoolId,
                Username = cognitoId,
                UserAttributes = new List<AttributeType>
                {
                    new AttributeType { Name = "custom:role", Value = roleName.ToLower() },
                },
            };

            await _cognitoClient!.AdminUpdateUserAttributesAsync(updateRequest, cancellationToken);

            _logger.LogInformation(
                "Successfully synced custom:role attribute to '{Role}' for Cognito user {CognitoId}",
                roleName,
                cognitoId
            );

            return Result.Success();
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("Cognito is not configured"))
        {
            _logger.LogWarning(
                "Cognito is not configured - skipping role sync for user {CognitoId}. Configure AWS Cognito credentials to enable this feature.",
                cognitoId
            );
            return Result.Failure("Cognito service is not available");
        }
        catch (UserNotFoundException ex)
        {
            _logger.LogWarning(
                ex,
                "User not found in Cognito when syncing role for {CognitoId}",
                cognitoId
            );
            return Result.Failure($"User {cognitoId} not found in Cognito");
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to sync custom:role attribute for Cognito user {CognitoId}",
                cognitoId
            );
            return Result.Failure("Failed to sync role attribute to Cognito");
        }
    }
}
