# Feature 03: Content Library

**Status:** 📝 Spec Phase  
**Priority:** P1 (High)  
**Effort:** 20-24 hours

---

## 1. Overview

Digital textbook management. Teachers browse textbooks to assign work or generate exams. Assets are stored in R2 (S3 compatible).

**Services:** `Content Service`  
**Frontend:** `/content`

---

## 2. User Stories

| ID  | As a... | I want to...       | So that...                            |
| --- | ------- | ------------------ | ------------------------------------- |
| 3.1 | Teacher | Browse Textbooks   | I can find material by Grade/Subject. |
| 3.2 | Teacher | View Chapter/Pages | I can preview the content.            |
| 3.3 | Admin   | Upload Textbooks   | Content is available in the system.   |
| 3.4 | Teacher | Filter Content     | I can quickly find Grade 5 Math.      |

---

## 3. Technical Specifications

### 3.1 Domain Model (Backend)

**Aggregate Root:** `Textbook`

- **Properties:** `Title`, `Subject`, `Grade`, `Publisher`.
- **Children:** `Chapters` -> `Pages`.

**Entity:** `Page`

- **Properties:** `PageNumber`, `Content` (OCR text), `ImageUrl` (R2 Key).

### 3.2 Database Schema (Neon: `frogedu-content`)

```sql
CREATE TABLE Textbooks (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Title NVARCHAR(200),
    Subject NVARCHAR(50),
    GradeLevel INT,
    CoverImageUrl NVARCHAR(500)
);

CREATE TABLE Chapters (
    Id UNIQUEIDENTIFIER,
    TextbookId UNIQUEIDENTIFIER REFERENCES Textbooks(Id),
    Title NVARCHAR(200),
    OrderIndex INT
);

CREATE TABLE Pages (
    Id UNIQUEIDENTIFIER,
    ChapterId UNIQUEIDENTIFIER REFERENCES Chapters(Id),
    PageNumber INT,
    S3Key NVARCHAR(200),
    ImageUrl NVARCHAR(500) -- Presigned URL generated on read? Or public CDN URL?
);
```

### 3.3 S3/R2 Structure

`edu-classroom-assets/textbooks/{textbookId}/pages/{pageNumber}.jpg`

---

## 4. Implementation Checklist

### 4.1 Backend (Content Service)

- [ ] **Domain**: `Textbook`, `Chapter`, `Page` entities.
- [ ] **Infrastructure**:
  - [ ] `S3Service`: Generate Presigned URLs.
  - [ ] EF Core Configuration for deep hierarchy.
- [ ] **Application**:
  - [ ] `GetTextbooksQuery` (with filters).
  - [ ] `GetTextbookDetailQuery` (Hierarchy).
  - [ ] `UploadTextbookCommand` (Admin only).

### 4.2 Frontend (Content Feature)

- [ ] **Components**:
  - [ ] `TextbookGrid`: Display covers.
  - [ ] `ChapterList`: Accordion view.
  - [ ] `PageViewer`: Image preview + Navigation (Next/Prev).
  - [ ] `Filters`: Grade/Subject dropdowns.
- [ ] **Pages**:
  - [ ] `LibraryPage`
  - [ ] `TextbookViewerPage`

---

## 5. Acceptance Criteria

- [ ] Supports filtering by Grade (1-5) and Subject.
- [ ] Page viewer allows smooth navigation.
- [ ] Images load from R2/CloudFront.
