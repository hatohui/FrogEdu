# Feature 01: Authentication & User Management

**Status:** ✅ Complete  
**Priority:** P0 (Blocking)  
**Effort:** 16-20 hours

---

## 1. Overview

Handles user authentication (login, register), profile management, and role-based access control (RBAC).

**Services:** `User Service`, `Cognito`  
**Frontend:** `/auth/*`, `/profile`

---

## 2. User Stories

| ID  | As a... | I want to...                | So that...                              |
| --- | ------- | --------------------------- | --------------------------------------- |
| 1.1 | User    | Login with Email/Password   | I can access the platform.              |
| 1.2 | User    | Register as Teacher/Student | I can start using the correct features. |
| 1.3 | User    | Reset my password           | I can recover my account.               |
| 1.4 | User    | Update my profile & avatar  | My identity is accurate.                |

---

## 3. Technical Specifications

### 3.1 Domain Model (Backend)

**Aggregate Root:** `User`

- **Properties:** `CognitoId`, `Email`, `FullName`, `Role` (Enum), `AvatarUrl`.
- **Logic:** `UpdateProfile`, `VerifyEmail`, `UpdateAvatar`.

**Value Objects:**

- `CognitoUserId`: Wraps Cognito GUID.
- `Email`: Validates format.
- `FullName`: FirstName + LastName.

### 3.2 Database Schema (Neon: `frogedu-users`)

```sql
CREATE TABLE Users (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    CognitoId NVARCHAR(100) NOT NULL UNIQUE,
    Email NVARCHAR(200) NOT NULL,
    FirstName NVARCHAR(100),
    LastName NVARCHAR(100),
    Role INT NOT NULL, -- 0=Student, 1=Teacher
    AvatarUrl NVARCHAR(500),
    IsDeleted BIT DEFAULT 0,
    CreatedAt DATETIME2,
    UpdatedAt DATETIME2
);
```

### 3.3 Frontend Architecture

**Routes:**

- `/auth/login`
- `/auth/register`
- `/auth/forgot-password`
- `/profile` (Protected)

**State Management (Zustand):**

- `useAuthStore`: Stores `user` object, `token`, `isAuthenticated`.
- Actions: `login()`, `logout()`, `updateProfile()`.

---

## 4. Implementation Checklist

### 4.1 Backend (User Service)

- [x] **Domain Layer**:
  - [x] Implement `User` entity and Value Objects.
  - [x] Define `IUserRepository`.
- [x] **Infrastructure Layer**:
  - [x] Implement `UserRepository` (EF Core).
  - [x] Implement `CognitoIdentityService` (wrapper for AWS SDK).
  - [x] Configure `DbContext` and Migrations.
- [x] **Application Layer (CQRS)**:
  - [x] `RegisterUserCommand` (Sync with Cognito).
  - [x] `LoginUserCommand` (or handle on client, verify token on API).
  - [x] `UpdateUserProfileCommand`.
  - [x] `GetUserProfileQuery`.
- [x] **API Layer**:
  - [x] `POST /api/auth/webhook` (Cognito Post-Confirmation trigger).
  - [x] `GET /api/users/me`
  - [x] `PUT /api/users/me`
  - [x] `POST /api/users/me/avatar` (Presigned URL)

### 4.2 Frontend (Auth Feature)

- [x] **Components**:
  - [x] `LoginForm`: Zod validation, error handling.
  - [x] `RegisterForm`: Role selection, password strength.
  - [x] `ProfileForm`: Edit inputs, avatar upload.
  - [x] `AvatarUpload`: Image cropper/preview.
- [x] **Services / Hooks**:
  - [x] `auth.service.ts`: AWS Amplify/Cognito integration or direct API.
  - [x] `useLogin`, `useRegister` (TanStack Query mutations).
  - [x] `useAuthStore` (Zustand sessions).
- [x] **Pages**:
  - [x] Create Auth Layout (Center card).
  - [x] Implement Login/Register pages.
  - [x] Implement Profile page.

### 4.3 Integration & Security

- [x] Configure AWS Cognito User Pool triggers.
- [x] Set up JWT Bearer Authentication in .NET Program.cs.
- [x] Implement Protected Route wrapper (`<ProtectedRoute />`).
- [ ] Verify RLS (Row Level Security) or logic to ensure users only edit their own profile.

---

## 5. Acceptance Criteria

- [ ] User can register and receives verification email.
- [ ] Login returns valid JWT.
- [ ] Profile updates persist to DB.
- [ ] Avatar upload works via Presigned URL to R2.
