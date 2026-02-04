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
        using var workbook = new XLWorkbook();
        var worksheet = workbook.AddWorksheet("Exam");

        // Header Section
        var currentRow = 1;

        worksheet.Cell(currentRow, 1).Value = "Exam Name:";
        worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
        worksheet.Cell(currentRow, 2).Value = examPreview.Name;
        worksheet.Range(currentRow, 2, currentRow, 4).Merge();
        currentRow++;

        worksheet.Cell(currentRow, 1).Value = "Subject:";
        worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
        worksheet.Cell(currentRow, 2).Value = examPreview.SubjectName;
        currentRow++;

        worksheet.Cell(currentRow, 1).Value = "Grade:";
        worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
        worksheet.Cell(currentRow, 2).Value = examPreview.Grade;
        currentRow++;

        worksheet.Cell(currentRow, 1).Value = "Description:";
        worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
        worksheet.Cell(currentRow, 2).Value = examPreview.Description;
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
        worksheet.Cell(currentRow, 3).Value = "Type";
        worksheet.Cell(currentRow, 4).Value = "Level";
        worksheet.Cell(currentRow, 5).Value = "Points";
        worksheet.Cell(currentRow, 6).Value = "Answers";

        var headerRange = worksheet.Range(currentRow, 1, currentRow, 6);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
        headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        currentRow++;

        // Questions
        foreach (var question in examPreview.Questions)
        {
            worksheet.Cell(currentRow, 1).Value = question.QuestionNumber;
            worksheet.Cell(currentRow, 2).Value = question.Content;
            worksheet.Cell(currentRow, 3).Value = question.Type;
            worksheet.Cell(currentRow, 4).Value = question.CognitiveLevel;
            worksheet.Cell(currentRow, 5).Value = question.Point;

            // Answers
            var answersText = string.Join(
                "\n",
                question.Answers.Select(a =>
                    $"{a.Label}. {a.Content}" + (a.IsCorrect ? " [Correct]" : "")
                )
            );
            worksheet.Cell(currentRow, 6).Value = answersText;
            worksheet.Cell(currentRow, 6).Style.Alignment.WrapText = true;

            // Apply borders
            var questionRange = worksheet.Range(currentRow, 1, currentRow, 6);
            questionRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            questionRange.Style.Border.InsideBorder = XLBorderStyleValues.Hair;

            currentRow++;
        }

        // Adjust column widths
        worksheet.Column(1).Width = 5;
        worksheet.Column(2).Width = 50;
        worksheet.Column(3).Width = 15;
        worksheet.Column(4).Width = 12;
        worksheet.Column(5).Width = 8;
        worksheet.Column(6).Width = 50;

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
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
                // Question header
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

                // Question metadata
                column
                    .Item()
                    .PaddingTop(5)
                    .Text(text =>
                    {
                        text.Span("Type: ").Italic();
                        text.Span(question.Type);
                        text.Span(" | ");
                        text.Span("Level: ").Italic();
                        text.Span(question.CognitiveLevel);
                    });

                // Answers
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

                                    if (answer.IsCorrect)
                                    {
                                        text.Span(" ✓").Bold();
                                    }
                                });
                        }
                    });
            });
    }
}
