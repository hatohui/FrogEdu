using FrogEdu.Exam.Application.DTOs;
using MediatR;

namespace FrogEdu.Exam.Application.Queries.GetMatrixByExamId;

public sealed record GetMatrixByExamIdQuery(Guid ExamId, string UserId) : IRequest<MatrixDto?>;
