# Frontend Overview & Architecture

**Version:** 2.0  
**Last Updated:** January 13, 2026  
**Status:** Specification Phase

---

## Table of Contents

1. [Technology Stack](#technology-stack)
2. [Architecture Principles](#architecture-principles)
3. [Project Structure](#project-structure)
4. [Routing Strategy](#routing-strategy)
5. [State Management](#state-management)
6. [Component Guidelines](#component-guidelines)
7. [Development Workflow](#development-workflow)

---

## Technology Stack

### Core Technologies

- **Framework:** React 19
- **Build Tool:** Vite 6.x
- **Language:** TypeScript 5.x (Strict Mode Enabled)
- **Routing:** React Router v7
- **Styling:** Tailwind CSS 4.x
- **Component Library:** Shadcn UI (Radix Primitives)
- **State Management:** TanStack Query v5 (React Query)
- **Internationalization:** i18next
- **Form Management:** React Hook Form + Zod Validation
- **HTTP Client:** Axios

### Development Tools

- **Linting:** ESLint 9.x
- **Formatting:** Prettier
- **Type Checking:** TypeScript Compiler
- **Testing:** Vitest + React Testing Library
- **E2E Testing:** Playwright

---

## Architecture Principles

### 1. Feature-Based Organization

Organize code by features/domains that **mirror backend microservices**:

```
features/
├── auth/           → User Service
├── content/        → Content Service
├── assessment/     → Assessment Service
├── ai-tutor/       → AI Service
└── user/           → User Service
```

**Benefits:**

- Clear domain boundaries
- Easy to locate related code
- Scalable as features grow
- Aligns with backend architecture

### 2. Separation of Concerns

Each feature follows this structure:

```
features/[feature-name]/
├── components/        # UI components specific to this feature
│   ├── [FeatureName].tsx
│   ├── [FeatureName]Form.tsx
│   └── [FeatureName]List.tsx
├── hooks/            # Custom hooks for this feature
│   ├── use[FeatureName].ts
│   ├── use[FeatureName]Mutations.ts
│   └── use[FeatureName]Queries.ts
├── services/         # API client functions
│   └── [feature-name].service.ts
├── types/            # TypeScript interfaces/types
│   └── [feature-name].types.ts
├── utils/            # Feature-specific utilities
│   └── [feature-name].utils.ts
└── constants.ts      # Feature constants
```

### 3. Component Hierarchy

```
┌─────────────────────────────────────┐
│          Page Components            │  (/pages/[feature]/page.tsx)
│  (Data Fetching, Route Params)      │
└──────────────┬──────────────────────┘
               │
┌──────────────▼──────────────────────┐
│      Container Components           │  (features/[feature]/components/)
│  (Business Logic, State Management) │
└──────────────┬──────────────────────┘
               │
┌──────────────▼──────────────────────┐
│    Presentational Components        │  (features/[feature]/components/)
│        (Pure UI, Props Only)        │
└──────────────┬──────────────────────┘
               │
┌──────────────▼──────────────────────┐
│         UI Primitives               │  (components/ui/)
│     (Shadcn Components)             │
└─────────────────────────────────────┘
```

### 4. Data Flow

```
User Action
    ↓
Event Handler (Component)
    ↓
Custom Hook (useFeatureMutation)
    ↓
Service Function (API Call)
    ↓
TanStack Query (Cache Update)
    ↓
Component Re-render
```

---

## Project Structure

```
frontend/
├── public/
│   ├── _redirects              # Cloudflare Pages redirects
│   └── languages/              # Static translation files
│       ├── en.json
│       └── vi.json
├── src/
│   ├── common/                 # Shared constants, enums
│   │   └── constants.ts
│   ├── components/
│   │   ├── ui/                 # Shadcn primitives (auto-generated)
│   │   │   ├── button.tsx
│   │   │   ├── card.tsx
│   │   │   ├── dialog.tsx
│   │   │   └── ...
│   │   └── common/             # Shared composite components
│   │       ├── Container.tsx
│   │       ├── PageHeader.tsx
│   │       ├── LoadingSpinner.tsx
│   │       └── ErrorBoundary.tsx
│   ├── config/                 # App-wide configuration
│   │   ├── dynamicRouter.tsx   # Route loader (Next.js-style)
│   │   ├── i18n.tsx           # i18next setup
│   │   ├── theme.tsx          # Theme provider
│   │   └── tanstack.ts        # TanStack Query config
│   ├── features/              # Feature modules (see below)
│   ├── hooks/                 # Global hooks
│   │   ├── useAuth.tsx
│   │   ├── useTheme.tsx
│   │   └── useToast.tsx
│   ├── languages/             # Translation files
│   │   ├── index.ts
│   │   └── en/
│   │       └── en.ts
│   ├── pages/                 # Route entry points (Next.js-style)
│   │   ├── layout.tsx         # Root layout
│   │   ├── page.tsx           # Home page
│   │   ├── (auth)/            # Auth group
│   │   │   ├── login/
│   │   │   │   └── page.tsx
│   │   │   └── register/
│   │   │       └── page.tsx
│   │   ├── (dashboard)/       # Dashboard group
│   │   │   ├── layout.tsx     # Dashboard layout
│   │   │   ├── dashboard/
│   │   │   │   └── page.tsx
│   │   │   ├── content/
│   │   │   ├── assessment/
│   │   │   └── profile/
│   │   └── (student)/         # Student group
│   │       └── tutor/
│   │           └── page.tsx
│   ├── services/              # Global API utilities
│   │   └── axios.ts           # Axios instance
│   ├── utils/                 # Global utilities
│   │   ├── cn.ts              # Class name merger
│   │   ├── localStorage.ts
│   │   └── searchParams.ts
│   ├── App.tsx                # App root
│   ├── main.tsx               # Entry point
│   └── global.css             # Global styles + Tailwind
├── components.json            # Shadcn config
├── tsconfig.json              # TypeScript config
├── vite.config.ts             # Vite config
└── package.json
```

---

## Routing Strategy

### Next.js-Style File-Based Routing

The project uses `dynamicRouter.tsx` to enable Next.js-style routing in React Router:

#### Route Patterns

- **File:** `pages/page.tsx` → **Route:** `/`
- **File:** `pages/about/page.tsx` → **Route:** `/about`
- **File:** `pages/dashboard/page.tsx` → **Route:** `/dashboard`
- **File:** `pages/exam/[id]/page.tsx` → **Route:** `/exam/:id`
- **File:** `pages/(auth)/login/page.tsx` → **Route:** `/login` (group ignored)

#### Layout Nesting

Layouts automatically wrap all nested pages:

```
pages/
├── layout.tsx                 # Root layout (Header, Footer)
└── (dashboard)/
    ├── layout.tsx             # Dashboard layout (Sidebar)
    ├── dashboard/
    │   └── page.tsx           # Wrapped by both layouts
    └── content/
        └── page.tsx           # Wrapped by both layouts
```

#### Route Groups

Groups organize routes without affecting URLs:

- `(auth)` - Authentication pages (special layout)
- `(dashboard)` - Teacher pages (dashboard layout)
- `(student)` - Student pages (minimal layout)

---

## State Management

### TanStack Query (React Query)

**All API state** is managed by TanStack Query.

#### Query Organization

```typescript
// features/content/hooks/useTextbooksQueries.ts

// Query Keys (hierarchical)
export const textbookKeys = {
  all: ["textbooks"] as const,
  lists: () => [...textbookKeys.all, "list"] as const,
  list: (filters: TextbookFilters) =>
    [...textbookKeys.lists(), filters] as const,
  details: () => [...textbookKeys.all, "detail"] as const,
  detail: (id: string) => [...textbookKeys.details(), id] as const,
};

// Query Hook
export function useTextbooks(filters: TextbookFilters) {
  return useQuery({
    queryKey: textbookKeys.list(filters),
    queryFn: () => textbookService.getTextbooks(filters),
    staleTime: 5 * 60 * 1000, // 5 minutes
  });
}

export function useTextbook(id: string) {
  return useQuery({
    queryKey: textbookKeys.detail(id),
    queryFn: () => textbookService.getTextbook(id),
    enabled: !!id,
  });
}
```

#### Mutation Hooks

```typescript
// features/content/hooks/useTextbookMutations.ts

export function useCreateTextbook() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: CreateTextbookDto) =>
      textbookService.createTextbook(data),
    onSuccess: () => {
      // Invalidate and refetch
      queryClient.invalidateQueries({ queryKey: textbookKeys.lists() });
      toast.success("Textbook created successfully");
    },
    onError: (error) => {
      toast.error("Failed to create textbook");
      console.error(error);
    },
  });
}
```

### Local UI State

Use React's built-in hooks for UI-only state:

- `useState` - Component state
- `useReducer` - Complex state logic
- `useContext` - Shared state (auth, theme)

**Do NOT use React Query for:**

- Form input values (use React Hook Form)
- UI toggles (modals, dropdowns)
- Temporary UI state

---

## Component Guidelines

### 1. TypeScript Interfaces

```typescript
// ✅ CORRECT: Props interface
interface TextbookCardProps {
  textbook: Textbook;
  onEdit?: (id: string) => void;
  onDelete?: (id: string) => void;
  className?: string;
}

// ✅ CORRECT: Component with typed props
export function TextbookCard({
  textbook,
  onEdit,
  onDelete,
  className,
}: TextbookCardProps) {
  // ...
}
```

### 2. Presentational vs Container Components

**Presentational (Dumb) Components:**

```typescript
// ✅ Pure UI component, no business logic
interface ButtonProps {
  label: string;
  variant: "primary" | "secondary";
  onClick: () => void;
}

export function Button({ label, variant, onClick }: ButtonProps) {
  return (
    <button className={cn("btn", `btn-${variant}`)} onClick={onClick}>
      {label}
    </button>
  );
}
```

**Container (Smart) Components:**

```typescript
// ✅ Handles data fetching and business logic
export function TextbookListContainer() {
  const { data: textbooks, isLoading } = useTextbooks({ grade: 5 });
  const { mutate: deleteTextbook } = useDeleteTextbook();

  if (isLoading) return <LoadingSpinner />;

  return <TextbookList textbooks={textbooks ?? []} onDelete={deleteTextbook} />;
}
```

### 3. Shadcn Component Usage

```typescript
// ✅ CORRECT: Import from ui folder
import { Button } from "@/components/ui/button";
import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";

// ✅ CORRECT: Compose Shadcn components
export function ExamCard({ exam }: ExamCardProps) {
  return (
    <Card>
      <CardHeader>
        <CardTitle>{exam.title}</CardTitle>
      </CardHeader>
      <CardContent>
        <Button onClick={handleGenerate}>Generate PDF</Button>
      </CardContent>
    </Card>
  );
}
```

---

## Development Workflow

### Phase 1: Setup & Configuration ✅

- [x] Initialize Vite project with React 19
- [x] Configure TypeScript strict mode
- [x] Setup Tailwind CSS
- [ ] Configure Shadcn UI (via MCP)
- [ ] Setup TanStack Query
- [x] Configure Axios interceptors
- [x] Setup i18next
- [ ] Configure ESLint + Prettier

### Phase 2: Core Infrastructure

See individual feature specs for detailed tasks.

### Phase 3: Feature Development

Each feature has its own spec file:

- [Authentication & User Management](./01-auth-feature.md)
- [Teacher Dashboard](./02-dashboard-feature.md)
- [Content Library](./03-content-feature.md)
- [Smart Exam Generator](./04-assessment-feature.md)
- [AI Student Tutor](./05-ai-tutor-feature.md)

---

## Technical Standards

### Code Quality Checklist

Before committing any code, verify:

- [ ] TypeScript compiles with zero errors (strict mode)
- [ ] ESLint shows no warnings or errors
- [ ] Components are properly typed (no `any`)
- [ ] Shadcn components installed via MCP (not manually copied)
- [ ] All API calls use TanStack Query
- [ ] Loading, error, and empty states are handled
- [ ] Responsive design works (mobile, tablet, desktop)
- [ ] Accessibility: keyboard navigation + ARIA labels
- [ ] i18next keys exist for all user-facing text
- [ ] No `console.log` statements in production code

### Performance Checklist

- [ ] Images are optimized and lazy-loaded
- [ ] Routes use React.lazy() for code splitting
- [ ] TanStack Query caching is configured appropriately
- [ ] Expensive computations are memoized (useMemo)
- [ ] Event handlers are memoized (useCallback)
- [ ] Bundle size is monitored (< 500KB initial)

---

## Next Steps

1. **Read** individual feature specs for detailed implementation tasks
2. **Setup** development environment following Phase 1 checklist
3. **Wait** for "LFG" command before starting implementation
4. **Update** checkboxes as tasks are completed
5. **Validate** against acceptance criteria before marking complete

---

**Remember:** This is a living document. Update specifications before implementing changes.
