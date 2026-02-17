using FrogEdu.Class.Application.Dtos;
using FrogEdu.Class.Application.Interfaces;
using FrogEdu.Class.Domain.Entities;
using FrogEdu.Class.Domain.Repositories;
using FrogEdu.Shared.Kernel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FrogEdu.Class.Application.Commands.SubmitExamAttempt;

public sealed class SubmitExamAttemptCommandHandler
    : IRequestHandler<SubmitExamAttemptCommand, Result<StudentExamAttemptResponse>>
{
    private readonly IStudentExamAttemptRepository _attemptRepository;
    private readonly IExamSessionRepository _examSessionRepository;
    private readonly IExamServiceClient _examServiceClient;
    private readonly ILogger<SubmitExamAttemptCommandHandler> _logger;

    public SubmitExamAttemptCommandHandler(
        IStudentExamAttemptRepository attemptRepository,
        IExamSessionRepository examSessionRepository,
        IExamServiceClient examServiceClient,
        ILogger<SubmitExamAttemptCommandHandler> logger
    )
    {
        _attemptRepository = attemptRepository;
        _examSessionRepository = examSessionRepository;
        _examServiceClient = examServiceClient;
        _logger = logger;
    }

    public async Task<Result<StudentExamAttemptResponse>> Handle(
        SubmitExamAttemptCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            if (!Guid.TryParse(request.StudentId, out var studentId))
                return Result<StudentExamAttemptResponse>.Failure("Invalid student ID format");

            var attempt = await _attemptRepository.GetByIdWithAnswersAsync(
                request.AttemptId,
                cancellationToken
            );

            if (attempt is null)
                return Result<StudentExamAttemptResponse>.Failure("Attempt not found");

            if (attempt.StudentId != studentId)
                return Result<StudentExamAttemptResponse>.Failure(
                    "You can only submit your own attempt"
                );

            if (attempt.ExamSessionId != request.ExamSessionId)
                return Result<StudentExamAttemptResponse>.Failure(
                    "Attempt does not belong to this session"
                );

            if (attempt.Status != Domain.Enums.AttemptStatus.InProgress)
                return Result<StudentExamAttemptResponse>.Failure(
                    "This attempt has already been submitted"
                );

            // Get exam session for scoring config
            var session = await _examSessionRepository.GetByIdAsync(
                request.ExamSessionId,
                cancellationToken
            );

            if (session is null)
                return Result<StudentExamAttemptResponse>.Failure("Exam session not found");

            // Fetch exam questions with correct answers from Exam service
            var examData = await _examServiceClient.GetExamWithAnswersAsync(
                session.ExamId,
                cancellationToken
            );

            if (examData is null)
                return Result<StudentExamAttemptResponse>.Failure(
                    "Failed to retrieve exam data for grading"
                );

            // Grade the answers
            double totalScore = 0;
            double totalPoints = 0;
            var answerResponses = new List<StudentAnswerResponse>();

            foreach (var question in examData.Questions)
            {
                // Skip Essay questions for MVP
                if (question.Type.Equals("Essay", StringComparison.OrdinalIgnoreCase))
                    continue;

                totalPoints += question.Point;

                var studentAnswer = request.Answers.FirstOrDefault(a =>
                    a.QuestionId == question.Id
                );

                if (studentAnswer is null)
                {
                    // No answer provided - score 0
                    var emptyAnswer = StudentAnswer.Create(attempt.Id, question.Id, string.Empty);
                    emptyAnswer.Grade(0, false, false);
                    attempt.AddAnswer(emptyAnswer);

                    answerResponses.Add(
                        new StudentAnswerResponse(
                            emptyAnswer.Id,
                            emptyAnswer.QuestionId,
                            emptyAnswer.SelectedAnswerIds,
                            0,
                            false,
                            false
                        )
                    );
                    continue;
                }

                var selectedAnswerIdsStr = string.Join(",", studentAnswer.SelectedAnswerIds);

                var answer = StudentAnswer.Create(attempt.Id, question.Id, selectedAnswerIdsStr);

                var (score, isCorrect, isPartial) = CalculateScore(
                    question,
                    selectedAnswerIdsStr,
                    session.AllowPartialScoring
                );

                answer.Grade(score, isCorrect, isPartial);
                attempt.AddAnswer(answer);
                totalScore += score;

                answerResponses.Add(
                    new StudentAnswerResponse(
                        answer.Id,
                        answer.QuestionId,
                        answer.SelectedAnswerIds,
                        score,
                        isCorrect,
                        isPartial
                    )
                );
            }

            attempt.Submit(totalScore, totalPoints);
            _attemptRepository.Update(attempt);
            await _attemptRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Student {StudentId} submitted attempt {AttemptId} with score {Score}/{TotalPoints}",
                studentId,
                attempt.Id,
                totalScore,
                totalPoints
            );

            return Result<StudentExamAttemptResponse>.Success(
                new StudentExamAttemptResponse(
                    attempt.Id,
                    attempt.ExamSessionId,
                    attempt.StudentId,
                    attempt.StartedAt,
                    attempt.SubmittedAt,
                    attempt.Score,
                    attempt.TotalPoints,
                    attempt.GetScorePercentage(),
                    attempt.AttemptNumber,
                    attempt.Status.ToString(),
                    answerResponses
                )
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error submitting exam attempt");
            return Result<StudentExamAttemptResponse>.Failure(
                "An error occurred while submitting the exam attempt"
            );
        }
    }

    /// <summary>
    /// Calculate the score for a question based on student's selected answers.
    /// Supports partial scoring for MultipleAnswer questions.
    /// </summary>
    private static (double Score, bool IsCorrect, bool IsPartial) CalculateScore(
        ExamQuestionDto question,
        string selectedAnswerIds,
        bool allowPartialScoring
    )
    {
        if (string.IsNullOrWhiteSpace(selectedAnswerIds))
            return (0, false, false);

        var selectedIds = selectedAnswerIds
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var correctAnswers = question.Answers.Where(a => a.IsCorrect).ToList();
        var correctIds = correctAnswers
            .Select(a => a.Id.ToString())
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        // For FillInTheBlank: compare text content directly
        if (question.Type.Equals("FillInTheBlank", StringComparison.OrdinalIgnoreCase))
        {
            var studentText = selectedAnswerIds.Trim();
            var isMatch = question.Answers.Any(a =>
                a.IsCorrect
                && a.Content.Trim().Equals(studentText, StringComparison.OrdinalIgnoreCase)
            );
            return isMatch ? (question.Point, true, false) : (0, false, false);
        }

        // For MultipleChoice and TrueFalse: exact match required
        if (
            question.Type.Equals("MultipleChoice", StringComparison.OrdinalIgnoreCase)
            || question.Type.Equals("TrueFalse", StringComparison.OrdinalIgnoreCase)
        )
        {
            if (selectedIds.Count != 1)
                return (0, false, false);

            var isCorrect = correctIds.Contains(selectedIds.First());
            return isCorrect ? (question.Point, true, false) : (0, false, false);
        }

        // For MultipleAnswer: partial scoring support
        if (question.Type.Equals("MultipleAnswer", StringComparison.OrdinalIgnoreCase))
        {
            var correctSelected = selectedIds.Intersect(correctIds).Count();
            var incorrectSelected = selectedIds.Except(correctIds).Count();
            var totalCorrect = correctIds.Count;

            if (correctSelected == totalCorrect && incorrectSelected == 0)
                return (question.Point, true, false);

            if (correctSelected == 0)
                return (0, false, false);

            if (allowPartialScoring)
            {
                // Partial scoring: (correct selected - incorrect selected) / total correct * points
                // Minimum 0
                var partialRatio = Math.Max(
                    0,
                    (double)(correctSelected - incorrectSelected) / totalCorrect
                );
                var partialScore = Math.Round(question.Point * partialRatio, 2);
                return (partialScore, false, partialScore > 0);
            }

            return (0, false, false);
        }

        return (0, false, false);
    }
}
