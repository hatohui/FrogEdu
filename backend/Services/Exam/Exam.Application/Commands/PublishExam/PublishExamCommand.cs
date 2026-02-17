using MediatR;

namespace FrogEdu.Exam.Application.Commands.PublishExam;

public sealed record PublishExamCommand(Guid ExamId, string UserId, bool IsAdmin = false)
    : IRequest<Unit>;
