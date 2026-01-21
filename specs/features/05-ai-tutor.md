# Feature 05: AI Student Tutor

**Status:** 📝 Spec Phase  
**Priority:** P2 (Medium)  
**Effort:** 30-40 hours

---

## 1. Overview

RAG-powered chat interface for Students. The AI acts as a Socratic tutor, using textbook content as the source of truth, and guiding students rather than giving direct answers.

**Services:** `AI Service`  
**Frontend:** `/tutor`

---

## 2. User Stories

| ID  | As a... | I want to...            | So that...                      |
| --- | ------- | ----------------------- | ------------------------------- |
| 5.1 | Student | Ask a question          | I can get help with homework.   |
| 5.2 | Student | Get hints (not answers) | I learn the concept myself.     |
| 5.3 | Student | See textbook refs       | I can read the source material. |

---

## 3. Technical Specifications

### 3.1 RAG Pipeline

1.  **Ingestion:** Textbooks processed into embeddings (OpenAI text-embedding-3-small).
2.  **Retrieval:** Vector search (pgvector in Neon?) or simple keyword/chapter match for V1. _Spec suggests Vector Search_.
3.  **Generation:** GPT-4o-mini with System Prompt: "You are a Socratic tutor..."

### 3.2 Database Schema (Neon: `frogedu-ai`)

```sql
CREATE TABLE TutorSessions (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    StudentId UNIQUEIDENTIFIER,
    StartedAt DATETIME2
);

CREATE TABLE Messages (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    SessionId UNIQUEIDENTIFIER,
    Role NVARCHAR(20), -- User/Assistant
    Content NVARCHAR(MAX),
    Citations NVARCHAR(MAX) -- JSON array of Page Ids
);

-- Vector Store (If using pgvector)
-- CREATE EXTENSION vector;
-- CREATE TABLE Embeddings ...
```

### 3.3 Frontend Architecture

**Components:**

- `ChatInterface`: Scrollable message list.
- `MessageBubble`: Standard vs AI (Markdown support).
- `CitationCard`: Link to Textbook viewer.
- `InputArea`: Text area + Send.

---

## 4. Implementation Checklist

### 4.1 Backend (AI Service)

- [ ] **Infrastructure**:
  - [ ] Integration with Gemini/OpenAI API.
  - [ ] Vector Store setup (or mocked for V1).
- [ ] **Application**:
  - [ ] `AskTutorCommand`: Input text -> RAG -> Stream response.
  - [ ] `IngestTextbookCommand`: Process PDF pages -> Embeddings.
- [ ] **Prompts**:
  - [ ] Tune System Prompt for Socratic method and 1st-5th grade tone.

### 4.2 Frontend (Tutor Feature)

- [ ] **UI**:
  - [ ] Implement Chat UI (similar to ChatGPT).
  - [ ] Handle Streaming responses (Server-Sent Events or just await for V1).
  - [ ] Markdown rendering for Math formulas (KaTeX).

---

## 5. Acceptance Criteria

- [ ] AI refuses to give direct answers (e.g., "The answer is 5") but gives steps.
- [ ] AI references specific textbook pages ("See Page 42").
- [ ] Chat history is saved per session.
