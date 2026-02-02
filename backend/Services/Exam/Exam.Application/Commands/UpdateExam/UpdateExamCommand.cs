using MediatR;

namespace FrogEdu.Exam.Application.Commands.UpdateExam;

public sealed record UpdateExamCommand(Guid ExamId, string Name, string Description, string UserId)
    : IRequest<Unit>;
