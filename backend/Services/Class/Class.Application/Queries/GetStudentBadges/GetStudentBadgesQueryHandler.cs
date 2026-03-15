using FrogEdu.Class.Application.Dtos.responses;
using FrogEdu.Class.Domain.Repositories;
using MediatR;

namespace FrogEdu.Class.Application.Queries.GetStudentBadges;

public sealed class GetStudentBadgesQueryHandler(
    IStudentBadgeRepository studentBadgeRepository,
    IBadgeRepository badgeRepository
) : IRequestHandler<GetStudentBadgesQuery, IReadOnlyList<StudentBadgeDto>>
{
    private readonly IStudentBadgeRepository _studentBadgeRepository = studentBadgeRepository;
    private readonly IBadgeRepository _badgeRepository = badgeRepository;

    public async Task<IReadOnlyList<StudentBadgeDto>> Handle(
        GetStudentBadgesQuery request,
        CancellationToken cancellationToken
    )
    {
        var studentBadges = request.ClassId.HasValue
            ? await _studentBadgeRepository.GetByStudentAndClassAsync(
                request.StudentId,
                request.ClassId.Value,
                cancellationToken
            )
            : await _studentBadgeRepository.GetByStudentIdAsync(
                request.StudentId,
                cancellationToken
            );

        var allBadges = await _badgeRepository.GetAllAsync(cancellationToken);
        var badgeLookup = allBadges.ToDictionary(b => b.Id);

        return studentBadges
            .Select(sb =>
            {
                badgeLookup.TryGetValue(sb.BadgeId, out var badge);
                return new StudentBadgeDto(
                    sb.Id,
                    sb.StudentId,
                    sb.BadgeId,
                    sb.ClassId,
                    sb.ExamSessionId,
                    sb.AwardedByTeacherId,
                    sb.CustomPraise,
                    sb.AwardedAt,
                    badge?.Name,
                    badge?.IconUrl,
                    badge?.BadgeType
                );
            })
            .ToList()
            .AsReadOnly();
    }
}
