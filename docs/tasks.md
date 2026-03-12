# FrogEdu — Feature Task List

> Columns: **No** | **Task Name** | **Category** | **%** | **Assignee** | **Status** | **Note**
> Categories: `Set Up` · `Deployment` · `Wiring` · `Data` · `Test`
> Statuses: `Done` · `In Progress` · `Not Started`

---

## Feature 1 — User Authentication & Identity

| No   | Task Name                                                                       | Category   | %   | Assignee | Status      | Note                                      |
| ---- | ------------------------------------------------------------------------------- | ---------- | --- | -------- | ----------- | ----------------------------------------- |
| 1.1  | Set up AWS Cognito User Pool (Hosted UI, Google OAuth, custom `role` attribute) | Set Up     | 100 |          | Done        |                                           |
| 1.2  | Set up Amazon SES for transactional email (verification, password reset)        | Set Up     | 100 |          | Done        | Custom `mail.` subdomain                  |
| 1.3  | Set up Cloudflare R2 bucket for user-uploaded assets                            | Set Up     | 100 |          | Done        | S3-compatible SDK                         |
| 1.4  | Set up MinIO locally (dev asset storage)                                        | Set Up     | 100 |          | Done        | Docker Compose                            |
| 1.5  | Deploy User Service as containerized Lambda (ECR image)                         | Deployment | 100 |          | Done        |                                           |
| 1.6  | Configure API Gateway HTTP routes + Cognito JWT authorizer for User Service     | Deployment | 100 |          | Done        | No-auth routes: `/auth/webhook`, `/roles` |
| 1.7  | Wire Cognito post-confirmation webhook → `CreateUser` command                   | Wiring     | 100 |          | Done        | User record created on Cognito sign-up    |
| 1.8  | Wire cross-service user lookup by Cognito sub (internal `/by-cognito/{id}`)     | Wiring     | 100 |          | Done        | Used by Class / Exam services             |
| 1.9  | Wire role-change → sync `custom:role` back to Cognito attribute                 | Wiring     | 100 |          | Done        | `UpdateUserRole` command                  |
| 1.10 | Wire Subscription Service HTTP call to enrich user profile with plan data       | Wiring     | 100 |          | Done        | `GET /users/{id}/subscription`            |
| 1.11 | User & Role DB schema + EF Core migrations                                      | Data       | 100 |          | Done        | Soft-delete on `User`                     |
| 1.12 | Pre-signed R2 upload URL generation (`GET /assets/sign-url`)                    | Data       | 100 |          | Done        |                                           |
| 1.13 | Implement asset delete endpoint (`DELETE /assets/{assetId}`)                    | Data       | 0   |          | Not Started | Stub — not yet implemented                |
| 1.14 | Test: Cognito webhook creates user record on sign-up                            | Test       | 100 |          | Done        |                                           |
| 1.15 | Test: Email verification and password reset end-to-end                          | Test       | 100 |          | Done        |                                           |
| 1.16 | Test: Role sync updates Cognito custom attribute                                | Test       | 100 |          | Done        |                                           |

---

## Feature 2 — Exam Content Management

| No   | Task Name                                                                            | Category   | %   | Assignee | Status | Note                                         |
| ---- | ------------------------------------------------------------------------------------ | ---------- | --- | -------- | ------ | -------------------------------------------- |
| 2.1  | Set up Exam Service project (Clean Architecture, CQRS, MediatR)                      | Set Up     | 100 |          | Done   |                                              |
| 2.2  | Set up dedicated PostgreSQL database for Exam Service                                | Set Up     | 100 |          | Done   | Port 5433 locally, NeonDB in prod            |
| 2.3  | Deploy Exam Service as containerized Lambda (ECR image)                              | Deployment | 100 |          | Done   |                                              |
| 2.4  | Configure API Gateway routes for Exam Service (no-auth: `/exams/{id}/session-data`)  | Deployment | 100 |          | Done   |                                              |
| 2.5  | Wire Exam Service `GET /exams/{id}/session-data` → Class Service (internal, no-auth) | Wiring     | 100 |          | Done   | Called by Class Service during exam sessions |
| 2.6  | Subject & Topic DB schema + EF Core migrations (grades 1–12 curriculum)              | Data       | 100 |          | Done   |                                              |
| 2.7  | Question + Answer DB schema (type, cognitive level, media URL, point, visibility)    | Data       | 100 |          | Done   | Bloom's Taxonomy enum                        |
| 2.8  | Exam DB schema (draft/publish lifecycle, shuffle & scoring config)                   | Data       | 100 |          | Done   |                                              |
| 2.9  | Matrix DB schema (`MatrixTopic`: topic × cognitive level × quantity)                 | Data       | 100 |          | Done   |                                              |
| 2.10 | PDF export handler (`GET /exams/{id}/export/pdf`)                                    | Data       | 100 |          | Done   |                                              |
| 2.11 | Excel export handler (`GET /exams/{id}/export/excel`)                                | Data       | 100 |          | Done   |                                              |
| 2.12 | Test: Exam draft → publish lifecycle                                                 | Test       | 100 |          | Done   |                                              |
| 2.13 | Test: Attach / detach matrix to exam                                                 | Test       | 100 |          | Done   |                                              |
| 2.14 | Test: Add / remove questions batch operations                                        | Test       | 100 |          | Done   |                                              |
| 2.15 | Test: PDF and Excel export correctness                                               | Test       | 100 |          | Done   |                                              |

---

## Feature 3 — Classroom Management

| No   | Task Name                                                                           | Category   | %   | Assignee | Status | Note              |
| ---- | ----------------------------------------------------------------------------------- | ---------- | --- | -------- | ------ | ----------------- |
| 3.1  | Set up Class Service project (Clean Architecture, CQRS, MediatR)                    | Set Up     | 100 |          | Done   |                   |
| 3.2  | Set up dedicated PostgreSQL database for Class Service                              | Set Up     | 100 |          | Done   | Port 5434 locally |
| 3.3  | Deploy Class Service as containerized Lambda (ECR image)                            | Deployment | 100 |          | Done   |                   |
| 3.4  | Configure API Gateway routes for Class Service                                      | Deployment | 100 |          | Done   |                   |
| 3.5  | Wire Class Service → Exam Service (`session-data`) for exam content during sessions | Wiring     | 100 |          | Done   |                   |
| 3.6  | Wire Class Service → User Service for student identity lookup                       | Wiring     | 100 |          | Done   |                   |
| 3.7  | ClassRoom DB schema (invite code, max students, banner, teacher FK)                 | Data       | 100 |          | Done   |                   |
| 3.8  | ClassEnrollment + Assignment DB schema                                              | Data       | 100 |          | Done   |                   |
| 3.9  | ExamSession DB schema (scheduling config, retry limit, shuffle, partial scoring)    | Data       | 100 |          | Done   |                   |
| 3.10 | StudentExamAttempt + StudentAnswer DB schema                                        | Data       | 100 |          | Done   |                   |
| 3.11 | Test: Student joins class via invite code                                           | Test       | 100 |          | Done   |                   |
| 3.12 | Test: Teacher assigns exam to class (with due date)                                 | Test       | 100 |          | Done   |                   |
| 3.13 | Test: Student starts attempt, answers questions, submits                            | Test       | 100 |          | Done   |                   |
| 3.14 | Test: Session results aggregation (scores, pass rates)                              | Test       | 100 |          | Done   |                   |

---

## Feature 4 — Subscription & Monetization

| No   | Task Name                                                                      | Category   | %   | Assignee | Status      | Note                               |
| ---- | ------------------------------------------------------------------------------ | ---------- | --- | -------- | ----------- | ---------------------------------- |
| 4.1  | Set up Subscription Service project (Clean Architecture, CQRS, MediatR)        | Set Up     | 100 |          | Done        |                                    |
| 4.2  | Set up dedicated PostgreSQL database for Subscription Service                  | Set Up     | 100 |          | Done        | Port 5435 locally                  |
| 4.3  | Deploy Subscription Service as containerized Lambda (ECR image)                | Deployment | 100 |          | Done        |                                    |
| 4.4  | Configure API Gateway routes for Subscription Service                          | Deployment | 100 |          | Done        |                                    |
| 4.5  | Wire internal claims endpoint (`GET /claims/{userId}`) → AI Service validation | Wiring     | 100 |          | Done        | AI Service checks Pro subscription |
| 4.6  | Wire Subscription → User Service for user profile enrichment                   | Wiring     | 100 |          | Done        |                                    |
| 4.7  | SubscriptionTier DB schema (Free / Pro, per-role pricing)                      | Data       | 100 |          | Done        |                                    |
| 4.8  | UserSubscription DB schema (start/end date, status lifecycle)                  | Data       | 100 |          | Done        |                                    |
| 4.9  | Transaction DB schema (payment provider enum, payment status ledger)           | Data       | 100 |          | Done        | Mock only — no real payment        |
| 4.10 | AIUsageRecord DB schema (per-user daily/monthly counters)                      | Data       | 100 |          | Done        |                                    |
| 4.11 | Integrate real payment provider (VNPay / Stripe)                               | Wiring     | 0   |          | Not Started | Currently mock transactions        |
| 4.12 | Test: Subscribe, cancel, renew subscription lifecycle                          | Test       | 100 |          | Done        |                                    |
| 4.13 | Test: AI usage limit enforcement per plan                                      | Test       | 100 |          | Done        |                                    |
| 4.14 | Test: Admin suspend / activate subscription                                    | Test       | 100 |          | Done        |                                    |

---

## Feature 5 — AI Question Generation & Tutoring

| No   | Task Name                                                                               | Category   | %   | Assignee | Status | Note                      |
| ---- | --------------------------------------------------------------------------------------- | ---------- | --- | -------- | ------ | ------------------------- |
| 5.1  | Set up Python/FastAPI AI Service project structure                                      | Set Up     | 100 |          | Done   | Mangum adapter for Lambda |
| 5.2  | Set up Google Gemini API integration                                                    | Set Up     | 100 |          | Done   |                           |
| 5.3  | Deploy AI Service as containerized Lambda (ECR image)                                   | Deployment | 100 |          | Done   |                           |
| 5.4  | Configure API Gateway routes for AI Service (all auth-required except `/health`)        | Deployment | 100 |          | Done   |                           |
| 5.5  | Wire AI Service → Cognito JWKS for JWT validation                                       | Wiring     | 100 |          | Done   |                           |
| 5.6  | Wire AI Service → Subscription Service claims check before serving AI responses         | Wiring     | 100 |          | Done   | Pro plan required         |
| 5.7  | Implement `POST /questions/generate` (batch question generation from matrix config)     | Data       | 100 |          | Done   | Teacher + Pro             |
| 5.8  | Implement `POST /questions/generate-single` (single question per topic/cognitive level) | Data       | 100 |          | Done   | Teacher + Pro             |
| 5.9  | Implement `POST /tutor/chat` (Socratic student tutoring)                                | Data       | 100 |          | Done   | Student + Pro             |
| 5.10 | Implement `POST /explain` (child-friendly question explanation)                         | Data       | 100 |          | Done   |                           |
| 5.11 | Implement `POST /grade-essay` (AI essay grading)                                        | Data       | 100 |          | Done   |                           |
| 5.12 | Wire AI usage recording → Subscription Service after each AI call                       | Wiring     | 100 |          | Done   | `RecordAIUsage` command   |
| 5.13 | Test: Question generation respects matrix topic/cognitive level distribution            | Test       | 100 |          | Done   |                           |
| 5.14 | Test: Non-Pro user is blocked from AI endpoints                                         | Test       | 100 |          | Done   |                           |
| 5.15 | Test: Daily/monthly usage limit blocks calls after threshold                            | Test       | 100 |          | Done   |                           |

---

## Feature 6 — Admin Dashboard

| No   | Task Name                                                                                | Category   | %   | Assignee | Status      | Note                                            |
| ---- | ---------------------------------------------------------------------------------------- | ---------- | --- | -------- | ----------- | ----------------------------------------------- |
| 6.1  | Set up admin route guard (`AdminRoute` component)                                        | Set Up     | 100 |          | Done        |                                                 |
| 6.2  | Deploy admin dashboard pages as part of frontend build                                   | Deployment | 100 |          | Done        |                                                 |
| 6.3  | Wire dashboard user stats → User Service (`/users/dashboard-stats`, `/users/statistics`) | Wiring     | 100 |          | Done        |                                                 |
| 6.4  | Wire dashboard subscription stats → Subscription Service (`/dashboard-stats`)            | Wiring     | 100 |          | Done        |                                                 |
| 6.5  | Wire admin user CRUD → User Service (search, role change, soft-delete)                   | Wiring     | 100 |          | Done        |                                                 |
| 6.6  | Wire admin subscription management → Subscription Service (activate, suspend, tier CRUD) | Wiring     | 100 |          | Done        |                                                 |
| 6.7  | Dashboard analytics charts (user growth, role distribution, daily activity)              | Data       | 100 |          | Done        | Recharts                                        |
| 6.8  | Admin subject/curriculum management page (`/dashboard/subjects`)                         | Data       | 100 |          | Done        |                                                 |
| 6.9  | Admin exam sessions overview page (`/dashboard/exam-sessions`)                           | Data       | 100 |          | Done        |                                                 |
| 6.10 | Build Analytics microservice API (dedicated analytics endpoints)                         | Data       | 0   |          | Not Started | Described in `docs/dashboard-implementation.md` |
| 6.11 | Test: Admin can view/edit all users across roles                                         | Test       | 100 |          | Done        |                                                 |
| 6.12 | Test: Admin role bypasses all per-role access checks                                     | Test       | 100 |          | Done        |                                                 |

---

## Feature 7 — Frontend Application (Teacher & Student)

| No   | Task Name                                                                                   | Category   | %   | Assignee | Status      | Note                               |
| ---- | ------------------------------------------------------------------------------------------- | ---------- | --- | -------- | ----------- | ---------------------------------- |
| 7.1  | Set up Vite + TypeScript + Tailwind + Shadcn/ui project                                     | Set Up     | 100 |          | Done        |                                    |
| 7.2  | Set up TanStack Query, Zustand, React Hook Form + Zod                                       | Set Up     | 100 |          | Done        |                                    |
| 7.3  | Set up AWS Amplify Cognito client (Hosted UI, OAuth callback)                               | Set Up     | 100 |          | Done        |                                    |
| 7.4  | Set up i18n (react-i18next, EN/VI language files)                                           | Set Up     | 70  |          | In Progress | See `specs/language-spec.md`       |
| 7.5  | Deploy frontend to Cloudflare Pages (via `wrangler.toml`)                                   | Deployment | 100 |          | Done        |                                    |
| 7.6  | Configure `_redirects` for SPA routing on Cloudflare Pages                                  | Deployment | 100 |          | Done        |                                    |
| 7.7  | Wire Axios interceptors (Cognito JWT injection, 401 refresh)                                | Wiring     | 100 |          | Done        |                                    |
| 7.8  | Wire dynamic router (role-based route rendering)                                            | Wiring     | 100 |          | Done        | `config/dynamicRouter.tsx`         |
| 7.9  | Landing page (hero, feature cards, FAQ, CTA)                                                | Data       | 100 |          | Done        | Fully i18n-translated              |
| 7.10 | Auth pages (login, register, forgot/reset password, role selection, subscription selection) | Data       | 100 |          | Done        |                                    |
| 7.11 | Exam management pages (list, create, edit, preview, question editor)                        | Data       | 100 |          | Done        |                                    |
| 7.12 | Matrix builder pages (list, create, edit)                                                   | Data       | 100 |          | Done        |                                    |
| 7.13 | Class management pages (list, detail, create, join, student list)                           | Data       | 100 |          | Done        |                                    |
| 7.14 | Exam session pages (schedule, take exam, results, attempt review)                           | Data       | 100 |          | Done        |                                    |
| 7.15 | AI question generation UI (`AIQuestionPreview`, cognitive level + topic selectors)          | Data       | 100 |          | Done        |                                    |
| 7.16 | AI tutor chat UI                                                                            | Data       | 100 |          | Done        |                                    |
| 7.17 | Subscription & profile pages                                                                | Data       | 100 |          | Done        |                                    |
| 7.18 | Complete i18n string audit and fill missing VI translations                                 | Data       | 30  |          | In Progress | `specs/language-spec.md` checklist |
| 7.19 | Test: Auth flow (register → verify → role select → subscribe)                               | Test       | 100 |          | Done        |                                    |
| 7.20 | Test: Exam creation → matrix attach → publish flow                                          | Test       | 100 |          | Done        |                                    |
| 7.21 | Test: Student joins class → takes exam → views results                                      | Test       | 100 |          | Done        |                                    |

---

## Feature 8 — Infrastructure & DevOps

| No   | Task Name                                                                        | Category   | %   | Assignee | Status | Note                         |
| ---- | -------------------------------------------------------------------------------- | ---------- | --- | -------- | ------ | ---------------------------- |
| 8.1  | Set up Docker Compose local dev environment (4 Postgres DBs + MinIO + Nginx)     | Set Up     | 100 |          | Done   |                              |
| 8.2  | Set up Terraform project structure (provider, modules, Doppler secrets)          | Set Up     | 100 |          | Done   |                              |
| 8.3  | Set up ECR repositories for all 5 service images (lifecycle policy: keep 3)      | Set Up     | 100 |          | Done   |                              |
| 8.4  | Set up GitHub Actions OIDC role for CI/CD (`scripts/build-and-push.ps1`)         | Set Up     | 100 |          | Done   |                              |
| 8.5  | Deploy CloudFront distribution + WAF Web ACL + custom domain                     | Deployment | 100 |          | Done   |                              |
| 8.6  | Deploy API Gateway HTTP API (Cognito JWT authorizer, per-service no-auth routes) | Deployment | 100 |          | Done   |                              |
| 8.7  | Deploy all Lambda functions from ECR images (env vars via Doppler)               | Deployment | 100 |          | Done   |                              |
| 8.8  | Configure Nginx API gateway for local dev (routing to all service containers)    | Deployment | 100 |          | Done   | `nginx.conf`                 |
| 8.9  | Wire Doppler → Terraform (all secrets sourced from Doppler provider)             | Wiring     | 100 |          | Done   |                              |
| 8.10 | Wire NeonDB connection strings → Lambda env vars per service                     | Wiring     | 100 |          | Done   |                              |
| 8.11 | Wire CloudFront → API Gateway origin with verification secret header             | Wiring     | 100 |          | Done   |                              |
| 8.12 | Dev Dockerfiles with hot-reload for all .NET services                            | Data       | 100 |          | Done   | `dev.Dockerfile` per service |
| 8.13 | EF Core migrations automated on Lambda cold start                                | Data       | 100 |          | Done   |                              |
| 8.14 | Test: `GET /health` endpoint for all services (DB connectivity check)            | Test       | 100 |          | Done   |                              |
| 8.15 | Test: Full prod traffic path (Browser → CF Pages → CloudFront → API GW → Lambda) | Test       | 100 |          | Done   |                              |
