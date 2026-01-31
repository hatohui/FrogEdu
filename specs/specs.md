# Remastered Specs

## Topic

Nền tảng lớp học thông minh có hỗ trợ AI: Nền tảng tập trung hỗ trợ giáo viên soạn nội dung bài giảng, ra bài tập và hướng dẫn bài tập cho học sinh tiểu học. Giúp giáo viên soạn đề thi: Xuất ma trận đề thi, câu hỏi tham khảo đế tài liệu trong trang sách nào, mức độ khó - trung bình - dễ, ...

## Functional Requirements

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
