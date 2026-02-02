using FrogEdu.Exam.Application.DTOs;
using MediatR;

namespace FrogEdu.Exam.Application.Queries.GetExams;

public sealed record GetExamsQuery(bool? IsDraft, string UserId) : IRequest<GetExamsResponse>;
