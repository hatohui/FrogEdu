using FrogEdu.Class.Application.Dtos;
using FrogEdu.Shared.Kernel;
using MediatR;

namespace FrogEdu.Class.Application.Commands.UpdateAssignment;

public sealed record UpdateAssignmentCommand(
    Guid ClassId,
    Guid AssignmentId,
    DateTime StartDate,
    DateTime DueDate,
    bool IsMandatory,
    int Weight,
    string UserId,
    string Role = "Teacher",
    int RetryTimes = 0,
    bool IsRetryable = false,
    bool ShouldShuffleQuestions = false,
    bool ShouldShuffleAnswers = false,
    bool AllowPartialScoring = true
) : IRequest<Result<AssignmentResponse>>;
