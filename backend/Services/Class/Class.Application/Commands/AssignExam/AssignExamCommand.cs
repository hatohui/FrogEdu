using FrogEdu.Class.Application.Dtos;
using FrogEdu.Shared.Kernel;
using MediatR;

namespace FrogEdu.Class.Application.Commands.AssignExam;

public sealed record AssignExamCommand(
    Guid ClassId,
    Guid ExamId,
    DateTime StartDate,
    DateTime DueDate,
    bool IsMandatory,
    int Weight,
    string UserId
) : IRequest<Result<AssignmentResponse>>;
