using FrogEdu.Class.Application.Dtos;
using FrogEdu.Class.Domain.Repositories;
using MediatR;

namespace FrogEdu.Class.Application.Queries.GetClassAssignments;

public sealed class GetClassAssignmentsQueryHandler
    : IRequestHandler<GetClassAssignmentsQuery, IReadOnlyList<AssignmentResponse>>
{
    private readonly IAssignmentRepository _assignmentRepository;

    public GetClassAssignmentsQueryHandler(IAssignmentRepository assignmentRepository)
    {
        _assignmentRepository = assignmentRepository;
    }

    public async Task<IReadOnlyList<AssignmentResponse>> Handle(
        GetClassAssignmentsQuery request,
        CancellationToken cancellationToken
    )
    {
        var assignments = await _assignmentRepository.GetByClassIdAsync(
            request.ClassId,
            cancellationToken
        );

        return assignments
            .Select(a => new AssignmentResponse(
                a.Id,
                a.ClassId,
                a.ExamId,
                a.StartDate,
                a.DueDate,
                a.IsMandatory,
                a.Weight,
                a.IsActive(),
                a.IsOverdue()
            ))
            .ToList();
    }
}
