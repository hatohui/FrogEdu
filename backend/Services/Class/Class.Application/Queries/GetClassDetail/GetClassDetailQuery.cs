using FrogEdu.Class.Application.Dtos;
using MediatR;

namespace FrogEdu.Class.Application.Queries.GetClassDetail;

public sealed record GetClassDetailQuery(Guid ClassId) : IRequest<ClassDetailResponse?>;
