using System.Security.Claims;
using FrogEdu.Subscription.Application.Commands.CancelSubscription;
using FrogEdu.Subscription.Application.Commands.SubscribeToPro;
using FrogEdu.Subscription.Application.DTOs;
using FrogEdu.Subscription.Application.Queries.GetSubscriptionClaims;
using FrogEdu.Subscription.Application.Queries.GetSubscriptionTiers;
using FrogEdu.Subscription.Application.Queries.GetUserSubscription;
using FrogEdu.Subscription.Application.Queries.GetUserTransactions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrogEdu.Subscription.API.Controllers;

[ApiController]
[Route("")]
[Tags("Subscriptions")]
public class SubscriptionController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<SubscriptionController> _logger;

    public SubscriptionController(IMediator mediator, ILogger<SubscriptionController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all available subscription tiers
    /// </summary>
    [HttpGet("tiers")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IReadOnlyList<SubscriptionTierDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSubscriptionTiers(CancellationToken cancellationToken)
    {
        var query = new GetSubscriptionTiersQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get current user's subscription
    /// </summary>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(UserSubscriptionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetMySubscription(CancellationToken cancellationToken)
    {
        var userId = GetUserIdFromClaims();
        if (userId is null)
            return Unauthorized();

        var query = new GetUserSubscriptionQuery(userId.Value);
        var result = await _mediator.Send(query, cancellationToken);

        if (result is null)
        {
            return Ok(
                new UserSubscriptionDto
                {
                    UserId = userId.Value,
                    PlanName = "Free",
                    Status = "None",
                    IsActive = false,
                    IsExpired = true,
                    ExpiresAtTimestamp = 0,
                }
            );
        }

        return Ok(result);
    }

    /// <summary>
    /// Get current user's transaction history
    /// </summary>
    [HttpGet("transactions/me")]
    [Authorize]
    [ProducesResponseType(typeof(IReadOnlyList<TransactionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetMyTransactions(CancellationToken cancellationToken)
    {
        var userId = GetUserIdFromClaims();
        if (userId is null)
            return Unauthorized();

        var query = new GetUserTransactionsQuery(userId.Value);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get subscription claims for a user (internal API for User Service)
    /// </summary>
    [HttpGet("claims/{userId:guid}")]
    [AllowAnonymous] // Internal API - should be secured via API key or service mesh in production
    [ProducesResponseType(typeof(SubscriptionClaimsDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSubscriptionClaims(
        Guid userId,
        CancellationToken cancellationToken
    )
    {
        var query = new GetSubscriptionClaimsQuery(userId);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Subscribe to Pro tier (mock - no payment required)
    /// </summary>
    [HttpPost("subscribe/pro")]
    [Authorize]
    [ProducesResponseType(typeof(SubscribeToProResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> SubscribeToPro(CancellationToken cancellationToken)
    {
        var userId = GetUserIdFromClaims();
        if (userId is null)
            return Unauthorized();

        var command = new SubscribeToProCommand(userId.Value);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(new { error = result.Error });
        }

        _logger.LogInformation(
            "User {UserId} subscribed to Pro tier. Subscription ID: {SubscriptionId}",
            userId,
            result.Value
        );

        return Ok(
            new SubscribeToProResponse
            {
                SubscriptionId = result.Value,
                Message = "Successfully subscribed to Pro tier!",
            }
        );
    }

    /// <summary>
    /// Cancel user's active subscription
    /// </summary>
    [HttpPost("cancel")]
    [Authorize]
    [ProducesResponseType(typeof(CancelSubscriptionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CancelSubscription(CancellationToken cancellationToken)
    {
        var userId = GetUserIdFromClaims();
        if (userId is null)
            return Unauthorized();

        var command = new CancelSubscriptionCommand(userId.Value);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(new { error = result.Error });
        }

        _logger.LogInformation(
            "User {UserId} cancelled subscription. Subscription ID: {SubscriptionId}",
            userId,
            result.Value
        );

        return Ok(
            new CancelSubscriptionResponse
            {
                SubscriptionId = result.Value,
                Message =
                    "Subscription cancelled successfully. Your access will continue until the end of your billing period.",
            }
        );
    }

    private Guid? GetUserIdFromClaims()
    {
        // First try to get the user ID from custom:userId claim
        var userIdClaim = User.FindFirstValue("custom:userId");
        if (!string.IsNullOrEmpty(userIdClaim) && Guid.TryParse(userIdClaim, out var userId))
        {
            return userId;
        }

        // Fallback to sub claim if it's a GUID
        var subClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (!string.IsNullOrEmpty(subClaim) && Guid.TryParse(subClaim, out var subId))
        {
            return subId;
        }

        return null;
    }
}

public sealed record SubscribeToProResponse
{
    public Guid SubscriptionId { get; init; }
    public string Message { get; init; } = null!;
}

public sealed record CancelSubscriptionResponse
{
    public Guid SubscriptionId { get; init; }
    public string Message { get; init; } = null!;
}
