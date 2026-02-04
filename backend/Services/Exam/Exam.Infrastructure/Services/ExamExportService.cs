using ClosedXML.Excel;
using FrogEdu.Exam.Application.DTOs;
using FrogEdu.Exam.Application.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace FrogEdu.Exam.Infrastructure.Services;

public class ExamExportService : IExamExportService
{
    public byte[] ExportToPdf(ExamPreviewDto examPreview)
    {
        return Document
            .Create(document =>
            {
                document.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    page.Header().Element(container => ComposeHeader(container, examPreview));

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Element(container => ComposeContent(container, examPreview));

                    page.Footer()
                        .AlignCenter()
                        .Text(text =>
                        {
                            text.Span("Page ");
                            text.CurrentPageNumber();
                            text.Span(" of ");
                            text.TotalPages();
                        });
                });
            })
            .GeneratePdf();
    }

    public byte[] ExportToExcel(ExamPreviewDto examPreview)
    {
        try
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.AddWorksheet("Exam");

            // Header Section
            var currentRow = 1;

            worksheet.Cell(currentRow, 1).Value = "Exam Name:";
            worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
            worksheet.Cell(currentRow, 2).Value = examPreview.Name ?? string.Empty;
            worksheet.Range(currentRow, 2, currentRow, 4).Merge();
            currentRow++;

            worksheet.Cell(currentRow, 1).Value = "Subject:";
            worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
            worksheet.Cell(currentRow, 2).Value = examPreview.SubjectName ?? string.Empty;
            currentRow++;

            worksheet.Cell(currentRow, 1).Value = "Grade:";
            worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
            worksheet.Cell(currentRow, 2).Value = examPreview.Grade;
            currentRow++;

            worksheet.Cell(currentRow, 1).Value = "Description:";
            worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
            worksheet.Cell(currentRow, 2).Value = examPreview.Description ?? string.Empty;
            worksheet.Range(currentRow, 2, currentRow, 4).Merge();
            currentRow++;

            worksheet.Cell(currentRow, 1).Value = "Total Questions:";
            worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
            worksheet.Cell(currentRow, 2).Value = examPreview.QuestionCount;
            currentRow++;

            worksheet.Cell(currentRow, 1).Value = "Total Points:";
            worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
            worksheet.Cell(currentRow, 2).Value = examPreview.TotalPoints;
            currentRow += 2;

            // Questions Table Header
            worksheet.Cell(currentRow, 1).Value = "No.";
            worksheet.Cell(currentRow, 2).Value = "Question";
            worksheet.Cell(currentRow, 3).Value = "Points";
            worksheet.Cell(currentRow, 4).Value = "Answers";

            var headerRange = worksheet.Range(currentRow, 1, currentRow, 4);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
            headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            currentRow++;

            // Questions
            foreach (var question in examPreview.Questions)
            {
                worksheet.Cell(currentRow, 1).Value = question.QuestionNumber;
                worksheet.Cell(currentRow, 2).Value = question.Content ?? string.Empty;
                worksheet.Cell(currentRow, 3).Value = question.Point;

                // Answer options (without correct answer indicators for student practice)
                var answersText = string.Join(
                    "\n",
                    question.Answers.Select(a => $"{a.Label}. {a.Content ?? string.Empty}")
                );
                worksheet.Cell(currentRow, 4).Value = answersText;
                worksheet.Cell(currentRow, 4).Style.Alignment.WrapText = true;

                // Apply borders
                var questionRange = worksheet.Range(currentRow, 1, currentRow, 4);
                questionRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                questionRange.Style.Border.InsideBorder = XLBorderStyleValues.Hair;

                currentRow++;
            }

            // Adjust column widths
            worksheet.Column(1).Width = 5;
            worksheet.Column(2).Width = 50;
            worksheet.Column(3).Width = 8;
            worksheet.Column(4).Width = 50;

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
        catch (Exception ex)
        {
            // Log the exception or rethrow with more context
            throw new InvalidOperationException(
                $"Failed to export exam to Excel: {ex.Message}",
                ex
            );
        }
    }

    private static void ComposeHeader(IContainer container, ExamPreviewDto examPreview)
    {
        container.Column(column =>
        {
            column.Item().Text(examPreview.Name).Bold().FontSize(20);

            column
                .Item()
                .PaddingTop(10)
                .Row(row =>
                {
                    row.RelativeItem()
                        .Column(col =>
                        {
                            col.Item()
                                .Text(text =>
                                {
                                    text.Span("Subject: ").Bold();
                                    text.Span(examPreview.SubjectName);
                                });
                            col.Item()
                                .Text(text =>
                                {
                                    text.Span("Grade: ").Bold();
                                    text.Span(examPreview.Grade.ToString());
                                });
                        });

                    row.RelativeItem()
                        .Column(col =>
                        {
                            col.Item()
                                .Text(text =>
                                {
                                    text.Span("Questions: ").Bold();
                                    text.Span(examPreview.QuestionCount.ToString());
                                });
                            col.Item()
                                .Text(text =>
                                {
                                    text.Span("Total Points: ").Bold();
                                    text.Span(examPreview.TotalPoints.ToString());
                                });
                        });
                });

            if (!string.IsNullOrWhiteSpace(examPreview.Description))
            {
                column
                    .Item()
                    .PaddingTop(10)
                    .Text(text =>
                    {
                        text.Span("Description: ").Bold();
                        text.Span(examPreview.Description);
                    });
            }

            column.Item().PaddingTop(5).LineHorizontal(1);
        });
    }

    private static void ComposeContent(IContainer container, ExamPreviewDto examPreview)
    {
        container.Column(column =>
        {
            foreach (var question in examPreview.Questions)
            {
                column.Item().PaddingBottom(15).Element(c => ComposeQuestion(c, question));
            }
        });
    }

    private static void ComposeQuestion(IContainer container, ExamPreviewQuestionDto question)
    {
        container
            .Border(1)
            .Padding(10)
            .Column(column =>
            {
                // Question header (student version - no type or cognitive level)
                column
                    .Item()
                    .Row(row =>
                    {
                        row.RelativeItem()
                            .Text(text =>
                            {
                                text.Span($"Question {question.QuestionNumber}. ").Bold();
                                text.Span(question.Content);
                            });

                        row.ConstantItem(80).AlignRight().Text($"({question.Point} pts)").Italic();
                    });

                // Answer options (no correct answer indicators)
                column
                    .Item()
                    .PaddingTop(10)
                    .Column(answerColumn =>
                    {
                        foreach (var answer in question.Answers)
                        {
                            answerColumn
                                .Item()
                                .PaddingBottom(5)
                                .Text(text =>
                                {
                                    text.Span($"{answer.Label}. ");
                                    text.Span(answer.Content);
                                    // IsCorrect is intentionally not displayed for student practice
                                });
                        }
                    });
            });
    }

    public byte[] ExportMatrixToPdf(
        MatrixDto matrix,
        string examName,
        string subjectName,
        int grade
    )
    {
        return Document
            .Create(document =>
            {
                document.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    page.Header()
                        .Element(container =>
                            ComposeMatrixHeader(container, examName, subjectName, grade, matrix)
                        );

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Element(container => ComposeMatrixContent(container, matrix));

                    page.Footer()
                        .AlignCenter()
                        .Text(text =>
                        {
                            text.Span("Page ");
                            text.CurrentPageNumber();
                            text.Span(" of ");
                            text.TotalPages();
                        });
                });
            })
            .GeneratePdf();
    }

    public byte[] ExportMatrixToExcel(
        MatrixDto matrix,
        string examName,
        string subjectName,
        int grade
    )
    {
        try
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.AddWorksheet("Exam Matrix");

            var currentRow = 1;

            // Header
            worksheet.Cell(currentRow, 1).Value = "Exam Matrix";
            worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
            worksheet.Cell(currentRow, 1).Style.Font.FontSize = 16;
            worksheet.Range(currentRow, 1, currentRow, 4).Merge();
            currentRow += 2;

            worksheet.Cell(currentRow, 1).Value = "Exam:";
            worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
            worksheet.Cell(currentRow, 2).Value = examName ?? string.Empty;
            currentRow++;

            worksheet.Cell(currentRow, 1).Value = "Subject:";
            worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
            worksheet.Cell(currentRow, 2).Value = subjectName ?? string.Empty;
            currentRow++;

            worksheet.Cell(currentRow, 1).Value = "Grade:";
            worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
            worksheet.Cell(currentRow, 2).Value = grade;
            currentRow++;

            worksheet.Cell(currentRow, 1).Value = "Total Questions:";
            worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
            worksheet.Cell(currentRow, 2).Value = matrix.TotalQuestionCount;
            currentRow += 2;

            // Matrix table
            worksheet.Cell(currentRow, 1).Value = "Topic";
            worksheet.Cell(currentRow, 2).Value = "Cognitive Level";
            worksheet.Cell(currentRow, 3).Value = "Quantity";

            var headerRange = worksheet.Range(currentRow, 1, currentRow, 3);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
            headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            currentRow++;

            foreach (var topic in matrix.MatrixTopics)
            {
                worksheet.Cell(currentRow, 1).Value = topic.TopicTitle ?? "Unknown Topic";
                worksheet.Cell(currentRow, 2).Value = topic.CognitiveLevel.ToString();
                worksheet.Cell(currentRow, 3).Value = topic.Quantity;

                var rowRange = worksheet.Range(currentRow, 1, currentRow, 3);
                rowRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                currentRow++;
            }

            worksheet.Column(1).Width = 40;
            worksheet.Column(2).Width = 20;
            worksheet.Column(3).Width = 10;

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Failed to export matrix to Excel: {ex.Message}",
                ex
            );
        }
    }

    private static void ComposeMatrixHeader(
        IContainer container,
        string examName,
        string subjectName,
        int grade,
        MatrixDto matrix
    )
    {
        container.Column(column =>
        {
            column.Item().Text("Exam Matrix").Bold().FontSize(20);

            column
                .Item()
                .PaddingTop(10)
                .Column(col =>
                {
                    col.Item()
                        .Text(text =>
                        {
                            text.Span("Exam: ").Bold();
                            text.Span(examName);
                        });
                    col.Item()
                        .Text(text =>
                        {
                            text.Span("Subject: ").Bold();
                            text.Span(subjectName);
                        });
                    col.Item()
                        .Text(text =>
                        {
                            text.Span("Grade: ").Bold();
                            text.Span(grade.ToString());
                        });
                    col.Item()
                        .Text(text =>
                        {
                            text.Span("Total Questions: ").Bold();
                            text.Span(matrix.TotalQuestionCount.ToString());
                        });
                });

            column.Item().PaddingTop(5).LineHorizontal(1);
        });
    }

    private static void ComposeMatrixContent(IContainer container, MatrixDto matrix)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.RelativeColumn(2);
                columns.RelativeColumn(2);
                columns.RelativeColumn(1);
            });

            // Header
            table.Header(header =>
            {
                header.Cell().Element(CellStyle).Text("Topic").Bold();
                header.Cell().Element(CellStyle).Text("Cognitive Level").Bold();
                header.Cell().Element(CellStyle).Text("Quantity").Bold();

                static IContainer CellStyle(IContainer c) =>
                    c.Border(1).Background(Colors.Grey.Lighten3).Padding(5);
            });

            // Content
            foreach (var topic in matrix.MatrixTopics)
            {
                table.Cell().Element(CellStyle).Text(topic.TopicTitle ?? "Unknown Topic");
                table.Cell().Element(CellStyle).Text(topic.CognitiveLevel.ToString());
                table.Cell().Element(CellStyle).Text(topic.Quantity.ToString());

                static IContainer CellStyle(IContainer c) => c.Border(1).Padding(5);
            }
        });
    }
}
