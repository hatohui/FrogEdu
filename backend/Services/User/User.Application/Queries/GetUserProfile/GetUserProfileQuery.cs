using FrogEdu.User.Application.DTOs;
using MediatR;

namespace FrogEdu.User.Application.Queries.GetUserProfile;

/// <summary>
/// Query to get user profile by Cognito ID
/// </summary>
public sealed record GetUserProfileQuery(string CognitoId) : IRequest<UserDto?>;
