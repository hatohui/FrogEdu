namespace FrogEdu.Class.Application.Dtos.requests;

public sealed record SubmitExamAttemptRequest(List<StudentAnswerDto> Answers);

public sealed record StudentAnswerDto(Guid QuestionId, List<string> SelectedAnswerIds);
