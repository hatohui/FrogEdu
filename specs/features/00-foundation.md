# Feature 00: Foundation & Infrastructure

**Status:** ✅ Complete  
**Priority:** P0 (Blocking)  
**Effort:** 8-10 hours  
**Dependencies:** None (Foundation)

---

## 1. Overview

This feature establishes the foundational infrastructure for the FrogEdu platform. It sets up the serverless backend (AWS Lambda, Cognito, Neon Postgres), the frontend (React + Vite + Cloudflare Pages), and the shared kernel for the Clean Architecture solution.

### Goals

- ✅ Initialize Monorepo/Solution structure
- ✅ Set up Infrastructure as Code (Terraform)
- ✅ Configure CI/CD Pipelines
- ✅ Initialize Database & Auth Provider
- ✅ Establish shared coding standards & linting

### Success Metrics

- [ ] All services can be deployed via CI/CD
- [ ] Infrastructure provisioning takes < 10 minutes
- [ ] Local development environment setup takes < 5 minutes
- [ ] Zero hardcoded secrets in repositories
- [ ] 100% infrastructure as code (no manual AWS console changes)

---

## 2. Architecture

### Tech Stack

- **Infrastructure:** AWS (Lambda, API Gateway, Cognito, CloudFront), Cloudflare (Pages, R2), Neon (Postgres)
- **Backend:** .NET 9, Entity Framework Core, gRPC (service-to-service), REST (client-server)
- **Frontend:** React 18, Vite, TypeScript, Shadcn UI, Zustand, TanStack Query
- **DevOps:** Terraform, GitHub Actions, Docker

### Architecture Diagram

```
┌─────────────────┐
│  Cloudflare     │
│  Pages (SPA)    │
└────────┬────────┘
         │ HTTPS
         ▼
┌─────────────────┐       ┌──────────────┐
│   CloudFront    │◄──────│  AWS WAF     │
│   (CDN)         │       └──────────────┘
└────────┬────────┘
         │
         ▼
┌─────────────────┐       ┌──────────────┐
│  API Gateway    │◄──────│  AWS Cognito │
│  (REST/HTTP)    │       │  (Auth)      │
└────────┬────────┘       └──────────────┘
         │
    ┌────┴─────┬──────────┬───────────┐
    │          │          │           │
    ▼          ▼          ▼           ▼
┌────────┐ ┌────────┐ ┌────────┐ ┌────────┐
│ User   │ │Content │ │Assess  │ │  AI    │
│Lambda  │ │Lambda  │ │Lambda  │ │Lambda  │
└───┬────┘ └───┬────┘ └───┬────┘ └───┬────┘
    │          │          │           │
    │          │          │           │ gRPC
    │          └──────────┴───────────┘
    │
    ▼
┌─────────────────────────────────────────┐
│         Neon Postgres (Serverless)      │
│  ┌──────┐ ┌──────┐ ┌──────┐ ┌──────┐   │
│  │users │ │content│ │assess│ │  ai  │   │
│  └──────┘ └──────┘ └──────┘ └──────┘   │
└─────────────────────────────────────────┘

┌─────────────────┐
│  Cloudflare R2  │
│  (Object Store) │
└─────────────────┘
```

### Communication Patterns

**Frontend → Backend:**

- Protocol: HTTPS/REST
- Authentication: JWT (AWS Cognito)
- Format: JSON
- CORS: Enabled with origin whitelist

**Lambda → Lambda:**

- Protocol: gRPC (internal)
- Authentication: IAM roles
- Timeouts: 30s max
- Retry: 3 attempts with exponential backoff

**Lambda → Neon:**

- Connection pooling via Npgsql
- Max connections: 20 per Lambda
- Command timeout: 30s
- Retry on transient errors

### Environment Strategy

| Environment | Branch    | Database     | Deploy Trigger  | URL                 |
| ----------- | --------- | ------------ | --------------- | ------------------- |
| Development | `dev`     | neon-dev     | Push to dev     | dev.frogedu.app     |
| Staging     | `staging` | neon-staging | Push to staging | staging.frogedu.app |
| Production  | `main`    | neon-prod    | Manual approval | frogedu.app         |

---

## 3. Implementation Checklist

### 3.1 Infrastructure & DevOps

- [x] **IaC Setup**: Initialize Terraform project in `infra/`.
  - [x] Provider configuration (AWS, Cloudflare, Neon).
  - [x] Remote state management (S3 backend).
- [x] **AWS Setup**:
  - [x] Create Free Tier AWS Org/Account.
  - [x] Configure `aws-cli` profiles.
- [x] **CI/CD**:
  - [x] Set up GitHub Actions for Backend Build & Test.
  - [x] Set up GitHub Actions for Frontend Build & Deploy (Cloudflare Pages).
  - [x] Configure Secrets in GitHub Repository.

### 3.2 Backend Foundation (`backend/`)

- [x] **Solution Initialization**:
  - [x] Create `FrogEdu.sln`.
  - [x] Create `Shared.Kernel` project (Result pattern, Entity base, Domain Events).
  - [x] Create `directory.packages.props` for Central Package Management.
- [x] **Database Setup**:
  - [x] Provision Neon Postgres instance (`frogedu-dev`).
  - [x] Create 4 databases: `users`, `content`, `assessments`, `ai`.
- [x] **Auth Setup**:
  - [x] Create AWS Cognito User Pool.
  - [x] Configure App Client (no secret for frontend).

### 3.3 Frontend Foundation (`frontend/`)

- [x] **Project Setup**:
  - [x] Initialize Vite + React + TypeScript project.
  - [x] Configure `tsconfig.json` (Strict mode, paths).
  - [x] Setup `vite.config.ts` (Aliases: `@ -> src`).
- [x] **UI Framework**:
  - [x] Install Tailwind CSS & `postcss`.
  - [x] Initialize shadcn/ui (`npx shadcn@latest init`).
  - [x] Configure `components.json`.
- [x] **Core Libraries**:
  - [x] Install `tanstack/react-query`, `zustand`, `react-router-dom`.
  - [x] Install `axios`, `zod`, `react-hook-form`.
  - [x] Install `lucide-react`, `i18next`.

### 3.4 Shared Kernel Implementation

- [x] **Base Entities**: Implement `Entity<T>`, `ValueObject`.
- [x] **Result Pattern**: Implement `Result<T>` class.
- [x] **Domain Events**: Implement `IDomainEvent` interface.
- [x] **Repository**: Define `IRepository<T>` and `IReadRepository<T>`.

---

## 4. Validation

- [x] Terraform `plan` runs without errors.
- [x] Backend solution compiles.
- [x] Database connection strings verified.
- [x] Frontend starts locally (`npm run dev`).
- [ ] Shared Kernel unit tests pass.
