using FrogEdu.Class.Application.Dtos.responses;
using MediatR;

namespace FrogEdu.Class.Application.Queries.GetStudentBadges;

public sealed record GetStudentBadgesQuery(Guid StudentId, Guid? ClassId = null)
    : IRequest<IReadOnlyList<StudentBadgeDto>>;
