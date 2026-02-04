using System.Security.Claims;
using FrogEdu.Subscription.API.Attributes;
using FrogEdu.Subscription.Application.Commands.ActivateSubscription;
using FrogEdu.Subscription.Application.Commands.ActivateSubscriptionTier;
using FrogEdu.Subscription.Application.Commands.CancelSubscription;
using FrogEdu.Subscription.Application.Commands.CreateSubscriptionTier;
using FrogEdu.Subscription.Application.Commands.DeactivateSubscriptionTier;
using FrogEdu.Subscription.Application.Commands.DeleteSubscription;
using FrogEdu.Subscription.Application.Commands.DeleteSubscriptionTier;
using FrogEdu.Subscription.Application.Commands.RenewSubscription;
using FrogEdu.Subscription.Application.Commands.SubscribeToPro;
using FrogEdu.Subscription.Application.Commands.SuspendSubscription;
using FrogEdu.Subscription.Application.Commands.UpdateSubscriptionTier;
using FrogEdu.Subscription.Application.Commands.UpdateTransactionStatus;
using FrogEdu.Subscription.Application.DTOs;
using FrogEdu.Subscription.Application.Queries.GetAllSubscriptions;
using FrogEdu.Subscription.Application.Queries.GetAllSubscriptionTiers;
using FrogEdu.Subscription.Application.Queries.GetAllTransactions;
using FrogEdu.Subscription.Application.Queries.GetSubscriptionById;
using FrogEdu.Subscription.Application.Queries.GetSubscriptionClaims;
using FrogEdu.Subscription.Application.Queries.GetSubscriptionTierById;
using FrogEdu.Subscription.Application.Queries.GetSubscriptionTiers;
using FrogEdu.Subscription.Application.Queries.GetTransactionById;
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
                    SubscriptionTierId = Guid.Empty,
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

    // ==================== ADMIN ENDPOINTS ====================

    #region Subscription Tier Management (Admin)

    /// <summary>
    /// Get all subscription tiers including inactive (Admin only)
    /// </summary>
    [HttpGet("admin/tiers")]
    [AuthorizeAdmin]
    [ProducesResponseType(typeof(IReadOnlyList<SubscriptionTierDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAllTiers(
        [FromQuery] bool includeInactive = true,
        CancellationToken cancellationToken = default
    )
    {
        var query = new GetAllSubscriptionTiersQuery(includeInactive);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get subscription tier by ID (Admin only)
    /// </summary>
    [HttpGet("admin/tiers/{id:guid}")]
    [AuthorizeAdmin]
    [ProducesResponseType(typeof(SubscriptionTierDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetTierById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetSubscriptionTierByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        if (result is null)
            return NotFound(new { error = "Subscription tier not found" });

        return Ok(result);
    }

    /// <summary>
    /// Create a new subscription tier (Admin only)
    /// </summary>
    [HttpPost("admin/tiers")]
    [AuthorizeAdmin]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateTier(
        [FromBody] CreateSubscriptionTierDto dto,
        CancellationToken cancellationToken
    )
    {
        var command = new CreateSubscriptionTierCommand(
            dto.Name,
            dto.Description,
            dto.Price,
            dto.Currency,
            dto.DurationInDays,
            dto.TargetRole,
            dto.ImageUrl
        );

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(new { error = result.Error });
        }

        return CreatedAtAction(
            nameof(GetTierById),
            new { id = result.Value },
            new { id = result.Value }
        );
    }

    /// <summary>
    /// Update an existing subscription tier (Admin only)
    /// </summary>
    [HttpPut("admin/tiers/{id:guid}")]
    [AuthorizeAdmin]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateTier(
        Guid id,
        [FromBody] UpdateSubscriptionTierDto dto,
        CancellationToken cancellationToken
    )
    {
        var command = new UpdateSubscriptionTierCommand(
            id,
            dto.Name,
            dto.Description,
            dto.Price,
            dto.Currency,
            dto.DurationInDays,
            dto.ImageUrl
        );

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(new { error = result.Error });
        }

        return NoContent();
    }

    /// <summary>
    /// Delete a subscription tier (Admin only)
    /// </summary>
    [HttpDelete("admin/tiers/{id:guid}")]
    [AuthorizeAdmin]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteTier(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteSubscriptionTierCommand(id);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(new { error = result.Error });
        }

        return NoContent();
    }

    /// <summary>
    /// Activate a subscription tier (Admin only)
    /// </summary>
    [HttpPost("admin/tiers/{id:guid}/activate")]
    [AuthorizeAdmin]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ActivateTier(Guid id, CancellationToken cancellationToken)
    {
        var command = new ActivateSubscriptionTierCommand(id);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(new { error = result.Error });
        }

        return NoContent();
    }

    /// <summary>
    /// Deactivate a subscription tier (Admin only)
    /// </summary>
    [HttpPost("admin/tiers/{id:guid}/deactivate")]
    [AuthorizeAdmin]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeactivateTier(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeactivateSubscriptionTierCommand(id);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(new { error = result.Error });
        }

        return NoContent();
    }

    #endregion

    #region User Subscription Management (Admin)

    /// <summary>
    /// Get all user subscriptions (Admin only)
    /// </summary>
    [HttpGet("admin/subscriptions")]
    [AuthorizeAdmin]
    [ProducesResponseType(typeof(IReadOnlyList<UserSubscriptionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAllSubscriptions(
        [FromQuery] string? status = null,
        CancellationToken cancellationToken = default
    )
    {
        var query = new GetAllSubscriptionsQuery(status);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get user subscription by ID (Admin only)
    /// </summary>
    [HttpGet("admin/subscriptions/{id:guid}")]
    [AuthorizeAdmin]
    [ProducesResponseType(typeof(UserSubscriptionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetSubscriptionById(
        Guid id,
        CancellationToken cancellationToken
    )
    {
        var query = new GetSubscriptionByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        if (result is null)
            return NotFound(new { error = "Subscription not found" });

        return Ok(result);
    }

    /// <summary>
    /// Suspend a user subscription (Admin only)
    /// </summary>
    [HttpPost("admin/subscriptions/{id:guid}/suspend")]
    [AuthorizeAdmin]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> SuspendSubscription(
        Guid id,
        CancellationToken cancellationToken
    )
    {
        var command = new SuspendSubscriptionCommand(id);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(new { error = result.Error });
        }

        return NoContent();
    }

    /// <summary>
    /// Activate a user subscription (Admin only)
    /// </summary>
    [HttpPost("admin/subscriptions/{id:guid}/activate")]
    [AuthorizeAdmin]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ActivateSubscription(
        Guid id,
        CancellationToken cancellationToken
    )
    {
        var command = new ActivateSubscriptionCommand(id);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(new { error = result.Error });
        }

        return NoContent();
    }

    /// <summary>
    /// Renew a user subscription (Admin only)
    /// </summary>
    [HttpPost("admin/subscriptions/{id:guid}/renew")]
    [AuthorizeAdmin]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> RenewSubscription(
        Guid id,
        [FromBody] RenewSubscriptionDto dto,
        CancellationToken cancellationToken
    )
    {
        var command = new RenewSubscriptionCommand(id, dto.NewEndDate);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(new { error = result.Error });
        }

        return NoContent();
    }

    /// <summary>
    /// Delete a user subscription (Admin only)
    /// </summary>
    [HttpDelete("admin/subscriptions/{id:guid}")]
    [AuthorizeAdmin]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteSubscription(
        Guid id,
        CancellationToken cancellationToken
    )
    {
        var command = new DeleteSubscriptionCommand(id);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(new { error = result.Error });
        }

        return NoContent();
    }

    #endregion

    #region Transaction Management (Admin)

    /// <summary>
    /// Get all transactions (Admin only)
    /// </summary>
    [HttpGet("admin/transactions")]
    [AuthorizeAdmin]
    [ProducesResponseType(typeof(IReadOnlyList<TransactionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAllTransactions(
        [FromQuery] string? paymentStatus = null,
        [FromQuery] string? paymentProvider = null,
        CancellationToken cancellationToken = default
    )
    {
        var query = new GetAllTransactionsQuery(paymentStatus, paymentProvider);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get transaction by ID (Admin only)
    /// </summary>
    [HttpGet("admin/transactions/{id:guid}")]
    [AuthorizeAdmin]
    [ProducesResponseType(typeof(TransactionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetTransactionById(
        Guid id,
        CancellationToken cancellationToken
    )
    {
        var query = new GetTransactionByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        if (result is null)
            return NotFound(new { error = "Transaction not found" });

        return Ok(result);
    }

    /// <summary>
    /// Update transaction payment status (Admin only)
    /// </summary>
    [HttpPut("admin/transactions/{id:guid}/status")]
    [AuthorizeAdmin]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateTransactionStatus(
        Guid id,
        [FromBody] UpdateTransactionStatusDto dto,
        CancellationToken cancellationToken
    )
    {
        var command = new UpdateTransactionStatusCommand(
            id,
            dto.PaymentStatus,
            dto.ProviderTransactionId
        );

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(new { error = result.Error });
        }

        return NoContent();
    }

    #endregion

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

// Response DTOs
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
