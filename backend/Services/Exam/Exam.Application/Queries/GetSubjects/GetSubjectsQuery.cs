using MediatR;

namespace FrogEdu.Exam.Application.Queries.GetSubjects;

public sealed record GetSubjectsQuery(int? Grade = null) : IRequest<GetSubjectsResponse>;
