using MediatR;

namespace FrogEdu.Exam.Application.Commands.AddQuestionsToExam;

public sealed record AddQuestionsToExamCommand(Guid ExamId, List<Guid> QuestionIds, string UserId)
    : IRequest;
