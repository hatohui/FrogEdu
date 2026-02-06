using System.Security.Claims;
using FrogEdu.User.Application.Queries.GetAssetUploadUrl;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrogEdu.User.API.Controllers;

[ApiController]
[Route("assets")]
[Tags("Assets")]
public class AssetsController(IMediator mediator, ILogger<AssetsController> logger) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<AssetsController> _logger = logger;

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

    [HttpDelete("{assetId:guid}")]
    public async Task<IActionResult> DeleteAsset(Guid assetId, CancellationToken cancellationToken)
    {
        return Ok(new { message = "DeleteAsset endpoint is not yet implemented." });
    }
}
