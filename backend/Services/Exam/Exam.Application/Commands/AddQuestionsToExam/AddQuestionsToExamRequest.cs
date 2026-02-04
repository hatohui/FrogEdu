namespace FrogEdu.Exam.Application.Commands.AddQuestionsToExam;

public sealed record AddQuestionsToExamRequest(List<Guid> QuestionIds);
