# Milestone 4: Smart Exam Generator

**Feature:** Question Bank & AI-Assisted Exam Generation  
**Epic:** Assessment  
**Priority:** P1 (High)  
**Estimated Effort:** 24-28 hours  
**Status:** 🔄 Ready for Implementation

---

## Overview

Build a question bank system with AI-powered exam generation based on teacher-defined matrices. Export exams to PDF and store in S3.

### User Stories

- **US-4.1:** Teacher creates multiple-choice questions linked to textbook chapters
- **US-4.2:** Teacher defines exam matrix (difficulty distribution, topics)
- **US-4.3:** System generates exam automatically based on matrix
- **US-4.4:** Teacher previews generated exam and can replace questions
- **US-4.5:** System exports exam to PDF and stores in S3
- **US-4.6:** Student downloads exam PDF via presigned URL

---

## Domain Model

### Entities

#### `Question` (Aggregate Root)

```csharp
public class Question : Entity
{
    public string Content { get; private set; }
    public QuestionType Type { get; private set; } // MCQ, TrueFalse, Essay
    public Difficulty Difficulty { get; private set; } // Easy, Medium, Hard
    public Guid TextbookId { get; private set; }
    public Guid ChapterId { get; private set; }
    public string? Topic { get; private set; }

    private readonly List<QuestionOption> _options = new();
    public IReadOnlyList<QuestionOption> Options => _options.AsReadOnly();

    public static Question Create(
        string content,
        QuestionType type,
        Difficulty difficulty,
        Guid textbookId,
        Guid chapterId,
        string? topic)
    {
        return new Question
        {
            Content = content,
            Type = type,
            Difficulty = difficulty,
            TextbookId = textbookId,
            ChapterId = chapterId,
            Topic = topic
        };
    }

    public void AddOption(string text, bool isCorrect)
    {
        if (Type != QuestionType.MCQ && Type != QuestionType.TrueFalse)
            throw new DomainException("Only MCQ and TrueFalse questions can have options");

        if (Type == QuestionType.TrueFalse && _options.Count >= 2)
            throw new DomainException("TrueFalse questions can only have 2 options");

        var option = QuestionOption.Create(Id, text, isCorrect);
        _options.Add(option);
    }
}

public enum QuestionType
{
    MCQ = 1,
    TrueFalse = 2,
    Essay = 3
}

public enum Difficulty
{
    Easy = 1,
    Medium = 2,
    Hard = 3
}
```

#### `QuestionOption` (Entity)

```csharp
public class QuestionOption : Entity
{
    public Guid QuestionId { get; private set; }
    public string Text { get; private set; }
    public bool IsCorrect { get; private set; }
    public int OrderIndex { get; private set; }

    public static QuestionOption Create(Guid questionId, string text, bool isCorrect)
    {
        return new QuestionOption
        {
            QuestionId = questionId,
            Text = text,
            IsCorrect = isCorrect
        };
    }
}
```

#### `Exam` (Aggregate Root)

```csharp
public class Exam : Entity
{
    public string Title { get; private set; }
    public Guid ClassId { get; private set; }
    public Guid CreatedByTeacherId { get; private set; }
    public int DurationMinutes { get; private set; }
    public int TotalPoints { get; private set; }
    public DateTime? ScheduledAt { get; private set; }
    public ExamStatus Status { get; private set; }
    public string? PdfUrl { get; private set; } // S3 URL

    private readonly List<ExamQuestion> _questions = new();
    public IReadOnlyList<ExamQuestion> Questions => _questions.AsReadOnly();

    public static Exam Create(
        string title,
        Guid classId,
        Guid teacherId,
        int durationMinutes)
    {
        return new Exam
        {
            Title = title,
            ClassId = classId,
            CreatedByTeacherId = teacherId,
            DurationMinutes = durationMinutes,
            Status = ExamStatus.Draft
        };
    }

    public void AddQuestion(Guid questionId, int points, int orderIndex)
    {
        var examQuestion = ExamQuestion.Create(Id, questionId, points, orderIndex);
        _questions.Add(examQuestion);
        RecalculateTotalPoints();
    }

    public void ReplaceQuestion(Guid oldQuestionId, Guid newQuestionId)
    {
        var examQuestion = _questions.FirstOrDefault(q => q.QuestionId == oldQuestionId);
        if (examQuestion == null)
            throw new DomainException("Question not found in exam");

        examQuestion.UpdateQuestion(newQuestionId);
        MarkAsUpdated();
    }

    private void RecalculateTotalPoints()
    {
        TotalPoints = _questions.Sum(q => q.Points);
    }

    public void Finalize(string pdfUrl)
    {
        PdfUrl = pdfUrl;
        Status = ExamStatus.Published;
        MarkAsUpdated();
    }
}

public enum ExamStatus
{
    Draft = 1,
    Published = 2,
    Archived = 3
}
```

#### `ExamQuestion` (Entity)

```csharp
public class ExamQuestion : Entity
{
    public Guid ExamId { get; private set; }
    public Guid QuestionId { get; private set; }
    public int Points { get; private set; }
    public int OrderIndex { get; private set; }

    public static ExamQuestion Create(Guid examId, Guid questionId, int points, int orderIndex)
    {
        return new ExamQuestion
        {
            ExamId = examId,
            QuestionId = questionId,
            Points = points,
            OrderIndex = orderIndex
        };
    }

    public void UpdateQuestion(Guid newQuestionId)
    {
        QuestionId = newQuestionId;
    }
}
```

#### `ExamMatrix` (Value Object)

```csharp
public class ExamMatrix : ValueObject
{
    public int EasyCount { get; private set; }
    public int MediumCount { get; private set; }
    public int HardCount { get; private set; }
    public List<string> RequiredTopics { get; private set; }

    public static ExamMatrix Create(int easy, int medium, int hard, List<string> topics)
    {
        if (easy + medium + hard == 0)
            throw new DomainException("Exam must have at least one question");

        return new ExamMatrix
        {
            EasyCount = easy,
            MediumCount = medium,
            HardCount = hard,
            RequiredTopics = topics ?? new List<string>()
        };
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return EasyCount;
        yield return MediumCount;
        yield return HardCount;
        yield return string.Join(",", RequiredTopics);
    }
}
```

---

## Database Schema (AssessmentDB)

```sql
CREATE TABLE [dbo].[Questions] (
    [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [Content] NVARCHAR(MAX) NOT NULL,
    [Type] INT NOT NULL, -- 1=MCQ, 2=TrueFalse, 3=Essay
    [Difficulty] INT NOT NULL, -- 1=Easy, 2=Medium, 3=Hard
    [TextbookId] UNIQUEIDENTIFIER NOT NULL,
    [ChapterId] UNIQUEIDENTIFIER NOT NULL,
    [Topic] NVARCHAR(100) NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] DATETIME2 NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,

    CONSTRAINT [CK_Questions_Type] CHECK ([Type] IN (1, 2, 3)),
    CONSTRAINT [CK_Questions_Difficulty] CHECK ([Difficulty] IN (1, 2, 3))
);

CREATE INDEX [IX_Questions_TextbookId_Difficulty] ON [Questions]([TextbookId], [Difficulty])
    WHERE [IsDeleted] = 0;
CREATE INDEX [IX_Questions_ChapterId] ON [Questions]([ChapterId]) WHERE [IsDeleted] = 0;

CREATE TABLE [dbo].[QuestionOptions] (
    [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [QuestionId] UNIQUEIDENTIFIER NOT NULL,
    [Text] NVARCHAR(500) NOT NULL,
    [IsCorrect] BIT NOT NULL,
    [OrderIndex] INT NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),

    CONSTRAINT [FK_QuestionOptions_Questions] FOREIGN KEY ([QuestionId])
        REFERENCES [Questions]([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_QuestionOptions_QuestionId] ON [QuestionOptions]([QuestionId]);

CREATE TABLE [dbo].[Exams] (
    [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [Title] NVARCHAR(200) NOT NULL,
    [ClassId] UNIQUEIDENTIFIER NOT NULL,
    [CreatedByTeacherId] UNIQUEIDENTIFIER NOT NULL,
    [DurationMinutes] INT NOT NULL,
    [TotalPoints] INT NOT NULL,
    [ScheduledAt] DATETIME2 NULL,
    [Status] INT NOT NULL DEFAULT 1, -- 1=Draft, 2=Published, 3=Archived
    [PdfUrl] NVARCHAR(512) NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] DATETIME2 NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,

    CONSTRAINT [CK_Exams_Status] CHECK ([Status] IN (1, 2, 3))
);

CREATE INDEX [IX_Exams_ClassId] ON [Exams]([ClassId]) WHERE [IsDeleted] = 0;
CREATE INDEX [IX_Exams_TeacherId] ON [Exams]([CreatedByTeacherId]) WHERE [IsDeleted] = 0;

CREATE TABLE [dbo].[ExamQuestions] (
    [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [ExamId] UNIQUEIDENTIFIER NOT NULL,
    [QuestionId] UNIQUEIDENTIFIER NOT NULL,
    [Points] INT NOT NULL,
    [OrderIndex] INT NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),

    CONSTRAINT [FK_ExamQuestions_Exams] FOREIGN KEY ([ExamId])
        REFERENCES [Exams]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ExamQuestions_Questions] FOREIGN KEY ([QuestionId])
        REFERENCES [Questions]([Id])
);

CREATE INDEX [IX_ExamQuestions_ExamId] ON [ExamQuestions]([ExamId]);
```

---

## Application Layer

### Commands

```csharp
// CreateQuestionCommand
public record CreateQuestionCommand(
    string Content,
    QuestionType Type,
    Difficulty Difficulty,
    Guid TextbookId,
    Guid ChapterId,
    string? Topic,
    List<QuestionOptionDto>? Options
) : IRequest<Result<QuestionDto>>;

public record QuestionOptionDto(string Text, bool IsCorrect);

// GenerateExamCommand (AI-Assisted)
public record GenerateExamCommand(
    string Title,
    Guid ClassId,
    Guid TextbookId,
    int DurationMinutes,
    ExamMatrix Matrix
) : IRequest<Result<ExamDto>>;

// Handler with AI logic
public class GenerateExamCommandHandler : IRequestHandler<GenerateExamCommand, Result<ExamDto>>
{
    private readonly IQuestionRepository _questionRepo;
    private readonly IExamRepository _examRepo;
    private readonly ContentService.ContentServiceClient _contentClient;

    public async Task<Result<ExamDto>> Handle(GenerateExamCommand request, CancellationToken ct)
    {
        // 1. Validate textbook exists via gRPC
        var textbookValidation = await _contentClient.ValidateTextbookExistsAsync(
            new ValidateTextbookRequest { TextbookId = request.TextbookId.ToString() },
            cancellationToken: ct
        );

        if (!textbookValidation.Exists || textbookValidation.IsDeleted)
        {
            return Result.Failure<ExamDto>("Textbook not found or deleted");
        }

        // 2. Build question pool
        var allQuestions = await _questionRepo.GetByTextbookIdAsync(request.TextbookId, ct);

        // 3. Apply AI selection algorithm
        var selectedQuestions = SelectQuestionsByMatrix(allQuestions, request.Matrix);

        if (selectedQuestions.Count < (request.Matrix.EasyCount + request.Matrix.MediumCount + request.Matrix.HardCount))
        {
            return Result.Failure<ExamDto>("Not enough questions to satisfy exam matrix");
        }

        // 4. Create exam
        var exam = Exam.Create(
            request.Title,
            request.ClassId,
            GetCurrentTeacherId(), // From JWT
            request.DurationMinutes
        );

        int orderIndex = 1;
        foreach (var question in selectedQuestions)
        {
            int points = question.Difficulty switch
            {
                Difficulty.Easy => 1,
                Difficulty.Medium => 2,
                Difficulty.Hard => 3,
                _ => 1
            };

            exam.AddQuestion(question.Id, points, orderIndex++);
        }

        await _examRepo.AddAsync(exam, ct);
        await _examRepo.SaveChangesAsync(ct);

        return Result.Success(ExamDto.FromEntity(exam));
    }

    private List<Question> SelectQuestionsByMatrix(List<Question> pool, ExamMatrix matrix)
    {
        var selected = new List<Question>();
        var random = new Random();

        // Easy questions
        var easyPool = pool.Where(q => q.Difficulty == Difficulty.Easy).ToList();
        selected.AddRange(easyPool.OrderBy(_ => random.Next()).Take(matrix.EasyCount));

        // Medium questions
        var mediumPool = pool.Where(q => q.Difficulty == Difficulty.Medium).ToList();
        selected.AddRange(mediumPool.OrderBy(_ => random.Next()).Take(matrix.MediumCount));

        // Hard questions
        var hardPool = pool.Where(q => q.Difficulty == Difficulty.Hard).ToList();
        selected.AddRange(hardPool.OrderBy(_ => random.Next()).Take(matrix.HardCount));

        return selected;
    }
}

// ExportExamToPdfCommand
public record ExportExamToPdfCommand(Guid ExamId) : IRequest<Result<string>>; // Returns PDF URL

public class ExportExamToPdfCommandHandler : IRequestHandler<ExportExamToPdfCommand, Result<string>>
{
    private readonly IExamRepository _examRepo;
    private readonly IPdfGenerationService _pdfService;
    private readonly IS3StorageService _s3Service;

    public async Task<Result<string>> Handle(ExportExamToPdfCommand request, CancellationToken ct)
    {
        var exam = await _examRepo.GetByIdWithQuestionsAsync(request.ExamId, ct);
        if (exam == null)
        {
            return Result.Failure<string>("Exam not found");
        }

        // Generate PDF
        var pdfStream = await _pdfService.GenerateExamPdfAsync(exam, ct);

        // Upload to S3
        var s3Key = $"generated-exams/{exam.Id}/exam.pdf";
        await _s3Service.UploadFileAsync(pdfStream, s3Key, "application/pdf", ct);

        // Get presigned URL (valid for 24 hours)
        var presignedUrl = await _s3Service.GetPresignedUrlAsync(s3Key, TimeSpan.FromHours(24), ct);

        // Update exam with PDF URL
        exam.Finalize(presignedUrl);
        await _examRepo.UpdateAsync(exam, ct);
        await _examRepo.SaveChangesAsync(ct);

        return Result.Success(presignedUrl);
    }
}
```

---

## PDF Generation Service

```csharp
public interface IPdfGenerationService
{
    Task<Stream> GenerateExamPdfAsync(Exam exam, CancellationToken ct);
}

public class PdfGenerationService : IPdfGenerationService
{
    // Using QuestPDF or IronPDF library
    public async Task<Stream> GenerateExamPdfAsync(Exam exam, CancellationToken ct)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);

                page.Header().Text(exam.Title).FontSize(20).Bold();

                page.Content().Column(column =>
                {
                    column.Item().Text($"Duration: {exam.DurationMinutes} minutes");
                    column.Item().Text($"Total Points: {exam.TotalPoints}");
                    column.Item().PaddingVertical(10);

                    int questionNumber = 1;
                    foreach (var examQuestion in exam.Questions.OrderBy(q => q.OrderIndex))
                    {
                        column.Item().Text($"{questionNumber}. {examQuestion.Question.Content}");

                        if (examQuestion.Question.Type == QuestionType.MCQ)
                        {
                            foreach (var option in examQuestion.Question.Options)
                            {
                                column.Item().Text($"   {option.Text}");
                            }
                        }

                        column.Item().PaddingVertical(5);
                        questionNumber++;
                    }
                });

                page.Footer().AlignCenter().Text("FrogEdu - Smart Classroom Platform");
            });
        });

        var stream = new MemoryStream();
        document.GeneratePdf(stream);
        stream.Position = 0;

        return stream;
    }
}
```

---

## API Endpoints

```csharp
[ApiController]
[Route("api/assessment")]
[Authorize]
public class AssessmentController : ControllerBase
{
    /// <summary>
    /// Create a question
    /// </summary>
    [HttpPost("questions")]
    [Authorize(Policy = "TeacherOnly")]
    public async Task<IActionResult> CreateQuestion(
        CreateQuestionCommand command,
        CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return result.IsSuccess ? CreatedAtAction(nameof(GetQuestion), new { id = result.Value.Id }, result.Value) : BadRequest(result.Error);
    }

    /// <summary>
    /// Get question by ID
    /// </summary>
    [HttpGet("questions/{id:guid}")]
    public async Task<IActionResult> GetQuestion(Guid id, CancellationToken ct)
    {
        var query = new GetQuestionByIdQuery(id);
        var result = await _mediator.Send(query, ct);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    /// <summary>
    /// Generate exam from matrix
    /// </summary>
    [HttpPost("exams/generate")]
    [Authorize(Policy = "TeacherOnly")]
    public async Task<IActionResult> GenerateExam(
        GenerateExamCommand command,
        CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    /// <summary>
    /// Export exam to PDF
    /// </summary>
    [HttpPost("exams/{id:guid}/export")]
    [Authorize(Policy = "TeacherOnly")]
    public async Task<IActionResult> ExportExam(Guid id, CancellationToken ct)
    {
        var command = new ExportExamToPdfCommand(id);
        var result = await _mediator.Send(command, ct);
        return result.IsSuccess ? Ok(new { pdfUrl = result.Value }) : BadRequest(result.Error);
    }

    /// <summary>
    /// Get all exams for a class
    /// </summary>
    [HttpGet("classes/{classId:guid}/exams")]
    public async Task<IActionResult> GetExamsByClass(Guid classId, CancellationToken ct)
    {
        var query = new GetExamsByClassQuery(classId);
        var result = await _mediator.Send(query, ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}
```

---

## Implementation Tasks

### Task 4.1: Domain Layer ⏸️

- [ ] **4.1.1** Create `Question`, `QuestionOption` entities
- [ ] **4.1.2** Create `Exam`, `ExamQuestion` entities
- [ ] **4.1.3** Create `ExamMatrix` value object
- [ ] **4.1.4** Create enums: `QuestionType`, `Difficulty`, `ExamStatus`
- [ ] **4.1.5** Create repositories: `IQuestionRepository`, `IExamRepository`
- [ ] **4.1.6** Write unit tests

### Task 4.2: Exam Generation Algorithm ⏸️

- [ ] **4.2.1** Implement question selection by difficulty
- [ ] **4.2.2** Implement topic-based filtering
- [ ] **4.2.3** Add randomization logic
- [ ] **4.2.4** Add constraints validation (minimum questions)
- [ ] **4.2.5** Write unit tests for algorithm

### Task 4.3: Infrastructure Layer ⏸️

- [ ] **4.3.1** Create `AssessmentDbContext`
- [ ] **4.3.2** Create EF Core configurations
- [ ] **4.3.3** Create initial migration
- [ ] **4.3.4** Implement repositories with eager loading
- [ ] **4.3.5** Write integration tests

### Task 4.4: PDF Generation ⏸️

- [ ] **4.4.1** Install QuestPDF or IronPDF package
- [ ] **4.4.2** Implement `IPdfGenerationService`
- [ ] **4.4.3** Design exam PDF template
- [ ] **4.4.4** Add answer key generation
- [ ] **4.4.5** Test PDF output

### Task 4.5: gRPC Integration ⏸️

- [ ] **4.5.1** Add gRPC client for Content service
- [ ] **4.5.2** Implement textbook validation via gRPC
- [ ] **4.5.3** Add Polly retry policies
- [ ] **4.5.4** Test gRPC calls

### Task 4.6: API Layer ⏸️

- [ ] **4.6.1** Create `AssessmentController`
- [ ] **4.6.2** Implement all endpoints
- [ ] **4.6.3** Add authorization
- [ ] **4.6.4** Write API tests

---

## Testing Strategy

```csharp
public class ExamGenerationTests
{
    [Fact]
    public void SelectQuestionsByMatrix_ShouldReturnCorrectCount()
    {
        // Arrange
        var questions = new List<Question>
        {
            Question.Create("Easy Q1", QuestionType.MCQ, Difficulty.Easy, Guid.NewGuid(), Guid.NewGuid(), null),
            Question.Create("Medium Q1", QuestionType.MCQ, Difficulty.Medium, Guid.NewGuid(), Guid.NewGuid(), null),
            Question.Create("Hard Q1", QuestionType.MCQ, Difficulty.Hard, Guid.NewGuid(), Guid.NewGuid(), null)
        };

        var matrix = ExamMatrix.Create(1, 1, 1, new List<string>());

        // Act
        var selected = SelectQuestionsByMatrix(questions, matrix);

        // Assert
        selected.Should().HaveCount(3);
        selected.Count(q => q.Difficulty == Difficulty.Easy).Should().Be(1);
        selected.Count(q => q.Difficulty == Difficulty.Medium).Should().Be(1);
        selected.Count(q => q.Difficulty == Difficulty.Hard).Should().Be(1);
    }
}
```

---

## Validation Checklist

- [ ] Questions created with options
- [ ] Exam matrix validated
- [ ] Questions selected by difficulty
- [ ] Exam generated successfully
- [ ] Teacher can replace questions
- [ ] PDF generated correctly
- [ ] PDF uploaded to S3
- [ ] Presigned URL returned
- [ ] gRPC call to Content service works

---

**Milestone Status:** Ready for Implementation ✅  
**Blocked By:** 03-content-library-feature  
**Blocking:** None  
**Estimated Completion:** 24-28 hours
