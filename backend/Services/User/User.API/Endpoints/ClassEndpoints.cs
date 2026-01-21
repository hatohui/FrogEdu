using System.Security.Claims;
using FrogEdu.User.Application.Commands.CreateClass;
using FrogEdu.User.Application.Commands.JoinClass;
using FrogEdu.User.Application.Commands.RegenerateInviteCode;
using FrogEdu.User.Application.DTOs;
using FrogEdu.User.Application.Queries.GetClassDetails;
using FrogEdu.User.Application.Queries.GetDashboardStats;
using FrogEdu.User.Application.Queries.GetStudentClasses;
using FrogEdu.User.Application.Queries.GetTeacherClasses;
using FrogEdu.User.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FrogEdu.User.API.Endpoints;

/// <summary>
/// Class management API endpoints
/// </summary>
public static class ClassEndpoints
{
    public static void MapClassEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/classes").WithTags("Classes").WithOpenApi();

        // GET /api/classes - Get all classes for current user (teacher or student)
        group
            .MapGet("/", GetMyClasses)
            .WithName("GetMyClasses")
            .WithDescription("Get all classes for the current authenticated user")
            .Produces<IReadOnlyList<ClassDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .RequireAuthorization();

        // GET /api/classes/{id} - Get class details
        group
            .MapGet("/{id:guid}", GetClassDetails)
            .WithName("GetClassDetails")
            .WithDescription("Get detailed information about a specific class including roster")
            .Produces<ClassDetailsDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization();

        // POST /api/classes - Create a new class (Teacher only)
        group
            .MapPost("/", CreateClass)
            .WithName("CreateClass")
            .WithDescription("Create a new class (Teacher only)")
            .Produces<CreateClassResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .RequireAuthorization("TeacherOnly");

        // POST /api/classes/join - Join a class via invite code (Student only)
        group
            .MapPost("/join", JoinClass)
            .WithName("JoinClass")
            .WithDescription("Join a class using an invite code (Student only)")
            .Produces<JoinClassResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .RequireAuthorization("StudentOnly");

        // POST /api/classes/{id}/regenerate-code - Regenerate invite code (Teacher only)
        group
            .MapPost("/{id:guid}/regenerate-code", RegenerateInviteCode)
            .WithName("RegenerateInviteCode")
            .WithDescription("Regenerate the invite code for a class (Teacher only)")
            .Produces<RegenerateCodeResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization("TeacherOnly");

        // GET /api/classes/dashboard/stats - Get dashboard statistics
        group
            .MapGet("/dashboard/stats", GetDashboardStats)
            .WithName("GetDashboardStats")
            .WithDescription("Get dashboard statistics for the current user")
            .Produces<DashboardStatsDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .RequireAuthorization();
    }

    private static async Task<IResult> GetMyClasses(
        ClaimsPrincipal user,
        IMediator mediator,
        IUserRepository userRepository,
        [FromQuery] bool includeArchived = false,
        CancellationToken cancellationToken = default
    )
    {
        var userId = await GetUserIdAsync(user, userRepository, cancellationToken);
        if (userId is null)
            return Results.Unauthorized();

        var isTeacher = IsTeacher(user);

        if (isTeacher)
        {
            var query = new GetTeacherClassesQuery(userId.Value, includeArchived);
            var result = await mediator.Send(query, cancellationToken);
            return Results.Ok(result);
        }
        else
        {
            var query = new GetStudentClassesQuery(userId.Value);
            var result = await mediator.Send(query, cancellationToken);
            return Results.Ok(result);
        }
    }

    private static async Task<IResult> GetClassDetails(
        Guid id,
        ClaimsPrincipal user,
        IMediator mediator,
        IUserRepository userRepository,
        CancellationToken cancellationToken = default
    )
    {
        var userId = await GetUserIdAsync(user, userRepository, cancellationToken);
        if (userId is null)
            return Results.Unauthorized();

        var query = new GetClassDetailsQuery(id, userId.Value);
        var result = await mediator.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error?.Contains("access") == true
                ? Results.Forbid()
                : Results.NotFound(result.Error);
        }

        return Results.Ok(result.Value);
    }

    private static async Task<IResult> CreateClass(
        [FromBody] CreateClassDto dto,
        ClaimsPrincipal user,
        IMediator mediator,
        IUserRepository userRepository,
        CancellationToken cancellationToken = default
    )
    {
        var userId = await GetUserIdAsync(user, userRepository, cancellationToken);
        if (userId is null)
            return Results.Unauthorized();

        var command = new CreateClassCommand(
            dto.Name,
            dto.Subject,
            dto.Grade,
            userId.Value,
            dto.School,
            dto.Description,
            dto.MaxStudents
        );

        var result = await mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.Created(
            $"/api/classes/{result.Value}",
            new CreateClassResponse(result.Value)
        );
    }

    private static async Task<IResult> JoinClass(
        [FromBody] JoinClassDto dto,
        ClaimsPrincipal user,
        IMediator mediator,
        IUserRepository userRepository,
        CancellationToken cancellationToken = default
    )
    {
        var userId = await GetUserIdAsync(user, userRepository, cancellationToken);
        if (userId is null)
            return Results.Unauthorized();

        var command = new JoinClassCommand(dto.InviteCode, userId.Value);
        var result = await mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.Ok(new JoinClassResponse(result.Value));
    }

    private static async Task<IResult> RegenerateInviteCode(
        Guid id,
        ClaimsPrincipal user,
        IMediator mediator,
        IUserRepository userRepository,
        [FromQuery] int expiresInDays = 7,
        CancellationToken cancellationToken = default
    )
    {
        var userId = await GetUserIdAsync(user, userRepository, cancellationToken);
        if (userId is null)
            return Results.Unauthorized();

        var command = new RegenerateInviteCodeCommand(id, userId.Value, expiresInDays);
        var result = await mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error?.Contains("authorized") == true
                ? Results.Forbid()
                : Results.NotFound(result.Error);
        }

        return Results.Ok(new RegenerateCodeResponse(result.Value!));
    }

    private static async Task<IResult> GetDashboardStats(
        ClaimsPrincipal user,
        IMediator mediator,
        IUserRepository userRepository,
        CancellationToken cancellationToken = default
    )
    {
        var userId = await GetUserIdAsync(user, userRepository, cancellationToken);
        if (userId is null)
            return Results.Unauthorized();

        var isTeacher = IsTeacher(user);
        var query = new GetDashboardStatsQuery(userId.Value, isTeacher);
        var result = await mediator.Send(query, cancellationToken);

        return Results.Ok(result);
    }

    private static async Task<Guid?> GetUserIdAsync(
        ClaimsPrincipal user,
        IUserRepository userRepository,
        CancellationToken cancellationToken
    )
    {
        var cognitoId =
            user.FindFirstValue(ClaimTypes.NameIdentifier) ?? user.FindFirstValue("sub");
        if (string.IsNullOrEmpty(cognitoId))
            return null;

        var userEntity = await userRepository.GetByCognitoIdAsync(cognitoId, cancellationToken);
        return userEntity?.Id;
    }

    private static bool IsTeacher(ClaimsPrincipal user)
    {
        // Check role claim
        var role = user.FindFirstValue(ClaimTypes.Role) ?? user.FindFirstValue("custom:role");
        return role?.Equals("Teacher", StringComparison.OrdinalIgnoreCase) == true;
    }
}

// Response models
public record CreateClassResponse(Guid ClassId);

public record JoinClassResponse(Guid ClassId);

public record RegenerateCodeResponse(string InviteCode);
