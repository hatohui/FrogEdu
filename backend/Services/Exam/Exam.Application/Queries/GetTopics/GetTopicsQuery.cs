using MediatR;

namespace FrogEdu.Exam.Application.Queries.GetTopics;

public sealed record GetTopicsQuery(Guid SubjectId) : IRequest<GetTopicsResponse>;
