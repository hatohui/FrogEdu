# Frontend Technical Specification: Edu-AI Classroom

## 1. Overview

This document outlines the frontend architecture for the "Edu-AI Classroom" platform. The application is built using React (Vite) and provides interfaces for Teachers (Dashboard, Exam Gen) and Students (AI Tutor).

## 2. Technology Stack

- **Framework:** React 19 (via Vite)
- **Language:** TypeScript 5.x (Strict Mode)
- **Styling:** Tailwind CSS
- **Component Library:** Shadcn UI (Radix Primitives)
- **State/Async Management:** TanStack Query (React Query)
- **Routing:** React Router DOM (Dynamic Routing)
- **Internationalization:** i18next
- **Build Tool:** Vite

## 3. Development Workflow (AI-Assisted)

The development process leverages specific MCP (Model Context Protocol) servers to ensure code quality and documentation accuracy.

### 3.1 Context7 MCP

- **Usage:** Used to resolve up-to-date library IDs and documentation for React, Vite, and third-party packages.
- **Rule:** Before integrating a new complex library, the Agent must query Context7 to verify version compatibility and best practices.

### 3.2 Shadcn MCP

- **Usage:** Used to generate and install Shadcn UI components.
- **Rule:** Do not manually copy-paste component code if the MCP can install it. Ensure `components.json` serves as the source of truth.

## 4. Architecture & Folder Structure

The project follows a feature-based structure to ensure scalability alongside the microservices backend.

```
src/
├── common/              # Shared constants, enums
├── components/          # Shared UI components
│   ├── ui/              # Shadcn primitives (Button, Card, etc.)
│   └── common/          # Composite shared components (Container, Loading)
├── config/              # App configuration (Theme, TanStack, i18n)
├── features/            # Feature-specific modules (replicating Service boundaries)
│   ├── auth/            # Cognito integration components
│   ├── content/         # Textbook/Lesson management
│   ├── assessment/      # Exam generation, Question banks
│   ├── ai-tutor/        # Chat interface, Socratic logic visualizers
│   └── user/            # Profile management
├── hooks/               # Global hooks (useAuth)
├── layouts/             # App shells (DashboardLayout, StudentLayout)
├── lib/                 # Utility libraries (axios instance, utils)
├── pages/               # Route entry points
├── services/            # API wrappers (one file per backend service)
└── types/               # Global TypeScript definitions
```

## 5. Core Features & UI Requirements

### 5.1 Teacher Dashboard

- **Layout:** Sidebar navigation, Header with User Profile.
- **Functionality:**
  - Create/Manage Classes.
  - Access "Smart Exam Generator".
  - View "Content Library".

### 5.2 Smart Exam Generator (Assessment Module)

- **UI:** Multi-step wizard or Split-pane view.
- **Matrix Builder:**
  - Interactive table to define distribution (e.g., 30% Easy, 50% Medium).
  - Dropdowns for Textbook/Chapter mappings (fetched from Content Service).
- **Preview:** Real-time PDF preview (rendered from S3 URL or generated client-side).

### 5.3 AI Student Tutor

- **UI:** Chat interface with rich-text support (Markdown/MathJax).
- **Interaction:**
  - Input field for student questions.
  - "Thinking" indicators for stream responses.
  - Side panel for reference material (Textbook pages).

## 6. API Integration

- **Gateway:** All requests route through a single API Gateway URL.
- **Authentication:**
  - AWS Cognito JWTs attached to `Authorization` header via Axios interceptors.
- **Code Generation:** API client code (interfaces/services) should be aligned with backend DTOs.

## 7. Milestones & Tasks

### Milestone 1: Project Setup & Core Infrastructure - [ ]

- [x] Initialize Vite Project with React 19 & TypeScript.
- [ ] Setup Tailwind CSS & Shadcn UI (via Shadcn MCP).
- [ ] Configure TanStack Query & Axios Interceptors (Auth).
- [ ] Setup React Router DOM with Dynamic Routing.
- [ ] Implement Basic Layouts (AuthLayout, DashboardLayout).
- [ ] Set up i18next for multi-language support.

### Milestone 2: Authentication & User Profile - [ ]

- [ ] Create Login/Register Pages (Cognito Integration).
- [ ] Implement `useAuth` hook with Context API.
- [ ] Create User Profile Page (View/Edit).
- [ ] Protect Private Routes (React Router Guards).

### Milestone 3: Content & Teacher Dashboard - [ ]

- [ ] Build Teacher Dashboard Landing Page.
- [ ] Create "Content Library" View (List Textbooks/Chapters).
- [ ] Implement Content Detail View (PDF Preview/Viewer).
- [ ] Dashboard Widgets (Recent Activity, Quick Actions).

### Milestone 4: Smart Exam Generator - [ ]

- [ ] Build "New Exam" Wizard/Form.
- [ ] Implement Matrix Builder (Distribution Table).
- [ ] Create Question Selection Interface (Mapping Textbooks).
- [ ] Build Exam Preview Component (PDF/Docx Rendering).

### Milestone 5: AI Student Tutor - [ ]

- [ ] Implement Chat Interface (Messages, Input, Stream).
- [ ] Integrate Markdown/MathJax Rendering for Tutor Responses.
- [ ] Build Side Panel for Lesson References.
- [ ] Connect Chat to AI-Orchestrator Service (Mock initially).

### Milestone 6: Polish & Optimization - [ ]

- [ ] Optimize Bundle Size (Main splits).
- [ ] Audit Accessibility (A11y) for primary school students.
- [ ] Comprehensive E2E Testing.
