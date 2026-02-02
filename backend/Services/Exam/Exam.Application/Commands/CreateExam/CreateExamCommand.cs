using MediatR;

namespace FrogEdu.Exam.Application.Commands.CreateExam;

public sealed record CreateExamCommand(
    string Title,
    int Duration,
    int PassScore,
    int MaxAttempts,
    DateTime StartTime,
    DateTime EndTime,
    Guid TopicId,
    bool ShouldShuffleQuestions,
    bool ShouldShuffleAnswerOptions,
    string UserId
) : IRequest<CreateExamResponse>;
