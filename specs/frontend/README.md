# AI-DLC Master Specification Index

**Project:** Edu-AI Classroom  
**Version:** 2.0  
**Last Updated:** January 13, 2026  
**Architecture:** Direct HTTP/gRPC (No Event-Driven/RabbitMQ)

---

## 📋 Quick Navigation

### Core Specifications

- **[prompt-spec.md](../prompt-spec.md)** - Master AI-DLC workflow, technical standards, coding rules
- **[user-stories.md](../user-stories.md)** - Business requirements and user stories

### Backend Specifications

- **[backend/00-architecture-overview.md](../backend/00-architecture-overview.md)** - System architecture, service boundaries
- **[backend/backend-spec.md](../backend/backend-spec.md)** - Complete backend technical specification
- **[backend/01-content-service.md](../backend/01-content-service.md)** - Content Service detailed spec
- **[backend/02-user-service.md](./backend/02-user-service.md)** - User Service spec _(To Be Created)_
- **[backend/03-assessment-service.md](./backend/03-assessment-service.md)** - Assessment Service spec _(To Be Created)_
- **[backend/04-ai-service.md](./backend/04-ai-service.md)** - AI Orchestrator Service spec _(To Be Created)_

### Frontend Specifications

- **[frontend/00-frontend-overview.md](./00-frontend-overview.md)** - Architecture, tech stack, project structure
- **[frontend/01-auth-feature.md](./01-auth-feature.md)** - Authentication & user management
- **[frontend/02-dashboard-feature.md](./02-dashboard-feature.md)** - Dashboard layout & navigation
- **[frontend/03-content-feature.md](./03-content-feature.md)** - Content library browsing
- **[frontend/04-assessment-feature.md](./04-assessment-feature.md)** - Exam generator _(To Be Created)_
- **[frontend/05-ai-tutor-feature.md](./05-ai-tutor-feature.md)** - AI tutor chat interface _(To Be Created)_

---

## 🎯 AI-DLC Workflow

### Phase 1: Specification (Current Phase) ✅

**Status:** Specifications are being refined and validated.

**Checklist:**

- [x] Define system architecture
- [x] Create master prompt spec
- [x] Document technical standards
- [x] Create frontend overview and feature specs
- [x] Break down into granular tasks with checkboxes
- [ ] Complete all backend service specs
- [ ] Complete all frontend feature specs
- [ ] Validate all specs are implementation-ready

### Phase 2: Implementation (Blocked ⏸️)

**Trigger:** AI Agent receives "LFG" command

**Rules:**

1. **MUST read relevant spec file** before starting any task
2. **MUST follow coding standards** defined in specs
3. **MUST update checkboxes** `[x]` immediately after completing tasks
4. **MUST write tests** for business logic
5. **MUST validate** against acceptance criteria
6. **MUST commit** with descriptive messages

### Phase 3: Validation

**For each completed task:**

- [ ] Run automated tests
- [ ] Verify acceptance criteria
- [ ] Code review (linters, analyzers)
- [ ] Update documentation if needed

---

## 🏗️ Architecture Overview

### System Architecture

```
Frontend (React + Vite)
    ↓ HTTPS
API Gateway
    ↓
┌─────────────┬─────────────┬─────────────┬─────────────┐
│   Content   │    User     │ Assessment  │     AI      │
│   Service   │   Service   │   Service   │  Service    │
└──────┬──────┴──────┬──────┴──────┬──────┴──────┬──────┘
       │             │              │             │
       └─────────────┴──────────────┴─────────────┘
                            │
       ┌────────────────────┴────────────────────┐
       │                                         │
   SQL Server                                AWS S3
   (4 Databases)                         (Asset Storage)
```

**Key Decisions:**

- ✅ Direct HTTP/gRPC communication (no message broker)
- ✅ Database-per-service
- ✅ AWS Cognito for authentication
- ✅ S3 for file storage with presigned URLs
- ✅ OpenTelemetry for observability

---

## 💻 Technical Stack

### Backend

- **Framework:** .NET 9, ASP.NET Core
- **Architecture:** Clean Architecture + DDD
- **Database:** SQL Server (Entity Framework Core)
- **Storage:** AWS S3 SDK
- **Authentication:** AWS Cognito
- **Communication:** HTTP/REST + gRPC
- **Observability:** OpenTelemetry, Serilog

### Frontend

- **Framework:** React 19
- **Build Tool:** Vite 6.x
- **Language:** TypeScript 5.x (Strict Mode)
- **Routing:** React Router v7 (Next.js-style)
- **UI Library:** Shadcn UI (Radix Primitives)
- **State:** TanStack Query v5
- **Styling:** Tailwind CSS 4.x
- **I18n:** i18next

---

## 📝 Coding Standards Summary

### Backend (.NET)

**MUST Follow:**

- [ ] Clean Architecture (Domain → Application → Infrastructure → API)
- [ ] Nullable reference types enabled
- [ ] Async/await for all I/O operations
- [ ] CancellationToken in all async methods
- [ ] CQRS pattern (Commands vs Queries)
- [ ] FluentValidation for input validation
- [ ] DTOs for API responses (never expose entities)
- [ ] ILogger<T> for structured logging
- [ ] Result<T> pattern (avoid throwing for business errors)

**MUST NOT:**

- ❌ Use `Task.Result` or `.Wait()` (deadlocks)
- ❌ Expose EF Core entities in API
- ❌ Hardcode connection strings
- ❌ Use static mutable state

### Frontend (React + TypeScript)

**MUST Follow:**

- [ ] TypeScript strict mode enabled
- [ ] Functional components only (hooks)
- [ ] Separation of concerns (container vs presentational)
- [ ] TanStack Query for all API calls
- [ ] Shadcn UI installed via MCP (not manual)
- [ ] Tailwind for 95% of styling
- [ ] i18next for all user-facing text
- [ ] Props destructuring in function signature
- [ ] Custom hooks for reusable logic

**MUST NOT:**

- ❌ Use `any` types
- ❌ Use class components
- ❌ Hardcode strings (use i18next)
- ❌ Inline styles (use Tailwind)

---

## 🎓 Feature Breakdown

### 1. Authentication & User Management

**Status:** Spec Complete ✅  
**Complexity:** Medium  
**Dependencies:** None

**Key Components:**

- Login/Register pages
- JWT token management
- User profile CRUD
- Protected routes
- Avatar upload to S3

**Implementation Estimate:** 5-7 days

---

### 2. Content Library

**Status:** Spec Complete ✅  
**Complexity:** Medium  
**Dependencies:** Auth

**Key Components:**

- Textbook browsing (grid view)
- Filtering by grade/subject
- Chapter navigation (accordion)
- Page preview (S3 presigned URLs)
- Search functionality

**Implementation Estimate:** 4-6 days

---

### 3. Teacher Dashboard

**Status:** Spec Complete ✅  
**Complexity:** Low  
**Dependencies:** Auth

**Key Components:**

- Dashboard layout (sidebar + header)
- Overview statistics
- Quick actions
- Recent activity feed
- Navigation system

**Implementation Estimate:** 3-4 days

---

### 4. Smart Exam Generator

**Status:** Spec Pending ⏳  
**Complexity:** High  
**Dependencies:** Auth, Content

**Key Components:**

- Exam matrix builder
- Question bank browser
- Question selection/replacement
- Exam preview
- PDF export

**Implementation Estimate:** 8-10 days

---

### 5. AI Student Tutor

**Status:** Spec Pending ⏳  
**Complexity:** High  
**Dependencies:** Auth, Content

**Key Components:**

- Chat interface with streaming
- Socratic dialogue engine
- Reference panel
- Conversation history
- Markdown/MathJax rendering

**Implementation Estimate:** 8-10 days

---

## ✅ Validation Checklist Template

Use this checklist for every completed feature:

### Functionality

- [ ] All user stories implemented
- [ ] Acceptance criteria met
- [ ] Happy path works end-to-end
- [ ] Edge cases handled

### Code Quality

- [ ] TypeScript/C# compiles with zero errors
- [ ] Linters pass (ESLint/Roslyn Analyzers)
- [ ] All functions/components typed
- [ ] No `any` or `dynamic` types
- [ ] Follows architecture patterns
- [ ] Separation of concerns maintained

### Testing

- [ ] Unit tests written (80%+ coverage)
- [ ] Integration tests written
- [ ] E2E tests for critical paths
- [ ] All tests passing

### UX/UI

- [ ] Loading states on async operations
- [ ] Error states with clear messages
- [ ] Empty states handled
- [ ] Responsive (mobile, tablet, desktop)
- [ ] Keyboard navigation works
- [ ] ARIA labels present

### Performance

- [ ] API responses < 500ms (p95)
- [ ] Frontend loads < 2 seconds
- [ ] No N+1 queries (backend)
- [ ] Images lazy loaded (frontend)
- [ ] Bundle size optimized

### Security

- [ ] Authentication required where needed
- [ ] Authorization checks in place
- [ ] Input validation on all endpoints
- [ ] SQL injection prevented (parameterized queries)
- [ ] XSS prevented (React escaping)
- [ ] Secrets not committed to git

### Documentation

- [ ] API endpoints documented (Swagger)
- [ ] Component props documented (JSDoc)
- [ ] README updated
- [ ] Spec updated if deviations occurred

---

## 🚀 Getting Started for AI Agents

### Step 1: Understand the System

1. Read [prompt-spec.md](../prompt-spec.md) for AI-DLC workflow
2. Read [Architecture Overview](../backend/00-architecture-overview.md)
3. Study the architecture diagram: `docs/architecture.png`

### Step 2: Pick a Feature

1. Start with dependencies resolved (Auth → Dashboard → Content)
2. Read the feature spec completely
3. Understand acceptance criteria

### Step 3: Wait for "LFG"

- Do NOT start coding until "LFG" command is given
- Use this time to ask clarifying questions

### Step 4: Implement

1. Follow implementation tasks in order
2. Update checkboxes as you complete tasks
3. Validate against acceptance criteria
4. Run tests before marking complete

### Step 5: Review & Iterate

1. Self-review code
2. Fix linter errors
3. Run full test suite
4. Update documentation

---

## 📊 Progress Tracking

### Overall Progress

**Specification Phase:** 70% Complete

- [x] Master prompt spec
- [x] Frontend overview
- [x] Auth feature spec
- [x] Dashboard feature spec
- [x] Content feature spec
- [ ] Assessment feature spec (Pending)
- [ ] AI Tutor feature spec (Pending)
- [ ] User Service spec (Pending)
- [ ] Assessment Service spec (Pending)
- [ ] AI Service spec (Pending)

**Implementation Phase:** 0% Complete (Blocked until "LFG")

---

## 🔗 Related Documents

- **Architecture Diagram:** `docs/architecture.png`
- **Dynamic Router Config:** `frontend/src/config/dynamicRouter.tsx`
- **Components Config:** `frontend/components.json`
- **Solution File:** `FrogEdu.sln`

---

## 📞 Support & Questions

If you encounter:

- **Missing requirements:** Update the spec first, then implement
- **Architectural conflicts:** Discuss before deviating from spec
- **Ambiguities:** Ask for clarification

**Remember:** Specs are living documents. Keep them synchronized with implementation.

---

**Version History:**

- v2.0 (2026-01-13): Reorganized into modular specs, added detailed checklists
- v1.0 (2026-01-10): Initial specification
