# FrogEdu Platform Architecture

**Version:** 1.0  
**Last Updated:** January 14, 2026  
**Status:** ✅ Active Architecture

---

## Table of Contents

1. [Overview](#overview)
2. [Architecture Principles](#architecture-principles)
3. [System Architecture Diagram](#system-architecture-diagram)
4. [Technology Stack](#technology-stack)
5. [Frontend Architecture](#frontend-architecture)
6. [Backend Architecture](#backend-architecture)
7. [Database Architecture](#database-architecture)
8. [Authentication & Authorization](#authentication--authorization)
9. [AI Services](#ai-services)
10. [Infrastructure & Deployment](#infrastructure--deployment)
11. [Security Architecture](#security-architecture)
12. [Cost Optimization](#cost-optimization)
13. [Scalability & Performance](#scalability--performance)
14. [Monitoring & Observability](#monitoring--observability)

---

## Overview

FrogEdu is a serverless, cloud-native educational platform built on modern web technologies with a focus on cost efficiency, scalability, and developer experience. The platform leverages fully managed services to minimize operational overhead while maximizing performance.

### Key Design Goals

- **Zero Infrastructure Management:** Fully serverless architecture with no servers to maintain
- **Cost Efficiency:** Optimized for free tiers and minimal operational costs
- **Global Scale:** CDN-first approach for worldwide accessibility
- **Developer Experience:** Modern tooling and rapid iteration cycles
- **Security First:** Enterprise-grade authentication and authorization

---

## Architecture Principles

### 1. **Serverless-First**

All compute resources are serverless (AWS Lambda, Cloudflare Pages Functions), eliminating server management and enabling automatic scaling.

### 2. **Managed Services**

Leverage fully managed services for databases (Neon Postgres), authentication (AWS Cognito), storage (Cloudflare R2), and CDN (CloudFront).

### 3. **API-Driven**

Clean separation between frontend and backend through well-defined REST APIs, enabling independent scaling and development.

### 4. **Edge Computing**

Static assets and cached responses served from edge locations worldwide for optimal performance.

### 5. **Security by Default**

Zero-trust architecture with JWT-based authentication, encrypted data in transit and at rest, and principle of least privilege IAM policies.

---

## System Architecture Diagram

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                             CLIENT LAYER                                     │
│  ┌──────────────────────────────────────────────────────────────────────┐  │
│  │  Web Browsers (Chrome, Firefox, Safari, Edge)                        │  │
│  │  Mobile Browsers (iOS Safari, Android Chrome)                        │  │
│  └──────────────────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────────────────┘
                                    ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│                          CLOUDFLARE NETWORK                                  │
│  ┌──────────────────────────────────────────────────────────────────────┐  │
│  │  Cloudflare Pages (Frontend Hosting)                                 │  │
│  │  • React SPA with Vite build                                         │  │
│  │  • Static assets (HTML, CSS, JS)                                     │  │
│  │  • Edge caching & distribution                                       │  │
│  │  • Automatic HTTPS                                                   │  │
│  └──────────────────────────────────────────────────────────────────────┘  │
│  ┌──────────────────────────────────────────────────────────────────────┐  │
│  │  Cloudflare R2 (Object Storage)                                      │  │
│  │  • User avatars                                                      │  │
│  │  • Course materials (PDFs, videos)                                   │  │
│  │  • Assessment attachments                                            │  │
│  │  • S3-compatible API                                                 │  │
│  └──────────────────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────────────────┘
                                    ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│                          AWS CLOUDFRONT (CDN)                                │
│  • Global edge caching for API responses                                    │
│  • Cache policies optimized for API Gateway                                 │
│  • HTTPS termination                                                        │
│  • DDoS protection (AWS Shield Standard)                                    │
└─────────────────────────────────────────────────────────────────────────────┘
                                    ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│                       AWS API GATEWAY (REST API)                             │
│  ┌──────────────────────────────────────────────────────────────────────┐  │
│  │  API Endpoints:                                                      │  │
│  │  • /auth/*         - Authentication operations                       │  │
│  │  • /users/*        - User profile management                         │  │
│  │  • /classes/*      - Class management                                │  │
│  │  • /content/*      - Content library                                 │  │
│  │  • /assessments/*  - Assessments & submissions                       │  │
│  │  • /ai/*           - AI tutor interactions                           │  │
│  └──────────────────────────────────────────────────────────────────────┘  │
│  • JWT token validation (Cognito authorizer)                                │
│  • Request/Response transformation                                          │
│  • Rate limiting & throttling                                               │
│  • API key management                                                       │
└─────────────────────────────────────────────────────────────────────────────┘
                                    ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│                        AWS LAMBDA FUNCTIONS                                  │
│  ┌──────────────────────────────────────────────────────────────────────┐  │
│  │  Microservices:                                                      │  │
│  │  ┌────────────────┐  ┌────────────────┐  ┌────────────────┐       │  │
│  │  │ User Service   │  │ Content Service│  │ Assessment Svc │       │  │
│  │  │ (.NET 9)       │  │ (.NET 9)       │  │ (.NET 9)       │       │  │
│  │  └────────────────┘  └────────────────┘  └────────────────┘       │  │
│  │  ┌────────────────┐                                                 │  │
│  │  │ AI Service     │  Each Lambda:                                   │  │
│  │  │ (.NET 9)       │  • Stateless execution                         │  │
│  │  └────────────────┘  • VPC integration for DB access              │  │
│  │                      • Environment-specific configs                 │  │
│  │                      • Connection pooling                           │  │
│  └──────────────────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────────────────┘
          │                    │                    │
          ▼                    ▼                    ▼
┌───────────────────┐  ┌──────────────────┐  ┌──────────────────────────────┐
│  AWS COGNITO      │  │  NEON POSTGRES   │  │  GOOGLE GEMINI AI            │
│                   │  │                  │  │                              │
│  • User Pool      │  │  • UserDB        │  │  • Content generation        │
│  • Identity Pool  │  │  • ContentDB     │  │  • Question generation       │
│  • JWT issuance   │  │  • AssessmentDB  │  │  • Answer evaluation         │
│  • MFA support    │  │  • AiContextDB   │  │  • Personalized tutoring     │
│  • Social login   │  │                  │  │  • Free tier (Flash model)   │
│  • Groups:        │  │  • Autoscaling   │  │                              │
│  │  - Teachers    │  │  • Branching     │  └──────────────────────────────┘
│  │  - Students    │  │  • Point-in-time │
│  │  - Admins      │  │    restore       │
│                   │  │  • Serverless    │
└───────────────────┘  └──────────────────┘
```

---

## Technology Stack

### Frontend Stack

| Component            | Technology            | Purpose                             |
| -------------------- | --------------------- | ----------------------------------- |
| **Framework**        | React 18 + TypeScript | UI component library                |
| **Build Tool**       | Vite 6                | Fast development & optimized builds |
| **Routing**          | React Router v6       | Client-side routing                 |
| **State Management** | Tanstack Query        | Server state management             |
| **UI Components**    | shadcn/ui             | Accessible component library        |
| **Styling**          | Tailwind CSS          | Utility-first CSS                   |
| **Animation**        | GSAP                  | High-performance animations         |
| **i18n**             | i18next               | Internationalization (EN/VI)        |
| **Hosting**          | Cloudflare Pages      | Edge-deployed static site           |
| **Storage**          | Cloudflare R2         | Object storage (S3-compatible)      |

### Backend Stack

| Component          | Technology              | Purpose                             |
| ------------------ | ----------------------- | ----------------------------------- |
| **Runtime**        | .NET 9 (C#)             | Application code                    |
| **Architecture**   | Clean Architecture      | Separation of concerns              |
| **Compute**        | AWS Lambda              | Serverless functions                |
| **API Gateway**    | AWS API Gateway         | RESTful API management              |
| **CDN**            | AWS CloudFront          | Global content delivery             |
| **Authentication** | AWS Cognito             | User authentication & authorization |
| **Database**       | Neon Postgres           | Serverless PostgreSQL               |
| **AI Provider**    | Google Gemini           | AI-powered features (free tier)     |
| **ORM**            | Entity Framework Core 9 | Database access                     |
| **Validation**     | FluentValidation        | Input validation                    |
| **Mediator**       | MediatR                 | CQRS pattern implementation         |

### Infrastructure & DevOps

| Component      | Technology          | Purpose                |
| -------------- | ------------------- | ---------------------- |
| **IaC**        | Terraform           | Infrastructure as Code |
| **CI/CD**      | GitHub Actions      | Automated deployments  |
| **Monitoring** | AWS CloudWatch      | Logs & metrics         |
| **Secrets**    | AWS Secrets Manager | Credential storage     |

---

## Frontend Architecture

### Cloudflare Pages Deployment

**Architecture Pattern:** Static Site Generation (SSG) + Edge Functions

#### Build Process

```bash
# Vite builds static assets
npm run build
# Output: dist/
#   ├── index.html
#   ├── assets/
#   │   ├── index-[hash].js
#   │   └── index-[hash].css
#   └── _redirects (for SPA routing)
```

#### Cloudflare Pages Configuration

```toml
# wrangler.toml (from src/)
name = "frogedu-frontend"
pages_build_output_dir = "dist"

[env.production]
name = "frogedu-frontend"
route = "frogedu.com/*"

[env.preview]
name = "frogedu-preview"
```

#### Deployment Pipeline

1. **Commit to `main` branch** → Triggers GitHub Actions
2. **Build with Vite** → Generates optimized static assets
3. **Deploy to Cloudflare Pages** → Distributed to 300+ edge locations
4. **DNS Update** → Custom domain routing

#### Performance Optimizations

- **Code Splitting:** Route-based lazy loading reduces initial bundle size
- **Tree Shaking:** Unused code eliminated at build time
- **Asset Compression:** Brotli compression for text assets
- **Image Optimization:** WebP format with fallbacks
- **Edge Caching:** Static assets cached at edge locations (Cache-Control: immutable)

### Cloudflare R2 Integration

**Purpose:** S3-compatible object storage for user-generated content

#### Use Cases

- User avatars (max 2MB, JPG/PNG)
- Course materials (PDFs, videos)
- Assessment attachments
- AI-generated content cache

#### Access Pattern

```typescript
// Frontend generates presigned URL request
const { presignedUrl } = await fetch("/api/storage/upload", {
  method: "POST",
  headers: { Authorization: `Bearer ${token}` },
  body: JSON.stringify({ fileName, contentType }),
});

// Frontend uploads directly to R2 (no Lambda proxy)
await fetch(presignedUrl, {
  method: "PUT",
  body: file,
  headers: { "Content-Type": contentType },
});
```

**Benefits:**

- Direct upload (no Lambda bandwidth costs)
- Automatic CDN integration
- No egress fees within Cloudflare network

---

## Backend Architecture

### Serverless Microservices Pattern

**Architecture Style:** Clean Architecture + CQRS + Domain Events

#### Lambda Function Structure

```
FrogEdu.{Service}.Lambda/
├── Function.cs                 # Lambda entry point
├── Startup.cs                  # DI configuration
├── appsettings.json            # Environment configs
└── Dependencies:
    ├── {Service}.Application   # Use cases (CQRS handlers)
    ├── {Service}.Domain        # Entities & value objects
    ├── {Service}.Infrastructure # Data access & external services
    └── Shared.Kernel           # Common abstractions
```

#### API Gateway → Lambda Integration

**Integration Type:** Lambda Proxy Integration

```json
// API Gateway passes full HTTP request to Lambda
{
  "httpMethod": "POST",
  "path": "/api/users/register",
  "headers": {
    "Authorization": "Bearer eyJhbGc...",
    "Content-Type": "application/json"
  },
  "body": "{\"email\":\"user@example.com\"}",
  "requestContext": {
    "authorizer": {
      "claims": {
        "sub": "cognito-user-id",
        "cognito:groups": "[\"Students\"]"
      }
    }
  }
}
```

**Lambda Response Format:**

```json
{
  "statusCode": 200,
  "headers": {
    "Content-Type": "application/json",
    "Access-Control-Allow-Origin": "*"
  },
  "body": "{\"id\":\"...\",\"email\":\"...\"}"
}
```

### CloudFront + API Gateway Architecture

**Purpose:** Cache API responses at edge locations for improved latency

#### Cache Behavior

| Path Pattern         | Origin      | Cache Policy       | TTL   |
| -------------------- | ----------- | ------------------ | ----- |
| `/api/content/*`     | API Gateway | Cache GET requests | 5 min |
| `/api/classes/*`     | API Gateway | Cache GET requests | 1 min |
| `/api/auth/*`        | API Gateway | No cache           | -     |
| `/api/assessments/*` | API Gateway | No cache           | -     |

**Cache Key Configuration:**

- Include `Authorization` header in cache key (user-specific responses)
- Include query string parameters
- Exclude cookies (stateless API)

**Cost Benefits:**

- Reduces Lambda invocations (free tier: 1M requests/month)
- Reduces API Gateway requests (free tier: 1M requests/month)
- Flat-rate CloudFront pricing (~$0.085/GB after free tier)

---

## Database Architecture

### Neon Postgres (Serverless)

**Why Neon:**

- **Serverless Scaling:** Auto-scale to zero when idle (free tier: 0.5 GB storage, 1 shared vCPU)
- **Branching:** Database branching for dev/staging environments (free tier: 10 branches)
- **Point-in-Time Restore:** Built-in backup & restore
- **Connection Pooling:** Built-in pooling (no RDS Proxy needed)
- **Instant Provisioning:** Database ready in <5 seconds

#### Database Separation

```
┌─────────────────────────────────────────────────────────────────┐
│  Neon Project: frogedu-production                               │
│                                                                   │
│  ┌────────────────┐  ┌────────────────┐  ┌─────────────────┐  │
│  │  UserDB        │  │  ContentDB     │  │  AssessmentDB   │  │
│  │  • Users       │  │  • Materials   │  │  • Assessments  │  │
│  │  • Profiles    │  │  • Lessons     │  │  • Submissions  │  │
│  │  • Classes     │  │  • Assignments │  │  • Grades       │  │
│  └────────────────┘  └────────────────┘  └─────────────────┘  │
│  ┌────────────────┐                                             │
│  │  AiContextDB   │                                             │
│  │  • Conversations                                             │
│  │  • Embeddings  │                                             │
│  │  • AI Metadata │                                             │
│  └────────────────┘                                             │
└─────────────────────────────────────────────────────────────────┘
```

**Rationale for Multiple Databases:**

1. **Service Isolation:** Each microservice owns its data
2. **Independent Scaling:** Scale databases based on specific load
3. **Deployment Independence:** Schema migrations don't block other services
4. **Security:** Principle of least privilege (service-specific credentials)

#### Lambda → Neon Connection Pattern

**Challenge:** Lambda's ephemeral nature can exhaust database connections

**Solution:** Neon's built-in connection pooling + Lambda best practices

```csharp
// Singleton DbContext per Lambda container
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Connection string from environment variable
        var connectionString = Environment.GetEnvironmentVariable("NEON_CONNECTION_STRING");

        // DbContext with connection pooling
        services.AddDbContext<UserDbContext>(options =>
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                // Enable connection pooling
                npgsqlOptions.EnableRetryOnFailure(maxRetryCount: 3);
                npgsqlOptions.CommandTimeout(30);
            }));

        // Singleton for Lambda container reuse
        services.AddSingleton<UserDbContext>(sp =>
            sp.GetRequiredService<IDbContextFactory<UserDbContext>>().CreateDbContext());
    }
}
```

**Neon Connection String:**

```
postgresql://user:password@ep-project-id.region.aws.neon.tech/dbname?sslmode=require&pooler=true
```

- `pooler=true`: Uses Neon's connection pooling (PgBouncer-based)
- `sslmode=require`: Encrypted connections

**Connection Limits:**

- Free tier: 100 concurrent connections
- Lambda typically reuses 1-5 connections per function instance

---

## Authentication & Authorization

### AWS Cognito User Pool

**Design Decision:** All authentication logic handled by Cognito (no local User.API service)

#### Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│  AWS Cognito User Pool: frogedu-users                           │
│                                                                   │
│  User Attributes:                                                │
│  • email (required, verified)                                    │
│  • given_name, family_name                                       │
│  • custom:role (Teacher | Student)                               │
│  • picture (avatar URL)                                          │
│                                                                   │
│  User Groups:                                                    │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐         │
│  │  Teachers    │  │  Students    │  │  Admins      │         │
│  │  (IAM role)  │  │  (IAM role)  │  │  (IAM role)  │         │
│  └──────────────┘  └──────────────┘  └──────────────┘         │
│                                                                   │
│  Password Policy:                                                │
│  • Min 8 characters                                              │
│  • Uppercase + lowercase + number                                │
│  • MFA optional (TOTP)                                           │
│                                                                   │
│  Token Configuration:                                            │
│  • Access token: 15 minutes                                      │
│  • ID token: 15 minutes                                          │
│  • Refresh token: 30 days                                        │
└─────────────────────────────────────────────────────────────────┘
```

#### Authentication Flows

**1. User Registration**

```
Frontend                 Cognito                 Lambda (User Service)
   │                        │                            │
   │ 1. SignUp(email, pw)   │                            │
   ├───────────────────────>│                            │
   │                        │ 2. Create user             │
   │                        │    (unverified)            │
   │ 3. ConfirmationCode    │                            │
   │<───────────────────────┤                            │
   │                        │                            │
   │ 4. ConfirmSignUp(code) │                            │
   ├───────────────────────>│                            │
   │                        │ 5. Mark verified           │
   │ 6. Success             │                            │
   │<───────────────────────┤                            │
   │                        │                            │
   │ 7. POST /api/users/sync (JWT token)                │
   ├────────────────────────────────────────────────────>│
   │                        │    8. Extract Cognito ID   │
   │                        │       from JWT             │
   │                        │    9. Create local profile │
   │                        │       (UserDB record)      │
   │ 10. UserDto            │                            │
   │<────────────────────────────────────────────────────┤
```

**2. User Login**

```
Frontend                 Cognito                 API Gateway
   │                        │                            │
   │ 1. InitiateAuth        │                            │
   │    (email, password)   │                            │
   ├───────────────────────>│                            │
   │                        │ 2. Verify credentials      │
   │                        │    (bcrypt)                │
   │ 3. JWT Tokens          │                            │
   │    (access, id, refresh)                            │
   │<───────────────────────┤                            │
   │                        │                            │
   │ 4. Store in memory     │                            │
   │                        │                            │
   │ 5. API Request         │                            │
   │    (Authorization: Bearer <access_token>)           │
   ├─────────────────────────────────────────────────────>│
   │                        │    6. Validate JWT         │
   │                        │       (signature, expiry)  │
   │                        │    7. Extract claims       │
   │                        │       (sub, cognito:groups)│
   │ 8. API Response        │                            │
   │<─────────────────────────────────────────────────────┤
```

#### API Gateway Cognito Authorizer

**Configuration:**

```json
{
  "type": "JWT",
  "identitySource": "$request.header.Authorization",
  "issuerUrl": "https://cognito-idp.ap-southeast-1.amazonaws.com/{userPoolId}",
  "audience": ["{appClientId}"]
}
```

**Authorization Flow:**

1. Client sends request with `Authorization: Bearer <token>`
2. API Gateway validates JWT signature using Cognito's JWKS
3. Checks token expiration (`exp` claim)
4. Verifies audience (`aud` claim matches app client ID)
5. Extracts claims and passes to Lambda in `event.requestContext.authorizer.claims`

**No Lambda invocation if token is invalid** → Saves compute costs

#### Role-Based Access Control (RBAC)

**Implementation in Lambda:**

```csharp
// Extract user role from JWT claims
var userGroups = context.Request.Headers["Authorization"]
    .ExtractClaims()["cognito:groups"]; // ["Teachers"] or ["Students"]

// Authorization logic
if (userGroups.Contains("Teachers"))
{
    // Allow class creation
}
else if (userGroups.Contains("Students"))
{
    // Allow class enrollment only
}
```

**Benefits:**

- No database lookup for permissions (embedded in JWT)
- Instant authorization decisions
- Group membership managed in Cognito

---

## AI Services

### Google Gemini Integration

**Model:** Gemini 2.0 Flash (Free Tier)

**Free Tier Limits:**

- 1,500 requests per day
- 1 million tokens per day
- 10 requests per minute

#### Architecture Pattern

```
Lambda (AI Service)          Gemini API          Database (AiContextDB)
        │                        │                        │
        │ 1. Generate question   │                        │
        ├───────────────────────>│                        │
        │                        │ 2. AI response         │
        │ 3. Response            │                        │
        │<───────────────────────┤                        │
        │                        │                        │
        │ 4. Store conversation  │                        │
        │    context & embeddings│                        │
        ├────────────────────────────────────────────────>│
        │                        │                        │
        │ 5. Retrieve history    │                        │
        │    for context         │                        │
        │<────────────────────────────────────────────────┤
```

#### Use Cases

1. **Content Generation:** Generate quiz questions from course materials
2. **Answer Evaluation:** Grade open-ended student responses
3. **Personalized Tutoring:** Adaptive learning recommendations
4. **Summarization:** Summarize long documents for students

#### Rate Limiting Strategy

```csharp
// Distributed rate limiting with DynamoDB
public class GeminiRateLimiter
{
    private readonly IAmazonDynamoDB _dynamoDb;

    public async Task<bool> AllowRequestAsync(string userId)
    {
        var key = $"gemini:{DateTime.UtcNow:yyyy-MM-dd}:{userId}";
        var count = await _dynamoDb.GetItemAsync(key);

        if (count >= 100) // Per-user daily limit
            return false;

        await _dynamoDb.IncrementAsync(key, ttl: 86400);
        return true;
    }
}
```

---

## Infrastructure & Deployment

### Terraform Configuration

**Structure:**

```
infra/
├── provider.tf           # AWS & Cloudflare providers
├── terraform.tf          # Backend configuration (S3 state)
├── variables.tf          # Input variables
├── main.tf               # Main resources
├── modules/
│   ├── lambda/           # Lambda function module
│   ├── api-gateway/      # API Gateway module
│   ├── cognito/          # Cognito User Pool module
│   └── cloudfront/       # CloudFront distribution module
└── environments/
    ├── dev.tfvars
    ├── staging.tfvars
    └── production.tfvars
```

### CI/CD Pipeline (GitHub Actions)

**Frontend Pipeline:**

```yaml
name: Deploy Frontend

on:
  push:
    branches: [main]
    paths: ["frontend/**"]

jobs:
  deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with:
          node-version: "20"
      - run: npm ci
      - run: npm run build
      - uses: cloudflare/wrangler-action@v3
        with:
          apiToken: ${{ secrets.CLOUDFLARE_API_TOKEN }}
          command: pages deploy dist --project-name=frogedu
```

**Backend Pipeline:**

```yaml
name: Deploy Backend

on:
  push:
    branches: [main]
    paths: ["backend/**"]

jobs:
  deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with:
          dotnet-version: "9.0.x"
      - run: dotnet build
      - run: dotnet test
      - run: dotnet lambda package
      - uses: aws-actions/configure-aws-credentials@v4
      - run: terraform apply -auto-approve
```

---

## Security Architecture

### Data Encryption

**In Transit:**

- All HTTP traffic over TLS 1.3 (HTTPS only)
- Cloudflare Universal SSL certificates
- AWS Certificate Manager for API Gateway

**At Rest:**

- Neon Postgres: AES-256 encryption at rest
- Cloudflare R2: Server-side encryption
- AWS Secrets Manager: Encrypted credential storage

### Secret Management

**Environment Variables (Lambda):**

```bash
# Stored in AWS Secrets Manager
NEON_CONNECTION_STRING=postgresql://...
GEMINI_API_KEY=AIza...
JWT_SECRET=<generated>
```

**Access Pattern:**

```csharp
// Lambda retrieves secrets at startup
var secretsManager = new AmazonSecretsManagerClient();
var response = await secretsManager.GetSecretValueAsync(new GetSecretValueRequest
{
    SecretId = "frogedu/production/database"
});
```

### IAM Least Privilege

**Lambda Execution Role:**

```json
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Action": [
        "logs:CreateLogGroup",
        "logs:CreateLogStream",
        "logs:PutLogEvents"
      ],
      "Resource": "arn:aws:logs:*:*:*"
    },
    {
      "Effect": "Allow",
      "Action": ["secretsmanager:GetSecretValue"],
      "Resource": "arn:aws:secretsmanager:ap-southeast-1:*:secret:frogedu/*"
    }
  ]
}
```

---

## Cost Optimization

### Monthly Cost Breakdown (Estimated)

**Free Tier (First 12 Months):**
| Service | Free Tier | Expected Usage | Estimated Cost |
|---------|-----------|----------------|----------------|
| **Cloudflare Pages** | Unlimited requests | ~500K/month | $0 |
| **Cloudflare R2** | 10 GB storage, 1M Class A, 10M Class B | 5 GB, 100K requests | $0 |
| **AWS Lambda** | 1M requests, 400K GB-sec | 200K requests | $0 |
| **API Gateway** | 1M requests | 200K requests | $0 |
| **CloudFront** | 1 TB data transfer | 100 GB | $0 |
| **Cognito** | 50K MAUs | 500 users | $0 |
| **Neon Postgres** | 0.5 GB storage | 0.3 GB | $0 |
| **Gemini AI** | 1,500 req/day | 1,000 req/day | $0 |
| **CloudWatch Logs** | 5 GB logs | 2 GB | $0 |

**Total Monthly Cost:** $0 (within free tiers)

**After Free Tier (Month 13+):**

- Lambda: ~$5/month (200K requests × 512MB × 3s avg)
- Neon: $0 (free tier is perpetual)
- Cloudflare: $0 (free tier is perpetual)
- Total: ~$5-10/month for 500-1000 active users

---

## Scalability & Performance

### Performance Targets

| Metric                        | Target | Current         |
| ----------------------------- | ------ | --------------- |
| **Page Load Time (FCP)**      | <1.5s  | ~800ms          |
| **API Response Time (P95)**   | <500ms | ~200ms          |
| **Lambda Cold Start**         | <1s    | ~600ms (.NET 9) |
| **Database Query Time (P95)** | <100ms | ~50ms           |
| **CDN Cache Hit Rate**        | >80%   | ~85%            |

### Auto-Scaling Configuration

**Lambda:**

- Concurrency: Auto-scale up to 1000 concurrent executions
- Provisioned Concurrency: 5 warm instances for User Service (critical path)

**Neon:**

- Auto-scale compute: 0.25 - 4 vCPU
- Scale to zero: 5 minutes of inactivity

**CloudFront:**

- Automatic edge scaling (no configuration needed)

---

## Monitoring & Observability

### CloudWatch Metrics

**Lambda Metrics:**

- Invocations
- Duration (P50, P95, P99)
- Errors
- Throttles
- Concurrent executions

**API Gateway Metrics:**

- 4XXError, 5XXError
- Count (requests)
- Latency (P50, P95, P99)

**Custom Application Metrics:**

```csharp
// .NET 9 with CloudWatch SDK
var cloudWatch = new AmazonCloudWatchClient();
await cloudWatch.PutMetricDataAsync(new PutMetricDataRequest
{
    Namespace = "FrogEdu/UserService",
    MetricData = new List<MetricDatum>
    {
        new MetricDatum
        {
            MetricName = "UserRegistrations",
            Value = 1,
            Unit = StandardUnit.Count,
            Timestamp = DateTime.UtcNow
        }
    }
});
```

### Logging Strategy

**Structured Logging:**

```csharp
_logger.LogInformation("User registered successfully. UserId={UserId}, Email={Email}",
    user.Id, user.Email.Value);
```

**CloudWatch Log Insights Queries:**

```
fields @timestamp, @message
| filter @message like /ERROR/
| stats count() by bin(5m)
```

---

## Disaster Recovery

### Backup Strategy

**Database (Neon):**

- Automatic point-in-time restore (7 days retention)
- Daily snapshots (30 days retention)

**Object Storage (R2):**

- Versioning enabled
- Lifecycle policy: Delete after 90 days

**RTO/RPO:**

- Recovery Time Objective (RTO): <1 hour
- Recovery Point Objective (RPO): <15 minutes

---

## Compliance & Data Privacy

**GDPR Compliance:**

- User data deletion API (`DELETE /api/users/{id}`)
- Data export API (`GET /api/users/{id}/export`)
- Consent management (stored in Cognito custom attributes)

**Data Residency:**

- AWS Region: `ap-southeast-1` (Singapore)
- Cloudflare: Automatic geo-routing (respects data sovereignty)

---

## Future Enhancements

### Phase 2 (Q2 2026)

- [ ] Real-time collaboration (WebSockets via API Gateway WebSocket API)
- [ ] Video streaming (Cloudflare Stream)
- [ ] Mobile app (React Native)
- [ ] Advanced analytics (ClickHouse)

### Phase 3 (Q3 2026)

- [ ] Multi-tenancy (organization accounts)
- [ ] Marketplace (third-party integrations)
- [ ] Advanced AI (fine-tuned models)

---

**Document Maintained By:** FrogEdu Engineering Team  
**Last Reviewed:** January 14, 2026  
**Next Review:** April 14, 2026
