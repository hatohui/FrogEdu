using FrogEdu.Class.Application.Dtos.responses;
using MediatR;

namespace FrogEdu.Class.Application.Queries.GetBadges;

public sealed record GetBadgesQuery(bool ActiveOnly = true) : IRequest<IReadOnlyList<BadgeDto>>;
