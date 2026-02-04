using MediatR;

namespace FrogEdu.Exam.Application.Queries.ExportExamToExcel;

public sealed record ExportExamToExcelQuery(Guid ExamId, string UserId) : IRequest<byte[]?>;
