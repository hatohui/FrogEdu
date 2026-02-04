using MediatR;

namespace FrogEdu.Exam.Application.Queries.ExportExamToPdf;

public sealed record ExportExamToPdfQuery(Guid ExamId, string UserId) : IRequest<byte[]?>;
