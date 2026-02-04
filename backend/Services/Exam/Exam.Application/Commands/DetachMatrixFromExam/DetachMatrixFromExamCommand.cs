using MediatR;

namespace FrogEdu.Exam.Application.Commands.DetachMatrixFromExam;

public sealed record DetachMatrixFromExamCommand(Guid ExamId, string UserId) : IRequest;
