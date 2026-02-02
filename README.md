# FrogEdu - Educational Platform

[![user-service](https://github.com/hatohui/FrogEdu/actions/workflows/user-cd.yaml/badge.svg)](https://github.com/hatohui/FrogEdu/actions/workflows/user-cd.yaml)
[![exam-service](https://github.com/hatohui/FrogEdu/actions/workflows/exam-cd.yaml/badge.svg)](https://github.com/hatohui/FrogEdu/actions/workflows/exam-cd.yaml)
[![class-service](https://github.com/hatohui/FrogEdu/actions/workflows/class-cd.yaml/badge.svg)](https://github.com/hatohui/FrogEdu/actions/workflows/class-cd.yaml)
[![subscription-service](https://github.com/hatohui/FrogEdu/actions/workflows/subscription-cd.yaml/badge.svg)](https://github.com/hatohui/FrogEdu/actions/workflows/subscription-cd.yaml)

FrogEdu is a modern educational platform built with microservices architecture, enabling schools to manage users, classes, exams, and subscriptions efficiently.

## 💻 Tech Stack

### 🖥️ Frontend

![React](https://img.shields.io/badge/React-19-61DAFB?style=for-the-badge&logo=react&logoColor=black)
![TypeScript](https://img.shields.io/badge/TypeScript-5-3178C6?style=for-the-badge&logo=typescript&logoColor=white)
![Vite](https://img.shields.io/badge/Vite-7-646CFF?style=for-the-badge&logo=vite&logoColor=white)
![TailwindCSS](https://img.shields.io/badge/TailwindCSS-4-38B2AC?style=for-the-badge&logo=tailwind-css&logoColor=white)
![shadcn/ui](https://img.shields.io/badge/shadcn/ui-000000?style=for-the-badge)
![Radix UI](https://img.shields.io/badge/Radix_UI-000000?style=for-the-badge)
![TanStack Query](https://img.shields.io/badge/TanStack_Query-5-FF4154?style=for-the-badge&logo=reactquery&logoColor=white)
![Zustand](https://img.shields.io/badge/Zustand-443E38?style=for-the-badge)
![React Router](https://img.shields.io/badge/React_Router-7-CA4245?style=for-the-badge&logo=reactrouter&logoColor=white)
![React Hook Form](https://img.shields.io/badge/React_Hook_Form-7-EC5990?style=for-the-badge)
![Zod](https://img.shields.io/badge/Zod-3-3E67B1?style=for-the-badge)
![i18next](https://img.shields.io/badge/i18next-25-26A69A?style=for-the-badge&logo=i18next&logoColor=white)
![Axios](https://img.shields.io/badge/Axios-1-5A29E4?style=for-the-badge&logo=axios&logoColor=white)
![GSAP](https://img.shields.io/badge/GSAP-3-88CE02?style=for-the-badge&logo=greensock&logoColor=white)

### 🧠 Backend

![.NET 9](https://img.shields.io/badge/.NET-9-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
![Entity Framework Core](https://img.shields.io/badge/EF_Core-9-512BD4?style=for-the-badge)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-17-4169E1?style=for-the-badge&logo=postgresql&logoColor=white)
![JWT](https://img.shields.io/badge/JWT-000000?style=for-the-badge&logo=jsonwebtokens&logoColor=white)
![Swagger](https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black)

### 🗄️ Storage & Infrastructure

![MinIO](https://img.shields.io/badge/MinIO-C72E49?style=for-the-badge&logo=minio&logoColor=white)
![NGINX](https://img.shields.io/badge/NGINX-009639?style=for-the-badge&logo=nginx&logoColor=white)
![Cloudflare R2](https://img.shields.io/badge/Cloudflare_R2-F38020?style=for-the-badge&logo=cloudflare&logoColor=white)
![Cloudflare Pages](https://img.shields.io/badge/Cloudflare_Pages-F38020?style=for-the-badge&logo=cloudflare&logoColor=white)
![AWS API Gateway](https://img.shields.io/badge/AWS_API_Gateway-FF4F8B?style=for-the-badge&logo=amazonapigateway&logoColor=white)
![AWS CloudFront](https://img.shields.io/badge/AWS_CloudFront-8C4FFF?style=for-the-badge&logo=amazonaws&logoColor=white)
![AWS Cognito](https://img.shields.io/badge/AWS_Cognito-FF9900?style=for-the-badge&logo=amazonaws&logoColor=white)
![AWS Lambda](https://img.shields.io/badge/AWS_Lambda-FF9900?style=for-the-badge&logo=awslambda&logoColor=white)
![AWS ECR](https://img.shields.io/badge/AWS_ECR-FF9900?style=for-the-badge&logo=amazonaws&logoColor=white)
![AWS SES](https://img.shields.io/badge/AWS_SES-DD344C?style=for-the-badge&logo=amazonaws&logoColor=white)
![AWS IAM](https://img.shields.io/badge/AWS_IAM-DD344C?style=for-the-badge&logo=amazonaws&logoColor=white)
![Terraform](https://img.shields.io/badge/Terraform-844FBA?style=for-the-badge&logo=terraform&logoColor=white)
![Doppler](https://img.shields.io/badge/Doppler-000000?style=for-the-badge)

### 🚀 DevOps & CI/CD

![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)
![Docker Compose](https://img.shields.io/badge/Docker_Compose-2496ED?style=for-the-badge&logo=docker&logoColor=white)
![GitHub Actions](https://img.shields.io/badge/GitHub_Actions-2088FF?style=for-the-badge&logo=githubactions&logoColor=white)
![CodeRabbit](https://img.shields.io/badge/CodeRabbit-FF6B6B?style=for-the-badge)
![NamingConventionBot](https://img.shields.io/badge/NamingConventionBot-4A90E2?style=for-the-badge)

## 🏗️ Architecture

### Microservices

FrogEdu consists of 4 independent microservices:

- **User Service** - User authentication, profiles, and authorization
- **Class Service** - Class management, student enrollment, and teacher assignments
- **Exam Service** - Exam creation, question banks, and assessment management
- **Subscription Service** - Subscription plans, billing, and feature access control

Each service follows **Clean Architecture** principles with:

- Domain Layer (Core business logic)
- Application Layer (Use cases & business workflows)
- Infrastructure Layer (Data access, external services)
- API Layer (HTTP endpoints, authentication)

## 🚀 Getting Started

### Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Node.js 20+](https://nodejs.org/)
- [Terraform](https://www.terraform.io/downloads) (for infrastructure)
- [Doppler CLI](https://docs.doppler.com/docs/install-cli) (for secrets management)

### Development Setup

#### 1. Environment Configuration

Clone this repository and set up environment variables:

```bash
# Backend services use Doppler for secrets management
# Contact the team for Doppler access token

# For local development without Doppler:
cp .env.example .env
# Then populate .env with required values
```

#### 2. Start Local Infrastructure

Start all backend services with Docker Compose:

```bash
cd backend
docker-compose -f docker-compose.dev.yml up
```

This starts:

- 4 PostgreSQL databases (ports 5432-5435)
- MinIO object storage (ports 9000-9001)
- NGINX API Gateway (port 80)

#### 3. Run Frontend

```bash
cd frontend
npm install
npm run dev
```

The frontend runs at [http://localhost:5173](http://localhost:5173)

### Service Endpoints

- **API Gateway**: [http://localhost:80](http://localhost:80)
- **User Service**: [http://localhost:5001/swagger](http://localhost:5001/swagger)
- **Class Service**: [http://localhost:5002/swagger](http://localhost:5002/swagger)
- **Exam Service**: [http://localhost:5003/swagger](http://localhost:5003/swagger)
- **Subscription Service**: [http://localhost:5004/swagger](http://localhost:5004/swagger)
- **MinIO Console**: [http://localhost:9001](http://localhost:9001)

### Database Access

Connect to PostgreSQL databases:

| Service      | Port | Database     | User | Password |
| ------------ | ---- | ------------ | ---- | -------- |
| User         | 5432 | user         | root | root     |
| Exam         | 5433 | exam         | root | root     |
| Class        | 5434 | class        | root | root     |
| Subscription | 5435 | subscription | root | root     |

## 📋 Development Workflow

### Git Workflow

#### 1. Create an Issue

Before starting work, create or assign an issue:

**Issue Naming Convention:**

```
[FE | feature-name] Issue description
[BE | feature-name] Issue description
```

Examples:

- `[FE | Auth] Implement login page`
- `[BE | Exam] Fix exam deletion bug`

#### 2. Create a Branch

Branch from `main` for each feature:

**Branch Naming Convention:**

```
feature/<description>
fix/<description>
task/<description>
```

Examples:

- ✅ `feature/login-page`
- ✅ `fix/exam-deletion-bug`
- ✅ `task/update-readme`
- ❌ `feature-login` (missing `/`)
- ❌ `feature/login page` (no spaces)

#### 3. Commit Changes

**Commit Message Convention:**

```
<type>: <description>

Types: fix, feat, chore
```

Examples:

- ✅ `feat: add user authentication`
- ✅ `fix: resolve database connection issue`
- ✅ `chore: update dependencies`
- ❌ `added login` (no type prefix)
- ❌ `fix user bug` (missing colon)

**Skip Deployment:**  
Include `#skip` in commit message to skip CI/CD deployment.

#### 4. Create a Pull Request

**PR Title Convention:**

```
[FE | feature] Description
[BE | feature] Description
```

The `| feature` part is optional but recommended.

Examples:

- ✅ `[FE | Auth] Add login page`
- ✅ `[BE | Exam] Fix exam deletion`
- ✅ `[FE] Update dashboard layout`
- ❌ `Add login page` (missing prefix)

**PR Description:**  
Link the related issue using keywords:

```
- close, closes, closed
- fix, fixes, fixed
- resolve, resolves, resolved
```

Example:

```markdown
[FE | Auth] Add password reset functionality

- Implemented password reset form and API integration
- Updated authentication flow

closes #42
```

### Repository Rules

1. ✅ `main` is protected - only merge via Pull Requests
2. ✅ All PRs require at least 1 approval
3. ✅ Code must pass CodeQL and GitGuardian security checks
4. ✅ Follow naming conventions for branches, commits, and PRs
5. ✅ Link all PRs to an issue

## 📁 Project Structure

### Backend (Clean Architecture)

```
backend/
├── Services/
│   ├── User/               # User authentication & profiles
│   │   ├── User.API/       # HTTP endpoints
│   │   ├── User.Application/   # Use cases & business logic
│   │   ├── User.Domain/    # Core domain entities
│   │   └── User.Infrastructure/   # Data access & external services
│   ├── Class/              # Class management
│   │   ├── Class.API/
│   │   ├── Class.Application/
│   │   ├── Class.Domain/
│   │   └── Class.Infrastructure/
│   ├── Exam/               # Exam & assessment management
│   │   ├── Exam.API/
│   │   ├── Exam.Application/
│   │   ├── Exam.Domain/
│   │   └── Exam.Infrastructure/
│   └── Subscription/       # Subscription & billing
│       ├── Subscription.API/
│       ├── Subscription.Application/
│       ├── Subscription.Domain/
│       └── Subscription.Infrastructure/
└── Shared/
    └── Shared.Kernel/      # Common domain primitives & utilities
```

### Frontend (Feature-Based)

```
frontend/
├── src/
│   ├── pages/              # Route components
│   │   ├── auth/           # Authentication pages
│   │   ├── dashboard/      # Dashboard & analytics
│   │   ├── profile/        # User profile
│   │   └── assessment/     # Exam pages
│   ├── components/         # Reusable components
│   │   ├── ui/             # shadcn/ui components (built on Radix UI)
│   │   ├── common/         # Shared components
│   │   └── layout/         # Layout components
│   ├── hooks/              # Custom React hooks
│   ├── services/           # API clients
│   ├── stores/             # State management
│   ├── types/              # TypeScript types
│   └── utils/              # Helper functions
├── public/
│   └── languages/          # i18n translations
└── components.json         # UI component config
```

### Infrastructure

```
infra/
├── main.tf                 # Main Terraform configuration
├── provider.tf             # AWS provider setup
├── variables.tf            # Input variables
├── outputs.tf              # Output values
├── doppler.tf              # Doppler secrets integration
└── modules/                # Reusable Terraform modules
    ├── api-gateway/
    ├── cloudfront/
    ├── cognito/
    ├── ecr/
    ├── iam/
    ├── microservice/
    └── ses/
```

## 🔐 Authentication & Authorization

FrogEdu uses **AWS Cognito** for authentication with JWT tokens.

### Authentication Flow

1. User signs up/logs in through AWS Cognito
2. Cognito returns JWT access & refresh tokens
3. Frontend stores tokens securely
4. Each API request includes JWT in Authorization header
5. Services validate JWT and extract user claims

### Authorization

- Role-based access control (RBAC)
- Supported roles: Student, Teacher, Admin
- Each microservice validates permissions independently

## 🚢 Deployment

### Frontend Deployment

Frontend is deployed to **Cloudflare Pages**:

```bash
cd frontend
npm run build
# Deploys automatically via GitHub Actions
```

### Backend Deployment

Backend services are deployed to **AWS Lambda** via GitHub Actions:

1. Build .NET 9 application
2. Package as container image
3. Push to **AWS ECR** (Elastic Container Registry)
4. Deploy to AWS Lambda
5. Update **AWS API Gateway** routes

### Infrastructure Deployment

```bash
cd infra
doppler run -- terraform init
doppler run -- terraform plan
doppler run -- terraform apply
```

## 🛠️ Tech Highlights

### Frontend

- **React 19** with TypeScript for type safety
- **Vite** for fast development and optimized builds
- **TailwindCSS 4** for utility-first styling
- **shadcn/ui** for beautifully designed components built on Radix UI primitives
- **TanStack Query** for server state management and data fetching
- **Zustand** for lightweight client state management
- **React Router 7** for client-side routing
- **i18next** for internationalization (English & Vietnamese)
- **GSAP** for smooth animations

### Backend

- **.NET 9** with C# for high-performance APIs
- **Entity Framework Core 9** for database access
- **PostgreSQL 17** for relational data storage
- **Clean Architecture** for maintainable codebase
- **JWT Authentication** for secure API access
- **Swagger/OpenAPI** for API documentation
- **AWS Lambda** for serverless deployment
- **AWS API Gateway** for HTTP routing and management
- **AWS ECR** for container image storage

### DevOps

- **Docker & Docker Compose** for local development
- **GitHub Actions** for CI/CD pipelines
- **Terraform** for infrastructure as code
- **Doppler** for secrets management
- **CodeRabbit** for AI-powered code reviews
- **NamingConventionBot** for enforcing naming standards
- **AWS Services**:
  - **Lambda** for serverless compute
  - **API Gateway** for HTTP routing
  - **CloudFront** for CDN and content delivery
  - **Cognito** for authentication
  - **ECR** for container registry
  - **SES** for email services
  - **IAM** for access management
- **Cloudflare** for frontend hosting (Pages) and object storage (R2)

## 📚 Documentation

- [Specifications](specs/)
- [Authorization Guide](docs/authorization.md)
- [Microservices Details](docs/microservices-details.md)

---

<div align="center">

### 🙏 Thank You!

Built with ❤️ by the FrogEdu Team

</div>
