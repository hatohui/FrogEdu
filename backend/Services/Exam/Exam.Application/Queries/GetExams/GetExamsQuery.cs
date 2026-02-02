using MediatR;

namespace FrogEdu.Exam.Application.Queries.GetExams;

public sealed record GetExamsQuery(bool? IsDraft = null) : IRequest<GetExamsResponse>;
