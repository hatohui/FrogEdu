using FrogEdu.Exam.Application.DTOs;
using MediatR;

namespace FrogEdu.Exam.Application.Commands.CreateMatrix;

public sealed record CreateMatrixCommand(
    Guid ExamId,
    List<MatrixTopicDto> MatrixTopics,
    string UserId
) : IRequest<CreateMatrixResponse>;
