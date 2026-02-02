using MediatR;

namespace FrogEdu.Exam.Application.Commands.DeleteMatrix;

public sealed record DeleteMatrixCommand(Guid MatrixId, string UserId) : IRequest<Unit>;
