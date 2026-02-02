using FrogEdu.Exam.Domain.Enums;
using MediatR;

namespace FrogEdu.Exam.Application.Commands.CreateMatrix;

public sealed record MatrixTopicDto(Guid TopicId, CognitiveLevel CognitiveLevel, int Quantity);

public sealed record CreateMatrixCommand(
    Guid ExamId,
    List<MatrixTopicDto> MatrixTopics,
    string UserId
) : IRequest<CreateMatrixResponse>;
