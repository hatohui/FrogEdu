using System.Security.Claims;
using FrogEdu.User.Application.Queries.GetAssetUploadUrl;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrogEdu.User.API.Controllers;

/// <summary>
/// Asset management endpoints for file uploads to R2
/// </summary>
[ApiController]
[Route("assets")]
[Tags("Assets")]
public class AssetsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AssetsController> _logger;

    public AssetsController(IMediator mediator, ILogger<AssetsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get a presigned URL for uploading an asset to R2
    /// </summary>
    /// <param name="folder">The folder to upload to (e.g., "avatars", "documents")</param>
    /// <param name="contentType">Optional content type (e.g., "image/png")</param>
    [HttpGet("sign-url")]
    [Authorize]
    [ProducesResponseType(typeof(AssetUploadUrlResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetSignedUploadUrl(
        [FromQuery] string folder,
        [FromQuery] string? contentType = null,
        CancellationToken cancellationToken = default
    )
    {
        if (string.IsNullOrWhiteSpace(folder))
        {
            return BadRequest("Folder parameter is required");
        }

        var cognitoId =
            User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (string.IsNullOrEmpty(cognitoId))
        {
            return Unauthorized();
        }

        var query = new GetAssetUploadUrlQuery(cognitoId, folder, contentType);
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning("Failed to generate signed URL: {Error}", result.Error);
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }
}
