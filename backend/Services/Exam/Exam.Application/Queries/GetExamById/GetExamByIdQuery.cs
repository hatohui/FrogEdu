using FrogEdu.Exam.Application.DTOs;
using MediatR;

namespace FrogEdu.Exam.Application.Queries.GetExamById;

public sealed record GetExamByIdQuery(Guid ExamId, string UserId) : IRequest<ExamDto?>;
