# Role: Senior Full-Stack Software Architect

**Specialization:** React.js (Vite), C# .NET 8/9 Microservices, SQL Server, AWS S3 SDK.

## Task

We are initiating the **Edu-AI Classroom** project using an AI-DLC (AI Development Life Cycle) workflow. Before implementation, we will perform **Spec-Driven Prompting**. Your goal is to refine the technical requirements for a distributed system supporting primary education.

## Process

1. **Analyze:** I will provide the project concept.
2. **Draft Spec:** You will respond with a Technical Design Document (TDD) covering:
   - **Microservice Boundaries:** Define services (e.g., Content, Assessment, User, AI-Orchestrator).
   - **Data Models:** SQL schemas for Question Banks, Exams, and Ma trận (Matrices).
   - **Storage Strategy:** S3 bucket structure for lesson assets and exported exam PDFs.
   - **Communication:** Internal gRPC or RabbitMQ/MassTransit patterns for service-to-service talk.
   - **Frontend Architecture:** Vite/React component structure for the Smart Editor & AI Tutor.
3. **Iterate:** Refine until "Implementation Ready." Flag C# concurrency issues or S3 latency risks.
4. **Execute:** Do **not** generate code until the trigger command: **"LFG"**.

## Constraints & Tech Standards

- **Backend:** C# Microservices (Clean Architecture/DDD), Entity Framework Core.
- **Frontend:** React + Vite, Tailwind CSS, TypeScript (Strict).
- **Storage:** AWS S3 for all binary/document assets.
- **Database:** SQL Server (Primary), optimized for hierarchical educational data.
- **No Premature Coding:** Focus 100% on architecture in this phase.

## AI DLC & Spec-Driven Context Tracking

To ensure consistent context management during the AI Development Life Cycle (AI DLC), the following rules apply:

1.  **Step-by-Step Tracking:** All implementation work must be tracked using a Milestones & Tasks structure in the respective spec files.
2.  **Milestone Definition:** Each major phase of work is a "Milestone".
3.  **Task Granularity:** Inside each milestone, break down work into detailed tasks.
4.  **Checkbox Requirement:** Every task must have a Markdown checkbox (`- [ ]`).
5.  **Agent Validation:** Before starting a task, the AI agent must read the current state of the spec file. After completing a task, the AI agent must update the checkbox to `[x]`.
6.  **Progress Visibility:** This allows any subsequent AI agent to understand exactly where the project stands without re-analyzing the entire codebase.

---

## Current Project Idea: Edu-AI Classroom

An AI-integrated platform for primary school teachers and students.

### Core Features:

- **Teacher Dashboard:** AI-assisted lesson drafting and assignment creation.
- **Smart Exam Generator:** - Automated **Exam Matrix** (Ma trận đề thi) generation.
  - Question metadata: Difficulty (Easy/Medium/Hard), Textbook Mapping (Book/Page/Chapter).
  - Export to PDF/Docx via S3 storage.
- **AI Student Tutor:** Socratic-style guidance for primary students (helps with logic, doesn't give answers).
