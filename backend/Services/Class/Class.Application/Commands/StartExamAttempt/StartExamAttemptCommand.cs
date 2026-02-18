using FrogEdu.Class.Application.Dtos;
using FrogEdu.Shared.Kernel;
using MediatR;

namespace FrogEdu.Class.Application.Commands.StartExamAttempt;

public sealed record StartExamAttemptCommand(
    Guid ExamSessionId,
    string StudentId,
    string Role = "Student"
) : IRequest<Result<StudentExamAttemptResponse>>;
