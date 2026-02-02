using FrogEdu.Exam.Application.DTOs;
using MediatR;

namespace FrogEdu.Exam.Application.Queries.GetSubjectById;

public sealed record GetSubjectByIdQuery(Guid SubjectId) : IRequest<SubjectDto?>;
