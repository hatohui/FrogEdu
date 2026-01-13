# Authentication & User Management Feature

**Feature:** Authentication & User Profile Management  
**Service:** User Service (Backend)  
**Version:** 1.0  
**Status:** Specification Phase

---

## Table of Contents

1. [Overview](#overview)
2. [User Stories](#user-stories)
3. [UI/UX Requirements](#uiux-requirements)
4. [Component Architecture](#component-architecture)
5. [API Integration](#api-integration)
6. [State Management](#state-management)
7. [Implementation Tasks](#implementation-tasks)
8. [Acceptance Criteria](#acceptance-criteria)

---

## Overview

This feature handles user authentication (login, registration, password reset) using **AWS Cognito** and manages user profiles (view, edit, avatar upload).

### Responsibilities

- ✅ User login with email/password
- ✅ User registration with email verification
- ✅ Password reset flow
- ✅ JWT token management (access + refresh)
- ✅ User profile view and edit
- ✅ Avatar upload to S3
- ✅ Role-based routing (Teacher vs Student)
- ✅ Protected route guards

---

## User Stories

### Story 1.1: User Login

**As a** Teacher or Student  
**I want to** log in with my email and password  
**So that** I can access my personalized dashboard

**Acceptance Criteria:**

- [ ] User can enter email and password
- [ ] Form validates email format and password length
- [ ] Loading state shown during authentication
- [ ] Success: Redirect to appropriate dashboard based on role
- [ ] Error: Show clear error message (invalid credentials, account not verified, etc.)
- [ ] "Remember me" option saves email (not password)
- [ ] "Forgot password" link navigates to reset flow

### Story 1.2: User Registration

**As a** new user  
**I want to** create an account with my email  
**So that** I can access the platform

**Acceptance Criteria:**

- [ ] User can select role (Teacher or Student)
- [ ] Form collects: email, password, full name, role
- [ ] Password strength indicator shows requirements
- [ ] Email verification code sent via AWS Cognito
- [ ] Verification page allows code entry
- [ ] Success: Auto-login and redirect to onboarding
- [ ] Error: Handle duplicate email, weak password

### Story 1.3: Password Reset

**As a** user who forgot their password  
**I want to** reset it via email  
**So that** I can regain access to my account

**Acceptance Criteria:**

- [ ] User enters email address
- [ ] Reset code sent to email
- [ ] User enters verification code + new password
- [ ] Password updated successfully
- [ ] User redirected to login page

### Story 1.4: User Profile

**As a** logged-in user  
**I want to** view and edit my profile  
**So that** my information is accurate

**Acceptance Criteria:**

- [ ] Display: Avatar, Name, Email, Role, Bio
- [ ] User can edit: Name, Bio, Avatar
- [ ] Avatar upload to S3 (max 5MB, jpg/png only)
- [ ] Changes saved and immediately reflected
- [ ] Email cannot be changed (requires re-verification)

---

## UI/UX Requirements

### Page: Login (`/login`)

**Layout:**

```
┌─────────────────────────────────┐
│                                 │
│         [App Logo]              │
│                                 │
│    Welcome Back to FrogEdu      │
│                                 │
│  ┌───────────────────────────┐ │
│  │ Email                     │ │
│  │ [input field]             │ │
│  └───────────────────────────┘ │
│                                 │
│  ┌───────────────────────────┐ │
│  │ Password                  │ │
│  │ [input field]             │ │
│  │         [eye icon toggle] │ │
│  └───────────────────────────┘ │
│                                 │
│  [ ] Remember me                │
│           [Forgot password?]    │
│                                 │
│  [Login Button - Full Width]   │
│                                 │
│  Don't have an account?         │
│         [Sign up]               │
└─────────────────────────────────┘
```

**Styling:**

- Centered card (max-width: 400px)
- Soft shadows
- Primary color: Brand color from theme
- Focus states on inputs
- Smooth transitions

**Behavior:**

- Enter key submits form
- Disable submit button during loading
- Show spinner inside button during loading
- Auto-focus email input on mount

### Page: Register (`/register`)

**Additional Fields:**

- Full Name input
- Role selector (Radio buttons: Teacher / Student)
- Password confirmation field
- Terms & conditions checkbox

**Validation:**

- Email: Valid format
- Password: Min 8 chars, 1 uppercase, 1 number, 1 special char
- Passwords must match
- Terms must be accepted

### Page: Profile (`/profile`)

**Layout:**

```
┌────────────────────────────────────┐
│  Profile Settings                  │
├────────────────────────────────────┤
│                                    │
│  ┌──────────┐                      │
│  │  Avatar  │  [Upload Button]     │
│  │  Image   │                      │
│  └──────────┘                      │
│                                    │
│  Full Name: [input]                │
│  Email: user@example.com (locked)  │
│  Role: Teacher (locked)            │
│  Bio: [textarea]                   │
│                                    │
│  [Cancel]  [Save Changes]          │
│                                    │
└────────────────────────────────────┘
```

---

## Component Architecture

### Component Tree

```
pages/
├── (auth)/
│   ├── layout.tsx                 # Auth layout (centered, no sidebar)
│   ├── login/
│   │   └── page.tsx              # LoginPage (container)
│   ├── register/
│   │   └── page.tsx              # RegisterPage (container)
│   ├── forgot-password/
│   │   └── page.tsx              # ForgotPasswordPage
│   └── verify-email/
│       └── page.tsx              # VerifyEmailPage

features/auth/
├── components/
│   ├── LoginForm.tsx             # Presentational: Login form UI
│   ├── RegisterForm.tsx          # Presentational: Registration form UI
│   ├── PasswordResetForm.tsx     # Presentational: Reset form UI
│   ├── PasswordStrengthMeter.tsx # Shows password strength
│   └── RoleSelector.tsx          # Radio group for role selection
├── hooks/
│   ├── useAuth.tsx               # Auth context & hooks
│   ├── useLogin.ts               # Login mutation
│   ├── useRegister.ts            # Registration mutation
│   └── useProfile.ts             # Profile queries/mutations
├── services/
│   └── auth.service.ts           # API calls to User Service
├── types/
│   └── auth.types.ts             # TypeScript interfaces
└── utils/
    └── validation.ts             # Form validation schemas (Zod)
```

### Component Implementation

#### 1. LoginForm Component (Presentational)

```typescript
// features/auth/components/LoginForm.tsx

import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Checkbox } from "@/components/ui/checkbox";

const loginSchema = z.object({
  email: z.string().email("Invalid email address"),
  password: z.string().min(1, "Password is required"),
  rememberMe: z.boolean().optional(),
});

type LoginFormData = z.infer<typeof loginSchema>;

interface LoginFormProps {
  onSubmit: (data: LoginFormData) => void;
  isLoading: boolean;
  error?: string;
}

export function LoginForm({ onSubmit, isLoading, error }: LoginFormProps) {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<LoginFormData>({
    resolver: zodResolver(loginSchema),
  });

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
      <div className="space-y-2">
        <Label htmlFor="email">Email</Label>
        <Input
          id="email"
          type="email"
          placeholder="your@email.com"
          autoFocus
          {...register("email")}
        />
        {errors.email && (
          <p className="text-sm text-destructive">{errors.email.message}</p>
        )}
      </div>

      <div className="space-y-2">
        <Label htmlFor="password">Password</Label>
        <Input
          id="password"
          type="password"
          placeholder="Enter your password"
          {...register("password")}
        />
        {errors.password && (
          <p className="text-sm text-destructive">{errors.password.message}</p>
        )}
      </div>

      {error && (
        <div className="rounded-md bg-destructive/10 p-3">
          <p className="text-sm text-destructive">{error}</p>
        </div>
      )}

      <div className="flex items-center justify-between">
        <div className="flex items-center space-x-2">
          <Checkbox id="rememberMe" {...register("rememberMe")} />
          <Label htmlFor="rememberMe" className="cursor-pointer">
            Remember me
          </Label>
        </div>
        <a
          href="/forgot-password"
          className="text-sm text-primary hover:underline"
        >
          Forgot password?
        </a>
      </div>

      <Button type="submit" className="w-full" disabled={isLoading}>
        {isLoading ? "Logging in..." : "Login"}
      </Button>

      <p className="text-center text-sm text-muted-foreground">
        Don't have an account?{" "}
        <a href="/register" className="text-primary hover:underline">
          Sign up
        </a>
      </p>
    </form>
  );
}
```

#### 2. LoginPage (Container)

```typescript
// pages/(auth)/login/page.tsx

import { useNavigate } from "react-router-dom";
import { LoginForm } from "@/features/auth/components/LoginForm";
import { useLogin } from "@/features/auth/hooks/useLogin";
import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card";
import { useAuth } from "@/features/auth/hooks/useAuth";
import { useEffect } from "react";

export default function LoginPage() {
  const navigate = useNavigate();
  const { user } = useAuth();
  const { mutate: login, isPending, error } = useLogin();

  // Redirect if already logged in
  useEffect(() => {
    if (user) {
      const redirectPath = user.role === "Teacher" ? "/dashboard" : "/tutor";
      navigate(redirectPath);
    }
  }, [user, navigate]);

  const handleLogin = (data: LoginFormData) => {
    login(data, {
      onSuccess: (response) => {
        const redirectPath =
          response.role === "Teacher" ? "/dashboard" : "/tutor";
        navigate(redirectPath);
      },
    });
  };

  return (
    <div className="flex min-h-screen items-center justify-center p-4">
      <Card className="w-full max-w-md">
        <CardHeader className="text-center">
          <div className="mb-4 flex justify-center">
            {/* Logo here */}
            <img src="/logo.svg" alt="FrogEdu" className="h-12" />
          </div>
          <CardTitle className="text-2xl">Welcome Back to FrogEdu</CardTitle>
        </CardHeader>
        <CardContent>
          <LoginForm
            onSubmit={handleLogin}
            isLoading={isPending}
            error={error?.message}
          />
        </CardContent>
      </Card>
    </div>
  );
}
```

#### 3. useLogin Hook

```typescript
// features/auth/hooks/useLogin.ts

import { useMutation } from "@tanstack/react-query";
import { authService } from "../services/auth.service";
import { useAuth } from "./useAuth";
import type { LoginRequest, LoginResponse } from "../types/auth.types";

export function useLogin() {
  const { setUser, setTokens } = useAuth();

  return useMutation({
    mutationFn: (data: LoginRequest) => authService.login(data),
    onSuccess: (response: LoginResponse) => {
      // Store tokens
      setTokens(response.accessToken, response.refreshToken);

      // Store user info
      setUser({
        id: response.userId,
        email: response.email,
        fullName: response.fullName,
        role: response.role,
        avatarUrl: response.avatarUrl,
      });
    },
  });
}
```

#### 4. Auth Service

```typescript
// features/auth/services/auth.service.ts

import api from "@/services/axios";
import type {
  LoginRequest,
  LoginResponse,
  RegisterRequest,
  RegisterResponse,
  RefreshTokenRequest,
  RefreshTokenResponse,
} from "../types/auth.types";

export const authService = {
  login: async (data: LoginRequest): Promise<LoginResponse> => {
    const response = await api.post<LoginResponse>(
      "/api/user/auth/login",
      data
    );
    return response.data;
  },

  register: async (data: RegisterRequest): Promise<RegisterResponse> => {
    const response = await api.post<RegisterResponse>(
      "/api/user/auth/register",
      data
    );
    return response.data;
  },

  refreshToken: async (
    data: RefreshTokenRequest
  ): Promise<RefreshTokenResponse> => {
    const response = await api.post<RefreshTokenResponse>(
      "/api/user/auth/refresh",
      data
    );
    return response.data;
  },

  logout: async (): Promise<void> => {
    await api.post("/api/user/auth/logout");
    // Clear local storage
    localStorage.removeItem("accessToken");
    localStorage.removeItem("refreshToken");
    localStorage.removeItem("user");
  },

  verifyEmail: async (email: string, code: string): Promise<void> => {
    await api.post("/api/user/auth/verify-email", { email, code });
  },

  requestPasswordReset: async (email: string): Promise<void> => {
    await api.post("/api/user/auth/forgot-password", { email });
  },

  resetPassword: async (
    email: string,
    code: string,
    newPassword: string
  ): Promise<void> => {
    await api.post("/api/user/auth/reset-password", {
      email,
      code,
      newPassword,
    });
  },
};
```

---

## API Integration

### Endpoints (User Service)

| Method | Endpoint                         | Description               |
| ------ | -------------------------------- | ------------------------- |
| POST   | `/api/user/auth/login`           | Login with email/password |
| POST   | `/api/user/auth/register`        | Create new account        |
| POST   | `/api/user/auth/logout`          | Logout current user       |
| POST   | `/api/user/auth/refresh`         | Refresh access token      |
| POST   | `/api/user/auth/verify-email`    | Verify email with code    |
| POST   | `/api/user/auth/forgot-password` | Request password reset    |
| POST   | `/api/user/auth/reset-password`  | Reset password with code  |
| GET    | `/api/user/profile`              | Get current user profile  |
| PUT    | `/api/user/profile`              | Update user profile       |
| POST   | `/api/user/profile/avatar`       | Upload avatar (multipart) |

### Request/Response Types

```typescript
// features/auth/types/auth.types.ts

export interface LoginRequest {
  email: string;
  password: string;
  rememberMe?: boolean;
}

export interface LoginResponse {
  accessToken: string;
  refreshToken: string;
  expiresIn: number; // seconds
  userId: string;
  email: string;
  fullName: string;
  role: "Teacher" | "Student";
  avatarUrl?: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
  fullName: string;
  role: "Teacher" | "Student";
}

export interface RegisterResponse {
  userId: string;
  email: string;
  message: string; // "Verification email sent"
}

export interface User {
  id: string;
  email: string;
  fullName: string;
  role: "Teacher" | "Student";
  avatarUrl?: string;
  bio?: string;
}

export interface UpdateProfileRequest {
  fullName?: string;
  bio?: string;
}
```

---

## State Management

### Auth Context

```typescript
// features/auth/hooks/useAuth.tsx

import {
  createContext,
  useContext,
  useState,
  useEffect,
  ReactNode,
} from "react";
import type { User } from "../types/auth.types";

interface AuthContextValue {
  user: User | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  setUser: (user: User | null) => void;
  setTokens: (accessToken: string, refreshToken: string) => void;
  logout: () => void;
}

const AuthContext = createContext<AuthContextValue | undefined>(undefined);

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<User | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  // Restore user from localStorage on mount
  useEffect(() => {
    const storedUser = localStorage.getItem("user");
    if (storedUser) {
      setUser(JSON.parse(storedUser));
    }
    setIsLoading(false);
  }, []);

  const setTokens = (accessToken: string, refreshToken: string) => {
    localStorage.setItem("accessToken", accessToken);
    localStorage.setItem("refreshToken", refreshToken);
  };

  const handleSetUser = (newUser: User | null) => {
    setUser(newUser);
    if (newUser) {
      localStorage.setItem("user", JSON.stringify(newUser));
    } else {
      localStorage.removeItem("user");
    }
  };

  const logout = () => {
    localStorage.removeItem("accessToken");
    localStorage.removeItem("refreshToken");
    localStorage.removeItem("user");
    setUser(null);
  };

  return (
    <AuthContext.Provider
      value={{
        user,
        isAuthenticated: !!user,
        isLoading,
        setUser: handleSetUser,
        setTokens,
        logout,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within AuthProvider");
  }
  return context;
}
```

### Protected Routes

```typescript
// components/common/ProtectedRoute.tsx

import { Navigate } from "react-router-dom";
import { useAuth } from "@/features/auth/hooks/useAuth";
import { LoadingSpinner } from "./LoadingSpinner";

interface ProtectedRouteProps {
  children: React.ReactNode;
  requiredRole?: "Teacher" | "Student";
}

export function ProtectedRoute({
  children,
  requiredRole,
}: ProtectedRouteProps) {
  const { user, isLoading, isAuthenticated } = useAuth();

  if (isLoading) {
    return <LoadingSpinner fullScreen />;
  }

  if (!isAuthenticated) {
    return <Navigate to="/login" replace />;
  }

  if (requiredRole && user?.role !== requiredRole) {
    return <Navigate to="/unauthorized" replace />;
  }

  return <>{children}</>;
}
```

---

## Implementation Tasks

### Milestone 1: Authentication Infrastructure

#### Task 1.1: Setup Auth Context - [ ]

- [ ] Create `AuthProvider` component with Context
- [ ] Implement `useAuth` hook
- [ ] Add user state management
- [ ] Add token storage (localStorage)
- [ ] Wrap App with `AuthProvider` in main.tsx

**Validation:**

- [ ] Context accessible in all components
- [ ] User persists across page refreshes
- [ ] TypeScript types are correct

#### Task 1.2: Configure Axios Interceptors - [ ]

- [ ] Create Axios instance in `services/axios.ts`
- [ ] Add request interceptor to attach JWT token
- [ ] Add response interceptor for 401 errors
- [ ] Implement token refresh logic
- [ ] Handle token refresh failure (logout)

**Validation:**

- [ ] All API calls include `Authorization` header
- [ ] Token refresh works automatically
- [ ] User logged out on refresh failure

#### Task 1.3: Create Auth Types - [ ]

- [ ] Define all TypeScript interfaces in `auth.types.ts`
- [ ] Export types for use in components
- [ ] Ensure types match backend DTOs

### Milestone 2: Login Feature

#### Task 2.1: Install Required Shadcn Components - [ ]

- [ ] Install Button component via Shadcn MCP
- [ ] Install Input component
- [ ] Install Label component
- [ ] Install Card component
- [ ] Install Checkbox component

#### Task 2.2: Create LoginForm Component - [ ]

- [ ] Create presentational component
- [ ] Setup React Hook Form with Zod validation
- [ ] Add email and password fields
- [ ] Add "Remember me" checkbox
- [ ] Add "Forgot password" link
- [ ] Add loading state
- [ ] Add error display
- [ ] Style with Tailwind + Shadcn

**Validation:**

- [ ] Form validates inputs correctly
- [ ] Enter key submits form
- [ ] Loading state disables button
- [ ] Error messages display correctly
- [ ] Responsive on mobile

#### Task 2.3: Implement useLogin Hook - [ ]

- [ ] Create mutation hook with TanStack Query
- [ ] Call `authService.login`
- [ ] Store tokens on success
- [ ] Update Auth context with user
- [ ] Handle errors gracefully

#### Task 2.4: Create LoginPage Container - [ ]

- [ ] Create page component in `pages/(auth)/login/page.tsx`
- [ ] Use `LoginForm` component
- [ ] Connect to `useLogin` hook
- [ ] Handle navigation after login
- [ ] Redirect based on user role

**Validation:**

- [ ] Successful login navigates to correct dashboard
- [ ] Already logged-in users are redirected
- [ ] Error handling works end-to-end

#### Task 2.5: Create Auth Layout - [ ]

- [ ] Create `pages/(auth)/layout.tsx`
- [ ] Center content vertically/horizontally
- [ ] Add responsive padding
- [ ] Add background styling

### Milestone 3: Registration Feature

#### Task 3.1: Create RegisterForm Component - [ ]

- [ ] Add all registration fields
- [ ] Add role selector (Radio buttons)
- [ ] Add password strength meter
- [ ] Add password confirmation
- [ ] Add terms checkbox
- [ ] Implement validation schema

#### Task 3.2: Create Password Strength Meter - [ ]

- [ ] Create reusable component
- [ ] Calculate strength (weak/medium/strong)
- [ ] Show visual indicator (color-coded bar)
- [ ] Show requirements list

#### Task 3.3: Implement useRegister Hook - [ ]

- [ ] Create mutation hook
- [ ] Call `authService.register`
- [ ] Navigate to verification page on success

#### Task 3.4: Create RegisterPage - [ ]

- [ ] Create page component
- [ ] Connect form to hook
- [ ] Handle success/error states

#### Task 3.5: Create Email Verification Page - [ ]

- [ ] Create `pages/(auth)/verify-email/page.tsx`
- [ ] Add code input field (6 digits)
- [ ] Add resend code button
- [ ] Implement verification logic

**Validation:**

- [ ] Registration creates account successfully
- [ ] Verification code sent to email
- [ ] User auto-logged in after verification

### Milestone 4: Password Reset

#### Task 4.1: Create ForgotPasswordPage - [ ]

- [ ] Create page with email input
- [ ] Send reset code to email
- [ ] Navigate to reset page

#### Task 4.2: Create ResetPasswordPage - [ ]

- [ ] Add code input
- [ ] Add new password fields
- [ ] Validate and submit
- [ ] Navigate to login on success

### Milestone 5: Profile Management

#### Task 5.1: Create Profile Page - [ ]

- [ ] Create `pages/(dashboard)/profile/page.tsx`
- [ ] Fetch user profile with TanStack Query
- [ ] Display current information

#### Task 5.2: Create Profile Form - [ ]

- [ ] Add editable fields (name, bio)
- [ ] Add avatar upload button
- [ ] Implement file upload to S3
- [ ] Update profile with mutation

#### Task 5.3: Implement Avatar Upload - [ ]

- [ ] Create file input component
- [ ] Validate file (type, size)
- [ ] Upload to S3 via presigned URL
- [ ] Update user profile with new avatar URL

**Validation:**

- [ ] Profile displays correctly
- [ ] Changes save successfully
- [ ] Avatar uploads and displays
- [ ] Error handling works

### Milestone 6: Protected Routes

#### Task 6.1: Create ProtectedRoute Component - [ ]

- [ ] Check authentication status
- [ ] Redirect to /login if not authenticated
- [ ] Check role if required
- [ ] Show loading state during check

#### Task 6.2: Protect Dashboard Routes - [ ]

- [ ] Wrap dashboard layout with ProtectedRoute
- [ ] Require "Teacher" role for teacher pages

#### Task 6.3: Protect Student Routes - [ ]

- [ ] Wrap student layout with ProtectedRoute
- [ ] Require "Student" role

---

## Acceptance Criteria

### Definition of Done

For this feature to be considered complete:

#### Functionality

- [ ] Users can register, login, and logout
- [ ] Email verification works
- [ ] Password reset works
- [ ] Token refresh works automatically
- [ ] Profile editing works (name, bio, avatar)
- [ ] Role-based routing works correctly

#### Code Quality

- [ ] All TypeScript types are defined
- [ ] No `any` types used
- [ ] Components follow separation of concerns
- [ ] Shadcn components used throughout
- [ ] All API calls use TanStack Query
- [ ] Error handling is comprehensive

#### Testing

- [ ] Unit tests for utility functions
- [ ] Integration tests for auth flow
- [ ] E2E tests for login/register/logout

#### UX

- [ ] Loading states on all async operations
- [ ] Error messages are user-friendly
- [ ] Form validation is clear
- [ ] Responsive on all screen sizes
- [ ] Keyboard navigation works
- [ ] ARIA labels present

#### Performance

- [ ] Login completes in < 2 seconds
- [ ] Avatar upload shows progress
- [ ] No unnecessary re-renders

---

## Technical Notes

### Token Management

**Access Token:**

- Stored in localStorage
- Attached to all API requests
- Short-lived (15 minutes)

**Refresh Token:**

- Stored in localStorage
- Used to obtain new access token
- Long-lived (7 days)

**Refresh Logic:**

1. API returns 401 Unauthorized
2. Interceptor catches error
3. Call refresh token endpoint
4. Retry original request with new token
5. If refresh fails, logout user

### Security Considerations

- [ ] Passwords never stored in localStorage
- [ ] Tokens cleared on logout
- [ ] HTTPS only in production
- [ ] XSS protection via React's built-in escaping
- [ ] CSRF protection via SameSite cookies (backend)

---

**Next Feature:** [Teacher Dashboard](./02-dashboard-feature.md)
