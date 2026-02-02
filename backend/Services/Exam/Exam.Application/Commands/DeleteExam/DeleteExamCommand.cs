using MediatR;

namespace FrogEdu.Exam.Application.Commands.DeleteExam;

public sealed record DeleteExamCommand(Guid ExamId, string UserId) : IRequest<Unit>;
