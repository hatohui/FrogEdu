using FrogEdu.User.Application.DTOs;
using MediatR;

namespace FrogEdu.User.Application.Queries.GetAvatarUploadUrl;

/// <summary>
/// Query to get a presigned URL for avatar upload
/// </summary>
public sealed record GetAvatarUploadUrlQuery(string CognitoId, string ContentType)
    : IRequest<AvatarPresignedUrlDto?>;
