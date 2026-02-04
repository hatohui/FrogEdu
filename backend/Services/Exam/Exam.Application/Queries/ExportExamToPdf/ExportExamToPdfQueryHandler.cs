using FrogEdu.Exam.Application.Interfaces;
using MediatR;

namespace FrogEdu.Exam.Application.Queries.ExportExamToPdf;

public sealed class ExportExamToPdfQueryHandler(
    IMediator mediator,
    IExamExportService exportService
) : IRequestHandler<ExportExamToPdfQuery, byte[]?>
{
    private readonly IMediator _mediator = mediator;
    private readonly IExamExportService _exportService = exportService;

    public async Task<byte[]?> Handle(
        ExportExamToPdfQuery request,
        CancellationToken cancellationToken
    )
    {
        var examPreview = await _mediator.Send(
            new GetExamPreview.GetExamPreviewQuery(request.ExamId, request.UserId),
            cancellationToken
        );

        if (examPreview is null)
            return null;

        return _exportService.ExportToPdf(examPreview);
    }
}
