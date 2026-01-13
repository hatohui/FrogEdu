# Content Library Feature

**Feature:** Digital Textbook & Content Management  
**Service:** Content Service (Backend)  
**Version:** 1.0  
**Status:** Specification Phase

---

## Table of Contents

1. [Overview](#overview)
2. [User Stories](#user-stories)
3. [UI/UX Requirements](#uiux-requirements)
4. [Component Architecture](#component-architecture)
5. [API Integration](#api-integration)
6. [State Management](#state-management)
7. [Implementation Tasks](#implementation-tasks)
8. [Acceptance Criteria](#acceptance-criteria)

---

## Overview

This feature allows teachers to browse, search, and view the digital textbook library. Teachers can filter by grade level, subject, and chapter, and preview textbook pages stored in S3.

### Responsibilities

- ✅ Display textbook catalog with filtering
- ✅ Show textbook details (chapters, pages)
- ✅ Preview textbook pages (PDF/images from S3)
- ✅ Search textbooks by title, subject, grade
- ✅ Navigate chapter hierarchy
- ✅ Provide textbook selection for exam generation

---

## User Stories

### Story 3.1: Browse Textbook Library

**As a** Teacher  
**I want to** browse the digital textbook library  
**So that** I can find curriculum materials for my lessons

**Acceptance Criteria:**

- [ ] Display grid/list of available textbooks
- [ ] Show textbook cover, title, grade, subject
- [ ] Filter by grade level (1-5)
- [ ] Filter by subject (Math, Vietnamese, English, Science)
- [ ] Search by textbook title
- [ ] Click textbook to view details
- [ ] Loading state while fetching
- [ ] Empty state if no textbooks found

### Story 3.2: View Textbook Details

**As a** Teacher  
**I want to** view a textbook's chapters and pages  
**So that** I can navigate to specific content

**Acceptance Criteria:**

- [ ] Display textbook metadata (title, grade, subject, publisher)
- [ ] Show list of chapters in order
- [ ] Expand chapter to see pages
- [ ] Click page to preview
- [ ] Breadcrumb navigation (Library > Textbook > Chapter)
- [ ] Back button to return to library

### Story 3.3: Preview Textbook Pages

**As a** Teacher  
**I want to** preview textbook pages  
**So that** I can review content before using it

**Acceptance Criteria:**

- [ ] Display page content (PDF or image from S3)
- [ ] Navigate between pages (prev/next)
- [ ] Zoom in/out functionality
- [ ] Download button for offline access
- [ ] Loading state while loading page
- [ ] Error state if page fails to load

---

## UI/UX Requirements

### Page: Content Library (`/content`)

**Layout:**

```
┌────────────────────────────────────────────────┐
│  Content Library                               │
├────────────────────────────────────────────────┤
│  [Search: Enter textbook name...]             │
│                                                │
│  Filters:                                      │
│  [Grade: All ▼] [Subject: All ▼]             │
│                                                │
├────────────────────────────────────────────────┤
│                                                │
│  ┌──────────┐  ┌──────────┐  ┌──────────┐   │
│  │ [Cover]  │  │ [Cover]  │  │ [Cover]  │   │
│  │ Math G5  │  │ Viet G4  │  │ Sci G5   │   │
│  │ 2024     │  │ 2024     │  │ 2024     │   │
│  └──────────┘  └──────────┘  └──────────┘   │
│                                                │
│  ┌──────────┐  ┌──────────┐  ┌──────────┐   │
│  │ [Cover]  │  │ [Cover]  │  │ [Cover]  │   │
│  └──────────┘  └──────────┘  └──────────┘   │
│                                                │
└────────────────────────────────────────────────┘
```

**Styling:**

- Grid layout (3-4 columns on desktop, 2 on tablet, 1 on mobile)
- Card component for each textbook
- Hover effect on cards
- Filter dropdowns in header
- Search bar with debounced input

### Page: Textbook Detail (`/content/:textbookId`)

**Layout:**

```
┌────────────────────────────────────────────────┐
│  ← Back to Library                             │
├────────────────────────────────────────────────┤
│  ┌────────┐  Mathematics Grade 5               │
│  │ Cover  │  Publisher: Vietnam Education      │
│  │ Image  │  Year: 2024                        │
│  └────────┘  [Use in Exam Generation]          │
│                                                │
├────────────────────────────────────────────────┤
│  Chapters:                                     │
│                                                │
│  ▼ Chapter 1: Numbers and Operations          │
│    • Page 1-5: Introduction                   │
│    • Page 6-12: Addition                      │
│    • Page 13-20: Subtraction                  │
│                                                │
│  ▶ Chapter 2: Fractions                       │
│                                                │
│  ▶ Chapter 3: Geometry                        │
│                                                │
└────────────────────────────────────────────────┘
```

### Component: Page Preview Modal

**Layout:**

```
┌────────────────────────────────────────────────┐
│  Page 5 of 120                           [✕]  │
├────────────────────────────────────────────────┤
│                                                │
│              [Textbook Page Image]             │
│                                                │
│                                                │
│                                                │
├────────────────────────────────────────────────┤
│  [←] Previous     [Download]     Next [→]     │
│                   [− Zoom +]                   │
└────────────────────────────────────────────────┘
```

---

## Component Architecture

### Component Tree

```
pages/
└── (dashboard)/
    └── content/
        ├── page.tsx                    # Content library page
        └── [textbookId]/
            └── page.tsx                # Textbook detail page

features/content/
├── components/
│   ├── TextbookCard.tsx               # Single textbook card
│   ├── TextbookGrid.tsx               # Grid of textbook cards
│   ├── TextbookFilters.tsx            # Filter controls
│   ├── TextbookDetail.tsx             # Textbook info display
│   ├── ChapterList.tsx                # List of chapters (accordion)
│   ├── ChapterItem.tsx                # Single chapter with pages
│   ├── PageList.tsx                   # List of pages in chapter
│   ├── PagePreviewModal.tsx           # Modal for page preview
│   ├── PageViewer.tsx                 # PDF/Image viewer
│   └── SearchBar.tsx                  # Search input with debounce
├── hooks/
│   ├── useTextbooks.ts                # Query: List textbooks
│   ├── useTextbook.ts                 # Query: Single textbook
│   ├── useChapters.ts                 # Query: Chapters of textbook
│   ├── usePages.ts                    # Query: Pages of chapter
│   └── usePagePreview.ts              # Get presigned URL for page
├── services/
│   └── content.service.ts             # API calls
├── types/
│   └── content.types.ts               # TypeScript interfaces
└── utils/
    └── content.utils.ts               # Helper functions
```

### Component Implementations

#### 1. TextbookCard (Presentational)

```typescript
// features/content/components/TextbookCard.tsx

import { Card, CardContent, CardFooter } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import type { Textbook } from "../types/content.types";

interface TextbookCardProps {
  textbook: Textbook;
  onClick: (id: string) => void;
}

export function TextbookCard({ textbook, onClick }: TextbookCardProps) {
  return (
    <Card
      className="cursor-pointer transition-all hover:shadow-lg hover:scale-105"
      onClick={() => onClick(textbook.id)}
    >
      <CardContent className="p-0">
        <div className="aspect-[3/4] relative">
          <img
            src={textbook.coverImageUrl || "/placeholder-book.png"}
            alt={textbook.title}
            className="w-full h-full object-cover rounded-t-lg"
          />
          <Badge className="absolute top-2 right-2" variant="secondary">
            Grade {textbook.gradeLevel}
          </Badge>
        </div>
      </CardContent>
      <CardFooter className="flex flex-col items-start p-4">
        <h3 className="font-semibold text-lg line-clamp-2">{textbook.title}</h3>
        <p className="text-sm text-muted-foreground">{textbook.subject}</p>
        <p className="text-xs text-muted-foreground">
          {textbook.publisher} • {textbook.publicationYear}
        </p>
      </CardFooter>
    </Card>
  );
}
```

#### 2. TextbookFilters

```typescript
// features/content/components/TextbookFilters.tsx

import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import type { TextbookFilters as Filters } from "../types/content.types";

interface TextbookFiltersProps {
  filters: Filters;
  onFilterChange: (key: keyof Filters, value: string) => void;
}

const GRADES = ["All", "1", "2", "3", "4", "5"];
const SUBJECTS = [
  "All",
  "Mathematics",
  "Vietnamese",
  "English",
  "Science",
  "History",
];

export function TextbookFilters({
  filters,
  onFilterChange,
}: TextbookFiltersProps) {
  return (
    <div className="flex flex-wrap gap-4">
      <div className="flex-1 min-w-[200px]">
        <Select
          value={filters.gradeLevel || "All"}
          onValueChange={(value) =>
            onFilterChange("gradeLevel", value === "All" ? "" : value)
          }
        >
          <SelectTrigger>
            <SelectValue placeholder="Grade Level" />
          </SelectTrigger>
          <SelectContent>
            {GRADES.map((grade) => (
              <SelectItem key={grade} value={grade}>
                {grade === "All" ? "All Grades" : `Grade ${grade}`}
              </SelectItem>
            ))}
          </SelectContent>
        </Select>
      </div>

      <div className="flex-1 min-w-[200px]">
        <Select
          value={filters.subject || "All"}
          onValueChange={(value) =>
            onFilterChange("subject", value === "All" ? "" : value)
          }
        >
          <SelectTrigger>
            <SelectValue placeholder="Subject" />
          </SelectTrigger>
          <SelectContent>
            {SUBJECTS.map((subject) => (
              <SelectItem key={subject} value={subject}>
                {subject}
              </SelectItem>
            ))}
          </SelectContent>
        </Select>
      </div>
    </div>
  );
}
```

#### 3. ChapterList (with Accordion)

```typescript
// features/content/components/ChapterList.tsx

import {
  Accordion,
  AccordionContent,
  AccordionItem,
  AccordionTrigger,
} from "@/components/ui/accordion";
import { Button } from "@/components/ui/button";
import type { Chapter } from "../types/content.types";

interface ChapterListProps {
  chapters: Chapter[];
  onPageClick: (pageId: string) => void;
}

export function ChapterList({ chapters, onPageClick }: ChapterListProps) {
  return (
    <Accordion type="single" collapsible className="w-full">
      {chapters.map((chapter) => (
        <AccordionItem key={chapter.id} value={chapter.id}>
          <AccordionTrigger>
            <div className="flex items-center gap-2">
              <span className="font-semibold">
                Chapter {chapter.chapterNumber}:
              </span>
              <span>{chapter.title}</span>
            </div>
          </AccordionTrigger>
          <AccordionContent>
            {chapter.description && (
              <p className="text-sm text-muted-foreground mb-4">
                {chapter.description}
              </p>
            )}
            <div className="space-y-2">
              {chapter.pages.map((page) => (
                <Button
                  key={page.id}
                  variant="ghost"
                  className="w-full justify-start"
                  onClick={() => onPageClick(page.id)}
                >
                  Page {page.pageNumber}
                </Button>
              ))}
            </div>
          </AccordionContent>
        </AccordionItem>
      ))}
    </Accordion>
  );
}
```

#### 4. Content Library Page (Container)

```typescript
// pages/(dashboard)/content/page.tsx

import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useTextbooks } from "@/features/content/hooks/useTextbooks";
import { TextbookGrid } from "@/features/content/components/TextbookGrid";
import { TextbookFilters } from "@/features/content/components/TextbookFilters";
import { SearchBar } from "@/features/content/components/SearchBar";
import { PageHeader } from "@/components/common/PageHeader";
import { LoadingSpinner } from "@/components/common/LoadingSpinner";
import type { TextbookFilters as Filters } from "@/features/content/types/content.types";

export default function ContentLibraryPage() {
  const navigate = useNavigate();
  const [filters, setFilters] = useState<Filters>({
    gradeLevel: "",
    subject: "",
    search: "",
  });

  const { data: textbooks, isLoading } = useTextbooks(filters);

  const handleFilterChange = (key: keyof Filters, value: string) => {
    setFilters((prev) => ({ ...prev, [key]: value }));
  };

  const handleTextbookClick = (textbookId: string) => {
    navigate(`/content/${textbookId}`);
  };

  return (
    <div className="container mx-auto py-6">
      <PageHeader
        title="Content Library"
        description="Browse digital textbooks and curriculum materials"
      />

      <div className="mt-6 space-y-4">
        <SearchBar
          value={filters.search || ""}
          onChange={(value) => handleFilterChange("search", value)}
          placeholder="Search textbooks..."
        />

        <TextbookFilters
          filters={filters}
          onFilterChange={handleFilterChange}
        />

        {isLoading ? (
          <LoadingSpinner />
        ) : (
          <TextbookGrid
            textbooks={textbooks ?? []}
            onTextbookClick={handleTextbookClick}
          />
        )}
      </div>
    </div>
  );
}
```

#### 5. Textbook Detail Page

```typescript
// pages/(dashboard)/content/[textbookId]/page.tsx

import { useParams, useNavigate } from "react-router-dom";
import { useState } from "react";
import { useTextbook } from "@/features/content/hooks/useTextbook";
import { TextbookDetail } from "@/features/content/components/TextbookDetail";
import { ChapterList } from "@/features/content/components/ChapterList";
import { PagePreviewModal } from "@/features/content/components/PagePreviewModal";
import { Button } from "@/components/ui/button";
import { ArrowLeft } from "lucide-react";
import { LoadingSpinner } from "@/components/common/LoadingSpinner";

export default function TextbookDetailPage() {
  const { textbookId } = useParams<{ textbookId: string }>();
  const navigate = useNavigate();
  const [selectedPageId, setSelectedPageId] = useState<string | null>(null);

  const { data: textbook, isLoading } = useTextbook(textbookId!);

  if (isLoading) {
    return <LoadingSpinner fullScreen />;
  }

  if (!textbook) {
    return <div>Textbook not found</div>;
  }

  return (
    <div className="container mx-auto py-6">
      <Button
        variant="ghost"
        onClick={() => navigate("/content")}
        className="mb-4"
      >
        <ArrowLeft className="mr-2 h-4 w-4" />
        Back to Library
      </Button>

      <TextbookDetail textbook={textbook} />

      <div className="mt-8">
        <h2 className="text-2xl font-bold mb-4">Chapters</h2>
        <ChapterList
          chapters={textbook.chapters}
          onPageClick={setSelectedPageId}
        />
      </div>

      {selectedPageId && (
        <PagePreviewModal
          pageId={selectedPageId}
          onClose={() => setSelectedPageId(null)}
        />
      )}
    </div>
  );
}
```

---

## API Integration

### Endpoints (Content Service)

| Method | Endpoint                         | Description                        |
| ------ | -------------------------------- | ---------------------------------- |
| GET    | `/api/content/textbooks`         | List textbooks with filters        |
| GET    | `/api/content/textbooks/:id`     | Get textbook with chapters         |
| GET    | `/api/content/chapters/:id`      | Get chapter with pages             |
| GET    | `/api/content/pages/:id`         | Get page metadata                  |
| GET    | `/api/content/pages/:id/preview` | Get presigned URL for page content |

### Query Parameters

```typescript
interface TextbookQueryParams {
  gradeLevel?: number;
  subject?: string;
  search?: string;
  page?: number;
  pageSize?: number;
}
```

### Response Types

```typescript
// features/content/types/content.types.ts

export interface Textbook {
  id: string;
  title: string;
  subject: string;
  gradeLevel: number;
  publisher: string;
  publicationYear: number;
  coverImageUrl?: string;
  isActive: boolean;
  chapters: Chapter[];
}

export interface Chapter {
  id: string;
  textbookId: string;
  chapterNumber: number;
  title: string;
  description?: string;
  orderIndex: number;
  pages: Page[];
}

export interface Page {
  id: string;
  chapterId: string;
  pageNumber: number;
  contentType: "pdf" | "image";
}

export interface PagePreview {
  pageId: string;
  presignedUrl: string;
  expiresIn: number; // seconds
  contentType: string;
}

export interface TextbookFilters {
  gradeLevel?: string;
  subject?: string;
  search?: string;
}
```

---

## State Management

### TanStack Query Hooks

```typescript
// features/content/hooks/useTextbooks.ts

import { useQuery } from "@tanstack/react-query";
import { contentService } from "../services/content.service";
import type { TextbookFilters } from "../types/content.types";

export const contentKeys = {
  all: ["content"] as const,
  textbooks: () => [...contentKeys.all, "textbooks"] as const,
  textbookList: (filters: TextbookFilters) =>
    [...contentKeys.textbooks(), filters] as const,
  textbook: (id: string) => [...contentKeys.textbooks(), id] as const,
  chapters: (textbookId: string) =>
    [...contentKeys.textbook(textbookId), "chapters"] as const,
  pages: (chapterId: string) =>
    [...contentKeys.all, "pages", chapterId] as const,
  pagePreview: (pageId: string) =>
    [...contentKeys.all, "preview", pageId] as const,
};

export function useTextbooks(filters: TextbookFilters) {
  return useQuery({
    queryKey: contentKeys.textbookList(filters),
    queryFn: () => contentService.getTextbooks(filters),
    staleTime: 5 * 60 * 1000, // 5 minutes (content rarely changes)
  });
}

export function useTextbook(id: string) {
  return useQuery({
    queryKey: contentKeys.textbook(id),
    queryFn: () => contentService.getTextbook(id),
    enabled: !!id,
  });
}

export function usePagePreview(pageId: string) {
  return useQuery({
    queryKey: contentKeys.pagePreview(pageId),
    queryFn: () => contentService.getPagePreview(pageId),
    enabled: !!pageId,
    staleTime: 10 * 60 * 1000, // 10 minutes (presigned URLs valid for 15 min)
  });
}
```

### Service Implementation

```typescript
// features/content/services/content.service.ts

import api from "@/services/axios";
import type {
  Textbook,
  Chapter,
  Page,
  PagePreview,
  TextbookFilters,
} from "../types/content.types";

export const contentService = {
  getTextbooks: async (filters: TextbookFilters): Promise<Textbook[]> => {
    const params = new URLSearchParams();
    if (filters.gradeLevel) params.append("gradeLevel", filters.gradeLevel);
    if (filters.subject) params.append("subject", filters.subject);
    if (filters.search) params.append("search", filters.search);

    const response = await api.get<Textbook[]>(
      `/api/content/textbooks?${params.toString()}`
    );
    return response.data;
  },

  getTextbook: async (id: string): Promise<Textbook> => {
    const response = await api.get<Textbook>(`/api/content/textbooks/${id}`);
    return response.data;
  },

  getChapter: async (id: string): Promise<Chapter> => {
    const response = await api.get<Chapter>(`/api/content/chapters/${id}`);
    return response.data;
  },

  getPagePreview: async (pageId: string): Promise<PagePreview> => {
    const response = await api.get<PagePreview>(
      `/api/content/pages/${pageId}/preview`
    );
    return response.data;
  },
};
```

---

## Implementation Tasks

### Milestone 1: Textbook Library

#### Task 1.1: Install Shadcn Components - [ ]

- [ ] Install Card component
- [ ] Install Badge component
- [ ] Install Select component
- [ ] Install Input component (search)
- [ ] Install Accordion component

#### Task 1.2: Create Content Types - [ ]

- [ ] Define TypeScript interfaces
- [ ] Export types from types file

#### Task 1.3: Implement Content Service - [ ]

- [ ] Create API service functions
- [ ] Implement error handling

#### Task 1.4: Create Query Hooks - [ ]

- [ ] Implement useTextbooks hook
- [ ] Implement useTextbook hook
- [ ] Implement usePagePreview hook
- [ ] Define query keys

#### Task 1.5: Create TextbookCard Component - [ ]

- [ ] Create presentational component
- [ ] Display cover, title, metadata
- [ ] Add hover effects
- [ ] Style with Tailwind

**Validation:**

- [ ] Card displays all information correctly
- [ ] Hover effect works smoothly
- [ ] Click handler triggers correctly

#### Task 1.6: Create TextbookGrid Component - [ ]

- [ ] Create grid container
- [ ] Map textbooks to cards
- [ ] Handle empty state
- [ ] Responsive grid layout

#### Task 1.7: Create Search & Filters - [ ]

- [ ] Create SearchBar component with debounce
- [ ] Create TextbookFilters component
- [ ] Wire up to state

**Validation:**

- [ ] Search debounces input (500ms)
- [ ] Filters update results
- [ ] Clear filters works

#### Task 1.8: Create Content Library Page - [ ]

- [ ] Create page component
- [ ] Integrate all components
- [ ] Handle loading state
- [ ] Handle empty state

**Validation:**

- [ ] Page loads textbooks correctly
- [ ] Filters work end-to-end
- [ ] Navigation to detail works

### Milestone 2: Textbook Detail

#### Task 2.1: Create TextbookDetail Component - [ ]

- [ ] Display textbook metadata
- [ ] Show cover image
- [ ] Add action buttons

#### Task 2.2: Create ChapterList Component - [ ]

- [ ] Use Accordion component
- [ ] Display chapters in order
- [ ] Show pages in each chapter
- [ ] Handle expand/collapse

**Validation:**

- [ ] Accordion works correctly
- [ ] Chapters display in order
- [ ] Pages are clickable

#### Task 2.3: Create Page Preview Modal - [ ]

- [ ] Install Dialog component
- [ ] Create modal layout
- [ ] Fetch presigned URL
- [ ] Display page content (image/PDF)
- [ ] Add prev/next navigation
- [ ] Add zoom controls
- [ ] Add download button

**Validation:**

- [ ] Modal opens with correct page
- [ ] Content loads from S3
- [ ] Navigation works
- [ ] Zoom works
- [ ] Download works

#### Task 2.4: Create Textbook Detail Page - [ ]

- [ ] Create page component
- [ ] Fetch textbook data
- [ ] Display detail and chapters
- [ ] Handle page preview modal

**Validation:**

- [ ] All data displays correctly
- [ ] Page preview works end-to-end
- [ ] Back button works

### Milestone 3: Polish & Optimization

#### Task 3.1: Add Loading Skeletons - [ ]

- [ ] Create skeleton for textbook cards
- [ ] Create skeleton for detail page
- [ ] Replace loading spinner

#### Task 3.2: Error Handling - [ ]

- [ ] Handle network errors
- [ ] Handle 404 (not found)
- [ ] Show user-friendly messages

#### Task 3.3: Performance Optimization - [ ]

- [ ] Lazy load images
- [ ] Optimize cover image sizes
- [ ] Implement virtual scrolling for large lists

#### Task 3.4: Accessibility - [ ]

- [ ] Add ARIA labels
- [ ] Test keyboard navigation
- [ ] Test screen reader

---

## Acceptance Criteria

### Definition of Done

#### Functionality

- [ ] Teachers can browse textbooks
- [ ] Filters work correctly (grade, subject, search)
- [ ] Textbook detail shows all chapters and pages
- [ ] Page preview modal works with S3 content
- [ ] Navigation works throughout

#### Code Quality

- [ ] All components are typed
- [ ] Separation of concerns maintained
- [ ] TanStack Query used for all data fetching
- [ ] Shadcn components used

#### UX

- [ ] Loading states on all async operations
- [ ] Empty states handled
- [ ] Error messages are clear
- [ ] Responsive design works
- [ ] Smooth transitions and animations

#### Performance

- [ ] Initial load < 2 seconds
- [ ] Images lazy loaded
- [ ] No unnecessary re-renders

---

**Next Feature:** [Smart Exam Generator](./04-assessment-feature.md)
