# Remastered Specs

## Topic

Nền tảng lớp học thông minh có hỗ trợ AI: Nền tảng tập trung hỗ trợ giáo viên soạn nội dung bài giảng, ra bài tập và hướng dẫn bài tập cho học sinh tiểu học. Giúp giáo viên soạn đề thi: Xuất ma trận đề thi, câu hỏi tham khảo đế tài liệu trong trang sách nào, mức độ khó - trung bình - dễ, ...

## Functional Requirements

1. Smart Exam Generation
   Exam Matrix Builder (difficulty, chapters, question types)
   AI auto-select questions from Question Bank
   Manual question swap with preview
   Export to PDF, upload to S3, generate presigned URL
   Free: 3 exams/month | Pro: unlimited
2. Authentication & User Management
   AWS Cognito email/password login
   Role-based access (Teacher/Student)
   Profile management with avatar (S3)
   JWT authentication and role-based routing
3. Class Management
   Create class with 6-digit invite code
   Student enrollment via code
   Dashboard with stats and activity feed
   Free: 1 class, 30 students | Pro: unlimited
4. Content Library
   Browse textbooks (Grade/Subject/Chapter)
   Free: Grade 1-3 | Pro: All grades
   Upload assets (PDF/images, 50MB max to S3)
   Asset management with presigned URLs
5. Payment & Subscription
   Free tier and Teacher Pro (299k VND/month)
   VNPay (Vietnam) + Stripe (International)
   Upgrade/downgrade/cancel functionality
   Usage tracking with auto-reset
6. Question Bank (Pro)
   CRUD questions with metadata
   Bulk import (Excel/CSV)
   Private/shared banks
7. Analytics
   Free: basic stats | Pro: advanced insights + export

## Infrastructure

Frontend: Browser -> Cloudflare Pages

Static Images: Cloudflare R2

Backend: Cloudflare Pages -> Cloudfront -> WAF -> Cognito -> API Gateway -> Lambdas (4 services) -> NeonDB (4 database per microservice)

## Tech

### Frontend

- React + Vite
- UX, UI: Shadcn + tailwindcss
- Validation + Form: React-hook-form + Zod
- Typescript for type safety
- AWS S3 bucket upload for R2 buckets
- AWS Amplify for Cognito intergration
- Animation: Gsap
- Language: i18next
- State management: Zustand
- Routing: React-router (/pages file based routing)
- Icon Library: lucide-react
- Query tools: Tanstack Query + Axios

### Backend

- .NET Core 9
- Database Migration: EF
- CLEAN architecture
- Docker
- gRPC for server to server (lambda to lambda) and Rest For client-server
- Microservices
- Fluent Validation
- Swagger + OpenAPI

## Mainflows

1. Smart Exam Generation (AI)
2. Analytics
3. Student Learning Flow

## AI-Agent Workflow

- Read Documentations to check for progress and milestones
- Imeplement the next tasks
- Tick the checkbox at the milestones as finished.
