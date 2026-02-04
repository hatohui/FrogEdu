using MediatR;

namespace FrogEdu.Exam.Application.Commands.RemoveQuestionFromExam;

public sealed record RemoveQuestionFromExamCommand(Guid ExamId, Guid QuestionId, string UserId)
    : IRequest;
