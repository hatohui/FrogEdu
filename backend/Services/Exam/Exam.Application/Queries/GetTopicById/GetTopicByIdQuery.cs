using FrogEdu.Exam.Application.DTOs;
using MediatR;

namespace FrogEdu.Exam.Application.Queries.GetTopicById;

public sealed record GetTopicByIdQuery(Guid TopicId) : IRequest<TopicDto?>;
