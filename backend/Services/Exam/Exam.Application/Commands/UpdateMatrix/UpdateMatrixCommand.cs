using FrogEdu.Exam.Application.DTOs;
using MediatR;

namespace FrogEdu.Exam.Application.Commands.UpdateMatrix;

public sealed record UpdateMatrixCommand(
    Guid MatrixId,
    List<MatrixTopicDto> MatrixTopics,
    string UserId
) : IRequest<Unit>;
