using FrogEdu.Class.Domain.Entities;
using FrogEdu.Class.Domain.Repositories;
using FrogEdu.Shared.Kernel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FrogEdu.Class.Application.Commands.AwardBadge;

public sealed class AwardBadgeCommandHandler(
    IStudentBadgeRepository studentBadgeRepository,
    IBadgeRepository badgeRepository,
    ILogger<AwardBadgeCommandHandler> logger
) : IRequestHandler<AwardBadgeCommand, Result<Guid>>
{
    private readonly IStudentBadgeRepository _studentBadgeRepository = studentBadgeRepository;
    private readonly IBadgeRepository _badgeRepository = badgeRepository;
    private readonly ILogger<AwardBadgeCommandHandler> _logger = logger;

    public async Task<Result<Guid>> Handle(
        AwardBadgeCommand request,
        CancellationToken cancellationToken
    )
    {
        var badge = await _badgeRepository.GetByIdAsync(request.BadgeId, cancellationToken);
        if (badge is null)
            return Result<Guid>.Failure("Badge not found");

        if (!badge.IsActive)
            return Result<Guid>.Failure("Badge is not active");

        var studentBadge = StudentBadge.Create(
            request.StudentId,
            request.BadgeId,
            request.ClassId,
            request.ExamSessionId,
            request.TeacherId,
            request.CustomPraise
        );

        await _studentBadgeRepository.AddAsync(studentBadge, cancellationToken);
        await _studentBadgeRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Badge {BadgeId} awarded to student {StudentId} in class {ClassId}",
            request.BadgeId,
            request.StudentId,
            request.ClassId
        );

        return Result<Guid>.Success(studentBadge.Id);
    }
}
