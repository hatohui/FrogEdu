# Backend Technical Specification: Edu-AI Classroom

**Version:** 2.0  
**Last Updated:** January 13, 2026  
**Status:** Implementation Ready ✅

---

## Table of Contents

1. [Overview](#1-overview)
2. [Global Architecture](#2-global-architecture)
3. [Service Boundaries](#3-service-boundaries)
4. [Data Models & Schemas](#4-data-models--schemas)
5. [Infrastructure Components](#5-infrastructure-components)
6. [Technical Standards](#6-technical-standards)
7. [Development Guidelines](#7-development-guidelines)
8. [Security & Authentication](#8-security--authentication)
9. [Testing Strategy](#9-testing-strategy)
10. [Milestones & Tasks](#10-milestones--tasks)

---

## 1. Overview

The backend is a distributed microservices system built on **.NET 9** using **Clean Architecture** and **Domain-Driven Design (DDD)**. It powers the Edu-AI Classroom platform for Vietnamese primary education.

### Key Architectural Decisions

- **Pattern:** Microservices (Service-oriented, event-driven)
- **Database Strategy:** Database-per-Service (SQL Server)
- **Communication:** Asynchronous (MassTransit/RabbitMQ) + Synchronous (gRPC)
- **Authentication:** AWS Cognito (OAuth 2.0 / OpenID Connect)
- **Storage:** AWS S3 (presigned URLs)
- **Observability:** OpenTelemetry + Serilog
- **API Gateway:** YARP (Yet Another Reverse Proxy)

### Non-Functional Requirements

- **Availability:** 99.5% uptime (scheduled maintenance windows allowed)
- **Performance:**
  - API response time: < 500ms (p95)
  - Exam generation: < 10 seconds
  - AI Tutor response: < 5 seconds (streaming)
- **Scalability:** Support 10,000 concurrent users per service
- **Data Retention:** 7 years (compliance with Vietnamese education regulations)

---

## 2. Global Architecture

### System Context Diagram

```
┌─────────────┐
│   Frontend  │ (React/Vite)
│  (Cloudflare│
│    Pages)   │
└──────┬──────┘
       │ HTTPS
       ↓
┌─────────────────────┐
│   API Gateway       │ (YARP / AWS API Gateway)
│  - JWT Validation   │
│  - Rate Limiting    │
│  - Routing          │
└──────┬──────────────┘
       │
       ├──→ Content Service      (Port 5001)
       ├──→ Assessment Service   (Port 5002)
       ├──→ User Service         (Port 5003)
       └──→ AI Orchestrator      (Port 5004)
              │
              ↓
       ┌─────────────┐
       │  RabbitMQ   │ (Event Bus)
       │  (MassTransit)
       └─────────────┘
              │
              ↓
       ┌─────────────┐
       │ SQL Server  │ (4 databases)
       └─────────────┘
       ┌─────────────┐
       │   AWS S3    │ (Asset storage)
       └─────────────┘
```

---

## 3. Service Boundaries

Each service is **autonomous** with its own:

- Database (no shared databases)
- Deployment pipeline
- Scaling configuration
- Team ownership

### Service Catalog

| Service         | Responsibility                       | Port | Database     | External Dependencies      |
| --------------- | ------------------------------------ | ---- | ------------ | -------------------------- |
| Content         | Textbook management, lesson assets   | 5001 | ContentDB    | S3                         |
| Assessment      | Question banks, exam generation      | 5002 | AssessmentDB | S3, Content (gRPC)         |
| User            | Profile management, class enrollment | 5003 | UserDB       | Cognito                    |
| AI Orchestrator | LLM integration, RAG pipeline        | 5004 | AiContextDB  | OpenAI API, Content (gRPC) |

---

### 3.1 Content Service

**Bounded Context:** Educational content and materials

**Responsibilities:**

- ✅ Manage textbook catalog (CRUD operations)
- ✅ Store chapter/page metadata (references to S3 assets)
- ✅ Provide search/filtering by grade, subject, chapter
- ✅ Generate presigned URLs for secure asset access
- ✅ Emit `ContentUpdated` events when textbooks change

**Domain Entities:**

- `Textbook` (Aggregate Root)
- `Chapter` (Entity, owned by Textbook)
- `Page` (Entity, owned by Chapter)
- `Subject` (Value Object)
- `GradeLevel` (Enum: Grade1-Grade5)

**Exposed APIs:**

```csharp
// REST Endpoints
GET    /api/content/textbooks                 // List all textbooks
GET    /api/content/textbooks/{id}            // Get textbook details
POST   /api/content/textbooks                 // Create textbook (Admin only)
PUT    /api/content/textbooks/{id}            // Update textbook
DELETE /api/content/textbooks/{id}            // Soft delete
GET    /api/content/chapters?textbookId={id}  // List chapters
GET    /api/content/pages/{id}/url            // Get presigned URL for page asset

// gRPC Services (internal)
rpc GetChapter(ChapterRequest) returns (ChapterResponse);
rpc GetPages(PagesRequest) returns (PagesResponse);
```

**Database Schema:**

```sql
-- Textbooks table
CREATE TABLE Textbooks (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Title NVARCHAR(200) NOT NULL,
    Subject NVARCHAR(50) NOT NULL,         -- Math, Vietnamese, English, etc.
    GradeLevel INT NOT NULL,                -- 1-5
    Publisher NVARCHAR(100),
    PublicationYear INT,
    CoverImageUrl NVARCHAR(500),           -- S3 URL
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy NVARCHAR(100),
    UpdatedAt DATETIME2,
    UpdatedBy NVARCHAR(100),
    IsDeleted BIT NOT NULL DEFAULT 0,
    INDEX IX_Textbooks_GradeLevel_Subject (GradeLevel, Subject)
);

-- Chapters table
CREATE TABLE Chapters (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    TextbookId UNIQUEIDENTIFIER NOT NULL,
    ChapterNumber INT NOT NULL,
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(1000),
    OrderIndex INT NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (TextbookId) REFERENCES Textbooks(Id) ON DELETE CASCADE,
    INDEX IX_Chapters_TextbookId (TextbookId)
);

-- Pages table
CREATE TABLE Pages (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ChapterId UNIQUEIDENTIFIER NOT NULL,
    PageNumber INT NOT NULL,
    ContentS3Key NVARCHAR(500) NOT NULL,   -- S3 object key
    ContentType NVARCHAR(50) NOT NULL,      -- application/pdf, image/png
    ContentHash NVARCHAR(64),               -- SHA-256 for cache invalidation
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (ChapterId) REFERENCES Chapters(Id) ON DELETE CASCADE,
    INDEX IX_Pages_ChapterId (ChapterId)
);
```

**Business Rules:**

- [ ] Textbooks cannot be deleted if referenced by active exams
- [ ] Chapter numbers must be unique within a textbook
- [ ] Page numbers must be unique within a chapter
- [ ] S3 assets must be validated (file size < 10MB, allowed MIME types)

---

### 3.2 Assessment Service

**Bounded Context:** Examination and evaluation

**Responsibilities:**

- ✅ Manage question bank (MCQ, Essay, True/False)
- ✅ Implement exam matrix logic (difficulty distribution)
- ✅ Generate exam papers based on teacher-defined criteria
- ✅ Export exams to PDF/Docx (store in S3)
- ✅ Track exam history and versioning
- ✅ Provide question recommendation based on learning objectives

**Domain Entities:**

- `Question` (Aggregate Root)
- `QuestionOption` (Entity, for MCQ)
- `ExamMatrix` (Value Object)
- `ExamPaper` (Aggregate Root)
- `ExamQuestion` (Entity, join with weight/order)
- `Difficulty` (Enum: Easy, Medium, Hard)
- `QuestionType` (Enum: MCQ, Essay, TrueFalse)

**Exposed APIs:**

```csharp
// REST Endpoints
GET    /api/assessment/questions                  // List questions (paginated)
GET    /api/assessment/questions/{id}             // Get question details
POST   /api/assessment/questions                  // Create question
PUT    /api/assessment/questions/{id}             // Update question
DELETE /api/assessment/questions/{id}             // Soft delete
POST   /api/assessment/exams/generate             // Generate exam from matrix
GET    /api/assessment/exams/{id}                 // Get exam details
GET    /api/assessment/exams/{id}/download        // Get presigned URL for PDF
POST   /api/assessment/exams/{id}/questions/{qId}/replace  // Replace question

// gRPC Services (internal)
rpc ValidateQuestion(QuestionRequest) returns (ValidationResponse);
```

**Database Schema:**

```sql
-- Questions table
CREATE TABLE Questions (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Content NVARCHAR(MAX) NOT NULL,
    QuestionType NVARCHAR(20) NOT NULL,         -- MCQ, Essay, TrueFalse
    Difficulty NVARCHAR(10) NOT NULL,           -- Easy, Medium, Hard
    BloomTaxonomy NVARCHAR(20),                 -- Remember, Understand, Apply, Analyze
    ChapterId UNIQUEIDENTIFIER NOT NULL,        -- FK to Content.Chapters (via gRPC)
    TextbookId UNIQUEIDENTIFIER NOT NULL,       -- Denormalized for filtering
    CorrectAnswer NVARCHAR(MAX),                -- For MCQ: option ID, Essay: rubric
    Points DECIMAL(5,2) NOT NULL DEFAULT 1.0,
    ExplanationText NVARCHAR(MAX),
    CreatedBy UNIQUEIDENTIFIER NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2,
    IsDeleted BIT NOT NULL DEFAULT 0,
    INDEX IX_Questions_Difficulty (Difficulty),
    INDEX IX_Questions_ChapterId (ChapterId),
    INDEX IX_Questions_TextbookId_Difficulty (TextbookId, Difficulty)
);

-- QuestionOptions table (for MCQ)
CREATE TABLE QuestionOptions (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    QuestionId UNIQUEIDENTIFIER NOT NULL,
    OptionLabel NVARCHAR(5) NOT NULL,           -- A, B, C, D
    OptionText NVARCHAR(1000) NOT NULL,
    IsCorrect BIT NOT NULL DEFAULT 0,
    OrderIndex INT NOT NULL,
    FOREIGN KEY (QuestionId) REFERENCES Questions(Id) ON DELETE CASCADE,
    INDEX IX_QuestionOptions_QuestionId (QuestionId)
);

-- ExamPapers table
CREATE TABLE ExamPapers (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(1000),
    TextbookId UNIQUEIDENTIFIER NOT NULL,
    GradeLevel INT NOT NULL,
    TotalPoints DECIMAL(5,2) NOT NULL,
    Duration INT NOT NULL,                      -- Minutes
    MatrixConfiguration NVARCHAR(MAX),          -- JSON: { "Easy": 5, "Medium": 10, "Hard": 5 }
    PdfS3Key NVARCHAR(500),                     -- Generated PDF location
    DocxS3Key NVARCHAR(500),                    -- Generated Docx location
    CreatedBy UNIQUEIDENTIFIER NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2,
    IsDeleted BIT NOT NULL DEFAULT 0,
    INDEX IX_ExamPapers_CreatedBy (CreatedBy),
    INDEX IX_ExamPapers_TextbookId (TextbookId)
);

-- ExamQuestions table (join table)
CREATE TABLE ExamQuestions (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ExamPaperId UNIQUEIDENTIFIER NOT NULL,
    QuestionId UNIQUEIDENTIFIER NOT NULL,
    OrderIndex INT NOT NULL,
    Points DECIMAL(5,2) NOT NULL,               -- Can override question default
    FOREIGN KEY (ExamPaperId) REFERENCES ExamPapers(Id) ON DELETE CASCADE,
    FOREIGN KEY (QuestionId) REFERENCES Questions(Id),
    UNIQUE (ExamPaperId, QuestionId),           -- No duplicate questions in same exam
    INDEX IX_ExamQuestions_ExamPaperId (ExamPaperId)
);
```

**Business Rules:**

- [ ] Exam must have at least 5 questions
- [ ] Total points must match sum of question points
- [ ] Matrix distribution must be achievable with available questions
- [ ] Replaced questions must match original difficulty level
- [ ] PDF generation must include answer key (separate file)

**Exam Generation Algorithm:**

```csharp
public async Task<ExamPaper> GenerateExam(ExamMatrix matrix, CancellationToken ct)
{
    // 1. Validate matrix (ensure enough questions exist)
    var availableQuestions = await GetAvailableQuestions(matrix.TextbookId, ct);
    ValidateMatrix(matrix, availableQuestions);

    // 2. Select questions using weighted random sampling
    var selectedQuestions = new List<Question>();
    foreach (var (difficulty, count) in matrix.Distribution)
    {
        var candidateQuestions = availableQuestions
            .Where(q => q.Difficulty == difficulty)
            .ToList();

        var selected = SelectRandom(candidateQuestions, count);
        selectedQuestions.AddRange(selected);
    }

    // 3. Shuffle questions (optional, based on teacher preference)
    if (matrix.ShuffleQuestions)
    {
        selectedQuestions = Shuffle(selectedQuestions);
    }

    // 4. Create exam paper entity
    var exam = ExamPaper.Create(matrix, selectedQuestions);
    await _repository.AddAsync(exam, ct);

    // 5. Queue PDF generation (background job)
    await _messageBus.Publish(new ExamGeneratedEvent(exam.Id), ct);

    return exam;
}
```

---

### 3.3 User Service

**Bounded Context:** User identity and profiles

**Responsibilities:**

- ✅ Sync user data from AWS Cognito (via webhook)
- ✅ Manage user profiles (avatar, bio, preferences)
- ✅ Handle class enrollment (student joins teacher's class)
- ✅ Store role-specific metadata (Teacher: school name, Student: grade level)
- ✅ Generate invite codes for class enrollment

**Domain Entities:**

- `UserProfile` (Aggregate Root)
- `Class` (Aggregate Root, owned by Teacher)
- `ClassMembership` (Entity, join table)
- `UserRole` (Enum: Teacher, Student, Admin)

**Exposed APIs:**

```csharp
GET    /api/users/profile                  // Get current user profile
PUT    /api/users/profile                  // Update profile
GET    /api/users/classes                  // List user's classes
POST   /api/users/classes                  // Create class (Teacher only)
POST   /api/users/classes/{id}/join        // Join class with invite code
GET    /api/users/classes/{id}/members     // List class members
```

**Database Schema:**

```sql
-- UserProfiles table
CREATE TABLE UserProfiles (
    Id UNIQUEIDENTIFIER PRIMARY KEY,            -- Same as Cognito sub
    CognitoUserId NVARCHAR(100) NOT NULL UNIQUE,
    Email NVARCHAR(256) NOT NULL,
    FullName NVARCHAR(200) NOT NULL,
    Role NVARCHAR(20) NOT NULL,                 -- Teacher, Student, Admin
    AvatarS3Key NVARCHAR(500),
    Bio NVARCHAR(1000),
    SchoolName NVARCHAR(200),                   -- For Teachers
    GradeLevel INT,                             -- For Students
    PreferredLanguage NVARCHAR(5) DEFAULT 'vi', -- vi, en
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2,
    IsActive BIT NOT NULL DEFAULT 1,
    INDEX IX_UserProfiles_Email (Email),
    INDEX IX_UserProfiles_Role (Role)
);

-- Classes table
CREATE TABLE Classes (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(1000),
    TeacherId UNIQUEIDENTIFIER NOT NULL,
    GradeLevel INT NOT NULL,
    Subject NVARCHAR(50) NOT NULL,
    InviteCode NVARCHAR(10) NOT NULL UNIQUE,    -- Auto-generated, 8-char alphanumeric
    MaxStudents INT DEFAULT 50,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2,
    FOREIGN KEY (TeacherId) REFERENCES UserProfiles(Id),
    INDEX IX_Classes_TeacherId (TeacherId),
    INDEX IX_Classes_InviteCode (InviteCode)
);

-- ClassMemberships table
CREATE TABLE ClassMemberships (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ClassId UNIQUEIDENTIFIER NOT NULL,
    StudentId UNIQUEIDENTIFIER NOT NULL,
    JoinedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    IsActive BIT NOT NULL DEFAULT 1,
    FOREIGN KEY (ClassId) REFERENCES Classes(Id) ON DELETE CASCADE,
    FOREIGN KEY (StudentId) REFERENCES UserProfiles(Id),
    UNIQUE (ClassId, StudentId),
    INDEX IX_ClassMemberships_ClassId (ClassId),
    INDEX IX_ClassMemberships_StudentId (StudentId)
);
```

**Business Rules:**

- [ ] Invite codes must be unique and expire after 90 days
- [ ] Students can only join classes matching their grade level (±1 grade tolerance)
- [ ] Teachers can create max 10 active classes
- [ ] Class size cannot exceed `MaxStudents`

---

### 3.4 AI Orchestrator Service

**Bounded Context:** AI/ML operations and context management

**Responsibilities:**

- ✅ Implement Socratic tutoring logic (no direct answers)
- ✅ RAG pipeline: Retrieve relevant textbook content for context
- ✅ Manage conversation history and context windows
- ✅ Integrate with OpenAI API (GPT-4 Turbo)
- ✅ Implement content safety filters (block inappropriate questions)
- ✅ Track token usage for cost management

**Domain Entities:**

- `TutorSession` (Aggregate Root)
- `ConversationMessage` (Entity)
- `ContextDocument` (Value Object, references Content service)
- `PromptTemplate` (Value Object)

**Exposed APIs:**

```csharp
POST   /api/ai/tutor/sessions               // Start new tutor session
POST   /api/ai/tutor/sessions/{id}/ask      // Ask question (streaming response)
GET    /api/ai/tutor/sessions/{id}/history  // Get conversation history
POST   /api/ai/tutor/feedback               // Submit feedback on AI response
```

**Database Schema:**

```sql
-- TutorSessions table
CREATE TABLE TutorSessions (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    StudentId UNIQUEIDENTIFIER NOT NULL,
    TextbookId UNIQUEIDENTIFIER,                -- Optional: Student working on specific textbook
    ChapterId UNIQUEIDENTIFIER,                 -- Optional: Specific chapter context
    StartedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    LastActivityAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    TotalTokensUsed INT NOT NULL DEFAULT 0,
    TotalCost DECIMAL(10,4) NOT NULL DEFAULT 0,
    IsActive BIT NOT NULL DEFAULT 1,
    INDEX IX_TutorSessions_StudentId (StudentId),
    INDEX IX_TutorSessions_LastActivityAt (LastActivityAt)
);

-- ConversationMessages table
CREATE TABLE ConversationMessages (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    SessionId UNIQUEIDENTIFIER NOT NULL,
    Role NVARCHAR(10) NOT NULL,                 -- user, assistant, system
    Content NVARCHAR(MAX) NOT NULL,
    TokenCount INT NOT NULL,
    ReferencedDocuments NVARCHAR(MAX),          -- JSON array of document IDs
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (SessionId) REFERENCES TutorSessions(Id) ON DELETE CASCADE,
    INDEX IX_ConversationMessages_SessionId (SessionId)
);

-- PromptTemplates table (for prompt engineering management)
CREATE TABLE PromptTemplates (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(100) NOT NULL UNIQUE,
    Description NVARCHAR(500),
    SystemPrompt NVARCHAR(MAX) NOT NULL,
    UserPromptTemplate NVARCHAR(MAX) NOT NULL,  -- With {{placeholders}}
    Model NVARCHAR(50) NOT NULL,                 -- gpt-4-turbo, gpt-3.5-turbo
    Temperature DECIMAL(3,2) NOT NULL DEFAULT 0.7,
    MaxTokens INT NOT NULL DEFAULT 1000,
    IsActive BIT NOT NULL DEFAULT 1,
    Version INT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    INDEX IX_PromptTemplates_Name (Name)
);
```

**RAG Pipeline Implementation:**

```csharp
public async Task<string> GetSocraticResponse(
    string studentQuestion,
    TutorSession session,
    CancellationToken ct)
{
    // 1. Generate embedding for student question
    var embedding = await _embeddingService.GenerateEmbedding(studentQuestion, ct);

    // 2. Retrieve relevant textbook content (vector search)
    var relevantPages = await _contentService.SearchByEmbedding(
        embedding,
        session.TextbookId,
        topK: 5,
        ct);

    // 3. Build context from retrieved documents
    var context = BuildContext(relevantPages);

    // 4. Load Socratic prompt template
    var promptTemplate = await _promptRepository.GetByNameAsync("Socratic-Tutor-V2", ct);

    // 5. Construct full prompt
    var systemPrompt = promptTemplate.SystemPrompt;
    var userPrompt = promptTemplate.UserPromptTemplate
        .Replace("{{CONTEXT}}", context)
        .Replace("{{QUESTION}}", studentQuestion)
        .Replace("{{GRADE_LEVEL}}", session.Student.GradeLevel.ToString());

    // 6. Call OpenAI API (streaming)
    var response = await _openAiService.StreamChatCompletion(
        systemPrompt,
        userPrompt,
        model: promptTemplate.Model,
        temperature: promptTemplate.Temperature,
        ct);

    // 7. Save conversation to database
    await SaveConversation(session.Id, studentQuestion, response, ct);

    return response;
}
```

**Socratic Prompt Engineering:**

```
System Prompt:
You are a Socratic tutor for Vietnamese primary school students (grades 1-5).
Your role is to guide students to discover answers themselves through questions.

RULES:
1. NEVER give direct answers to homework questions.
2. Ask probing questions to help students think critically.
3. Reference the textbook content provided in context.
4. Use simple, age-appropriate language.
5. Encourage and praise effort, not just correct answers.
6. If a student is frustrated, provide a small hint but still require thinking.

User Prompt Template:
Context from textbook:
{{CONTEXT}}

Student grade level: {{GRADE_LEVEL}}
Student question: {{QUESTION}}

Respond with Socratic guidance:
```

**Business Rules:**

- [ ] Sessions auto-expire after 60 minutes of inactivity
- [ ] Content safety filter must block profanity and off-topic questions
- [ ] Max 20 messages per session (prevent abuse)
- [ ] Teachers can review student conversation history

---

## 4. Data Models & Schemas

### Cross-Service Data Consistency

**Challenge:** Microservices have denormalized data (e.g., Assessment service stores `TextbookId` but doesn't own Textbooks).

**Solution: Event-Driven Synchronization**

```csharp
// When Content service publishes an event
public class TextbookUpdatedEvent : IEvent
{
    public Guid TextbookId { get; set; }
    public string NewTitle { get; set; }
    public bool IsDeleted { get; set; }
}

// Assessment service consumes the event
public class TextbookUpdatedConsumer : IConsumer<TextbookUpdatedEvent>
{
    public async Task Consume(ConsumeContext<TextbookUpdatedEvent> context)
    {
        var evt = context.Message;

        // Update denormalized data
        await _repository.UpdateTextbookReferenceAsync(evt.TextbookId, evt.NewTitle);

        // If deleted, mark related questions as inactive
        if (evt.IsDeleted)
        {
            await _repository.DeactivateQuestionsByTextbookAsync(evt.TextbookId);
        }
    }
}
```

### Database Migration Strategy

**Tool:** EF Core Migrations

**Workflow:**

1. **Development:** Run `dotnet ef migrations add <MigrationName>`
2. **Review:** Inspect generated SQL in `Migrations/` folder
3. **Staging:** Apply with `dotnet ef database update`
4. **Production:** Use CI/CD pipeline with automated migration runner

**Rollback Strategy:**

```bash
# Rollback to previous migration
dotnet ef database update <PreviousMigrationName>

# Generate SQL script for manual review
dotnet ef migrations script <FromMigration> <ToMigration> --output migration.sql
```

---

## 5. Infrastructure Components

### 5.1 API Gateway (YARP)

**Configuration (appsettings.json):**

```json
{
  "ReverseProxy": {
    "Routes": {
      "content-route": {
        "ClusterId": "content-cluster",
        "Match": {
          "Path": "/api/content/{**catch-all}"
        },
        "Transforms": [{ "PathRemovePrefix": "/api/content" }]
      },
      "assessment-route": {
        "ClusterId": "assessment-cluster",
        "Match": {
          "Path": "/api/assessment/{**catch-all}"
        }
      }
    },
    "Clusters": {
      "content-cluster": {
        "Destinations": {
          "content-service": {
            "Address": "http://content-service:5001"
          }
        }
      },
      "assessment-cluster": {
        "Destinations": {
          "assessment-service": {
            "Address": "http://assessment-service:5002"
          }
        }
      }
    }
  }
}
```

**Middleware Pipeline:**

```csharp
app.UseRouting();
app.UseAuthentication();  // JWT validation
app.UseAuthorization();
app.UseRateLimiter();     // Prevent DDoS
app.MapReverseProxy();
```

### 5.2 Event Bus (MassTransit + RabbitMQ)

**Configuration:**

```csharp
services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

    // Register consumers
    x.AddConsumer<ExamGeneratedConsumer>();
    x.AddConsumer<TextbookUpdatedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        // Retry policy: 3 attempts with exponential backoff
        cfg.UseMessageRetry(r => r.Exponential(
            retryLimit: 3,
            minInterval: TimeSpan.FromSeconds(2),
            maxInterval: TimeSpan.FromSeconds(30),
            intervalDelta: TimeSpan.FromSeconds(5)
        ));

        cfg.ConfigureEndpoints(context);
    });
});
```

### 5.3 AWS S3 Integration

**Service Implementation:**

```csharp
public class S3StorageService : IStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName = "edu-classroom-assets";

    public async Task<string> UploadFileAsync(
        Stream fileStream,
        string key,
        string contentType,
        CancellationToken ct)
    {
        var request = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = key,
            InputStream = fileStream,
            ContentType = contentType,
            ServerSideEncryptionMethod = ServerSideEncryptionMethod.AES256
        };

        await _s3Client.PutObjectAsync(request, ct);
        return $"s3://{_bucketName}/{key}";
    }

    public async Task<string> GetPresignedUrlAsync(string key, TimeSpan expiration, CancellationToken ct)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _bucketName,
            Key = key,
            Expires = DateTime.UtcNow.Add(expiration),
            Verb = HttpVerb.GET
        };

        return await _s3Client.GetPreSignedURLAsync(request);
    }
}
```

---

## 6. Technical Standards

### 6.1 Clean Architecture Enforcement

**Dependency Rules:**

```
API → Application → Domain
  ↓         ↓
Infrastructure  (Can reference all layers)
```

**Anti-Patterns to Avoid:**

- ❌ Domain layer referencing Infrastructure (violates dependency inversion)
- ❌ Controllers containing business logic (should delegate to Application layer)
- ❌ Direct database access in API controllers (use repositories)

### 6.2 CQRS Implementation

**Command Example:**

```csharp
// Application/Commands/CreateQuestion.cs
public record CreateQuestionCommand(
    string Content,
    QuestionType Type,
    Difficulty Difficulty,
    Guid ChapterId,
    List<QuestionOptionDto> Options
) : IRequest<Result<Guid>>;

// Application/Handlers/CreateQuestionHandler.cs
public class CreateQuestionHandler : IRequestHandler<CreateQuestionCommand, Result<Guid>>
{
    private readonly IQuestionRepository _repository;
    private readonly IValidator<CreateQuestionCommand> _validator;

    public async Task<Result<Guid>> Handle(CreateQuestionCommand request, CancellationToken ct)
    {
        // 1. Validate
        var validationResult = await _validator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
            return Result<Guid>.Failure(validationResult.Errors);

        // 2. Create domain entity
        var question = Question.Create(
            request.Content,
            request.Type,
            request.Difficulty,
            request.ChapterId);

        // 3. Persist
        await _repository.AddAsync(question, ct);
        await _repository.SaveChangesAsync(ct);

        // 4. Return ID
        return Result<Guid>.Success(question.Id);
    }
}
```

**Query Example:**

```csharp
// Application/Queries/GetQuestionById.cs
public record GetQuestionByIdQuery(Guid Id) : IRequest<QuestionDto?>;

public class GetQuestionByIdHandler : IRequestHandler<GetQuestionByIdQuery, QuestionDto?>
{
    private readonly IQuestionRepository _repository;
    private readonly IMapper _mapper;

    public async Task<QuestionDto?> Handle(GetQuestionByIdQuery request, CancellationToken ct)
    {
        var question = await _repository.GetByIdAsync(request.Id, ct);
        return _mapper.Map<QuestionDto>(question);
    }
}
```

### 6.3 Error Handling

**ProblemDetails Standard:**

```csharp
// Global exception handler middleware
public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken ct)
    {
        var problemDetails = exception switch
        {
            ValidationException valEx => new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation Error",
                Detail = string.Join(", ", valEx.Errors),
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            },
            NotFoundException notFoundEx => new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Resource Not Found",
                Detail = notFoundEx.Message,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4"
            },
            _ => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Server Error",
                Detail = "An unexpected error occurred.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            }
        };

        context.Response.StatusCode = problemDetails.Status.Value;
        await context.Response.WriteAsJsonAsync(problemDetails, ct);
        return true;
    }
}
```

---

## 7. Development Guidelines

### 7.1 Code Review Checklist

Before merging any PR, verify:

- [ ] All tests pass (unit + integration)
- [ ] Code coverage ≥ 80%
- [ ] No new SonarQube critical/major issues
- [ ] Follows C# naming conventions (PascalCase for public, camelCase for private)
- [ ] XML documentation comments on public APIs
- [ ] Async methods have `Async` suffix
- [ ] Null checks for nullable parameters
- [ ] Proper disposal of `IDisposable` resources (`using` statements)

### 7.2 Logging Standards

**Structured Logging with Serilog:**

```csharp
_logger.LogInformation(
    "Exam generated. ExamId={ExamId}, QuestionCount={QuestionCount}, Duration={Duration}ms",
    exam.Id,
    exam.Questions.Count,
    stopwatch.ElapsedMilliseconds
);
```

**Log Levels:**

- `Trace`: Detailed diagnostic (not in production)
- `Debug`: Development insights
- `Information`: General flow (exam generated, user logged in)
- `Warning`: Recoverable issues (API rate limit approached)
- `Error`: Failures (database timeout, S3 upload failed)
- `Critical`: System-wide failures (database unavailable)

---

## 8. Security & Authentication

### 8.1 JWT Validation

**Configuration:**

```csharp
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://cognito-idp.ap-southeast-1.amazonaws.com/ap-southeast-1_XXXXX";
        options.Audience = "your-cognito-app-client-id";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(5)
        };
    });
```

### 8.2 Authorization Policies

```csharp
services.AddAuthorization(options =>
{
    options.AddPolicy("TeacherOnly", policy =>
        policy.RequireClaim("custom:role", "Teacher"));

    options.AddPolicy("StudentOnly", policy =>
        policy.RequireClaim("custom:role", "Student"));

    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});

// Usage in controller
[Authorize(Policy = "TeacherOnly")]
[HttpPost("classes")]
public async Task<IActionResult> CreateClass(CreateClassCommand command, CancellationToken ct)
{
    // Only teachers can create classes
}
```

---

## 9. Testing Strategy

### 9.1 Unit Tests

**Guidelines:**

- Test business logic in Domain and Application layers
- Use `xUnit` as testing framework
- Use `FluentAssertions` for readable assertions
- Mock dependencies with `NSubstitute`
- Target: 80% code coverage

**Example:**

```csharp
public class QuestionTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateQuestion()
    {
        // Arrange
        var content = "What is 2 + 2?";
        var type = QuestionType.MCQ;
        var difficulty = Difficulty.Easy;
        var chapterId = Guid.NewGuid();

        // Act
        var question = Question.Create(content, type, difficulty, chapterId);

        // Assert
        question.Should().NotBeNull();
        question.Content.Should().Be(content);
        question.Difficulty.Should().Be(difficulty);
        question.IsDeleted.Should().BeFalse();
    }
}
```

### 9.2 Integration Tests

**Setup:**

```csharp
public class AssessmentApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    public AssessmentApiTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Testing");
            builder.ConfigureServices(services =>
            {
                // Replace production DbContext with in-memory database
                services.RemoveAll<DbContextOptions<AssessmentDbContext>>();
                services.AddDbContext<AssessmentDbContext>(options =>
                    options.UseInMemoryDatabase("TestDb"));
            });
        });

        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetQuestions_ShouldReturnOk()
    {
        // Act
        var response = await _client.GetAsync("/api/assessment/questions");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
```

---

## 10. Milestones & Tasks

### Milestone 1: Infrastructure & Shared Kernel

**Status:** 🟡 In Progress

#### Task 1.1: Solution Structure Setup

- [x] Create solution file (`FrogEdu.sln`)
- [x] Create service folders (Content, Assessment, User, AI)
- [x] Create 4 projects per service (Domain, Application, Infrastructure, API)
- [ ] Configure project references (enforce dependency rules)
- [ ] Setup NuGet packages (MediatR, FluentValidation, Serilog)

**Validation:**

- [ ] Solution builds without errors
- [ ] Project dependency graph matches Clean Architecture
- [ ] All projects target .NET 9

#### Task 1.2: Shared Kernel Implementation

- [ ] Create `FrogEdu.Shared.Kernel` project
- [ ] Implement `Entity` base class (Id, CreatedAt, UpdatedAt)
- [ ] Implement `ValueObject` base class (equality by value)
- [ ] Implement `Result<T>` pattern for error handling
- [ ] Implement `IDomainEvent` interface
- [ ] Create common exceptions (NotFoundException, ValidationException)
- [ ] Create extension methods (StringExtensions, DateTimeExtensions)

**Validation:**

- [ ] Shared.Kernel project referenced by all Domain projects
- [ ] Unit tests for Result pattern (success/failure scenarios)
- [ ] XML documentation on all public classes

#### Task 1.3: Docker Compose for Local Development

- [ ] Create `docker-compose.yml` for SQL Server
- [ ] Configure SQL Server with initial database creation
- [ ] Add RabbitMQ service
- [ ] Add LocalStack (for S3 simulation)
- [ ] Add environment variables for connection strings
- [ ] Create seed data scripts

**Validation:**

- [ ] `docker-compose up` starts all services successfully
- [ ] SQL Server accessible on `localhost:1433`
- [ ] RabbitMQ management UI accessible on `localhost:15672`
- [ ] LocalStack S3 accessible on `localhost:4566`

#### Task 1.4: API Gateway (YARP) Foundation

- [ ] Create `FrogEdu.Gateway` project (ASP.NET Core)
- [ ] Configure YARP routing for all 4 services
- [ ] Implement JWT validation middleware
- [ ] Add rate limiting (100 requests/minute per user)
- [ ] Configure CORS (allow frontend domain)
- [ ] Add health check endpoints (`/health`)

**Validation:**

- [ ] Gateway routes requests to downstream services
- [ ] Unauthorized requests (no JWT) receive 401 status
- [ ] Rate limiting triggers 429 after 100 requests
- [ ] Health check returns 200 OK

---

### Milestone 2: Content Service Implementation

**Status:** ⚪ Not Started

#### Task 2.1: Domain Layer

- [ ] Create `Textbook` aggregate root (Id, Title, Subject, GradeLevel)
- [ ] Create `Chapter` entity (ChapterNumber, Title, Textbook FK)
- [ ] Create `Page` entity (PageNumber, S3Key, Chapter FK)
- [ ] Create `Subject` value object (validation: Math, Vietnamese, English, etc.)
- [ ] Create `GradeLevel` enum (Grade1-Grade5)
- [ ] Implement domain events (TextbookCreated, TextbookUpdated)
- [ ] Create `ITextbookRepository` interface

**Validation:**

- [ ] Domain entities have private setters
- [ ] Constructors enforce invariants (e.g., GradeLevel 1-5)
- [ ] Unit tests for entity creation and business rules

#### Task 2.2: Application Layer (CQRS)

- [ ] Create `CreateTextbookCommand` and handler
- [ ] Create `UpdateTextbookCommand` and handler
- [ ] Create `DeleteTextbookCommand` and handler (soft delete)
- [ ] Create `GetTextbooksQuery` and handler (with pagination)
- [ ] Create `GetTextbookByIdQuery` and handler
- [ ] Implement FluentValidation validators for all commands
- [ ] Create DTOs (TextbookDto, ChapterDto, PageDto)
- [ ] Configure AutoMapper profiles

**Validation:**

- [ ] All commands/queries use MediatR
- [ ] Validators reject invalid input (e.g., empty Title)
- [ ] Unit tests for handlers (mock repository)

#### Task 2.3: Infrastructure Layer

- [ ] Create `ContentDbContext` with EF Core
- [ ] Configure entity mappings (Fluent API)
- [ ] Implement `TextbookRepository` (CRUD methods)
- [ ] Create initial migration (`InitialCreate`)
- [ ] Implement seed data (5 sample textbooks)
- [ ] Create `S3StorageService` (upload/download/presigned URLs)
- [ ] Implement MassTransit publishers (TextbookUpdated event)

**Validation:**

- [ ] Migration generates correct SQL schema
- [ ] Seed data populates database on startup
- [ ] S3 upload/download works with LocalStack
- [ ] Events published to RabbitMQ

#### Task 2.4: API Layer

- [ ] Create `TextbooksController` with REST endpoints
- [ ] Implement GET `/api/content/textbooks` (list, paginated)
- [ ] Implement GET `/api/content/textbooks/{id}` (details)
- [ ] Implement POST `/api/content/textbooks` (admin only)
- [ ] Implement PUT `/api/content/textbooks/{id}`
- [ ] Implement DELETE `/api/content/textbooks/{id}` (soft delete)
- [ ] Implement GET `/api/content/pages/{id}/url` (presigned URL)
- [ ] Add Swagger/OpenAPI documentation
- [ ] Configure dependency injection in `Program.cs`

**Validation:**

- [ ] All endpoints return proper status codes (200, 201, 404, 400)
- [ ] Authorization works (only admins can create textbooks)
- [ ] Swagger UI displays all endpoints with examples
- [ ] Integration tests verify end-to-end flow

#### Task 2.5: gRPC Service (Internal)

- [ ] Create `content.proto` file (GetChapter, GetPages)
- [ ] Implement `ContentGrpcService`
- [ ] Configure gRPC in `Program.cs`
- [ ] Test with gRPC client (Postman or BloomRPC)

**Validation:**

- [ ] gRPC service accessible on port 5011
- [ ] Assessment service can call gRPC methods

---

### Milestone 3: Assessment Service Implementation

**Status:** ⚪ Not Started

#### Task 3.1: Domain Layer

- [ ] Create `Question` aggregate root (Content, Type, Difficulty, ChapterId)
- [ ] Create `QuestionOption` entity (for MCQ questions)
- [ ] Create `ExamPaper` aggregate root (Title, Matrix, Questions)
- [ ] Create `ExamQuestion` entity (join table with order/points)
- [ ] Create `ExamMatrix` value object (distribution logic)
- [ ] Create `Difficulty` enum (Easy, Medium, Hard)
- [ ] Create `QuestionType` enum (MCQ, Essay, TrueFalse)
- [ ] Implement `IQuestionRepository` and `IExamRepository` interfaces

**Validation:**

- [ ] Exam matrix validates against available questions
- [ ] No duplicate questions in same exam (enforced)
- [ ] Unit tests for business rules

#### Task 3.2: Application Layer

- [ ] Create `CreateQuestionCommand` and handler
- [ ] Create `GenerateExamCommand` and handler (implement algorithm)
- [ ] Create `ReplaceQuestionInExamCommand` and handler
- [ ] Create `GetQuestionsQuery` (with filtering by difficulty/chapter)
- [ ] Create `GetExamByIdQuery` and handler
- [ ] Implement exam generation algorithm (weighted selection)
- [ ] Create background job for PDF generation (Hangfire or Quartz.NET)

**Validation:**

- [ ] Exam generation selects correct number of questions
- [ ] Matrix distribution is honored
- [ ] Validation prevents impossible matrix configurations

#### Task 3.3: Infrastructure Layer

- [ ] Create `AssessmentDbContext`
- [ ] Configure entity mappings (Questions, Exams, join table)
- [ ] Implement repositories
- [ ] Create initial migration
- [ ] Integrate PDF generation library (QuestPDF or PdfSharp)
- [ ] Implement S3 upload for generated PDFs
- [ ] Create MassTransit consumer for `TextbookUpdatedEvent`

**Validation:**

- [ ] PDF generation produces valid PDF files
- [ ] Generated PDFs uploaded to S3 successfully
- [ ] Event consumer updates denormalized textbook data

#### Task 3.4: API Layer

- [ ] Create `QuestionsController` and `ExamsController`
- [ ] Implement all REST endpoints (CRUD + generate + download)
- [ ] Add authorization (teachers only for exam generation)
- [ ] Configure Swagger

**Validation:**

- [ ] Integration tests verify exam generation workflow
- [ ] PDF download returns presigned URL

---

### Milestone 4: User Service Implementation

**Status:** ⚪ Not Started

#### Task 4.1: Cognito Integration

- [ ] Setup AWS Cognito User Pool (via Terraform or AWS Console)
- [ ] Configure user attributes (custom:role, custom:gradeLevel)
- [ ] Create Lambda trigger for post-confirmation (sync to User Service)
- [ ] Test user registration and role assignment

**Validation:**

- [ ] New Cognito users automatically created in UserProfiles table
- [ ] JWT tokens include custom claims (role, userId)

#### Task 4.2: Domain & Application Layers

- [ ] Create `UserProfile` aggregate root
- [ ] Create `Class` aggregate root
- [ ] Create `ClassMembership` entity
- [ ] Implement invite code generation (8-char alphanumeric)
- [ ] Create commands: CreateClass, JoinClass, UpdateProfile
- [ ] Create queries: GetUserProfile, GetClasses, GetClassMembers

**Validation:**

- [ ] Invite codes are unique
- [ ] Students can only join classes matching grade level

#### Task 4.3: Infrastructure & API

- [ ] Create `UserDbContext` and migrations
- [ ] Implement repositories
- [ ] Create REST endpoints
- [ ] Add avatar upload to S3

**Validation:**

- [ ] Class creation returns invite code
- [ ] Student can join with valid invite code

---

### Milestone 5: AI Orchestrator Service Implementation

**Status:** ⚪ Not Started

#### Task 5.1: OpenAI Integration

- [ ] Install `Semantic.Kernel` NuGet package
- [ ] Configure OpenAI API key (from appsettings or secrets)
- [ ] Implement chat completion service (streaming)
- [ ] Test basic Q&A flow

**Validation:**

- [ ] OpenAI API returns responses successfully
- [ ] Streaming works (SSE or WebSockets)

#### Task 5.2: RAG Pipeline

- [ ] Implement embedding generation (text-embedding-ada-002)
- [ ] Create vector store (SQL Server with vector extension or Pinecone)
- [ ] Implement similarity search (cosine similarity)
- [ ] Integrate with Content service (gRPC) to fetch textbook pages
- [ ] Build context from retrieved documents

**Validation:**

- [ ] Similar questions retrieve relevant textbook content
- [ ] Context included in LLM prompt

#### Task 5.3: Socratic Tutoring Logic

- [ ] Create prompt templates in database
- [ ] Implement Socratic prompt engineering (no direct answers)
- [ ] Add content safety filter (Azure Content Safety or custom)
- [ ] Implement conversation history management (context window)

**Validation:**

- [ ] AI never gives direct answers to homework questions
- [ ] Responses reference textbook content
- [ ] Inappropriate questions are blocked

#### Task 5.4: API & Database

- [ ] Create `TutorSessionsController`
- [ ] Implement session management (create, ask, history)
- [ ] Create database migrations (TutorSessions, ConversationMessages)
- [ ] Add token usage tracking (for cost monitoring)

**Validation:**

- [ ] Session persists conversation history
- [ ] Teachers can review student sessions

---

### Milestone 6: Integration & Testing

**Status:** ⚪ Not Started

#### Task 6.1: Event-Driven Integration

- [ ] Verify all event publishers/consumers work
- [ ] Test retry policies (simulate failures)
- [ ] Test dead-letter queue handling

**Validation:**

- [ ] Events flow between services correctly
- [ ] Failed messages retry 3 times then move to DLQ

#### Task 6.2: End-to-End Testing

- [ ] Write E2E tests for exam generation workflow
- [ ] Write E2E tests for AI tutor session
- [ ] Test cross-service scenarios

**Validation:**

- [ ] Teacher can create exam and download PDF
- [ ] Student can ask tutor and receive guidance

#### Task 6.3: Performance Testing

- [ ] Load test API Gateway (1000 concurrent users)
- [ ] Benchmark exam generation (should be < 10 seconds)
- [ ] Benchmark AI tutor response time (should be < 5 seconds)

**Validation:**

- [ ] System handles target load without errors
- [ ] Response times meet SLA

---

### Milestone 7: Deployment & DevOps

**Status:** ⚪ Not Started

#### Task 7.1: Dockerization

- [ ] Create Dockerfile for each service
- [ ] Multi-stage builds (build + runtime)
- [ ] Optimize image sizes (< 200MB per service)

**Validation:**

- [ ] All services run in Docker containers
- [ ] Images pushed to container registry (Docker Hub or ECR)

#### Task 7.2: CI/CD Pipeline

- [ ] Setup GitHub Actions workflows
- [ ] Automated testing on PR
- [ ] Automated deployment to staging
- [ ] Manual approval for production

**Validation:**

- [ ] Pipeline runs on every commit
- [ ] Tests must pass before merge

#### Task 7.3: Infrastructure as Code (Terraform)

- [ ] Define AWS resources (RDS, S3, ECS/EKS)
- [ ] Configure networking (VPC, subnets, security groups)
- [ ] Apply Terraform scripts to create infrastructure

**Validation:**

- [ ] `terraform apply` provisions all resources
- [ ] Infrastructure documented in code

---

## Completion Checklist

Before marking backend as **DONE**, verify:

- [ ] All 4 microservices are operational
- [ ] All database migrations applied successfully
- [ ] API Gateway routes correctly to all services
- [ ] Authentication/authorization works across services
- [ ] Events flow correctly between services
- [ ] S3 integration works (upload/download/presigned URLs)
- [ ] OpenTelemetry tracing configured
- [ ] Swagger/OpenAPI documentation complete
- [ ] All unit tests passing (>80% coverage)
- [ ] All integration tests passing
- [ ] Load tests meet performance requirements
- [ ] Docker images built and pushed
- [ ] CI/CD pipeline operational
- [ ] Infrastructure deployed via Terraform

---

## Notes for AI Agents

**When starting a new task:**

1. Read this spec file completely to understand context.
2. Check that all prerequisite tasks are marked `[x]`.
3. Review the technical standards section for coding rules.
4. Implement the feature following Clean Architecture.
5. Write tests before marking task complete.
6. Update the checkbox to `[x]` immediately after verification.
7. Commit with message: `feat(service-name): implement Task X.Y - description`

**If you encounter blockers:**

1. Document the blocker in a comment above the task.
2. Do not mark task as complete.
3. Notify the user/team about the blocker.

**Remember:** Specs and code must stay in sync. If you discover the spec is wrong during implementation, update the spec first, then implement.
