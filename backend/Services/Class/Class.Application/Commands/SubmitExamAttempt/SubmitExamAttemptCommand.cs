using FrogEdu.Class.Application.Dtos;
using FrogEdu.Class.Application.Dtos.requests;
using FrogEdu.Shared.Kernel;
using MediatR;

namespace FrogEdu.Class.Application.Commands.SubmitExamAttempt;

public sealed record SubmitExamAttemptCommand(
    Guid ExamSessionId,
    Guid AttemptId,
    string StudentId,
    List<StudentAnswerDto> Answers
) : IRequest<Result<StudentExamAttemptResponse>>;
