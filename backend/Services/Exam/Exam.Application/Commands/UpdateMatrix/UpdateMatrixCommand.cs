using FrogEdu.Exam.Application.DTOs;
using MediatR;

namespace FrogEdu.Exam.Application.Commands.UpdateMatrix;

public sealed record UpdateMatrixCommand(
    Guid MatrixId,
    string? Name,
    string? Description,
    List<MatrixTopicDto> MatrixTopics,
    string UserId
) : IRequest<Unit>;
