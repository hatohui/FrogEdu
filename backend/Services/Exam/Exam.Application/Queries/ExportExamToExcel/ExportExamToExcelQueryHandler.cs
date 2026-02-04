using FrogEdu.Exam.Application.Interfaces;
using MediatR;

namespace FrogEdu.Exam.Application.Queries.ExportExamToExcel;

public sealed class ExportExamToExcelQueryHandler(
    IMediator mediator,
    IExamExportService exportService
) : IRequestHandler<ExportExamToExcelQuery, byte[]?>
{
    private readonly IMediator _mediator = mediator;
    private readonly IExamExportService _exportService = exportService;

    public async Task<byte[]?> Handle(
        ExportExamToExcelQuery request,
        CancellationToken cancellationToken
    )
    {
        var examPreview = await _mediator.Send(
            new GetExamPreview.GetExamPreviewQuery(request.ExamId, request.UserId),
            cancellationToken
        );

        if (examPreview is null)
            return null;

        return _exportService.ExportToExcel(examPreview);
    }
}
