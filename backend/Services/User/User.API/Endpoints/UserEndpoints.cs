using System.Security.Claims;
using FrogEdu.User.Application.Commands.CreateUser;
using FrogEdu.User.Application.Commands.UpdateAvatar;
using FrogEdu.User.Application.Commands.UpdateProfile;
using FrogEdu.User.Application.DTOs;
using FrogEdu.User.Application.Queries.GetAvatarUploadUrl;
using FrogEdu.User.Application.Queries.GetUserProfile;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FrogEdu.User.API.Endpoints;

/// <summary>
/// User API endpoints
/// </summary>
public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/users").WithTags("Users").WithOpenApi();

        // GET /api/users/me - Get current user profile
        group
            .MapGet("/me", GetCurrentUser)
            .WithName("GetCurrentUser")
            .WithDescription("Get the current authenticated user's profile")
            .Produces<UserDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization();

        // PUT /api/users/me - Update current user profile
        group
            .MapPut("/me", UpdateCurrentUser)
            .WithName("UpdateCurrentUser")
            .WithDescription("Update the current authenticated user's profile")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization();

        // POST /api/users/me/avatar - Get presigned URL for avatar upload
        group
            .MapPost("/me/avatar", GetAvatarUploadUrl)
            .WithName("GetAvatarUploadUrl")
            .WithDescription("Get a presigned URL for uploading a new avatar")
            .Produces<AvatarPresignedUrlDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization();

        // PUT /api/users/me/avatar - Confirm avatar upload
        group
            .MapPut("/me/avatar", ConfirmAvatarUpload)
            .WithName("ConfirmAvatarUpload")
            .WithDescription("Confirm avatar upload and update user profile")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization();
    }

    private static async Task<IResult> GetCurrentUser(
        ClaimsPrincipal user,
        IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        var cognitoId = GetCognitoId(user);
        if (string.IsNullOrEmpty(cognitoId))
            return Results.Unauthorized();

        var query = new GetUserProfileQuery(cognitoId);
        var result = await mediator.Send(query, cancellationToken);

        return result is null ? Results.NotFound("User not found") : Results.Ok(result);
    }

    private static async Task<IResult> UpdateCurrentUser(
        ClaimsPrincipal user,
        [FromBody] UpdateProfileDto dto,
        IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        var cognitoId = GetCognitoId(user);
        if (string.IsNullOrEmpty(cognitoId))
            return Results.Unauthorized();

        var command = new UpdateProfileCommand(cognitoId, dto.FirstName, dto.LastName);
        var result = await mediator.Send(command, cancellationToken);

        return result.IsFailure ? Results.BadRequest(result.Error) : Results.NoContent();
    }

    private static async Task<IResult> GetAvatarUploadUrl(
        ClaimsPrincipal user,
        [FromBody] AvatarUploadRequest request,
        IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        var cognitoId = GetCognitoId(user);
        if (string.IsNullOrEmpty(cognitoId))
            return Results.Unauthorized();

        var query = new GetAvatarUploadUrlQuery(cognitoId, request.ContentType);
        var result = await mediator.Send(query, cancellationToken);

        return result is null ? Results.NotFound("User not found") : Results.Ok(result);
    }

    private static async Task<IResult> ConfirmAvatarUpload(
        ClaimsPrincipal user,
        [FromBody] ConfirmAvatarRequest request,
        IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        var cognitoId = GetCognitoId(user);
        if (string.IsNullOrEmpty(cognitoId))
            return Results.Unauthorized();

        var command = new UpdateAvatarCommand(cognitoId, request.AvatarUrl);
        var result = await mediator.Send(command, cancellationToken);

        return result.IsFailure ? Results.BadRequest(result.Error) : Results.NoContent();
    }

    private static string? GetCognitoId(ClaimsPrincipal user)
    {
        // AWS Cognito uses "sub" claim for user ID
        return user.FindFirstValue(ClaimTypes.NameIdentifier) ?? user.FindFirstValue("sub");
    }
}

/// <summary>
/// Request model for avatar upload
/// </summary>
public record AvatarUploadRequest(string ContentType);

/// <summary>
/// Request model for confirming avatar upload
/// </summary>
public record ConfirmAvatarRequest(string AvatarUrl);
