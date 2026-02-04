using FrogEdu.Exam.Application.DTOs;

namespace FrogEdu.Exam.Application.Interfaces;

public interface IExamExportService
{
    byte[] ExportToPdf(ExamPreviewDto examPreview);
    byte[] ExportToExcel(ExamPreviewDto examPreview);
}
