# Frontend Technical Specification: Edu-AI Classroom

**⚠️ NOTICE:** This file has been reorganized into modular feature specs for optimal token usage.

**Version:** 2.0  
**Last Updated:** January 14, 2026  
**Status:** Modular Structure Implemented ✅

---

## Overview & Architecture

- **[00-frontend-overview.md](./00-frontend-overview.md)** - Technology stack, architecture principles, project structure, routing, state management, technical standards

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
   - Avatar upload
   - Protected routes

2. **[02-dashboard-feature.md](./02-dashboard-feature.md)** - Teacher Dashboard & Layout
   - Dashboard layout system (sidebar, header)
   - Navigation components
   - Overview statistics
   - Quick actions
   - Role-based views

3. **[03-content-feature.md](./03-content-feature.md)** - Content Library
   - Textbook browsing and filtering
   - Chapter navigation
   - Page preview with S3 integration
   - Search and filter functionality

4. **[04-assessment-feature.md](./04-assessment-feature.md)** - Smart Exam Generator
   - Exam matrix builder
   - Question bank integration
   - Exam preview and PDF export
   - Question selection and replacement
   - Multi-step wizard

5. **[05-ai-tutor-feature.md](./05-ai-tutor-feature.md)** - AI Student Tutor
   - Chat interface with streaming
   - Socratic dialogue
   - Reference panel with textbook content
   - Conversation history
   - Real-time responses

---

## Development Workflow

### Phase 1: Setup (Complete ✅)

- [x] Vite + React 19 + TypeScript
- [x] Tailwind CSS configured
- [x] React Router with dynamic routing
- [x] i18next for internationalization
- [x] ESLint and TypeScript strict mode

### Phase 2: Infrastructure (Complete ✅)

- [x] Shadcn UI components installation
- [x] TanStack Query setup with proper query keys
- [x] Axios interceptors (JWT, error handling)
- [x] Auth context and protected routes
- [x] Layout system (RootLayout, DashboardLayout)
- [x] Theme provider (light/dark mode)
- [x] Toast notifications (Sonner)
- [x] API service layer with type safety
- [x] Assessment, Content, User, AI Tutor services
- [x] Header and Sidebar components

### Phase 3: Feature Development (Blocked ⏸️)

**⚠️ DO NOT START UNTIL "LFG" COMMAND IS GIVEN**

Work through features in this order:

1. **Authentication** (01-auth-feature.md) - Login, registration, profile
2. **Dashboard Layout** (02-dashboard-feature.md) - Navigation, sidebar, header
3. **Content Library** (03-content-feature.md) - Browse textbooks, chapters
4. **Exam Generator** (04-assessment-feature.md) - Create and manage exams
5. **AI Tutor** (05-ai-tutor-feature.md) - Chat interface, streaming

---

## Technical Standards Summary

### Mandatory Rules

**TypeScript:**

- [ ] Strict mode enabled (`"strict": true`)
- [ ] No `any` types (use `unknown` if needed)
- [ ] All props and state properly typed
- [ ] Interface for object shapes

**React:**

- [ ] Functional components only
- [ ] Custom hooks for reusable logic
- [ ] Separation of concerns (container vs presentational)
- [ ] Props destructuring in function signature
- [ ] Unique `key` prop in `.map()`

**Shadcn UI:**

- [ ] Install via MCP tools (`mcp_shadcn`)
- [ ] Never manual copy-paste
- [ ] Customize in `/components/ui/` after installation
- [ ] Use for all UI elements (buttons, forms, dialogs)

**TanStack Query:**

- [ ] All API calls use TanStack Query
- [ ] Hierarchical query keys: `['exams', 'list']`, `['exams', examId]`
- [ ] Use `useMutation` for write operations
- [ ] Implement optimistic updates
- [ ] Proper stale time configuration

**Styling:**

- [ ] Tailwind utility classes (95% of styling)
- [ ] No inline styles (`style={{}}`)
- [ ] Responsive breakpoints (`sm:`, `md:`, `lg:`)
- [ ] Dark mode support via CSS variables
- [ ] Consistent spacing (Tailwind scale: 4, 8, 12, 16, 24)

**Internationalization:**

- [ ] All user-facing text uses i18next
- [ ] No hardcoded strings
- [ ] Namespaced translation keys: `auth.login.title`
- [ ] Pluralization support

**Folder Structure:**

```
src/
├── features/              # Feature-based organization
│   ├── auth/
│   ├── content/
│   ├── assessment/
│   └── ai-tutor/
├── components/
│   ├── ui/               # Shadcn components
│   └── common/           # Shared components
├── layouts/              # Application shells
├── lib/                  # Utilities (axios, cn)
├── hooks/                # Global hooks
└── types/                # Global types
```

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
- [ ] i18next keys exist for all text

---

## API Integration

All API calls go through a single Axios instance:

```typescript
// lib/api.ts
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

// Response interceptor: handle 401
api.interceptors.response.use(
  (response) => response,
  async (error) => {
    if (error.response?.status === 401) {
      // Trigger logout or token refresh
    }
    return Promise.reject(error);
  },
);
```

**Service Layer Pattern:**

```typescript
// features/assessment/services/assessment.service.ts
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
};
```

**TanStack Query Hook:**

```typescript
// features/assessment/hooks/useExams.ts
export function useExams() {
  return useQuery({
    queryKey: ["exams", "list"],
    queryFn: assessmentService.getExams,
    staleTime: 5 * 60 * 1000, // 5 minutes
  });
}

export function useGenerateExam() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: assessmentService.generateExam,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["exams"] });
    },
  });
}
```

---

## Component Patterns

### Container/Presentational Pattern

```typescript
// Container Component (handles data, logic)
export function ExamListContainer() {
  const { data: exams, isLoading, error } = useExams();
  const navigate = useNavigate();

  if (isLoading) return <LoadingSpinner />;
  if (error) return <ErrorMessage error={error} />;
  if (!exams || exams.length === 0) return <EmptyState />;

  return (
    <ExamList exams={exams} onExamClick={(id) => navigate(`/exams/${id}`)} />
  );
}

// Presentational Component (pure UI)
interface ExamListProps {
  exams: Exam[];
  onExamClick: (id: string) => void;
}

export function ExamList({ exams, onExamClick }: ExamListProps) {
  return (
    <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
      {exams.map((exam) => (
        <ExamCard
          key={exam.id}
          exam={exam}
          onClick={() => onExamClick(exam.id)}
        />
      ))}
    </div>
  );
}
```

### Custom Hook Pattern

```typescript
// features/auth/hooks/useAuth.tsx
export function useAuth() {
  const [user, setUser] = useState<User | null>(null);
  const navigate = useNavigate();

  const login = async (credentials: LoginCredentials) => {
    const { data } = await authService.login(credentials);
    localStorage.setItem("accessToken", data.token);
    setUser(data.user);
    navigate("/dashboard");
  };

  const logout = () => {
    localStorage.removeItem("accessToken");
    setUser(null);
    navigate("/login");
  };

  return { user, login, logout, isAuthenticated: !!user };
}
```

---

## Routing Structure

```typescript
// config/dynamicRouter.tsx
const routes = [
  {
    path: "/",
    element: <RootLayout />,
    children: [
      { index: true, element: <Navigate to="/dashboard" /> },
      { path: "login", element: <LoginPage /> },
      { path: "register", element: <RegisterPage /> },
      {
        path: "dashboard",
        element: (
          <ProtectedRoute>
            <DashboardLayout />
          </ProtectedRoute>
        ),
        children: [
          { index: true, element: <DashboardOverview /> },
          { path: "textbooks", element: <TextbookList /> },
          { path: "textbooks/:id", element: <TextbookDetail /> },
          { path: "exams", element: <ExamList /> },
          { path: "exams/create", element: <ExamWizard /> },
          { path: "exams/:id", element: <ExamDetail /> },
          { path: "tutor", element: <AITutorChat /> },
        ],
      },
    ],
  },
];
```

---

## Next Steps for AI Agents

1. **Read** the overview (00-frontend-overview.md) thoroughly
2. **Install** required Shadcn UI components using MCP tools
3. **Setup** TanStack Query and Axios configuration
4. **Study** the specific feature spec you'll work on
5. **Wait** for "LFG" command before implementing features
6. **Follow** the implementation tasks in order
7. **Update** checkboxes as you complete tasks
8. **Validate** against acceptance criteria before marking complete

---

## Completion Criteria

### Feature Completeness

- [ ] All user stories implemented
- [ ] All screens responsive (mobile, tablet, desktop)
- [ ] All API integrations working
- [ ] All error states handled gracefully

### Technical Quality

- [ ] TypeScript strict mode with zero errors
- [ ] No ESLint warnings
- [ ] All components properly typed
- [ ] Accessibility: keyboard navigation + ARIA labels
- [ ] i18next: all strings translated
- [ ] Performance: lazy loading, code splitting

### User Experience

- [ ] Loading states for all async operations
- [ ] Error messages are user-friendly
- [ ] Success feedback via toast notifications
- [ ] Smooth transitions and animations
- [ ] Dark mode fully supported

---

**Remember:** This is a living spec. Update individual feature files if you discover missing requirements or better approaches during implementation.

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
  },
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
