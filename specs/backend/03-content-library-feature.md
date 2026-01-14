# Milestone 3: Content Library Feature

**Feature:** Textbook Catalog & Content Management  
**Epic:** Content Management  
**Priority:** P1 (High)  
**Estimated Effort:** 20-24 hours  
**Status:** 🔄 Ready for Implementation

---

## Overview

Manage textbook catalog with chapters, pages, and S3-stored assets. Provides gRPC API for other services.

### User Stories

- **US-3.1:** Teacher browses textbook catalog filtered by subject/grade
- **US-3.2:** Teacher views textbook chapters and pages
- **US-3.3:** Admin uploads textbook PDFs to S3
- **US-3.4:** Student accesses textbook content via presigned URLs
- **US-3.5:** Assessment service queries textbook data via gRPC

---

## Domain Model

### Entities

#### `Textbook` (Aggregate Root)

```csharp
public class Textbook : Entity
{
    public TextbookTitle Title { get; private set; }
    public Subject Subject { get; private set; }
    public GradeLevel GradeLevel { get; private set; }
    public string Publisher { get; private set; }
    public int PublicationYear { get; private set; }
    public string CoverImageUrl { get; private set; } // S3 URL
    public string Description { get; private set; }

    private readonly List<Chapter> _chapters = new();
    public IReadOnlyList<Chapter> Chapters => _chapters.AsReadOnly();

    public static Textbook Create(
        string title,
        string subject,
        int gradeLevel,
        string publisher,
        int year,
        string description)
    {
        return new Textbook
        {
            Title = TextbookTitle.Create(title),
            Subject = Subject.Create(subject),
            GradeLevel = GradeLevel.Create(gradeLevel),
            Publisher = publisher,
            PublicationYear = year,
            Description = description
        };
    }

    public void AddChapter(string title, int orderIndex)
    {
        var chapter = Chapter.Create(Id, title, orderIndex);
        _chapters.Add(chapter);
        MarkAsUpdated();
    }
}
```

#### `Chapter` (Entity)

```csharp
public class Chapter : Entity
{
    public Guid TextbookId { get; private set; }
    public string Title { get; private set; }
    public int OrderIndex { get; private set; }

    private readonly List<Page> _pages = new();
    public IReadOnlyList<Page> Pages => _pages.AsReadOnly();

    public static Chapter Create(Guid textbookId, string title, int orderIndex)
    {
        return new Chapter
        {
            TextbookId = textbookId,
            Title = title,
            OrderIndex = orderIndex
        };
    }

    public void AddPage(int pageNumber, string content, string? imageUrl)
    {
        var page = Page.Create(Id, pageNumber, content, imageUrl);
        _pages.Add(page);
    }
}
```

#### `Page` (Entity)

```csharp
public class Page : Entity
{
    public Guid ChapterId { get; private set; }
    public int PageNumber { get; private set; }
    public string Content { get; private set; } // Text content or markdown
    public string? ImageUrl { get; private set; } // S3 URL for page image
    public string? S3Key { get; private set; } // S3 key for direct access

    public static Page Create(Guid chapterId, int pageNumber, string content, string? imageUrl)
    {
        return new Page
        {
            ChapterId = chapterId,
            PageNumber = pageNumber,
            Content = content ?? string.Empty,
            ImageUrl = imageUrl
        };
    }
}
```

---

## Database Schema (ContentDB)

```sql
CREATE TABLE [dbo].[Textbooks] (
    [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [Title] NVARCHAR(200) NOT NULL,
    [Subject] NVARCHAR(50) NOT NULL,
    [GradeLevel] INT NOT NULL,
    [Publisher] NVARCHAR(100) NOT NULL,
    [PublicationYear] INT NOT NULL,
    [CoverImageUrl] NVARCHAR(512) NULL,
    [Description] NVARCHAR(MAX) NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] DATETIME2 NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,

    CONSTRAINT [CK_Textbooks_GradeLevel] CHECK ([GradeLevel] BETWEEN 1 AND 12)
);

CREATE INDEX [IX_Textbooks_Subject_GradeLevel] ON [Textbooks]([Subject], [GradeLevel])
    WHERE [IsDeleted] = 0;

CREATE TABLE [dbo].[Chapters] (
    [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [TextbookId] UNIQUEIDENTIFIER NOT NULL,
    [Title] NVARCHAR(200) NOT NULL,
    [OrderIndex] INT NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [IsDeleted] BIT NOT NULL DEFAULT 0,

    CONSTRAINT [FK_Chapters_Textbooks] FOREIGN KEY ([TextbookId])
        REFERENCES [Textbooks]([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_Chapters_TextbookId_OrderIndex] ON [Chapters]([TextbookId], [OrderIndex]);

CREATE TABLE [dbo].[Pages] (
    [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [ChapterId] UNIQUEIDENTIFIER NOT NULL,
    [PageNumber] INT NOT NULL,
    [Content] NVARCHAR(MAX) NOT NULL,
    [ImageUrl] NVARCHAR(512) NULL,
    [S3Key] NVARCHAR(256) NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [IsDeleted] BIT NOT NULL DEFAULT 0,

    CONSTRAINT [FK_Pages_Chapters] FOREIGN KEY ([ChapterId])
        REFERENCES [Chapters]([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_Pages_ChapterId_PageNumber] ON [Pages]([ChapterId], [PageNumber]);
```

---

## Application Layer

### Commands

```csharp
// CreateTextbookCommand
public record CreateTextbookCommand(
    string Title,
    string Subject,
    int GradeLevel,
    string Publisher,
    int PublicationYear,
    string Description
) : IRequest<Result<TextbookDto>>;

// UploadTextbookCoverCommand
public record UploadTextbookCoverCommand(
    Guid TextbookId,
    Stream FileStream,
    string FileName
) : IRequest<Result<string>>;

// AddChapterCommand
public record AddChapterCommand(
    Guid TextbookId,
    string Title,
    int OrderIndex
) : IRequest<Result<ChapterDto>>;
```

### Queries

```csharp
// GetTextbooksQuery - with filtering
public record GetTextbooksQuery(
    string? Subject = null,
    int? GradeLevel = null
) : IRequest<Result<List<TextbookDto>>>;

// GetTextbookByIdQuery - includes chapters and pages
public record GetTextbookByIdQuery(Guid TextbookId) : IRequest<Result<TextbookDetailDto>>;

// GetChapterQuery
public record GetChapterQuery(Guid ChapterId) : IRequest<Result<ChapterDetailDto>>;
```

---

## gRPC Service Definition

### Proto File: `content_service.proto`

```protobuf
syntax = "proto3";

package content;

service ContentService {
  rpc GetTextbook (GetTextbookRequest) returns (TextbookResponse);
  rpc GetChapter (GetChapterRequest) returns (ChapterResponse);
  rpc GetPages (GetPagesRequest) returns (PagesResponse);
  rpc ValidateTextbookExists (ValidateTextbookRequest) returns (ValidationResponse);
}

message GetTextbookRequest {
  string id = 1;
}

message TextbookResponse {
  string id = 1;
  string title = 2;
  string subject = 3;
  int32 grade_level = 4;
  bool is_deleted = 5;
}

message GetChapterRequest {
  string id = 1;
}

message ChapterResponse {
  string id = 1;
  string textbook_id = 2;
  string title = 3;
  int32 order_index = 4;
}

message GetPagesRequest {
  string chapter_id = 1;
}

message PagesResponse {
  repeated PageInfo pages = 1;
}

message PageInfo {
  string id = 1;
  int32 page_number = 2;
  string content = 3;
  string image_url = 4;
}

message ValidateTextbookRequest {
  string textbook_id = 1;
}

message ValidationResponse {
  bool exists = 1;
  bool is_deleted = 2;
}
```

### gRPC Service Implementation

```csharp
public class ContentGrpcService : ContentService.ContentServiceBase
{
    private readonly ITextbookRepository _repository;
    private readonly IS3StorageService _s3Service;

    public override async Task<TextbookResponse> GetTextbook(
        GetTextbookRequest request,
        ServerCallContext context)
    {
        var textbook = await _repository.GetByIdAsync(
            Guid.Parse(request.Id),
            context.CancellationToken);

        if (textbook == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Textbook not found"));
        }

        return new TextbookResponse
        {
            Id = textbook.Id.ToString(),
            Title = textbook.Title.Value,
            Subject = textbook.Subject.Value,
            GradeLevel = textbook.GradeLevel.Value,
            IsDeleted = textbook.IsDeleted
        };
    }

    public override async Task<ValidationResponse> ValidateTextbookExists(
        ValidateTextbookRequest request,
        ServerCallContext context)
    {
        var textbook = await _repository.GetByIdAsync(
            Guid.Parse(request.TextbookId),
            context.CancellationToken);

        return new ValidationResponse
        {
            Exists = textbook != null,
            IsDeleted = textbook?.IsDeleted ?? false
        };
    }
}
```

---

## API Endpoints

```csharp
[ApiController]
[Route("api/content/textbooks")]
[Authorize]
public class TextbooksController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Get all textbooks (with optional filters)
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<TextbookDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? subject,
        [FromQuery] int? gradeLevel,
        CancellationToken ct)
    {
        var query = new GetTextbooksQuery(subject, gradeLevel);
        var result = await _mediator.Send(query, ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    /// <summary>
    /// Get textbook by ID with chapters
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(TextbookDetailDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var query = new GetTextbookByIdQuery(id);
        var result = await _mediator.Send(query, ct);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    /// <summary>
    /// Create new textbook (Admin only)
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(TextbookDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        CreateTextbookCommand command,
        CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value)
            : BadRequest(result.Error);
    }

    /// <summary>
    /// Upload textbook cover image
    /// </summary>
    [HttpPost("{id:guid}/cover")]
    [Authorize(Policy = "AdminOnly")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadCover(
        Guid id,
        IFormFile file,
        CancellationToken ct)
    {
        await using var stream = file.OpenReadStream();
        var command = new UploadTextbookCoverCommand(id, stream, file.FileName);
        var result = await _mediator.Send(command, ct);
        return result.IsSuccess ? Ok(new { coverUrl = result.Value }) : BadRequest(result.Error);
    }
}
```

---

## AWS S3 Integration

### Bucket Structure

```
frogedu-assets-dev/
└── textbooks/
    └── {textbookId}/
        ├── cover.jpg
        └── pages/
            ├── page-001.png
            ├── page-002.png
            └── ...
```

### Upload Implementation

```csharp
public async Task<Result<string>> UploadTextbookPageAsync(
    Guid textbookId,
    int pageNumber,
    Stream fileStream,
    string contentType,
    CancellationToken ct)
{
    var s3Key = $"textbooks/{textbookId}/pages/page-{pageNumber:D3}.png";

    await _s3Service.UploadFileAsync(fileStream, s3Key, contentType, ct);

    // Generate presigned URL (valid for 1 hour)
    var presignedUrl = await _s3Service.GetPresignedUrlAsync(
        s3Key,
        TimeSpan.FromHours(1),
        ct
    );

    return Result.Success(presignedUrl);
}
```

---

## Implementation Tasks

### Task 3.1: Domain Layer ⏸️

- [ ] **3.1.1** Create `Textbook`, `Chapter`, `Page` entities
- [ ] **3.1.2** Create value objects: `TextbookTitle`
- [ ] **3.1.3** Create domain events
- [ ] **3.1.4** Create `ITextbookRepository`, `IChapterRepository`
- [ ] **3.1.5** Write unit tests

### Task 3.2: Infrastructure Layer ⏸️

- [ ] **3.2.1** Create `ContentDbContext`
- [ ] **3.2.2** Create EF Core configurations
- [ ] **3.2.3** Create initial migration
- [ ] **3.2.4** Implement repositories
- [ ] **3.2.5** Configure S3 service for textbook uploads
- [ ] **3.2.6** Write integration tests

### Task 3.3: Application Layer ⏸️

- [ ] **3.3.1** Create CQRS commands (Create, Upload, AddChapter)
- [ ] **3.3.2** Create queries (GetAll, GetById, GetChapter)
- [ ] **3.3.3** Create DTOs (TextbookDto, ChapterDto, PageDto)
- [ ] **3.3.4** Add validators
- [ ] **3.3.5** Write unit tests for handlers

### Task 3.4: gRPC Service ⏸️

- [ ] **3.4.1** Create `content_service.proto`
- [ ] **3.4.2** Generate C# classes: `dotnet grpc-tool`
- [ ] **3.4.3** Implement `ContentGrpcService`
- [ ] **3.4.4** Configure gRPC server in Program.cs
- [ ] **3.4.5** Test with grpcurl or Postman

### Task 3.5: API Layer ⏸️

- [ ] **3.5.1** Create `TextbooksController`
- [ ] **3.5.2** Create `ChaptersController`
- [ ] **3.5.3** Implement all endpoints
- [ ] **3.5.4** Add authorization policies
- [ ] **3.5.5** Write API integration tests

---

## Testing Strategy

### Unit Tests

```csharp
public class TextbookTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateTextbook()
    {
        // Arrange & Act
        var textbook = Textbook.Create(
            "Math Grade 5",
            "Math",
            5,
            "Education Press",
            2024,
            "Primary math textbook"
        );

        // Assert
        textbook.Should().NotBeNull();
        textbook.Title.Value.Should().Be("Math Grade 5");
        textbook.GradeLevel.Value.Should().Be(5);
    }

    [Fact]
    public void AddChapter_ShouldIncreaseChapterCount()
    {
        // Arrange
        var textbook = Textbook.Create("Math", "Math", 5, "Publisher", 2024, "Desc");

        // Act
        textbook.AddChapter("Chapter 1: Numbers", 1);

        // Assert
        textbook.Chapters.Should().HaveCount(1);
    }
}
```

### gRPC Client Test

```csharp
public class ContentGrpcClientTests
{
    [Fact]
    public async Task GetTextbook_ShouldReturnTextbookData()
    {
        // Arrange
        var channel = GrpcChannel.ForAddress("http://localhost:5001");
        var client = new ContentService.ContentServiceClient(channel);

        // Act
        var response = await client.GetTextbookAsync(new GetTextbookRequest
        {
            Id = testTextbookId.ToString()
        });

        // Assert
        response.Should().NotBeNull();
        response.Title.Should().NotBeEmpty();
    }
}
```

---

## Validation Checklist

- [ ] Textbooks filterable by subject/grade
- [ ] Chapters ordered correctly
- [ ] Pages linked to chapters
- [ ] S3 uploads working for cover images
- [ ] S3 uploads working for page images
- [ ] Presigned URLs generated correctly
- [ ] gRPC service accessible from other services
- [ ] Textbook validation via gRPC works

---

## gRPC Client Configuration (for Assessment Service)

```csharp
// In Assessment.API/Program.cs
builder.Services.AddGrpcClient<ContentService.ContentServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["Services:Content:GrpcUrl"]!);
})
.AddPolicyHandler(GetRetryPolicy())
.AddPolicyHandler(GetCircuitBreakerPolicy());

// Usage in Assessment service
public class QuestionService
{
    private readonly ContentService.ContentServiceClient _contentClient;

    public async Task<bool> ValidateTextbookAsync(Guid textbookId, CancellationToken ct)
    {
        var response = await _contentClient.ValidateTextbookExistsAsync(
            new ValidateTextbookRequest { TextbookId = textbookId.ToString() },
            cancellationToken: ct
        );

        return response.Exists && !response.IsDeleted;
    }
}
```

---

**Milestone Status:** Ready for Implementation ✅  
**Blocked By:** 00-foundation-milestone  
**Blocking:** 04-assessment-feature, 05-ai-tutor-feature  
**Estimated Completion:** 20-24 hours
