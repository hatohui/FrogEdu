namespace FrogEdu.Class.Application.Dtos.requests;

public sealed record SubmitExamAttemptRequest(List<StudentAnswerDto> Answers);

/// <summary>
/// A single answer submitted by the student.
/// </summary>
/// <param name="QuestionId">The question being answered.</param>
/// <param name="SelectedAnswerIds">
///   For MC/MA/TF: the IDs of the chosen answer options.
///   For FillInTheBlank: a single-element list containing the typed text.
///   For Essay: leave empty — use EssayText instead.
/// </param>
/// <param name="EssayText">Student's free-text essay response (Essay type only).</param>
public sealed record StudentAnswerDto(
    Guid QuestionId,
    List<string> SelectedAnswerIds,
    string? EssayText = null
);
