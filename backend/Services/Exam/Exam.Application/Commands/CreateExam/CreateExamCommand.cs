using FrogEdu.Exam.Application.DTOs;
using MediatR;

namespace FrogEdu.Exam.Application.Commands.CreateExam;

public sealed record CreateExamCommand(
    string Name,
    string Description,
    Guid TopicId,
    Guid SubjectId,
    int Grade,
    string UserId
) : IRequest<CreateExamResponse>;
