using FrogEdu.Class.Application.Dtos;
using MediatR;

namespace FrogEdu.Class.Application.Queries.GetAttemptDetail;

public sealed record GetAttemptDetailQuery(Guid AttemptId) : IRequest<StudentExamAttemptResponse?>;
