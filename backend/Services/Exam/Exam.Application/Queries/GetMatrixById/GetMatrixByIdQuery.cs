using FrogEdu.Exam.Application.DTOs;
using MediatR;

namespace FrogEdu.Exam.Application.Queries.GetMatrixById;

public sealed record GetMatrixByIdQuery(Guid MatrixId, string UserId) : IRequest<MatrixDto?>;
