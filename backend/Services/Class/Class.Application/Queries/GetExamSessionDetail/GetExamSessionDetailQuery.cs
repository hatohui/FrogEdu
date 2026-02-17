using FrogEdu.Class.Application.Dtos;
using MediatR;

namespace FrogEdu.Class.Application.Queries.GetExamSessionDetail;

public sealed record GetExamSessionDetailQuery(Guid SessionId) : IRequest<ExamSessionResponse?>;
