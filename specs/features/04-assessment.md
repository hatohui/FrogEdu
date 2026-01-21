# Feature 04: Assessment & Exam Generator

**Status:** 📝 Spec Phase  
**Priority:** P1 (High)  
**Effort:** 24-28 hours

---

## 1. Overview

The core value proposition: "Smart Exam Generator". Teachers define a matrix (difficulty/chapters), and the system generates an exam from the question bank.

**Services:** `Assessment Service`  
**Frontend:** `/assessment`

---

## 2. User Stories

| ID  | As a... | I want to...       | So that...                                                |
| --- | ------- | ------------------ | --------------------------------------------------------- |
| 4.1 | Teacher | Define Exam Matrix | The test covers specific chapters and difficulty balance. |
| 4.2 | Teacher | Auto-Generate Exam | I save time selecting questions manually.                 |
| 4.3 | Teacher | Swap Questions     | I can replace questions I don't like.                     |
| 4.4 | Teacher | Export to PDF      | I can print it for class.                                 |

---

## 3. Technical Specifications

### 3.1 Domain Model (Backend)

**Aggregate Root:** `Exam`

- **Properties:** `ClassId`, `Matrix` (JSON/ValueObject), `Status`.
- **Collections:** `ExamQuestions`.

**Aggregate Root:** `Question` (Question Bank)

- **Properties:** `Content`, `Type` (MCQ/Essay), `Difficulty`, `ChapterId`.

### 3.2 Database Schema (Neon: `frogedu-assessments`)

```sql
CREATE TABLE Questions (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Content NVARCHAR(MAX),
    Type INT,
    Difficulty INT, -- 1=Easy, 2=Medium, 3=Hard
    ChapterId UNIQUEIDENTIFIER -- Link to Content Service? (Loosely coupled via GUID)
);

CREATE TABLE Exams (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    TeacherId UNIQUEIDENTIFIER,
    MatrixConfig NVARCHAR(MAX), -- Store config used
    PdfUrl NVARCHAR(500)
);

CREATE TABLE ExamQuestions (
    ExamId UNIQUEIDENTIFIER,
    QuestionId UNIQUEIDENTIFIER,
    OrderIndex INT
);
```

### 3.3 Frontend Architecture

**Wizard Flow:**

1.  **Setup:** Name, Class, Duration.
2.  **Matrix:** Select Chapters, set % Easy/Med/Hard.
3.  **Preview:** View generated list, Swap button per question.
4.  **Finalize:** Export PDF.

---

## 4. Implementation Checklist

### 4.1 Backend (Assessment Service)

- [ ] **Domain**:
  - [ ] `Question` entity.
  - [ ] `Exam` entity logic (`Generate` method utilizing a domain service).
  - [ ] `ExamGenerationService`: Logic to pick random questions matching matrix.
- [ ] **Application**:
  - [ ] `GenerateExamCommand`: Input Matrix -> Output Draft Exam.
  - [ ] `FinalizeExamCommand`: Generate PDF -> S3 -> Save URL.
  - [ ] `SwapQuestionCommand`: Find alternative with same difficulty/chapter.
- [ ] **Infrastructure**:
  - [ ] `PdfGenerationService`: Use library (e.g., QuestPDF) to render PDF.

### 4.2 Frontend (Assessment Feature)

- [ ] **Components**:
  - [ ] `MatrixBuilder`: Sliders/Inputs for difficulty.
  - [ ] `ChapterSelector`: Tree view.
  - [ ] `ExamPreview`: List with "Swap" action.
  - [ ] `PdfViewer`: Preview before download.
- [ ] **Pages**:
  - [ ] `ExamWizardPage` (Multi-step).
  - [ ] `ExamListPage`.

---

## 5. Acceptance Criteria

- [ ] Matrix respects total question count (e.g., 20) and distribution (e.g., 50% Easy).
- [ ] "Swap" finds a different question of same Difficulty + Chapter.
- [ ] Generated PDF is clean, readable, and includes Answer Key (separate page).
