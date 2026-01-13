# Content Service Specification

**Service:** Content Service  
**Port:** 5001  
**Database:** ContentDB  
**Version:** 1.0  
**Status:** Implementation Ready ✅

---

## Table of Contents

1. [Overview](#overview)
2. [Bounded Context](#bounded-context)
3. [Domain Model](#domain-model)
4. [Database Schema](#database-schema)
5. [API Endpoints](#api-endpoints)
6. [gRPC Services](#grpc-services)
7. [Business Rules](#business-rules)
8. [Events Published](#events-published)
9. [Integration Points](#integration-points)
10. [Implementation Tasks](#implementation-tasks)

---

## Overview

The Content Service manages the educational content catalog for the platform, including textbooks, chapters, and pages. It serves as the source of truth for all curriculum-related materials.

### Responsibilities

- ✅ Manage textbook catalog (CRUD operations)
- ✅ Store chapter/page metadata and references to S3 assets
- ✅ Provide search and filtering by grade, subject, chapter
- ✅ Generate presigned URLs for secure asset access
- ✅ Emit events when content is updated
- ✅ Expose gRPC endpoints for internal service-to-service calls

### Technology Stack

- **Framework:** .NET 9, ASP.NET Core
- **Database:** SQL Server (ContentDB)
- **Storage:** AWS S3 (textbook assets)
- **Messaging:** MassTransit + RabbitMQ
- **Communication:** REST API + gRPC

---

## Bounded Context

**Domain:** Educational Content Management

**Ubiquitous Language:**

- **Textbook**: A complete curriculum book for a specific subject and grade
- **Chapter**: A logical division within a textbook (e.g., "Chapter 1: Numbers")
- **Page**: A single page of content (PDF/image stored in S3)
- **Subject**: Academic discipline (Math, Vietnamese, English, Science, etc.)
- **Grade Level**: Primary school grade (1-5)

---

## Domain Model

### Aggregate Roots

#### Textbook

```csharp
public class Textbook : Entity
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public Subject Subject { get; private set; }
    public GradeLevel GradeLevel { get; private set; }
    public string Publisher { get; private set; } = string.Empty;
    public int PublicationYear { get; private set; }
    public string? CoverImageS3Key { get; private set; }
    public bool IsActive { get; private set; }

    private readonly List<Chapter> _chapters = new();
    public IReadOnlyCollection<Chapter> Chapters => _chapters.AsReadOnly();

    public DateTime CreatedAt { get; private set; }
    public string CreatedBy { get; private set; } = string.Empty;
    public DateTime? UpdatedAt { get; private set; }
    public string? UpdatedBy { get; private set; }
    public bool IsDeleted { get; private set; }

    // Factory method
    public static Textbook Create(
        string title,
        Subject subject,
        GradeLevel gradeLevel,
        string publisher,
        int publicationYear,
        string createdBy)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));

        if (publicationYear < 2000 || publicationYear > DateTime.UtcNow.Year)
            throw new ArgumentException("Invalid publication year", nameof(publicationYear));

        return new Textbook
        {
            Id = Guid.NewGuid(),
            Title = title,
            Subject = subject,
            GradeLevel = gradeLevel,
            Publisher = publisher,
            PublicationYear = publicationYear,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy,
            IsDeleted = false
        };
    }

    // Domain methods
    public void UpdateDetails(string title, string publisher, string updatedBy)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));

        Title = title;
        Publisher = publisher;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;

        // Raise domain event
        AddDomainEvent(new TextbookUpdatedEvent(Id, Title, IsDeleted));
    }

    public void AddChapter(Chapter chapter)
    {
        if (_chapters.Any(c => c.ChapterNumber == chapter.ChapterNumber))
            throw new InvalidOperationException($"Chapter {chapter.ChapterNumber} already exists");

        _chapters.Add(chapter);
    }

    public void SoftDelete(string deletedBy)
    {
        IsDeleted = true;
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = deletedBy;

        AddDomainEvent(new TextbookUpdatedEvent(Id, Title, IsDeleted));
    }
}
```

### Entities

#### Chapter

```csharp
public class Chapter : Entity
{
    public Guid Id { get; private set; }
    public Guid TextbookId { get; private set; }
    public int ChapterNumber { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public int OrderIndex { get; private set; }

    private readonly List<Page> _pages = new();
    public IReadOnlyCollection<Page> Pages => _pages.AsReadOnly();

    public DateTime CreatedAt { get; private set; }

    // Navigation property
    public Textbook Textbook { get; private set; } = null!;

    public static Chapter Create(
        Guid textbookId,
        int chapterNumber,
        string title,
        int orderIndex)
    {
        if (chapterNumber < 1)
            throw new ArgumentException("Chapter number must be positive", nameof(chapterNumber));

        return new Chapter
        {
            Id = Guid.NewGuid(),
            TextbookId = textbookId,
            ChapterNumber = chapterNumber,
            Title = title,
            OrderIndex = orderIndex,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void AddPage(Page page)
    {
        if (_pages.Any(p => p.PageNumber == page.PageNumber))
            throw new InvalidOperationException($"Page {page.PageNumber} already exists");

        _pages.Add(page);
    }
}
```

#### Page

```csharp
public class Page : Entity
{
    public Guid Id { get; private set; }
    public Guid ChapterId { get; private set; }
    public int PageNumber { get; private set; }
    public string ContentS3Key { get; private set; } = string.Empty;
    public string ContentType { get; private set; } = string.Empty;
    public string? ContentHash { get; private set; }  // SHA-256 for cache invalidation
    public DateTime CreatedAt { get; private set; }

    // Navigation property
    public Chapter Chapter { get; private set; } = null!;

    public static Page Create(
        Guid chapterId,
        int pageNumber,
        string s3Key,
        string contentType)
    {
        if (pageNumber < 1)
            throw new ArgumentException("Page number must be positive", nameof(pageNumber));

        if (string.IsNullOrWhiteSpace(s3Key))
            throw new ArgumentException("S3 key cannot be empty", nameof(s3Key));

        return new Page
        {
            Id = Guid.NewGuid(),
            ChapterId = chapterId,
            PageNumber = pageNumber,
            ContentS3Key = s3Key,
            ContentType = contentType,
            CreatedAt = DateTime.UtcNow
        };
    }
}
```

### Value Objects

#### Subject

```csharp
public record Subject
{
    public string Name { get; init; }

    private Subject(string name) => Name = name;

    public static Subject Math => new("Mathematics");
    public static Subject Vietnamese => new("Vietnamese");
    public static Subject English => new("English");
    public static Subject Science => new("Science");
    public static Subject Art => new("Art");

    public static Subject Create(string name)
    {
        var validSubjects = new[] { "Mathematics", "Vietnamese", "English", "Science", "Art" };
        if (!validSubjects.Contains(name))
            throw new ArgumentException($"Invalid subject: {name}");

        return new Subject(name);
    }
}
```

#### GradeLevel (Enum)

```csharp
public enum GradeLevel
{
    Grade1 = 1,
    Grade2 = 2,
    Grade3 = 3,
    Grade4 = 4,
    Grade5 = 5
}
```

---

## Database Schema

### Tables

```sql
-- Textbooks table
CREATE TABLE Textbooks (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Title NVARCHAR(200) NOT NULL,
    Subject NVARCHAR(50) NOT NULL,
    GradeLevel INT NOT NULL CHECK (GradeLevel BETWEEN 1 AND 5),
    Publisher NVARCHAR(100),
    PublicationYear INT NOT NULL CHECK (PublicationYear >= 2000),
    CoverImageS3Key NVARCHAR(500),
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy NVARCHAR(100) NOT NULL,
    UpdatedAt DATETIME2,
    UpdatedBy NVARCHAR(100),
    IsDeleted BIT NOT NULL DEFAULT 0,

    INDEX IX_Textbooks_GradeLevel_Subject (GradeLevel, Subject),
    INDEX IX_Textbooks_IsActive (IsActive) WHERE IsActive = 1,
    INDEX IX_Textbooks_CreatedAt (CreatedAt DESC)
);

-- Chapters table
CREATE TABLE Chapters (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    TextbookId UNIQUEIDENTIFIER NOT NULL,
    ChapterNumber INT NOT NULL CHECK (ChapterNumber > 0),
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(1000),
    OrderIndex INT NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),

    FOREIGN KEY (TextbookId) REFERENCES Textbooks(Id) ON DELETE CASCADE,
    UNIQUE (TextbookId, ChapterNumber),
    INDEX IX_Chapters_TextbookId (TextbookId),
    INDEX IX_Chapters_OrderIndex (TextbookId, OrderIndex)
);

-- Pages table
CREATE TABLE Pages (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ChapterId UNIQUEIDENTIFIER NOT NULL,
    PageNumber INT NOT NULL CHECK (PageNumber > 0),
    ContentS3Key NVARCHAR(500) NOT NULL,
    ContentType NVARCHAR(50) NOT NULL,
    ContentHash NVARCHAR(64),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),

    FOREIGN KEY (ChapterId) REFERENCES Chapters(Id) ON DELETE CASCADE,
    UNIQUE (ChapterId, PageNumber),
    INDEX IX_Pages_ChapterId (ChapterId),
    INDEX IX_Pages_ContentS3Key (ContentS3Key)
);
```

### Seed Data

```sql
-- Sample Vietnamese Math textbook (Grade 1)
INSERT INTO Textbooks (Id, Title, Subject, GradeLevel, Publisher, PublicationYear, CreatedBy, IsActive, IsDeleted)
VALUES
('11111111-1111-1111-1111-111111111111', 'Toán 1 - Tập 1', 'Mathematics', 1, 'Nhà xuất bản Giáo dục Việt Nam', 2024, 'system', 1, 0),
('22222222-2222-2222-2222-222222222222', 'Tiếng Việt 1 - Tập 1', 'Vietnamese', 1, 'Nhà xuất bản Giáo dục Việt Nam', 2024, 'system', 1, 0);

-- Sample chapters for Math Grade 1
INSERT INTO Chapters (Id, TextbookId, ChapterNumber, Title, Description, OrderIndex, CreatedAt)
VALUES
('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', '11111111-1111-1111-1111-111111111111', 1, 'Đếm và viết các số từ 1 đến 5', 'Học đếm và viết các số cơ bản', 1, GETUTCDATE()),
('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', '11111111-1111-1111-1111-111111111111', 2, 'Phép cộng trong phạm vi 5', 'Làm quen với phép cộng đơn giản', 2, GETUTCDATE());
```

---

## API Endpoints

### REST API

Base URL: `https://api.frogedu.com/api/content`

#### GET /textbooks

List all textbooks with pagination and filtering.

**Query Parameters:**

- `gradeLevel` (optional, int): Filter by grade (1-5)
- `subject` (optional, string): Filter by subject
- `page` (optional, int, default: 1): Page number
- `pageSize` (optional, int, default: 20): Items per page

**Response:**

```json
{
  "items": [
    {
      "id": "11111111-1111-1111-1111-111111111111",
      "title": "Toán 1 - Tập 1",
      "subject": "Mathematics",
      "gradeLevel": 1,
      "publisher": "Nhà xuất bản Giáo dục Việt Nam",
      "publicationYear": 2024,
      "coverImageUrl": "https://s3.amazonaws.com/...",
      "chapterCount": 12
    }
  ],
  "totalCount": 50,
  "page": 1,
  "pageSize": 20
}
```

#### GET /textbooks/{id}

Get textbook details including chapters.

**Response:**

```json
{
  "id": "11111111-1111-1111-1111-111111111111",
  "title": "Toán 1 - Tập 1",
  "subject": "Mathematics",
  "gradeLevel": 1,
  "publisher": "Nhà xuất bản Giáo dục Việt Nam",
  "publicationYear": 2024,
  "coverImageUrl": "https://s3.amazonaws.com/...",
  "chapters": [
    {
      "id": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
      "chapterNumber": 1,
      "title": "Đếm và viết các số từ 1 đến 5",
      "description": "Học đếm và viết các số cơ bản",
      "pageCount": 8
    }
  ]
}
```

#### POST /textbooks

Create new textbook (Admin only).

**Authorization:** `[Authorize(Policy = "AdminOnly")]`

**Request:**

```json
{
  "title": "Toán 2 - Tập 1",
  "subject": "Mathematics",
  "gradeLevel": 2,
  "publisher": "Nhà xuất bản Giáo dục Việt Nam",
  "publicationYear": 2024
}
```

**Response:** `201 Created`

#### PUT /textbooks/{id}

Update textbook details.

**Authorization:** `[Authorize(Policy = "AdminOnly")]`

#### DELETE /textbooks/{id}

Soft delete textbook.

**Authorization:** `[Authorize(Policy = "AdminOnly")]`

#### GET /chapters

List chapters by textbook.

**Query Parameters:**

- `textbookId` (required, guid): Textbook ID

#### GET /pages/{id}/url

Get presigned URL for page asset.

**Response:**

```json
{
  "url": "https://edu-classroom-assets.s3.amazonaws.com/textbooks/...?X-Amz-Expires=900&...",
  "expiresAt": "2026-01-13T15:30:00Z"
}
```

---

## gRPC Services

### content.proto

```protobuf
syntax = "proto3";

package content;

service ContentService {
  rpc GetChapter(ChapterRequest) returns (ChapterResponse);
  rpc GetPages(PagesRequest) returns (PagesResponse);
  rpc GetTextbookInfo(TextbookRequest) returns (TextbookResponse);
}

message ChapterRequest {
  string chapter_id = 1;
}

message ChapterResponse {
  string id = 1;
  string textbook_id = 2;
  int32 chapter_number = 3;
  string title = 4;
  string description = 5;
}

message PagesRequest {
  string chapter_id = 1;
}

message PagesResponse {
  repeated Page pages = 1;
}

message Page {
  string id = 1;
  int32 page_number = 2;
  string content_s3_key = 3;
  string content_type = 4;
}

message TextbookRequest {
  string textbook_id = 1;
}

message TextbookResponse {
  string id = 1;
  string title = 2;
  string subject = 3;
  int32 grade_level = 4;
}
```

---

## Business Rules

### Validation Rules

- [ ] **BR-001:** Textbook title must be 5-200 characters
- [ ] **BR-002:** Grade level must be between 1 and 5
- [ ] **BR-003:** Publication year must be >= 2000 and <= current year
- [ ] **BR-004:** Subject must be one of: Mathematics, Vietnamese, English, Science, Art
- [ ] **BR-005:** Chapter numbers must be unique within a textbook
- [ ] **BR-006:** Page numbers must be unique within a chapter
- [ ] **BR-007:** S3 object keys must follow pattern: `textbooks/{textbookId}/pages/{pageId}.{ext}`

### Business Logic Rules

- [ ] **BR-101:** Textbooks cannot be hard deleted (soft delete only)
- [ ] **BR-102:** Deleting a textbook marks all chapters and pages as inactive
- [ ] **BR-103:** Presigned URLs expire after 15 minutes
- [ ] **BR-104:** Only PDF and image files (png, jpg, jpeg) are allowed for pages
- [ ] **BR-105:** Maximum file size for page uploads: 10 MB
- [ ] **BR-106:** When textbook is updated, emit `TextbookUpdatedEvent` for denormalization

---

## Events Published

### TextbookUpdatedEvent

Published when a textbook is created, updated, or deleted.

```csharp
public record TextbookUpdatedEvent : IEvent
{
    public Guid TextbookId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Subject { get; init; } = string.Empty;
    public int GradeLevel { get; init; }
    public bool IsDeleted { get; init; }
    public DateTime OccurredAt { get; init; }
    public Guid CorrelationId { get; init; }
}
```

**Consumers:**

- Assessment Service (updates denormalized textbook references in questions)

---

## Integration Points

### Outbound Dependencies

**AWS S3:**

- Upload textbook assets (covers, page PDFs/images)
- Generate presigned URLs for secure access
- Delete assets when textbooks are removed

**RabbitMQ:**

- Publish `TextbookUpdatedEvent` to exchange `content.textbook.updated`

### Inbound Dependencies

**API Gateway:**

- Receives HTTP requests from frontend
- JWT validation performed at gateway level

---

## Implementation Tasks

### Milestone 1: Domain Layer Implementation

**Status:** ⚪ Not Started

#### Task 1.1: Create Domain Entities

- [ ] Implement `Textbook` aggregate root with factory method
- [ ] Implement `Chapter` entity with validation
- [ ] Implement `Page` entity
- [ ] Create `Subject` value object with validation
- [ ] Create `GradeLevel` enum
- [ ] Add unit tests for entity creation and business rules

**Validation:**

- [ ] All entities enforce invariants in constructors
- [ ] Domain methods update state correctly
- [ ] Unit tests achieve 90%+ coverage

**Estimated Effort:** 4 hours

---

#### Task 1.2: Create Domain Events

- [ ] Implement `TextbookUpdatedEvent`
- [ ] Implement `TextbookCreatedEvent`
- [ ] Add `IDomainEvent` interface to Shared.Kernel
- [ ] Create domain event dispatcher

**Validation:**

- [ ] Events include all necessary data for consumers
- [ ] Events immutable (record types)

**Estimated Effort:** 2 hours

---

#### Task 1.3: Create Repository Interfaces

- [ ] Create `ITextbookRepository` with CRUD methods
- [ ] Create `IChapterRepository`
- [ ] Create `IPageRepository`
- [ ] Add specification methods (e.g., `GetByGradeAndSubject`)

**Validation:**

- [ ] Interfaces follow repository pattern
- [ ] No implementation details leak into domain layer

**Estimated Effort:** 2 hours

---

### Milestone 2: Application Layer Implementation

**Status:** ⚪ Not Started

#### Task 2.1: CQRS Commands

- [ ] Create `CreateTextbookCommand` with FluentValidation
- [ ] Create `UpdateTextbookCommand`
- [ ] Create `DeleteTextbookCommand` (soft delete)
- [ ] Create `CreateChapterCommand`
- [ ] Create `UploadPageCommand`

**Validation:**

- [ ] All commands have validators
- [ ] Validators test all business rules

**Estimated Effort:** 6 hours

---

#### Task 2.2: CQRS Queries

- [ ] Create `GetTextbooksQuery` (with pagination)
- [ ] Create `GetTextbookByIdQuery`
- [ ] Create `GetChaptersByTextbookQuery`
- [ ] Create `GetPagesByChapterQuery`
- [ ] Create `GetPagePresignedUrlQuery`

**Validation:**

- [ ] Queries use `.AsNoTracking()` for performance
- [ ] Pagination implemented correctly

**Estimated Effort:** 4 hours

---

#### Task 2.3: MediatR Handlers

- [ ] Implement handlers for all commands
- [ ] Implement handlers for all queries
- [ ] Add AutoMapper profiles for DTOs
- [ ] Create DTOs (TextbookDto, ChapterDto, PageDto)

**Validation:**

- [ ] Handlers follow single responsibility principle
- [ ] Unit tests mock repository dependencies

**Estimated Effort:** 8 hours

---

### Milestone 3: Infrastructure Layer Implementation

**Status:** ⚪ Not Started

#### Task 3.1: EF Core Configuration

- [ ] Create `ContentDbContext` with entity configurations
- [ ] Configure fluent API for relationships
- [ ] Add global query filters (IsDeleted)
- [ ] Configure indexes for performance

**Validation:**

- [ ] DbContext registered in DI container
- [ ] Lazy loading disabled

**Estimated Effort:** 4 hours

---

#### Task 3.2: Repository Implementation

- [ ] Implement `TextbookRepository`
- [ ] Implement `ChapterRepository`
- [ ] Implement `PageRepository`
- [ ] Add unit of work pattern

**Validation:**

- [ ] Repositories use async/await
- [ ] CancellationToken passed to all methods

**Estimated Effort:** 6 hours

---

#### Task 3.3: Database Migrations

- [ ] Create initial migration (`InitialCreate`)
- [ ] Run migration on local SQL Server
- [ ] Add seed data migration
- [ ] Test rollback scenarios

**Validation:**

- [ ] Migration generates correct SQL
- [ ] Seed data populates successfully

**Estimated Effort:** 3 hours

---

#### Task 3.4: S3 Integration

- [ ] Create `IS3StorageService` interface
- [ ] Implement `S3StorageService` (upload, download, presigned URLs)
- [ ] Add S3 configuration (bucket name, region)
- [ ] Test with LocalStack for local development

**Validation:**

- [ ] Upload/download works with LocalStack
- [ ] Presigned URLs expire correctly

**Estimated Effort:** 5 hours

---

#### Task 3.5: MassTransit Integration

- [ ] Configure MassTransit with RabbitMQ
- [ ] Create event publishers
- [ ] Add retry policies and error handling
- [ ] Test event publishing

**Validation:**

- [ ] Events published to correct exchange
- [ ] Dead-letter queue configured

**Estimated Effort:** 4 hours

---

### Milestone 4: API Layer Implementation

**Status:** ⚪ Not Started

#### Task 4.1: REST Controllers

- [ ] Create `TextbooksController` with CRUD endpoints
- [ ] Create `ChaptersController`
- [ ] Create `PagesController`
- [ ] Add authorization attributes
- [ ] Add XML documentation comments

**Validation:**

- [ ] Swagger UI displays all endpoints
- [ ] Authorization enforced correctly

**Estimated Effort:** 6 hours

---

#### Task 4.2: gRPC Service

- [ ] Create `content.proto` file
- [ ] Implement `ContentGrpcService`
- [ ] Configure gRPC in `Program.cs`
- [ ] Test with gRPC client

**Validation:**

- [ ] gRPC service accessible on port 5011
- [ ] Other services can call gRPC methods

**Estimated Effort:** 4 hours

---

#### Task 4.3: Middleware & Filters

- [ ] Add global exception handler
- [ ] Add request logging middleware
- [ ] Add response compression
- [ ] Add CORS configuration

**Validation:**

- [ ] Errors return ProblemDetails format
- [ ] All requests logged with correlation ID

**Estimated Effort:** 3 hours

---

#### Task 4.4: Dependency Injection Setup

- [ ] Configure all services in `Program.cs`
- [ ] Add health checks
- [ ] Configure Swagger/OpenAPI
- [ ] Add JWT authentication

**Validation:**

- [ ] Health check returns 200 OK
- [ ] Unauthorized requests return 401

**Estimated Effort:** 3 hours

---

### Milestone 5: Testing

**Status:** ⚪ Not Started

#### Task 5.1: Unit Tests

- [ ] Write unit tests for domain entities (90% coverage)
- [ ] Write unit tests for command handlers
- [ ] Write unit tests for query handlers
- [ ] Write unit tests for validators

**Validation:**

- [ ] All tests pass
- [ ] Coverage >= 80%

**Estimated Effort:** 8 hours

---

#### Task 5.2: Integration Tests

- [ ] Setup WebApplicationFactory for testing
- [ ] Write integration tests for all endpoints
- [ ] Test with in-memory database
- [ ] Test S3 integration with LocalStack

**Validation:**

- [ ] All integration tests pass
- [ ] Tests clean up after execution

**Estimated Effort:** 6 hours

---

### Total Estimated Effort

**Total:** ~78 hours (~2 weeks for 1 developer)

---

## Notes for AI Agents

Before starting any task:

1. [ ] Read this entire spec file
2. [ ] Review [Architecture Overview](00-architecture-overview.md)
3. [ ] Review [Shared Kernel](07-shared-kernel.md)
4. [ ] Check prerequisite tasks are complete
5. [ ] Set up local development environment

After completing a task:

1. [ ] Run all tests and verify they pass
2. [ ] Update checkbox to `[x]`
3. [ ] Commit with descriptive message
4. [ ] Update this spec if implementation differs
