using FrogEdu.Exam.Application.DTOs;
using MediatR;

namespace FrogEdu.Exam.Application.Commands.CreateMatrix;

public sealed record CreateMatrixCommand(
    string Name,
    string? Description,
    Guid SubjectId,
    int Grade,
    List<MatrixTopicDto> MatrixTopics,
    string UserId
) : IRequest<CreateMatrixResponse>;
