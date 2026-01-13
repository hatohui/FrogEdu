# Frontend Technical Specification: Edu-AI Classroom

**⚠️ NOTICE:** This file has been reorganized into modular feature specs.

**Please refer to:**

## Overview & Architecture

- **[00-frontend-overview.md](./00-frontend-overview.md)** - Technology stack, architecture principles, project structure, routing, state management

## Feature Specifications

Each feature has detailed implementation specs with:

- ✅ User stories with acceptance criteria
- ✅ UI/UX mockups and requirements
- ✅ Component architecture and code examples
- ✅ API integration details
- ✅ State management (TanStack Query hooks)
- ✅ Granular implementation tasks with checkboxes
- ✅ Technical validation criteria

### Core Features

1. **[01-auth-feature.md](./01-auth-feature.md)** - Authentication & User Management
   - Login, registration, password reset
   - JWT token management
   - User profile management
   - Protected routes
2. **[02-dashboard-feature.md](./02-dashboard-feature.md)** - Teacher Dashboard & Layout
   - Dashboard layout system (sidebar, header)
   - Navigation components
   - Overview statistics
   - Quick actions
3. **[03-content-feature.md](./03-content-feature.md)** - Content Library
   - Textbook browsing and filtering
   - Chapter navigation
   - Page preview with S3 integration
4. **[04-assessment-feature.md](./04-assessment-feature.md)** - Smart Exam Generator
   - Exam matrix builder
   - Question bank integration
   - Exam preview and PDF export
   - Question selection and replacement
5. **[05-ai-tutor-feature.md](./05-ai-tutor-feature.md)** - AI Student Tutor
   - Chat interface with streaming
   - Socratic dialogue
   - Reference panel with textbook content
   - Conversation history

---

## Development Workflow

### Phase 1: Setup (Complete ✅)

- [x] Vite + React 19 + TypeScript
- [x] Tailwind CSS configured
- [x] React Router with dynamic routing
- [x] i18next for internationalization

### Phase 2: Infrastructure (In Progress 🔄)

- [ ] Shadcn UI components installation
- [ ] TanStack Query setup
- [ ] Axios interceptors
- [ ] Auth context
- [ ] Layout system

### Phase 3: Feature Development (Blocked ⏸️)

**⚠️ DO NOT START UNTIL "LFG" COMMAND IS GIVEN**

Work through features in this order:

1. Authentication (01-auth-feature.md)
2. Dashboard Layout (02-dashboard-feature.md)
3. Content Library (03-content-feature.md)
4. Exam Generator (04-assessment-feature.md)
5. AI Tutor (05-ai-tutor-feature.md)

---

## Technical Standards

### Mandatory Rules

**TypeScript:**

- [ ] Strict mode enabled
- [ ] No `any` types
- [ ] All props and state typed

**React:**

- [ ] Functional components only
- [ ] Custom hooks for reusable logic
- [ ] Separation of concerns (container vs presentational)

**Shadcn UI:**

- [ ] Install via MCP tools (not manual copy-paste)
- [ ] Use for all UI elements
- [ ] Customize in `/components/ui/` after installation

**TanStack Query:**

- [ ] All API calls use TanStack Query
- [ ] Hierarchical query keys
- [ ] Proper stale time configuration
- [ ] Optimistic updates where appropriate

**Styling:**

- [ ] Tailwind utility classes (95% of styling)
- [ ] No inline styles
- [ ] Responsive breakpoints (sm:, md:, lg:)
- [ ] Dark mode support via CSS variables

**Internationalization:**

- [ ] All user-facing text uses i18next
- [ ] No hardcoded strings
- [ ] Namespaced translation keys

### Code Quality Checklist

Before marking any task complete:

- [ ] TypeScript compiles with zero errors
- [ ] ESLint shows no warnings
- [ ] Components are properly typed
- [ ] Loading, error, empty states handled
- [ ] Responsive on mobile, tablet, desktop
- [ ] Keyboard navigation works
- [ ] ARIA labels present
- [ ] No `console.log` statements

---

## API Integration

All API calls go through a single gateway:

```typescript
// services/axios.ts
const api = axios.create({
  baseURL: import.meta.env.VITE_API_GATEWAY_URL,
  timeout: 30000,
});

// Request interceptor: attach JWT
api.interceptors.request.use((config) => {
  const token = localStorage.getItem("accessToken");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// Response interceptor: handle 401 (token refresh)
api.interceptors.response.use(
  (response) => response,
  async (error) => {
    if (error.response?.status === 401) {
      // Trigger token refresh or logout
    }
    return Promise.reject(error);
  }
);
```

---

## Next Steps for AI Agents

1. **Read** the overview (00-frontend-overview.md)
2. **Study** the specific feature spec you'll work on
3. **Wait** for "LFG" command before implementing
4. **Follow** the implementation tasks in order
5. **Update** checkboxes as you complete tasks
6. **Validate** against acceptance criteria before marking complete

---

**Remember:** This is a living spec. Update it if you discover missing requirements or better approaches during implementation.
