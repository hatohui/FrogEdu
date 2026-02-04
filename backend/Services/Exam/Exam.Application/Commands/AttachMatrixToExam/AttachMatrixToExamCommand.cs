using MediatR;

namespace FrogEdu.Exam.Application.Commands.AttachMatrixToExam;

public sealed record AttachMatrixToExamCommand(Guid ExamId, Guid MatrixId, string UserId)
    : IRequest;
