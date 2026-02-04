using FrogEdu.Exam.Application.DTOs;

namespace FrogEdu.Exam.Application.Interfaces;

public interface IExamExportService
{
    byte[] ExportToPdf(ExamPreviewDto examPreview);
    byte[] ExportToExcel(ExamPreviewDto examPreview);
    byte[] ExportMatrixToPdf(MatrixDto matrix, string examName, string subjectName, int grade);
    byte[] ExportMatrixToExcel(MatrixDto matrix, string examName, string subjectName, int grade);
}
