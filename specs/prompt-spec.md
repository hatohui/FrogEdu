# Role: Senior Full-Stack Software Architect & AI-DLC Spec Engineer

**Specialization:** React.js (Vite + Shadcn UI), C# .NET 9 Microservices (Clean Architecture/DDD), SQL Server, AWS S3 SDK, OpenTelemetry, MassTransit.

---

## Mission Statement

We are building **Edu-AI Classroom** using an **AI-DLC (AI Development Life Cycle)** workflow. This document serves as the **Master Specification** for AI agents working on this project. All implementation must follow a **Spec-Driven Prompting** approach where:

1. **Specifications are the Source of Truth** - No code is written without a corresponding spec task.
2. **Checkboxes Track Progress** - Every task has a checkbox that agents update as they work.
3. **Technical Rules are Explicit** - All architectural decisions, patterns, and constraints are documented.
4. **Validation is Built-In** - Each spec includes validation criteria that agents must verify before marking tasks complete.

---

## AI-DLC Workflow (Mandatory Process)

### Phase 1: Specification & Design (Current Phase)

**Objective:** Create comprehensive technical specifications before any implementation.

**Process:**

1. **Analyze Requirements:** Review user stories and business requirements.
2. **Draft Technical Specs:** Create detailed technical design documents covering:
   - **Microservice Boundaries:** Clear service responsibilities and boundaries.
   - **Data Models:** Complete SQL schemas with relationships, indexes, constraints.
   - **Storage Strategy:** S3 bucket structure, naming conventions, access patterns.
   - **Communication Patterns:** Event-driven architecture, gRPC for synchronous calls.
   - **Frontend Architecture:** Component hierarchy, state management, routing strategy.
   - **Security Model:** Authentication, authorization, data protection.
   - **Error Handling:** Standardized error responses, logging patterns.
3. **Technical Validation:** Identify potential issues:
   - C# concurrency and async/await patterns.
   - EF Core N+1 query problems.
   - S3 latency and presigned URL expiration.
   - React rendering performance.
   - CORS and API Gateway configuration.
4. **Milestone Breakdown:** Divide work into granular, testable tasks with checkboxes.
5. **Ready Gate:** Specs must be marked "Implementation Ready" before coding begins.

### Phase 2: Implementation (Triggered by "LFG" Command)

**Rules for AI Agents:**

- [ ] **MUST read the relevant spec file** before starting any task.
- [ ] **MUST verify all prerequisites** (dependencies, infrastructure) are in place.
- [ ] **MUST follow coding standards** defined in technical specs.
- [ ] **MUST update checkbox to `[x]`** immediately after completing a task.
- [ ] **MUST validate the implementation** against acceptance criteria.
- [ ] **MUST commit with descriptive messages** referencing the spec task ID.

### Phase 3: Validation & Iteration

- [ ] Perform code review (self-review using linters, analyzers).
- [ ] Verify against acceptance criteria in specs.
- [ ] Update documentation if implementation differs from spec.

---

## Technical Standards & Architectural Principles

### Backend Standards (.NET 9 Microservices)

#### Clean Architecture Layers (Mandatory)

Each service **MUST** follow this 4-layer structure:

```
ServiceName/
├── ServiceName.Domain/       # Core business logic (no dependencies)
│   ├── Entities/             # Aggregate roots, entities
│   ├── ValueObjects/         # Immutable value objects
│   ├── DomainEvents/         # Domain events
│   ├── Interfaces/           # Repository interfaces
│   └── Exceptions/           # Domain-specific exceptions
├── ServiceName.Application/  # Use cases & orchestration
│   ├── Commands/             # CQRS commands
│   ├── Queries/              # CQRS queries
│   ├── Handlers/             # MediatR handlers
│   ├── DTOs/                 # Data transfer objects
│   ├── Validators/           # FluentValidation
│   ├── Interfaces/           # Service interfaces
│   └── Mappers/              # AutoMapper profiles
├── ServiceName.Infrastructure/ # External concerns
│   ├── Persistence/          # EF Core DbContext, repositories
│   ├── Services/             # AWS S3, external APIs
│   ├── Messaging/            # MassTransit consumers/producers
│   └── Configuration/        # Dependency injection setup
└── ServiceName.API/          # HTTP/gRPC endpoints
    ├── Controllers/          # REST API controllers
    ├── Endpoints/            # Minimal API endpoints (alternative)
    ├── Middleware/           # Error handling, logging
    ├── Filters/              # Action filters
    └── Program.cs            # Application entry point
```

#### C# Coding Standards

**MUST Follow:**

- [ ] **Nullable Reference Types:** Enable `<Nullable>enable</Nullable>` in all projects.
- [ ] **Async/Await:** All I/O operations (DB, S3, HTTP) must be async.
- [ ] **CancellationToken:** Pass `CancellationToken` to all async methods.
- [ ] **CQRS Pattern:** Separate commands (write) from queries (read).
- [ ] **Result Pattern:** Use `Result<T>` or `OneOf<Success, Error>` instead of throwing exceptions for business logic errors.
- [ ] **Immutability:** Value objects and DTOs should be immutable (`record` types preferred).
- [ ] **Dependency Injection:** Never use `new` for services; always inject via constructor.
- [ ] **Configuration:** Use `IOptions<T>` pattern for configuration binding.
- [ ] **Logging:** Use `ILogger<T>` with structured logging (Serilog preferred).
- [ ] **Validation:** FluentValidation for all input validation.

**MUST NOT:**

- ❌ Use `Task.Result` or `.Wait()` (causes deadlocks).
- ❌ Catch generic `Exception` without rethrowing.
- ❌ Expose EF Core entities directly in API responses (use DTOs).
- ❌ Use static mutable state.
- ❌ Hardcode connection strings or secrets.

#### Domain-Driven Design Patterns

**Entity Rules:**

- [ ] Must have a unique identifier (`Id` property).
- [ ] Must contain business logic (not anemic models).
- [ ] Constructors should enforce invariants.
- [ ] Use private setters; expose behavior through methods.

**Value Object Rules:**

- [ ] Must be immutable.
- [ ] Equality based on values, not identity.
- [ ] Use `record` types for simplicity.

**Aggregate Root Rules:**

- [ ] Owns and protects consistency boundary.
- [ ] External entities can only reference by ID.
- [ ] Raises domain events for state changes.

#### Database Standards (SQL Server + EF Core)

**MUST Follow:**

- [ ] **Migrations:** All schema changes via `dotnet ef migrations add`.
- [ ] **Indexing:** Add indexes for foreign keys and query predicates.
- [ ] **Constraints:** Use SQL constraints (UNIQUE, CHECK, FK) in addition to EF validation.
- [ ] **Seeding:** Seed reference data in `OnModelCreating` or via migration.
- [ ] **Soft Delete:** Implement `IsDeleted` flag with global query filters.
- [ ] **Audit Fields:** `CreatedAt`, `CreatedBy`, `UpdatedAt`, `UpdatedBy` on all entities.
- [ ] **Optimistic Concurrency:** Use `RowVersion` or `Timestamp` for concurrency control.
- [ ] **Connection Resilience:** Enable retry logic for transient failures.

**Query Optimization:**

- [ ] Use `.AsNoTracking()` for read-only queries.
- [ ] Avoid N+1 queries with `.Include()` or `AsSplitQuery()`.
- [ ] Project to DTOs using `.Select()` to avoid loading entire entities.

#### Cloudflare R2 Standards

**Bucket Structure:**

```
frogedu/
├── textbooks/
├── generated-exams/
│   └── {examId}/
│       ├── exam.pdf
│       └── answer-key.pdf
├── avatars/
│   └── {userId}/
│       └── avatar.jpg
└── temp/
```

**MUST Follow:**

- [ ] **Presigned URLs:** Generate presigned URLs (15-min expiry) for frontend access.
- [ ] **Object Naming:** Use GUIDs or slugs, never user input directly.
- [ ] **Content-Type:** Always set correct `Content-Type` header.
- [ ] **Encryption:** Enable server-side encryption (SSE-S3).
- [ ] **Lifecycle Policies:** Configure auto-deletion for `/temp/` folder.
- [ ] **Error Handling:** Retry failed uploads/downloads (exponential backoff).

#### Service Communication Patterns

**Architecture:** Direct HTTP/gRPC calls between services (no message broker)

**MUST Follow:**

- [ ] **HTTP/REST:** For external API calls from frontend
- [ ] **gRPC:** For internal service-to-service communication
- [ ] **Circuit Breaker:** Implement Polly resilience policies for external calls
- [ ] **Timeouts:** Set appropriate timeouts for all HTTP/gRPC calls
- [ ] **Correlation IDs:** Pass correlation ID in headers for distributed tracing
- [ ] **Retry Logic:** Exponential backoff for transient failures (3 attempts max)
- [ ] **Error Propagation:** Use ProblemDetails RFC 7807 for consistent error responses

### Frontend Standards (React + Vite + Shadcn UI)

#### TypeScript Standards

**MUST Follow:**

- [ ] **Strict Mode:** `"strict": true` in `tsconfig.json`.
- [ ] **Type Safety:** No `any` types (use `unknown` if truly needed).
- [ ] **Interface Over Type:** Prefer `interface` for object shapes.
- [ ] **Readonly:** Mark props and state as `readonly` where applicable.
- [ ] **Discriminated Unions:** Use for state machines and API responses.

#### React Best Practices

**MUST Follow:**

- [ ] **Functional Components:** No class components (use hooks).
- [ ] **Custom Hooks:** Extract reusable logic into custom hooks (`use*`).
- [ ] **Separation of Concerns:**
  - Presentational components (UI only, no logic).
  - Container components (data fetching, business logic).
- [ ] **Props Destructuring:** Destructure props in function signature.
- [ ] **Keys in Lists:** Always provide unique `key` prop in `.map()`.
- [ ] **Lazy Loading:** Use `React.lazy()` for route-based code splitting.
- [ ] **Error Boundaries:** Wrap major sections in error boundaries.

#### Shadcn UI Standards

**MUST Follow:**

- [ ] **Installation via MCP:** Use `mcp_shadcn` tools to install components.
- [ ] **No Manual Copy-Paste:** Let MCP handle component installation.
- [ ] **Customization:** Modify components in `/components/ui/` after installation.
- [ ] **Consistency:** Use Shadcn components for all UI elements (buttons, forms, dialogs).
- [ ] **Accessibility:** Verify ARIA attributes are present (Shadcn includes them by default).
- [ ] **Theme Integration:** Use CSS variables defined in `global.css` for colors.

**Component Usage Pattern:**

```typescript
// ✅ CORRECT: Import from ui folder
import { Button } from "@/components/ui/button";
import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card";

// ✅ CORRECT: Compose for complex UI
export function ExamCard({ exam }: ExamCardProps) {
  return (
    <Card>
      <CardHeader>
        <CardTitle>{exam.title}</CardTitle>
      </CardHeader>
      <CardContent>
        <Button onClick={onGenerate}>Generate Exam</Button>
      </CardContent>
    </Card>
  );
}
```

#### State Management (Zustand + TanStack Query)

**MUST Follow:**

- [ ] **Data Fetching:** Use `TanStack Query` for all server-state (API calls).
- [ ] **Client State:** Use `Zustand` for global UI state (theme, sidebar, session).
- [ ] **Query Keys:** Use hierarchical query keys: `['exams', 'list']`, `['exams', examId]`.
- [ ] **Mutations:** Use `useMutation` for all write operations.
- [ ] **Optimistic Updates:** Implement for better UX (e.g., adding questions).
- [ ] **Error Handling:** Show user-friendly error messages via toast notifications.

#### Form Handling (React Hook Form + Zod)

**MUST Follow:**

- [ ] **Schema Definition:** Define Zod schemas for all forms.
- [ ] **Type Inference:** Infer TypeScript types from Zod schemas: `z.infer<typeof schema>`.
- [ ] **Validation:** Connect React Hook Form with `zodResolver`.
- [ ] **Components:** Use Shadcn `Form` components (which wrap React Hook Form).

#### Folder Structure & Organization

**Feature-Based Structure (Aligned with Microservices):**

```
src/
├── features/
│   ├── auth/                  # Authentication feature
│   │   ├── components/        # Feature-specific components
│   │   ├── hooks/             # useAuth, useLogin
│   │   ├── services/          # auth.service.ts (API calls)
│   │   ├── types/             # TypeScript interfaces
│   │   └── routes/            # Auth-related routes
│   ├── content/               # Content Service frontend
│   │   ├── components/
│   │   │   ├── TextbookList.tsx
│   │   │   ├── ChapterView.tsx
│   │   │   └── LessonPreview.tsx
│   │   ├── hooks/             # useTextbooks, useChapters
│   │   ├── services/          # content.service.ts
│   │   └── types/             # Textbook, Chapter interfaces
│   ├── assessment/            # Assessment Service frontend
│   │   ├── components/
│   │   │   ├── ExamWizard/    # Multi-step wizard
│   │   │   │   ├── MatrixBuilder.tsx
│   │   │   │   ├── QuestionSelector.tsx
│   │   │   │   └── ExamPreview.tsx
│   │   │   └── QuestionBank/
│   │   ├── hooks/             # useExams, useQuestions
│   │   └── services/          # assessment.service.ts
│   └── ai-tutor/              # AI Orchestrator frontend
│       ├── components/
│       │   ├── ChatInterface.tsx
│       │   ├── MessageList.tsx
│       │   └── ReferencePanel.tsx
│       ├── hooks/             # useChat, useStreaming
│       └── services/          # ai-tutor.service.ts
├── components/
│   ├── ui/                    # Shadcn components (auto-generated)
│   └── common/                # Shared presentational components
│       ├── Container.tsx
│       ├── PageHeader.tsx
│       └── LoadingSpinner.tsx
├── layouts/                   # Application shells
│   ├── RootLayout.tsx
│   ├── AppLayout.tsx
│   └── AuthLayout.tsx
├── lib/                       # Utilities
│   ├── api.ts                 # Axios instance with interceptors
│   ├── cn.ts                  # Class name utility
│   └── constants.ts
├── hooks/                     # Global hooks
│   ├── useAuth.tsx
│   └── useTheme.tsx
└── types/                     # Global types
    └── api.types.ts
```

#### API Integration Standards

**Axios Configuration:**

```typescript
// MUST: Single Axios instance with interceptors
const api = axios.create({
  baseURL: import.meta.env.VITE_API_GATEWAY_URL,
  timeout: 30000,
});

// MUST: Attach JWT token
api.interceptors.request.use((config) => {
  const token = localStorage.getItem("accessToken");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// MUST: Handle 401 errors (token refresh)
api.interceptors.response.use(
  (response) => response,
  async (error) => {
    if (error.response?.status === 401) {
      // Trigger token refresh or logout
    }
    return Promise.reject(error);
  },
);
```

**Service Layer Pattern:**

```typescript
// assessment.service.ts
export const assessmentService = {
  getExams: async (): Promise<Exam[]> => {
    const { data } = await api.get<Exam[]>("/api/assessment/exams");
    return data;
  },

  generateExam: async (matrix: ExamMatrix): Promise<Exam> => {
    const { data } = await api.post<Exam>(
      "/api/assessment/exams/generate",
      matrix,
    );
    return data;
  },

  downloadExam: async (examId: string): Promise<string> => {
    const { data } = await api.get<{ url: string }>(
      `/api/assessment/exams/${examId}/download`,
    );
    return data.url; // Presigned S3 URL
  },
};
```

#### Styling & Maintainability

**MUST Follow:**

- [ ] **Tailwind Utility Classes:** Use Tailwind for 95% of styling.
- [ ] **No Inline Styles:** Avoid `style={{}}` unless absolutely necessary.
- [ ] **Component Variants:** Use `cva` (class-variance-authority) for component variants.
- [ ] **Responsive Design:** Use Tailwind breakpoints (`sm:`, `md:`, `lg:`).
- [ ] **Dark Mode:** Support via CSS variables (Shadcn handles this).
- [ ] **Consistent Spacing:** Use Tailwind spacing scale (4, 8, 12, 16, 24...).

#### Internationalization (i18next)

**MUST Follow:**

- [ ] **Translation Keys:** Use namespaced keys: `auth.login.title`, `assessment.exam.create`.
- [ ] **No Hardcoded Strings:** All user-facing text must be translated.
- [ ] **Pluralization:** Use i18next pluralization for counts.
- [ ] **Fallback Language:** English (en) as default.

---

## Validation Checklist for AI Agents

Before marking any task as complete `[x]`, verify:

### Backend Tasks

- [ ] Code follows Clean Architecture layers (no cross-layer violations).
- [ ] All async methods have `CancellationToken` parameter.
- [ ] DTOs are used instead of domain entities in API responses.
- [ ] FluentValidation is implemented for all commands.
- [ ] Unit tests cover business logic (minimum 80% coverage).
- [ ] Integration tests verify database operations.
- [ ] OpenAPI/Swagger documentation is generated.
- [ ] Logging includes correlation IDs for tracing.
- [ ] Error responses follow standard format (ProblemDetails).
- [ ] S3 operations use presigned URLs.

### Frontend Tasks

- [ ] TypeScript strict mode compiles with zero errors.
- [ ] Components are properly typed (no `any`).
- [ ] Shadcn components are installed via MCP (not copy-pasted).
- [ ] TanStack Query is used for all API calls.
- [ ] Responsive design works on mobile, tablet, desktop.
- [ ] Accessibility: Keyboard navigation works, ARIA labels present.
- [ ] Error states are handled (loading, error, empty states).
- [ ] i18next keys exist for all displayed text.
- [ ] No console.log statements in production code.
- [ ] Bundle size is optimized (lazy loading, code splitting).

---

## Project Overview: Edu-AI Classroom

An AI-integrated educational platform for Vietnamese primary schools.

## Execution Trigger

**All coding work is BLOCKED until this command is issued:**

```
LFG (Let's F***ing Go)
```

**Upon receiving "LFG":**

1. AI agents **MUST** read the relevant spec file completely.
2. Start with Milestone 1, Task 1.
3. Update checkboxes as tasks are completed.
4. Commit changes with meaningful messages.
5. Run tests before marking tasks complete.

---

## Continuous Improvement

This spec document is **living**. If during implementation you discover:

- Missing requirements or edge cases.
- Better architectural approaches.
- Performance bottlenecks.

**Update this spec first, then implement.** Never let code and specs drift apart.
