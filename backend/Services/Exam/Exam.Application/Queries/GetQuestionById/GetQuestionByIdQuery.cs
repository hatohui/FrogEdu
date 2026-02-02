using FrogEdu.Exam.Application.DTOs;
using MediatR;

namespace FrogEdu.Exam.Application.Queries.GetQuestionById;

public sealed record GetQuestionByIdQuery(Guid QuestionId, string UserId) : IRequest<QuestionDto?>;
