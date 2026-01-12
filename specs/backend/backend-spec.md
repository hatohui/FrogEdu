# Backend Technical Specification: Edu-AI Classroom

## 1. Overview

The backend is a distributed microservices system built on .NET 9. It utilizes domain-driven design (DDD) principles and Clean Architecture to ensure separation of concerns.

## 2. Global Architecture

- **Pattern:** Microservices
- **Communication:**
  - **External:** HTTP/REST via API Gateway.
  - **Internal:** Asynchronous Event-Driven (MassTransit/RabbitMQ) + Direct gRPC for high-performance synchronous reads.
- **Auth:** AWS Cognito (Identity Provider).

## 3. Service Boundaries

Each service follows the "Clean Architecture" structure with 4 specific projects:

1. `*.Domain` (Entities, Value Objects, Domain Events)
2. `*.Application` (Use Cases, CQRS Handlers, Interfaces)
3. `*.Infrastructure` (Ef Core, External adapters, AWS SDK implementations)
4. `*.API` (Controllers, Minimal API endpoints, DI Setup)

### 3.1 Service Catalogue

#### A. Content Service

_Responsible for educational materials structure._

- **Responsibilities:**
  - Manage Textbooks, Chapters, Lessons.
  - Store metadata for static assets (PDFs, Images in S3).
- **Data Model:**
  - `Textbook` (Id, Name, GradeLevel)
  - `Chapter` (TextbookId, Order)
  - `Page` (ContentUrl, vectors)

#### B. Assessment Service

_Responsible for testing and evaluation._

- **Responsibilities:**
  - **Question Bank:** CRUD for questions (MCQ, Essay).
  - **Exam Matrix:** Logic for selecting questions based on metadata.
  - **Exam Generation:** Assembling specific instances of exams.
- **Data Model:**
  - `Question` (Content, Difficulty, ChapterRef)
  - `ExamMatrix` (Config rules)
  - `ExamPaper` (Generated artifact references)

#### C. User Profile Service

_Responsible for user-specific data not handled by Cognito._

- **Responsibilities:**
  - Profile management (Avatar, Bio).
  - Role management (Teacher/Student specific settings).
  - Class enrollment mapping.
- **Auth:** Syncs simplistic user existence via Cognito Post-Confirmation Lambda or Webhooks.

#### D. AI-Orchestrator Service

_Responsible for LLM interactions and context management._

- **Responsibilities:**
  - "Socratic Tutor" logic pipeline.
  - RAG (Retrieval-Augmented Generation) coordination using vectors from Content Service.
  - Prompt Engineering management.
- **Tech:** Semantic Kernel or LangChain for .NET.

## 4. Infrastructure Components

### 4.1 API Gateway

- **Technology:** YARP (Yet Another Reverse Proxy) or AWS API Gateway.
- **Role:**
  - Single entry point.
  - JWT System Validation (Cognito).
  - Route routing to `http://content-service`, `http://assessment-service`, etc.

### 4.2 Database Strategy

- **Type:** SQL Server.
- **Pattern:** Database-per-Service.
  - `ContentDB`
  - `AssessmentDB`
  - `UserDB`
  - `AiContextDB` (or Vector DB like Pinecone/pgvector if needed, strictly SQL Server for transactional logs).

### 4.3 Storage (AWS S3)

- **Usage:**
  - `edu-classroom-assets/textbooks/`
  - `edu-classroom-assets/generated-exams/`
- **Access:** Services generate Presigned URLs for frontend access.

## 5. Development Guidelines

- **DTOs:** Request/Response models must be strictly defined in the Application layer.
- **Validation:** FluentValidation for all input commands.
- **Observability:** OpenTelemetry for distributed tracing.

## 6. Milestones & Tasks

### Milestone 1: Infrastructure & Shared Kernel - [ ]

- [x] Set up Solution Structure (4 folders: Domain, App, Infra, API per service).
- [ ] Configure "Shared Kernel" (Common DTOs, Exceptions, Extensions).
- [ ] Setup Docker Compose for SQL and Localstack (S3).
- [ ] Implement API Gateway (YARP) foundation.

### Milestone 2: Content Service - [ ]

- [ ] Define Domain Entities (Textbook, Chapter).
- [ ] Implement EF Core Migrations & Seed Data.
- [ ] Create S3 Service for Asset Upload/Retrieval (Presigned URLs).
- [ ] Create API Endpoints (CRUD for Content).

### Milestone 3: Use Profile & Assessment Service - [ ]

- [ ] **User Service:** Implement User Profile Sync (Webhook/Lambda from Cognito).
- [ ] **Assessment Service:** Define Question Bank Entities.
- [ ] **Assessment Service:** Implement Matrix Logic (Question Selection Algorithm).
- [ ] **Assessment Service:** API for Exam Generation (PDF Export).

### Milestone 4: AI Orchestrator Service - [ ]

- [ ] Setup Semantic Kernel / LangChain integration.
- [ ] Implement RAG Pipeline (Vector Search connection).
- [ ] Create "Tutor Chat" Endpoint (Streaming Response).
- [ ] Implement Prompt Engineering Templates.

### Milestone 5: Integration & Deployment - [ ]

- [ ] Configure Internal Event Bus (MassTransit) for async communication.
- [ ] Ensure API Gateway routes correctly to all microservices.
- [ ] Dockerize all 4 services.
- [ ] Finalize Swagger/OpenAPI documentation.
