using FrogEdu.Class.Application.Dtos.responses;
using FrogEdu.Class.Domain.Repositories;
using MediatR;

namespace FrogEdu.Class.Application.Queries.GetBadges;

public sealed class GetBadgesQueryHandler(IBadgeRepository badgeRepository)
    : IRequestHandler<GetBadgesQuery, IReadOnlyList<BadgeDto>>
{
    private readonly IBadgeRepository _badgeRepository = badgeRepository;

    public async Task<IReadOnlyList<BadgeDto>> Handle(
        GetBadgesQuery request,
        CancellationToken cancellationToken
    )
    {
        var badges = request.ActiveOnly
            ? await _badgeRepository.GetAllActiveAsync(cancellationToken)
            : await _badgeRepository.GetAllAsync(cancellationToken);

        return badges
            .Select(b => new BadgeDto(
                b.Id,
                b.Name,
                b.Description,
                b.IconUrl,
                b.BadgeType,
                b.RequiredScore,
                b.IsActive
            ))
            .ToList()
            .AsReadOnly();
    }
}
