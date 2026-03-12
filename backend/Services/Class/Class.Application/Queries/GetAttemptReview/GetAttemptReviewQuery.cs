using FrogEdu.Class.Application.Dtos;
using MediatR;

namespace FrogEdu.Class.Application.Queries.GetAttemptReview;

/// <summary>
/// Query to get a full review of a student's attempt, including question content,
/// correct answers and explanations. Enriched by calling the Exam service.
/// </summary>
public sealed record GetAttemptReviewQuery(Guid AttemptId) : IRequest<AttemptReviewResponse?>;
