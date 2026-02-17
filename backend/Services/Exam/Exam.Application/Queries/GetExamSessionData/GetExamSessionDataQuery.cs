using FrogEdu.Exam.Application.DTOs;
using MediatR;

namespace FrogEdu.Exam.Application.Queries.GetExamSessionData;

/// <summary>
/// Query to fetch exam data for session grading and display.
/// Does not require ownership check — used for service-to-service communication
/// and student exam-taking.
/// </summary>
public sealed record GetExamSessionDataQuery(Guid ExamId) : IRequest<ExamSessionDataDto?>;
