using FrogEdu.Exam.Application.DTOs;
using MediatR;

namespace FrogEdu.Exam.Application.Queries.GetExamPreview;

public sealed record GetExamPreviewQuery(Guid ExamId, string UserId) : IRequest<ExamPreviewDto?>;
