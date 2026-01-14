# Milestone 5: AI Student Tutor

**Feature:** AI-Powered Socratic Tutor with RAG Pipeline  
**Epic:** AI Features  
**Priority:** P2 (Medium)  
**Estimated Effort:** 32-40 hours  
**Status:** 🔄 Ready for Implementation

---

## Overview

Build an AI tutor that answers student questions using Socratic method + RAG (Retrieval-Augmented Generation) with textbook content.

### User Stories

- **US-5.1:** Student asks question about textbook content
- **US-5.2:** AI retrieves relevant textbook sections via RAG
- **US-5.3:** AI responds with Socratic questioning (not direct answers)
- **US-5.4:** Student conversation history persisted
- **US-5.5:** AI streams responses for better UX
- **US-5.6:** Teacher reviews student-AI conversation logs

---

## Architecture: RAG Pipeline

```
Student Question
       ↓
1. Embedding Generation (OpenAI text-embedding-3-small)
       ↓
2. Vector Search (SQL Server or in-memory)
       ↓
3. Retrieve Top-K Textbook Pages
       ↓
4. Build Context Prompt
       ↓
5. Send to OpenAI GPT-4o-mini
       ↓
6. Stream Response to Frontend
       ↓
7. Store Conversation in AiContextDB
```

---

## Domain Model

### Entities

#### `TutorSession` (Aggregate Root)

```csharp
public class TutorSession : Entity
{
    public Guid StudentId { get; private set; }
    public Guid? TextbookId { get; private set; }
    public Guid? ChapterId { get; private set; }
    public DateTime StartedAt { get; private set; }
    public DateTime? EndedAt { get; private set; }
    public SessionStatus Status { get; private set; }

    private readonly List<TutorMessage> _messages = new();
    public IReadOnlyList<TutorMessage> Messages => _messages.AsReadOnly();

    public static TutorSession Create(Guid studentId, Guid? textbookId, Guid? chapterId)
    {
        return new TutorSession
        {
            StudentId = studentId,
            TextbookId = textbookId,
            ChapterId = chapterId,
            StartedAt = DateTime.UtcNow,
            Status = SessionStatus.Active
        };
    }

    public void AddMessage(string role, string content)
    {
        var message = TutorMessage.Create(Id, role, content);
        _messages.Add(message);
        MarkAsUpdated();
    }

    public void End()
    {
        EndedAt = DateTime.UtcNow;
        Status = SessionStatus.Completed;
        MarkAsUpdated();
    }
}

public enum SessionStatus
{
    Active = 1,
    Completed = 2
}
```

#### `TutorMessage` (Entity)

```csharp
public class TutorMessage : Entity
{
    public Guid SessionId { get; private set; }
    public string Role { get; private set; } // "user" or "assistant"
    public string Content { get; private set; }
    public DateTime Timestamp { get; private set; }
    public List<string>? RetrievedPageIds { get; private set; } // For RAG traceability

    public static TutorMessage Create(Guid sessionId, string role, string content)
    {
        return new TutorMessage
        {
            SessionId = sessionId,
            Role = role,
            Content = content,
            Timestamp = DateTime.UtcNow
        };
    }
}
```

#### `TextbookEmbedding` (Entity for RAG)

```csharp
public class TextbookEmbedding : Entity
{
    public Guid PageId { get; private set; }
    public string Content { get; private set; }
    public byte[] Embedding { get; private set; } // Float array serialized
    public DateTime GeneratedAt { get; private set; }

    public static TextbookEmbedding Create(Guid pageId, string content, byte[] embedding)
    {
        return new TextbookEmbedding
        {
            PageId = pageId,
            Content = content,
            Embedding = embedding,
            GeneratedAt = DateTime.UtcNow
        };
    }
}
```

---

## Database Schema (AiContextDB)

```sql
CREATE TABLE [dbo].[TutorSessions] (
    [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [StudentId] UNIQUEIDENTIFIER NOT NULL,
    [TextbookId] UNIQUEIDENTIFIER NULL,
    [ChapterId] UNIQUEIDENTIFIER NULL,
    [StartedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [EndedAt] DATETIME2 NULL,
    [Status] INT NOT NULL DEFAULT 1, -- 1=Active, 2=Completed
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [IsDeleted] BIT NOT NULL DEFAULT 0
);

CREATE INDEX [IX_TutorSessions_StudentId] ON [TutorSessions]([StudentId]);

CREATE TABLE [dbo].[TutorMessages] (
    [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [SessionId] UNIQUEIDENTIFIER NOT NULL,
    [Role] NVARCHAR(20) NOT NULL, -- 'user', 'assistant'
    [Content] NVARCHAR(MAX) NOT NULL,
    [Timestamp] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [RetrievedPageIds] NVARCHAR(MAX) NULL, -- JSON array
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),

    CONSTRAINT [FK_TutorMessages_Sessions] FOREIGN KEY ([SessionId])
        REFERENCES [TutorSessions]([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_TutorMessages_SessionId] ON [TutorMessages]([SessionId]);

CREATE TABLE [dbo].[TextbookEmbeddings] (
    [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [PageId] UNIQUEIDENTIFIER NOT NULL,
    [Content] NVARCHAR(MAX) NOT NULL,
    [Embedding] VARBINARY(MAX) NOT NULL, -- 1536 float array (6144 bytes)
    [GeneratedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [IsDeleted] BIT NOT NULL DEFAULT 0
);

CREATE UNIQUE INDEX [IX_TextbookEmbeddings_PageId] ON [TextbookEmbeddings]([PageId])
    WHERE [IsDeleted] = 0;
```

---

## Application Layer

### Commands

#### `AskTutorCommand` (Main Entry Point)

```csharp
public record AskTutorCommand(
    Guid SessionId,
    string Question
) : IRequest<Result<string>>; // Returns AI response

public class AskTutorCommandHandler : IRequestHandler<AskTutorCommand, Result<string>>
{
    private readonly ITutorSessionRepository _sessionRepo;
    private readonly IOpenAIService _openAIService;
    private readonly IRAGService _ragService;

    public async Task<Result<string>> Handle(AskTutorCommand request, CancellationToken ct)
    {
        // 1. Load session
        var session = await _sessionRepo.GetByIdAsync(request.SessionId, ct);
        if (session == null)
        {
            return Result.Failure<string>("Session not found");
        }

        // 2. Store user message
        session.AddMessage("user", request.Question);

        // 3. Retrieve relevant context via RAG
        var relevantPages = await _ragService.RetrieveRelevantPagesAsync(
            request.Question,
            session.TextbookId,
            topK: 3,
            ct
        );

        // 4. Build Socratic prompt
        var systemPrompt = BuildSocraticSystemPrompt(relevantPages);

        // 5. Generate AI response
        var aiResponse = await _openAIService.GenerateChatCompletionAsync(
            systemPrompt,
            session.Messages.Select(m => (m.Role, m.Content)).ToList(),
            ct
        );

        // 6. Store AI message
        session.AddMessage("assistant", aiResponse);
        await _sessionRepo.UpdateAsync(session, ct);
        await _sessionRepo.SaveChangesAsync(ct);

        return Result.Success(aiResponse);
    }

    private string BuildSocraticSystemPrompt(List<string> relevantContent)
    {
        return $@"
You are a Socratic tutor for Vietnamese primary students (grades 1-12).

**Rules:**
1. Never give direct answers. Ask guiding questions instead.
2. Break down complex problems into smaller steps.
3. Encourage critical thinking.
4. Be patient and encouraging.
5. Use simple language appropriate for the student's grade level.
6. Reference the textbook content below when relevant.

**Textbook Context:**
{string.Join("\n\n", relevantContent)}

**Example Interaction:**
Student: ""What is 5 + 3?""
You: ""Great question! Let's think step by step. If you have 5 apples and your friend gives you 3 more, how many apples do you have now? Can you count them together?""

Now respond to the student's question.
";
    }
}
```

#### `GenerateEmbeddingsCommand` (Batch Job)

```csharp
public record GenerateEmbeddingsCommand(Guid TextbookId) : IRequest<Result>;

public class GenerateEmbeddingsCommandHandler : IRequestHandler<GenerateEmbeddingsCommand, Result>
{
    private readonly ContentService.ContentServiceClient _contentClient;
    private readonly IOpenAIService _openAIService;
    private readonly IEmbeddingRepository _embeddingRepo;

    public async Task<Result> Handle(GenerateEmbeddingsCommand request, CancellationToken ct)
    {
        // 1. Fetch all pages for textbook via gRPC
        var pages = await _contentClient.GetAllPagesForTextbookAsync(
            new GetAllPagesRequest { TextbookId = request.TextbookId.ToString() },
            cancellationToken: ct
        );

        // 2. Generate embeddings for each page
        foreach (var page in pages.Pages)
        {
            var embedding = await _openAIService.GenerateEmbeddingAsync(page.Content, ct);

            var embeddingEntity = TextbookEmbedding.Create(
                Guid.Parse(page.Id),
                page.Content,
                SerializeEmbedding(embedding)
            );

            await _embeddingRepo.AddAsync(embeddingEntity, ct);
        }

        await _embeddingRepo.SaveChangesAsync(ct);
        return Result.Success();
    }

    private byte[] SerializeEmbedding(float[] embedding)
    {
        var buffer = new byte[embedding.Length * 4];
        Buffer.BlockCopy(embedding, 0, buffer, 0, buffer.Length);
        return buffer;
    }
}
```

---

## OpenAI Service

```csharp
public interface IOpenAIService
{
    Task<string> GenerateChatCompletionAsync(
        string systemPrompt,
        List<(string Role, string Content)> messages,
        CancellationToken ct);

    Task<string> GenerateChatCompletionStreamingAsync(
        string systemPrompt,
        List<(string Role, string Content)> messages,
        Action<string> onChunk,
        CancellationToken ct);

    Task<float[]> GenerateEmbeddingAsync(string text, CancellationToken ct);
}

public class OpenAIService : IOpenAIService
{
    private readonly OpenAIClient _client;
    private readonly string _chatModel = "gpt-4o-mini"; // Cheaper model
    private readonly string _embeddingModel = "text-embedding-3-small";

    public OpenAIService(IOptions<OpenAISettings> settings)
    {
        _client = new OpenAIClient(settings.Value.ApiKey);
    }

    public async Task<string> GenerateChatCompletionAsync(
        string systemPrompt,
        List<(string Role, string Content)> messages,
        CancellationToken ct)
    {
        var chatMessages = new List<ChatMessage>
        {
            new SystemChatMessage(systemPrompt)
        };

        foreach (var (role, content) in messages)
        {
            chatMessages.Add(role == "user"
                ? new UserChatMessage(content)
                : new AssistantChatMessage(content));
        }

        var response = await _client.GetChatCompletionsAsync(
            _chatModel,
            chatMessages,
            new ChatCompletionsOptions
            {
                Temperature = 0.7f,
                MaxTokens = 500
            },
            ct
        );

        return response.Value.Choices[0].Message.Content;
    }

    public async Task<string> GenerateChatCompletionStreamingAsync(
        string systemPrompt,
        List<(string Role, string Content)> messages,
        Action<string> onChunk,
        CancellationToken ct)
    {
        var chatMessages = new List<ChatMessage>
        {
            new SystemChatMessage(systemPrompt)
        };

        foreach (var (role, content) in messages)
        {
            chatMessages.Add(role == "user"
                ? new UserChatMessage(content)
                : new AssistantChatMessage(content));
        }

        var fullResponse = new StringBuilder();

        await foreach (var update in _client.GetChatCompletionsStreamingAsync(
            _chatModel,
            chatMessages,
            new ChatCompletionsOptions { Temperature = 0.7f },
            ct))
        {
            if (update.ContentUpdate != null)
            {
                var chunk = update.ContentUpdate;
                fullResponse.Append(chunk);
                onChunk(chunk);
            }
        }

        return fullResponse.ToString();
    }

    public async Task<float[]> GenerateEmbeddingAsync(string text, CancellationToken ct)
    {
        var response = await _client.GetEmbeddingsAsync(
            _embeddingModel,
            new EmbeddingsOptions { Input = { text } },
            ct
        );

        return response.Value.Data[0].Embedding.ToArray();
    }
}
```

---

## RAG Service (Vector Search)

```csharp
public interface IRAGService
{
    Task<List<string>> RetrieveRelevantPagesAsync(
        string query,
        Guid? textbookId,
        int topK,
        CancellationToken ct);
}

public class RAGService : IRAGService
{
    private readonly IOpenAIService _openAIService;
    private readonly IEmbeddingRepository _embeddingRepo;

    public async Task<List<string>> RetrieveRelevantPagesAsync(
        string query,
        Guid? textbookId,
        int topK,
        CancellationToken ct)
    {
        // 1. Generate query embedding
        var queryEmbedding = await _openAIService.GenerateEmbeddingAsync(query, ct);

        // 2. Fetch all embeddings for textbook
        var embeddings = await _embeddingRepo.GetByTextbookIdAsync(textbookId, ct);

        // 3. Compute cosine similarity
        var similarities = embeddings
            .Select(e => new
            {
                Content = e.Content,
                Similarity = CosineSimilarity(queryEmbedding, DeserializeEmbedding(e.Embedding))
            })
            .OrderByDescending(x => x.Similarity)
            .Take(topK)
            .Select(x => x.Content)
            .ToList();

        return similarities;
    }

    private float[] DeserializeEmbedding(byte[] bytes)
    {
        var floats = new float[bytes.Length / 4];
        Buffer.BlockCopy(bytes, 0, floats, 0, bytes.Length);
        return floats;
    }

    private float CosineSimilarity(float[] a, float[] b)
    {
        float dotProduct = 0, magnitudeA = 0, magnitudeB = 0;

        for (int i = 0; i < a.Length; i++)
        {
            dotProduct += a[i] * b[i];
            magnitudeA += a[i] * a[i];
            magnitudeB += b[i] * b[i];
        }

        return dotProduct / (MathF.Sqrt(magnitudeA) * MathF.Sqrt(magnitudeB));
    }
}
```

---

## API Endpoints with Streaming

```csharp
[ApiController]
[Route("api/ai-tutor")]
[Authorize]
public class AITutorController : ControllerBase
{
    /// <summary>
    /// Start a new tutor session
    /// </summary>
    [HttpPost("sessions")]
    [Authorize(Policy = "StudentOnly")]
    public async Task<IActionResult> StartSession(
        [FromBody] StartSessionCommand command,
        CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    /// <summary>
    /// Ask question (with streaming response)
    /// </summary>
    [HttpPost("sessions/{sessionId:guid}/ask")]
    [Authorize(Policy = "StudentOnly")]
    public async Task AskQuestion(
        Guid sessionId,
        [FromBody] AskQuestionRequest request,
        CancellationToken ct)
    {
        Response.Headers.Add("Content-Type", "text/event-stream");
        Response.Headers.Add("Cache-Control", "no-cache");
        Response.Headers.Add("Connection", "keep-alive");

        var session = await _tutorSessionRepo.GetByIdAsync(sessionId, ct);
        if (session == null)
        {
            await Response.WriteAsync("data: {\"error\": \"Session not found\"}\n\n", ct);
            return;
        }

        // Store user message
        session.AddMessage("user", request.Question);

        // Retrieve relevant pages
        var relevantPages = await _ragService.RetrieveRelevantPagesAsync(
            request.Question,
            session.TextbookId,
            topK: 3,
            ct
        );

        var systemPrompt = BuildSocraticSystemPrompt(relevantPages);

        // Stream AI response
        var fullResponse = await _openAIService.GenerateChatCompletionStreamingAsync(
            systemPrompt,
            session.Messages.Select(m => (m.Role, m.Content)).ToList(),
            chunk =>
            {
                Response.WriteAsync($"data: {JsonSerializer.Serialize(new { chunk })}\n\n", ct).Wait();
                Response.Body.FlushAsync(ct).Wait();
            },
            ct
        );

        // Store AI message
        session.AddMessage("assistant", fullResponse);
        await _tutorSessionRepo.SaveChangesAsync(ct);

        await Response.WriteAsync("data: [DONE]\n\n", ct);
    }

    /// <summary>
    /// Get session history
    /// </summary>
    [HttpGet("sessions/{sessionId:guid}")]
    public async Task<IActionResult> GetSession(Guid sessionId, CancellationToken ct)
    {
        var query = new GetSessionByIdQuery(sessionId);
        var result = await _mediator.Send(query, ct);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }
}
```

---

## Implementation Tasks

### Task 5.1: Domain Layer ⏸️

- [ ] **5.1.1** Create `TutorSession`, `TutorMessage` entities
- [ ] **5.1.2** Create `TextbookEmbedding` entity
- [ ] **5.1.3** Create repositories
- [ ] **5.1.4** Write unit tests

### Task 5.2: OpenAI Integration ⏸️

- [ ] **5.2.1** Install Azure.AI.OpenAI NuGet package
- [ ] **5.2.2** Implement `IOpenAIService`
- [ ] **5.2.3** Test chat completion
- [ ] **5.2.4** Test streaming
- [ ] **5.2.5** Test embedding generation

### Task 5.3: RAG Pipeline ⏸️

- [ ] **5.3.1** Implement `IRAGService`
- [ ] **5.3.2** Implement vector similarity search
- [ ] **5.3.3** Test retrieval accuracy
- [ ] **5.3.4** Optimize for performance

### Task 5.4: Socratic Logic ⏸️

- [ ] **5.4.1** Design Socratic system prompt
- [ ] **5.4.2** Add grade-level awareness
- [ ] **5.4.3** Add conversation context management
- [ ] **5.4.4** Test with sample questions

### Task 5.5: API Layer ⏸️

- [ ] **5.5.1** Create `AITutorController`
- [ ] **5.5.2** Implement SSE streaming endpoint
- [ ] **5.5.3** Add session management
- [ ] **5.5.4** Write integration tests

### Task 5.6: Content Integration ⏸️

- [ ] **5.6.1** Add gRPC client for Content service
- [ ] **5.6.2** Fetch textbook pages for embedding
- [ ] **5.6.3** Batch generate embeddings (background job)
- [ ] **5.6.4** Test end-to-end RAG flow

---

## Testing Strategy

```csharp
public class RAGServiceTests
{
    [Fact]
    public async Task RetrieveRelevantPages_ShouldReturnTopKResults()
    {
        // Arrange
        var query = "What is photosynthesis?";
        var embeddings = CreateMockEmbeddings();

        // Act
        var results = await _ragService.RetrieveRelevantPagesAsync(query, null, 3, ct);

        // Assert
        results.Should().HaveCount(3);
        results.Should().Contain(content => content.Contains("photosynthesis"));
    }
}

public class OpenAIServiceTests
{
    [Fact]
    public async Task GenerateChatCompletion_ShouldReturnResponse()
    {
        // Arrange
        var systemPrompt = "You are a helpful tutor.";
        var messages = new List<(string, string)> { ("user", "Hello") };

        // Act
        var response = await _openAIService.GenerateChatCompletionAsync(
            systemPrompt,
            messages,
            CancellationToken.None
        );

        // Assert
        response.Should().NotBeNullOrEmpty();
    }
}
```

---

## Cost Optimization (AWS Free Tier + OpenAI)

### OpenAI Pricing (Estimated for Learning Project)

- **gpt-4o-mini:** $0.15 / 1M input tokens, $0.60 / 1M output tokens
- **text-embedding-3-small:** $0.02 / 1M tokens

**Monthly Cost Estimate (500 active students):**

- 10,000 questions/month × 500 tokens avg = 5M tokens
- Input: 5M × $0.15 / 1M = $0.75
- Output: 5M × $0.60 / 1M = $3.00
- Embeddings: 10,000 pages × 500 tokens = 5M tokens × $0.02 / 1M = $0.10
- **Total: ~$4/month**

**Optimization Tips:**

- Use `gpt-4o-mini` instead of `gpt-4` (10x cheaper)
- Cache embeddings (generate once, reuse forever)
- Limit conversation history to last 5 messages
- Set `max_tokens` to 500

---

## Configuration

```json
{
  "OpenAI": {
    "ApiKey": "sk-xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx",
    "ChatModel": "gpt-4o-mini",
    "EmbeddingModel": "text-embedding-3-small",
    "MaxTokens": 500,
    "Temperature": 0.7
  }
}
```

---

## Validation Checklist

- [ ] Student can start tutor session
- [ ] AI responds with Socratic questions
- [ ] RAG retrieves relevant textbook content
- [ ] Responses stream to frontend
- [ ] Conversation history persisted
- [ ] Embeddings generated for textbooks
- [ ] Vector search returns accurate results
- [ ] Cost stays within budget

---

**Milestone Status:** Ready for Implementation ✅  
**Blocked By:** 03-content-library-feature  
**Blocking:** None  
**Estimated Completion:** 32-40 hours
