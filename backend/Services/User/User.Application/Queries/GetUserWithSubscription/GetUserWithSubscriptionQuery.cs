using FrogEdu.User.Application.DTOs;
using MediatR;

namespace FrogEdu.User.Application.Queries.GetUserWithSubscription;

/// <summary>
/// Query to get user profile with subscription information by Cognito ID
/// </summary>
public sealed record GetUserWithSubscriptionQuery(string CognitoId)
    : IRequest<UserWithSubscriptionDto?>;
